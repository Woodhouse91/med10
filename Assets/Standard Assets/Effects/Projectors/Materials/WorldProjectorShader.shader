// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

Shader "Projector/World" {
	Properties{
		_ShadowTex("Cookie", 2D) = "" {}
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
					return float4(texS.r, texS.g, texS.b, 1);
				}
				else
					return float4(0.0, 0.0, 0.0, 0.0);
			}
			ENDCG
		}
	}
}