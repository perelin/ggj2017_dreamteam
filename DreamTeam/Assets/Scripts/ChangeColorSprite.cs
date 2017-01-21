using UnityEngine;
using Tobii.EyeTracking;

/// <summary>
/// Changes the color of the game object's material, when the the game object 
/// is in focus of the user's eye-gaze.
/// </summary>
/// <remarks>
/// Referenced by the Target game objects in the Simple Gaze Selection example scene.
/// </remarks>
[RequireComponent(typeof(GazeAware))]
[RequireComponent(typeof(SpriteRenderer))]
public class ChangeColorSprite : MonoBehaviour {

    public Color selectionColor;
 
    private GazeAware       _gazeAwareComponent;
    private SpriteRenderer  _spriteRenderer;

    private Color           _deselectionColor;
    private Color           _lerpColor;
    private float           _fadeSpeed = 0.1f;

    /// <summary>
    /// Set the lerp color
    /// </summary>
    void Start()
    {
        _gazeAwareComponent = GetComponent<GazeAware>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _lerpColor = _spriteRenderer.material.color;
        _deselectionColor = Color.white;
    }

    /// <summary>
    /// Lerping the color
    /// </summary>
    void Update ()
    {

        if (_spriteRenderer.material.color != _lerpColor)
        {
            _spriteRenderer.material.color = Color.Lerp(_spriteRenderer.material.color, _lerpColor, _fadeSpeed);
        }

        // Change the color of the cube
        if (_gazeAwareComponent.HasGazeFocus)
        {
            SetLerpColor(selectionColor);
        }
        else
        {
            SetLerpColor(_deselectionColor);
        }
    }

    /// <summary>
    /// Update the color, which should used for the lerping
    /// </summary>
    /// <param name="lerpColor"></param>
    public void SetLerpColor(Color lerpColor)
    {
        this._lerpColor = lerpColor;
    }
}
