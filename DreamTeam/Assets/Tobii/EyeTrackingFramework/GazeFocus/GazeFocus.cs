//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Keeps track of the current eye-gaze focus, whether there is a game
    /// object with an IGazeFocusable component that is focused, or not.
    /// </summary>
    public class GazeFocus : IGazeFocus, IRegisterGazeFocusable, IGazeFocusInternal
    {
        private static readonly Dictionary<int, IGazeFocusable> FocusableObjects = new Dictionary<int, IGazeFocusable>();
        private static IScorer _multiScorer;
        private static bool _isInitialized;
        private readonly GameObject _identifier = new GameObject("GazeFocus_UniqueId");
        private IDataProvider<GazePoint> _gazePointDataProvider; 
        private FocusedObject _focusedObject = FocusedObject.Invalid;
        private GazePoint _lastHandledGazePoint = GazePoint.Invalid;
        private bool _useMouseEmulation = false; // TODO: use logic that can be controlled from UnityEditor?
        private Camera _camera;

        //---------------------------------------------------------------------
        // Implementing IGazeFocus
        //---------------------------------------------------------------------

        public Camera Camera
        {
            get { return _camera ?? Camera.main; }
            set
            {
                _camera = value;
                if (Scorer != null)
                {
                    Scorer.Reset();
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="GameObject"/> with gaze focus. Only game objects
        /// with a <see cref="GazeAware"/> component can be focused using gaze.
        /// Returns null if no object is focused.
        /// </summary>
        public FocusedObject FocusedObject
        {
            get { return _focusedObject; }

            private set
            {
                if (!_focusedObject.Equals(value))
                {
                    if (_focusedObject.IsValid)
                    {
                        var lostFocusComponent = FocusableObjects[_focusedObject.Key];
                        lostFocusComponent.UpdateGazeFocus(false);
                    }

                    _focusedObject = value;

                    if (_focusedObject.IsValid)
                    {
                        var receivedFocusComponent = FocusableObjects[_focusedObject.Key];
                        receivedFocusComponent.UpdateGazeFocus(true);
                    }
                }
            }
        }

        //---------------------------------------------------------------------
        // Implementing IGazeFocusInternal
        //---------------------------------------------------------------------

        /// <summary>
        /// Registers the supplied <see cref="IGazeFocusable"/> component so that
        /// the <see cref="GameObject"/> it belongs to can be focused using eye-gaze.
        /// </summary>
        /// <param name="gazeFocusableComponent"></param>
        public void RegisterFocusableComponent(IGazeFocusable gazeFocusableComponent)
        {
            if (FocusableObjects.Count == 0)
            {
                Initialize();
            }

            var instanceId = gazeFocusableComponent.gameObject.GetInstanceID();
            if (!FocusableObjects.ContainsKey(instanceId))
            {
                FocusableObjects.Add(instanceId, gazeFocusableComponent);
            }
        }

        /// <summary>
        /// Unregisters the supplied <see cref="IGazeFocusable"/> component so
        /// that the <see cref="GameObject"/> it belongs to no longer can be 
        /// focused using eye-gaze.
        /// </summary>
        /// <param name="gazeFocusableComponent"></param>
        public void UnregisterFocusableComponent(IGazeFocusable gazeFocusableComponent)
        {
            var instanceId = gazeFocusableComponent.gameObject.GetInstanceID();
            if (_focusedObject.IsValid && 
                _focusedObject.GameObject.GetInstanceID() == instanceId)
            {
                // TODO: set to other object with high focus confidence instead?
                FocusedObject = FocusedObject.Invalid;
            }

            FocusableObjects.Remove(instanceId);

            if (Scorer != null)
            {
                Scorer.RemoveObject(gazeFocusableComponent.gameObject);
            }
            if (_multiScorer != null)
            {
                _multiScorer.RemoveObject(gazeFocusableComponent.gameObject);
            }
        }

        /// <summary>
        /// Updates the gaze focus according to the latest gaze data.
        /// </summary>
        public void UpdateGazeFocus()
        {
            if (!IsInitialized) { return; }

            IEnumerable<GazePoint> lastGazePoints;
            if (!TryGetLastGazePoints(out lastGazePoints))
            {
                FocusedObject = Scorer.GetFocusedObject();
                return;
            }

            FocusedObject = Scorer.GetFocusedObject(lastGazePoints, Camera);
        }

        //---------------------------------------------------------------------
        // Public static properties and methods
        //---------------------------------------------------------------------

        public static bool IsInitialized
        {
            get { return _isInitialized; }
        }

        /// <summary>
        /// Notifies that the gaze focus settings have changed and need to be reloaded.
        /// </summary>
        public static void SettingsUpdated()
        {
            if (IsInitialized)
            {
                ReloadSettings();
            }
        }

        /// <summary>
        /// Checks if a component is registered as a focusable object.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static bool IsFocusableObject(GameObject gameObject)
        {
            var instanceId = gameObject.GetInstanceID();
            return FocusableObjects.ContainsKey(instanceId);
        }

        //---------------------------------------------------------------------
        // Internal static properties
        //---------------------------------------------------------------------

        internal static IScorer Scorer { get; set; }

        /// <summary>
        /// Layers to detect gaze focus on.
        /// </summary>
        internal static LayerMask LayerMask { get; private set; } // TODO: make internal?

        /// <summary>
        /// Maximum distance to detect gaze focus within.
        /// </summary>
        internal static float MaximumDistance { get; private set; } // TODO: make internal?

        //---------------------------------------------------------------------
        // Private methods
        //---------------------------------------------------------------------

        private void Initialize()
        {
            if (_gazePointDataProvider == null)
            {
                ReloadSettings();
                _gazePointDataProvider = EyeTrackingHost.GetInstance().GetGazePointDataProvider();
                _gazePointDataProvider.Start(_identifier.GetInstanceID());
            }

            if (Scorer == null)
            {
                //Scorer = new SingleRayCastNoScore();
                Scorer = new SingleRaycastHistoricHitScore();
                //Scorer = new MultiRaycastHistoricHitScore();
            }
            if (_multiScorer == null)
            {
                _multiScorer = new MultiRaycastHistoricHitScore();
            }

            _isInitialized = true;
        }

        private static void ReloadSettings()
        {
            var gazeFocusSettings = GazeFocusSettings.Get();
            LayerMask = gazeFocusSettings.LayerMask;
            MaximumDistance = gazeFocusSettings.MaximumDistance;
            if (Scorer != null)
            {
                Scorer.Reconfigure(MaximumDistance, LayerMask.value);
            }
            if (_multiScorer != null)
            {
                _multiScorer.Reconfigure(MaximumDistance, LayerMask.value);
            } 
        }

        private bool TryGetGazePoint(out GazePoint gazePoint)
        {
            if (!IsInitialized)
            {
                gazePoint = GazePoint.Invalid;
                return false;
            }

            if (_useMouseEmulation)
            {
                var host = EyeTrackingHost.GetInstance() as EyeTrackingHost;
                if (host)
                {
                    gazePoint = new GazePoint(Input.mousePosition, (double)Time.time / 1000.0f, Time.time);
                    return true;
                }
            }

            gazePoint = GetLastGazePoint();
            return gazePoint.IsWithinScreenBounds;
        }

        private bool TryGetLastGazePoints(out IEnumerable<GazePoint> gazePoints)
        {
            if (!IsInitialized)
            {
                gazePoints = new List<GazePoint>();
                return false;
            }

            if (_useMouseEmulation)
            {
                var host = EyeTrackingHost.GetInstance() as EyeTrackingHost;
                if (host)
                {
                    var gazePoint = new GazePoint(Input.mousePosition, (double)Time.time / 1000.0f, Time.time);
                    gazePoints = new List<GazePoint>() {gazePoint};
                    return true;
                }
            }

            gazePoints = _gazePointDataProvider.GetDataPointsSince(_lastHandledGazePoint);
            UpdateLastHandledGazePoint(gazePoints);

            return gazePoints.Any();
        }

        private void UpdateLastHandledGazePoint(IEnumerable<GazePoint> gazePoints)
        {
            foreach (var gazePoint in gazePoints)
            {
                if (gazePoint.SequentialId > _lastHandledGazePoint.SequentialId)
                {
                    _lastHandledGazePoint = gazePoint;
                }
            }
        }

        private bool IsDifferent(GameObject first, GameObject second)
        {
            if (null == first && null == second)
            {
                return false;
            }

            if (null == first || null == second)
            {
                return true;
            }

            return first.GetInstanceID() != second.GetInstanceID();
        }

        /// <summary>
        /// Gets the last frame consistent gaze point.
        /// </summary>
        /// <remarks>Requirers the gaze point data provider is not null.</remarks>
        private GazePoint GetLastGazePoint()
        {
            return _gazePointDataProvider.GetFrameConsistentDataPoint();
        }
    }
}

#else
using System.Collections.Generic;
using UnityEngine;
namespace Tobii.EyeTracking
{
    public class GazeFocus : GazeFocusStub
    {
        // all implementation is in the stub
    }
}
#endif
