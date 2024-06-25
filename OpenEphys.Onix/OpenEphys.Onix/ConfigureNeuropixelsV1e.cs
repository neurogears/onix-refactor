using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Threading;
using Bonsai;

namespace OpenEphys.Onix
{
    [Editor("OpenEphys.Onix.Design.NeuropixelsV1eEditor, OpenEphys.Onix.Design", typeof(ComponentEditor))]
    public class ConfigureNeuropixelsV1e : SingleDeviceFactory
    {
        public ConfigureNeuropixelsV1e()
            : base(typeof(NeuropixelsV1e))
        {
        }

        public ConfigureNeuropixelsV1e(ConfigureNeuropixelsV1e configureNeuropixelsV1e)
            : base(typeof(NeuropixelsV1e))
        {
            Enable = configureNeuropixelsV1e.Enable;
            EnableLed = configureNeuropixelsV1e.EnableLed;
            GainCalibrationFile = configureNeuropixelsV1e.GainCalibrationFile;
            AdcCalibrationFile = configureNeuropixelsV1e.AdcCalibrationFile;
            ProbeConfiguration = configureNeuropixelsV1e.ProbeConfiguration;
        }

        [Category(ConfigurationCategory)]
        [Description("Specifies whether the Neuropixels data stream is enabled.")]
        public bool Enable { get; set; } = true;

        [Category(ConfigurationCategory)]
        [Description("If true, the headstage LED will illuminate during acquisition. Otherwise it will remain off.")]
        public bool EnableLed { get; set; } = true;

