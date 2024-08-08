﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Bonsai;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace OpenEphys.Onix1
{
    [Editor("OpenEphys.Onix1.Design.HeadstageRhs2116Editor, OpenEphys.Onix1.Design", typeof(ComponentEditor))]
    public class ConfigureHeadstageRhs2116 : MultiDeviceFactory
    {
        PortName port;
        readonly ConfigureHeadstageRhs2116LinkController LinkController = new();

        public ConfigureHeadstageRhs2116()
        {
            Port = PortName.PortA;
            LinkController.HubConfiguration = HubConfiguration.Standard;
        }

        [Category(ConfigurationCategory)]
        [TypeConverter(typeof(SingleDeviceFactoryConverter))]
        public ConfigureRhs2116 Rhs2116A { get; set; } = new();

        [Category(ConfigurationCategory)]
        [TypeConverter(typeof(SingleDeviceFactoryConverter))]
        public ConfigureRhs2116 Rhs2116B { get; set; } = new();

        [Category(ConfigurationCategory)]
        [TypeConverter(typeof(SingleDeviceFactoryConverter))]
        public ConfigureRhs2116Trigger StimulusTrigger { get; set; } = new();

        internal override void UpdateDeviceNames()
        {
            LinkController.DeviceName = GetFullDeviceName(nameof(LinkController));
            Rhs2116A.DeviceName = GetFullDeviceName(nameof(Rhs2116A));
            Rhs2116B.DeviceName = GetFullDeviceName(nameof(Rhs2116B));
            StimulusTrigger.DeviceName = GetFullDeviceName(nameof(StimulusTrigger));
        }

        public PortName Port
        {
            get { return port; }
            set
            {
                port = value;
                var offset = (uint)port << 8;
                LinkController.DeviceAddress = (uint)port;
                Rhs2116A.DeviceAddress = HeadstageRhs2116.GetRhs2116ADeviceAddress(offset);
                Rhs2116B.DeviceAddress = HeadstageRhs2116.GetRhs2116BDeviceAddress(offset);
                StimulusTrigger.DeviceAddress = HeadstageRhs2116.GetRhs2116StimulusTriggerDeviceAddress(offset);
            }
        }

        [XmlIgnore]
        [Category(ConfigurationCategory)]
        [Description("Defines the physical channel configuration")]
        public Rhs2116ProbeGroup ChannelConfiguration { get; set; } = new();

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
                ChannelConfiguration = JsonConvert.DeserializeObject<Rhs2116ProbeGroup>(jsonString);
            }
        }

        [Description("If defined, it will override automated voltage discovery and apply the specified voltage" +
                     "to the headstage. Warning: this device requires 3.4V to 4.4V for proper operation." +
                     "Supplying higher voltages may result in damage to the headstage.")]
        public double? PortVoltage
        {
            get => LinkController.PortVoltage;
            set => LinkController.PortVoltage = value;
        }

        internal override IEnumerable<IDeviceConfiguration> GetDevices()
        {
            yield return LinkController;
            yield return Rhs2116A;
            yield return Rhs2116B;
            yield return StimulusTrigger;
        }

        class ConfigureHeadstageRhs2116LinkController : ConfigureFmcLinkController
        {
            protected override bool ConfigurePortVoltage(DeviceContext device)
            {
                const double MinVoltage = 3.3;
                const double MaxVoltage = 4.4;
                const double VoltageOffset = 2.0;
                const double VoltageIncrement = 0.2;

                for (var voltage = MinVoltage; voltage <= MaxVoltage; voltage += VoltageIncrement)
                {
                    SetPortVoltage(device, voltage);
                    if (base.CheckLinkState(device))
                    {
                        SetPortVoltage(device, voltage + VoltageOffset);
                        return CheckLinkState(device);
                    }
                }

                return false;
            }

            private void SetPortVoltage(DeviceContext device, double voltage)
            {
                device.WriteRegister(FmcLinkController.PORTVOLTAGE, 0);
                Thread.Sleep(500);
                device.WriteRegister(FmcLinkController.PORTVOLTAGE, (uint)(10 * voltage));
                Thread.Sleep(500);
            }

            protected override bool CheckLinkState(DeviceContext device)
            {
                // NB: The RHS2116 headstage needs an additional reset after power on to provide its device table.
                device.Context.Reset();
                var linkState = device.ReadRegister(FmcLinkController.LINKSTATE);
                return (linkState & FmcLinkController.LINKSTATE_SL) != 0;
            }
        }
    }

    internal static class HeadstageRhs2116
    {
        public static uint GetRhs2116ADeviceAddress(uint baseAddress) => baseAddress + 0;
        public static uint GetRhs2116BDeviceAddress(uint baseAddress) => baseAddress + 1;
        public static uint GetRhs2116StimulusTriggerDeviceAddress(uint baseAddress) => baseAddress + 2;
    }
}
