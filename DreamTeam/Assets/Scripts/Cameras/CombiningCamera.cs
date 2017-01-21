using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class CombiningCamera : MonoBehaviour {

    private Material material;

    public RenderTexture lightnessTexture;
    public RenderTexture lightsTexture;
    public Material SwitchUVMaterial;
    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("Custom/DarknessShader"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if ((SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11 ||
            SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D12 ||
            SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D9))
        {
            RenderTexture temp = RenderTexture.GetTemporary(1920, 1080);
            RenderTexture temp2 = RenderTexture.GetTemporary(1920, 1080);
            Graphics.Blit(lightsTexture, temp, SwitchUVMaterial);
            Graphics.Blit(lightnessTexture, temp2, SwitchUVMaterial);

            material.SetTexture("_Alpha", temp);
            material.SetTexture("_LightnessTex", temp2);
            Graphics.Blit(source, destination, material);
            RenderTexture.ReleaseTemporary(temp);
            RenderTexture.ReleaseTemporary(temp2);
        }
        else
        {
            material.SetTexture("_Alpha", lightsTexture);
            material.SetTexture("_LightnessTex", lightnessTexture);
            Graphics.Blit(source, destination, material);
        }
        


    }
}
