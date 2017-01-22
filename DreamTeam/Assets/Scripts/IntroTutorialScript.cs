using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTutorialScript : MonoBehaviour
{

    public PlayerMovement PlayerMovement;

	// Use this for initialization
	void Start ()
	{
	    PlayerMovement.enableControls = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKey(KeyCode.Space))
	    {
	        GetComponent<Animator>().SetTrigger("Finish");
	    }
	}

    void RemoveTutorial()
    {
        gameObject.SetActive(false);
    }
}
