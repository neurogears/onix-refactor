﻿using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Bonsai;

namespace OpenEphys.Onix
{
    public class ConfigureUclaMiniscopeV4Camera : SingleDeviceFactory
    {
        readonly BehaviorSubject<double> ledBrightness = new(0);
        readonly BehaviorSubject<UclaMiniscopeV4SensorGain> sensorGain = new(UclaMiniscopeV4SensorGain.Low);
        readonly BehaviorSubject<double> liquidLensVoltage = new(47); // NB: middle of range

        public ConfigureUclaMiniscopeV4Camera()
            : base(typeof(UclaMiniscopeV4))
        {
        }

        [Category(ConfigurationCategory)]
        [Description("Specifies whether the camera is enabled.")]
        public bool Enable { get; set; } = true;

        [Category(ConfigurationCategory)]
        [Description("Camera video rate in frames per second.")]
        public UclaMiniscopeV4FramesPerSecond FrameRate { get; set; } = UclaMiniscopeV4FramesPerSecond.Fps30Hz;

        [Description("Camera sensor analog gain.")]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        public UclaMiniscopeV4SensorGain SensorGain
        {
            get => sensorGain.Value;
            set => sensorGain.OnNext(value);
        }

        [Category(ConfigurationCategory)]
        [Description("Only turn on excitation LED during camera exposures.")]
        public bool InterleaveLed { get; set; } = false;

