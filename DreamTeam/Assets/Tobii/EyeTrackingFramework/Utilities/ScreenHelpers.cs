//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Provides functions related to game view bounds resolution.
    /// </summary>
    public abstract class GameViewBoundsProvider
    {
        protected IntPtr _hwnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameViewBoundsProvider"/> class.
        /// </summary>
        protected GameViewBoundsProvider()
        {
            _hwnd = GetGameViewWindowHandle();
            GameWindowId = _hwnd.ToString();
        }

        /// <summary>
        /// Gets the Window ID for the game window.
        /// </summary>
        public string GameWindowId { get; private set; }

        /// <summary>
        /// Gets the updated Position of the screen view in desktop coordinates (physical pixels).
        /// </summary>
        /// <remarks>This method has to be called from within a single MonoBehavior.Update() call chain.</remarks>
        /// <returns>Position in physical desktop pixels.</returns>
        public Rect GetGameViewPhysicalBounds()
        {
            return LogicalToPhysical(GetGameViewLogicalBounds());
        }

        /// <summary>
        /// Gets the updated Position of the screen view in logical pixels.
        /// </summary>
        /// <remarks>This method has to be called from within a single MonoBehavior.Update() call chain.</remarks>
        /// <returns>Position in logical pixels.</returns>
        protected abstract Rect GetGameViewLogicalBounds();

        /// <summary>
        /// Maps from logical pixels to physical desktop pixels.
        /// </summary>
        /// <param name="rect">Rectangle to be transformed.</param>
        /// <returns>Transformed rectangle.</returns>
        protected virtual Rect LogicalToPhysical(Rect rect)
        {
            var topLeft = new Win32Helpers.POINT { x = (int)rect.x, y = (int)rect.y };
            Win32Helpers.LogicalToPhysicalPoint(_hwnd, ref topLeft);

            var bottomRight = new Win32Helpers.POINT { x = (int)(rect.x + rect.width), y = (int)(rect.y + rect.height) };
            Win32Helpers.LogicalToPhysicalPoint(_hwnd, ref bottomRight);

            return new Rect(topLeft.x, topLeft.y, bottomRight.x - topLeft.x, bottomRight.y - topLeft.y);
        }

        /// <summary>
        /// Finds the window associated with the current thread and process.
        /// </summary>
        /// <returns>A window handle represented as a <see cref="IntPtr"/>.</returns>
        protected virtual IntPtr GetGameViewWindowHandle()
        {
            var processId = Process.GetCurrentProcess().Id;
            return WindowHelpers.FindWindowWithThreadProcessId(processId);
        }

        /// <summary>
        /// Gets the Game View window's top left Position.
        /// </summary>
        /// <remarks>Overridden in test project. Do not remove without updating tests.</remarks>
        protected virtual Vector2 GetWindowPosition()
        {
            var windowPosition = new Win32Helpers.POINT();
            Win32Helpers.ClientToScreen(_hwnd, ref windowPosition);
            return new Vector2(windowPosition.x, windowPosition.y);
        }

        /// <summary>
        /// Gets the Game View window's bottom right corner Position.
        /// </summary>
        /// <remarks>Overridden in test project. Do not remove without updating tests.</remarks>
        protected virtual Vector2 GetWindowBottomRight()
        {
            var clientRect = new Win32Helpers.RECT();
            Win32Helpers.GetClientRect(_hwnd, ref clientRect);

            var bottomRight = new Win32Helpers.POINT { x = clientRect.right, y = clientRect.bottom };
            Win32Helpers.ClientToScreen(_hwnd, ref bottomRight);

            return new Vector2(bottomRight.x, bottomRight.y);
        }

        /// <summary>
        /// Gets the (Unity) screen size.
        /// </summary>
        /// <returns></returns>
        protected virtual Vector2 GetScreenSize()
        {
            return new Vector2(Screen.width, Screen.height);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Gets the Unity toolbar height.
        /// </summary>
        /// <returns></returns>
        protected virtual float GetToolbarHeight()
        {
            try
            {
                return UnityEditor.EditorStyles.toolbar.fixedHeight * 2.0f; // seems only half the physica pixel size is returned by Unity, or that there are two stacked toolbars.
            }
            catch (NullReferenceException)
            {
                return 0f;
            }
        }

        /// <summary>
        /// Gets the Unity game view.
        /// </summary>
        /// <returns></returns>
        protected virtual UnityEditor.EditorWindow GetMainGameView()
        {
            var unityEditorType = Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Diagnostics.Debug.Assert(unityEditorType != null);
            var getMainGameViewMethod = unityEditorType.GetMethod("GetMainGameView", BindingFlags.NonPublic | BindingFlags.Static);
            System.Diagnostics.Debug.Assert(getMainGameViewMethod != null);
            var result = getMainGameViewMethod.Invoke(null, null);
            return (UnityEditor.EditorWindow)result;
        }
#endif
    }

    /// <summary>
    /// Provides utility functions related to screen and window handling within the Unity Player.
    /// </summary>
    public class UnityPlayerGameViewBoundsProvider : GameViewBoundsProvider
    {
        protected override Rect GetGameViewLogicalBounds()
        {
            var topLeft = GetWindowPosition();
            var bottomRight = GetWindowBottomRight();

            var logicalWidth = (int) (bottomRight.x - topLeft.x);
            var logicalHeight = (int) (bottomRight.y - topLeft.y);

            var isTrueFullScreen = (logicalWidth == Screen.width) && (logicalHeight == Screen.height);
            if (Screen.fullScreen && !isTrueFullScreen)
            {
                //Full screen with abnormal settings (aspect ratio or scaling), unity always seems to gazePointVisualisationScale to the physical screen
                float gameAspectRatio = (float)Screen.width / Screen.height;
                float logicalAspectRatio = (float)logicalWidth / logicalHeight;

                if (gameAspectRatio > logicalAspectRatio)
                {
                    //Bars to top and bottom
                    float gameHeightInLogicalPixels = logicalWidth / gameAspectRatio;
                    return new Rect(topLeft.x, topLeft.y + ((logicalHeight - gameHeightInLogicalPixels) / 2.0f), logicalWidth, gameHeightInLogicalPixels);
                }
                else
                {
                    //Bars to left and right
                    float gameWidthInLogicalPixels = logicalHeight * gameAspectRatio;
                    return new Rect(topLeft.x + ((logicalWidth - gameWidthInLogicalPixels) / 2.0f), topLeft.y, gameWidthInLogicalPixels, logicalHeight);
                }
            }
            else
            {
                //Simple full screen where aspect is equal or windowed
                return new Rect(topLeft.x, topLeft.y, logicalWidth, logicalHeight);
            }
        }

        public static Rect GetGameViewLogicalBounds(int screenWidth, int screenHeight, bool screenIsFullScreen,
            Vector2 winTopLeft, Vector2 winBottomRight)
        {
            var logicalWidth = (int)(winBottomRight.x - winTopLeft.x);
            var logicalHeight = (int)(winBottomRight.y - winTopLeft.y);

            var isTrueFullScreen = (logicalWidth == screenWidth) && (logicalHeight == screenHeight);
            if (screenIsFullScreen && !isTrueFullScreen)
            {
                //Full screen with abnormal settings (aspect ratio or scaling), unity always seems to gazePointVisualisationScale to the physical screen
                float gameAspectRatio = (float)screenWidth / screenHeight;
                float logicalAspectRatio = (float)logicalWidth / logicalHeight;

                if (gameAspectRatio > logicalAspectRatio)
                {
                    //Bars to top and bottom
                    float gameHeightInLogicalPixels = logicalWidth / gameAspectRatio;
                    return new Rect(winTopLeft.x, winTopLeft.y + ((logicalHeight - gameHeightInLogicalPixels) / 2.0f), logicalWidth, gameHeightInLogicalPixels);
                }
                else
                {
                    //Bars to left and right
                    float gameWidthInLogicalPixels = logicalHeight * gameAspectRatio;
                    return new Rect(winTopLeft.x + ((logicalWidth - gameWidthInLogicalPixels) / 2.0f), winTopLeft.y, gameWidthInLogicalPixels, logicalHeight);
                }
            }
            else
            {
                //Simple full screen where aspect is equal or windowed
                return new Rect(winTopLeft.x, winTopLeft.y, logicalWidth, logicalHeight);
            }
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// This class is used to resolve the editor game view bounds in 
    /// Unity versions previous to 4.6.
    /// </summary>
    internal class LegacyEditorGameViewBoundsProvider : GameViewBoundsProvider
    {
        private UnityEditor.EditorWindow _gameWindow;
        private bool _initialized;

        private void Initialize()
        {
            _gameWindow = GetMainGameView();
            _initialized = true;
        }

        /// <summary>
        /// Gets the Position of the game view in logical pixels when run from Unity Editor.
        /// </summary>
        /// <returns>The Position of the game view in logical pixels.</returns>
        protected override Rect GetGameViewLogicalBounds()
        {
            if (!_initialized)
            {
                Initialize();
            }

            var gameWindowBounds = _gameWindow.position;

            // Adjust for the toolbar
            var toolbarHeight = GetToolbarHeight();
            gameWindowBounds.y += toolbarHeight;
            gameWindowBounds.height -= toolbarHeight;

            // Get the screen size.
            var screenSize = GetScreenSize();

            // Adjust for unused areas caused by fixed aspect ratio or resolution vs game window size mismatch
            var gameViewOffsetX = (gameWindowBounds.width - screenSize.x) / 2.0f;
            var gameViewOffsetY = (gameWindowBounds.height - screenSize.y) / 2.0f;

            return new Rect(
                gameWindowBounds.x + gameViewOffsetX,
                gameWindowBounds.y + gameViewOffsetY,
                screenSize.x,
                screenSize.y);
        }
    }

    /// <summary>
    /// This class is used to resolve the editor game view bounds for 
    /// Unity version 4.6 and above.
    /// </summary>
    internal class EditorGameViewBoundsProvider : GameViewBoundsProvider
    {
        private float _newHandleTimer = 2.0f;

        /// <summary>
        /// Gets the Position of the game view in logical pixels when run from Unity Editor.
        /// </summary>
        /// <returns>The Position of the game view in logical pixels.</returns>
        protected override Rect GetGameViewLogicalBounds()
        {
            const float handleUpdateIntervalSecs = 2.0f;

            _newHandleTimer += Time.deltaTime;
            if (_newHandleTimer > handleUpdateIntervalSecs)
            {
                //This function costs 0.5ms, so we want to do it as seldom as we can get away with. Caching it will not work though since it can change parents etc. etc.
                _hwnd = GetGameViewWindowHandle();
                _newHandleTimer = 0.0f;
            }

            var topLeft = GetWindowPosition();
            var bottomRight = GetWindowBottomRight();

            var toolbarHeight = GetToolbarHeight();
            return new Rect(topLeft.x, topLeft.y + toolbarHeight, bottomRight.x - topLeft.x, bottomRight.y - topLeft.y - toolbarHeight);
        }

        /// <summary>
        /// Gets the Game View window handle.
        /// </summary>
        /// <remarks>Overridden in test project. Do not remove without updating tests.</remarks>
        protected override IntPtr GetGameViewWindowHandle()
        {
            return WindowHelpers.GetGameViewWindowHandle(Process.GetCurrentProcess().Id);
        }
    }
#endif
}

#endif
