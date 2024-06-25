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

        /// <summary>
        /// Convert a ProbeInterface object to a list of electrodes, which includes all possible electrodes
        /// </summary>
        /// <param name="channelConfiguration">A <see cref="NeuropixelsV1eProbeGroup"/> object</param>
        /// <returns>List of <see cref="NeuropixelsV1eElectrode"/> electrodes</returns>
        public static List<NeuropixelsV1eElectrode> ToElectrodes(NeuropixelsV1eProbeGroup channelConfiguration)
        {
            List<NeuropixelsV1eElectrode> electrodes = new();

            foreach (var c in channelConfiguration.GetContacts())
            {
                electrodes.Add(new NeuropixelsV1eElectrode(c));
            }

            return electrodes;
        }

        public static void UpdateElectrodes(List<NeuropixelsV1eElectrode> electrodes, NeuropixelsV1eProbeGroup channelConfiguration)
        {
            if (electrodes.Count != channelConfiguration.NumberOfContacts)
            {
                throw new InvalidOperationException($"Different number of electrodes found in {nameof(electrodes)} versus {nameof(channelConfiguration)}");
            }

            int index = 0;

            foreach (var c in channelConfiguration.GetContacts())
            {
                electrodes[index++] = new NeuropixelsV1eElectrode(c);
            }
        }

        /// <summary>
        /// Convert a ProbeInterface object to a list of electrodes, which only includes currently enabled electrodes
        /// </summary>
        /// <param name="channelConfiguration">A <see cref="NeuropixelsV1eProbeGroup"/> object</param>
        /// <returns>List of <see cref="NeuropixelsV1eElectrode"/> electrodes that are enabled</returns>
        public static List<NeuropixelsV1eElectrode> ToChannelMap(NeuropixelsV1eProbeGroup channelConfiguration)
        {
            List<NeuropixelsV1eElectrode> channelMap = new();

            foreach (var c in channelConfiguration.GetContacts().Where(c => c.DeviceId != -1))
            {
                channelMap.Add(new NeuropixelsV1eElectrode(c));
            }

            return channelMap.OrderBy(e => e.Channel).ToList();
        }

        public static void UpdateChannelMap(List<NeuropixelsV1eElectrode> channelMap, NeuropixelsV1eProbeGroup channelConfiguration)
        {
            var enabledElectrodes = channelConfiguration.GetContacts()
                                                        .Where(c => c.DeviceId != -1);

            if (channelMap.Count != enabledElectrodes.Count())
            {
                throw new InvalidOperationException($"Different number of enabled electrodes found in {nameof(channelMap)} versus {nameof(channelConfiguration)}");
            }

            int index = 0;

            foreach (var c in enabledElectrodes)
            {
                channelMap[index++] = new NeuropixelsV1eElectrode(c);
            }
        }

        /// <summary>
        /// Update the currently enabled contacts in the probe group, based on the currently selected contacts in 
        /// the given channel map. The only operation that occurs is an update of the DeviceChannelIndices field,
        /// where -1 indicates the contact is no longer enabled
        /// </summary>
        /// <param name="channelMap">List of <see cref="NeuropixelsV1eElectrode"/> objects, which contain the index of the selected contact</param>
        /// <param name="probeGroup"><see cref="NeuropixelsV1eProbeGroup"/></param>
        public static void UpdateProbeGroup(List<NeuropixelsV1eElectrode> channelMap, NeuropixelsV1eProbeGroup probeGroup)
        {
            int[] deviceChannelIndices = new int[probeGroup.NumberOfContacts];

            deviceChannelIndices = deviceChannelIndices.Select(i => i = -1).ToArray();

            foreach (var e in channelMap)
            {
                deviceChannelIndices[e.ElectrodeNumber] = e.Channel;
            }

            probeGroup.UpdateDeviceChannelIndices(0, deviceChannelIndices);
        }

        public static List<NeuropixelsV1eElectrode> SelectElectrodes(this List<NeuropixelsV1eElectrode> channelMap, List<NeuropixelsV1eElectrode> electrodes)
        {
            foreach (var e in electrodes)
            {
                channelMap[e.Channel] = e;
            }

            return channelMap;
        }
    }
}
