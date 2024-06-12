using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;
using System;
using OpenEphys.ProbeInterface;

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
        /// Standardize the format of the string used for creating tags, so that
        /// they can be searched for effectively
        /// </summary>
        public static readonly string ContactStringFormat = "Contact_{0}";
        /// <summary>
        /// Standardize the format of the string used for creating tags, so that
        /// they can be searched for effectively
        /// </summary>
        public static readonly string TextStringFormat = "TextContact_{0}";

        /// <summary>
        /// Local variable that holds the channel configuration in memory until the user presses Okay
        /// </summary>
        public ProbeGroup ChannelConfiguration;

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
                ChannelConfiguration = DefaultChannelLayout();
            }

            InitializeZedGraphChannels(zedGraphChannels);
            DrawChannels(zedGraphChannels, ChannelConfiguration);
        }

        /// <summary>
        /// Return the default channel layout of the current device, which fully instatiates the probe group object
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

        /// <summary>
        /// After every zoom event, check that the axis liimits are equal to maintain the equal
        /// aspect ratio of the graph, ensuring that all contacts do not look smashed or stretched.
        /// </summary>
        /// <param name="sender">Incoming <see cref="ZedGraphControl"/> object</param>
        /// <param name="oldState"><code>null</code></param>
        /// <param name="newState">New state, of type <see cref="ZoomState"/></param>
        public virtual void ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            if (newState.Type == ZoomState.StateType.Zoom)
            {
                var rangeX = sender.GraphPane.XAxis.Scale.Max - sender.GraphPane.XAxis.Scale.Min;
                var rangeY = sender.GraphPane.YAxis.Scale.Max - sender.GraphPane.YAxis.Scale.Min;

                if (rangeX > rangeY)
                {
                    var diff = rangeX - rangeY;

                    sender.GraphPane.YAxis.Scale.Max += diff / 2;
                    sender.GraphPane.YAxis.Scale.Min -= diff / 2;
                }
                else
                {
                    var diff = rangeY - rangeX;

                    sender.GraphPane.XAxis.Scale.Max += diff / 2;
                    sender.GraphPane.XAxis.Scale.Min -= diff / 2;
                }
            }

            sender.AxisChange();
            sender.Refresh();
        }

        private void FormShown(object sender, EventArgs e)
        {
            if (!TopLevel)
            {
                splitContainer1.Panel2Collapsed = true;
                splitContainer1.Panel2.Hide();

                menuStrip.Visible = false;
            }

            UpdateFontSize(zedGraphChannels);
        }

        private void LoadDefaultChannelLayout()
        {
            ChannelConfiguration = DefaultChannelLayout();
        }

        private void OpenFile()
        {
            using OpenFileDialog ofd = new();

            ofd.Filter = "Probe Interface Files (*.json)|*.json";
            ofd.FilterIndex = 1;
            ofd.Multiselect = false;
            ofd.Title = "Choose probe interface file";

            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName))
            {
                var channelConfiguration = DesignHelper.DeserializeString<ProbeGroup>(File.ReadAllText(ofd.FileName));

                if (channelConfiguration == null || channelConfiguration.NumContacts != 32)
                {
                    MessageBox.Show("Error opening the JSON file. Incorrect number of contacts.");
                    return;
                }
                else
                {
                    ChannelConfiguration = channelConfiguration;
                }
            }
        }

        /// <summary>
        /// Given a <see cref="ZedGraphControl"/> and a <see cref="ProbeGroup"/>-inherited class, draw all available contacts
        /// in the probe contour, with the device channel indices plotted to indicate the contact number.
        /// </summary>
        /// <param name="zedGraph">A <see cref="ZedGraphControl"/> holding the current graph to plot in</param>
        /// <param name="probeGroup">Fully instantiated <see cref="ProbeGroup"/> object, implemented for a specific device</param>
        public static void DrawChannels(ZedGraphControl zedGraph, ProbeGroup probeGroup)
        {
            if (probeGroup == null)
                return;

            zedGraph.GraphPane.GraphObjList.Clear();

            for (int i = 0; i < probeGroup.Probes.Count(); i++)
            {
                PointD[] planarContours = ConvertFloatArrayToPointD(probeGroup.Probes.ElementAt(i).ProbePlanarContour);
                PolyObj contour = new(planarContours, Color.Black, Color.White)
                {
                    ZOrder = ZOrder.E_BehindCurves
                };

                zedGraph.GraphPane.GraphObjList.Add(contour);

                for (int j = 0; j < probeGroup.Probes.ElementAt(i).ContactPositions.Length; j++)
                {
                    Contact contact = probeGroup.Probes.ElementAt(i).GetContact(j);

                    if (contact.Shape.Equals(ContactShape.Circle))
                    {
                        var size = contact.ShapeParams.Radius.Value * 2;

                        EllipseObj contactObj = new(contact.PosX - size / 2, contact.PosY + size / 2,
                            size, size, Color.DarkGray, Color.WhiteSmoke)
                        {
                            ZOrder = ZOrder.B_BehindLegend,
                            Tag = string.Format(ContactStringFormat, contact.DeviceId)
                        };

                        zedGraph.GraphPane.GraphObjList.Add(contactObj);
                    }
                    else if (contact.Shape.Equals(ContactShape.Square))
                    {
                        var size = contact.ShapeParams.Width.Value;

                        BoxObj contactObj = new(contact.PosX - size / 2, contact.PosY + size / 2,
                            size, size, Color.DarkGray, Color.WhiteSmoke)
                        {
                            ZOrder = ZOrder.B_BehindLegend,
                            Tag = string.Format(ContactStringFormat, contact.DeviceId)
                        };

                        zedGraph.GraphPane.GraphObjList.Add(contactObj);
                    }
                    else
                    {
                        MessageBox.Show("Contact shapes other than 'circle' and 'square' not implemented yet.");
                        return;
                    }

                    TextObj textObj = new(contact.DeviceId.ToString(), contact.PosX, contact.PosY)
                    {
                        ZOrder = ZOrder.A_InFront,
                        Tag = string.Format(TextStringFormat, contact.DeviceId)
                    };
                    textObj.FontSpec.IsBold = true;
                    textObj.FontSpec.Border.IsVisible = false;
                    textObj.FontSpec.Fill.IsVisible = false;

                    zedGraph.GraphPane.GraphObjList.Add(textObj);
                }
            }

            zedGraph.Refresh();
        }

        internal static void UpdateFontSize(ZedGraphControl zedGraph)
        {
            var fontSize = CalculateFontSize(zedGraph);

            foreach (var obj in zedGraph.GraphPane.GraphObjList)
            {
                if (obj == null) continue;

                if (obj is TextObj textObj)
                {
                    textObj.FontSpec.Size = fontSize;
                }
            }

            zedGraph.Refresh();
        }

        internal static float CalculateFontSize(ZedGraphControl zedGraph)
        {
            float rangeY = (float)(zedGraph.GraphPane.YAxis.Scale.Max - zedGraph.GraphPane.YAxis.Scale.Min);

            float contactSize = ContactSize(zedGraph);

            var fontSize = 300f * contactSize / rangeY;

            fontSize = fontSize < 1f ? 1f : fontSize;
            fontSize = fontSize > 100f ? 200f : fontSize;

            return fontSize;
        }

        internal static float ContactSize(ZedGraphControl zedGraph)
        {
            var obj = zedGraph.GraphPane.GraphObjList
                        .OfType<BoxObj>()
                        .Where(obj => obj is not PolyObj)
                        .FirstOrDefault();

            if (obj != null && obj != default(BoxObj))
            {
                return (float)obj.Location.Width;
            }

            return 1f;
        }

        /// <summary>
        /// After a resize event (such as changing the window size), readjust the size of the control to 
        /// ensure an equal aspect ratio for axes.
        /// </summary>
        /// <param name="zedGraph">A <see cref="ZedGraphControl"/> containing the current control to resize</param>
        public static void ResizeAxes(ZedGraphControl zedGraph)
        {
            SetEqualAspectRatio(zedGraph);

            RectangleF axisRect = zedGraph.GraphPane.Rect;

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

            zedGraph.GraphPane.Rect = axisRect;
            zedGraph.Size = new Size((int)axisRect.Width, (int)axisRect.Height);
            zedGraph.Location = new Point((int)axisRect.X, (int)axisRect.Y);
        }

        internal static void SetEqualAspectRatio(ZedGraphControl zedGraph)
        {
            if (zedGraph.GraphPane.GraphObjList.Count == 0)
                return;

            var minX = zedGraph.GraphPane.GraphObjList.Min<GraphObj, double>(obj =>
            {
                if (obj is PolyObj polyObj)
                {
                    return polyObj.Points.Min(p => p.X);
                }

                return double.MaxValue;
            });

            var minY = zedGraph.GraphPane.GraphObjList.Min<GraphObj, double>(obj =>
            {
                if (obj is PolyObj polyObj)
                {
                    return polyObj.Points.Min(p => p.Y);
                }

                return double.MaxValue;
            });

            var maxX = zedGraph.GraphPane.GraphObjList.Max<GraphObj, double>(obj =>
            {
                if (obj is PolyObj polyObj)
                {
                    return polyObj.Points.Max(p => p.X);
                }

                return double.MinValue;
            });

            var maxY = zedGraph.GraphPane.GraphObjList.Max<GraphObj, double>(obj =>
            {
                if (obj is PolyObj polyObj)
                {
                    return polyObj.Points.Max(p => p.Y);
                }

                return double.MinValue;
            });

            var min = Math.Min(minX, minY);
            var max = Math.Max(maxX, maxY);

            var margin = (max - min) * 0.05;

            var rangeX = maxX - minX;
            var rangeY = maxY - minY;

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

            zedGraph.GraphPane.XAxis.Scale.Min = minX;
            zedGraph.GraphPane.XAxis.Scale.Max = maxX;

            zedGraph.GraphPane.YAxis.Scale.Min = minY;
            zedGraph.GraphPane.YAxis.Scale.Max = maxY;
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
        /// <param name="zedGraph">A <see cref="ZedGraphControl"/> containing the current control to initialize</param>
        public static void InitializeZedGraphChannels(ZedGraphControl zedGraph)
        {
            zedGraph.GraphPane.Title.IsVisible = false;
            zedGraph.GraphPane.TitleGap = 0;
            zedGraph.GraphPane.Border.IsVisible = false;
            zedGraph.GraphPane.Border.Width = 0;
            zedGraph.GraphPane.Chart.Border.IsVisible = false;
            zedGraph.GraphPane.Margin.All = -1;
            zedGraph.GraphPane.IsFontsScaled = true;
            zedGraph.BorderStyle = BorderStyle.None;

            zedGraph.GraphPane.XAxis.IsVisible = false;
            zedGraph.GraphPane.XAxis.IsAxisSegmentVisible = false;
            zedGraph.GraphPane.XAxis.Scale.MaxAuto = true;
            zedGraph.GraphPane.XAxis.Scale.MinAuto = true;

            zedGraph.GraphPane.YAxis.IsVisible = false;
            zedGraph.GraphPane.YAxis.IsAxisSegmentVisible = false;
            zedGraph.GraphPane.YAxis.Scale.MaxAuto = true;
            zedGraph.GraphPane.YAxis.Scale.MinAuto = true;
        }

        private void MenuItemSaveFile_Click(object sender, EventArgs e)
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

        private void ZedGraphChannels_Resize(object sender, EventArgs e)
        {
            ResizeAxes(zedGraphChannels);
            zedGraphChannels.AxisChange();
            zedGraphChannels.Refresh();
            UpdateFontSize(zedGraphChannels);
        }

        private void MenuItemOpenFile_Click(object sender, EventArgs e)
        {
            OpenFile();
            DrawChannels(zedGraphChannels, ChannelConfiguration);
        }

        private void LoadDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDefaultChannelLayout();
            DrawChannels(zedGraphChannels, ChannelConfiguration);
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (TopLevel)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        public void ManualZoom(double zoomFactor)
        {
            var center = new PointF(zedGraphChannels.GraphPane.Rect.Left + zedGraphChannels.GraphPane.Rect.Width / 2,
                                    zedGraphChannels.GraphPane.Rect.Top  + zedGraphChannels.GraphPane.Rect.Height / 2);

            zedGraphChannels.ZoomPane(zedGraphChannels.GraphPane, 1 / zoomFactor, center, true);

            UpdateFontSize(zedGraphChannels);
        }

        public void ResetZoom()
        {
            SetEqualAspectRatio(zedGraphChannels);
            UpdateFontSize(zedGraphChannels);
            zedGraphChannels.Refresh();
        }
    }
}
