using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningScript : MonoBehaviour {

	public float Speed;


	// Update is called once per frame
	void Update () {
		this.gameObject.transform.eulerAngles = new Vector3 (0, 0, this.gameObject.transform.eulerAngles.z - Speed * Time.deltaTime);
	}
}
