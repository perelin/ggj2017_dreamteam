#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine;
using NUnit.Framework;

namespace Tobii.EyeTracking.Tests
{
    [TestFixture]
    public sealed class UnityPlayerGameViewBoundsTest
    {
        /**
          * Terminology in test case description and corresponding meaning in production code:
          * - windowPos = top left window corner in Windows' logical pixels 
          * - gameAspect = game view (area excluding black bars) aspect ratio = Screen.width / Screen.height
          * - logicalAspect = game window aspect ratio in logical pixels (calculated using Win32 API: ClientToScreen(topLeft,...) and GetClientRect(...))
          */
        [TestCase(false, 1200, 675, 
                  0f, 0f, 1200f, 675f, 
                  0f, 0f, 1200f, 675f,
                  Description = "windowed,   gameAspect == logicalAspect, windowPos = top left primary")]
        [TestCase(false, 1200, 675, 
                  200f, 50f, 1400f, 725f, 
                  200f, 50f, 1200f, 675f,
                  Description = "windowed,   gameAspect == logicalAspect, windowPos = offset from top left primary")]
        [TestCase(true, 1600, 900, 
                  0f, 0f, 1600f, 900f, 
                  0f, 0f, 1600f, 900f,
                  Description = "fullscreen, gameAspect == logicalAspect, primary screen")]
        [TestCase(true, 1600, 900, 
                  1200f, 50f, 2800f, 950f, 
                  1200f, 50f, 1600f, 900f,
                  Description = "fullscreen, gameAspect == logicalAspect, non-primary screen")]
        [TestCase(true, 1600, 900, 
                  0f, 0f, 1600f, 900f, 
                  0f, 0f, 1600f, 900f,
                  Description = "fullscreen, gameAspect == logicalAspect, primary screen")]
        [TestCase(true, 1600, 900, 
                  0f, 0f, 1200f, 900f, 
                  0f, 112.5f, 1200f, 675f,
                  Description = "fullscreen, gameAspect  > logicalAspect, primary screen")]
        [TestCase(true, 1600, 900, 
                  0f, 0f, 900f, 1200f, 
                  0f, 346.875f, 900f, 506.25f,
                  Description = "fullscreen, gameAspect  > logicalAspect, primary screen, logicalAspect = portrait")]
        [TestCase(true, 1600, 900, 
                  1200f, 50f, 2400f, 950f, 
                  1200f, 162.5f, 1200f, 675f,
                  Description = "fullscreen, gameAspect  > logicalAspect, non-primary screen")]
        [TestCase(true, 1200, 900, 
                  0f, 0f, 1600f, 900f, 
                  200f, 0f, 1200f, 900f,
                  Description = "fullscreen, gameAspect  < logicalAspect, primary screen")]
        [TestCase(true, 900, 1200, 
                  0f, 0f, 1600f, 900f, 
                  462.5f, 0f, 675f, 900f,
                  Description = "fullscreen, gameAspect  < logicalAspect, non-primary screen")]
        [TestCase(true, 1200, 900, 
                  1200f, 50f, 2800f, 950f, 
                  1400f, 50f, 1200f, 900f,
                  Description = "fullscreen, gameAspect  < logicalAspect, primary screen, gameAspect = portrait")]
        public void Should_Return_Correct_Game_View_Bounds(
            bool isFullscreen, int screenWidth, int screenHeight,
            float winPosX, float winPosY, float winBottomRightX, float winBottomRightY,
            float expectedX, float expectedY, float expectedWidth, float expectedHeight)
        {
            var winTopLeft = new Vector2(winPosX, winPosY);
            var winBottomRight = new Vector2(winBottomRightX, winBottomRightY);

            Rect result = UnityPlayerGameViewBoundsProvider.GetGameViewLogicalBounds(screenWidth, screenHeight, isFullscreen, winTopLeft, winBottomRight);

            Assert.AreEqual(expectedX, result.x);
            Assert.AreEqual(expectedY, result.y);
            Assert.AreEqual(expectedWidth, result.width);
            Assert.AreEqual(expectedHeight, result.height);
        }
    }
}
#endif
