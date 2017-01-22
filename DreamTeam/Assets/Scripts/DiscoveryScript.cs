using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class DiscoveryScript : MonoBehaviour {

	// Determines how fast to count down up countdown
	public float timeSubstractFactor = 1 / 2f;
	public float timeToAction = 2f;

	public MonoBehaviour actionToPerform;

	private float currentTimer = 0;

	Ray ray;
	RaycastHit2D hit;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		ray = Camera.main.ScreenPointToRay(TrackingStuff.getTrackingPos());
		hit = Physics2D.Raycast(ray.origin, ray.direction);

		// if collision: increment timer
		if (hit.collider != null && hit.collider == this.gameObject.GetComponent<Collider2D> ()) {
			currentTimer += Time.deltaTime;
			if (currentTimer > timeToAction) {
				DoAction ();
				Object.Destroy(this);
			}

		// otherwise decrease
		} else if (currentTimer > 0) {
			currentTimer -= timeSubstractFactor * Time.deltaTime;

		}

		// reset timer when negative
		if (currentTimer < 0) {
			currentTimer = 0;
		}
	}

	public void DoAction() {
		actionToPerform.SendMessage("PerformAction");
	}
}
