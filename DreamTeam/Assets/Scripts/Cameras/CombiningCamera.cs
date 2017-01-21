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
        material.SetTexture("_Alpha", lightsTexture);
        material.SetTexture("_LightnessTex", lightnessTexture);
        Graphics.Blit(source, destination, material);
    }
}
