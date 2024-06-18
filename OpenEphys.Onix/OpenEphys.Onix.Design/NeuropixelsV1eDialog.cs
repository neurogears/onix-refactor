using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ZedGraph;

namespace OpenEphys.Onix.Design
{
    public partial class NeuropixelsV1eDialog : Form
    {
        readonly NeuropixelsV1eChannelConfigurationDialog channelConfigurationDialog;

        NeuropixelsV1Adc[] Adcs;

        double ApGainCorrection = default;
        double LfpGainCorrection = default;

        public ConfigureNeuropixelsV1e ConfigureNode
        {
            get { return (ConfigureNeuropixelsV1e)propertyGrid.SelectedObject; }
            set { propertyGrid.SelectedObject = value; }
        }

        public NeuropixelsV1eDialog(ConfigureNeuropixelsV1e configureNode)
        {
            InitializeComponent();

            ConfigureNode = new(configureNode);

            channelConfigurationDialog = new(ConfigureNode.ChannelConfiguration)
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
            comboBoxApGain.SelectedItem = ConfigureNode.SpikeAmplifierGain;
            comboBoxApGain.SelectedIndexChanged += SelectedIndexChanged;

            comboBoxLfpGain.DataSource = Enum.GetValues(typeof(NeuropixelsV1Gain));
            comboBoxLfpGain.SelectedItem = ConfigureNode.LfpAmplifierGain;
            comboBoxLfpGain.SelectedIndexChanged += SelectedIndexChanged;

            comboBoxReference.DataSource = Enum.GetValues(typeof(NeuropixelsV1ReferenceSource));
            comboBoxReference.SelectedItem = ConfigureNode.Reference;
            comboBoxReference.SelectedIndexChanged += SelectedIndexChanged;

            checkBoxSpikeFilter.Checked = ConfigureNode.SpikeFilter;
            checkBoxSpikeFilter.CheckedChanged += SelectedIndexChanged;

            textBoxAdcCalibrationFile.TextChanged += FileTextChanged;
            textBoxAdcCalibrationFile.Text = ConfigureNode.AdcCalibrationFile;

            textBoxGainCalibrationFile.TextChanged += FileTextChanged;
            textBoxGainCalibrationFile.Text = ConfigureNode.GainCalibrationFile;

            CheckStatus();
        }

        private void FileTextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox && textBox != null)
            {
                if (textBox.Name == nameof(textBoxGainCalibrationFile))
                {
                    ConfigureNode.GainCalibrationFile = textBox.Text;
                    ParseGainCalibrationFile();
                }
                else if (textBox.Name == nameof(textBoxAdcCalibrationFile))
                {
                    ConfigureNode.AdcCalibrationFile = textBox.Text;
                    ParseAdcCalibrationFile();
                }
            }

