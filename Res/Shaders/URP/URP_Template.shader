Shader "MGFramework/URP/Template"
{
	Properties
	{
		_Color("Color(RGB)",Color) = (1,1,1,1)
		_MainTex("MainTex",2D) = "white"{}
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
			Cull Back
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
				float2 uv :TEXCOORD0;

			};

			Varyings vert(Attributes v)
			{
				Varyings o = (Varyings)0;
				o.uv = TRANSFORM_TEX(v.uv,_MainTex);
				o.positionCS = TransformObjectToHClip(v.positionOS);
				return o;
			}

			half4 frag(Varyings i) : SV_TARGET
			{
				half4 mainTex = SAMPLE_TEXTURE2D(_MainTex,smp,i.uv);
				half4 c = _Color * mainTex;
				return c;
			}

			ENDHLSL
		}
	}
	FallBack "Hidden/Shader Graph/FallbackError"
}