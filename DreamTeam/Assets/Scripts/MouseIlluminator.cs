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

    public bool UseEyeTracker = true;

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

	        Vector3 lookPos = Vector3.zero;

	        if (UseEyeTracker)
	        {
                // get eye tracking point
                GazePoint gazePoint = EyeTracking.GetGazePoint();
                if (gazePoint.IsValid)
                {
                    lookPos = new Vector3(gazePoint.Screen.x, gazePoint.Screen.y);
                }
            }
            else
	        {
                // get mouse position as replacement for eye tracking
	            lookPos = Input.mousePosition;
	            
	        }

            var objectPos = Camera.ScreenToWorldPoint(lookPos);

            light.transform.position = objectPos;
	        light.transform.localPosition -= new Vector3(0, 0, light.transform.localPosition.z);

	    }

    }
}
