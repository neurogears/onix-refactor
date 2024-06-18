using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenEphys.Onix
{
    public static class NeuropixelsV1
    {
        public const int AdcCount = 32;
        public const int ChannelCount = 384;
        public const int FramesPerRoundRobin = 12;
        public const int FramesPerSuperframe = 13;
        public const int SuperframesPerUltraframe = 12;
        internal const int NumberOfGains = 8;

        public static float GainToFloat(NeuropixelsV1Gain gain) => gain switch
        {
            NeuropixelsV1Gain.x50 => 50f,
            NeuropixelsV1Gain.x125 => 125f,
            NeuropixelsV1Gain.x250 => 250f,
            NeuropixelsV1Gain.x500 => 500f,
            NeuropixelsV1Gain.x1000 => 1000f,
            NeuropixelsV1Gain.x1500 => 1500f,
            NeuropixelsV1Gain.x2000 => 2000f,
            NeuropixelsV1Gain.x3000 => 3000f,
            _ => throw new ArgumentOutOfRangeException(nameof(gain), $"Unexpected gain value: {gain}"),
        };

        public static BitArray MakeShankBits(NeuropixelsV1eProbeGroup channelConfiguration, NeuropixelsV1ReferenceSource referenceSource)
        {
            const int ShankConfigurationBitCount = 968;
            const int ShankBitExt1 = 965;
            const int ShankBitExt2 = 2;
            const int ShankBitTip1 = 484;
            const int ShankBitTip2 = 483;
            const int InternalReferenceChannel = 191;

            BitArray shankBits = new(ShankConfigurationBitCount);

            List<int> deviceChannelIds = channelConfiguration.GetDeviceChannelIndices().ToList();

            // TODO: validate channel combinations here, reference the same logic in GUI

            for (int i = 0; i < deviceChannelIds.Count; i++)
            {
                if (i == InternalReferenceChannel || deviceChannelIds.ElementAt(i) == -1) continue;

                int bitIndex = i % 2 == 0 ?
                        485 + (i / 2) : // even electrode
                        482 - (i / 2);  // odd electrode

                shankBits[bitIndex] = true;
            }

            switch (referenceSource)
            {
                case NeuropixelsV1ReferenceSource.Ext:
                    {
                        shankBits[ShankBitExt1] = true;
                        shankBits[ShankBitExt2] = true;
                        break;
                    }
                case NeuropixelsV1ReferenceSource.Tip:
                    {
                        shankBits[ShankBitTip1] = true;
                        shankBits[ShankBitTip2] = true;
                        break;
                    }
            }

            return shankBits;
        }

        public static NeuropixelsV1Adc[] ParseAdcCalibrationFile(StreamReader file)
        {
            NeuropixelsV1Adc[] adcs = new NeuropixelsV1Adc[NeuropixelsV1e.AdcCount];

            for (var i = 0; i < NeuropixelsV1e.AdcCount; i++)
            {
                var adcCal = file.ReadLine().Split(',').Skip(1);
                if (adcCal.Count() != NumberOfGains)
                {
                    throw new ArgumentException("Incorrectly formatted ADC calibration file.");
                }

                adcs[i] = new NeuropixelsV1Adc
                {
                    CompP = int.Parse(adcCal.ElementAt(0)),
                    CompN = int.Parse(adcCal.ElementAt(1)),
                    Slope = int.Parse(adcCal.ElementAt(2)),
                    Coarse = int.Parse(adcCal.ElementAt(3)),
                    Fine = int.Parse(adcCal.ElementAt(4)),
                    Cfix = int.Parse(adcCal.ElementAt(5)),
                    Offset = int.Parse(adcCal.ElementAt(6)),
                    Threshold = int.Parse(adcCal.ElementAt(7))
                };
            }

            return adcs;
        }

        public static void ParseGainCalibrationFile(StreamReader file, NeuropixelsV1Gain apGain, NeuropixelsV1Gain lfpGain, ref double ApGainCorrection, ref double LfpGainCorrection)
        {
            var gainCorrections = file.ReadLine().Split(',').Skip(1);

            if (gainCorrections.Count() != 2 * NumberOfGains)
                throw new ArgumentException("Incorrectly formatted gain correction calibration file.");

            ApGainCorrection = double.Parse(gainCorrections.ElementAt(Array.IndexOf(Enum.GetValues(typeof(NeuropixelsV1Gain)), apGain)));
            LfpGainCorrection = double.Parse(gainCorrections.ElementAt(Array.IndexOf(Enum.GetValues(typeof(NeuropixelsV1Gain)), lfpGain) + 8));
        }
    }

    public class NeuropixelsV1Adc
    {
        public int CompP { get; set; } = 16;
        public int CompN { get; set; } = 16;
        public int Slope { get; set; } = 0;
        public int Coarse { get; set; } = 0;
        public int Fine { get; set; } = 0;
        public int Cfix { get; set; } = 0;
        public int Offset { get; set; } = 0;
        public int Threshold { get; set; } = 512;
    }
    public enum NeuropixelsV1ReferenceSource : byte
    {
        Ext = 0b001,
        Tip = 0b010
    }

    public enum NeuropixelsV1Gain : byte
    {
        x50 = 0b000,
        x125 = 0b001,
        x250 = 0b010,
        x500 = 0b011,
        x1000 = 0b100,
        x1500 = 0b101,
        x2000 = 0b110,
        x3000 = 0b111
    }

    [Flags]
    public enum NeuropixelsV1CalibrationRegisterValues : uint
    {
        CAL_OFF = 0,
        OSC_ACTIVE = 1 << 4, // 0 = external osc inactive, 1 = activate the external calibration oscillator
        ADC_CAL = 1 << 5, // Enable ADC calibration
        CH_CAL = 1 << 6, // Enable channel gain calibration
        PIX_CAL = 1 << 7, // Enable pixel + channel gain calibration

        // Useful combinations
        OSC_ACTIVE_AND_ADC_CAL = OSC_ACTIVE | ADC_CAL,
        OSC_ACTIVE_AND_CH_CAL = OSC_ACTIVE | CH_CAL,
        OSC_ACTIVE_AND_PIX_CAL = OSC_ACTIVE | PIX_CAL,

    };

    [Flags]
    public enum NeuropixelsV1RecordRegisterValues : uint
    {
        SR_RESET = 1 << 5, // 1 = Set analog SR chains to default values
        DIG_ENABLE = 1 << 6, // 0 = Reset the MUX, ADC, and PSB counter, 1 = Disable reset
        CH_ENABLE = 1 << 7, // 0 = Reset channel pseudo-registers, 1 = Disable reset

        // Useful combinations
        DIG_CH_RESET = 0,  // Yes, this is actually correct
        ACTIVE = DIG_ENABLE | CH_ENABLE,
    };

    [Flags]
    public enum NeuropixelsV1OperationRegisterValues : uint
    {
        TEST = 1 << 3, // Enable Test mode
        DIG_TEST = 1 << 4, // Enable Digital Test mode
        CALIBRATE = 1 << 5, // Enable calibration mode
        RECORD = 1 << 6, // Enable recording mode
        POWER_DOWN = 1 << 7, // Enable power down mode

        // Useful combinations
        RECORD_AND_DIG_TEST = RECORD | DIG_TEST,
        RECORD_AND_CALIBRATE = RECORD | CALIBRATE,
    };
}
