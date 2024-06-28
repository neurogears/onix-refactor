using System;
using System.Windows.Forms;
using OpenEphys.ProbeInterface;

namespace OpenEphys.Onix.Design
{
    public partial class Rhs2116ChannelConfigurationDialog : ChannelConfigurationDialog
    {
        public event EventHandler OnSelect;

        public Rhs2116ChannelConfigurationDialog(Rhs2116ProbeGroup probeGroup)
            : base(probeGroup)
        {
            InitializeComponent();
            ChannelConfiguration = probeGroup;

            zedGraphChannels.ZoomButtons = MouseButtons.None;
            zedGraphChannels.ZoomButtons2 = MouseButtons.None;

            zedGraphChannels.ZoomStepFraction = 0.5;

            HighlightEnabledContacts();
            DrawContactLabels();
            RefreshZedGraph();
        }

        public override ProbeGroup DefaultChannelLayout()
        {
            return new Rhs2116ProbeGroup();
        }

        internal override void OpenFile<T>()
        {
            base.OpenFile<Rhs2116ProbeGroup>();
        }

        internal override float CalculateFontSize()
        {
            return 20;
        }

        internal override string ContactString(Contact contact)
        {
            return contact.DeviceId.ToString();
        }

        internal override void SelectedContactChanged()
        {
            OnSelectHandler();
        }

        private void OnSelectHandler()
        {
            OnSelect?.Invoke(this, EventArgs.Empty);
        }

        // HERE: Use this logic to highlight the invalid channels above

        //private void VisualizeSelectedChannels()
        //{
        //    bool showAllChannels = SelectedChannels.All(x => x == false);

        //    for (int i = 0; i < SelectedChannels.Length; i++)
        //    {
        //        EllipseObj circleObj = (EllipseObj)zedGraphChannels.GraphPane.GraphObjList[string.Format(ChannelConfigurationDialog.ContactStringFormat, i)];

        //        if (circleObj != null)
        //        {
        //            if (!Sequence.Stimuli[i].IsValid())
        //            {
        //                circleObj.Fill.Color = Color.Red;
        //            }
        //            else if (showAllChannels || !SelectedChannels[i])
        //            {
        //                circleObj.Fill.Color = Color.White;
        //            }
        //            else
        //            {
        //                circleObj.Fill.Color = Color.SlateGray;
        //            }
        //        }
        //    }

        //    zedGraphChannels.Refresh();
        //}
    }
}
