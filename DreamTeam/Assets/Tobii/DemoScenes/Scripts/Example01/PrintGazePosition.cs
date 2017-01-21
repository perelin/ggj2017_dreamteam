using UnityEngine;
using UnityEngine.UI;
using Tobii.EyeTracking;

/// <summary>
/// Writes the position of the eye gaze point into a UI Text view
/// </summary>
/// <remarks>
/// Referenced by the Data View in the Eye Tracking Data example scene.
/// </remarks>
public class PrintGazePosition : MonoBehaviour
{
    public Text xCoord;
    public Text yCoord;

    void Update()
    {
        Vector2 gazePosition = EyeTracking.GetGazePoint().Screen;

        if (EyeTracking.GetGazePoint().IsValid)
        {
            Vector2 roundedSampleInput = new Vector2(Mathf.RoundToInt(gazePosition.x), Mathf.RoundToInt(gazePosition.y));
            xCoord.text = "x (in px): " + roundedSampleInput.x;
            yCoord.text = "y (in px): " + roundedSampleInput.y;
        }
    }
}
