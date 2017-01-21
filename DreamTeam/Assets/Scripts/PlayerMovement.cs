using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public BezierCurve curve;
	public const float speed = 2f;

	private float currentProgress = 0;


	private float progressPerTime {
		get {
			return (PlayerMovement.speed / curve.length);
		}
	}

	// Use this for initialization
	void Start () {
		if (curve == null) {
			Debug.LogError ("Did not specify BezierCurve for Player");
		}
			
	}
		
	bool IsWalking() {
		return Input.GetKey(KeyCode.Space);
	}

	// Update is called once per frame
	void Update () {

		// check if player wants to move forward
		if (IsWalking ()) {
			currentProgress += progressPerTime * Time.deltaTime;

			if (currentProgress >= 1) {
				currentProgress = 1;
				PlayerWin ();
			}
		}

		// apply new position to player
		Vector3 nextPos = curve.GetPointAt (currentProgress);
		this.gameObject.transform.position = nextPos;
		float scale = 2 - 3 * nextPos.z / 20;
		this.gameObject.transform.localScale = new Vector3 (scale, scale, scale);
	}



	void PlayerWin() {
		// TODO do sth
	}
}
