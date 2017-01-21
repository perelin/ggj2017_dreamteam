//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using UnityEngine;

namespace Tobii.EyeTracking
{
    public struct GameViewInfo
    {
        public Vector2 Position;
        public Vector2 PixelsPerDesktopPixel;

        public GameViewInfo(Vector2 position, Vector2 pixelsPerDesktopPixel)
        {
            Position = position;
            PixelsPerDesktopPixel = pixelsPerDesktopPixel;
        }

	    public Vector2 DisplaySpaceToUnityScreenSpace(float x, float y)
	    {
			return new Vector2(
						(x - Position.x) * PixelsPerDesktopPixel.x,
						Screen.height - 1 - (y - Position.y) * PixelsPerDesktopPixel.y);
	    }
    }
}