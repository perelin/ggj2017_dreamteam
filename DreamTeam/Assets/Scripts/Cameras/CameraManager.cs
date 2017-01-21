using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{

    public Camera LightnessCamera;
    public Camera LightsCamera;
    public Camera MainCamera;

	// Use this for initialization
	void Start () {
        // flip the cameras for direct X ...
        if ((SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11 ||
            SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D12 ||
            SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D9))
        {
            LightnessCamera.transform.localRotation = Quaternion.Euler(0, 180, 180);
            LightnessCamera.transform.localPosition = new Vector3(0, 0, 10);
            LightsCamera.transform.localRotation = Quaternion.Euler(0, 180, 180);
            LightsCamera.transform.localPosition = new Vector3(0, 0, 10);
        }
        else
        {
            LightnessCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
            LightnessCamera.transform.localPosition = new Vector3(0, 0, -10);
            LightsCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
            LightsCamera.transform.localPosition = new Vector3(0, 0, -10);
        }
    }
	
	// Update is called once per frame
	void Update () {
    }
}
