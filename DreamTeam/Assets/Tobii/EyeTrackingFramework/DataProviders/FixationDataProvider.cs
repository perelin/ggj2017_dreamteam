//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using UnityEngine;
using Tobii.EyeX.Client;
using Tobii.EyeX.Framework;
using System.Collections.Generic;

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Provider of fixation data. When the provider has been started it
    /// will continuously update the Last property with the latest gaze point 
    /// value received from Tobii Engine.
    /// </summary>
    public class FixationDataProvider : DataProviderBase<FixationPoint>
    {
        private readonly FixationDataMode _mode;

        /// <summary>
        /// Creates a new instance.
        /// Note: don't create instances of this class directly. Use the <see cref="EyeTrackingHost.GetFixationDataProvider"/> method instead.
        /// </summary>
        /// <param name="mode">Data mode.</param>
        public FixationDataProvider(FixationDataMode mode)
        {
            _mode = mode;
            Last = FixationPoint.Invalid;
        }

        public override string Id
        {
            get { return string.Format("FixationDataStream/{0}", _mode); }
        }

        protected override void AssignBehavior(Interactor interactor)
        {
            var behaviorParams = new FixationDataParams() { FixationDataMode = _mode };
            interactor.CreateFixationDataBehavior(ref behaviorParams);
        }

        protected override void HandleEvent(IEnumerable<Behavior> eventBehaviors, float frameTimestamp, GameViewInfo gameViewInfo)
        {
            // Note that this method is called on a worker thread, so we MAY NOT access any game objects from here.
            // The data is stored in the Last property and used from the main thread.
            foreach (var behavior in eventBehaviors)
            {
                FixationDataEventParams eventParams;
                if (behavior.TryGetFixationDataEventParams(out eventParams))
                {
	                var gazePointInUnityScreenSpace = gameViewInfo.DisplaySpaceToUnityScreenSpace((float)eventParams.X, (float)eventParams.Y);

                    var gazePoint = new GazePoint(
						gazePointInUnityScreenSpace,
                        eventParams.Timestamp,
                        frameTimestamp);

                    Last = new FixationPoint(gazePoint, eventParams.EventType, eventParams.Timestamp, frameTimestamp);
                }
            }
        }

        protected override void OnStreamingStarted()
        {
            Last = FixationPoint.Invalid;
        }
    }
}
#endif
