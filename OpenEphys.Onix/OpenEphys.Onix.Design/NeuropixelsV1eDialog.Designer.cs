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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripSplitButtonUpload = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripProgressBarUpload = new System.Windows.Forms.ToolStripProgressBar();
            this.probeSN = new System.Windows.Forms.ToolStripStatusLabel();
            this.configSN = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageProbe = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panelChannelConfiguration = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabPageChannels = new System.Windows.Forms.TabPage();
            this.tabPageADCs = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOkay = new System.Windows.Forms.Button();
            this.linkLabelDocumentation = new System.Windows.Forms.LinkLabel();
            toolStripStatusLabelProbe = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabelConfigSN = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageProbe.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
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
            // menuStrip
            // 
            this.menuStrip.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
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
            this.splitContainer1.Location = new System.Drawing.Point(0, 36);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1130, 666);
            this.splitContainer1.SplitterDistance = 614;
            this.splitContainer1.TabIndex = 2;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageProbe);
            this.tabControl.Controls.Add(this.tabPageChannels);
            this.tabControl.Controls.Add(this.tabPageADCs);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1130, 614);
            this.tabControl.TabIndex = 0;
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
            this.splitContainer2.Panel1.Controls.Add(this.panelChannelConfiguration);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel2);
            this.splitContainer2.Size = new System.Drawing.Size(1116, 575);
            this.splitContainer2.SplitterDistance = 858;
            this.splitContainer2.TabIndex = 0;
            // 
            // panelChannelConfiguration
            // 
            this.panelChannelConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChannelConfiguration.Location = new System.Drawing.Point(0, 0);
            this.panelChannelConfiguration.Name = "panelChannelConfiguration";
            this.panelChannelConfiguration.Size = new System.Drawing.Size(858, 575);
            this.panelChannelConfiguration.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(254, 575);
            this.panel2.TabIndex = 0;
            // 
            // tabPageChannels
            // 
            this.tabPageChannels.Location = new System.Drawing.Point(4, 29);
            this.tabPageChannels.Name = "tabPageChannels";
            this.tabPageChannels.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageChannels.Size = new System.Drawing.Size(1020, 573);
            this.tabPageChannels.TabIndex = 1;
            this.tabPageChannels.Text = "Channels";
            this.tabPageChannels.UseVisualStyleBackColor = true;
            // 
            // tabPageADCs
            // 
            this.tabPageADCs.Location = new System.Drawing.Point(4, 29);
            this.tabPageADCs.Name = "tabPageADCs";
            this.tabPageADCs.Size = new System.Drawing.Size(1020, 573);
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
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(994, 7);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(124, 34);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOkay
            // 
            this.buttonOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOkay.Location = new System.Drawing.Point(861, 7);
            this.buttonOkay.Name = "buttonOkay";
            this.buttonOkay.Size = new System.Drawing.Size(124, 34);
            this.buttonOkay.TabIndex = 0;
            this.buttonOkay.Text = "OK";
            this.buttonOkay.UseVisualStyleBackColor = true;
            // 
            // linkLabelDocumentation
            // 
            this.linkLabelDocumentation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelDocumentation.AutoSize = true;
            this.linkLabelDocumentation.Location = new System.Drawing.Point(1005, 4);
            this.linkLabelDocumentation.Name = "linkLabelDocumentation";
            this.linkLabelDocumentation.Size = new System.Drawing.Size(118, 20);
            this.linkLabelDocumentation.TabIndex = 3;
            this.linkLabelDocumentation.TabStop = true;
            this.linkLabelDocumentation.Text = "Documentation";
            this.linkLabelDocumentation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
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
            this.Text = "NeuropixelsV1eDialog";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageProbe.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageProbe;
        private System.Windows.Forms.TabPage tabPageChannels;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonUpload;
        private System.Windows.Forms.Panel panelChannelConfiguration;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOkay;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarUpload;
        private System.Windows.Forms.ToolStripStatusLabel probeSN;
        private System.Windows.Forms.ToolStripStatusLabel configSN;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.TabPage tabPageADCs;
        private System.Windows.Forms.LinkLabel linkLabelDocumentation;
    }
}
