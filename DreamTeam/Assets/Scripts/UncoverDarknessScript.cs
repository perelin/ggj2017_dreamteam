using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncoverDarknessScript : MonoBehaviour {

	public void PerformAction()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Darkness");
    }
}
