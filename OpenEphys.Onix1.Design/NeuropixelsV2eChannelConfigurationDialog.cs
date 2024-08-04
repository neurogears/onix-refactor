﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenEphys.ProbeInterface;
using ZedGraph;

namespace OpenEphys.Onix1.Design
{
    public partial class NeuropixelsV2eChannelConfigurationDialog : ChannelConfigurationDialog
    {
        internal event EventHandler OnZoom;
        internal event EventHandler OnFileLoad;

        internal readonly List<NeuropixelsV2QuadShankElectrode> Electrodes;
        internal readonly List<NeuropixelsV2QuadShankElectrode> ChannelMap;

        public NeuropixelsV2eChannelConfigurationDialog(NeuropixelsV2eProbeGroup probeGroup)
            : base(probeGroup)
        {
            zedGraphChannels.ZoomButtons = MouseButtons.None;
            zedGraphChannels.ZoomButtons2 = MouseButtons.None;

            zedGraphChannels.ZoomStepFraction = 0.5;

            ChannelMap = NeuropixelsV2eProbeGroup.ToChannelMap((NeuropixelsV2eProbeGroup)ChannelConfiguration);
            Electrodes = NeuropixelsV2eProbeGroup.ToElectrodes((NeuropixelsV2eProbeGroup)ChannelConfiguration);

            HighlightEnabledContacts();
            UpdateContactLabels();
            RefreshZedGraph();
        }

        internal override ProbeGroup DefaultChannelLayout()
        {
            return new NeuropixelsV2eProbeGroup();
        }

        internal override void LoadDefaultChannelLayout()
        {
            base.LoadDefaultChannelLayout();

            NeuropixelsV2eProbeGroup.UpdateElectrodes(Electrodes, (NeuropixelsV2eProbeGroup)ChannelConfiguration);
            NeuropixelsV2eProbeGroup.UpdateChannelMap(ChannelMap, (NeuropixelsV2eProbeGroup)ChannelConfiguration);

            OnFileOpenHandler();
        }

        internal override void OpenFile<T>()
        {
            base.OpenFile<NeuropixelsV2eProbeGroup>();

            NeuropixelsV2eProbeGroup.UpdateElectrodes(Electrodes, (NeuropixelsV2eProbeGroup)ChannelConfiguration);
            NeuropixelsV2eProbeGroup.UpdateChannelMap(ChannelMap, (NeuropixelsV2eProbeGroup)ChannelConfiguration);

            OnFileOpenHandler();
        }

        private void OnFileOpenHandler()
        {
            OnFileLoad?.Invoke(this, EventArgs.Empty);
        }

        internal override void ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            base.ZoomEvent(sender, oldState, newState);

            UpdateFontSize();
            DrawScale();
            RefreshZedGraph();

            OnZoomHandler();
        }

        private void OnZoomHandler()
        {
            OnZoom?.Invoke(this, EventArgs.Empty);
        }

