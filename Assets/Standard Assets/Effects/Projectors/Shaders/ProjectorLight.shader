// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

Shader "Projector/Light" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_ShadowTex ("Cookie", 2D) = "" {}
		_FalloffTex ("FallOff", 2D) = "" {}
		_greenTH("gTH",Range(0.0,1.0)) = 0.5
		_blueTH("bTH",Range(0.0,1.0)) = 0.8
		_redTH	("rTH",Range(0.0,1.0)) = 0.8

		

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
			float _greenTH, _redTH, _blueTH;
			fixed4 frag(v2f i) : SV_Target
			{
				float4 uv = UNITY_PROJ_COORD(i.uvShadow); //239 208 207
				if (uv.x / uv.w > 0.0 && uv.x / uv.w < 1.0 && uv.y / uv.w > 0.0 && uv.y / uv.w < 1.0) {
					
					fixed4 texS = tex2Dproj(_ShadowTex, UNITY_PROJ_COORD(i.uvShadow));
					if (texS.r > _redTH) {
						texS = texS;
					}
					else {
						texS = float4(0.0, 0.0, 0.0, 0.0);
					}
					/*float avg = (texS.r + texS.g + texS.b) / 3.0f;
					if (abs(texS.r-239.0/255.0)>_greenTH && abs(texS.g-208.0/255.0)>_greenTH && abs(texS.b-207.0/255.0)>_greenTH)
					{
						texS = texS;
					}
					else
					{
						texS = float4(0.0, 0.0, 0.0, 0.0);
					}*/
				

					return texS;
				}
				else {
					return float4(0.0, 0.0, 0.0, 0.0);
				}

			}
			ENDCG
		}
	}
}
