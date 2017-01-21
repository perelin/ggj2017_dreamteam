using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBackgroundScript : MonoBehaviour {
	
	void Start() {
		Vector2 scale = this.gameObject.transform.localScale;
		int width = (int) (scale.x / scale.y);

		this.gameObject.GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (width*3, 3);

	}

}
