using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialDiscoveryScript : MonoBehaviour
{

    public float TimeToAction;
    public SpriteRenderer SpriteRenderer;
    public float LerpTime;
    public GameObject Tutorial;
    public float TutorialTime;

    // Use this for initialization
    void Start ()
	{
        SpriteRenderer.color = new Color(0, 0, 0, 0);

        var discoveryScript = gameObject.AddComponent<DiscoveryScript>();
        discoveryScript.timeToAction = TimeToAction;

        var discoveryAction = gameObject.AddComponent<InitialDiscoveryAction>();
        discoveryAction.LerpTime = LerpTime;
        discoveryAction.SpriteRenderer = SpriteRenderer;
        discoveryAction.Tutorial = Tutorial;
        discoveryAction.TutorialTime = TutorialTime;
        discoveryScript.actionToPerform = discoveryAction;
        Destroy(this);
    }
	
	// Update is called once per frame
	void Update () {

 

    }
}
