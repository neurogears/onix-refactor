﻿using System.Runtime.InteropServices;
using OpenCV.Net;

namespace OpenEphys.Onix1
{
    /// <summary>
    /// Buffered analog data produced by the ONIX breakout board.
    /// </summary>
    public class BreakoutAnalogInputDataFrame : BufferedDataFrame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BreakoutAnalogInputDataFrame"/> class.
        /// </summary>
        /// <param name="clock">A buffered array of <see cref="DataFrame.Clock"/> values.</param>
        /// <param name="hubClock"> A buffered array of hub clock counter values.</param>
        /// <param name="analogData">A buffered array of multi-channel analog data.</param>
        public BreakoutAnalogInputDataFrame(ulong[] clock, ulong[] hubClock, Mat analogData)
            : base(clock, hubClock)
        {
            AnalogData = analogData;
        }

        /// <summary>
        /// Get the buffered analog data array.
        /// </summary>
        public Mat AnalogData { get; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct BreakoutAnalogInputPayload
    {
        public ulong HubClock;
        public fixed short AnalogData[BreakoutAnalogIO.ChannelCount];
    }
}
