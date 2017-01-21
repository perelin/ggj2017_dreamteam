using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {


	public GameObject camTarget;

	public float camXOffset = 3;

	// Update is called once per frame
	void Update () {
		float camYMovement = camTarget.transform.position.y > -2 ? camTarget.transform.position.y + 2 : 0;

		this.gameObject.transform.position = new Vector3 (camTarget.transform.position.x + camXOffset, camYMovement, 0);
	}
}
