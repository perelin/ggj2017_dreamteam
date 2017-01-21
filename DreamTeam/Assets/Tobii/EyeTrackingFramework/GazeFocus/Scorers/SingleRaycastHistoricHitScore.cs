//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//---------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tobii.EyeTracking
{
    public class SingleRaycastHistoricHitScore : IScorer
    {
        private static readonly float GainGazeDwellTime = 0.0f;
        private static readonly float LoseGazeDwellTime = 0.15f;
        private static readonly float Threshold = 0.04f;

        private readonly Dictionary<int, ScoredObject> _scoredObjects = new Dictionary<int, ScoredObject>();
        private ScoredObject _focusedObject = ScoredObject.Empty();
        private int _layerMask;

        public SingleRaycastHistoricHitScore()
        {
            
            MaximumDistance = GazeFocus.MaximumDistance;
            LayerMask = GazeFocus.LayerMask;
        }
        
        public SingleRaycastHistoricHitScore(float maximumDistance, int layerMask)
        {
            MaximumDistance = maximumDistance;
            LayerMask = layerMask;
        }

        private FocusedObject FocusedGameObject
        {
            get
            {
                if (_focusedObject.Equals(ScoredObject.Empty()))
                {
                    return FocusedObject.Invalid;
                }

                return new FocusedObject(_focusedObject.GameObject);
            }
        }

        /// <summary>
        /// Maximum distance to detect gaze focus within.
        /// </summary>
        private float MaximumDistance { get; set; }

        /// <summary>
        /// Layers to detect gaze focus on.
        /// </summary>
        private LayerMask LayerMask
        {
            get { return _layerMask; }
            set { _layerMask = value.value; }
        }

        public FocusedObject GetFocusedObject(IEnumerable<GazePoint> lastGazePoints, Camera camera)
        {
            var objectsInGaze = FindObjectsInGaze(lastGazePoints, camera);
            UpdateFocusConfidenceScore(objectsInGaze);
            var focusChallenger = FindFocusChallenger();

            if (focusChallenger.GetScore()
                > _focusedObject.GetScore() + Threshold)
            {
                _focusedObject = focusChallenger;
            }

            return FocusedGameObject;
        }

        public IEnumerable<GameObject> GetObjectsInGaze(IEnumerable<GazePoint> lastGazePoints, Camera camera)
        {
            GetFocusedObject(lastGazePoints, camera);
            return _scoredObjects
                .Where(kvp => kvp.Value.GetScore() > 0.0f)
                .Select(kvp => kvp.Value.GameObject);
        }

        public FocusedObject GetFocusedObject()
        {
            ClearFocusedObjectIfOld();
            return FocusedGameObject;
        }

        public void Reconfigure(float maximumDistance, int layerMask)
        {
            Reset();
            MaximumDistance = maximumDistance;
            LayerMask = layerMask;
        }

        public void RemoveObject(GameObject gameObject)
        {
            _scoredObjects.Remove(gameObject.GetInstanceID());
            if (_focusedObject.GameObject.GetInstanceID() == gameObject.GetInstanceID())
            {
                _focusedObject = ScoredObject.Empty();
            }
        }

        public void Reset()
        {
            _scoredObjects.Clear();
            _focusedObject = ScoredObject.Empty();
        }

        private IEnumerable<GameObject> FindObjectsInGaze(IEnumerable<GazePoint> gazePoints, Camera camera)
        {
            var points = gazePoints
                .Where(gazePoint => gazePoint.IsWithinScreenBounds)
                .Select(gazePoint => gazePoint.Screen);
            var objectsInGaze = new List<GameObject>();

            IEnumerable<RaycastHit> hitInfos;
            if (HitTestFromPoint.FindMultipleObjectsInWorldFromMultiplePoints(out hitInfos, points, camera,
                MaximumDistance, LayerMask))
            {
                foreach (var raycastHit in hitInfos)
                {
                    objectsInGaze.Add(raycastHit.collider.gameObject);
                }
            }

            return objectsInGaze;
        } 

        private void UpdateFocusConfidenceScore(IEnumerable<GameObject> objectsInGaze)
        {
            foreach (var objectInGaze in objectsInGaze)
            {
                var instanceId = objectInGaze.GetInstanceID();
                if (!_scoredObjects.ContainsKey(instanceId))
                {
                    if (!GazeFocus.IsFocusableObject(objectInGaze))
                    {
                        continue;
                    }

                    _scoredObjects.Add(objectInGaze.GetInstanceID(), new ScoredObject(objectInGaze, GainGazeDwellTime, LoseGazeDwellTime));
                }

                ScoredObject hitObject = _scoredObjects[instanceId];
                hitObject.AddHit(Time.time, Time.deltaTime);
            }

            ClearFocusedObjectIfOld();
        }

        private ScoredObject FindFocusChallenger()
        {
            ScoredObject topFocusChallenger = ScoredObject.Empty();
            float topScore = 0.0f;

            foreach (var key in _scoredObjects.Keys)
            {
                ScoredObject scoredObject = _scoredObjects[key];

                var score = scoredObject.GetScore(Time.time - LoseGazeDwellTime, Time.time - GainGazeDwellTime);

                if (score > topScore)
                {
                    topScore = score;
                    topFocusChallenger = scoredObject;
                }
            }

            return topFocusChallenger;
        }

        private void ClearFocusedObjectIfOld()
        {
            if (!_focusedObject.IsRecentlyHit())
            {
                _focusedObject = ScoredObject.Empty();
            }
        }
    }
}