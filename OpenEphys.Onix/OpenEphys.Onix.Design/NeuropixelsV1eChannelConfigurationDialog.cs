using System.Windows.Forms;
using OpenEphys.ProbeInterface;

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
    }
}
