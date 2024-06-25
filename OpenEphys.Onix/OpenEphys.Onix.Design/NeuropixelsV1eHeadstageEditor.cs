using Bonsai.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System;

namespace OpenEphys.Onix.Design
{
    public class NeuropixelsV1eHeadstageEditor : WorkflowComponentEditor
    {
        public override bool EditComponent(ITypeDescriptorContext context, object component, IServiceProvider provider, IWin32Window owner)
        {
            if (provider != null)
            {
                var editorState = (IWorkflowEditorState)provider.GetService(typeof(IWorkflowEditorState));
                if (editorState != null && !editorState.WorkflowRunning && component is ConfigureNeuropixelsV1eHeadstage configureHeadstage)
                {
                    using var editorDialog = new NeuropixelsV1eHeadstageDialog(configureHeadstage.NeuropixelsV1, configureHeadstage.Bno055);

                    if (editorDialog.ShowDialog() == DialogResult.OK)
                    {
                        configureHeadstage.Bno055.Enable = editorDialog.ConfigureBno055.ConfigureNode.Enable;

                        configureHeadstage.NeuropixelsV1.AdcCalibrationFile = editorDialog.ConfigureNeuropixelsV1e.ConfigureNode.AdcCalibrationFile;
                        configureHeadstage.NeuropixelsV1.GainCalibrationFile = editorDialog.ConfigureNeuropixelsV1e.ConfigureNode.GainCalibrationFile;
                        configureHeadstage.NeuropixelsV1.Enable = editorDialog.ConfigureNeuropixelsV1e.ConfigureNode.Enable;
                        configureHeadstage.NeuropixelsV1.EnableLed = editorDialog.ConfigureNeuropixelsV1e.ConfigureNode.EnableLed;
                        configureHeadstage.NeuropixelsV1.ProbeConfiguration = editorDialog.ConfigureNeuropixelsV1e.ConfigureNode.ProbeConfiguration;

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
