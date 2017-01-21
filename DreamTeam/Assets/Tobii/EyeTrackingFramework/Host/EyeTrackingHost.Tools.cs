//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System;
using Tobii.EyeX.Framework;

namespace Tobii.EyeTracking
{
    public partial class EyeTrackingHost
    {
        /// <summary>
        /// Gets the availability of Tobii Engine.
        /// </summary>
        public static EngineAvailability TobiiEngineAvailability
        {
            get { return (EngineAvailability)Tobii.EyeX.Client.Environment.GetEyeXAvailability(); }
        }
    }
}

#endif
