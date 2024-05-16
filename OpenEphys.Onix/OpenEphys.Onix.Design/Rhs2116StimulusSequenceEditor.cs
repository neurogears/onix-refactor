﻿using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace OpenEphys.Onix.Design
{
    public class Rhs2116StimulusSequenceEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (editorService != null)
            {
                var editorDialog = new Rhs2116StimulusSequenceDialog(value as Rhs2116StimulusSequence);
                
                if (editorDialog.ShowDialog() == DialogResult.OK)
                {
                    if (editorDialog.Sequence.Valid)
                        return editorDialog.Sequence;

                    MessageBox.Show("Warning: Sequence was not valid; all settings were discarded.");
                }
            }

            return base.EditValue(context, provider, value);
        }
    }
}