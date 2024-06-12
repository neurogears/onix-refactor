using System;
using System.Windows.Forms;
using Bonsai.Design;

namespace OpenEphys.Onix.Design
{
    public partial class NeuropixelsV1eDialog : Form
    {
        /// <summary>
        /// Local variable that holds the channel configuration in memory until the user presses Okay
        /// </summary>
        public NeuropixelsV1eProbeGroup ChannelConfiguration;

        readonly NeuropixelsV1eChannelConfigurationDialog channelConfigurationDialog;

        public ConfigureNeuropixelsV1e configureNeuropixelsV1e
        {
            get { return (ConfigureNeuropixelsV1e)propertyGrid.SelectedObject; }
            set { propertyGrid.SelectedObject = value; }
        }

        bool RefreshNeeded = false;

        public NeuropixelsV1eDialog(ConfigureNeuropixelsV1e configureNode)
        {
            InitializeComponent();

            configureNeuropixelsV1e = new(configureNode);

            ChannelConfiguration = new NeuropixelsV1eProbeGroup();

            channelConfigurationDialog = new(ChannelConfiguration)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill,
                Parent = this,
            };

            panelProbe.Controls.Add(channelConfigurationDialog);
            this.AddMenuItemsFromDialog(channelConfigurationDialog, "Channel Configuration");

            channelConfigurationDialog.Show();

            comboBoxApGain.DataSource = Enum.GetValues(typeof(NeuropixelsV1Gain));
            comboBoxApGain.SelectedItem = configureNeuropixelsV1e.SpikeAmplifierGain;
            comboBoxApGain.SelectedIndexChanged += SelectedIndexChanged;

            comboBoxLfpGain.DataSource = Enum.GetValues(typeof(NeuropixelsV1Gain));
            comboBoxLfpGain.SelectedItem = configureNeuropixelsV1e.LfpAmplifierGain;
            comboBoxLfpGain.SelectedIndexChanged += SelectedIndexChanged;

            comboBoxReference.DataSource = Enum.GetValues(typeof(NeuropixelsV1Reference));
            comboBoxReference.SelectedItem = configureNeuropixelsV1e.Reference;
            comboBoxReference.SelectedIndexChanged += SelectedIndexChanged;

            checkBoxSpikeFilter.Checked = configureNeuropixelsV1e.SpikeFilter;
            checkBoxSpikeFilter.CheckedChanged += SelectedIndexChanged;

            textBoxAdcCalibrationFile.Text = configureNeuropixelsV1e.AdcCalibrationFile;
            textBoxAdcCalibrationFile.TextChanged += FileTextChanged;

            textBoxGainCalibrationFile.Text = configureNeuropixelsV1e.GainCalibrationFile;
            textBoxGainCalibrationFile.TextChanged += FileTextChanged;

            CheckStatus();
        }

        private void FileTextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox && textBox != null)
            {
                if (textBox.Name == nameof(textBoxGainCalibrationFile))
                {
                    configureNeuropixelsV1e.GainCalibrationFile = textBox.Text;
                }
                else if (textBox.Name == nameof(textBoxAdcCalibrationFile))
                { 
                    configureNeuropixelsV1e.AdcCalibrationFile = textBox.Text;
                }
            }
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox != null)
            {
                if (comboBox.Name == nameof(comboBoxApGain))
                {
                    configureNeuropixelsV1e.SpikeAmplifierGain = (NeuropixelsV1Gain)comboBox.SelectedItem;
                }
                else if (comboBox.Name == nameof(comboBoxLfpGain))
                {
                    configureNeuropixelsV1e.LfpAmplifierGain = (NeuropixelsV1Gain)comboBox.SelectedItem;
                }
                else if (comboBox.Name == nameof(comboBoxReference))
                {
                    configureNeuropixelsV1e.Reference = (NeuropixelsV1Reference)comboBox.SelectedItem;
                }
            }
            else if (sender is CheckBox checkBox && checkBox != null)
            {
                if (checkBox.Name == nameof(checkBoxSpikeFilter))
                {
                    configureNeuropixelsV1e.SpikeFilter = checkBox.Checked;
                }
            }
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

        private void ButtonClick(object sender, EventArgs e)
        {
            if (sender is Button button && button != null)
            {
                if (button.Name == nameof(buttonOkay))
                {
                    DialogResult = DialogResult.OK;
                }
                else if (button.Name == nameof(buttonCancel))
                {
                    DialogResult = DialogResult.Cancel;
                }
                else if (button.Name == nameof(buttonGainCalibrationFile))
                {
                    var ofd = new OpenFileDialog()
                    {
                        CheckFileExists = true,
                        Filter = "Gain calibration files (*_gainCalValues.csv)|*_gainCalValues.csv|All Files|*.*",
                        FilterIndex = 0
                    };

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        textBoxGainCalibrationFile.Text = ofd.FileName;
                    }
                }
                else if (button.Name == nameof(buttonAdcCalibrationFile))
                {
                    var ofd = new OpenFileDialog()
                    {
                        CheckFileExists = true,
                        Filter = "ADC calibration files (*_ADCCalibration.csv)|*_ADCCalibration.csv|All Files|*.*",
                        FilterIndex = 0
                    };

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        textBoxAdcCalibrationFile.Text = ofd.FileName;
                    }
                }
            }
        }
    }
}
