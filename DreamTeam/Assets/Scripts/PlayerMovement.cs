using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float speed = 8f;
    public float walkDamp = 2f;
    //public float stopDamp = 1f;
    public float walkVelo = 0;

	//public BezierCurve[] C;

	public bool enableControls;

	private int currentCurve = 0;
	public float currentProgress = 0;

    public LevelSection[] Sections;
    private int currentSectionId = 0;

    private BezierCurve[] curves {
        get {
            return Sections[currentSectionId].Curves;
        }    
    }
    private BezierCurve curve
    {
        get
        {
            return curves[currentCurve];
        }
    }

    private float progressPerTime {
		get {
			return (speed / curve.length);
		}
	}

	// Use this for initialization
	void Start () {
		if (curves == null || curves.Length < 1) {
			Debug.LogError ("Did not specify BezierCurves for Player");
		}
			
	}
		
	bool IsWalking() {
		return Input.GetKey(KeyCode.Space) && enableControls;
	}

	// Update is called once per frame
	void Update () {

        currentProgress = Mathf.SmoothDamp(currentProgress, IsWalking() ? 1 : currentProgress, ref walkVelo, walkDamp, speed);
        currentProgress = Mathf.Min(currentProgress, 1);

        // if player reached end of currentCurve
        if (currentProgress >= 1 - 0.001)
        {
            NextCurve();
        }

        // apply new position to player
        Vector3 nextPos = curve.GetPointAt (currentProgress);
		this.gameObject.transform.position = nextPos;
		float scale = 2 - 3 * nextPos.z / 20;
		this.gameObject.transform.localScale = new Vector3 (scale, scale, scale);
	}

    void NextCurve()
    {
        // go to next Curve and reset progress
        currentCurve++;
        currentProgress = 0;

        // if there is no curve left
        if (currentCurve >= curves.Length)
        {
            NextLevelSection();
        }
    }


    void NextLevelSection()
    {
        // go to next LevelSection and reset progress
        currentSectionId++;
        currentCurve = 0;

        // if there is no LevelSection left
        if (currentSectionId >= Sections.Length)
        {
            currentSectionId = Sections.Length - 1;
            currentProgress = 1;
            PlayerWin();
        }
    }



    void PlayerWin() {
		// TODO do sth
		Debug.Log("PlayerWin");
	}
}
