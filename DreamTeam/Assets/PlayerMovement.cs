using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public BezierCurve curve;

	// Use this for initialization
	void Start () {
		
		if (curve == null) {
			Debug.LogError ("Did not specify BezierCurve for Player");
		}
	}


	private float progress = 0;
	// Update is called once per frame
	void Update () {
		progress = progress <= 1 ? progress + 0.01f : 0;

		this.gameObject.transform.position = curve.GetPointAt (progress);


	}
}
