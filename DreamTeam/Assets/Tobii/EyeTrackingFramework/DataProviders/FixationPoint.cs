//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

using Tobii.EyeX.Framework;

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Holds a fixation point.
    /// </summary>
    public sealed class FixationPoint : ITimestamped
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="gazePoint">The location of the fixation. See also <seealso cref="Tobii.EyeTracking.GazePoint"/>.</param>
        /// <param name="sequentialId">The sequential ID of the fixation.</param>
        /// <param name="timestamp">The timestamp of the frame the event was received, in seconds (<see cref="UnityEngine.Time.time"/>).</param>
        /// <param name="eventType">The event type of the original fixation event.</param>
        public FixationPoint(GazePoint gazePoint, FixationDataEventType eventType, double sequentialId, float timestamp)
        {
            GazePoint = gazePoint;
            SequentialId = sequentialId;
            EventType = eventType;
            Timestamp = timestamp;
        }

        /// <summary>
        /// Gets a value representing an invalid fixation point.
        /// </summary>
        public static FixationPoint Invalid
        {
            get
            {
                return new FixationPoint(GazePoint.Invalid, (FixationDataEventType)0, double.NaN, float.NaN);
            }
        }

        /// <summary>
        /// Gets the location of the fixation. See also <seealso cref="Tobii.EyeTracking.GazePoint"/>.
        /// </summary>
        public GazePoint GazePoint { get; private set; }

        /// <summary>
        /// Gets the sequential ID for the data point.
        /// <para>
        /// This ID can be used to compare if a data point is newer than
        /// another. A higher value means a newer value.
        /// </para>
        /// </summary>
        public double SequentialId { get; private set; }


        /// <summary>
        /// Gets the <see cref="UnityEngine.Time.time"/> timestamp for the data point
        /// in seconds.
        /// <remarks>
        /// Note that more than one data point can be received in the same frame
        /// and have the same timestamp.
        /// </remarks>
        /// </summary>
        public float Timestamp { get; private set; }

        /// <summary>
        /// Gets a value indicating the kind of event this fixation point originates from.
        /// <para>
        /// - Begin: This is the beginning of a fixation
        /// </para><para>
        /// - Data: This is an ongoing fixation
        /// </para><para>
        /// - End: This is the end of a fixation
        /// </para>
        /// </summary>
        public FixationDataEventType EventType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the point is valid or not.
        /// <remarks>
        /// This indicates if the point was created with valid data. To check
        /// if a point is stale, use <see cref="Timestamp"/> instead.
        /// </remarks>
        /// </summary>
        public bool IsValid
        {
            get { return GazePoint.IsValid; }
        }
    }
}