        [Description("Excitation LED brightness (0-100%).")]
        [Range(0, 100)]
        [Precision(1, 1)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        public double LedBrightness
        {
            get => ledBrightness.Value;
            set => ledBrightness.OnNext(value);
        }

        [Description("Liquid lens voltage (Volts RMS).")]
        [Range(24.4, 69.7)]
        [Precision(1, 1)]
        [Editor(DesignTypes.SliderEditor, typeof(UITypeEditor))]
        public double LiquidLensVoltage
        {
            get => liquidLensVoltage.Value;
            set => liquidLensVoltage.OnNext(value);
        }

        public override IObservable<ContextTask> Process(IObservable<ContextTask> source)
        {
            var enable = Enable;
            var deviceName = DeviceName;
            var deviceAddress = DeviceAddress;
            uint shutterWidth = FrameRate switch
            {
                UclaMiniscopeV4FramesPerSecond.Fps10Hz => 10000,
                UclaMiniscopeV4FramesPerSecond.Fps15Hz => 6667,
                UclaMiniscopeV4FramesPerSecond.Fps20Hz => 5000,
                UclaMiniscopeV4FramesPerSecond.Fps25Hz => 4000,
                UclaMiniscopeV4FramesPerSecond.Fps30Hz => 3300,
                _ => 3300
            };
            var interleaveLED = InterleaveLed;

            return source.ConfigureDevice(context =>
            {
                // configure device via the DS90UB9x deserializer device
                var device = context.GetPassthroughDeviceContext(deviceAddress, DS90UB9x.ID);
                device.WriteRegister(DS90UB9x.ENABLE, enable ? 1u : 0);

                // configure deserializer, chip states, and camera PLL
                ConfigureMiniscope(device);

                // configuration properties
                var atMega = new I2CRegisterContext(device, UclaMiniscopeV4.AtMegaAddress);
                atMega.WriteByte(0x04, (uint)(interleaveLED ? 0x00 : 0x03));
                WriteCameraRegister(atMega, 200, shutterWidth);

                var deviceInfo = new DeviceInfo(context, DeviceType, deviceAddress);
                var disposable = DeviceManager.RegisterDevice(deviceName, deviceInfo);
                var shutdown = Disposable.Create(() =>
                {
                    // turn off EWL
                    var max14574 = new I2CRegisterContext(device, UclaMiniscopeV4.Max14574Address);
                    max14574.WriteByte(0x03, 0x00);

                    // turn off LED
                    var atMega = new I2CRegisterContext(device, UclaMiniscopeV4.AtMegaAddress);
                    atMega.WriteByte(1, 0xFF);
                });
                return new CompositeDisposable(
                    ledBrightness.Subscribe(value => SetLedBrightness(device, value)),
                    sensorGain.Subscribe(value => SetSensorGain(device, value)),
                    liquidLensVoltage.Subscribe(value => SetLiquidLensVoltage(device, value)),
                    shutdown,
                    disposable);
            });
        }

        internal static void ConfigureMiniscope(DeviceContext device)
        {
            // configure deserializer
            device.WriteRegister(DS90UB9x.TRIGGEROFF, 0);
            device.WriteRegister(DS90UB9x.READSZ, UclaMiniscopeV4.SensorColumns);
            device.WriteRegister(DS90UB9x.TRIGGER, (uint)DS90UB9xTriggerMode.HsyncEdgePositive);
            device.WriteRegister(DS90UB9x.SYNCBITS, 0);
            device.WriteRegister(DS90UB9x.DATAGATE, (uint)DS90UB9xDataGate.VsyncPositive);

            // NB: This is required because Bonsai is not garuenteed to capure every frame at the start of acqusition.
            // For this reason, the frame start needs to be marked.
            device.WriteRegister(DS90UB9x.MARK, (uint)DS90UB9xMarkMode.VsyncRising);

            // configure deserializer I2C aliases
            var deserializer = new I2CRegisterContext(device, DS90UB9x.DES_ADDR);
            uint coaxMode = 0x4 + (uint)DS90UB9xMode.Raw12BitLowFrequency; // 0x4 maintains coax mode
            deserializer.WriteByte((uint)DS90UB9xDeserializerI2CRegister.PortMode, coaxMode);

            uint i2cAlias = UclaMiniscopeV4.AtMegaAddress << 1;
            deserializer.WriteByte((uint)DS90UB9xDeserializerI2CRegister.SlaveID1, i2cAlias);
            deserializer.WriteByte((uint)DS90UB9xDeserializerI2CRegister.SlaveAlias1, i2cAlias);

            i2cAlias = UclaMiniscopeV4.Tpl0102Address << 1;
            deserializer.WriteByte((uint)DS90UB9xDeserializerI2CRegister.SlaveID2, i2cAlias);
            deserializer.WriteByte((uint)DS90UB9xDeserializerI2CRegister.SlaveAlias2, i2cAlias);

            i2cAlias = UclaMiniscopeV4.Max14574Address << 1;
            deserializer.WriteByte((uint)DS90UB9xDeserializerI2CRegister.SlaveID3, i2cAlias);
            deserializer.WriteByte((uint)DS90UB9xDeserializerI2CRegister.SlaveAlias3, i2cAlias);

            // set up potentiometer
            var tpl0102 = new I2CRegisterContext(device, UclaMiniscopeV4.Tpl0102Address);
            tpl0102.WriteByte(0x00, 0x72);
            tpl0102.WriteByte(0x01, 0x00);

            // turn on EWL
            var max14574 = new I2CRegisterContext(device, UclaMiniscopeV4.Max14574Address);
            max14574.WriteByte(0x08, 0x7F);
            max14574.WriteByte(0x09, 0x02);

            // turn on LED and setup Python480
            var atMega = new I2CRegisterContext(device, UclaMiniscopeV4.AtMegaAddress);
            WriteCameraRegister(atMega, 16, 3); // Turn on PLL
            WriteCameraRegister(atMega, 32, 0x7007); // Turn on clock managment
            WriteCameraRegister(atMega, 199, 666); // Defines granularity (unit = 1/PLL clock) of exposure and reset_length
            WriteCameraRegister(atMega, 200, 3300); // Set frame rate to 30 Hz
            WriteCameraRegister(atMega, 201, 3000); // Set Exposure
        }

        private static void WriteCameraRegister(I2CRegisterContext i2c, uint register, uint value)
        {
            // ATMega -> Python480 passthrough protocol
            var regLow = register & 0xFF;
            var regHigh = (register >> 8) & 0xFF;
            var valLow = value & 0xFF;
            var valHigh = (value >> 8) & 0xFF;

            i2c.WriteByte(0x05, regHigh);
            i2c.WriteByte(regLow, valHigh);
            i2c.WriteByte(valLow, 0x00);
        }

        internal static void SetLedBrightness(DeviceContext device, double percent)
        {
            var des = device.Context.GetPassthroughDeviceContext(device.Address, DS90UB9x.ID);

            var atMega = new I2CRegisterContext(des, UclaMiniscopeV4.AtMegaAddress);
            atMega.WriteByte(0x01, (uint)((percent == 0) ? 0xFF : 0x08));

            var tpl0102 = new I2CRegisterContext(des, UclaMiniscopeV4.Tpl0102Address);
            tpl0102.WriteByte(0x01, (uint)(255 * ((100 - percent) / 100.0)));
        }

        internal static void SetSensorGain(DeviceContext device, UclaMiniscopeV4SensorGain gain)
        {
            var des = device.Context.GetPassthroughDeviceContext(device.Address, DS90UB9x.ID);

            var atMega = new I2CRegisterContext(des, UclaMiniscopeV4.AtMegaAddress);
            WriteCameraRegister(atMega, 204, (uint)gain);
        }

        internal static void SetLiquidLensVoltage(DeviceContext device, double voltage)
        {
            var des = device.Context.GetPassthroughDeviceContext(device.Address, DS90UB9x.ID);

            var max14574 = new I2CRegisterContext(des, UclaMiniscopeV4.Max14574Address);
            max14574.WriteByte(0x08, (uint)((voltage - 24.4) / 0.0445) >> 2);
            max14574.WriteByte(0x09, 0x02);
        }
    }

    static class UclaMiniscopeV4
    {
        public const int AtMegaAddress = 0x10;
        public const int Tpl0102Address = 0x50;
        public const int Max14574Address = 0x77;

        public const int SensorRows = 608;
        public const int SensorColumns = 608;

        internal class NameConverter : DeviceNameConverter
        {
            public NameConverter()
                : base(typeof(UclaMiniscopeV4))
            {
            }
        }
    }

    public enum UclaMiniscopeV4SensorGain
    {
        Low = 0x00E1,
        Medium = 0x00E4,
        High = 0x0024,
    }

    public enum UclaMiniscopeV4FramesPerSecond
    {
        Fps10Hz,
        Fps15Hz,
        Fps20Hz,
        Fps25Hz,
        Fps30Hz,
    }
}
