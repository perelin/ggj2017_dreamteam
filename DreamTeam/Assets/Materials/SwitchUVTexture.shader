Shader "Custom/SwitchUVTexture" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
			Pass{
			CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag

#include "UnityCG.cginc"

			uniform sampler2D _MainTex;

		float4 frag(v2f_img i) : COLOR{
			float4 c = tex2D(_MainTex, float2(i.uv.x,1-i.uv.y));
		
			return c;
		}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
