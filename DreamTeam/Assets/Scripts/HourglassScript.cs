using System.Collections;
using System.Collections.Generic;
using Tobii.EyeTracking;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class HourglassScript : MonoBehaviour {

    private Animator _animator;
    private bool inside = false;

    public Animator ContentAnimator;

	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
	}

    void Update()
    {
        if (ContentAnimator.enabled && ContentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !ContentAnimator.IsInTransition(0))
        {
            _animator.SetTrigger("stopTrigger");
            _animator.SetBool("done",true);
        }
    } 


    void PerformEnter ()
    {
        _animator.SetTrigger("stopTrigger");
        ContentAnimator.enabled = false;
    }
    void PerformExit ()
    {
        _animator.SetTrigger("restartTrigger");
        ContentAnimator.enabled = true;
    }
}
