﻿using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace OpenEphys.Onix1
{
    /// <summary>
    /// A class that contains 3D orientation data produced by a Bosch BNO055 9-axis inertial measurement unit (IMU).
    /// </summary>
    public class Bno055DataFrame : DataFrame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bno055DataFrame"/> class.
        /// </summary>
        /// <param name="frame">An ONI data frame containing BNO055 data.</param>
        public unsafe Bno055DataFrame(oni.Frame frame)
            : this(frame.Clock, (Bno055Payload*)frame.Data.ToPointer())
        {
        }

        internal unsafe Bno055DataFrame(ulong clock, Bno055Payload* payload)
            : this(clock, &payload->Data)
        {
            HubClock = payload->HubClock;
        }

        internal unsafe Bno055DataFrame(ulong clock, Bno055DataPayload* payload)
            : base(clock)
        {
            EulerAngle = new Vector3(
                x: Bno055.EulerAngleScale * payload->EulerAngle[0],
                y: Bno055.EulerAngleScale * payload->EulerAngle[1],
                z: Bno055.EulerAngleScale * payload->EulerAngle[2]);
            Quaternion = new Quaternion(
                w: Bno055.QuaternionScale * payload->Quaternion[0],
                x: Bno055.QuaternionScale * payload->Quaternion[1],
                y: Bno055.QuaternionScale * payload->Quaternion[2],
                z: Bno055.QuaternionScale * payload->Quaternion[3]);
            Acceleration = new Vector3(
                x: Bno055.AccelerationScale * payload->Acceleration[0],
                y: Bno055.AccelerationScale * payload->Acceleration[1],
                z: Bno055.AccelerationScale * payload->Acceleration[2]);
            Gravity = new Vector3(
                x: Bno055.AccelerationScale * payload->Gravity[0],
                y: Bno055.AccelerationScale * payload->Gravity[1],
                z: Bno055.AccelerationScale * payload->Gravity[2]);
            Temperature = payload->Temperature;
            Calibration = payload->Calibration;
        }

        /// <summary>
        /// Gets the 3D orientation in Euler angle format with units of degrees.
        /// </summary>
        /// <remark>
        /// The Tait-Bryan formalism is used:
        /// <list type="bullet">
        /// <item><description>Yaw: 0 to 360 degrees.</description></item>
        /// <item><description>Roll: -180 to 180 degrees</description></item>
        /// <item><description>Pitch: -90 to 90 degrees</description></item>
        /// </list>
        /// </remark>
        public Vector3 EulerAngle { get; }

        /// <summary>
        /// Gets the 3D orientation represented as a Quaternion.
        /// </summary>
        public Quaternion Quaternion { get; }

        /// <summary>
        /// Gets the linear acceleration vector in units of m / s^2.
        /// </summary>
        public Vector3 Acceleration { get; }

        /// <summary>
        /// Gets the gravity acceleration vector in units of m / s^2.
        /// </summary>
        public Vector3 Gravity { get; }

        /// <summary>
        /// Gets the chip temperature in Celsius.
        /// </summary>
        public int Temperature { get; }

        /// <summary>
        /// Gets MEMS subsystem and sensor fusion calibration status.
        /// </summary>
        public Bno055CalibrationFlags Calibration { get; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct Bno055Payload
    {
        public ulong HubClock;
        public Bno055DataPayload Data;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct Bno055DataPayload
    {
        public fixed short EulerAngle[3];
        public fixed short Quaternion[4];
        public fixed short Acceleration[3];
        public fixed short Gravity[3];
        public byte Temperature;
        public Bno055CalibrationFlags Calibration;
    }

    /// <summary>
    /// Specifies the MEMS subsystem and sensor fusion calibration status.
    /// </summary>
    [Flags]
    public enum Bno055CalibrationFlags : byte
    {
        /// <summary>
        /// Specifies that no sub-system is calibrated.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies all three sub-systems (gyroscope, accelerometer, and magnetometer) along with sensor fusion are calibrated.
        /// </summary>
        System = 0x3,
        /// <summary>
        /// Specifies that the gyroscope is calibrated.
        /// </summary>
        Gyroscope = 0xC,
        /// <summary>
        /// Specifies that the accelerometer is calibrated.
        /// </summary>
        Accelerometer = 0x30,
        /// <summary>
        /// Specifies that the magnetometer is calibrated.
        /// </summary>
        Magnetometer = 0xC0
    }
}
