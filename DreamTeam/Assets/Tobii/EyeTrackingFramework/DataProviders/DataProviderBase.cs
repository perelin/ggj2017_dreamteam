//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System;
using UnityEngine;
using Tobii.EyeX.Client;
using Tobii.EyeX.Framework;
using System.Collections.Generic;

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Base class for data streams.
    /// </summary>
    /// <typeparam name="T">Type of the provided data value object.</typeparam>
    public abstract class DataProviderBase<T> : IDataProvider<T>, IDataProviderInternal where T : ITimestamped
    {
        private readonly Dictionary<int, int> _subscribers = new Dictionary<int, int>();
        private readonly List<T> _lastDataPoints = new List<T>(); 
        private readonly object _lock = new object();
        private const float PruneIntervalSecs = 5.0f;
        private float _pruneLastDataPointsTimer = PruneIntervalSecs;
        private T _last;
        private T _lastReadInFrame;
        private bool _isLastReadInFrame;


        private bool IsStarted
        {
            get { return _subscribers.Count > 0; }
        }

        // --------------------------------------------------------------------
        //  Implementation of IDataProvider<T>
        // --------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the latest value of the data stream. The value is never null but 
        /// it might be invalid.
        /// </summary>
        public T Last
        {
            get
            {
                lock (_lock)
                {
                    _lastReadInFrame = _last;
                    _isLastReadInFrame = true;
                    return _last;
                }
            }

            protected set
            {
                lock(_lock)
                {
                    _last = value;
                }
            }
        }

        /// <summary>
        /// Gets the last possible data value that is also consistent with previous
        /// reads in the frame. As soon as the Last value is accessed, or this
        /// function is called in a frame, all subsequent calls to this function 
        /// within that frame will return the same value.
        /// </summary>
        /// <returns>The last data point that can be consistently read in the frame.</returns>
        public T GetFrameConsistentDataPoint()
        {
            if (!_isLastReadInFrame)
            {
                return Last;
            }

            return _lastReadInFrame;
        }

        /// <summary>
        /// Gets all data points since the supplied data point. 
        /// Points older than 500 ms will not be included.
        /// </summary>
        public IEnumerable<T> GetDataPointsSince(ITimestamped dataPoint)
        {
            var dataPointSequentialId = dataPoint.IsValid ? dataPoint.SequentialId : 0.0;

            lock (_lock)
            {
                return _lastDataPoints.FindAll(point => 
                    (point.SequentialId > dataPointSequentialId) && 
                    (point.Timestamp > Time.time - 0.5f));
            }
        }

        /// <summary>
        /// Starts the provider. Data will continuously be updated in the Last
        /// property as events are received from Tobii Engine.
        /// </summary>
        public void Start(int subscriberId)
        {
            var oldCount = _subscribers.Count;

            _subscribers[subscriberId] = subscriberId;
            if ((oldCount == 0) && (_subscribers.Count == 1))
            {
                OnStreamingStarted();
                RaiseUpdatedEvent();
            }
        }

        /// <summary>
        /// Requests to stop the data provider. If there are no other clients
        /// that are currently requesting the provider to keep providing data,
        /// the provider will stop the stream of data from Tobii Engine and
        /// stop updating the Last property.
        /// </summary>
        public void Stop(int subscriberId)
        {
            if (_subscribers.Remove(subscriberId))
            {
                if (_subscribers.Count == 0)
                {
                    OnStreamingStopped();
                    RaiseUpdatedEvent();
                }
            }
        }

        // --------------------------------------------------------------------
        //  Implementation of IDataProviderInternal
        // --------------------------------------------------------------------

        /// <summary>
        /// Event raised when the state of the global interactor has changed
        /// in a way so that Tobii Engine has to be updated with the new
        /// parameter settings.
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        /// Gets the unique ID of the interactor that provides the data stream.
        /// </summary>
        public abstract string Id
        {
            get;
        }

        /// <summary>
        /// Creates an Tobii Engine Interactor object from the properties and behaviors of
        /// this global interactor and adds it to the provided snapshot.
        /// </summary>
        /// <param name="snapshot">The <see cref="Snapshot"/> to
        /// add the interactor to.</param>
        /// <param name="forceDeletion">If true, forces the interactor to be deleted.</param>
        public void AddToSnapshot(Snapshot snapshot, bool forceDeletion)
        {
            using (var interactor = snapshot.CreateInteractor(Id, Literals.RootId, Literals.GlobalInteractorWindowId))
            {
                var bounds = interactor.CreateBounds(BoundsType.None);
                bounds.Dispose();

                if (!IsStarted || forceDeletion)
                {
                    interactor.IsDeleted = true;
                }

                AssignBehavior(interactor);
            }
        }

        /// <summary>
        /// Handles interaction events.
        /// </summary>
        /// <param name="event_">The <see cref="InteractionEvent"/> instance containing the event data.</param>
        /// <param name="frameTimestamp">The timestamp of the frame the event was received.</param>

        /// <param name="gameViewInfo">Information about game view position and pixel scaling.</param>
        public void HandleEvent(InteractionEvent event_, float frameTimestamp, GameViewInfo gameViewInfo)
        {
            var eventBehaviors = event_.Behaviors;

            HandleEvent(eventBehaviors, frameTimestamp, gameViewInfo);

            lock (_lock)
            {
                _lastDataPoints.Add(Last);
            }

            foreach (var eventBehavior in eventBehaviors)
            {
                eventBehavior.Dispose();
            }
        }

        /// <summary>
        /// Signals the end of the frame. Perform end-of-frame cleanup of persisted state.
        /// </summary>
        public void EndFrame()
        {
            lock (_lock)
            {
                _isLastReadInFrame = false;
                _lastReadInFrame = default(T);
            }

            _pruneLastDataPointsTimer += Time.deltaTime;
            if (_pruneLastDataPointsTimer > PruneIntervalSecs)
            {
                PruneLastDataPoints(Time.time - 0.5f);
            }
        }

        // --------------------------------------------------------------------
        //  Protected and private methods
        // --------------------------------------------------------------------

        private void PruneLastDataPoints(float minimumTimestamp)
        {
            lock (_lock)
            {
                _lastDataPoints.RemoveAll(point => point.Timestamp < minimumTimestamp);
            }
        }

        protected abstract void AssignBehavior(Interactor interactor);

        protected abstract void HandleEvent(IEnumerable<Behavior> eventBehaviors, float frameTimestamp, GameViewInfo gameViewInfo);

        protected virtual void OnStreamingStarted()
        {
            // default implementation does nothing
        }

        protected virtual void OnStreamingStopped()
        {
            // default implementation does nothing
        }

        private void RaiseUpdatedEvent()
        {
            var handler = Updated;
            if (null != handler)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
#endif
