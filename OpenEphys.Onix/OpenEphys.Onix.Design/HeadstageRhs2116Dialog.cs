﻿using System;
using System.Linq;
using System.Windows.Forms;

namespace OpenEphys.Onix.Design
{
    public partial class HeadstageRhs2116Dialog : Form
    {
        public Rhs2116ProbeGroup ChannelConfiguration;

        public HeadstageRhs2116Dialog(Rhs2116ProbeGroup channelConfiguration, Rhs2116StimulusSequence sequence,
            ConfigureRhs2116 rhs2116A)
        {
            InitializeComponent();

            ChannelConfiguration = channelConfiguration;

            var channelConfigurationDialog = new ChannelConfigurationDialog(ChannelConfiguration)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill,
                Parent = this,
            };

            tabPageChannelConfiguration.Controls.Add(channelConfigurationDialog);

            channelConfigurationDialog.Show();

            var stimulusSequenceDialog = new Rhs2116StimulusSequenceDialog(sequence, channelConfiguration)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill,
                Parent = this,
                Tag = nameof(Rhs2116StimulusSequenceDialog)
            };

            tabPageStimulusSequence.Controls.Add(stimulusSequenceDialog);

            stimulusSequenceDialog.Show();

            var rhs2116ADialog = new Rhs2116Dialog(rhs2116A)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill,
                Parent = this,
                Tag = nameof(Rhs2116Dialog) + "A"
            };

            tabPageRhs2116A.Controls.Add(rhs2116ADialog);
            rhs2116ADialog.Show();
        }

        private void OnClickOK(object sender, EventArgs e)
        {
            var stimSequenceDialog = this.GetAllChildren()
                                         .OfType<Rhs2116StimulusSequenceDialog>()
                                         .First();

            ChannelConfiguration = this.GetAllChildren()
                                       .OfType<ChannelConfigurationDialog>()
                                       .Select(dialog => dialog.ChannelConfiguration)
                                       .First();

            if (Rhs2116StimulusSequenceDialog.CanCloseForm(stimSequenceDialog.Sequence, out DialogResult result))
            {
                DialogResult = result;
                Close();
            }
        }

        private void OnClickCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void TabPage_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabPageStimulusSequence)
            {
                var stimSequenceDialog = this.GetAllChildren()
                                             .OfType<Rhs2116StimulusSequenceDialog>()
                                             .First();

                var channelConfigurationDialog = this.GetAllChildren()
                                                     .OfType<ChannelConfigurationDialog>()
                                                     .First();

                if (!stimSequenceDialog.UpdateChannelConfiguration(channelConfigurationDialog.ChannelConfiguration))
                {
                    MessageBox.Show("Warning: Channel configuration was not updated for the stimulus sequence tab.");
                }
            }
        }
    }
}