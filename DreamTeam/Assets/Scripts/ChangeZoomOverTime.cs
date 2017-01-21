using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeZoomOverTime : MonoBehaviour
{

    public float TargetZoom;
    public float LerpTime;
    private float timeElapsed;
    private CameraManager cameraManager;
    private float initialZoom;

    // Use this for initialization
    void Start ()
	{
	    timeElapsed = 0;
	    cameraManager = GetComponent<CameraManager>();
	    initialZoom = cameraManager.Zoom;
        Debug.Log("INITIAL ZOOM: " + initialZoom);

    }

    // Update is called once per frame
    void Update ()
	{
	    timeElapsed += Time.deltaTime;
	    if (timeElapsed > LerpTime)
	    {
            // finish 
	        cameraManager.Zoom = TargetZoom;
	        Destroy(this);
	        return;
	    }

	    cameraManager.Zoom = Mathf.Lerp(initialZoom, TargetZoom, timeElapsed / LerpTime);
        
	}
}
