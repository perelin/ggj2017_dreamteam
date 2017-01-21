using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderToFullscreenTexture : MonoBehaviour
{

    private int _lastWidth;
    private int _lastHeight;

    private Camera camera;
    public Camera MainCamera;
    public SpriteRenderer RenderTo;
    
    void Start()
    {
        _lastWidth = MainCamera.pixelWidth;
        _lastHeight = MainCamera.pixelHeight;
        camera = GetComponent<Camera>();
    }

    void Update()
    {

        Debug.Log(MainCamera.pixelWidth);
        Debug.Log("update");
        if (_lastWidth != MainCamera.pixelWidth || _lastHeight != MainCamera.pixelHeight)
        {
            _lastWidth = MainCamera.pixelWidth;
            _lastHeight = MainCamera.pixelHeight;

            // resultion changed
            Debug.Log("changed resolution");

            if (camera.targetTexture != null)
            {
                camera.targetTexture.Release();
            }
            RenderTexture texture = new RenderTexture(Screen.width, Screen.height, 24);
            camera.targetTexture = texture;
        }

    }
}
