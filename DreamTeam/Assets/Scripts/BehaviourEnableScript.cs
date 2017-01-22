using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourEnableScript : MonoBehaviour {

    public Behaviour[] components;

	void PerformAction()
    {
        foreach (var bhv in components)
        {
            bhv.enabled = true;
        }
    }
}