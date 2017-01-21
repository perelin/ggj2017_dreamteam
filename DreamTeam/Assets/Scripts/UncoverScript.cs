using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]


public class UncoverScript : MonoBehaviour {

    public float Time;

    void PerformAction()
    {
        var go = new GameObject();
        go.transform.SetParent(this.transform, false);
        go.layer = LayerMask.NameToLayer("Darkness");

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = this.GetComponent<SpriteRenderer>().sprite;
        var orig_color = this.GetComponent<SpriteRenderer>().color;
        sr.color = new Color(orig_color.r, orig_color.g, orig_color.b, 0);
        
        var ccot = go.AddComponent<ChangeColorOverTime>();
        ccot.TargetColor = orig_color;
        ccot.LerpTime = Time;
    }
}
