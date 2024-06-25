using System;
using System.ComponentModel;
using System.Windows.Forms;
using Bonsai.Design;

namespace OpenEphys.Onix.Design
{
    public class NeuropixelsV1eEditor : WorkflowComponentEditor
    {
        public override bool EditComponent(ITypeDescriptorContext context, object component, IServiceProvider provider, IWin32Window owner)
        {
            if (provider != null)
            {
                var editorState = (IWorkflowEditorState)provider.GetService(typeof(IWorkflowEditorState));  
                if (editorState != null && !editorState.WorkflowRunning && component is ConfigureNeuropixelsV1e configureNeuropixelsV1e)
                {
                    using var editorDialog = new NeuropixelsV1eDialog(configureNeuropixelsV1e);

                    if (editorDialog.ShowDialog() == DialogResult.OK)
                    {
                        configureNeuropixelsV1e.Enable = editorDialog.ConfigureNode.Enable;
                        configureNeuropixelsV1e.GainCalibrationFile = editorDialog.ConfigureNode.GainCalibrationFile;
                        configureNeuropixelsV1e.AdcCalibrationFile = editorDialog.ConfigureNode.AdcCalibrationFile;
                        configureNeuropixelsV1e.ProbeConfiguration = editorDialog.ConfigureNode.ProbeConfiguration;

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
