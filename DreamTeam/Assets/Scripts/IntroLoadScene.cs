using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroLoadScene : MonoBehaviour
{

    public InitialDiscoveryScript InitialScript;
    public PlayerMovement PlayerMovement;
    public Animator Player;
    public MouseIlluminator MouseIlluminator;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start()
    {

        SceneManager.LoadScene("IntroScene", LoadSceneMode.Additive);
        InitialScript.enabled = false;
        PlayerMovement.enableControls = false;
        MouseIlluminator.enabled = false;
        Player.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FinishedStartScreen()
    {
        InitialScript.enabled = true;
        MouseIlluminator.enabled = true;
        Player.enabled = true;
    }
}
