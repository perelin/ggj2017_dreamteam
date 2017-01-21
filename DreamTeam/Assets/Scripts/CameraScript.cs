using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {


	public GameObject camTarget;

	private float camXOffset;


	void Start () {
		camXOffset = 3;
	}

	// Update is called once per frame
	void LateUpdate () {
		this.gameObject.transform.position = new Vector3 (camTarget.transform.position.x + 3, 0, 0);
	}
}
