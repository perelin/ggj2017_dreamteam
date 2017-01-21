using UnityEngine;
using Tobii.EyeTracking;
using UnityEngine.UI;

/// <summary>
/// Various settings and utilities to manage calibrations and the presence of the user
/// </summary>
/// <remarks>
/// Referenced in the Eye Tracking Settings and Configuration example scene.
/// </remarks>
public class EyeTrackerSettingsMenu : MonoBehaviour
{
    public Dropdown      DropdownProfileSelection;
    public Text          TextViewUserPresenceStatus;
    public Text          TextViewIsUserPresent;
    public Text          TextViewGazeTracking;
    public Text          TextViewIsGazeTracked;
    public Text          TextViewEngineAvailability;
    public Text          TextViewDeviceStatus; 

    private string[]     _names;
    private bool         _isLoaded = false; 

    void Update()
    {
        WriteCalibrationProfilesIntoProfileSelection();

        UpdateUserPresenceView();
        UpdateGazeTrackingView();
        UpdateEngineAvailability();
        UpdateDeviceStatus();
    }

    public void SetCalibrationProfile(int profile)
    {
        EyeTrackingHost.GetInstance().SetCurrentProfile(_names[profile]);
    }

    /// <summary>
    /// Print the User Presence status
    /// </summary>
    private void UpdateUserPresenceView()
    {
        var userPresence = EyeTracking.GetUserPresence().Status;
        TextViewUserPresenceStatus.text = userPresence.ToString();

        if (EyeTracking.GetUserPresence().IsUserPresent)
        {
            TextViewIsUserPresent.text = "Yes";
        }
        else
        {
            TextViewIsUserPresent.text = "No";
        }
    }

    /// <summary>
    /// Print the Gaze Tracking status 
    /// </summary>
    private void UpdateGazeTrackingView()
    {
        var gazeTracking = EyeTracking.GetGazeTrackingStatus().Status;
        TextViewGazeTracking.text = gazeTracking.ToString();

        if (EyeTracking.GetGazeTrackingStatus().IsTrackingEyeGaze)
        {
            TextViewIsGazeTracked.text = "Yes";
        }
        else
        {
            TextViewIsGazeTracked.text = "No";
        }
    }

    private void UpdateEngineAvailability()
    {
        TextViewEngineAvailability.text = EyeTrackingHost.TobiiEngineAvailability.ToString();
    }

    /// <summary>
    /// Print the Eye Tracking Device Status
    /// </summary>
    private void UpdateDeviceStatus()
    {
        TextViewDeviceStatus.text = EyeTrackingHost.GetInstance().EyeTrackingDeviceStatus.ToString();
    }

    private void WriteCalibrationProfilesIntoProfileSelection()
    {
        IStateValue<string[]> profiles = EyeTrackingHost.GetInstance().UserProfileNames;
        IStateValue<string> selectedProfile = EyeTrackingHost.GetInstance().UserProfileName;

        if (profiles.IsValid && _isLoaded == false)
        {
            DropdownProfileSelection.options.Clear();
            _names = new string[profiles.Value.Length];

            for (int i = 0; i < profiles.Value.Length; i++)
            {
                _names[i] = profiles.Value[i];

                Dropdown.OptionData option = new Dropdown.OptionData(_names[i]);
                DropdownProfileSelection.options.Add(option);

                if (_names[i].Equals(selectedProfile.Value))
                {
                    DropdownProfileSelection.value = i;
                }
            }

            _isLoaded = true;
        }
    }
}
