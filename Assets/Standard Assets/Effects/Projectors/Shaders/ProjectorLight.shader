// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

Shader "Projector/Light" {
	Properties{
		_ShadowTex("Cookie", 2D) = "" {}
		_THa("Threshold_a",Range(0.0,1.0)) = 0.8
		_THb("Threshold_b",Range(0.0,1.0)) = 0.8
		_keyingColor("Keying Color", Color) = (1,1,1,1)
			
	}

	Subshader
	{
		Tags{ "Queue" = "Transparent" }

		Pass
		{
			ZWrite Off
			ColorMask RGB
			Blend  DstAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _ShadowTex;
			float4 _keyingColor;
			float _THa, _THb;
			float4x4 unity_Projector;

			struct v2f {
				float4 uvShadow : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			v2f vert(float4 vertex : POSITION)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				o.uvShadow = mul(unity_Projector, vertex);
				return o;
			}


			float4 frag(v2f i) : SV_Target
			{
				float4 uv = UNITY_PROJ_COORD(i.uvShadow);
				float4 texS = tex2Dproj(_ShadowTex, uv);
				if (uv.x / uv.w > 0.0 && uv.x / uv.w < 1.0 && uv.y / uv.w > 0.0 && uv.y / uv.w < 1.0) 
				{
					float Cb = 0.5 - 0.168736*texS.r - 0.331264*texS.g + 0.5*texS.b;
					float Cr = 0.5 + 0.5*texS.r - 0.418688*texS.g - 0.081312*texS.b;
					float Cb_key = 0.5 - 0.168736*_keyingColor.r - 0.331264*_keyingColor.g + 0.5*_keyingColor.b;
					float Cr_key = 0.5 + 0.5*_keyingColor.r - 0.418688*_keyingColor.g - 0.081312*_keyingColor.b;
					float temp = sqrt((Cb_key - Cb)*(Cb_key - Cb) + (Cr_key - Cr)*(Cr_key - Cr));
					float mask = 0.0;
					if (temp < _THa)
						mask = 0.0;
					else if (temp < _THb) //Handle "unknown" by giving it partial alpha
						mask = (temp - _THa) / (_THb - _THa);
					else
						mask = 1.0;
					return float4(texS.r * mask, texS.g * mask, texS.b* mask, mask);
				}
				else
					return float4(0.0, 0.0, 0.0, 0.0);
			}
			ENDCG
		}
	}
}