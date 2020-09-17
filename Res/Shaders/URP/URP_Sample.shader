Shader "MGFramework/URP/Sample"
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
			// 渲染管线标记，对应的管线C#代码UniversalRenderPipeline.cs中的
			// Shader.globalRenderPipeline = UniversalPipeline,LightweightPipeline,
			// 只有带有UniversalPipeline或LightweightPipeline的Tag的SubShader才会生效.
			// 主要作用是用于标记当前这个SubShader是属于哪个管线下的.
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
	
			// Render State
			Blend One Zero, One Zero
			Cull Back
			ZTest LEqual
			ZWrite On
			//在OpenGL ES2.0中使用HLSLcc编译器,目前除了OpenGL ES2.0全都默认使用HLSLcc编译器.
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0
			#pragma multi_compile_instancing
	
			// Includes
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
	
			// 常量缓冲区的定义，GPU现在里面某一块区域，这一块区域可以非常快速的和GPU传输数据
			// 因为要占GPU显存，所以其容量不是很大。
			// CBUFFER_START = 常量缓冲区的开始，CBUFFER_END = 常量缓冲区的结束。
			// UnityPerMaterial = 每一个材质球都用这一个Cbuffer，凡是在Properties里面定义的数据(Texture除外)，
			// 都需要在常量缓冲区进行声明，并且都用这一个Cbuffer，通过这些操作可以享受到SRP的合批功能
			CBUFFER_START(UnityPerMaterial)
			// 常量缓冲区所填充的内容
			half4 _Color;
			CBUFFER_END

			// 纹理与采样器的分离定义:
			// 采样器和纹理分开定义的好处是什么？
			// 内置管线的纹理的定义是和采样器的定义绑定的，采样器的定义是有限制的。
			// 在URP中纹理的定义和采样器的定义是分离的，假如声明了很多个纹理，可以都用这一个采样器
			TEXTURE2D(_MainTex);//纹理的定义，如果是编译到GLES2.0平台，相当于samler2D _MainTex;否则TEXTURE2D _MainTex
			float4 _MainTex_ST;
			half4 _MainTex_TexelSize;
			// 采样器的定义(纹理与采样器分离定义),采样器是指纹理的过滤模式与重复模式,此功能在OpenGL ES2.0上不支持，相当于没写.
			// 1.SAMPLER(sampler_textureName):sampler+纹理名称，这种定义形式是表示采用textureName这个纹理Inspector面板中的采样方式.
			// 2.SAMPLER(_filter_wrap):比如SAMPLER(point_clamp),使用自定义的采样器设置，自定义的采样器一定要同时包含过滤模式<filter>与重复模式<wrap>的设置.
			// 3.SAMPLER(_filter_wrapU_wrapV):比如SAMPLER(linear_clampU_mirrorV),可同时设置重复模式的U与V的不同值.
			// 4.filter:point/linear/triLinear
			// 5.wrap:clamp/repeat/mirror/mirrorOnce

			//为了方便操作 定义预定义
			#define smp SamplerState_Point_Repeat
			// SAMPLER(sampler_MainTex); 默认采样器
			SAMPLER(smp);
			// 顶点着色器的输入
			struct Attributes
			{
				float3 positionOS : POSITION;
				float2 uv :TEXCOORD0;
			};

			// 顶点着色器的输出
			struct Varyings
			{
				float4 positionCS : SV_POSITION;
				float2 uv :TEXCOORD0;

			};


			// 顶点着色器
			Varyings vert(Attributes v)
			{
				Varyings o = (Varyings)0;
				o.uv = TRANSFORM_TEX(v.uv,_MainTex);
				o.positionCS = TransformObjectToHClip(v.positionOS);
				return o;
			}

			// 片段着色器
			half4 frag(Varyings i) : SV_TARGET
			{
				//进行纹理采样 SAMPLE_TEXTURE2D(纹理名，采样器名，uv)
				half4 mainTex = SAMPLE_TEXTURE2D(_MainTex,smp,i.uv);
				half4 c = _Color * mainTex;
				return c;
			}

			ENDHLSL
		}
	}
	FallBack "Hidden/Shader Graph/FallbackError"
}