using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTurnScript : MonoBehaviour {
	
	public Vector3 middle;
	public float radius;

	public float progressSpeed;

	private float currentProgress;
	
	// Update is called once per frame
	void Update () {

		currentProgress += progressSpeed * Time.deltaTime;

		float inner = currentProgress * 2 * Mathf.PI;

		this.gameObject.transform.localPosition = middle + new Vector3 (-radius * Mathf.Sin(inner), radius * Mathf.Cos(inner), 0);
		this.gameObject.transform.localEulerAngles = new Vector3 (0, 0, currentProgress*360+180);

	}
}
