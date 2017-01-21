//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using UnityEngine;

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Holds a gaze point with a timestamp and converts to either Screen space, Viewport, or GUI space coordinates.
    /// </summary>
    public sealed class GazePoint : ITimestamped
    {
        private readonly Vector2 _gazePointInUnityScreenSpace;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="unityScreenSpacePoint">Gaze point in unity screen coordinates.</param>
        /// <param name="sequentialId">The sequential ID of the gaze point.</param>
        /// <param name="timestamp">The timestamp of the frame the gaze point was received, in seconds <see cref="Time.time"/>.</param>
        public GazePoint(Vector2 unityScreenSpacePoint, double sequentialId, float timestamp)
        {
            _gazePointInUnityScreenSpace = unityScreenSpacePoint;
            SequentialId = sequentialId;
            Timestamp = timestamp;
        }

        /// <summary>
        /// Gets a value representing an invalid gaze point.
        /// </summary>
        public static GazePoint Invalid
        {
            get
            {
                return new GazePoint(new Vector2(float.NaN, float.NaN), -1.0, -1.0f);
            }
        }

        /// <summary>
        /// Gets the gaze point in (Unity) screen space pixels.
        /// <para>
        /// The bottom-left of the screen/camera is (0, 0); the right-top is (pixelWidth, pixelHeight).
        /// </para>
        /// </summary>
        public Vector2 Screen
        {
            get
            {
                return _gazePointInUnityScreenSpace;
            }
        }

        /// <summary>
        /// Gets the gaze point in the viewport coordinate system.
        /// <para>
        /// The bottom-left of the screen/camera is (0, 0); the top-right is (1, 1).
        /// </para>
        /// </summary>
        public Vector2 Viewport
        {
            get
            {
                var point = Screen;
                point.x /= UnityEngine.Screen.width;
                point.y /= UnityEngine.Screen.height;
                return point;
            }
        }

        /// <summary>
        /// Gets the gaze point in GUI space pixels.
        /// <para>
        /// The top-left of the screen is (0, 0); the bottom-right is (pixelWidth, pixelHeight).
        /// </para>
        /// </summary>
        public Vector2 GUI
        {
            get
            {
                var point = _gazePointInUnityScreenSpace;
                point.y = UnityEngine.Screen.height - 1 - point.y;
                return point;
            }
        }

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
        /// Gets a value indicating whether the point is valid or not.
        /// <remarks>
        /// This indicates if the point was created with valid data. To check
        /// if a point is stale, use <see cref="Timestamp"/> instead.
        /// </remarks>
        /// </summary>
        public bool IsValid
        {
            get { return !float.IsNaN(_gazePointInUnityScreenSpace.x); }
        }

        /// <summary>
        /// Gets a value indicating whether the point is within the bounds of the game window or not.
        /// </summary>
        public bool IsWithinScreenBounds
        {
            get
            {
                var screenPoint = Screen;
                return IsValid &&
                       screenPoint.x >= 0 &&
                       screenPoint.x < UnityEngine.Screen.width &&
                       screenPoint.y >= 0 &&
                       screenPoint.y < UnityEngine.Screen.height;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", _gazePointInUnityScreenSpace, SequentialId, Timestamp);
        }
    }
}
