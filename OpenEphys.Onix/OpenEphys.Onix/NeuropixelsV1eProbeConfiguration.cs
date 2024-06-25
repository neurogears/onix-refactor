using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;
using Bonsai;
using Newtonsoft.Json;
using OpenEphys.ProbeInterface;

namespace OpenEphys.Onix
{
    public enum NeuropixelsV1Bank
    {
        A = 0,
        B,
        C
    }

    public class NeuropixelsV1eProbeConfiguration
    {
        [Category("Configuration")]
        [Description("Amplifier gain for spike-band.")]
        public NeuropixelsV1Gain SpikeAmplifierGain { get; set; } = NeuropixelsV1Gain.x1000;

        [Category("Configuration")]
        [Description("Amplifier gain for LFP-band.")]
        public NeuropixelsV1Gain LfpAmplifierGain { get; set; } = NeuropixelsV1Gain.x50;

        [Category("Configuration")]
        [Description("Reference selection.")]
        public NeuropixelsV1ReferenceSource Reference { get; set; } = NeuropixelsV1ReferenceSource.Ext;

        [Category("Configuration")]
        [Description("If true, activates a 300 Hz high-pass filter in the spike-band data stream.")]
        public bool SpikeFilter { get; set; } = true;

        [XmlIgnore]
        [Category("Configuration")]
        [Description("Defines the shape of the probe, and which contacts are currently selected for streaming")]
        public NeuropixelsV1eProbeGroup ChannelConfiguration { get; private set; } = new();

        [Browsable(false)]
        [Externalizable(false)]
        [XmlElement(nameof(ChannelConfiguration))]
        public string ChannelConfigurationString
        {
            get
            {
                var jsonString = JsonConvert.SerializeObject(ChannelConfiguration);
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonString));
            }
            set
            {
                var jsonString = Encoding.UTF8.GetString(Convert.FromBase64String(value));
                ChannelConfiguration = JsonConvert.DeserializeObject<NeuropixelsV1eProbeGroup>(jsonString);
            }
        }

        public NeuropixelsV1eProbeConfiguration()
        {
        }
    }

    public class NeuropixelsV1eElectrode
    {
        /// <summary>
        /// Index of the electrode within the context of the probe
        /// </summary>
        public int ElectrodeNumber { get; private set; }
        /// <summary>
        /// The shank this electrode belongs to. For NeuropixelsV1e, there is only one shank, so all shanks will equal 0
        /// </summary>
        public int Shank { get; private set; } = 0;
        /// <summary>
        /// Index of the electrode within this shank
        /// </summary>
        public int ShankIndex { get; private set; }
        /// <summary>
        /// The bank, or logical block of channels, this electrode belongs to
        /// </summary>
        public NeuropixelsV1Bank Bank { get; private set; }
        /// <summary>
        /// Index of the electrode within a logical block. For NeuropixelsV1e, this is the index within a Bank
        /// </summary>
        public int Channel { get; private set; }
        /// <summary>
        /// Location of the electrode in two-dimensional space
        /// </summary>
        public PointF Position { get; private set; }

        public NeuropixelsV1eElectrode(int electrodeNumber)
        {
            ElectrodeNumber = electrodeNumber;
            Shank = 0;
            ShankIndex = electrodeNumber;
            Bank = (NeuropixelsV1Bank)(electrodeNumber / NeuropixelsV1.ChannelCount);
            Channel = electrodeNumber % NeuropixelsV1.ChannelCount;
            var position = NeuropixelsV1eProbeGroup.DefaultContactPosition(electrodeNumber);
            Position = new PointF(position[0], position[1]);
        }

        public NeuropixelsV1eElectrode(int electrodeNumber, int channel, int shank, int shankIndex, NeuropixelsV1Bank bank, PointF position, bool enabled)
        {
            ElectrodeNumber = electrodeNumber;
            Shank = shank;
            ShankIndex = shankIndex;
            Bank = bank;
            Channel = channel;
            Position = position;
        }

        public NeuropixelsV1eElectrode(Contact contact)
        {
            ElectrodeNumber = contact.Index;
            Shank = int.TryParse(contact.ShankId, out int result) ? result : 0;
            ShankIndex = contact.Index;
            Bank = (NeuropixelsV1Bank)(contact.Index / NeuropixelsV1.ChannelCount);
            Channel = contact.Index % NeuropixelsV1.ChannelCount;
            Position = new(contact.PosX, contact.PosY);
        }
    }
}
