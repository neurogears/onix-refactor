﻿using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;
using System;
using OpenEphys.ProbeInterface;
using System.Collections.Generic;

namespace OpenEphys.Onix.Design
{
    /// <summary>
    /// Simple dialog window that serves as the base class for all Channel Configuration windows.
    /// Within, there are a number of useful methods for initializing, resizing, and drawing channels.
    /// Each device must implement their own ChannelConfigurationDialog.
    /// </summary>
    public abstract partial class ChannelConfigurationDialog : Form
    {
        /// <summary>
        /// Local variable that holds the channel configuration in memory until the user presses Okay
        /// </summary>
        internal ProbeGroup ChannelConfiguration;

        internal List<int> ReferenceContacts = new();

        internal readonly bool[] SelectedContacts = null;

        /// <summary>
        /// Constructs the dialog window using the given probe group, and plots all contacts after loading.
        /// </summary>
        /// <param name="probeGroup">Channel configuration given as a <see cref="ProbeGroup"/></param>
        public ChannelConfigurationDialog(ProbeGroup probeGroup)
        {
            InitializeComponent();
            Shown += FormShown;

            if (probeGroup == null)
            {
                LoadDefaultChannelLayout();
            }
            else
            {
                ChannelConfiguration = probeGroup;
            }

            SelectedContacts = new bool[ChannelConfiguration.NumberOfContacts];

            zedGraphChannels.MouseDownEvent += MouseDownEvent;
            zedGraphChannels.MouseMoveEvent += MouseMoveEvent;
            zedGraphChannels.MouseUpEvent += MouseUpEvent;

            InitializeZedGraphChannels();
            DrawProbeGroup();
            RefreshZedGraph();
        }

        /// <summary>
        /// Return the default channel layout of the current device, which fully instantiates the probe group object
        /// </summary>
        /// <example>
        /// Using a class that inherits from ProbeGroup, the general usage would
        /// be the default constructor which should fully initialize a <see cref="ProbeGroup"/> object.
        /// For example, if there was <code>SampleDeviceProbeGroup : ProbeGroup</code>, the body of this 
        /// function could be:
        /// <code>
        /// return new SampleDeviceProbeGroup();
        /// </code>
        /// </example>
        /// <returns>Returns an object that inherits from <see cref="ProbeGroup"/></returns>
        public abstract ProbeGroup DefaultChannelLayout();

        internal virtual void LoadDefaultChannelLayout()
        {
            ChannelConfiguration = DefaultChannelLayout();
        }

        /// <summary>
        /// After every zoom event, check that the axis limits are equal to maintain the equal
        /// aspect ratio of the graph, ensuring that all contacts do not look smashed or stretched.
        /// </summary>
        /// <param name="sender">Incoming <see cref="ZedGraphControl"/> object</param>
        /// <param name="oldState"><code>null</code></param>
        /// <param name="newState">New state, of type <see cref="ZoomState"/></param>
        public virtual void ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            if (newState.Type == ZoomState.StateType.Zoom || newState.Type == ZoomState.StateType.WheelZoom)
            {
                SetEqualAxisLimits(sender);
            }
        }

        private void SetEqualAxisLimits(ZedGraphControl zedGraphControl)
        {
            var rangeX = zedGraphControl.GraphPane.XAxis.Scale.Max - zedGraphControl.GraphPane.XAxis.Scale.Min;
            var rangeY = zedGraphControl.GraphPane.YAxis.Scale.Max - zedGraphControl.GraphPane.YAxis.Scale.Min;

            if (rangeX > rangeY)
            {
                var diff = rangeX - rangeY;

                zedGraphControl.GraphPane.YAxis.Scale.Max += diff / 2;
                zedGraphControl.GraphPane.YAxis.Scale.Min -= diff / 2;
            }
            else if (rangeX < rangeY)
            {
                var diff = rangeY - rangeX;

                zedGraphControl.GraphPane.XAxis.Scale.Max += diff / 2;
                zedGraphControl.GraphPane.XAxis.Scale.Min -= diff / 2;
            }
        }

