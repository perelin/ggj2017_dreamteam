using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBoxChangeCameraZoom : MonoBehaviour
{
    public CameraManager CameraManager;
    public float Zoom;
    public float Time;
    void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Player"))
        {
            ChangeZoomOverTime zoomScript = CameraManager.gameObject.AddComponent<ChangeZoomOverTime>();
            zoomScript.TargetZoom = Zoom;
            zoomScript.LerpTime = Time;
        }
    }

}
