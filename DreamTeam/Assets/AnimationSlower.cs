using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSlower : MonoBehaviour {

	public float speedFactor;

	void PerformEnter () {
		this.gameObject.GetComponent<Animator> ().speed *= speedFactor;
	}

	void PerformExit () {
		this.gameObject.GetComponent<Animator> ().speed /= speedFactor;
	}
}
