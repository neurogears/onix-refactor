using System;
using System.ComponentModel;
using System.Windows.Forms;
using Bonsai.Design;

namespace OpenEphys.Onix.Design
{
    public class Rhs2116Editor : WorkflowComponentEditor
    {
        public override bool EditComponent(ITypeDescriptorContext context, object component, IServiceProvider provider, IWin32Window owner)
        {
            if (provider != null)
            {
                var editorState = (IWorkflowEditorState)provider.GetService(typeof(IWorkflowEditorState));
                if (editorState != null && !editorState.WorkflowRunning && component is ConfigureRhs2116 configureNode)
                {
                    using var editorDialog = new Rhs2116Dialog(configureNode);

                    if (editorDialog.ShowDialog() == DialogResult.OK)
                    {
                        configureNode.AnalogHighCutoff = editorDialog.ConfigureNode.AnalogHighCutoff;
                        configureNode.AnalogLowCutoff = editorDialog.ConfigureNode.AnalogLowCutoff;
                        configureNode.AnalogLowCutoffRecovery = editorDialog.ConfigureNode.AnalogLowCutoffRecovery;
                        configureNode.DspCutoff = editorDialog.ConfigureNode.DspCutoff;
                        configureNode.Enable = editorDialog.ConfigureNode.Enable;
                        configureNode.RespectExternalActiveStim = editorDialog.ConfigureNode.RespectExternalActiveStim;

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
