//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tobii.EyeTracking
{
    public class ScoredObject
    {
        private static readonly GameObject EmptyGameObject = new GameObject("ScoredObject_EmptyGameObject");
        private readonly float _gainGazeDwellTime;
        private readonly float _loseGazeDwellTime;

        // Hits are pairs of Time.time and Time.deltaTime timestamps
        private readonly List<KeyValuePair<float,float>> _hits = new List<KeyValuePair<float, float>>();

        public static ScoredObject Empty()
        {
            return new ScoredObject(EmptyGameObject);
        }

        public ScoredObject(GameObject gameObject, float gainGazeDwellTime = 0.05f, float loseGazeDwellTime = 0.15f)
        {
            GameObject = gameObject;
            _gainGazeDwellTime = gainGazeDwellTime;
            _loseGazeDwellTime = loseGazeDwellTime;
        }

        public GameObject GameObject { get; private set; }

        public bool IsRecentlyHit()
        {
            return IsRecentlyHit(Time.time - _loseGazeDwellTime, Time.time - _gainGazeDwellTime);
        }

        public bool IsRecentlyHit(float minimumTimestamp, float maximumTimestamp)
        {
            PruneOldHits(minimumTimestamp);
            int lastIndex = _hits.FindLastIndex(kvp => kvp.Key < maximumTimestamp);
            return lastIndex >= 0;
        }

        public float GetScore()
        {
            return GetScore(Time.time - _loseGazeDwellTime, Time.time - _gainGazeDwellTime);
        }

        public float GetScore(float minimumTimestamp, float maximumTimestamp)
        {
            PruneOldHits(minimumTimestamp);
            return _hits.Where(kvp => kvp.Key < maximumTimestamp)
                          .Sum(kvp => kvp.Value);
        }

        public void AddHit(float hitTimestamp, float deltaTime)
        {
            _hits.Add(new KeyValuePair<float, float>(hitTimestamp, deltaTime));
        }

        public bool Equals(ScoredObject otherObject)
        {
            return GameObject.GetInstanceID() == otherObject.GameObject.GetInstanceID();
        }

        private void PruneOldHits(float minimumTimestamp)
        {
            _hits.RemoveAll(kvp => kvp.Key < minimumTimestamp);
        }
    }
}