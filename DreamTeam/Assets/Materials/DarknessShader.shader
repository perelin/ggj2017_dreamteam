Shader "Custom/DarknessShader" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_DarknessTex("Lightness (RGB)", 2D) = "white" {}
		_Alpha("Alpha (A)", 2D) = "white" {}
	}




	SubShader{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }

		ZWrite Off

		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB

		Pass {

			SetTexture[_Alpha]{
				Combine texture
			}
			SetTexture[_LightnessTex]{
				Combine texture, previous
			}

		}
	}
}
