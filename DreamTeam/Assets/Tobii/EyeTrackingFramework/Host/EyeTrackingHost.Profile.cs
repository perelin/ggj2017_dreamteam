//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System;

namespace Tobii.EyeTracking
{
    public partial class EyeTrackingHost
    {
        private StateAccessor<string> _userProfileNameStateAccessor;
        private EnumerableStateAccessor<string> _userProfileNamesStateAccessor;

        /// <summary>
        /// Gets the engine state: Current user profile.
        /// </summary>
        public IStateValue<string> UserProfileName
        {
            get
            {
                return _userProfileNameStateAccessor.GetCurrentValue(_context);
            }
        }

        /// <summary>
        /// Gets the engine state: User profiles.
        /// </summary>
        public IStateValue<string[]> UserProfileNames
        {
            get
            {
                return _userProfileNamesStateAccessor.GetCurrentValue(_context);
            }
        }

        /// <summary>
        /// Sets the current EyeTracking user profile.
        /// </summary>
        /// <remarks>
        /// Requires the EyeTrackingHost to be initialized. Throws <see cref="InvalidOperationException"/>
        /// if the EyeTrackingHost is not initialized.
        /// </remarks>
        /// <param name="profileName">The name of the profile to set.</param>
        public void SetCurrentProfile(string profileName)
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("EyeTracking is not initialized.");
            }

            _context.SetCurrentProfile(profileName, null);
        }
    }
}
#endif
