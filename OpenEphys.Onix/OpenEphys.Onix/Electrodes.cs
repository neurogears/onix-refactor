using System.Drawing;

namespace OpenEphys.Onix
{
    public abstract class Electrodes
    {
        /// <summary>
        /// Index of the electrode within the context of the probe
        /// </summary>
        public int ElectrodeNumber { get; internal set; }
        /// <summary>
        /// The shank this electrode belongs to
        /// </summary>
        public int Shank { get; internal set; }
        /// <summary>
        /// Index of the electrode within this shank
        /// </summary>
        public int ShankIndex { get; internal set; }
        /// <summary>
        /// The bank, or logical block of channels, this electrode belongs to
        /// </summary>
        public int Channel { get; internal set; }
        /// <summary>
        /// Location of the electrode in two-dimensional space
        /// </summary>
        public PointF Position { get; internal set; }

        public Electrodes()
        {
        }
    }
}
