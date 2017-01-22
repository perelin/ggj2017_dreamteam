using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxScript : MonoBehaviour {

	public int changeMusicTo;

	void OnTriggerEnter(Collider other) {
		if (other.name.Equals("Player")) {
			SoundSystem.instance.ChangeMusic(changeMusicTo);
		}
	}

}
