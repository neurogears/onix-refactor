namespace OpenEphys.Onix.Design
{
    partial class NeuropixelsV1eDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelGainCalSN;
            System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelAdcCalSN;
            System.Windows.Forms.Label apGain;
            System.Windows.Forms.Label lfpGain;
            System.Windows.Forms.Label Reference;
            System.Windows.Forms.Label spikeFilter;
            System.Windows.Forms.Label gainCalibrationFile;
            System.Windows.Forms.Label adcCalibrationFile;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label labelSelection;
            System.Windows.Forms.Label labelPresets;
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.gainCalibrationSN = new System.Windows.Forms.ToolStripStatusLabel();
            this.adcCalibrationSN = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControlProbe = new System.Windows.Forms.TabControl();
            this.tabPageProbe = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panelProbe = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControlOptions = new System.Windows.Forms.TabControl();
            this.tabPageOptions = new System.Windows.Forms.TabPage();
            this.panelOptions = new System.Windows.Forms.Panel();
            this.textBoxLfpGainCorrection = new System.Windows.Forms.TextBox();
            this.labelLfpGainCorrection = new System.Windows.Forms.Label();
            this.textBoxApGainCorrection = new System.Windows.Forms.TextBox();
            this.labelApGainCorrection = new System.Windows.Forms.Label();
            this.checkBoxSpikeFilter = new System.Windows.Forms.CheckBox();
            this.buttonAdcCalibrationFile = new System.Windows.Forms.Button();
            this.textBoxAdcCalibrationFile = new System.Windows.Forms.TextBox();
            this.buttonGainCalibrationFile = new System.Windows.Forms.Button();
            this.textBoxGainCalibrationFile = new System.Windows.Forms.TextBox();
            this.comboBoxReference = new System.Windows.Forms.ComboBox();
            this.comboBoxLfpGain = new System.Windows.Forms.ComboBox();
            this.comboBoxApGain = new System.Windows.Forms.ComboBox();
            this.tabPageContactsOptions = new System.Windows.Forms.TabPage();
            this.panelChannelOptions = new System.Windows.Forms.Panel();
            this.comboBoxChannelPresets = new System.Windows.Forms.ComboBox();
            this.trackBarProbePosition = new System.Windows.Forms.TrackBar();
            this.buttonEnableContacts = new System.Windows.Forms.Button();
            this.buttonClearSelections = new System.Windows.Forms.Button();
            this.buttonResetZoom = new System.Windows.Forms.Button();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.tabPageADCs = new System.Windows.Forms.TabPage();
            this.dataGridViewAdcs = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOkay = new System.Windows.Forms.Button();
            this.linkLabelDocumentation = new System.Windows.Forms.LinkLabel();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabelGainCalSN = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabelAdcCalSN = new System.Windows.Forms.ToolStripStatusLabel();
            apGain = new System.Windows.Forms.Label();
            lfpGain = new System.Windows.Forms.Label();
            Reference = new System.Windows.Forms.Label();
            spikeFilter = new System.Windows.Forms.Label();
            gainCalibrationFile = new System.Windows.Forms.Label();
            adcCalibrationFile = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            labelSelection = new System.Windows.Forms.Label();
            labelPresets = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlProbe.SuspendLayout();
            this.tabPageProbe.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControlOptions.SuspendLayout();
            this.tabPageOptions.SuspendLayout();
            this.panelOptions.SuspendLayout();
            this.tabPageContactsOptions.SuspendLayout();
            this.panelChannelOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarProbePosition)).BeginInit();
            this.tabPageADCs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAdcs)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripStatusLabelGainCalSN
            // 
            toolStripStatusLabelGainCalSN.Name = "toolStripStatusLabelGainCalSN";
            toolStripStatusLabelGainCalSN.Size = new System.Drawing.Size(117, 25);
            toolStripStatusLabelGainCalSN.Text = "Gain Cal. SN: ";
            // 
            // toolStripStatusLabelAdcCalSN
            // 
            toolStripStatusLabelAdcCalSN.Name = "toolStripStatusLabelAdcCalSN";
            toolStripStatusLabelAdcCalSN.Size = new System.Drawing.Size(118, 25);
            toolStripStatusLabelAdcCalSN.Text = "ADC Cal. SN: ";
            // 
            // apGain
            // 
            apGain.AutoSize = true;
            apGain.Location = new System.Drawing.Point(13, 17);
            apGain.Name = "apGain";
            apGain.Size = new System.Drawing.Size(68, 20);
            apGain.TabIndex = 0;
            apGain.Text = "AP Gain";
            // 
            // lfpGain
            // 
            lfpGain.AutoSize = true;
            lfpGain.Location = new System.Drawing.Point(13, 97);
            lfpGain.Name = "lfpGain";
            lfpGain.Size = new System.Drawing.Size(76, 20);
            lfpGain.TabIndex = 2;
            lfpGain.Text = "LFP Gain";
            // 
            // Reference
            // 
            Reference.AutoSize = true;
            Reference.Location = new System.Drawing.Point(13, 187);
            Reference.Name = "Reference";
            Reference.Size = new System.Drawing.Size(84, 20);
            Reference.TabIndex = 4;
            Reference.Text = "Reference";
            // 
            // spikeFilter
            // 
            spikeFilter.AutoSize = true;
            spikeFilter.Location = new System.Drawing.Point(13, 230);
            spikeFilter.Name = "spikeFilter";
            spikeFilter.Size = new System.Drawing.Size(88, 20);
            spikeFilter.TabIndex = 6;
            spikeFilter.Text = "Spike Filter";
            // 
            // gainCalibrationFile
            // 
            gainCalibrationFile.AutoSize = true;
            gainCalibrationFile.Location = new System.Drawing.Point(13, 275);
            gainCalibrationFile.MaximumSize = new System.Drawing.Size(200, 45);
            gainCalibrationFile.Name = "gainCalibrationFile";
            gainCalibrationFile.Size = new System.Drawing.Size(151, 20);
            gainCalibrationFile.TabIndex = 8;
            gainCalibrationFile.Text = "Gain Calibration File";
            // 
            // adcCalibrationFile
            // 
            adcCalibrationFile.AutoSize = true;
            adcCalibrationFile.Location = new System.Drawing.Point(13, 382);
            adcCalibrationFile.MaximumSize = new System.Drawing.Size(200, 45);
            adcCalibrationFile.Name = "adcCalibrationFile";
            adcCalibrationFile.Size = new System.Drawing.Size(151, 20);
            adcCalibrationFile.TabIndex = 11;
            adcCalibrationFile.Text = "ADC Calibration File";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(20, 16);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(66, 20);
            label1.TabIndex = 5;
            label1.Text = "Jump to";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(161, 16);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(50, 20);
            label2.TabIndex = 6;
            label2.Text = "Zoom";
            // 
            // labelSelection
            // 
            labelSelection.AutoSize = true;
            labelSelection.Location = new System.Drawing.Point(149, 191);
            labelSelection.Name = "labelSelection";
            labelSelection.Size = new System.Drawing.Size(75, 20);
            labelSelection.TabIndex = 18;
            labelSelection.Text = "Selection";
            // 
            // labelPresets
            // 
            labelPresets.AutoSize = true;
            labelPresets.Location = new System.Drawing.Point(149, 384);
            labelPresets.Name = "labelPresets";
            labelPresets.Size = new System.Drawing.Size(63, 20);
            labelPresets.TabIndex = 23;
            labelPresets.Text = "Presets";
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1130, 36);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripStatusLabelGainCalSN,
            this.gainCalibrationSN,
            toolStripStatusLabelAdcCalSN,
            this.adcCalibrationSN,
            this.toolStripStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 702);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1130, 32);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // gainCalibrationSN
            // 
            this.gainCalibrationSN.AutoSize = false;
            this.gainCalibrationSN.Name = "gainCalibrationSN";
            this.gainCalibrationSN.Size = new System.Drawing.Size(150, 25);
            this.gainCalibrationSN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // adcCalibrationSN
            // 
            this.adcCalibrationSN.AutoSize = false;
            this.adcCalibrationSN.Name = "adcCalibrationSN";
            this.adcCalibrationSN.Size = new System.Drawing.Size(150, 25);
            this.adcCalibrationSN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 36);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControlProbe);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1130, 666);
            this.splitContainer1.SplitterDistance = 614;
            this.splitContainer1.TabIndex = 2;
            // 
            // tabControlProbe
            // 
            this.tabControlProbe.Controls.Add(this.tabPageProbe);
            this.tabControlProbe.Controls.Add(this.tabPageADCs);
            this.tabControlProbe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlProbe.Location = new System.Drawing.Point(0, 0);
            this.tabControlProbe.Name = "tabControlProbe";
            this.tabControlProbe.SelectedIndex = 0;
            this.tabControlProbe.Size = new System.Drawing.Size(1130, 614);
            this.tabControlProbe.TabIndex = 0;
            // 
            // tabPageProbe
            // 
            this.tabPageProbe.Controls.Add(this.splitContainer2);
            this.tabPageProbe.Location = new System.Drawing.Point(4, 29);
            this.tabPageProbe.Name = "tabPageProbe";
            this.tabPageProbe.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProbe.Size = new System.Drawing.Size(1122, 581);
            this.tabPageProbe.TabIndex = 0;
            this.tabPageProbe.Text = "Probe";
            this.tabPageProbe.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panelProbe);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel2);
            this.splitContainer2.Size = new System.Drawing.Size(1116, 575);
            this.splitContainer2.SplitterDistance = 858;
            this.splitContainer2.TabIndex = 0;
            // 
            // panelProbe
            // 
            this.panelProbe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProbe.Location = new System.Drawing.Point(0, 0);
            this.panelProbe.Name = "panelProbe";
            this.panelProbe.Size = new System.Drawing.Size(858, 575);
            this.panelProbe.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControlOptions);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(254, 575);
            this.panel2.TabIndex = 0;
            // 
            // tabControlOptions
            // 
            this.tabControlOptions.Controls.Add(this.tabPageOptions);
            this.tabControlOptions.Controls.Add(this.tabPageContactsOptions);
            this.tabControlOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlOptions.Location = new System.Drawing.Point(0, 0);
            this.tabControlOptions.Name = "tabControlOptions";
            this.tabControlOptions.SelectedIndex = 0;
            this.tabControlOptions.Size = new System.Drawing.Size(254, 575);
            this.tabControlOptions.TabIndex = 0;
            // 
            // tabPageOptions
            // 
            this.tabPageOptions.Controls.Add(this.panelOptions);
            this.tabPageOptions.Location = new System.Drawing.Point(4, 29);
            this.tabPageOptions.Name = "tabPageOptions";
            this.tabPageOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOptions.Size = new System.Drawing.Size(246, 542);
            this.tabPageOptions.TabIndex = 0;
            this.tabPageOptions.Text = "Options";
            this.tabPageOptions.UseVisualStyleBackColor = true;
            // 
            // panelOptions
            // 
            this.panelOptions.Controls.Add(this.textBoxLfpGainCorrection);
            this.panelOptions.Controls.Add(this.labelLfpGainCorrection);
            this.panelOptions.Controls.Add(this.textBoxApGainCorrection);
            this.panelOptions.Controls.Add(this.labelApGainCorrection);
            this.panelOptions.Controls.Add(this.checkBoxSpikeFilter);
            this.panelOptions.Controls.Add(this.buttonAdcCalibrationFile);
            this.panelOptions.Controls.Add(this.textBoxAdcCalibrationFile);
            this.panelOptions.Controls.Add(adcCalibrationFile);
            this.panelOptions.Controls.Add(this.buttonGainCalibrationFile);
            this.panelOptions.Controls.Add(this.textBoxGainCalibrationFile);
            this.panelOptions.Controls.Add(gainCalibrationFile);
            this.panelOptions.Controls.Add(spikeFilter);
            this.panelOptions.Controls.Add(this.comboBoxReference);
            this.panelOptions.Controls.Add(Reference);
            this.panelOptions.Controls.Add(this.comboBoxLfpGain);
            this.panelOptions.Controls.Add(lfpGain);
            this.panelOptions.Controls.Add(this.comboBoxApGain);
            this.panelOptions.Controls.Add(apGain);
            this.panelOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOptions.Location = new System.Drawing.Point(3, 3);
            this.panelOptions.Name = "panelOptions";
            this.panelOptions.Size = new System.Drawing.Size(240, 536);
            this.panelOptions.TabIndex = 0;
            // 
            // textBoxLfpGainCorrection
            // 
            this.textBoxLfpGainCorrection.Location = new System.Drawing.Point(100, 130);
            this.textBoxLfpGainCorrection.Name = "textBoxLfpGainCorrection";
            this.textBoxLfpGainCorrection.ReadOnly = true;
            this.textBoxLfpGainCorrection.Size = new System.Drawing.Size(123, 26);
            this.textBoxLfpGainCorrection.TabIndex = 18;
            this.textBoxLfpGainCorrection.Visible = false;
            // 
            // labelLfpGainCorrection
            // 
            this.labelLfpGainCorrection.AutoSize = true;
            this.labelLfpGainCorrection.Location = new System.Drawing.Point(12, 129);
            this.labelLfpGainCorrection.Name = "labelLfpGainCorrection";
            this.labelLfpGainCorrection.Size = new System.Drawing.Size(82, 20);
            this.labelLfpGainCorrection.TabIndex = 17;
            this.labelLfpGainCorrection.Text = "Correction";
            this.labelLfpGainCorrection.Visible = false;
            // 
            // textBoxApGainCorrection
            // 
            this.textBoxApGainCorrection.Location = new System.Drawing.Point(101, 48);
            this.textBoxApGainCorrection.Name = "textBoxApGainCorrection";
            this.textBoxApGainCorrection.ReadOnly = true;
            this.textBoxApGainCorrection.Size = new System.Drawing.Size(123, 26);
            this.textBoxApGainCorrection.TabIndex = 16;
            this.textBoxApGainCorrection.Visible = false;
            // 
            // labelApGainCorrection
            // 
            this.labelApGainCorrection.AutoSize = true;
            this.labelApGainCorrection.Location = new System.Drawing.Point(13, 47);
            this.labelApGainCorrection.Name = "labelApGainCorrection";
            this.labelApGainCorrection.Size = new System.Drawing.Size(82, 20);
            this.labelApGainCorrection.TabIndex = 15;
            this.labelApGainCorrection.Text = "Correction";
            this.labelApGainCorrection.Visible = false;
            // 
            // checkBoxSpikeFilter
            // 
            this.checkBoxSpikeFilter.AutoSize = true;
            this.checkBoxSpikeFilter.Location = new System.Drawing.Point(118, 230);
            this.checkBoxSpikeFilter.Name = "checkBoxSpikeFilter";
            this.checkBoxSpikeFilter.Size = new System.Drawing.Size(94, 24);
            this.checkBoxSpikeFilter.TabIndex = 14;
            this.checkBoxSpikeFilter.Text = "Enabled";
            this.checkBoxSpikeFilter.UseVisualStyleBackColor = true;
            // 
            // buttonAdcCalibrationFile
            // 
            this.buttonAdcCalibrationFile.Location = new System.Drawing.Point(43, 437);
            this.buttonAdcCalibrationFile.Name = "buttonAdcCalibrationFile";
            this.buttonAdcCalibrationFile.Size = new System.Drawing.Size(141, 32);
            this.buttonAdcCalibrationFile.TabIndex = 13;
            this.buttonAdcCalibrationFile.Text = "Choose";
            this.buttonAdcCalibrationFile.UseVisualStyleBackColor = true;
            this.buttonAdcCalibrationFile.Click += new System.EventHandler(this.ButtonClick);
            // 
            // textBoxAdcCalibrationFile
            // 
            this.textBoxAdcCalibrationFile.Location = new System.Drawing.Point(17, 405);
            this.textBoxAdcCalibrationFile.Name = "textBoxAdcCalibrationFile";
            this.textBoxAdcCalibrationFile.ReadOnly = true;
            this.textBoxAdcCalibrationFile.Size = new System.Drawing.Size(207, 26);
            this.textBoxAdcCalibrationFile.TabIndex = 12;
            this.textBoxAdcCalibrationFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // buttonGainCalibrationFile
            // 
            this.buttonGainCalibrationFile.Location = new System.Drawing.Point(43, 330);
            this.buttonGainCalibrationFile.Name = "buttonGainCalibrationFile";
            this.buttonGainCalibrationFile.Size = new System.Drawing.Size(141, 32);
            this.buttonGainCalibrationFile.TabIndex = 10;
            this.buttonGainCalibrationFile.Text = "Choose";
            this.buttonGainCalibrationFile.UseVisualStyleBackColor = true;
            this.buttonGainCalibrationFile.Click += new System.EventHandler(this.ButtonClick);
            // 
            // textBoxGainCalibrationFile
            // 
            this.textBoxGainCalibrationFile.Location = new System.Drawing.Point(17, 298);
            this.textBoxGainCalibrationFile.Name = "textBoxGainCalibrationFile";
            this.textBoxGainCalibrationFile.ReadOnly = true;
            this.textBoxGainCalibrationFile.Size = new System.Drawing.Size(207, 26);
            this.textBoxGainCalibrationFile.TabIndex = 9;
            this.textBoxGainCalibrationFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // comboBoxReference
            // 
            this.comboBoxReference.FormattingEnabled = true;
            this.comboBoxReference.Location = new System.Drawing.Point(102, 184);
            this.comboBoxReference.Name = "comboBoxReference";
            this.comboBoxReference.Size = new System.Drawing.Size(121, 28);
            this.comboBoxReference.TabIndex = 5;
            // 
            // comboBoxLfpGain
            // 
            this.comboBoxLfpGain.FormattingEnabled = true;
            this.comboBoxLfpGain.Location = new System.Drawing.Point(102, 94);
            this.comboBoxLfpGain.Name = "comboBoxLfpGain";
            this.comboBoxLfpGain.Size = new System.Drawing.Size(121, 28);
            this.comboBoxLfpGain.TabIndex = 3;
            // 
            // comboBoxApGain
            // 
            this.comboBoxApGain.FormattingEnabled = true;
            this.comboBoxApGain.Location = new System.Drawing.Point(102, 14);
            this.comboBoxApGain.Name = "comboBoxApGain";
            this.comboBoxApGain.Size = new System.Drawing.Size(121, 28);
            this.comboBoxApGain.TabIndex = 1;
            // 
            // tabPageContactsOptions
            // 
            this.tabPageContactsOptions.Controls.Add(this.panelChannelOptions);
            this.tabPageContactsOptions.Location = new System.Drawing.Point(4, 29);
            this.tabPageContactsOptions.Name = "tabPageContactsOptions";
            this.tabPageContactsOptions.Size = new System.Drawing.Size(246, 542);
            this.tabPageContactsOptions.TabIndex = 2;
            this.tabPageContactsOptions.Text = "Contacts";
            this.tabPageContactsOptions.UseVisualStyleBackColor = true;
            // 
            // panelChannelOptions
            // 
            this.panelChannelOptions.Controls.Add(this.comboBoxChannelPresets);
            this.panelChannelOptions.Controls.Add(labelPresets);
            this.panelChannelOptions.Controls.Add(this.trackBarProbePosition);
            this.panelChannelOptions.Controls.Add(this.buttonEnableContacts);
            this.panelChannelOptions.Controls.Add(this.buttonClearSelections);
            this.panelChannelOptions.Controls.Add(labelSelection);
            this.panelChannelOptions.Controls.Add(label2);
            this.panelChannelOptions.Controls.Add(label1);
            this.panelChannelOptions.Controls.Add(this.buttonResetZoom);
            this.panelChannelOptions.Controls.Add(this.buttonZoomOut);
            this.panelChannelOptions.Controls.Add(this.buttonZoomIn);
            this.panelChannelOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChannelOptions.Location = new System.Drawing.Point(0, 0);
            this.panelChannelOptions.Name = "panelChannelOptions";
            this.panelChannelOptions.Size = new System.Drawing.Size(246, 542);
            this.panelChannelOptions.TabIndex = 0;
            // 
            // comboBoxChannelPresets
            // 
            this.comboBoxChannelPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChannelPresets.FormattingEnabled = true;
            this.comboBoxChannelPresets.Location = new System.Drawing.Point(134, 420);
            this.comboBoxChannelPresets.Name = "comboBoxChannelPresets";
            this.comboBoxChannelPresets.Size = new System.Drawing.Size(102, 28);
            this.comboBoxChannelPresets.TabIndex = 24;
            // 
            // trackBarProbePosition
            // 
            this.trackBarProbePosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.trackBarProbePosition.Location = new System.Drawing.Point(17, 39);
            this.trackBarProbePosition.Maximum = 100;
            this.trackBarProbePosition.Name = "trackBarProbePosition";
            this.trackBarProbePosition.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarProbePosition.Size = new System.Drawing.Size(69, 482);
            this.trackBarProbePosition.TabIndex = 22;
            this.trackBarProbePosition.Scroll += new System.EventHandler(this.TrackBarScroll);
            // 
            // buttonEnableContacts
            // 
            this.buttonEnableContacts.Location = new System.Drawing.Point(140, 229);
            this.buttonEnableContacts.Name = "buttonEnableContacts";
            this.buttonEnableContacts.Size = new System.Drawing.Size(96, 56);
            this.buttonEnableContacts.TabIndex = 20;
            this.buttonEnableContacts.Text = "Enable Contacts";
            this.buttonEnableContacts.UseVisualStyleBackColor = true;
            this.buttonEnableContacts.Click += new System.EventHandler(this.ButtonClick);
            // 
            // buttonClearSelections
            // 
            this.buttonClearSelections.Location = new System.Drawing.Point(140, 291);
            this.buttonClearSelections.Name = "buttonClearSelections";
            this.buttonClearSelections.Size = new System.Drawing.Size(96, 59);
            this.buttonClearSelections.TabIndex = 19;
            this.buttonClearSelections.Text = "Clear Selection";
            this.buttonClearSelections.UseVisualStyleBackColor = true;
            this.buttonClearSelections.Click += new System.EventHandler(this.ButtonClick);
            // 
            // buttonResetZoom
            // 
            this.buttonResetZoom.Location = new System.Drawing.Point(140, 129);
            this.buttonResetZoom.Name = "buttonResetZoom";
            this.buttonResetZoom.Size = new System.Drawing.Size(96, 34);
            this.buttonResetZoom.TabIndex = 4;
            this.buttonResetZoom.Text = "Reset";
            this.buttonResetZoom.UseVisualStyleBackColor = true;
            this.buttonResetZoom.Click += new System.EventHandler(this.ButtonClick);
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.Location = new System.Drawing.Point(140, 89);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(96, 34);
            this.buttonZoomOut.TabIndex = 3;
            this.buttonZoomOut.Text = "Zoom Out";
            this.buttonZoomOut.UseVisualStyleBackColor = true;
            this.buttonZoomOut.Click += new System.EventHandler(this.ButtonClick);
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.Location = new System.Drawing.Point(140, 49);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(96, 34);
            this.buttonZoomIn.TabIndex = 2;
            this.buttonZoomIn.Text = "Zoom In";
            this.buttonZoomIn.UseVisualStyleBackColor = true;
            this.buttonZoomIn.Click += new System.EventHandler(this.ButtonClick);
            // 
            // tabPageADCs
            // 
            this.tabPageADCs.Controls.Add(this.dataGridViewAdcs);
            this.tabPageADCs.Location = new System.Drawing.Point(4, 29);
            this.tabPageADCs.Name = "tabPageADCs";
            this.tabPageADCs.Size = new System.Drawing.Size(1122, 581);
            this.tabPageADCs.TabIndex = 2;
            this.tabPageADCs.Text = "ADCs";
            this.tabPageADCs.UseVisualStyleBackColor = true;
            // 
            // dataGridViewAdcs
            // 
            this.dataGridViewAdcs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAdcs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAdcs.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewAdcs.Name = "dataGridViewAdcs";
            this.dataGridViewAdcs.RowHeadersWidth = 62;
            this.dataGridViewAdcs.RowTemplate.Height = 28;
            this.dataGridViewAdcs.Size = new System.Drawing.Size(1122, 581);
            this.dataGridViewAdcs.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonOkay);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1130, 48);
            this.panel1.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(994, 7);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(124, 34);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonClick);
            // 
            // buttonOkay
            // 
            this.buttonOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOkay.Location = new System.Drawing.Point(861, 7);
            this.buttonOkay.Name = "buttonOkay";
            this.buttonOkay.Size = new System.Drawing.Size(124, 34);
            this.buttonOkay.TabIndex = 0;
            this.buttonOkay.Text = "OK";
            this.buttonOkay.UseVisualStyleBackColor = true;
            this.buttonOkay.Click += new System.EventHandler(this.ButtonClick);
            // 
            // linkLabelDocumentation
            // 
            this.linkLabelDocumentation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelDocumentation.AutoSize = true;
            this.linkLabelDocumentation.BackColor = System.Drawing.Color.GhostWhite;
            this.linkLabelDocumentation.Location = new System.Drawing.Point(1009, 4);
            this.linkLabelDocumentation.Name = "linkLabelDocumentation";
            this.linkLabelDocumentation.Size = new System.Drawing.Size(118, 20);
            this.linkLabelDocumentation.TabIndex = 3;
            this.linkLabelDocumentation.TabStop = true;
            this.linkLabelDocumentation.Text = "Documentation";
            this.linkLabelDocumentation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 30);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Image = global::OpenEphys.Onix.Design.Properties.Resources.StatusReadyImage;
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(24, 24);
            // 
            // NeuropixelsV1eDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1130, 734);
            this.Controls.Add(this.linkLabelDocumentation);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "NeuropixelsV1eDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NeuropixelsV1eDialog";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlProbe.ResumeLayout(false);
            this.tabPageProbe.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControlOptions.ResumeLayout(false);
            this.tabPageOptions.ResumeLayout(false);
            this.panelOptions.ResumeLayout(false);
            this.panelOptions.PerformLayout();
            this.tabPageContactsOptions.ResumeLayout(false);
            this.panelChannelOptions.ResumeLayout(false);
            this.panelChannelOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarProbePosition)).EndInit();
            this.tabPageADCs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAdcs)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControlProbe;
        private System.Windows.Forms.TabPage tabPageProbe;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelProbe;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOkay;
        private System.Windows.Forms.ToolStripStatusLabel gainCalibrationSN;
        private System.Windows.Forms.ToolStripStatusLabel adcCalibrationSN;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.TabPage tabPageADCs;
        private System.Windows.Forms.LinkLabel linkLabelDocumentation;
        private System.Windows.Forms.TabControl tabControlOptions;
        private System.Windows.Forms.TabPage tabPageOptions;
        private System.Windows.Forms.Panel panelOptions;
        private System.Windows.Forms.ComboBox comboBoxReference;
        private System.Windows.Forms.ComboBox comboBoxLfpGain;
        private System.Windows.Forms.ComboBox comboBoxApGain;
        private System.Windows.Forms.TextBox textBoxGainCalibrationFile;
        private System.Windows.Forms.Button buttonGainCalibrationFile;
        private System.Windows.Forms.Button buttonAdcCalibrationFile;
        private System.Windows.Forms.TextBox textBoxAdcCalibrationFile;
        private System.Windows.Forms.TabPage tabPageContactsOptions;
        private System.Windows.Forms.Panel panelChannelOptions;
        private System.Windows.Forms.CheckBox checkBoxSpikeFilter;
        private System.Windows.Forms.Button buttonZoomIn;
        private System.Windows.Forms.Button buttonResetZoom;
        private System.Windows.Forms.Button buttonZoomOut;
        private System.Windows.Forms.DataGridView dataGridViewAdcs;
        private System.Windows.Forms.Label labelApGainCorrection;
        private System.Windows.Forms.TextBox textBoxApGainCorrection;
        private System.Windows.Forms.TextBox textBoxLfpGainCorrection;
        private System.Windows.Forms.Label labelLfpGainCorrection;
        private System.Windows.Forms.Button buttonClearSelections;
        private System.Windows.Forms.Button buttonEnableContacts;
        private System.Windows.Forms.TrackBar trackBarProbePosition;
        private System.Windows.Forms.ComboBox comboBoxChannelPresets;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    }
}
