using OpenEphys.ProbeInterface;
using ZedGraph;

namespace OpenEphys.Onix.Design
{
    public partial class NeuropixelsV1eChannelConfigurationDialog : ChannelConfigurationDialog
    {
        public NeuropixelsV1eChannelConfigurationDialog(NeuropixelsV1eProbeGroup probeGroup)
            : base(probeGroup)
        {
            InitializeComponent();
        }

        public override ProbeGroup DefaultChannelLayout()
        {
            return new NeuropixelsV1eProbeGroup();
        }

        public override void ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            base.ZoomEvent(sender, oldState, newState);
        }
    }
}
