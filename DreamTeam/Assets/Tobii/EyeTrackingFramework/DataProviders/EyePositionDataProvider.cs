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
    /// Provider of eye Position data. When the provider has been started it
    /// will continuously update the Last property with the latest gaze point 
    /// value received from Tobii Engine.
    /// </summary>
    public class EyePositionDataProvider : DataProviderBase<EyePositions>
    {
        /// <summary>
        /// Creates a new instance.
        /// Note: don't create instances of this class directly. Use the <see cref="EyeTrackingHost.GetEyePositionDataProvider"/> method instead.
        /// </summary>
        public EyePositionDataProvider()
        {
            Last = EyePositions.Invalid;
        }

        public override string Id
        {
            get { return "EyePositionDataStream"; }
        }

        protected override void AssignBehavior(Interactor interactor)
        {
            var behavior = interactor.CreateBehavior(BehaviorType.EyePositionData);
            behavior.Dispose();
        }

        protected override void HandleEvent(IEnumerable<Behavior> eventBehaviors, float frameTimestamp, GameViewInfo gameViewInfo)
        {
            // Note that this method is called on a worker thread, so we MAY NOT access any game objects from here.
            // The data is stored in the Last property and used from the main thread.
            foreach (var behavior in eventBehaviors)
            {
                EyePositionDataEventParams eventParams;
                if (behavior.TryGetEyePositionDataEventParams(out eventParams))
                {
                    var left = new SingleEyePosition(eventParams.HasLeftEyePosition != EyeXBoolean.False, (float)eventParams.LeftEyeX, (float)eventParams.LeftEyeY, (float)eventParams.LeftEyeZ);
                    var leftNormalized = new SingleEyePosition(eventParams.HasLeftEyePosition != EyeXBoolean.False, (float)eventParams.LeftEyeXNormalized, (float)eventParams.LeftEyeYNormalized, (float)eventParams.LeftEyeZNormalized);

                    var right = new SingleEyePosition(eventParams.HasRightEyePosition != EyeXBoolean.False, (float)eventParams.RightEyeX, (float)eventParams.RightEyeY, (float)eventParams.RightEyeZ);
                    var rightNormalized = new SingleEyePosition(eventParams.HasRightEyePosition != EyeXBoolean.False, (float)eventParams.RightEyeXNormalized, (float)eventParams.RightEyeYNormalized, (float)eventParams.RightEyeZNormalized);

                    Last = new EyePositions(left, leftNormalized, right, rightNormalized, eventParams.Timestamp, frameTimestamp);
                }
            }
        }

        protected override void OnStreamingStarted()
        {
            Last = EyePositions.Invalid;
        }
    }
}
#endif
