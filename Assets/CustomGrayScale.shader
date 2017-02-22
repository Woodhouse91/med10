	Shader "Custom/MultipassTest" {
		Properties{

		}
		SubShader{
			Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" }

			////////////////////////////////////////////////////////
			//Surface Shader 1 - BLUE                             //
			////////////////////////////////////////////////////////
			CGPROGRAM
			#pragma target 3.0
			#pragma surface surf BlinnPhong

			struct Input {
			float4 color : COLOR;
			};

		void surf(Input IN, inout SurfaceOutput o) {
			o.Emission = float3(0,0,1);
		}
		ENDCG


			////////////////////////////////////////////////////////
			//Surface Shader 2 - RED                              //
			////////////////////////////////////////////////////////
			CGPROGRAM
#pragma target 3.0
#pragma surface surf BlinnPhong

			struct Input {
			float4 color : COLOR;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			o.Emission = float3(1,0,0);
		}
		ENDCG


			////////////////////////////////////////////////////////
			//Fixed Functionality shader - GREEN                  //
			////////////////////////////////////////////////////////
			Pass
		{
			Color(0,1,0,1)
		}

			////////////////////////////////////////////////////////
			//Fixed Functionality shader - WHITE                  //
			////////////////////////////////////////////////////////
			Pass{
			CGPROGRAM
			#pragma fragment frag

			fixed4 frag() : COLOR{
			return fixed4(1,1,1,1);
		}
			ENDCG
		}


		}
			Fallback "Diffuse"
	}