        [Category(ConfigurationCategory)]
        [FileNameFilter("Gain calibration files (*_gainCalValues.csv)|*_gainCalValues.csv")]
        [Description("Path to the NRIC1384 gain calibration file.")]
        [Editor("Bonsai.Design.OpenFileNameEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
        public string GainCalibrationFile { get; set; }

        [Category(ConfigurationCategory)]
        [FileNameFilter("ADC calibration files (*_ADCCalibration.csv)|*_ADCCalibration.csv")]
        [Description("Path to the NRIC1384 ADC calibration file.")]
        [Editor("Bonsai.Design.OpenFileNameEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
        public string AdcCalibrationFile { get; set; }

        [Category(ConfigurationCategory)]
        [Description("Neuropixels 1.0e probe configuration")]
        public NeuropixelsV1eProbeConfiguration ProbeConfiguration { get; set; } = new();

        public override IObservable<ContextTask> Process(IObservable<ContextTask> source)
        {
            var enable = Enable;
            var deviceName = DeviceName;
            var deviceAddress = DeviceAddress;
            var ledEnabled = EnableLed;
            return source.ConfigureDevice(context =>
            {
                // configure device via the DS90UB9x deserializer device
                var device = context.GetPassthroughDeviceContext(deviceAddress, DS90UB9x.ID);
                device.WriteRegister(DS90UB9x.ENABLE, enable ? 1u : 0);

                // configure deserializer aliases and serializer power supply
                ConfigureDeserializer(device);
                var serializer = new I2CRegisterContext(device, DS90UB9x.SER_ADDR);

                // set I2C clock rate to ~400 kHz
                serializer.WriteByte((uint)DS90UB9xSerializerI2CRegister.SCLHIGH, 20);
                serializer.WriteByte((uint)DS90UB9xSerializerI2CRegister.SCLLOW, 20);

                // read probe metadata
                var probeMetadata = ReadProbeMetadata(serializer);

                // issue full mux reset to the probe
                var gpo10Config = NeuropixelsV1e.DefaultGPO10Config;
                ResetProbe(serializer, gpo10Config);

                // configure probe streaming
                if (probeMetadata.ProbeSerialNumber == null)
                    throw new InvalidOperationException("Probe serial number could not be read.");

                // program shift registers
                var probeControl = new NeuropixelsV1eRegisterContext(device, NeuropixelsV1e.ProbeAddress, ProbeConfiguration, GainCalibrationFile, AdcCalibrationFile);
                probeControl.InitializeProbe();
                probeControl.WriteConfiguration();
                probeControl.StartAcquisition();

                // turn on LED
                if (ledEnabled)
                {
                    TurnOnLed(serializer, NeuropixelsV1e.DefaultGPO32Config);
                }

                var deviceInfo = new NeuropixelsV1eDeviceInfo(context, DeviceType, deviceAddress, probeControl);
                var disposable = DeviceManager.RegisterDevice(deviceName, deviceInfo);
                var shutdown = Disposable.Create(() =>
                {
                    serializer.WriteByte((uint)DS90UB9xSerializerI2CRegister.GPIO10, NeuropixelsV1e.DefaultGPO10Config);
                    serializer.WriteByte((uint)DS90UB9xSerializerI2CRegister.GPIO32, NeuropixelsV1e.DefaultGPO32Config);
                });
                return new CompositeDisposable(
                    shutdown,
                    disposable);
            });
        }

        static void ConfigureDeserializer(DeviceContext device)
        {
            // configure deserializer trigger mode
            device.WriteRegister(DS90UB9x.TRIGGEROFF, 0);
            device.WriteRegister(DS90UB9x.TRIGGER, (uint)DS90UB9xTriggerMode.Continuous);
            device.WriteRegister(DS90UB9x.SYNCBITS, 0);
            device.WriteRegister(DS90UB9x.DATAGATE, 0b0000_0001_0001_0011_0000_0000_0000_0001);
            device.WriteRegister(DS90UB9x.MARK, (uint)DS90UB9xMarkMode.Disabled);

            // configure one magic word-triggered stream for the PSB bus
            device.WriteRegister(DS90UB9x.READSZ, 851973); // 13 frames/superframe,  7x 140-bit words on each serial line per frame
            device.WriteRegister(DS90UB9x.MAGIC_MASK, 0b11000000000000000000001111111111); // Enable inverse, wait for non-inverse, 10-bit magic word
            device.WriteRegister(DS90UB9x.MAGIC, 816); // Super-frame sync word
            device.WriteRegister(DS90UB9x.MAGIC_WAIT, 0);
            device.WriteRegister(DS90UB9x.DATAMODE, 913);
            device.WriteRegister(DS90UB9x.DATALINES0, 0x3245106B); // Sync, psb[0], psb[1], psb[2], psb[3], psb[4], psb[5], psb[6],
            device.WriteRegister(DS90UB9x.DATALINES1, 0xFFFFFFFF);

            // configure deserializer I2C aliases
            var deserializer = new I2CRegisterContext(device, DS90UB9x.DES_ADDR);
            uint coaxMode = 0x4 + (uint)DS90UB9xMode.Raw12BitHighFrequency; // 0x4 maintains coax mode
            deserializer.WriteByte((uint)DS90UB9xDeserializerI2CRegister.PortMode, coaxMode);

            uint alias = NeuropixelsV1e.ProbeAddress << 1;
            deserializer.WriteByte((uint)DS90UB9xDeserializerI2CRegister.SlaveID1, alias);
            deserializer.WriteByte((uint)DS90UB9xDeserializerI2CRegister.SlaveAlias1, alias);

            alias = NeuropixelsV1e.FlexEEPROMAddress << 1;
            deserializer.WriteByte((uint)DS90UB9xDeserializerI2CRegister.SlaveID2, alias);
            deserializer.WriteByte((uint)DS90UB9xDeserializerI2CRegister.SlaveAlias2, alias);
        }

        static NeuropixelsV1eMetadata ReadProbeMetadata(I2CRegisterContext serializer)
        {
            return new NeuropixelsV1eMetadata(serializer);
        }

        static void ResetProbe(I2CRegisterContext serializer, uint gpo10Config)
        {
            gpo10Config &= ~NeuropixelsV1e.Gpo10ResetMask;
            serializer.WriteByte((uint)DS90UB9xSerializerI2CRegister.GPIO10, gpo10Config);
            Thread.Sleep(1);
            gpo10Config |= NeuropixelsV1e.Gpo10ResetMask;
            serializer.WriteByte((uint)DS90UB9xSerializerI2CRegister.GPIO10, gpo10Config);
        }

        static uint TurnOnLed(I2CRegisterContext serializer, uint gpo23Config)
        {
            gpo23Config &= ~NeuropixelsV1e.Gpo32LedMask;
            serializer.WriteByte((uint)DS90UB9xSerializerI2CRegister.GPIO32, gpo23Config);

            return gpo23Config;
        }
    }

    static class NeuropixelsV1e
    {
        public const int ProbeAddress = 0x70;
        public const int FlexEEPROMAddress = 0x50;
        // TODO: Who's business is this?
        // public const int HeadstageEEPROMAddress = 0x51;

        public const byte DefaultGPO10Config = 0b0001_0001; // GPIO0 Low, NP in MUX reset
        public const byte DefaultGPO32Config = 0b1001_0001; // LED off, GPIO1 Low
        public const uint Gpo10ResetMask = 1 << 3; // Used to issue mux reset command to probe
        public const uint Gpo32LedMask = 1 << 7; // Used to turn on and off LED

        public const int FramesPerSuperFrame = 13;
        public const int FramesPerRoundRobin = 12;
        public const int AdcCount = 32;
        public const int ChannelCount = 384;
        public const int FrameWords = 40;

        // unmanaged regiseters
        public const uint OP_MODE = 0X00;
        public const uint REC_MOD = 0X01;
        public const uint CAL_MOD = 0X02;
        public const uint TEST_CONFIG1 = 0x03;
        public const uint TEST_CONFIG2 = 0x04;
        public const uint TEST_CONFIG3 = 0x05;
        public const uint TEST_CONFIG4 = 0x06;
        public const uint TEST_CONFIG5 = 0x07;
        public const uint STATUS = 0X08;
        public const uint SYNC = 0X09;
        public const uint SR_CHAIN1 = 0X0E; // Shank configuration
        public const uint SR_CHAIN3 = 0X0C; // Odd channels
        public const uint SR_CHAIN2 = 0X0D; // Even channels
        public const uint SR_LENGTH2 = 0X0F;
        public const uint SR_LENGTH1 = 0X10;
        public const uint SOFT_RESET = 0X11;

        internal class NameConverter : DeviceNameConverter
        {
            public NameConverter()
                : base(typeof(NeuropixelsV1e))
            {
            }
        }
    }
}
