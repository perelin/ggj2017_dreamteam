using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour {

	public float FishSpeed = 4f;
	public float FishMinX = -14;
	public float FishMaxX = -26;

	public float FishY = -10;

	public float FishWavelength = 8;
	public float FishAmplitude = 2.4f;

	// Update is called once per frame
	void Update () {


		this.gameObject.transform.Translate (new Vector3 (FishSpeed * Time.deltaTime, 0, 0));

		if (this.gameObject.transform.position.x > FishMaxX) {
			this.gameObject.transform.position = new Vector3 (FishMinX, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		}
			
		float x = this.gameObject.transform.position.x;
		float y = FishY + FishAmplitude * Mathf.Sin (x * 2 * Mathf.PI / FishWavelength);

		float dy = FishAmplitude * Mathf.Cos (x * 2 * Mathf.PI / FishWavelength);

		this.gameObject.transform.position = new Vector3 (x, y, this.gameObject.transform.position.z);
		this.gameObject.transform.eulerAngles = new Vector3 (0, 0, dy / FishAmplitude * 45);

	}
}