        internal override void DrawScale()
        {
            const string ScalePointsTag = "scale_points";
            const string ScaleTextTag = "scale_text";

            zedGraphChannels.GraphPane.GraphObjList.RemoveAll(obj => obj is TextObj && obj.Tag is string tag && tag == ScaleTextTag);
            zedGraphChannels.GraphPane.CurveList.RemoveAll(curve => curve.Tag is string tag && tag == ScalePointsTag);

            const int MajorTickIncrement = 100;
            const int MajorTickLength = 10;
            const int MinorTickIncrement = 10;
            const int MinorTickLength = 5;

            if (ChannelConfiguration.Probes.ElementAt(0).SiUnits != ProbeSiUnits.um)
            {
                MessageBox.Show("Warning: Expected ProbeGroup units to be in microns, but it is in millimeters. Scale might not be accurate.");
            }

            var fontSize = CalculateFontSize();

            var zoomedOut = fontSize <= 2;

            fontSize = zoomedOut ? 8 : fontSize * 4;
            var majorTickOffset = MajorTickLength + GetXRange(zedGraphChannels) * 0.015;
            majorTickOffset = majorTickOffset > 50 ? 50 : majorTickOffset;

            var x = MaxX(zedGraphChannels.GraphPane.GraphObjList) + 100;
            var minY = MinY(zedGraphChannels.GraphPane.GraphObjList);
            var maxY = MaxY(zedGraphChannels.GraphPane.GraphObjList);

            zedGraphChannels.GraphPane.CurveList.Clear();

            PointPairList pointList = new();

            var countMajorTicks = 0;

            for (int i = (int)minY; i < maxY; i += MajorTickIncrement)
            {
                PointPair majorTickLocation = new(x + majorTickOffset, minY + MajorTickIncrement * countMajorTicks);

                pointList.Add(new PointPair(x, minY + MajorTickIncrement * countMajorTicks));
                pointList.Add(majorTickLocation);
                pointList.Add(new PointPair(x, minY + MajorTickIncrement * countMajorTicks));

                if (!zoomedOut || i % (5 * MajorTickIncrement) == 0)
                {
                    TextObj textObj = new($"{i} µm\n", majorTickLocation.X + 5, majorTickLocation.Y, CoordType.AxisXYScale, AlignH.Left, AlignV.Center)
                    {
                        Tag = ScaleTextTag,
                    };
                    textObj.FontSpec.Border.IsVisible = false;
                    textObj.FontSpec.Size = fontSize;
                    zedGraphChannels.GraphPane.GraphObjList.Add(textObj);
                }

                if (!zoomedOut)
                {
                    var countMinorTicks = 1;

                    for (int j = i + MinorTickIncrement; j < i + MajorTickIncrement && i + MinorTickIncrement * countMinorTicks < maxY; j += MinorTickIncrement)
                    {
                        pointList.Add(new PointPair(x, minY + MajorTickIncrement * countMajorTicks + MinorTickIncrement * countMinorTicks));
                        pointList.Add(new PointPair(x + MinorTickLength, minY + MajorTickIncrement * countMajorTicks + MinorTickIncrement * countMinorTicks));
                        pointList.Add(new PointPair(x, minY + MajorTickIncrement * countMajorTicks + MinorTickIncrement * countMinorTicks));

                        countMinorTicks++;
                    }
                }

                countMajorTicks++;
            }

            var curve = zedGraphChannels.GraphPane.AddCurve(ScalePointsTag, pointList, Color.Black, SymbolType.None);

            curve.Line.Width = 4;
            curve.Label.IsVisible = false;
            curve.Symbol.IsVisible = false;
        }

        private static double GetXRange(ZedGraphControl zedGraph)
        {
            return zedGraph.GraphPane.XAxis.Scale.Max - zedGraph.GraphPane.XAxis.Scale.Min;
        }

        internal override void HighlightEnabledContacts()
        {
            if (ChannelConfiguration == null || ChannelMap == null)
                return;

            var contactObjects = zedGraphChannels.GraphPane.GraphObjList.OfType<BoxObj>()
                                                                        .Where(c => c is not PolyObj);

            var enabledContacts = contactObjects.Where(c => c.Fill.Color == EnabledContactFill);

            foreach (var contact in enabledContacts)
            {
                contact.Fill.Color = DisabledContactFill;
            }

            var contactsToEnable = contactObjects.Where(c =>
                                                        {
                                                            var tag = c.Tag as ContactTag;
                                                            var channel = NeuropixelsV2QuadShankElectrode.GetChannelNumber(tag.ContactNumber);
                                                            return ChannelMap[channel].Index == tag.ContactNumber;
                                                        });

            foreach (var contact in contactsToEnable)
            {
                var tag = (ContactTag)contact.Tag;

                contact.Fill.Color = ReferenceContacts.Any(x => x == tag.ContactNumber) ? ReferenceContactFill : EnabledContactFill;
            }
        }

        internal override void UpdateContactLabels()
        {
            if (ChannelConfiguration == null)
                return;

            var indices = ChannelConfiguration.GetDeviceChannelIndices()
                                              .Select(ind => ind == -1).ToArray();

            var textObjs = zedGraphChannels.GraphPane.GraphObjList.OfType<TextObj>()
                                                                  .Where(t => t.Tag is ContactTag);

            var textObjsToUpdate = textObjs.Where(t => t.FontSpec.FontColor != DisabledContactTextColor);

            foreach (var textObj in textObjsToUpdate)
            {
                textObj.FontSpec.FontColor = DisabledContactTextColor;
            }

            textObjsToUpdate = textObjs.Where(c =>
                                           {
                                               var tag = c.Tag as ContactTag;
                                               var channel = NeuropixelsV2QuadShankElectrode.GetChannelNumber(tag.ContactNumber);
                                               return ChannelMap[channel].Index == tag.ContactNumber;
                                           });

            foreach (var textObj in textObjsToUpdate)
            {
                textObj.FontSpec.FontColor = EnabledContactTextColor;
            }
        }

        internal override string ContactString(int deviceChannelIndex, int index)
        {
            return index.ToString();
        }

        internal void EnableElectrodes(List<NeuropixelsV2QuadShankElectrode> electrodes)
        {
            ChannelMap.SelectElectrodes(electrodes);
        }
    }
}
