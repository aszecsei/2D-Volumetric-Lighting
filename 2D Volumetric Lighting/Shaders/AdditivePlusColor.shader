Shader "Custom/AdditivePlusColor" {
	Properties{
		_Mul("Multiplier", Range(0, 4)) = .7
	}

		Category{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend SrcAlpha One

		ZWrite Off
		//	ZTest Always
		//	ColorMask RGB

		SubShader{
		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		half _Mul;

	struct appdata_t {
		float4 vertex : POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	float4 _MainTex_ST;

	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.color = v.color;
		o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
		return o;
	}

	//			sampler2D_float _CameraDepthTexture;
	//			float _InvFade;

	fixed4 frag(v2f i) : SV_Target
	{

		return i.color*i.color*_Mul;
	//2.0f * _TintColor + i.color;// * tex2D(_MainTex, i.texcoord)+i.color;
	}
		ENDCG
	}
	}
	}
		FallBack "Diffuse"
}