            CheckStatus();
        }

        private void ParseAdcCalibrationFile()
        {
            if (ConfigureNode.AdcCalibrationFile != null && ConfigureNode.AdcCalibrationFile != "")
            {
                if (File.Exists(ConfigureNode.AdcCalibrationFile))
                {
                    StreamReader adcFile = new(ConfigureNode.AdcCalibrationFile);

                    adcCalibrationSN.Text = ulong.Parse(adcFile.ReadLine()).ToString();

                    Adcs = NeuropixelsV1.ParseAdcCalibrationFile(adcFile);

                    dataGridViewAdcs.DataSource = Adcs;

                    adcFile.Close();
                }
            }
        }

        private void ParseGainCalibrationFile()
        {
            if (ConfigureNode.GainCalibrationFile != null && ConfigureNode.GainCalibrationFile != "")
            {
                if (File.Exists(ConfigureNode.GainCalibrationFile))
                {
                    StreamReader gainCalibrationFile = new(ConfigureNode.GainCalibrationFile);

                    gainCalibrationSN.Text = ulong.Parse(gainCalibrationFile.ReadLine()).ToString();

                    NeuropixelsV1.ParseGainCalibrationFile(gainCalibrationFile, ConfigureNode.SpikeAmplifierGain,
                        ConfigureNode.LfpAmplifierGain, ref ApGainCorrection, ref LfpGainCorrection);

                    gainCalibrationFile.Close();
                }
            }
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox != null)
            {
                if (comboBox.Name == nameof(comboBoxApGain))
                {
                    ConfigureNode.SpikeAmplifierGain = (NeuropixelsV1Gain)comboBox.SelectedItem;
                    ParseGainCalibrationFile();

                    if (ApGainCorrection != default && LfpGainCorrection != default)
                    {
                        ShowCorrectionValues();
                    }
                }
                else if (comboBox.Name == nameof(comboBoxLfpGain))
                {
                    ConfigureNode.LfpAmplifierGain = (NeuropixelsV1Gain)comboBox.SelectedItem;
                    ParseGainCalibrationFile();

                    if (ApGainCorrection != default && LfpGainCorrection != default)
                    {
                        ShowCorrectionValues();
                    }
                }
                else if (comboBox.Name == nameof(comboBoxReference))
                {
                    ConfigureNode.Reference = (NeuropixelsV1ReferenceSource)comboBox.SelectedItem;
                }
            }
            else if (sender is CheckBox checkBox && checkBox != null)
            {
                if (checkBox.Name == nameof(checkBoxSpikeFilter))
                {
                    ConfigureNode.SpikeFilter = checkBox.Checked;
                }
            }
        }

        private void CheckStatus()
        {
            if (gainCalibrationSN.Text == "" || adcCalibrationSN.Text == "" || gainCalibrationSN.Text != adcCalibrationSN.Text)
            {
                toolStripStatus.Image = Properties.Resources.StatusWarningImage;
                toolStripStatus.Text = "Serial number mismatch";
            }
            else if (!channelConfigurationDialog.GetProbeGroup().ValidateDeviceChannelIndices())
            {
                toolStripStatus.Image = Properties.Resources.StatusBlockedImage;
                toolStripStatus.Text = "Invalid channels selected";
            }
            else
            {
                toolStripStatus.Image = Properties.Resources.StatusReadyImage;
                toolStripStatus.Text = "";
            }

            if (ApGainCorrection != default && LfpGainCorrection != default)
            {
                labelApGainCorrection.Visible = true;
                labelLfpGainCorrection.Visible = true;

                ShowCorrectionValues();
            }
            else
            {
                labelApGainCorrection.Visible = false;
                labelLfpGainCorrection.Visible = false;

                textBoxApGainCorrection.Visible = false;
                textBoxLfpGainCorrection.Visible = false;
            }
        }

        private void ShowCorrectionValues()
        {
            textBoxApGainCorrection.Text = ApGainCorrection.ToString();
            textBoxLfpGainCorrection.Text = LfpGainCorrection.ToString();

            textBoxApGainCorrection.Visible = true;
            textBoxLfpGainCorrection.Visible = true;
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
            const float zoomFactor = 8f;

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
                else if (button.Name == nameof(buttonZoomIn))
                {
                    ZoomIn(zoomFactor);
                }
                else if (button.Name == nameof(buttonZoomOut))
                {
                    ZoomOut(1 / zoomFactor);
                }
                else if (button.Name == nameof(buttonResetZoom))
                {
                    ResetZoom();
                }
                else if (button.Name == nameof(buttonJumpTo0))
                {
                    MoveToVerticalPosition(0f);
                }
                else if (button.Name == nameof(buttonJumpTo1))
                {
                    MoveToVerticalPosition(0.1f);
                }
                else if (button.Name == nameof(buttonJumpTo2))
                {
                    MoveToVerticalPosition(0.2f);
                }
                else if (button.Name == nameof(buttonJumpTo3))
                {
                    MoveToVerticalPosition(0.3f);
                }
                else if (button.Name == nameof(buttonJumpTo4))
                {
                    MoveToVerticalPosition(0.4f);
                }
                else if (button.Name == nameof(buttonJumpTo5))
                {
                    MoveToVerticalPosition(0.5f);
                }
                else if (button.Name == nameof(buttonJumpTo6))
                {
                    MoveToVerticalPosition(0.6f);
                }
                else if (button.Name == nameof(buttonJumpTo7))
                {
                    MoveToVerticalPosition(0.7f);
                }
                else if (button.Name == nameof(buttonJumpTo8))
                {
                    MoveToVerticalPosition(0.8f);
                }
                else if (button.Name == nameof(buttonJumpTo9))
                {
                    MoveToVerticalPosition(0.9f);
                }
                else if (button.Name == nameof(buttonJumpTo10))
                {
                    MoveToVerticalPosition(1f);
                }
            }
        }

        private void ZoomIn(double zoom)
        {
            if (zoom <= 1)
            {
                throw new ArgumentOutOfRangeException($"Argument {nameof(zoom)} must be greater than 1.0 to zoom in");
            }

            channelConfigurationDialog.ManualZoom(zoom);
            channelConfigurationDialog.RefreshZedGraph();
        }

        private void ZoomOut(double zoom)
        {
            if (zoom >= 1)
            {
                throw new ArgumentOutOfRangeException($"Argument {nameof(zoom)} must be less than 1.0 to zoom out");
            }

            channelConfigurationDialog.ManualZoom(zoom);
            channelConfigurationDialog.RefreshZedGraph();
        }

        private void ResetZoom()
        {
            channelConfigurationDialog.ResetZoom();
            channelConfigurationDialog.RefreshZedGraph();
        }

        private void MoveToVerticalPosition(float relativePosition)
        {
            channelConfigurationDialog.MoveToVerticalPosition(relativePosition);
            channelConfigurationDialog.RefreshZedGraph();
        }

        public NeuropixelsV1eProbeGroup GetProbeGroup()
        {
            return (NeuropixelsV1eProbeGroup)channelConfigurationDialog.GetProbeGroup();
        }
    }
}
