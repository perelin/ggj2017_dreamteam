using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutoAddRenderTexture : MonoBehaviour
{
    public Camera Camera;
    public RenderTexture RenderTexture;
	
	// Update is called once per frame
	void Update () {
	    if (Camera.targetTexture == null)
	    {
	        Camera.targetTexture = RenderTexture;
	    }
	}
}
