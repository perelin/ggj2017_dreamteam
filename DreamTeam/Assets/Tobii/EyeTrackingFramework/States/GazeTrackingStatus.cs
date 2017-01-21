//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Represents different gaze tracking states.
    /// </summary>
    public enum GazeTrackingStatus
    {
        /// <summary>
        /// Gaze tracking is unknown.
        /// This might be due to an error.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The user's eye-gaze is tracked.
        /// This means that gaze point data is delivered from the eye tracker.
        /// </summary>
        GazeTracked = 1,
        /// <summary>
        /// The user's eye-gaze is not tracked.
        /// This means that the eye tracker does not deliver gaze point data.
        /// </summary>
        GazeNotTracked = 2,
        /// <summary>
        /// Gaze tracking state is not supported
        /// in the version of Tobii Engine that the user has.
        /// </summary>
        NotSupported = 3,
    }
}
