using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCloseEyes : MonoBehaviour
{

    private Animator _animator;

	// Use this for initialization
	void Start ()
	{
	    _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () { 
		_animator.SetBool("EyesClosed", AreEyesClosed());
	}

    bool AreEyesClosed()
    {
        if (TrackingStuff.isEyeTracking())
        {
            return TrackingStuff.areEyesClosed();
        }
        else
        {
            return Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);
        }
    }

    void StartGame()
    {
        Debug.Log("START SCENE");
        // load the actual game scene
        SceneManager.UnloadSceneAsync("IntroScene");

        GameObject.Find("LoadIntro").GetComponent<IntroLoadScene>().FinishedStartScreen();

    }
}
