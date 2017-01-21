using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectBoxScript : MonoBehaviour {

	public SoundList list;
	public float deltaTime;


	private float currentDeltaTimeCounter;

	private bool playEffect = false;

	void OnTriggerEnter(Collider other) {
		if (other.name.Equals("Player")) {
			playEffect = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.name.Equals ("Player")) {
			playEffect = false;
		}
	}

	void Update() {
		if (playEffect) {
			currentDeltaTimeCounter += Time.deltaTime;
			if (currentDeltaTimeCounter > deltaTime) {
				currentDeltaTimeCounter = 0;
				list.Play ();
			}
		}
	}
}
