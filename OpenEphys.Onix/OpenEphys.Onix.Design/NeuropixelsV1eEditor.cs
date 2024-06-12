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
                        configureNeuropixelsV1e.Enable = editorDialog.configureNeuropixelsV1e.Enable;
                        configureNeuropixelsV1e.SpikeAmplifierGain = editorDialog.configureNeuropixelsV1e.SpikeAmplifierGain;
                        configureNeuropixelsV1e.LfpAmplifierGain = editorDialog.configureNeuropixelsV1e.LfpAmplifierGain;
                        configureNeuropixelsV1e.Reference = editorDialog.configureNeuropixelsV1e.Reference;
                        configureNeuropixelsV1e.SpikeFilter = editorDialog.configureNeuropixelsV1e.SpikeFilter;
                        configureNeuropixelsV1e.GainCalibrationFile = editorDialog.configureNeuropixelsV1e.GainCalibrationFile;
                        configureNeuropixelsV1e.AdcCalibrationFile = editorDialog.configureNeuropixelsV1e.AdcCalibrationFile;
                        configureNeuropixelsV1e.ChannelConfiguration = editorDialog.configureNeuropixelsV1e.ChannelConfiguration;

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
