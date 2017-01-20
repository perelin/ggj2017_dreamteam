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
		
		if (IsWalking ()) {
			currentProgress += progressPerTime * Time.deltaTime;

			if (currentProgress >= 1) {
				currentProgress = 1;
			}
		}
			
		this.gameObject.transform.position = curve.GetPointAt (currentProgress);
	}
}
