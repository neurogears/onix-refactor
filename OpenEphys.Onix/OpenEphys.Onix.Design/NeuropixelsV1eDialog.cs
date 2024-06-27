using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OpenEphys.Onix.Design
{
    public partial class NeuropixelsV1eDialog : Form
    {
        readonly NeuropixelsV1eChannelConfigurationDialog channelConfiguration;

        private enum ChannelPreset
        {
            BankA,
            BankB,
            BankC,
            SingleColumn,
            Tetrodes,
            None
        }

        NeuropixelsV1Adc[] Adcs;

        double ApGainCorrection = default;
        double LfpGainCorrection = default;

        public ConfigureNeuropixelsV1e ConfigureNode { get; set; }

        public NeuropixelsV1eDialog(ConfigureNeuropixelsV1e configureNode)
        {
            InitializeComponent();
            Shown += FormShown;

            ConfigureNode = new(configureNode);

            channelConfiguration = new(ConfigureNode.ProbeConfiguration.ChannelConfiguration)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill,
                Parent = this,
            };

            panelProbe.Controls.Add(channelConfiguration);
            this.AddMenuItemsFromDialogToFileOption(channelConfiguration);

            channelConfiguration.OnZoom += UpdateTrackBarLocation;
            channelConfiguration.OnFileLoad += UpdateChannelPresetIndex;

            comboBoxApGain.DataSource = Enum.GetValues(typeof(NeuropixelsV1Gain));
            comboBoxApGain.SelectedItem = ConfigureNode.ProbeConfiguration.SpikeAmplifierGain;
            comboBoxApGain.SelectedIndexChanged += SelectedIndexChanged;

            comboBoxLfpGain.DataSource = Enum.GetValues(typeof(NeuropixelsV1Gain));
            comboBoxLfpGain.SelectedItem = ConfigureNode.ProbeConfiguration.LfpAmplifierGain;
            comboBoxLfpGain.SelectedIndexChanged += SelectedIndexChanged;

            comboBoxReference.DataSource = Enum.GetValues(typeof(NeuropixelsV1ReferenceSource));
            comboBoxReference.SelectedItem = ConfigureNode.ProbeConfiguration.Reference;
            comboBoxReference.SelectedIndexChanged += SelectedIndexChanged;

            checkBoxSpikeFilter.Checked = ConfigureNode.ProbeConfiguration.SpikeFilter;
            checkBoxSpikeFilter.CheckedChanged += SelectedIndexChanged;

            textBoxAdcCalibrationFile.TextChanged += FileTextChanged;
            textBoxAdcCalibrationFile.Text = ConfigureNode.AdcCalibrationFile;

            textBoxGainCalibrationFile.TextChanged += FileTextChanged;
            textBoxGainCalibrationFile.Text = ConfigureNode.GainCalibrationFile;

            comboBoxChannelPresets.DataSource = Enum.GetValues(typeof(ChannelPreset));
            CheckForExistingChannelPreset();
            comboBoxChannelPresets.SelectedIndexChanged += SelectedIndexChanged;

            CheckStatus();
        }

        private void FormShown(object sender, EventArgs e)
        {
            if (!TopLevel)
            {
                splitContainer1.Panel2Collapsed = true;
                splitContainer1.Panel2.Hide();

                menuStrip.Visible = false;
            }

            channelConfiguration.Show();

            channelConfiguration.ConnectResizeEventHandler();
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

                    var gainCorrection = NeuropixelsV1Helper.ParseGainCalibrationFile(gainCalibrationFile, ConfigureNode.ProbeConfiguration.SpikeAmplifierGain, ConfigureNode.ProbeConfiguration.LfpAmplifierGain);

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
                    ConfigureNode.ProbeConfiguration.SpikeAmplifierGain = (NeuropixelsV1Gain)comboBox.SelectedItem;
                    ParseGainCalibrationFile();

                    if (ApGainCorrection != default && LfpGainCorrection != default)
                    {
                        ShowCorrectionValues();
                    }
                }
                else if (comboBox.Name == nameof(comboBoxLfpGain))
                {
                    ConfigureNode.ProbeConfiguration.LfpAmplifierGain = (NeuropixelsV1Gain)comboBox.SelectedItem;
                    ParseGainCalibrationFile();

                    if (ApGainCorrection != default && LfpGainCorrection != default)
                    {
                        ShowCorrectionValues();
                    }
                }
                else if (comboBox.Name == nameof(comboBoxReference))
                {
                    ConfigureNode.ProbeConfiguration.Reference = (NeuropixelsV1ReferenceSource)comboBox.SelectedItem;
                }
                else if (comboBox.Name == nameof(comboBoxChannelPresets))
                {
                    if ((ChannelPreset)comboBox.SelectedItem != ChannelPreset.None)
                    {
                        SetChannelPreset((ChannelPreset)comboBox.SelectedItem);
                    }
                }
            }
            else if (sender is CheckBox checkBox && checkBox != null)
            {
                if (checkBox.Name == nameof(checkBoxSpikeFilter))
                {
                    ConfigureNode.ProbeConfiguration.SpikeFilter = checkBox.Checked;
                }
            }
        }

        private void SetChannelPreset(ChannelPreset preset)
        {
            var channelMap = channelConfiguration.ChannelMap;
            var electrodes = channelConfiguration.Electrodes;

            switch (preset)
            {
                case ChannelPreset.BankA:
                    channelMap.SelectElectrodes(
                        electrodes.Where(e => e.Bank == NeuropixelsV1Bank.A).ToList());
                    break;

                case ChannelPreset.BankB:
                    channelMap.SelectElectrodes(
                        electrodes.Where(e => e.Bank == NeuropixelsV1Bank.B).ToList());
                    break;

                case ChannelPreset.BankC:
                    channelMap.SelectElectrodes(
                        electrodes.Where(e => e.Bank == NeuropixelsV1Bank.C ||
                                                                  (e.Bank == NeuropixelsV1Bank.B && e.ElectrodeNumber >= 576)).ToList());
                    break;

                case ChannelPreset.SingleColumn:
                    channelMap.SelectElectrodes(
                        electrodes.Where(e => (e.ElectrodeNumber % 2 == 0 && e.Bank == NeuropixelsV1Bank.A) ||
                                              (e.ElectrodeNumber % 2 == 1 && e.Bank == NeuropixelsV1Bank.B)).ToList());
                    break;

                case ChannelPreset.Tetrodes:
                    channelMap.SelectElectrodes(
                        electrodes.Where(e => (e.ElectrodeNumber % 8 < 4 && e.Bank == NeuropixelsV1Bank.A) ||
                                              (e.ElectrodeNumber % 8 > 3 && e.Bank == NeuropixelsV1Bank.B)).ToList());
                    break;
            }

            channelConfiguration.DrawChannels();
        }

        private void CheckForExistingChannelPreset()
        {
            var channelMap = channelConfiguration.ChannelMap;

            if (channelMap.All(e => e.Bank == NeuropixelsV1Bank.A))
            {
                comboBoxChannelPresets.SelectedItem = ChannelPreset.BankA;
            }
            else if (channelMap.All(e => e.Bank == NeuropixelsV1Bank.B))
            {
                comboBoxChannelPresets.SelectedItem = ChannelPreset.BankB;
            }
            else if (channelMap.All(e => e.Bank == NeuropixelsV1Bank.C ||
                                                             (e.Bank == NeuropixelsV1Bank.B && e.ElectrodeNumber >= 576)))
            {
                comboBoxChannelPresets.SelectedItem = ChannelPreset.BankC;
            }
            else if (channelMap.All(e => (e.ElectrodeNumber % 2 == 0 && e.Bank == NeuropixelsV1Bank.A) ||
                                              (e.ElectrodeNumber % 2 == 1 && e.Bank == NeuropixelsV1Bank.B)))
            {
                comboBoxChannelPresets.SelectedItem = ChannelPreset.SingleColumn;
            }
            else if (channelMap.All(e => (e.ElectrodeNumber % 8 < 4 && e.Bank == NeuropixelsV1Bank.A) ||
                                              (e.ElectrodeNumber % 8 > 3 && e.Bank == NeuropixelsV1Bank.B)))
            {
                comboBoxChannelPresets.SelectedItem = ChannelPreset.Tetrodes;
            }
            else
            {
                comboBoxChannelPresets.SelectedItem = ChannelPreset.None;
            }
        }

        private void UpdateChannelPresetIndex(object sender, EventArgs e)
        {
            CheckForExistingChannelPreset();
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
                    DesignHelper.UpdateProbeGroup(channelConfiguration.ChannelMap, ConfigureNode.ProbeConfiguration.ChannelConfiguration);

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
            if (channelConfiguration.SelectedContacts.Length != channelConfiguration.Electrodes.Count)
                throw new Exception("Invalid number of contacts versus electrodes found.");

            var selectedElectrodes = channelConfiguration.Electrodes
                                                         .Where((e, ind) => channelConfiguration.SelectedContacts[ind])
                                                         .ToList();

            channelConfiguration.EnableElectrodes(selectedElectrodes);

            CheckForExistingChannelPreset();
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
