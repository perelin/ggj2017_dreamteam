using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSwimScript : MonoBehaviour {

	public float FishSpeed = 4f;
	public float FishMinX = -14;
	public float FishMaxX = -26;

	public float FishY = -10;

	public float FishWavelength = 8;
	public float FishAmplitude = 2.4f;

	// Update is called once per frame
	void Update () {

		float x = this.gameObject.transform.localPosition.x + FishSpeed * Time.deltaTime;
		this.gameObject.transform.localPosition = new Vector3 (x, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);

		Debug.Log (this.gameObject.transform.localPosition.x);


		if (this.gameObject.transform.localPosition.x > FishMaxX) {
			this.gameObject.transform.localPosition = new Vector3 (FishMinX, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
			x = FishMinX;
		}
			
		float y = FishY + FishAmplitude * Mathf.Sin (x * 2 * Mathf.PI / FishWavelength);

		float dy = FishAmplitude * Mathf.Cos (x * 2 * Mathf.PI / FishWavelength);

		this.gameObject.transform.localPosition = new Vector3 (x, y, this.gameObject.transform.localPosition.z);
		this.gameObject.transform.eulerAngles = new Vector3 (0, 0, dy / FishAmplitude * 45);

	}
}
