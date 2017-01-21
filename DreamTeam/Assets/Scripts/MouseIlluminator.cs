using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseIlluminator : MonoBehaviour
{
    public Transform Lights;
    public LightPoint LightPoint;
    private float _count;
    public float SpawnNewLightAfterSeconds = 0.1f;
    public Camera Camera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _count += Time.deltaTime;
	    if (_count > SpawnNewLightAfterSeconds)
	    {
	        _count -= SpawnNewLightAfterSeconds;
	        var light = Instantiate(LightPoint, Lights);
            var mousePos = Input.mousePosition;
	        var objectPos = Camera.ScreenToWorldPoint(mousePos);
	        light.transform.position = objectPos;

	    }

    }
}
