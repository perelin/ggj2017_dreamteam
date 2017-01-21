//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using UnityEngine;

namespace Tobii.EyeTracking
{
    [AddComponentMenu("Eye Tracking/Gaze Aware")]
    public class GazeAware : MonoBehaviour, IGazeFocusable
    {
        /// <summary>
        /// True if the user is focusing this object using his or her eye-gaze,
        /// false otherwise.
        /// </summary>
        public bool HasGazeFocus { get; private set; }

        void OnEnable()
        {
            WarnIfAttachedToUIElement();
            GazeFocusHandler().RegisterFocusableComponent(this);
        }

        void OnDisable()
        {
            GazeFocusHandler().UnregisterFocusableComponent(this);
        }

        void Reset()
        {
            WarnIfAttachedToUIElement();
        }

        /// <summary>
        /// Function called from the gaze focus handler when the gaze focus for
        /// this object changes.
        /// </summary>
        /// <remarks>Since the implementation is explicit, it will not be 
        /// visible on instances of this component (unless cast to 
        /// <see cref="IGazeFocusable"/>).
        /// </remarks>
        /// <param name="hasFocus">True if the game object has gaze focus, 
        /// false otherwise.</param>
        void IGazeFocusable.UpdateGazeFocus(bool hasFocus)
        {
            HasGazeFocus = hasFocus;
        }

        /// <summary>
        /// Logs a warning if the Gaze Aware component seems to have been attached
        /// to a UI element (which is not supported).
        /// </summary>
        private void WarnIfAttachedToUIElement()
        {
            if (IsAttachedToUIElement())
            {
                Debug.LogWarning("It seems a Gaze Aware component has been attached to a UI element, which is not supported. Gaze focus can only be detected on 3D and 2D GameObjects with a Collider.");
            }
        }

        private IRegisterGazeFocusable GazeFocusHandler()
        {
            return (IRegisterGazeFocusable)EyeTrackingHost.GetInstance().GazeFocus;
        }

        private bool IsAttachedToUIElement()
        {
            if (GetComponent<RectTransform>() != null)
            {
                return true;
            }

            return false;
        }
    }
}
