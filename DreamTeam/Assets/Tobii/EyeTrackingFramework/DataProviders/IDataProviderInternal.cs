//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System;
using Tobii.EyeX.Client;

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Interface of a global interactor. Used by the EyeTrackingHost.
    /// </summary>
    internal interface IDataProviderInternal
    {
        /// <summary>
        /// Event raised when the state of the global interactor has changed
        /// in a way so that Tobii Engine has to be updated with the new
        /// parameter settings.
        /// <para>
        /// For example: when the state has changed so the global interactor
        /// should be removed from Tobii Engine, that is the data stream
        /// should be stopped. 
        /// </para>
        /// </summary>
        event EventHandler Updated;

        /// <summary>
        /// Gets the unique ID of the interactor that provides the data stream.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Creates an Tobii Engine Interactor object from the properties and behaviors of
        /// this global interactor and adds it to the provided snapshot.
        /// </summary>
        /// <param name="snapshot">The <see cref="Snapshot"/> to
        /// add the interactor to.</param>
        /// <param name="forceDeletion">If true, forces the interactor to be deleted.</param>
        void AddToSnapshot(Snapshot snapshot, bool forceDeletion);

        /// <summary>
        /// Handles interaction events.
        /// </summary>
        /// <param name="event_">The <see cref="InteractionEvent"/> instance containing the event data.</param>
        /// <param name="frameTimestamp">The timestamp of the frame the event was received.</param>

        /// <param name="gameViewInfo">Information about game view position and pixel scaling.</param>
        void HandleEvent(InteractionEvent event_, float frameTimestamp, GameViewInfo gameViewInfo);

        /// <summary>
        /// Signals the end of the frame.
        /// </summary>
        void EndFrame();
    }
}
#endif
