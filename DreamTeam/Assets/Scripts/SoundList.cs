using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundList : MonoBehaviour {
	// sound clip lists
	public AudioClip[] clips;

	public void Play() {
		SoundSystem.instance.RandomizeSfx (clips);
	}
}
