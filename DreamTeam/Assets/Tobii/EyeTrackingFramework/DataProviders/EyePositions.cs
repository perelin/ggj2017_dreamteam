//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using System;
using UnityEngine;

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Value object for eye Position data, containing the eye positions of the left
    /// and right eyes and a timestamp.
    /// </summary>
    public sealed class EyePositions : ITimestamped
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EyePositions"/> class.
        /// </summary>
        /// <param name="leftEye">The eye position of the left eye, see <see cref="SingleEyePosition"/>.</param>
        /// <param name="leftEyeNormalized">The normalized eye position of the left eye, see <see cref="SingleEyePosition"/>.</param>
        /// <param name="rightEye">The eye position of the right eye, see <see cref="SingleEyePosition"/>.</param>
        /// <param name="rightEyeNormalized">The normalized eye position of the right eye, see <see cref="SingleEyePosition"/>.</param>
        /// <param name="sequentialId">The sequential ID of the eye position data.</param>
        /// <param name="timestamp">The timestamp of the frame the event was received, in seconds (<see cref="Time.time"/>).</param>
        public EyePositions(SingleEyePosition leftEye, SingleEyePosition leftEyeNormalized,
            SingleEyePosition rightEye, SingleEyePosition rightEyeNormalized, double sequentialId, float timestamp)
        {
            LeftEye = leftEye;
            LeftEyeNormalized = leftEyeNormalized;
            RightEye = rightEye;
            RightEyeNormalized = rightEyeNormalized;
            SequentialId = sequentialId;
            Timestamp = timestamp;
        }

        /// <summary>
        /// Gets a value representing an invalid EyePosition
        /// </summary>
        public static EyePositions Invalid
        {
            get
            {
                return new EyePositions(SingleEyePosition.Invalid, SingleEyePosition.Invalid,
                    SingleEyePosition.Invalid, SingleEyePosition.Invalid, double.NaN, float.NaN);
            }
        }

        /// <summary>
        /// Gets the Position of the left eye.
        /// </summary>
        public SingleEyePosition LeftEye { get; private set; }

        /// <summary>
        /// Gets the normalized Position of the left eye.
        /// </summary>
        public SingleEyePosition LeftEyeNormalized { get; private set; }

        /// <summary>
        /// Gets the Position of the right eye.
        /// </summary>
        public SingleEyePosition RightEye { get; private set; }

        /// <summary>
        /// Gets the normalized Position of the right eye.
        /// </summary>
        public SingleEyePosition RightEyeNormalized { get; private set; }

        /// <summary>
        /// Gets the sequential ID for the data point.
        /// <para>
        /// This ID can be used to compare if a data point is newer than
        /// another. A higher value means a newer value.
        /// </para>
        /// </summary>
        public double SequentialId { get; private set; }

        /// <summary>
        /// Gets the <see cref="Time.time"/> timestamp for the data point
        /// in seconds.
        /// <remarks>
        /// Note that more than one data point can be received in the same frame
        /// and have the same timestamp.
        /// </remarks>
        /// </summary>
        public float Timestamp { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the data point is valid or not.
        /// <remarks>
        /// This indicates if the point was created with valid data. To check
        /// if a point is stale, use <see cref="Timestamp"/> instead.
        /// </remarks>
        /// </summary>
        public bool IsValid
        {
            get { return !double.IsNaN(SequentialId); }
        }
    }

    /// <summary>
    /// Position of an eye in 3D space.
    /// <para>
    /// The Position is taken relative to the center of the screen where the eye tracker is mounted.
    /// </para>
    /// </summary>
    public sealed class SingleEyePosition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleEyePosition"/> class.
        /// </summary>
        /// <param name="isValid">Flag indicating if the eye Position is valid. (Sometimes only a single eye is tracked.)</param>
        /// <param name="x">X coordinate of the eye Position, in millimeters.</param>
        /// <param name="y">Y coordinate of the eye Position, in millimeters.</param>
        /// <param name="z">Z coordinate of the eye Position, in millimeters.</param>
        public SingleEyePosition(bool isValid, float x, float y, float z)
        {
            IsValid = isValid;
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Gets a value representing an invalid eye Position.
        /// </summary>
        public static SingleEyePosition Invalid
        {
            get
            {
                return new SingleEyePosition(false, float.NaN, float.NaN, float.NaN);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the eye Position is valid. (Sometimes only a single eye is tracked.)
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Gets the X coordinate of the eye Position, in millimeters.
        /// </summary>
        public float X { get; private set; }

        /// <summary>
        /// Gets the Y coordinate of the eye Position, in millimeters.
        /// </summary>
        public float Y { get; private set; }

        /// <summary>
        /// Gets the Z coordinate of the eye Position, in millimeters.
        /// </summary>
        public float Z { get; private set; }

        /// <summary>
        /// Gets the X, Y, Z coordinates of the eye Position as a <see cref="UnityEngine.Vector3"/>.
        /// </summary>
        public Vector3 Vector3 { get { return new Vector3(X, Y, Z); } }
    }
}
