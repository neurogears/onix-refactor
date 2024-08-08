using System;
using System.Windows.Forms;
using OpenEphys.ProbeInterface;
using ZedGraph;

namespace OpenEphys.Onix1.Design
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

            RefreshZedGraph();
        }

        public override ProbeGroup DefaultChannelLayout()
        {
            return new Rhs2116ProbeGroup();
        }

        internal override void OpenFile<T>()
        {
            base.OpenFile<Rhs2116ProbeGroup>();
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
