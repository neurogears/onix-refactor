using System;
using System.Windows.Forms;
using OpenEphys.ProbeInterface;
using ZedGraph;

namespace OpenEphys.Onix.Design
{
    public partial class Rhs2116ChannelConfigurationDialog : ChannelConfigurationDialog
    {
        public event EventHandler OnSelect;
        public event EventHandler OnZoom;

        public Rhs2116ChannelConfigurationDialog(Rhs2116ProbeGroup probeGroup)
            : base(probeGroup)
        {
            InitializeComponent();
            ChannelConfiguration = probeGroup;

            zedGraphChannels.ZoomButtons = MouseButtons.None;
            zedGraphChannels.ZoomButtons2 = MouseButtons.None;

            zedGraphChannels.ZoomStepFraction = 0.5;

            HighlightEnabledContacts();
            DrawContactLabels();
            RefreshZedGraph();
        }

        public override ProbeGroup DefaultChannelLayout()
        {
            return new Rhs2116ProbeGroup();
        }

        internal override void OpenFile<T>()
        {
            Rhs2116ProbeGroup newConfiguration = OpenAndParseConfigurationFile<Rhs2116ProbeGroup>();

            if (newConfiguration == null)
            {
                return;
            }

            if (ChannelConfiguration.NumberOfContacts == newConfiguration.NumberOfContacts)
            {
                newConfiguration.Validate();

                ChannelConfiguration = newConfiguration;
                DrawChannels();
                SetEqualAspectRatio();
            }
            else
            {
                throw new InvalidOperationException($"Number of contacts does not match; expected {ChannelConfiguration.NumberOfContacts}" +
                    $", but found {newConfiguration.NumberOfContacts}");
            }
        }

        internal override string ContactString(Contact contact)
        {
            return contact.DeviceId.ToString();
        }

        internal override void SelectedContactChanged()
        {
            OnSelectHandler();
        }

        private void OnSelectHandler()
        {
            OnSelect?.Invoke(this, EventArgs.Empty);
        }

        public override void ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            base.ZoomEvent(sender, oldState, newState);
            OnZoomHandler();
        }

        private void OnZoomHandler()
        {
            OnZoom?.Invoke(this, EventArgs.Empty);
        }
    }
}
