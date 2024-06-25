using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenEphys.Onix
{
    public static class NeuropixelsV1Helper
    {
        internal const int NumberOfGains = 8;

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

        public static GainCorrection ParseGainCalibrationFile(StreamReader file, NeuropixelsV1Gain apGain, NeuropixelsV1Gain lfpGain)
        {
            var gainCorrections = file.ReadLine().Split(',').Skip(1);

            if (gainCorrections.Count() != 2 * NumberOfGains)
                throw new ArgumentException("Incorrectly formatted gain correction calibration file.");

            var ap = double.Parse(gainCorrections.ElementAt(Array.IndexOf(Enum.GetValues(typeof(NeuropixelsV1Gain)), apGain)));
            var lfp = double.Parse(gainCorrections.ElementAt(Array.IndexOf(Enum.GetValues(typeof(NeuropixelsV1Gain)), lfpGain) + 8));

            return new GainCorrection(ap, lfp);
        }
    }
}
