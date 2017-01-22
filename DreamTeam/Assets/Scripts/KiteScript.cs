using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteScript : MonoBehaviour {

	public float Speed;

	public GameObject GameObjectToHide;

	private bool doFly = false;

	private Vector3 orig;
	private float x = 0;

	private float sinAmp = 1;
	private float sinWvl = 6;

	void Update () {
		if (doFly) {
			x += Speed * Time.deltaTime;
			float y = sinAmp * Mathf.Sin (x * 2 * Mathf.PI / sinWvl);
			this.gameObject.transform.position = orig + new Vector3 (-x, y, 0);

			float alpha = 0;
			if (x < 2) {
				alpha = (2 - x) / 2;
			}
			Color col = GameObjectToHide.GetComponent<SpriteRenderer> ().color;
			GameObjectToHide.GetComponent<SpriteRenderer> ().color = new Color (col.r, col.g, col.b, alpha);
		}

	}


	void PerformAction() {
		doFly = true;
		orig = this.gameObject.transform.position;
	}
}
