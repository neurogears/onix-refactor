using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace OpenEphys.Onix.Design
{
    public class NeuropixelsV1eEditor : UITypeEditor
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
                var editorDialog = new NeuropixelsV1eDialog();

                if (editorDialog.ShowDialog() == DialogResult.OK)
                {
                    return value;
                }
            }

            return base.EditValue(context, provider, value);
        }
    }
}
