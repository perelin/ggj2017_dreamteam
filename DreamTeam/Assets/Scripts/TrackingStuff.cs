using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tobii.EyeTracking;

class TrackingStuff
{
    private static bool UseEyeTracking = true;

    // eye tracking is activated
    public static bool isEyeTracking()
    {
        var status = EyeTracking.GetGazeTrackingStatus().Status;
        return UseEyeTracking && status != GazeTrackingStatus.NotSupported && status != GazeTrackingStatus.Unknown;
    }

    public static bool areEyesClosed()
    {
        var status = EyeTracking.GetGazeTrackingStatus().Status;
        return isEyeTracking() && status == GazeTrackingStatus.GazeNotTracked;
    }

    public static Vector2 getTrackingPos()
    {
        if (TrackingStuff.isEyeTracking())
        {
            // get eye tracking point
            GazePoint gazePoint = EyeTracking.GetGazePoint();
            if (gazePoint.IsValid)
            {
                return new Vector3(gazePoint.Screen.x, gazePoint.Screen.y);
            }
            return Vector3.zero;
        }
        else
        {
            // get mouse position as replacement for eye tracking
            return Input.mousePosition;
        }
    }
}