        private void FormShown(object sender, EventArgs e)
        {
            if (!TopLevel)
            {
                DisconnectResizeEventHandler();

                splitContainer1.Panel2Collapsed = true;
                splitContainer1.Panel2.Hide();

                menuStrip.Visible = false;

                ConnectResizeEventHandler();
                ZedGraphChannels_Resize(null, null);
            }
            else
            {
                UpdateFontSize();
                zedGraphChannels.Refresh();
            }
        }

        internal virtual void OpenFile<T>() where T : ProbeGroup
        {
            var newConfiguration = OpenAndParseConfigurationFile<T>();

            if (newConfiguration == null)
            {
                return;
            }

            if (ChannelConfiguration.NumberOfContacts == newConfiguration.NumberOfContacts)
            {
                newConfiguration.Validate();

                ChannelConfiguration = newConfiguration;
                DrawProbeGroup();
                RefreshZedGraph();
            }
            else
            {
                throw new InvalidOperationException($"Number of contacts does not match; expected {ChannelConfiguration.NumberOfContacts} contacts" +
                    $", but found {newConfiguration.NumberOfContacts} contacts");
            }
        }

        internal T OpenAndParseConfigurationFile<T>() where T : ProbeGroup
        {
            using OpenFileDialog ofd = new();

            ofd.Filter = "Probe Interface Files (*.json)|*.json";
            ofd.FilterIndex = 1;
            ofd.Multiselect = false;
            ofd.Title = "Choose probe interface file";

            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName))
            {
                var newConfiguration = DesignHelper.DeserializeString<T>(File.ReadAllText(ofd.FileName));

                return newConfiguration ?? throw new InvalidOperationException($"Unable to open {ofd.FileName}");
            }

            return null;
        }

        internal void DrawProbeGroup()
        {
            zedGraphChannels.GraphPane.GraphObjList.Clear();

            DrawProbeContour();
            SetEqualAspectRatio();
            DrawContacts();
            HighlightEnabledContacts();
            HighlightSelectedContacts();
            DrawContactLabels();
            DrawScale();
        }

        internal void DrawProbeContour()
        {
            if (ChannelConfiguration == null)
                return;

            foreach (var probe in ChannelConfiguration.Probes)
            {
                PointD[] planarContours = ConvertFloatArrayToPointD(probe.ProbePlanarContour);
                PolyObj contour = new(planarContours, Color.Black, Color.White)
                {
                    ZOrder = ZOrder.C_BehindChartBorder
                };

                zedGraphChannels.GraphPane.GraphObjList.Add(contour);
            }
        }

        internal void SetEqualAspectRatio()
        {
            if (zedGraphChannels.GraphPane.GraphObjList.Count == 0)
                return;

            var minX = MinX(zedGraphChannels.GraphPane.GraphObjList);
            var minY = MinY(zedGraphChannels.GraphPane.GraphObjList);
            var maxX = MaxX(zedGraphChannels.GraphPane.GraphObjList);
            var maxY = MaxY(zedGraphChannels.GraphPane.GraphObjList);

            var rangeX = maxX - minX;
            var rangeY = maxY - minY;

            if (rangeX == rangeY) return;

            if (rangeY < rangeX)
            {
                var diff = (rangeX - rangeY) / 2;
                minY -= diff;
                maxY += diff;
            }
            else
            {
                var diff = (rangeY - rangeX) / 2;
                minX -= diff;
                maxX += diff;
            }

            var margin = Math.Max(rangeX, rangeY) * 0.05;

            zedGraphChannels.GraphPane.XAxis.Scale.Min = minX - margin;
            zedGraphChannels.GraphPane.XAxis.Scale.Max = maxX + margin;

            zedGraphChannels.GraphPane.YAxis.Scale.Min = minY - margin;
            zedGraphChannels.GraphPane.YAxis.Scale.Max = maxY + margin;
        }

        internal void DrawContacts()
        {
            if (ChannelConfiguration == null)
                return;

            for (int probeNumber = 0; probeNumber < ChannelConfiguration.Probes.Count(); probeNumber++)
            {
                var probe = ChannelConfiguration.Probes.ElementAt(probeNumber);

                const int borderWidth = 4;

                for (int j = 0; j < probe.ContactPositions.Length; j++)
                {
                    Contact contact = probe.GetContact(j);

                    if (contact.Shape.Equals(ContactShape.Circle))
                    {
                        var size = contact.ShapeParams.Radius.Value * 2;

                        EllipseObj contactObj = new(contact.PosX - size / 2, contact.PosY + size / 2, size, size)
                        {
                            ZOrder = ZOrder.B_BehindLegend,
                            Tag = ContactTag.GetContactString(probeNumber, contact.Index)
                        };

                        contactObj.Border.Width = borderWidth;

                        zedGraphChannels.GraphPane.GraphObjList.Add(contactObj);
                    }
                    else if (contact.Shape.Equals(ContactShape.Square))
                    {
                        var size = contact.ShapeParams.Width.Value;

                        BoxObj contactObj = new(contact.PosX - size / 2, contact.PosY + size / 2, size, size)
                        {
                            ZOrder = ZOrder.B_BehindLegend,
                            Tag = ContactTag.GetContactString(probeNumber, contact.Index)
                        };

                        contactObj.Border.Width = borderWidth;

                        zedGraphChannels.GraphPane.GraphObjList.Add(contactObj);
                    }
                    else
                    {
                        MessageBox.Show("Contact shapes other than 'circle' and 'square' not implemented yet.");
                        return;
                    }
                }
            }
        }

        internal readonly Color DisabledContactFill = Color.DarkGray;
        internal readonly Color EnabledContactFill = Color.LightYellow;
        internal readonly Color ReferenceContactFill = Color.Black;

        internal virtual void HighlightEnabledContacts()
        {
            if (ChannelConfiguration == null)
                return;

            for (int probeNumber = 0; probeNumber < ChannelConfiguration.Probes.Count(); probeNumber++)
            {
                var probe = ChannelConfiguration.Probes.ElementAt(probeNumber);

                for (int j = 0; j < probe.ContactPositions.Length; j++)
                {
                    Contact contact = probe.GetContact(j);

                    var tag = ContactTag.GetContactString(probeNumber, contact.Index);

                    if (zedGraphChannels.GraphPane.GraphObjList[tag] is BoxObj graphObj)
                    {
                        graphObj.Fill.Color = contact.DeviceId == -1 ?
                                              DisabledContactFill :
                                              (ReferenceContacts.Any(x => x == contact.Index) ? ReferenceContactFill : EnabledContactFill);
                    }
                    else
                    {
                        throw new NullReferenceException($"Tag {tag} is not found in the graph object list");
                    }
                }
            }
        }

        internal readonly Color DeselectedContactBorder = Color.LightGray;
        internal readonly Color SelectedContactBorder = Color.YellowGreen;

        internal virtual void HighlightSelectedContacts()
        {
            if (ChannelConfiguration == null)
                return;

            for (int probeNumber = 0; probeNumber < ChannelConfiguration.Probes.Count(); probeNumber++)
            {
                var probe = ChannelConfiguration.Probes.ElementAt(probeNumber);
                var probeOffset = GetProbeIndexOffset(probeNumber);

                for (int j = 0; j < probe.ContactPositions.Length; j++)
                {
                    var tag = ContactTag.GetContactString(probeNumber, probe.GetContact(j).Index);

                    if (zedGraphChannels.GraphPane.GraphObjList[tag] is BoxObj graphObj)
                    {
                        graphObj.Border.Color = SelectedContacts[probeOffset + j] ?
                                                SelectedContactBorder :
                                                DeselectedContactBorder; ;
                    }
                    else
                    {
                        throw new NullReferenceException($"Tag {tag} is not found in the graph object list");
                    }
                }
            }
        }

        internal virtual void DrawContactLabels()
        {
            if (ChannelConfiguration == null)
                return;

            var fontSize = CalculateFontSize();

            for (int probeNumber = 0; probeNumber < ChannelConfiguration.Probes.Count(); probeNumber++)
            {
                var probe = ChannelConfiguration.Probes.ElementAt(probeNumber);

                for (int j = 0; j < probe.ContactPositions.Length; j++)
                {
                    Contact contact = probe.GetContact(j);
                    bool inactiveContact = contact.DeviceId == -1;

                    string id = inactiveContact ? "Off" : ContactString(contact);

                    TextObj textObj = new(id, contact.PosX, contact.PosY)
                    {
                        ZOrder = ZOrder.A_InFront,
                        Tag = ContactTag.GetTextString(probeNumber, contact.Index)
                    };

                    textObj.FontSpec.IsBold = true;
                    textObj.FontSpec.Border.IsVisible = false;
                    textObj.FontSpec.Fill.IsVisible = false;
                    textObj.FontSpec.Size = fontSize;

                    zedGraphChannels.GraphPane.GraphObjList.Add(textObj);
                }
            }
        }

        internal virtual string ContactString(Contact contact)
        {
            return contact.Index.ToString();
        }

        internal virtual void DrawScale()
        {
        }

        internal void UpdateFontSize()
        {
            var fontSize = CalculateFontSize();

            foreach (var obj in zedGraphChannels.GraphPane.GraphObjList)
            {
                if (obj == null) continue;

                if (obj is TextObj textObj)
                {
                    textObj.FontSpec.Size = fontSize;
                }
            }
        }

        internal virtual float CalculateFontSize()
        {
            float rangeY = (float)(zedGraphChannels.GraphPane.YAxis.Scale.Max - zedGraphChannels.GraphPane.YAxis.Scale.Min);

            float contactSize = ContactSize();

            var fontSize = 300f * contactSize / rangeY;

            fontSize = fontSize < 1f ? 1f : fontSize;
            fontSize = fontSize > 100f ? 100f : fontSize;

            return fontSize;
        }

        internal float ContactSize()
        {
            var obj = zedGraphChannels.GraphPane.GraphObjList
                        .OfType<BoxObj>()
                        .Where(obj => obj is not PolyObj)
                        .FirstOrDefault();

            if (obj != null && obj != default(BoxObj))
            {
                return (float)obj.Location.Width;
            }

            return 1f;
        }

        private static double MinX(GraphObjList graphObjs)
        {
            return graphObjs.OfType<PolyObj>()
                            .Min(obj => { return obj.Points.Min(p => p.X); });
        }

        private static double MinY(GraphObjList graphObjs)
        {
            return graphObjs.OfType<PolyObj>()
                            .Min(obj => { return obj.Points.Min(p => p.Y); });
        }

        private static double MaxX(GraphObjList graphObjs)
        {
            return graphObjs.OfType<PolyObj>()
                            .Max(obj => { return obj.Points.Max(p => p.X); });
        }

        private static double MaxY(GraphObjList graphObjs)
        {
            return graphObjs.OfType<PolyObj>()
                            .Max(obj => { return obj.Points.Max(p => p.Y); });
        }

        /// <summary>
        /// Converts a two-dimensional <see cref="float"/> array into an array of <see cref="PointD"/>
        /// objects. Assumes that the float array is ordered so that the first index of each pair is 
        /// the X position, and the second index is the Y position.
        /// </summary>
        /// <param name="floats">Two-dimensional array of <see cref="float"/> values</param>
        /// <returns></returns>
        public static PointD[] ConvertFloatArrayToPointD(float[][] floats)
        {
            PointD[] pointD = new PointD[floats.Length];

            for (int i = 0; i < floats.Length; i++)
            {
                pointD[i] = new PointD(floats[i][0], floats[i][1]);
            }

            return pointD;
        }

        /// <summary>
        /// Initialize the given <see cref="ZedGraphControl"/> so that almost everything other than the 
        /// axis itself is hidden, reducing visual clutter before plotting contacts
        /// </summary>
        public void InitializeZedGraphChannels()
        {
            zedGraphChannels.GraphPane.Title.IsVisible = false;
            zedGraphChannels.GraphPane.TitleGap = 0;
            zedGraphChannels.GraphPane.Border.IsVisible = false;
            zedGraphChannels.GraphPane.Border.Width = 0;
            zedGraphChannels.GraphPane.Chart.Border.IsVisible = false;
            zedGraphChannels.GraphPane.Margin.All = -1;
            zedGraphChannels.GraphPane.IsFontsScaled = true;
            zedGraphChannels.BorderStyle = BorderStyle.None;

            zedGraphChannels.GraphPane.XAxis.IsVisible = false;
            zedGraphChannels.GraphPane.XAxis.IsAxisSegmentVisible = false;
            zedGraphChannels.GraphPane.XAxis.Scale.MaxAuto = true;
            zedGraphChannels.GraphPane.XAxis.Scale.MinAuto = true;

            zedGraphChannels.GraphPane.YAxis.IsVisible = false;
            zedGraphChannels.GraphPane.YAxis.IsAxisSegmentVisible = false;
            zedGraphChannels.GraphPane.YAxis.Scale.MaxAuto = true;
            zedGraphChannels.GraphPane.YAxis.Scale.MinAuto = true;
        }

        private void MenuItemSaveFile(object sender, EventArgs e)
        {
            using SaveFileDialog sfd = new();
            sfd.Filter = "Probe Interface Files (*.json)|*.json";
            sfd.FilterIndex = 1;
            sfd.Title = "Choose where to save the probe interface file";
            sfd.OverwritePrompt = true;
            sfd.ValidateNames = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                DesignHelper.SerializeObject(ChannelConfiguration, sfd.FileName);
            }
        }

        internal void ConnectResizeEventHandler()
        {
            DisconnectResizeEventHandler();
            zedGraphChannels.Resize += ZedGraphChannels_Resize;
        }

        internal void DisconnectResizeEventHandler()
        {
            zedGraphChannels.Resize -= ZedGraphChannels_Resize;
        }

        private void ZedGraphChannels_Resize(object sender, EventArgs e)
        {
            if (zedGraphChannels.Size.Width == zedGraphChannels.Size.Height &&
                zedGraphChannels.Size.Height == zedGraphChannels.GraphPane.Rect.Height &&
                zedGraphChannels.Location.X == zedGraphChannels.GraphPane.Rect.X)
            {
                if (zedGraphChannels.GraphPane.Chart.Rect != zedGraphChannels.GraphPane.Rect)
                {
                    zedGraphChannels.GraphPane.Chart.Rect = zedGraphChannels.GraphPane.Rect;
                }

                return;
            }

            ResizeAxes();
            UpdateControlSizeBasedOnAxisSize();
            UpdateFontSize();
            zedGraphChannels.AxisChange();
            zedGraphChannels.Refresh();
        }

        /// <summary>
        /// After a resize event (such as changing the window size), readjust the size of the control to 
        /// ensure an equal aspect ratio for axes.
        /// </summary>
        public void ResizeAxes()
        {
            SetEqualAspectRatio();

            RectangleF axisRect = zedGraphChannels.GraphPane.Rect;

            if (axisRect.Width > axisRect.Height)
            {
                axisRect.X += (axisRect.Width - axisRect.Height) / 2;
                axisRect.Width = axisRect.Height;
            }
            else if (axisRect.Height > axisRect.Width)
            {
                axisRect.Y += (axisRect.Height - axisRect.Width) / 2;
                axisRect.Height = axisRect.Width;
            }
            else
            {
                zedGraphChannels.GraphPane.Chart.Rect = axisRect;
                return;
            }

            zedGraphChannels.GraphPane.Rect = axisRect;
            zedGraphChannels.GraphPane.Chart.Rect = axisRect;
        }

        private void UpdateControlSizeBasedOnAxisSize()
        {
            RectangleF axisRect = zedGraphChannels.GraphPane.Rect;

            zedGraphChannels.Size = new Size((int)axisRect.Width, (int)axisRect.Height);
            zedGraphChannels.Location = new Point((int)axisRect.X, (int)axisRect.Y);
        }

        private void MenuItemOpenFile(object sender, EventArgs e)
        {
            OpenFile<ProbeGroup>();
            DrawProbeGroup();
            RefreshZedGraph();
        }

        private void MenuItemLoadDefaultConfig(object sender, EventArgs e)
        {
            LoadDefaultChannelLayout();
            DrawProbeGroup();
            RefreshZedGraph();
        }

        private void ButtonOK(object sender, EventArgs e)
        {
            if (TopLevel)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        internal void ManualZoom(double zoomFactor)
        {
            var center = new PointF(zedGraphChannels.GraphPane.Rect.Left + zedGraphChannels.GraphPane.Rect.Width / 2,
                                    zedGraphChannels.GraphPane.Rect.Top  + zedGraphChannels.GraphPane.Rect.Height / 2);

            zedGraphChannels.ZoomPane(zedGraphChannels.GraphPane, 1 / zoomFactor, center, true);

            UpdateFontSize();
        }

        internal void ResetZoom()
        {
            SetEqualAspectRatio();
            UpdateFontSize();
        }

        /// <summary>
        /// Shifts the whole ZedGraph to the given relative position, where 0.0 is the very bottom of the horizontal 
        /// space, and 1.0 is the very top. Note that this accounts for a buffer on the top and bottom, so giving a 
        /// value of 0.0 would have the minimum value of Y axis equal to the bottom of the graph, and keep the range 
        /// the same. Similarly, a value of 1.0 would set the maximum value of the Y axis to the top of the graph, 
        /// and keep the range the same.
        /// </summary>
        /// <param name="relativePosition">A float value defining the percentage of the graph to move to vertically</param>
        public void MoveToVerticalPosition(float relativePosition)
        {
            if (relativePosition < 0.0 || relativePosition > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(relativePosition));
            }

            var currentRange = zedGraphChannels.GraphPane.YAxis.Scale.Max - zedGraphChannels.GraphPane.YAxis.Scale.Min;

            var minY = MinY(zedGraphChannels.GraphPane.GraphObjList);
            var maxY = MaxY(zedGraphChannels.GraphPane.GraphObjList);

            var newMinY = (maxY - minY - currentRange) * relativePosition;

            zedGraphChannels.GraphPane.YAxis.Scale.Min = newMinY;
            zedGraphChannels.GraphPane.YAxis.Scale.Max = newMinY + currentRange;
        }

        internal float GetRelativeVerticalPosition()
        {
            var minY = MinY(zedGraphChannels.GraphPane.GraphObjList);
            var maxY = MaxY(zedGraphChannels.GraphPane.GraphObjList);

            var currentRange = zedGraphChannels.GraphPane.YAxis.Scale.Max - zedGraphChannels.GraphPane.YAxis.Scale.Min;

            if (zedGraphChannels.GraphPane.YAxis.Scale.Min <= minY)
                return 0.0f;
            else if (zedGraphChannels.GraphPane.YAxis.Scale.Min >= maxY - currentRange)
                return 1.0f;
            else
            {
                return (float)((zedGraphChannels.GraphPane.YAxis.Scale.Min - minY) / (maxY - minY - currentRange));
            }
        }

        internal void RefreshZedGraph()
        {
            zedGraphChannels.AxisChange();
            zedGraphChannels.Refresh();
        }

        PointD clickStart = new(0.0, 0.0);

        private bool MouseDownEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                clickStart = TransformPixelsToCoordinates(e.Location, sender.GraphPane);
            }

            return false;
        }

        const string SelectionAreaTag = "Selection";

        private bool MouseMoveEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                sender.Cursor = Cursors.Cross;

                if (clickStart.X == default && clickStart.Y == default)
                    return false;

                BoxObj oldArea = (BoxObj)sender.GraphPane.GraphObjList[SelectionAreaTag];
                if (oldArea != null)
                {
                    sender.GraphPane.GraphObjList.Remove(oldArea);
                }

                var mouseLocation = TransformPixelsToCoordinates(e.Location, sender.GraphPane);

                BoxObj selectionArea = new(
                    mouseLocation.X < clickStart.X ? mouseLocation.X : clickStart.X,
                    mouseLocation.Y > clickStart.Y ? mouseLocation.Y : clickStart.Y,
                    Math.Abs(mouseLocation.X - clickStart.X),
                    Math.Abs(mouseLocation.Y - clickStart.Y));
                selectionArea.Border.Color = Color.DarkSlateGray;
                selectionArea.Fill.IsVisible = false;
                selectionArea.ZOrder = ZOrder.A_InFront;
                selectionArea.Tag = SelectionAreaTag;

                sender.GraphPane.GraphObjList.Add(selectionArea);
                sender.Refresh();

                return true;
            }
            else if (e.Button == MouseButtons.None)
            {
                sender.Cursor = Cursors.Arrow;

                return true;
            }

            return false;
        }

        private bool MouseUpEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            sender.Cursor = Cursors.Arrow;

            if (e.Button == MouseButtons.Left)
            {
                if (sender.GraphPane.GraphObjList[SelectionAreaTag] is BoxObj selectionArea && selectionArea != null && ChannelConfiguration != null)
                {
                    RectangleF rect = selectionArea.Location.Rect;

                    if (!rect.IsEmpty)
                    {
                        for (int i = 0; i < ChannelConfiguration.Probes.Count(); i++)
                        {
                            var probe = ChannelConfiguration.Probes.ElementAt(i);

                            for (int j = 0; j < probe.NumberOfContacts; j++)
                            {
                                if (sender.GraphPane.GraphObjList[ContactTag.GetContactString(i, j)] is BoxObj contact && contact != null)
                                {
                                    if (Contains(rect, contact.Location))
                                    {
                                        SetSelectedContact(contact.Tag as string, true);
                                    }
                                }
                            }
                        }
                    }

                    sender.GraphPane.GraphObjList.Remove(selectionArea);
                    clickStart.X = default;
                    clickStart.Y = default;
                }
                else
                {
                    PointF mouseClick = new(e.X, e.Y);

                    if (zedGraphChannels.GraphPane.FindNearestObject(mouseClick, CreateGraphics(), out object nearestObject, out int _))
                    {
                        if (nearestObject is TextObj textObj)
                        {
                            ToggleSelectedContact(textObj.Tag as string);
                        }
                        else if (nearestObject is BoxObj boxObj)
                        {
                            ToggleSelectedContact(boxObj.Tag as string);
                        }
                    }
                    else
                    {
                        SetAllSelections(false);
                    }
                }

                HighlightSelectedContacts();
                SelectedContactChanged();
                RefreshZedGraph();

                return true;
            }

            return false;
        }

        private void ToggleSelectedContact(string tag)
        {
            SetSelectedContact(tag, !GetContactStatus(tag));
        }

        private void SetSelectedContact(string tag, bool status)
        {
            ContactTag parsedTag = new(tag);

            var index = GetProbeIndexOffset(parsedTag.ProbeNumber) + parsedTag.ContactNumber;

            SetSelectedContact(index, status);
        }

        private void SetSelectedContact(int index, bool status)
        {
            SelectedContacts[index] = status;
        }

        internal virtual void SelectedContactChanged()
        {
        }

        internal void SetAllSelections(bool newStatus)
        {
            for (int i = 0; i < SelectedContacts.Length; i++)
            {
                SetSelectedContact(i, newStatus);
            }
        }

        private bool GetContactStatus(string tag)
        {
            ContactTag parsedTag = new(tag);

            var index = GetProbeIndexOffset(parsedTag.ProbeNumber) + parsedTag.ContactNumber;

            return SelectedContacts[index];
        }

        private static PointD TransformPixelsToCoordinates(Point pixels, GraphPane graphPane)
        {
            graphPane.ReverseTransform(pixels, out double x, out double y);

            return new PointD(x, y);
        }

        private bool Contains(RectangleF rect, Location location)
        {
            if (!rect.IsEmpty)
            {
                if (location != null)
                {
                    var x = location.X + location.Width / 2;
                    var y = location.Y - location.Height / 2;

                    if (x >= rect.X && x <= rect.X + rect.Width && y <= rect.Y && y >= rect.Y - rect.Height)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal int GetProbeIndexOffset(int currentProbeIndex)
        {
            int offset = 0;

            for (int i = currentProbeIndex - 1; i >= 0; i--)
            {
                offset += ChannelConfiguration.Probes.ElementAt(i).NumberOfContacts;
            }

            return offset;
        }
    }
}
