using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public const float speed = 4f;

	public BezierCurve[] curves;

	public bool enableControls;

	private int currentCurve = 0;
	private float currentProgress = 0;



	private float progressPerTime {
		get {
			return (PlayerMovement.speed / curves[currentCurve].length);
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

		// check if player wants to move forward
		if (IsWalking ()) {
			currentProgress += progressPerTime * Time.deltaTime;

			// if player reached end of currentCurve
			if (currentProgress >= 1) {

				// go to next Curve and reset progress
				currentCurve++;
				currentProgress = 0;

				// if there is no curve left
				if (currentCurve >= curves.Length) {
					PlayerWin ();
					currentCurve = curves.Length - 1;
					currentProgress = 1;
				}
			}
		}

		// apply new position to player
		Vector3 nextPos = curves[currentCurve].GetPointAt (currentProgress);
		this.gameObject.transform.position = nextPos;
		float scale = 2 - 3 * nextPos.z / 20;
		this.gameObject.transform.localScale = new Vector3 (scale, scale, scale);
	}



	void PlayerWin() {
		// TODO do sth
		Debug.Log("PlayerWin");
	}
}
