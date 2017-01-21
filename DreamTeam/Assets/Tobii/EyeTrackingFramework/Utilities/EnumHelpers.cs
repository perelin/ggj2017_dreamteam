//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System;
using Tobii.EyeX.Client;
using Tobii.EyeX.Framework;
using UnityEngine;

namespace Tobii.EyeTracking
{
    internal static class EnumHelpers
    {
        public static IStateValue<UnityEngine.Rect> ConvertFromEyeXRect(IStateValue<Tobii.EyeX.Client.Rect> state)
        {
            if (!state.IsValid)
            {
                return StateValue<UnityEngine.Rect>.Invalid;
            }

            return new StateValue<UnityEngine.Rect>(new UnityEngine.Rect()
            {
                x = (float)state.Value.X,
                y = (float)state.Value.Y,
                width = (float)state.Value.Width,
                height = (float)state.Value.Height
            });
        } 

        public static IStateValue<Vector2> ConvertFromEyeXSize2(IStateValue<Size2> state)
        {
            if (!state.IsValid)
            {
                return StateValue<Vector2>.Invalid;
            }

            return new StateValue<Vector2>(new Vector2((float)state.Value.Width, (float)state.Value.Height));
        }

        public static DeviceStatus ConvertFromEyeXDeviceStatus(IStateValue<EyeTrackingDeviceStatus> state)
        {
            if (state == null || !state.IsValid)
            {
                return DeviceStatus.Unknown;
            }

            switch (state.Value)
            {
                // Pending?
                case EyeTrackingDeviceStatus.Initializing:
                case EyeTrackingDeviceStatus.Configuring:
                    return DeviceStatus.Pending;

                // Tracking?
                case EyeTrackingDeviceStatus.Tracking:
                    return DeviceStatus.Tracking;

                // Disabled?
                case EyeTrackingDeviceStatus.TrackingPaused:
                    return DeviceStatus.Disabled;

                // Not available
                default:
                    return DeviceStatus.NotAvailable;
            }
        }

        public static UserPresenceStatus ConvertFromEyeXUserPresence(IStateValue<Tobii.EyeX.Framework.UserPresence> state)
        {
            if (state == null || !state.IsValid)
            {
                return UserPresenceStatus.Unknown;
            }

            switch (state.Value)
            {
                // Present?
                case Tobii.EyeX.Framework.UserPresence.Present:
                    return UserPresenceStatus.Present;

                // Not present?
                case Tobii.EyeX.Framework.UserPresence.NotPresent:
                    return UserPresenceStatus.NotPresent;

                // Unknown?
                case Tobii.EyeX.Framework.UserPresence.Unknown:
                    return UserPresenceStatus.Unknown;

                default:
                    throw new InvalidOperationException("Unrecognized user presence value.");
            }
        }

        public static GazeTrackingStatus ConvertFromEyeXGazeTracking(IEyeTrackingHost host, IStateValue<Tobii.EyeX.Framework.GazeTracking> state)
        {
            if (host.EngineVersion == null || (host.EngineVersion != null && host.EngineVersion.Major >= 1 && host.EngineVersion.Minor >= 4))
            {
                if (state == null || !state.IsValid || state.Value == 0)
                {
                    return GazeTrackingStatus.Unknown;
                }

                switch (state.Value)
                {
                    // Gaze tracked?
                    case Tobii.EyeX.Framework.GazeTracking.GazeTracked:
                        return GazeTrackingStatus.GazeTracked;

                    // Gaze not tracked?
                    case Tobii.EyeX.Framework.GazeTracking.GazeNotTracked:
                        return GazeTrackingStatus.GazeNotTracked;

                    default:
                        throw new InvalidOperationException("Unknown gaze tracking value.");
                }
            }
            return GazeTrackingStatus.NotSupported;
        }
    }
}
#endif
