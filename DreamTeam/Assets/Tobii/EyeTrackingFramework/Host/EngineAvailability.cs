//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Describes the availability statuses of Tobii Engine.
    /// </summary>
    public enum EngineAvailability
    {
        /// <summary>
        /// Tobii Engine is not available.
        /// </summary>
        NotAvailable = 1,

        /// <summary>
        /// Tobii Engine is installed but not running.
        /// </summary>
        NotRunning = 2,

        /// <summary>
        /// Tobii Engine is running.
        /// </summary>
        Running = 3,
    }
}
