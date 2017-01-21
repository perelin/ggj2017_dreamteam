using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CombiningCamera : MonoBehaviour {

    private Material material;

    public RenderTexture lightnessTexture;
    public RenderTexture lightsTexture;

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("Custom/DarknessShader"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture temp = RenderTexture.GetTemporary(1920, 1080);
        RenderTexture temp2 = RenderTexture.GetTemporary(1920, 1080);
        Graphics.Blit(lightsTexture, temp);
        Graphics.Blit(lightnessTexture, temp2);

        material.SetTexture("_Alpha", temp);
        material.SetTexture("_LightnessTex", temp2);
        Graphics.Blit(source, destination, material);
        RenderTexture.ReleaseTemporary(temp);
        RenderTexture.ReleaseTemporary(temp2);


    }
}
