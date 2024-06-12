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
            System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelProbe;
            System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelConfigSN;
            System.Windows.Forms.Label apGain;
            System.Windows.Forms.Label lfpGain;
            System.Windows.Forms.Label Reference;
            System.Windows.Forms.Label spikeFilter;
            System.Windows.Forms.Label gainCalibrationFile;
            System.Windows.Forms.Label adcCalibrationFile;
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripSplitButtonUpload = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripProgressBarUpload = new System.Windows.Forms.ToolStripProgressBar();
            this.probeSN = new System.Windows.Forms.ToolStripStatusLabel();
            this.configSN = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControlProbe = new System.Windows.Forms.TabControl();
            this.tabPageProbe = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panelProbe = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControlOptions = new System.Windows.Forms.TabControl();
            this.tabPageOptions = new System.Windows.Forms.TabPage();
            this.panelOptions = new System.Windows.Forms.Panel();
            this.checkBoxSpikeFilter = new System.Windows.Forms.CheckBox();
            this.buttonAdcCalibrationFile = new System.Windows.Forms.Button();
            this.textBoxAdcCalibrationFile = new System.Windows.Forms.TextBox();
            this.buttonGainCalibrationFile = new System.Windows.Forms.Button();
            this.textBoxGainCalibrationFile = new System.Windows.Forms.TextBox();
            this.comboBoxReference = new System.Windows.Forms.ComboBox();
            this.comboBoxLfpGain = new System.Windows.Forms.ComboBox();
            this.comboBoxApGain = new System.Windows.Forms.ComboBox();
            this.tabPageChannelOptions = new System.Windows.Forms.TabPage();
            this.panelChannelOptions = new System.Windows.Forms.Panel();
            this.buttonZoomIn100 = new System.Windows.Forms.Button();
            this.buttonZoomIn10 = new System.Windows.Forms.Button();
            this.tabPagePropertyGrid = new System.Windows.Forms.TabPage();
            this.propertyGrid = new Bonsai.Design.PropertyGrid();
            this.tabPageChannels = new System.Windows.Forms.TabPage();
            this.tabPageADCs = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOkay = new System.Windows.Forms.Button();
            this.linkLabelDocumentation = new System.Windows.Forms.LinkLabel();
            this.buttonZoomOut100 = new System.Windows.Forms.Button();
            this.buttonZoomOut10 = new System.Windows.Forms.Button();
            this.buttonResetZoom = new System.Windows.Forms.Button();
            toolStripStatusLabelProbe = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabelConfigSN = new System.Windows.Forms.ToolStripStatusLabel();
            apGain = new System.Windows.Forms.Label();
            lfpGain = new System.Windows.Forms.Label();
            Reference = new System.Windows.Forms.Label();
            spikeFilter = new System.Windows.Forms.Label();
            gainCalibrationFile = new System.Windows.Forms.Label();
            adcCalibrationFile = new System.Windows.Forms.Label();
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
            this.tabPageChannelOptions.SuspendLayout();
            this.panelChannelOptions.SuspendLayout();
            this.tabPagePropertyGrid.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripStatusLabelProbe
            // 
            toolStripStatusLabelProbe.Name = "toolStripStatusLabelProbe";
            toolStripStatusLabelProbe.Size = new System.Drawing.Size(91, 25);
            toolStripStatusLabelProbe.Text = "Probe SN:";
            // 
            // toolStripStatusLabelConfigSN
            // 
            toolStripStatusLabelConfigSN.Name = "toolStripStatusLabelConfigSN";
            toolStripStatusLabelConfigSN.Size = new System.Drawing.Size(97, 25);
            toolStripStatusLabelConfigSN.Text = "Config. SN";
            // 
            // apGain
            // 
            apGain.AutoSize = true;
            apGain.Location = new System.Drawing.Point(13, 30);
            apGain.Name = "apGain";
            apGain.Size = new System.Drawing.Size(68, 20);
            apGain.TabIndex = 0;
            apGain.Text = "AP Gain";
            // 
            // lfpGain
            // 
            lfpGain.AutoSize = true;
            lfpGain.Location = new System.Drawing.Point(13, 88);
            lfpGain.Name = "lfpGain";
            lfpGain.Size = new System.Drawing.Size(76, 20);
            lfpGain.TabIndex = 2;
            lfpGain.Text = "LFP Gain";
            // 
            // Reference
            // 
            Reference.AutoSize = true;
            Reference.Location = new System.Drawing.Point(13, 147);
            Reference.Name = "Reference";
            Reference.Size = new System.Drawing.Size(84, 20);
            Reference.TabIndex = 4;
            Reference.Text = "Reference";
            // 
            // spikeFilter
            // 
            spikeFilter.AutoSize = true;
            spikeFilter.Location = new System.Drawing.Point(13, 202);
            spikeFilter.Name = "spikeFilter";
            spikeFilter.Size = new System.Drawing.Size(88, 20);
            spikeFilter.TabIndex = 6;
            spikeFilter.Text = "Spike Filter";
            // 
            // gainCalibrationFile
            // 
            gainCalibrationFile.AutoSize = true;
            gainCalibrationFile.Location = new System.Drawing.Point(13, 247);
            gainCalibrationFile.MaximumSize = new System.Drawing.Size(200, 45);
            gainCalibrationFile.Name = "gainCalibrationFile";
            gainCalibrationFile.Size = new System.Drawing.Size(151, 20);
            gainCalibrationFile.TabIndex = 8;
            gainCalibrationFile.Text = "Gain Calibration File";
            // 
            // adcCalibrationFile
            // 
            adcCalibrationFile.AutoSize = true;
            adcCalibrationFile.Location = new System.Drawing.Point(13, 354);
            adcCalibrationFile.MaximumSize = new System.Drawing.Size(200, 45);
            adcCalibrationFile.Name = "adcCalibrationFile";
            adcCalibrationFile.Size = new System.Drawing.Size(151, 20);
            adcCalibrationFile.TabIndex = 11;
            adcCalibrationFile.Text = "ADC Calibration File";
            // 
            // menuStrip
            // 
            this.menuStrip.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1130, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButtonUpload,
            this.toolStripProgressBarUpload,
            toolStripStatusLabelProbe,
            this.probeSN,
            toolStripStatusLabelConfigSN,
            this.configSN,
            this.toolStripStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 702);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1130, 32);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripSplitButtonUpload
            // 
            this.toolStripSplitButtonUpload.DropDownButtonWidth = 0;
            this.toolStripSplitButtonUpload.Image = global::OpenEphys.Onix.Design.Properties.Resources.UploadImage;
            this.toolStripSplitButtonUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripSplitButtonUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonUpload.Name = "toolStripSplitButtonUpload";
            this.toolStripSplitButtonUpload.Size = new System.Drawing.Size(99, 29);
            this.toolStripSplitButtonUpload.Text = "Upload";
            // 
            // toolStripProgressBarUpload
            // 
            this.toolStripProgressBarUpload.Name = "toolStripProgressBarUpload";
            this.toolStripProgressBarUpload.Size = new System.Drawing.Size(100, 24);
            // 
            // probeSN
            // 
            this.probeSN.AutoSize = false;
            this.probeSN.Name = "probeSN";
            this.probeSN.Size = new System.Drawing.Size(200, 25);
            this.probeSN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // configSN
            // 
            this.configSN.AutoSize = false;
            this.configSN.Name = "configSN";
            this.configSN.Size = new System.Drawing.Size(200, 25);
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Image = global::OpenEphys.Onix.Design.Properties.Resources.StatusReadyImage;
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(24, 25);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
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
            this.splitContainer1.Size = new System.Drawing.Size(1130, 678);
            this.splitContainer1.SplitterDistance = 626;
            this.splitContainer1.TabIndex = 2;
            // 
            // tabControlProbe
            // 
            this.tabControlProbe.Controls.Add(this.tabPageProbe);
            this.tabControlProbe.Controls.Add(this.tabPageChannels);
            this.tabControlProbe.Controls.Add(this.tabPageADCs);
            this.tabControlProbe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlProbe.Location = new System.Drawing.Point(0, 0);
            this.tabControlProbe.Name = "tabControlProbe";
            this.tabControlProbe.SelectedIndex = 0;
            this.tabControlProbe.Size = new System.Drawing.Size(1130, 626);
            this.tabControlProbe.TabIndex = 0;
            // 
            // tabPageProbe
            // 
            this.tabPageProbe.Controls.Add(this.splitContainer2);
            this.tabPageProbe.Location = new System.Drawing.Point(4, 29);
            this.tabPageProbe.Name = "tabPageProbe";
            this.tabPageProbe.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProbe.Size = new System.Drawing.Size(1122, 593);
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
            this.splitContainer2.Size = new System.Drawing.Size(1116, 587);
            this.splitContainer2.SplitterDistance = 858;
            this.splitContainer2.TabIndex = 0;
            // 
            // panelProbe
            // 
            this.panelProbe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProbe.Location = new System.Drawing.Point(0, 0);
            this.panelProbe.Name = "panelProbe";
            this.panelProbe.Size = new System.Drawing.Size(858, 587);
            this.panelProbe.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControlOptions);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(254, 587);
            this.panel2.TabIndex = 0;
            // 
            // tabControlOptions
            // 
            this.tabControlOptions.Controls.Add(this.tabPageOptions);
            this.tabControlOptions.Controls.Add(this.tabPageChannelOptions);
            this.tabControlOptions.Controls.Add(this.tabPagePropertyGrid);
            this.tabControlOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlOptions.Location = new System.Drawing.Point(0, 0);
            this.tabControlOptions.Name = "tabControlOptions";
            this.tabControlOptions.SelectedIndex = 0;
            this.tabControlOptions.Size = new System.Drawing.Size(254, 587);
            this.tabControlOptions.TabIndex = 0;
            // 
            // tabPageOptions
            // 
            this.tabPageOptions.Controls.Add(this.panelOptions);
            this.tabPageOptions.Location = new System.Drawing.Point(4, 29);
            this.tabPageOptions.Name = "tabPageOptions";
            this.tabPageOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOptions.Size = new System.Drawing.Size(246, 554);
            this.tabPageOptions.TabIndex = 0;
            this.tabPageOptions.Text = "Options";
            this.tabPageOptions.UseVisualStyleBackColor = true;
            // 
            // panelOptions
            // 
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
            this.panelOptions.Size = new System.Drawing.Size(240, 548);
            this.panelOptions.TabIndex = 0;
            // 
            // checkBoxSpikeFilter
            // 
            this.checkBoxSpikeFilter.AutoSize = true;
            this.checkBoxSpikeFilter.Location = new System.Drawing.Point(118, 202);
            this.checkBoxSpikeFilter.Name = "checkBoxSpikeFilter";
            this.checkBoxSpikeFilter.Size = new System.Drawing.Size(94, 24);
            this.checkBoxSpikeFilter.TabIndex = 14;
            this.checkBoxSpikeFilter.Text = "Enabled";
            this.checkBoxSpikeFilter.UseVisualStyleBackColor = true;
            // 
            // buttonAdcCalibrationFile
            // 
            this.buttonAdcCalibrationFile.Location = new System.Drawing.Point(43, 409);
            this.buttonAdcCalibrationFile.Name = "buttonAdcCalibrationFile";
            this.buttonAdcCalibrationFile.Size = new System.Drawing.Size(141, 32);
            this.buttonAdcCalibrationFile.TabIndex = 13;
            this.buttonAdcCalibrationFile.Text = "Choose";
            this.buttonAdcCalibrationFile.UseVisualStyleBackColor = true;
            this.buttonAdcCalibrationFile.Click += new System.EventHandler(this.ButtonClick);
            // 
            // textBoxAdcCalibrationFile
            // 
            this.textBoxAdcCalibrationFile.Location = new System.Drawing.Point(17, 377);
            this.textBoxAdcCalibrationFile.Name = "textBoxAdcCalibrationFile";
            this.textBoxAdcCalibrationFile.ReadOnly = true;
            this.textBoxAdcCalibrationFile.Size = new System.Drawing.Size(207, 26);
            this.textBoxAdcCalibrationFile.TabIndex = 12;
            this.textBoxAdcCalibrationFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // buttonGainCalibrationFile
            // 
            this.buttonGainCalibrationFile.Location = new System.Drawing.Point(43, 302);
            this.buttonGainCalibrationFile.Name = "buttonGainCalibrationFile";
            this.buttonGainCalibrationFile.Size = new System.Drawing.Size(141, 32);
            this.buttonGainCalibrationFile.TabIndex = 10;
            this.buttonGainCalibrationFile.Text = "Choose";
            this.buttonGainCalibrationFile.UseVisualStyleBackColor = true;
            this.buttonGainCalibrationFile.Click += new System.EventHandler(this.ButtonClick);
            // 
            // textBoxGainCalibrationFile
            // 
            this.textBoxGainCalibrationFile.Location = new System.Drawing.Point(17, 270);
            this.textBoxGainCalibrationFile.Name = "textBoxGainCalibrationFile";
            this.textBoxGainCalibrationFile.ReadOnly = true;
            this.textBoxGainCalibrationFile.Size = new System.Drawing.Size(207, 26);
            this.textBoxGainCalibrationFile.TabIndex = 9;
            this.textBoxGainCalibrationFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // comboBoxReference
            // 
            this.comboBoxReference.FormattingEnabled = true;
            this.comboBoxReference.Location = new System.Drawing.Point(102, 144);
            this.comboBoxReference.Name = "comboBoxReference";
            this.comboBoxReference.Size = new System.Drawing.Size(121, 28);
            this.comboBoxReference.TabIndex = 5;
            // 
            // comboBoxLfpGain
            // 
            this.comboBoxLfpGain.FormattingEnabled = true;
            this.comboBoxLfpGain.Location = new System.Drawing.Point(102, 85);
            this.comboBoxLfpGain.Name = "comboBoxLfpGain";
            this.comboBoxLfpGain.Size = new System.Drawing.Size(121, 28);
            this.comboBoxLfpGain.TabIndex = 3;
            // 
            // comboBoxApGain
            // 
            this.comboBoxApGain.FormattingEnabled = true;
            this.comboBoxApGain.Location = new System.Drawing.Point(102, 27);
            this.comboBoxApGain.Name = "comboBoxApGain";
            this.comboBoxApGain.Size = new System.Drawing.Size(121, 28);
            this.comboBoxApGain.TabIndex = 1;
            // 
            // tabPageChannelOptions
            // 
            this.tabPageChannelOptions.Controls.Add(this.panelChannelOptions);
            this.tabPageChannelOptions.Location = new System.Drawing.Point(4, 29);
            this.tabPageChannelOptions.Name = "tabPageChannelOptions";
            this.tabPageChannelOptions.Size = new System.Drawing.Size(246, 554);
            this.tabPageChannelOptions.TabIndex = 2;
            this.tabPageChannelOptions.Text = "Channels";
            this.tabPageChannelOptions.UseVisualStyleBackColor = true;
            // 
            // panelChannelOptions
            // 
            this.panelChannelOptions.Controls.Add(this.buttonResetZoom);
            this.panelChannelOptions.Controls.Add(this.buttonZoomOut100);
            this.panelChannelOptions.Controls.Add(this.buttonZoomOut10);
            this.panelChannelOptions.Controls.Add(this.buttonZoomIn100);
            this.panelChannelOptions.Controls.Add(this.buttonZoomIn10);
            this.panelChannelOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChannelOptions.Location = new System.Drawing.Point(0, 0);
            this.panelChannelOptions.Name = "panelChannelOptions";
            this.panelChannelOptions.Size = new System.Drawing.Size(246, 554);
            this.panelChannelOptions.TabIndex = 0;
            // 
            // buttonZoomIn100
            // 
            this.buttonZoomIn100.Location = new System.Drawing.Point(16, 79);
            this.buttonZoomIn100.Name = "buttonZoomIn100";
            this.buttonZoomIn100.Size = new System.Drawing.Size(96, 57);
            this.buttonZoomIn100.TabIndex = 1;
            this.buttonZoomIn100.Text = "Zoom In x100";
            this.buttonZoomIn100.UseVisualStyleBackColor = true;
            this.buttonZoomIn100.Click += new System.EventHandler(this.ButtonZoomClick);
            // 
            // buttonZoomIn10
            // 
            this.buttonZoomIn10.Location = new System.Drawing.Point(16, 16);
            this.buttonZoomIn10.Name = "buttonZoomIn10";
            this.buttonZoomIn10.Size = new System.Drawing.Size(96, 57);
            this.buttonZoomIn10.TabIndex = 0;
            this.buttonZoomIn10.Text = "Zoom In x10";
            this.buttonZoomIn10.UseVisualStyleBackColor = true;
            this.buttonZoomIn10.Click += new System.EventHandler(this.ButtonZoomClick);
            // 
            // tabPagePropertyGrid
            // 
            this.tabPagePropertyGrid.Controls.Add(this.propertyGrid);
            this.tabPagePropertyGrid.Location = new System.Drawing.Point(4, 29);
            this.tabPagePropertyGrid.Name = "tabPagePropertyGrid";
            this.tabPagePropertyGrid.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePropertyGrid.Size = new System.Drawing.Size(246, 554);
            this.tabPagePropertyGrid.TabIndex = 1;
            this.tabPagePropertyGrid.Text = "Property Grid";
            this.tabPagePropertyGrid.UseVisualStyleBackColor = true;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(240, 548);
            this.propertyGrid.TabIndex = 0;
            // 
            // tabPageChannels
            // 
            this.tabPageChannels.Location = new System.Drawing.Point(4, 29);
            this.tabPageChannels.Name = "tabPageChannels";
            this.tabPageChannels.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageChannels.Size = new System.Drawing.Size(1122, 593);
            this.tabPageChannels.TabIndex = 1;
            this.tabPageChannels.Text = "Channels";
            this.tabPageChannels.UseVisualStyleBackColor = true;
            // 
            // tabPageADCs
            // 
            this.tabPageADCs.Location = new System.Drawing.Point(4, 29);
            this.tabPageADCs.Name = "tabPageADCs";
            this.tabPageADCs.Size = new System.Drawing.Size(1122, 593);
            this.tabPageADCs.TabIndex = 2;
            this.tabPageADCs.Text = "ADCs";
            this.tabPageADCs.UseVisualStyleBackColor = true;
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
            // buttonZoomOut100
            // 
            this.buttonZoomOut100.Location = new System.Drawing.Point(129, 79);
            this.buttonZoomOut100.Name = "buttonZoomOut100";
            this.buttonZoomOut100.Size = new System.Drawing.Size(96, 57);
            this.buttonZoomOut100.TabIndex = 3;
            this.buttonZoomOut100.Text = "Zoom Out x100";
            this.buttonZoomOut100.UseVisualStyleBackColor = true;
            this.buttonZoomOut100.Click += new System.EventHandler(this.ButtonZoomClick);
            // 
            // buttonZoomOut10
            // 
            this.buttonZoomOut10.Location = new System.Drawing.Point(129, 16);
            this.buttonZoomOut10.Name = "buttonZoomOut10";
            this.buttonZoomOut10.Size = new System.Drawing.Size(96, 57);
            this.buttonZoomOut10.TabIndex = 2;
            this.buttonZoomOut10.Text = "Zoom Out x10";
            this.buttonZoomOut10.UseVisualStyleBackColor = true;
            this.buttonZoomOut10.Click += new System.EventHandler(this.ButtonZoomClick);
            // 
            // buttonResetZoom
            // 
            this.buttonResetZoom.Location = new System.Drawing.Point(58, 153);
            this.buttonResetZoom.Name = "buttonResetZoom";
            this.buttonResetZoom.Size = new System.Drawing.Size(124, 37);
            this.buttonResetZoom.TabIndex = 4;
            this.buttonResetZoom.Text = "Reset Zoom";
            this.buttonResetZoom.UseVisualStyleBackColor = true;
            this.buttonResetZoom.Click += new System.EventHandler(this.ButtonClick);
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
            this.MainMenuStrip = this.menuStrip;
            this.Name = "NeuropixelsV1eDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NeuropixelsV1eDialog";
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
            this.tabPageChannelOptions.ResumeLayout(false);
            this.panelChannelOptions.ResumeLayout(false);
            this.tabPagePropertyGrid.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tabPageChannels;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonUpload;
        private System.Windows.Forms.Panel panelProbe;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOkay;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarUpload;
        private System.Windows.Forms.ToolStripStatusLabel probeSN;
        private System.Windows.Forms.ToolStripStatusLabel configSN;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.TabPage tabPageADCs;
        private System.Windows.Forms.LinkLabel linkLabelDocumentation;
        private System.Windows.Forms.TabControl tabControlOptions;
        private System.Windows.Forms.TabPage tabPageOptions;
        private System.Windows.Forms.Panel panelOptions;
        private System.Windows.Forms.TabPage tabPagePropertyGrid;
        private Bonsai.Design.PropertyGrid propertyGrid;
        private System.Windows.Forms.ComboBox comboBoxReference;
        private System.Windows.Forms.ComboBox comboBoxLfpGain;
        private System.Windows.Forms.ComboBox comboBoxApGain;
        private System.Windows.Forms.TextBox textBoxGainCalibrationFile;
        private System.Windows.Forms.Button buttonGainCalibrationFile;
        private System.Windows.Forms.Button buttonAdcCalibrationFile;
        private System.Windows.Forms.TextBox textBoxAdcCalibrationFile;
        private System.Windows.Forms.TabPage tabPageChannelOptions;
        private System.Windows.Forms.Panel panelChannelOptions;
        private System.Windows.Forms.CheckBox checkBoxSpikeFilter;
        private System.Windows.Forms.Button buttonZoomIn100;
        private System.Windows.Forms.Button buttonZoomIn10;
        private System.Windows.Forms.Button buttonZoomOut100;
        private System.Windows.Forms.Button buttonZoomOut10;
        private System.Windows.Forms.Button buttonResetZoom;
    }
}
