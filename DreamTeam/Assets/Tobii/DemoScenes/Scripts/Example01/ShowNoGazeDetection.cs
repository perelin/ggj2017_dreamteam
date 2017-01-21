using UnityEngine;
using Tobii.EyeTracking;

/// <summary>
/// Enable a set of UI elements if there is no gaze detected
/// </summary>
/// <remarks>
/// Referenced by the No Gaze Tracked visualization in the Eye Tracking Data example scene.
/// </remarks>
public class ShowNoGazeDetection : MonoBehaviour
{
    public GameObject Icon;
    public GameObject Text;

    void Update()
    {
        switch (EyeTrackingHost.GetInstance().GazeTracking.Status)
        {
            case GazeTrackingStatus.GazeNotTracked: // no gaze point data is streaming, inform the user why nothing is happening
                ShowGraphic(true);
                break;

            case GazeTrackingStatus.GazeTracked:    // gaze point data is streaming, no need to show this message
            case GazeTrackingStatus.NotSupported:   // the status is not available on older Tobii Engines, do not show message
            case GazeTrackingStatus.Unknown:        // we don't know what is happening, so let's not say anything :)
            default:
                ShowGraphic(false);
                break;
        }
    }

    private void ShowGraphic(bool isVisible)
    {
        Icon.SetActive(isVisible);
        Text.SetActive(isVisible);
    }
}
