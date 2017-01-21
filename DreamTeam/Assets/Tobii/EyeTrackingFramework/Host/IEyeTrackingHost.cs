//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using System;
using UnityEngine;

namespace Tobii.EyeTracking
{
    public interface IEyeTrackingHost
    {
        /// <summary>
        /// Gets the GazeFocus handler.
        /// </summary>
        IGazeFocus GazeFocus { get; }

        /// <summary>
        /// Gets the engine state: Screen bounds in pixels. The eye-tracked display monitor's screen size in physical (desktop) pixels.
        /// </summary>
        IStateValue<UnityEngine.Rect> ScreenBounds { get; }

        /// <summary>
        /// Gets the engine state: Display size as Vector2(width, height), in millimeters. The eye-tracked display monitor's screen size in millimeters.
        /// </summary>
        IStateValue<Vector2> DisplaySize { get; }

        /// <summary>
        /// Gets the engine state: Eye tracking status.
        /// </summary>
        DeviceStatus EyeTrackingDeviceStatus { get; }

        /// <summary>
        /// Gets the engine state: User presence.
        /// </summary>
        UserPresence UserPresence { get; }

        /// <summary>
        /// Gets the engine state: Gaze tracking.
        /// </summary>
        /// <value>The gaze tracking.</value>
        GazeTracking GazeTracking { get; }

        /// <summary>
        /// Gets the engine state: Engine version.
        /// </summary>
        /// <value>The gaze tracking.</value>
        Version EngineVersion { get; }

        /// <summary>
        /// Returns a value indicating whether the EyeTracking host has been initialized
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Gets a provider of gaze point data using default data processing.
        /// </summary>
        /// <returns>The data provider.</returns>
        IDataProvider<GazePoint> GetGazePointDataProvider();

        /// <summary>
        /// EXPERIMENTAL
        /// Gets a provider of eye position data.
        /// See <see cref="IDataProvider{T}"/>.
        /// </summary>
        /// <returns>The data provider.</returns>
        /// <remarks>
        /// This data stream should be considered experimental. It might be
        /// removed or changed in a future release. We have not been able
        /// to find any kind of proper or good-enough user experience based 
        /// on this data but provide it here for you to experiment with.
        /// </remarks>
        IDataProvider<EyePositions> GetEyePositionDataProvider();

        /// <summary>
        /// Initializes the EyeTracking host and connection to Tobii Engine.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Shuts down the EyeTracking host.
        /// </summary>
        void Shutdown();

        /// <summary>
        /// Gets the engine state: Current user profile.
        /// </summary>
        IStateValue<string> UserProfileName { get; }

        /// <summary>
        /// Gets the engine state: User profiles.
        /// </summary>
        IStateValue<string[]> UserProfileNames { get; }

        /// <summary>
        /// Sets the current EyeTracking user profile.
        /// </summary>
        /// <remarks>
        /// Requires the EyeTrackingHost to be initialized. Throws <see cref="InvalidOperationException"/>
        /// if the EyeTrackingHost is not initialized.
        /// </remarks>
        /// <param name="profileName">The name of the profile to set.</param>
        void SetCurrentProfile(string profileName);
    }
}
