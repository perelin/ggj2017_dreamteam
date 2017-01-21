//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Tobii.EyeTracking
{
    public interface IGazeFocus
    {
        /// <summary>
        /// Settable camera that defines the user's current view point.
        /// </summary>
        Camera Camera { get; set; }

        /// <summary>
        /// Gets the <see cref="FocusedObject"/> with gaze focus. Only game 
        /// objects with a <see cref="GazeAware"/> or other 
        /// <see cref="IGazeFocusable"/> component can be focused using gaze. 
        /// <para>
        /// Returns null if no object is focused.
        /// </para>
        /// </summary>
        FocusedObject FocusedObject { get; }
    }
}
