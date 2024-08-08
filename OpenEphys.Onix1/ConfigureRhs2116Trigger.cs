using System;
using System.ComponentModel;
using System.Reactive.Subjects;
using Bonsai;
using System.Reactive.Disposables;

namespace OpenEphys.Onix1
{
    [Editor("OpenEphys.Onix1.Design.Rhs2116StimulusSequenceEditor, OpenEphys.Onix1.Design", typeof(ComponentEditor))]
    public class ConfigureRhs2116Trigger : SingleDeviceFactory
    {
        readonly BehaviorSubject<Rhs2116StimulusSequenceDual> stimulusSequence = new(new Rhs2116StimulusSequenceDual());

        public ConfigureRhs2116Trigger()
            : base(typeof(Rhs2116Trigger))
        {
        }

        [Category(ConfigurationCategory)]
        [Description("Specifies whether the RHS2116 device is enabled.")]
        public Rhs2116TriggerSource TriggerSource { get; set; } = Rhs2116TriggerSource.Local;

        [Category("Acquisition")]
        [Description("Stimulus sequence.")]
        public Rhs2116StimulusSequenceDual StimulusSequence
        {
            get => stimulusSequence.Value;
            set => stimulusSequence.OnNext(value);
        }

        public override IObservable<ContextTask> Process(IObservable<ContextTask> source)
        {
            var triggerSource = TriggerSource;
            var deviceName = DeviceName;
            var deviceAddress = DeviceAddress;
            return source.ConfigureDevice(context =>
            {
                var rhs2116AAddress = HeadstageRhs2116.GetRhs2116ADeviceAddress(GenericHelper.GetHubAddressFromDeviceAddress(deviceAddress));
                var rhs2116A = context.GetDeviceContext(rhs2116AAddress, DeviceType);
                var rhs2116BAddress = HeadstageRhs2116.GetRhs2116BDeviceAddress(GenericHelper.GetHubAddressFromDeviceAddress(deviceAddress));
                var rhs2116B = context.GetDeviceContext(rhs2116BAddress, DeviceType);

                var device = context.GetDeviceContext(deviceAddress, DeviceType);
                device.WriteRegister(Rhs2116Trigger.TRIGGERSOURCE, (uint)triggerSource);

                static void WriteStimulusSequence(DeviceContext device, Rhs2116StimulusSequence sequence)
                {
                    if (!sequence.Valid)
                    {
                        throw new WorkflowException("The requested stimulus sequence is invalid.");
                    }

                    if (!sequence.FitsInHardware)
                    {
                        throw new WorkflowException(string.Format("The requested stimulus is too complex. {0}/{1} memory slots are required.",
                        sequence.StimulusSlotsRequired,
                        Rhs2116.StimMemorySlotsAvailable));
                    }

                    var registerValue = Rhs2116Config.StimulatorStepSizeToRegisters[sequence.CurrentStepSize];
                    device.WriteRegister(Rhs2116.STEPSZ, registerValue[2] << 13 | registerValue[1] << 7 | registerValue[0]);

                    // Anodic amplitudes
                    // TODO: cache last write and compare
                    var registerAddress = Rhs2116.POS00;
                    int i = 0;
                    foreach (var a in sequence.AnodicAmplitudes)
                    {
                        device.WriteRegister((uint)(registerAddress + i++), a);
                    }

                    // Cathodic amplitudes
                    // TODO: cache last write and compare
                    registerAddress = Rhs2116.NEG00;
                    i = 0;
                    foreach (var a in sequence.CathodicAmplitudes)
                    {
                        device.WriteRegister((uint)(registerAddress + i++), a);
                    }

                    // Create delta table and set length
                    var dt = sequence.DeltaTable;
                    device.WriteRegister(Rhs2116.NUMDELTAS, (uint)dt.Count);

                    // If we want to do this efficiently, we probably need a different data structure on the
                    // FPGA ram that allows columns to be out of order (e.g. linked list)
                    uint j = 0;
                    foreach (var d in dt)
                    {
                        uint idxTime = j++ << 22 | (d.Key & 0x003FFFFF);
                        device.WriteRegister(Rhs2116.DELTAIDXTIME, idxTime);
                        device.WriteRegister(Rhs2116.DELTAPOLEN, d.Value);
                    }
                }

                return new CompositeDisposable(
                    stimulusSequence.Subscribe(newValue =>
                    {
                        // TODO: These are the wrong devices
                        WriteStimulusSequence(rhs2116A, newValue.StimulusSequenceA);
                        WriteStimulusSequence(rhs2116B, newValue.StimulusSequenceB);
                    }),
                    DeviceManager.RegisterDevice(deviceName, device, DeviceType));
            });
        }
    }

    static class Rhs2116Trigger
    {
        public const int ID = 32;

        // managed registers
        public const uint ENABLE = 0; // Writes and reads to ENABLE are ignored without error
        public const uint TRIGGERSOURCE = 1; // The LSB is used to determine the trigger source
        public const uint TRIGGER = 2; // Writing 0x1 to this register will trigger a stimulation sequence if the TRIGGERSOURCE is set to 0.

        internal class NameConverter : DeviceNameConverter
        {
            public NameConverter()
                : base(typeof(Rhs2116Trigger))
            {
            }
        }
    }

    public enum Rhs2116TriggerSource
    {
        [Description("Respect local triggers (e.g. via GPIO or TRIGGER register) and broadcast via sync pin. ")]
        Local = 0,
        [Description("Receiver. Only respect triggers received from sync pin")]
        External = 1,
    }
}
