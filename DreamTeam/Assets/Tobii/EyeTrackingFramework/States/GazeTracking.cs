//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

namespace Tobii.EyeTracking
{
    public class GazeTracking
    {
        public GazeTracking(GazeTrackingStatus gazeTrackingStatus)
        {
            Status = gazeTrackingStatus;
        }

        public GazeTrackingStatus Status { get; private set; }

        public bool IsTrackingEyeGaze
        {
            get
            {
                return Status == GazeTrackingStatus.GazeTracked;
            }
        }
    }
}