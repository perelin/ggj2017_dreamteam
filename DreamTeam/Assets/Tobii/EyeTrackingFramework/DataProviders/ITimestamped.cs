//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using UnityEngine;

namespace Tobii.EyeTracking
{
    public interface ITimestamped
    {
        /// <summary>
        /// Returns a value indicating if the timestamped value is valid or not.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Gets the sequential ID for the data point.
        /// <para>
        /// This ID can be used to compare if a data point is newer than
        /// another. A higher value means a newer value.
        /// </para>
        /// </summary>
        double SequentialId { get; }

        /// <summary>
        /// Gets the <see cref="Time.time"/> timestamp for the data point
        /// in seconds.
        /// <remarks>
        /// Note that more than one data point can be received in the same frame
        /// and have the same timestamp.
        /// </remarks>
        /// </summary>
        float Timestamp { get; }
    }
}
