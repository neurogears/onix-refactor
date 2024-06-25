using System.Windows.Forms;

namespace OpenEphys.Onix.Design
{
    public partial class NeuropixelsV1eHeadstageDialog : Form
    {
        public readonly NeuropixelsV1eDialog ConfigureNeuropixelsV1e;
        public readonly NeuropixelsV1eBno055Dialog ConfigureBno055;

        public NeuropixelsV1eHeadstageDialog(ConfigureNeuropixelsV1e configureNeuropixelsV1e, ConfigureNeuropixelsV1eBno055 configureBno055)
        {
            InitializeComponent();

            ConfigureNeuropixelsV1e = new(configureNeuropixelsV1e)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill,
                Parent = this
            };

            panelNeuropixelsV1e.Controls.Add(ConfigureNeuropixelsV1e);
            this.AddMenuItemsFromDialogToFileOption(ConfigureNeuropixelsV1e, "NeuropixelsV1e");
            ConfigureNeuropixelsV1e.Show();

            ConfigureBno055 = new(configureBno055)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill,
                Parent = this
            };

            panelBno055.Controls.Add(ConfigureBno055);
            this.AddMenuItemsFromDialog(ConfigureBno055, "Bno055");
            ConfigureBno055.Show();
            ConfigureBno055.Invalidate();
        }
    }
}
