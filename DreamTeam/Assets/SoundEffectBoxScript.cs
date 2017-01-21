using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectBoxScript : MonoBehaviour {

	public AudioClip clip;
	public float deltaTime;


	private float currentDeltaTimeCounter;

	private bool playEffect = false;

	void OnTriggerEnter(Collider other) {
		playEffect = true;
	}

	void OnTriggerExit(Collider other) {
		playEffect = false;
	}

	void Update() {
		if (playEffect) {
			currentDeltaTimeCounter += Time.deltaTime;
			if (currentDeltaTimeCounter > deltaTime) {
				currentDeltaTimeCounter = 0;
				SoundSystem.instance.PlaySingle (clip);
			}
		}
	}
}
