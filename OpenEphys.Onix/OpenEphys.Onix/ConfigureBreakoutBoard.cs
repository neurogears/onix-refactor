﻿using System.Collections.Generic;
using System.ComponentModel;

namespace OpenEphys.Onix
{
    public class ConfigureBreakoutBoard : HubDeviceFactory
    {
        [TypeConverter(typeof(HubDeviceConverter))]
        public ConfigureHeartbeat Heartbeat { get; set; } = new();

        [TypeConverter(typeof(HubDeviceConverter))]
        public ConfigureAnalogIO AnalogIO { get; set; } = new();

        [TypeConverter(typeof(HubDeviceConverter))]
        public ConfigureDigitalIO DigitalIO { get; set; } = new();

        [TypeConverter(typeof(HubDeviceConverter))]
        public ConfigureMemoryMonitor MemoryMonitor { get; set; } = new();

        internal override IEnumerable<IDeviceConfiguration> GetDevices()
        {
            yield return Heartbeat;
            yield return AnalogIO;
            yield return DigitalIO;
            yield return MemoryMonitor; 
        }
    }
}
