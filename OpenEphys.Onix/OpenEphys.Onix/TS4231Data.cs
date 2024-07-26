﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Bonsai;

namespace OpenEphys.Onix
{
    /// <summary>
    /// A class that produces a sequence of decoded optical signals produced by a pair of SteamVR V1 base stations.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This data stream class must be linked to an appropriate configuration, such as a <see cref="ConfigureTS4231"/>,
    /// in order to stream 3D position data.
    /// </para>
    /// <para>
    /// The data produced by this class contains individual base station pulse/sweep codes and timing information. These data provide
    /// rapid updates that constrain the possible position of a sensor and therefore can be combined with orientation information
    /// in a downstream predictive model (e.g. Kalman filter) for high-accuracy and robust position tracking. To produce naïve
    /// position estimates, use the <see cref="TS4231V1GeometricPositionData"/> operator instead of this one.
    /// </para>
    /// </remarks>
    public class TS4231Data : Source<TS4231DataFrame>
    {
        /// <inheritdoc cref = "SingleDeviceFactory.DeviceName"/>
        [TypeConverter(typeof(TS4231.NameConverter))]
        public string DeviceName { get; set; }

        /// <summary>
        /// Generates a sequence of <see cref="TS4231DataFrame"/> objects, each of which contains information on a single
        /// lighthouse optical sweep or pulse.
        /// </summary>
        /// <returns>A sequence of <see cref="TS4231DataFrame"/> objects.</returns>
        public override IObservable<TS4231DataFrame> Generate()
        {
            return DeviceManager.GetDevice(DeviceName).SelectMany(deviceInfo =>
            {
                var device = deviceInfo.GetDeviceContext(typeof(TS4231));
                return deviceInfo.Context.FrameReceived
                    .Where(frame => frame.DeviceAddress == device.Address)
                    .Select(frame => new TS4231DataFrame(frame));
            });
        }
    }
}
