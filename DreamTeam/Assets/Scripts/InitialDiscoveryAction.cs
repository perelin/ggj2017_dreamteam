using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialDiscoveryAction : MonoBehaviour {
    public SpriteRenderer SpriteRenderer;
    public float LerpTime;
    public GameObject Tutorial;
    public float TutorialTime;

    void PerformAction()
    {
        Debug.Log("PERFORM");
        var component = gameObject.AddComponent<InitialDiscoveryComponent>();
        component.SpriteRenderer = SpriteRenderer;
        component.LerpTime = LerpTime;
        component.Tutorial = Tutorial;
        component.TutorialTime = TutorialTime;
    }
}
