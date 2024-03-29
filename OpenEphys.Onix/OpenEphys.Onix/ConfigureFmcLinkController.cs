﻿using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Threading;

namespace OpenEphys.Onix
{
    public class ConfigureFmcLinkController : SingleDeviceFactory
    {
        public ConfigureFmcLinkController()
            : base(typeof(FmcLinkController))
        {
        }

        [Category(ConfigurationCategory)]
        [Description("Specifies whether the link controller device is enabled.")]
        public bool Enable { get; set; } = true;

        [Category(ConfigurationCategory)]
        [Description("Specifies whether the hub device should be configured in standard or passthrough mode.")]
        public HubConfiguration HubConfiguration { get; set; }

        public bool GPO1 { get; set; }

        [Description("Specifies the minimum link voltage at which the hub is expected to operate.")]
        public double MinVoltage { get; set; }

        [Description("Specifies the maximum link voltage at which the hub is expected to operate.")]
        public double MaxVoltage { get; set; }

        [Description("Specifies an optional link voltage offset to ensure stable operation after lock is established.")]
        public double VoltageOffset { get; set; } = 0.2;

        protected virtual bool CheckLinkState(DeviceContext context)
        {
            var linkState = context.ReadRegister(FmcLinkController.LINKSTATE);
            return (linkState & FmcLinkController.LINKSTATE_SL) != 0;
        }

        public override IObservable<ContextTask> Process(IObservable<ContextTask> source)
        {
            var deviceName = DeviceName;
            var deviceAddress = DeviceAddress;
            var hubConfiguration = HubConfiguration;
            return source.ConfigureHost(context =>
            {
                // configure passthrough mode on the FMC link controller
                // assuming the device address is the port number
                var portShift = ((int)deviceAddress - 1) * 2;
                var passthroughState = (hubConfiguration == HubConfiguration.Passthrough ? 1 : 0) << portShift;
                context.HubState = (PassthroughState)(((int)context.HubState & ~(1 << portShift)) | passthroughState);
                return Disposable.Empty;
            })
            .ConfigureLink(context =>
            {
                var device = context.GetDeviceContext(deviceAddress, FmcLinkController.ID);
                device.WriteRegister(FmcLinkController.ENABLE, 1);

                var hasLock = false;
                var minVoltage = (uint)(MinVoltage * 10);
                var maxVoltage = (uint)(MaxVoltage * 10);
                var safetyVoltage = (uint)(VoltageOffset * 10);
                for (uint voltage = minVoltage; voltage <= maxVoltage; voltage += 2)
                {
                    const int WaitUntilVoltageSettles = 200;
                    device.WriteRegister(FmcLinkController.PORTVOLTAGE, 0);
                    Thread.Sleep(WaitUntilVoltageSettles);
                    device.WriteRegister(FmcLinkController.PORTVOLTAGE, voltage);
                    Thread.Sleep(WaitUntilVoltageSettles);

                    if (CheckLinkState(device))
                    {
                        device.WriteRegister(
                            FmcLinkController.PORTVOLTAGE,
                            voltage + safetyVoltage);
                        hasLock = true;
                        break;
                    }
                }

                void dispose() => device.WriteRegister(FmcLinkController.PORTVOLTAGE, 0);
                if (!hasLock)
                {
                    dispose();
                    throw new InvalidOperationException("Unable to get SERDES lock on FMC link controller.");
                }
                return Disposable.Create(dispose);
            })
            .ConfigureDevice(context => 
            {
                var deviceInfo = new DeviceInfo(context, DeviceType, deviceAddress);
                return DeviceManager.RegisterDevice(deviceName, deviceInfo);
            });
        }

        static class FmcLinkController
        {
            public const int ID = 23;

            public const uint ENABLE = 0; // The LSB is used to enable or disable the device data stream
            public const uint GPOSTATE = 1; // GPO output state (bits 31 downto 3: ignore. bits 2 downto 0: ‘1’ = high, ‘0’ = low)
            public const uint DESPWR = 2; // Set link deserializer PDB state, 0 = deserializer power off else on. Does not affect port voltage.
            public const uint PORTVOLTAGE = 3; // 10 * link voltage
            public const uint SAVEVOLTAGE = 4; // Save link voltage to non-volatile EEPROM if greater than 0. This voltage will be applied after POR.
            public const uint LINKSTATE = 5; // bit 1 pass; bit 0 lock

            public const uint LINKSTATE_PP = 0x2; // parity check pass bit
            public const uint LINKSTATE_SL = 0x1; // SERDES lock bit
        }
    }
}
