using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenEphys.ProbeInterface;
using ZedGraph;

namespace OpenEphys.Onix.Design
{
    public partial class NeuropixelsV1eChannelConfigurationDialog : ChannelConfigurationDialog
    {
        public event EventHandler OnZoom;
        public event EventHandler OnFileLoad;

        public readonly List<NeuropixelsV1eElectrode> Electrodes;
        public readonly List<NeuropixelsV1eElectrode> ChannelMap;

        public NeuropixelsV1eChannelConfigurationDialog(NeuropixelsV1eProbeGroup probeGroup)
            : base(probeGroup)
        {
            zedGraphChannels.IsEnableHPan = false;
            zedGraphChannels.ZoomButtons = MouseButtons.None;
            zedGraphChannels.ZoomButtons2 = MouseButtons.None;

            zedGraphChannels.ZoomStepFraction = 0.5;

            ReferenceContacts = new List<int> { 191, 575, 959 };

            ChannelMap = DesignHelper.ToChannelMap((NeuropixelsV1eProbeGroup)ChannelConfiguration);
            Electrodes = DesignHelper.ToElectrodes((NeuropixelsV1eProbeGroup)ChannelConfiguration);

            HighlightEnabledContacts();
            DrawContactLabels();
            RefreshZedGraph();
        }

        public override ProbeGroup DefaultChannelLayout()
        {
            return new NeuropixelsV1eProbeGroup();
        }

        internal override void LoadDefaultChannelLayout()
        {
            base.LoadDefaultChannelLayout();

            DesignHelper.UpdateElectrodes(Electrodes, (NeuropixelsV1eProbeGroup)ChannelConfiguration);
            DesignHelper.UpdateChannelMap(ChannelMap, (NeuropixelsV1eProbeGroup)ChannelConfiguration);

            OnFileOpenHandler();
        }

        internal override void OpenFile<T>()
        {
            base.OpenFile<NeuropixelsV1eProbeGroup>();

            DesignHelper.UpdateChannelMap(ChannelMap, (NeuropixelsV1eProbeGroup)ChannelConfiguration);

            OnFileOpenHandler();
        }

        private void OnFileOpenHandler()
        {
            OnFileLoad?.Invoke(this, EventArgs.Empty);
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
            OnZoom?.Invoke(this, EventArgs.Empty);
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

        public override void HighlightEnabledContacts()
        {
            if (Electrodes == null || Electrodes.Count == 0) 
                return;

            foreach(var e in Electrodes)
            {
                var tag = string.Format(ContactStringFormat, 0, e.ElectrodeNumber);

                var fillColor = ChannelMap[e.Channel].ElectrodeNumber == e.ElectrodeNumber ?
                                (ReferenceContacts.Any(x => x == e.ElectrodeNumber) ? ReferenceContactFill : EnabledContactFill) : 
                                DisabledContactFill;

                if (zedGraphChannels.GraphPane.GraphObjList[tag] is BoxObj graphObj)
                {
                    graphObj.Fill.Color = fillColor;
                }
                else
                {
                    throw new NullReferenceException($"Tag {tag} is not found in the graph object list");
                }
            }
        }

        public override void DrawContactLabels()
        {
            if (Electrodes == null || Electrodes.Count == 0)
                return;

            var fontSize = CalculateFontSize();

            foreach (var e in Electrodes)
            {
                string id = ChannelMap[e.Channel].ElectrodeNumber == e.ElectrodeNumber ? e.ElectrodeNumber.ToString(): "Off";

                TextObj textObj = new(id, e.Position.X, e.Position.Y)
                {
                    ZOrder = ZOrder.A_InFront,
                    Tag = string.Format(TextStringFormat, 0, e.ElectrodeNumber)
                };

                SetTextObj(textObj, fontSize);

                zedGraphChannels.GraphPane.GraphObjList.Add(textObj);
            }
        }

        public void EnableElectrodes(List<NeuropixelsV1eElectrode> electrodes)
        {
            ChannelMap.SelectElectrodes(electrodes);
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
