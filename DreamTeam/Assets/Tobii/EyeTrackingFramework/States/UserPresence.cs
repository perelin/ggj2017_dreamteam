//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

namespace Tobii.EyeTracking
{
    public class UserPresence
    {
        public UserPresence(UserPresenceStatus userPresenceStatus)
        {
            Status = userPresenceStatus;
        }

        public UserPresenceStatus Status { get; private set; }

        public bool IsUserPresent
        {
            get
            {
                return Status == UserPresenceStatus.Present;
            }
        }
    }
}