using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimationScript : MonoBehaviour {

    void PerformAction()
    {
        var ani = GetComponent<Animator>();
        ani.enabled = true;
    }
}
