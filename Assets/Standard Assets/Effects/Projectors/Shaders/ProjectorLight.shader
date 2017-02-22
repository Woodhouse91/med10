// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

Shader "Projector/Light" {
	Properties {
		
		_ShadowTex ("Cookie", 2D) = "" {}
		
		_TH("Threshold",Range(0.0,1.0)) = 0.8
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
			float4 _keyingColor;
			float _TH, _slope, _Wvalue;
			float4 frag(v2f i) : SV_Target
			{ 
				float4 uv = UNITY_PROJ_COORD(i.uvShadow);
				if (uv.x / uv.w > 0.0 && uv.x / uv.w < 1.0 && uv.y / uv.w > 0.0 && uv.y / uv.w < 1.0) {
					  
					float4 texS = tex2Dproj(_ShadowTex, uv);
					
					float d = abs(length(abs(_keyingColor.rgb - texS.rgb)));
					float edge0 = _TH*(1 - _slope);
					float alpha = smoothstep(edge0, _TH, d);

					/*
					if (texS.g > _greenTH && texS.b < _blueTH && texS.r < _redTH)
					{
						texS = float4(0.0, 0.0, 0.0, 0.0);
					}
					else
					{
						texS.a = 1.00;
					}
					*/

					if (uv.w < _Wvalue)
					{
						return fixed4(1.0, 0.0, 0.0, 1.0);
					}
					else
					{
						return fixed4 (texS.r, texS.g, texS.b ,alpha);
					}
				}
				else {
					return float4(0.0, 0.0, 0.0, 0.0);
				}

			}
			ENDCG
		}
	}
}
