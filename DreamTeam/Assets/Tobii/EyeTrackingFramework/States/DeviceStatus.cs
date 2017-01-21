//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Represents different eye tracker device statuses.
    /// </summary>
    public enum DeviceStatus
    {
        /// <summary>
        /// The eye tracking device is in an unknown state.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Connection to the eye tracking device is pending.
        /// It might be initializing or connecting.
        /// </summary>
        Pending = 1,
        /// <summary>
        /// The eye tracking device is tracking.
        /// </summary>
        Tracking = 2,
        /// <summary>
        /// The eye tracking device is disabled.
        /// </summary>
        Disabled = 3,
        /// <summary>
        /// The eye tracking device is unavailable.
        /// </summary>
        NotAvailable = 4
    }
}
