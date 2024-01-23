﻿using System.Collections.Generic;
using System.ComponentModel;
using Bonsai;

namespace OpenEphys.Onix
{
    public class ConfigureHeadstage64 : HubDeviceFactory, INamedElement
    {
        string name;
        PortName port;
        readonly ConfigureFmcLinkController LinkController = new();

        public ConfigureHeadstage64()
        {
            Port = PortName.PortA;
            LinkController.HubConfiguration = HubConfiguration.Standard;
            LinkController.MinVoltage = 5.0;
            LinkController.MaxVoltage = 7.0;
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                UpdateDeviceNames(name);
            }
        }

        [Category(ConfigurationCategory)]
        [TypeConverter(typeof(HubDeviceConverter))]
        public ConfigureRhd2164 Rhd2164 { get; set; } = new();

        [Category(ConfigurationCategory)]
        [TypeConverter(typeof(HubDeviceConverter))]
        public ConfigureBno055 Bno055 { get; set; } = new();

        [Category(ConfigurationCategory)]
        [TypeConverter(typeof(HubDeviceConverter))]
        public ConfigureTS4231 TS4231 { get; set; } = new() { Enable = false };

        public PortName Port
        {
            get { return port; }
            set
            {
                port = value;
                var offset = (uint)port << 8;
                LinkController.DeviceAddress = (uint)port;
                Rhd2164.DeviceAddress = offset + 0;
                Bno055.DeviceAddress = offset + 1;
                TS4231.DeviceAddress = offset + 2;
                UpdateDeviceNames(Name);
            }
        }

        private void UpdateDeviceNames(string name)
        {
            LinkController.DeviceName = !string.IsNullOrEmpty(name) ? $"{name}.LinkController" : null;
            Rhd2164.DeviceName = !string.IsNullOrEmpty(name) ? $"{name}.Rhd2164" : null;
            Bno055.DeviceName = !string.IsNullOrEmpty(name) ? $"{name}.Bno055" : null;
            TS4231.DeviceName = !string.IsNullOrEmpty(name) ? $"{name}.TS4231" : null;
        }

        internal override IEnumerable<IDeviceConfiguration> GetDevices()
        {
            yield return LinkController;
            yield return Rhd2164;
            yield return Bno055;
            yield return TS4231;
        }
    }

    public enum PortName
    {
        PortA = 1,
        PortB = 2
    }
}
