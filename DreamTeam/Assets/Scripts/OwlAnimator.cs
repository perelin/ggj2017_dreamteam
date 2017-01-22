using System.Collections;
using System.Collections.Generic;
using Tobii.EyeTracking;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class OwlAnimator : MonoBehaviour {

    private Animator _animator;
    private bool inside = false;


	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
	}


    void PerformEnter ()
    {
        _animator.enabled = true;
    }
    void PerformExit ()
    {
        _animator.enabled = false;
    }
}
