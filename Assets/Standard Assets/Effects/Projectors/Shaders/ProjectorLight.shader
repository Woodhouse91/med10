// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

Shader "Projector/Light" {
	Properties {
		
		_ShadowTex ("Cookie", 2D) = "" {}
		_THa("Threshold_a",Range(0.0,1.0)) = 0.8
		_THb("Threshold_b",Range(0.0,1.0)) = 0.8
		_slope("Slope",Range(0.0,1.0)) = 0.2
		_keyingColor("Keying Color", Color) = (1,1,1,1)
		_Wvalue("W value changer", Range(0.0,1.0)) = 1.0
	}
	
	Subshader {
		Tags {"Queue"="Transparent"}
		Pass {
			ZWrite Off
			ColorMask RGB
		Blend  DstAlpha OneMinusSrcAlpha
			Offset -1, -1
	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			
			struct v2f {
				float4 uvShadow : TEXCOORD0;
				float4 uvFalloff : TEXCOORD1;
				UNITY_FOG_COORDS(2)
				float4 pos : SV_POSITION;
			};
			
			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;
			v2f vert (float4 vertex : POSITION)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, vertex);
				o.uvShadow = mul (unity_Projector, vertex);
				o.uvFalloff = mul (unity_ProjectorClip, vertex);
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			
			fixed4 _Color;
			sampler2D _ShadowTex;
			sampler2D _FalloffTex;
			float4 _keyingColor, _bg = float4(0.0, 0.0, 0.0, 0.0);
			float _THa, _THb, _slope, _Wvalue;
			float4 frag(v2f i) : SV_Target
			{ 
				float4 uv = UNITY_PROJ_COORD(i.uvShadow);
				float4 texS = tex2Dproj(_ShadowTex, uv);
				if (uv.x / uv.w > 0.0 && uv.x / uv.w < 1.0 && uv.y / uv.w > 0.0 && uv.y / uv.w < 1.0) {
					/*float y = 0.299*texS.r + 0.587*texS.g + 0.114*texS.b;*/
					float Cb = 0.5 - 0.168736*texS.r - 0.331264*texS.g + 0.5*texS.b;
					float Cr = 0.5 + 0.5*texS.r - 0.418688*texS.g - 0.081312*texS.b;
				/*	float y_key = 0.299*_keyingColor.r + 0.587*_keyingColor.g + 0.114*_keyingColor.b;*/
					float Cb_key = 0.5 - 0.168736*_keyingColor.r - 0.331264*_keyingColor.g + 0.5*_keyingColor.b;
					float Cr_key = 0.5 + 0.5*_keyingColor.r - 0.418688*_keyingColor.g - 0.081312*_keyingColor.b;
					float temp = sqrt((Cb_key - Cb)*(Cb_key - Cb) + (Cr_key - Cr)*(Cr_key - Cr));
					float mask = 0.0;
					if (temp < _THa)
						mask = 0.0;
					else if (temp < _THb)
						mask = (temp - _THa) / (_THb - _THa);
					else
						mask = 1.0;
					//mask = 1.0 - mask;
					float red = max(texS.r - mask*_keyingColor.r, 0);
					float green = max(texS.g - mask*_keyingColor.g, 0);
					float blue = max(texS.b - mask*_keyingColor.b, 0);
					//return float4(red, green, blue, 1);
					return float4(texS.r * mask, texS.g * mask, texS.b* mask, mask);
				}
				else {
					return float4(0.0, 0.0, 0.0, 0.0);
				}

			}
			ENDCG
		}
	}
}
