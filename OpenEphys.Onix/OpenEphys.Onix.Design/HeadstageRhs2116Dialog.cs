﻿using System;
using System.Linq;
using System.Windows.Forms;

namespace OpenEphys.Onix.Design
{
    public partial class HeadstageRhs2116Dialog : Form
    {
        public readonly Rhs2116StimulusSequenceDialog StimulusSequenceDialog;
        public readonly Rhs2116Dialog Rhs2116Dialog;

        public Rhs2116ProbeGroup ChannelConfiguration;

        public HeadstageRhs2116Dialog(Rhs2116ProbeGroup channelConfiguration, Rhs2116StimulusSequenceDual sequence,
            ConfigureRhs2116 rhs2116)
        {
            InitializeComponent();

            ChannelConfiguration = new Rhs2116ProbeGroup(channelConfiguration);

            StimulusSequenceDialog = new Rhs2116StimulusSequenceDialog(sequence, channelConfiguration)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill,
                Parent = this,
            };

            tabPageStimulusSequence.Controls.Add(StimulusSequenceDialog);
            AddMenuItemsFromDialog(StimulusSequenceDialog, "Stimulus Sequence");

            StimulusSequenceDialog.Show();

            Rhs2116Dialog = new Rhs2116Dialog(rhs2116)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill,
                Parent = this,
            };

            tabPageRhs2116A.Controls.Add(Rhs2116Dialog);
            Rhs2116Dialog.Show();
        }

        private void OnClickOK(object sender, EventArgs e)
        {
            if (Rhs2116StimulusSequenceDialog.CanCloseForm(StimulusSequenceDialog.Sequence, out DialogResult result))
            {
                DialogResult = result;
                Close();
            }
        }

        private void AddMenuItemsFromDialog(Form form, string menuName)
        {
            if (form != null)
            {
                var menuStrips = form.GetAllChildren()
                                     .OfType<MenuStrip>()
                                     .ToList();

                if (menuStrips != null && menuStrips.Count > 0)
                {
                    foreach (var menuStrip in menuStrips)
                    {
                        var toolStripMenuItem = new ToolStripMenuItem(menuName);

                        toolStripMenuItem.DropDownItems.AddRange(menuStrip.Items);

                        this.menuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuItem });
                    }
                }
            }
        }
    }
}