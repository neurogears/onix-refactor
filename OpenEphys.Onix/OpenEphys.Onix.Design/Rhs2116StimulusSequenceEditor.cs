using System;
using System.ComponentModel;
using System.Windows.Forms;
using Bonsai.Design;

namespace OpenEphys.Onix.Design
{
    public class Rhs2116StimulusSequenceEditor : WorkflowComponentEditor
    {
        public override bool EditComponent(ITypeDescriptorContext context, object component, IServiceProvider provider, IWin32Window owner)
        {
            if (provider != null)
            {
                var editorState = (IWorkflowEditorState)provider.GetService(typeof(IWorkflowEditorState));
                if (editorState != null && !editorState.WorkflowRunning && component is ConfigureRhs2116Trigger configureNode)
                {
                    using var editorDialog = new Rhs2116StimulusSequenceDialog(configureNode.StimulusSequence, configureNode.ChannelConfiguration);

                    if (editorDialog.ShowDialog() == DialogResult.OK)
                    {
                        configureNode.StimulusSequence = editorDialog.Sequence;
                        configureNode.ChannelConfiguration = (Rhs2116ProbeGroup)editorDialog.ChannelConfiguration.GetProbeGroup();

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
