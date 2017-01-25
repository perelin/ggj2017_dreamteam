using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class UncoverDarknessScript : MonoBehaviour {

	public float Time = 2.5f;
	public float alphaFrom = 0.3f;

	public void PerformAction()
    {
		var go = this.gameObject;
		var sr = this.GetComponent<SpriteRenderer> ();

        go.layer = LayerMask.NameToLayer("Darkness");

		var ccot = go.AddComponent<ChangeColorOverTime>();
		ccot.TargetColor = sr.color;
		ccot.LerpTime = Time;

		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, alphaFrom);
    }
}
