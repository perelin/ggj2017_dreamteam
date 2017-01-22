using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

using Tobii.EyeTracking;

public class MouseIlluminator : MonoBehaviour
{
    public Transform Lights;
    public LightPoint LightPoint;
    private float _count;
    public float SpawnNewLightAfterSeconds = 0.1f;
    public Camera Camera;

	// Use this for initialization
	void OnAwake () {
		EyeTracking.Initialize();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _count += Time.deltaTime;
	    if (_count > SpawnNewLightAfterSeconds)
	    {
	        _count -= SpawnNewLightAfterSeconds;
	        var light = Instantiate(LightPoint, Lights);

	        Vector3 lookPos = TrackingStuff.getTrackingPos();

            var objectPos = Camera.ScreenToWorldPoint(lookPos);

            light.transform.position = objectPos;
	        light.transform.localPosition -= new Vector3(0, 0, light.transform.localPosition.z);

	    }

    }

    
}
