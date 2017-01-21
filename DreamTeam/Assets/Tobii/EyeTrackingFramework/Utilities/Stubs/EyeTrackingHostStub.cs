//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using System;
using Tobii.EyeTracking.Stubs;
using UnityEngine;

namespace Tobii.EyeTracking
{
    public class EyeTrackingHostStub : IEyeTrackingHost
    {
        private static EyeTrackingHostStub _instance;

        public IGazeFocus GazeFocus { get { return new GazeFocusStub(); } }
        public IStateValue<UnityEngine.Rect> ScreenBounds { get { return StateValueStub<Rect>.Invalid; } }
        public IStateValue<Vector2> DisplaySize { get { return StateValueStub<Vector2>.Invalid; } }
        public DeviceStatus EyeTrackingDeviceStatus { get { return DeviceStatus.NotAvailable; } }
        public UserPresence UserPresence { get { return new UserPresence(UserPresenceStatus.Unknown); } }
        public GazeTracking GazeTracking { get { return new GazeTracking(GazeTrackingStatus.Unknown); } }
        public Version EngineVersion { get { return new Version("0.0.0.0"); } }
        public bool IsInitialized { get { return false; } }

        public int GetInstanceID() { return 0; }

        public static IEyeTrackingHost GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EyeTrackingHostStub();
            }

            return _instance;
        }

        public IDataProvider<GazePoint> GetGazePointDataProvider()
        {
            return new GazePointDataProviderStub();
        }

        public IDataProvider<EyePositions> GetEyePositionDataProvider()
        {
            return new EyePositionDataProviderStub();
        }

        public void Initialize() { /** no implementation **/ }
        public void Shutdown() { /** no implementation **/ }

        internal GameViewInfo GetGameViewInfo()
        {
            return new GameViewInfo(new Vector2(float.NaN, float.NaN), Vector2.one);
        }

        public IStateValue<string> UserProfileName { get { return StateValueStub<string>.Invalid; } }
        public IStateValue<string[]> UserProfileNames { get { return StateValueStub<string[]>.Invalid; } }
        public void SetCurrentProfile(string profileName) { /** no implementation **/ }

        public static EngineAvailability TobiiEngineAvailability { get { return EngineAvailability.NotAvailable; } }
        public void LaunchRecalibration() { /** no implentation **/ }
        public void LaunchCalibrationTesting() { /** no implentation **/ }

        public static implicit operator bool(EyeTrackingHostStub exists)
        {
            return null != exists;
        }
    }
}