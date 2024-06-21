using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Bonsai.Reactive;
using OpenEphys.ProbeInterface;

namespace OpenEphys.Onix.Design
{
    public partial class NeuropixelsV1eDialog : Form
    {
        readonly NeuropixelsV1eChannelConfigurationDialog channelConfiguration;

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

            channelConfiguration = new(ConfigureNode.ChannelConfiguration)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill,
                Parent = this,
            };

            panelProbe.Controls.Add(channelConfiguration);
            this.AddMenuItemsFromDialog(channelConfiguration, "Channel Configuration");

            channelConfiguration.OnZoom += UpdateTrackBarLocation;

            channelConfiguration.Show();

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

                    Adcs = NeuropixelsV1Helper.ParseAdcCalibrationFile(adcFile);

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

                    var gainCorrection = NeuropixelsV1Helper.ParseGainCalibrationFile(gainCalibrationFile, ConfigureNode.SpikeAmplifierGain, ConfigureNode.LfpAmplifierGain);

                    ApGainCorrection = gainCorrection.AP;
                    LfpGainCorrection = gainCorrection.LFP;

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
            else if (!channelConfiguration.GetProbeGroup().ValidateDeviceChannelIndices())
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
                else if (button.Name == nameof(buttonClearSelections))
                {
                    channelConfiguration.SetAllSelections(false);
                    channelConfiguration.DrawChannels();
                    channelConfiguration.RefreshZedGraph();
                }
                else if (button.Name == nameof(buttonEnableContacts))
                {
                    EnableSelectedContacts();
                    channelConfiguration.SetAllSelections(false);
                    channelConfiguration.DrawChannels();
                    channelConfiguration.RefreshZedGraph();
                }
            }
        }

        private void EnableSelectedContacts()
        {
            const int probeIndex = 0;

            var selectedContacts = channelConfiguration.SelectedContacts;

            for (int i = 0; i < selectedContacts.Length; i++)
            {
                if (selectedContacts[i])
                {
                    EnableContact(probeIndex, i);
                }
            }

            var probe = channelConfiguration.GetProbeGroup().Probes.ElementAt(probeIndex);
        }

        private void EnableContact(int probeNumber, int contactNumber) 
        {
            var probe = channelConfiguration.GetProbeGroup().Probes.ElementAt(probeNumber);

            if (probe.DeviceChannelIndices[contactNumber] != -1)
                return;

            var contact = probe.GetContact(contactNumber);

            const int BankA_Start = 0;
            const int BankA_End = 383;
            const int BankB_Start = 384;
            const int BankB_End = 767;
            const int BankC_Start = 768;
            const int BankC_End = 959;

            if (contact.Index >= BankA_Start && contact.Index <= BankA_End)
            {
                var index = contact.Index + BankB_Start;

                if (probe.DeviceChannelIndices[index] != -1)
                {
                    SwapIndices(probe, contact.Index, index);
                    return;
                }

                index = contact.Index + BankC_Start;

                if (index > BankC_End)
                    return;

                if (probe.DeviceChannelIndices[index] != -1)
                {
                    SwapIndices(probe, contact.Index, index);
                    return;
                }
            }
            else if (contact.Index >= BankB_Start && contact.Index <= BankB_End)
            {
                var index = contact.Index - BankB_Start;

                if (probe.DeviceChannelIndices[index] != -1)
                {
                    SwapIndices(probe, contact.Index, index);
                    return;
                }

                index = contact.Index - BankB_Start + BankC_Start;

                if (index > BankC_End)
                    return;

                if (probe.DeviceChannelIndices[index] != -1)
                {
                    SwapIndices(probe, contact.Index, index);
                    return;
                }
            }
            else if (contact.Index >= BankC_Start && contact.Index <= BankC_End)
            {
                var index = contact.Index - BankC_Start;

                if (probe.DeviceChannelIndices[index] != -1)
                {
                    SwapIndices(probe, contact.Index, index);
                    return;
                }

                index = contact.Index - BankC_Start + BankB_Start;

                if (index > BankC_End)
                    return;

                if (probe.DeviceChannelIndices[index] != -1)
                {
                    SwapIndices(probe, contact.Index, index);
                    return;
                }
            }
        }

        private static void SwapIndices(Probe probe, int currentIndex, int offsetIndex)
        {
            (probe.DeviceChannelIndices[currentIndex], probe.DeviceChannelIndices[offsetIndex]) = (probe.DeviceChannelIndices[offsetIndex], probe.DeviceChannelIndices[currentIndex]);
        }
        private void ZoomIn(double zoom)
        {
            if (zoom <= 1)
            {
                throw new ArgumentOutOfRangeException($"Argument {nameof(zoom)} must be greater than 1.0 to zoom in");
            }
            channelConfiguration.ManualZoom(zoom);
            channelConfiguration.RefreshZedGraph();
        }

        private void ZoomOut(double zoom)
        {
            if (zoom >= 1)
            {
                throw new ArgumentOutOfRangeException($"Argument {nameof(zoom)} must be less than 1.0 to zoom out");
            }
            channelConfiguration.ManualZoom(zoom);
            channelConfiguration.RefreshZedGraph();
        }

        private void ResetZoom()
        {
            channelConfiguration.ResetZoom();
            channelConfiguration.RefreshZedGraph();
        }

        private void MoveToVerticalPosition(float relativePosition)
        {
            channelConfiguration.MoveToVerticalPosition(relativePosition);
            channelConfiguration.RefreshZedGraph();
        }

        public NeuropixelsV1eProbeGroup GetProbeGroup()
        {
            return (NeuropixelsV1eProbeGroup)channelConfiguration.GetProbeGroup();
        }

        private void TrackBarScroll(object sender, EventArgs e)
        {
            if (sender is TrackBar trackBar && trackBar != null)
            {
                if (trackBar.Name == nameof(trackBarProbePosition))
                {
                    MoveToVerticalPosition(trackBar.Value / 100.0f);
                }
            }
        }

        private void UpdateTrackBarLocation(object sender, EventArgs e)
        {
            trackBarProbePosition.Value = (int)(channelConfiguration.GetRelativeVerticalPosition() * 100);
        }
    }
}
