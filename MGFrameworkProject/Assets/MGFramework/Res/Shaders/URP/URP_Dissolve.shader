Shader "MGFramework/URP/Dissolve"
{
	Properties
	{
		_Color("Color(RGB)",Color) = (1,1,1,1)
		_MainTex("MainTex",2D) = "white"{}
		_NoiseTex("NoiseTex",2D) = "bump"{}
		_Threshold("Threshold",Range(0,1)) = 0.5
		_EdgeLength("Edge Length",Range(0,0.2)) = 0.1
		_EdgeColor("Edge Color",Color) = (1,1,1,1)
	}
		SubShader
		{
			Tags
			{
				"RenderPipeline" = "UniversalPipeline"
				"RenderType" = "Opaque"
				"Queue" = "Geometry+0"
			}

			Pass
			{
				Name "Pass"
				Tags
				{

				}

				Blend One Zero, One Zero
				Cull Off
				ZTest LEqual
				ZWrite On

				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma prefer_hlslcc gles
				#pragma exclude_renderers d3d11_9x
				#pragma target 2.0
				#pragma multi_compile_instancing

				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
				#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

				CBUFFER_START(UnityPerMaterial)
				half4 _Color;
				CBUFFER_END

				TEXTURE2D(_MainTex);
				float4 _MainTex_ST;
				half4 _MainTex_TexelSize;
				
				TEXTURE2D(_NoiseTex);
				float4 _NoiseTex_ST;

				half _Threshold;
				half _EdgeLength;
				half4 _EdgeColor;
					
				#define smp SamplerState_Point_Repeat
				SAMPLER(smp);

				struct Attributes
				{
					float3 positionOS : POSITION;
					float2 uv :TEXCOORD0;
				};

				struct Varyings
				{
					float4 positionCS : SV_POSITION;
					float2 main_uv :TEXCOORD0;
					float2 noise_uv :TEXCOORD1;
				};

				Varyings vert(Attributes v)
				{
					Varyings o = (Varyings)0;
					o.main_uv = TRANSFORM_TEX(v.uv,_MainTex);
					o.noise_uv = TRANSFORM_TEX(v.uv,_NoiseTex);
					o.positionCS = TransformObjectToHClip(v.positionOS);
					return o;
				}

				half4 frag(Varyings i) : SV_TARGET
				{
					half4 mainTex = SAMPLE_TEXTURE2D(_MainTex,smp,i.main_uv);
					half r =  SAMPLE_TEXTURE2D(_NoiseTex,smp,i.noise_uv).r;

					half delta = r - _Threshold;

					clip( delta );
					
					int edgeFlag = step ( _Threshold , 0 );
					int edge = step( r - _Threshold , _EdgeLength ) * (1 - edgeFlag);
					
					half4 clr = _Color * mainTex;
					
					clr.rgb  = _EdgeColor.rgb * edge + clr.rgb * (1 - edge);

					return clr;
				}

				ENDHLSL
			}
		}

		FallBack "Hidden/Shader Graph/FallbackError"
}