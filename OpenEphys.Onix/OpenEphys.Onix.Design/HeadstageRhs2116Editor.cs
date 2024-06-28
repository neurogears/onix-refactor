using System;
using System.ComponentModel;
using System.Windows.Forms;
using Bonsai.Design;

namespace OpenEphys.Onix.Design
{
    public class HeadstageRhs2116Editor : WorkflowComponentEditor
    {
        public override bool EditComponent(ITypeDescriptorContext context, object component, IServiceProvider provider, IWin32Window owner)
        {
            if (provider != null)
            {
                var editorState = (IWorkflowEditorState)provider.GetService(typeof(IWorkflowEditorState));
                if (editorState != null && !editorState.WorkflowRunning && component is ConfigureHeadstageRhs2116 configureNode)
                {
                    using var editorDialog = new HeadstageRhs2116Dialog(configureNode.StimulusTrigger.ChannelConfiguration, 
                        configureNode.StimulusTrigger.StimulusSequence, configureNode.Rhs2116A);

                    if (editorDialog.ShowDialog() == DialogResult.OK)
                    {
                        configureNode.StimulusTrigger.StimulusSequence = editorDialog.StimulusSequenceDialog.Sequence;
                        configureNode.ChannelConfiguration = (Rhs2116ProbeGroup)editorDialog.StimulusSequenceDialog.ChannelConfiguration.GetProbeGroup();
                        configureNode.Rhs2116A = editorDialog.Rhs2116Dialog.ConfigureNode;

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
