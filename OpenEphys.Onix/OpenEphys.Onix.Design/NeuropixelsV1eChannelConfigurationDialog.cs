using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenEphys.ProbeInterface;
using ZedGraph;

namespace OpenEphys.Onix.Design
{
    public partial class NeuropixelsV1eChannelConfigurationDialog : ChannelConfigurationDialog
    {
        public event EventHandler OnZoom;

        public NeuropixelsV1eChannelConfigurationDialog(NeuropixelsV1eProbeGroup probeGroup)
            : base(probeGroup)
        {
            InitializeComponent();

            zedGraphChannels.IsEnableHPan = false;
            zedGraphChannels.ZoomButtons = MouseButtons.None;
            zedGraphChannels.ZoomButtons2 = MouseButtons.None;

            zedGraphChannels.ZoomStepFraction = 0.5;

            ReferenceContacts = new List<int> { 191, 575, 959 };

            DrawChannels();
            RefreshZedGraph();
        }

        public override ProbeGroup DefaultChannelLayout()
        {
            return new NeuropixelsV1eProbeGroup();
        }

        public override void ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            base.ZoomEvent(sender, oldState, newState);

            UpdateFontSize();
            RefreshZedGraph();

            OnZoomHandler();
        }

        private void OnZoomHandler()
        {
            if (OnZoom != null)
            {
                OnZoom(this, EventArgs.Empty);
            }
        }

        public override void DrawScale()
        {
            const int MajorTickIncrement = 100;
            const int MajorTickLength = 10;
            const int MinorTickIncrement = 10;
            const int MinorTickLength = 5;

            var fontSize = CalculateFontSize();

            var x = MaxX(zedGraphChannels.GraphPane.GraphObjList) + 10;
            var minY = MinY(zedGraphChannels.GraphPane.GraphObjList);
            var maxY = MaxY(zedGraphChannels.GraphPane.GraphObjList);

            zedGraphChannels.GraphPane.CurveList.Clear();

            PointPairList pointList = new();

            var countMajorTicks = 0;

            for (int i = (int)minY; i < maxY; i += MajorTickIncrement)
            {
                pointList.Add(new PointPair(x, minY + MajorTickIncrement * countMajorTicks));
                PointPair majorTickLocation = new(x + MajorTickLength, minY + MajorTickIncrement * countMajorTicks);
                pointList.Add(majorTickLocation);
                pointList.Add(new PointPair(x, minY + MajorTickIncrement * countMajorTicks));

                TextObj textObj = new($"{i} µm", majorTickLocation.X + 10, majorTickLocation.Y);
                textObj.FontSpec.Border.IsVisible = false;
                textObj.FontSpec.Size = fontSize;
                zedGraphChannels.GraphPane.GraphObjList.Add(textObj);

                var countMinorTicks = 1;

                for (int j = i + MinorTickIncrement; j < i + MajorTickIncrement && i + MinorTickIncrement * countMinorTicks < maxY; j += MinorTickIncrement)
                {
                    pointList.Add(new PointPair(x, minY + MajorTickIncrement * countMajorTicks + MinorTickIncrement * countMinorTicks));
                    pointList.Add(new PointPair(x + MinorTickLength, minY + MajorTickIncrement * countMajorTicks + MinorTickIncrement * countMinorTicks));
                    pointList.Add(new PointPair(x, minY + MajorTickIncrement * countMajorTicks + MinorTickIncrement * countMinorTicks));

                    countMinorTicks++;
                }

                countMajorTicks++;
            }

            AddPointsToCurve(pointList);
        }

        private void AddPointsToCurve(PointPairList pointList)
        {
            var curve = zedGraphChannels.GraphPane.AddCurve("", pointList, Color.Black, SymbolType.None);

            curve.Line.Width = 4;
            curve.Label.IsVisible = false;
            curve.Symbol.IsVisible = false;
        }
    }
}
