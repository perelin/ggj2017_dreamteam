//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using UnityEngine;

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Static access point for Tobii EyeTracking gaze data.
    /// </summary>
    public static class EyeTracking
    {
        private static readonly GameObject Identifier = new GameObject("EyeTracking_UniqueId");
        private static IDataProvider<GazePoint> _gazePointProvider;
        private static bool _isSubscribingToGazePointData;

        // --------------------------------------------------------------------
        //  Public methods
        // --------------------------------------------------------------------

        /// <summary>
        /// Initializes EyeTracking for use, and initializes and starts a gaze 
        /// point data stream with default filtering.
        /// </summary>
        public static void Initialize()
        {
            InitGazePointDataProvider();
        }

        /// <summary>
        /// Gets the <see cref="FocusedObject"/> with gaze focus. Only game 
        /// objects with a <see cref="GazeAware"/> component can be focused 
        /// using gaze.
        /// </summary>
        /// <returns>Returns the gaze-aware game object that has gaze focus, 
        /// or null if no gaze-aware object is focused.</returns>
        public static GameObject GetFocusedObject()
        {
            if (!_isSubscribingToGazePointData)
            {
                InitGazePointDataProvider();
            }

            var focusedObject = Host.GazeFocus.FocusedObject;

            if (!focusedObject.IsValid)
            {
                return null;
            }

            return focusedObject.GameObject;
        }

        /// <summary>
        /// Gets the gaze point. Subsequent calls to this function will return
        /// the same value within the frame.
        /// <para>
        /// The first time this function is called it will return an invalid 
        /// data point. To avoid this, call <see cref="Initialize()"/> some 
        /// frames before calling this function for the first time.
        /// </para>
        /// </summary>
        public static GazePoint GetGazePoint()
        {
            if (!_isSubscribingToGazePointData)
            {
                InitGazePointDataProvider();
            }

            return _gazePointProvider.GetFrameConsistentDataPoint();
        }

        /// <summary>
        /// Get the user presence, which indicates if there is a user present 
        /// in front of the screen.
        /// </summary>
        public static UserPresence GetUserPresence()
        {
            return Host.UserPresence;
        }

        /// <summary>
        /// Get the gaze tracking status, which indicates if the user's 
        /// eye-gaze is currently tracked or not. This status can be used in
        /// combination with a data point's IsValid flag to know if a data 
        /// point from <see cref="GetGazePoint()"/> is recent and valid.
        /// <para>
        /// This status reflects the status of the Tobii Engine. Before a gaze 
        /// point data stream has been initialized this status can return
        /// <see cref="GazeTrackingStatus.GazeTracked"/> even though 
        /// <see cref="GetGazePoint()"/> returns an invalid 
        /// <see cref="GazePoint"/> in the same frame.
        /// To avoid this, call <see cref="Initialize()"/> some frames
        /// before calling these functions.
        /// </para>
        /// </summary>
        public static GazeTracking GetGazeTrackingStatus()
        {
            return Host.GazeTracking;
        }

        /// <summary>
        /// Sets the camera that defines the user's current view point.
        /// </summary>
        /// <param name="camera"></param>
        public static void SetCurrentUserViewPointCamera(Camera camera)
        {
            Host.GazeFocus.Camera = camera;
        }

        /// <summary>
        /// Unsubscribes this class from the gaze data provider.
        /// To re-subscribe, make a call to the <see cref="GetGazePoint()"/> 
        /// function.
        /// </summary>
        public static void UnsubscribeGazePointData()
        {
            if (null == _gazePointProvider) { return; }

            _gazePointProvider.Stop(Identifier.GetInstanceID());
            _isSubscribingToGazePointData = false;
        }

        // --------------------------------------------------------------------
        //  Private properties and methods
        // --------------------------------------------------------------------

        private static IEyeTrackingHost Host
        {
            get
            {
                EyeTrackingHost.GetInstance().Initialize();
                return EyeTrackingHost.GetInstance();
            }
        }

        private static void InitGazePointDataProvider()
        {
            if (null == _gazePointProvider)
            {
               _gazePointProvider = Host.GetGazePointDataProvider();
            }

            _gazePointProvider.Start(Identifier.GetInstanceID());
            _isSubscribingToGazePointData = true;
        }
    }
}
