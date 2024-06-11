using System;
using System.Windows.Forms;

namespace OpenEphys.Onix.Design
{
    public partial class NeuropixelsV1eDialog : Form
    {
        /// <summary>
        /// Local variable that holds the channel configuration in memory until the user presses Okay
        /// </summary>
        public NeuropixelsV1eProbeGroup ChannelConfiguration;

        readonly NeuropixelsV1eChannelConfigurationDialog npxChannelConfigurationDialog;
        readonly ConfigureNeuropixelsV1e configureNeuropixelsV1e;

        bool RefreshNeeded = false;

        public NeuropixelsV1eDialog()
        {
            InitializeComponent();

            // TODO: Pull out the entire ConfigureNeuropixelsV1e node here, and assign to local variable
            configureNeuropixelsV1e = new();

            var ChannelConfiguration = new NeuropixelsV1eProbeGroup();

            npxChannelConfigurationDialog = new(ChannelConfiguration)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill,
                Parent = this,
            };

            panelChannelConfiguration.Controls.Add(npxChannelConfigurationDialog);
            this.AddMenuItemsFromDialog(npxChannelConfigurationDialog, "Channel Configuration");

            npxChannelConfigurationDialog.Show();

            CheckStatus();
        }

        private void CheckStatus()
        {
            if (probeSN.Text == "" || configSN.Text == "" || probeSN.Text != configSN.Text)
            {
                toolStripStatus.Image = Properties.Resources.StatusWarningImage;
                toolStripStatus.Text = "Serial number mismatch";
            }
            else if (RefreshNeeded)
            {
                toolStripStatus.Image = Properties.Resources.StatusWarningImage;
                toolStripStatus.Text = "Upload required";
            }
            else
            {
                toolStripStatus.Image = Properties.Resources.StatusReadyImage;
                toolStripStatus.Text = "";
            }
        }

        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://open-ephys.github.io/onix-docs/Software%20Guide/Bonsai.ONIX/Nodes/NeuropixelsV1eDevice.html");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open documentation link.");
            }
        }
    }
}
