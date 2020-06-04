Shader "MGFramework/UIGradient"
{
	Properties
	{
		_TopColor("TopColor",Color) = (1,1,1,1)
		_BottomColor("BottomColor",Color) = (1,1,1,1)

		_BottomToTop("BottomToTop",int) = 0
		_LeftToRight("LeftToRight", int) = 0
		//_RightBottomToLeftTop("RightBottomToLeftTop", int) = 0
		//_LeftBottomToRightTop("LeftBottomToRightTop", int) = 0

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

		SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass
		{
			CGPROGRAM
				#pragma vertex vert	
				#pragma fragment frag

				#include "UnityCG.cginc"
				#include "UnityUI.cginc"

				#pragma multi_compile __ UNITY_UI_ALPHACLIP

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
			};

			half4 _TopColor;
			half4 _BottomColor;
			int _BottomToTop;
			int _LeftToRight;
			//int _RightBottomToLeftTop;
			//int _LeftBottomToRightTop;
			float4 _ClipRect;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;

#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw - 1.0) * float2(-1 , 1);
#endif
				return OUT;
			}


			fixed4 frag(v2f IN) : SV_Target
			{
				half4 tClr = _TopColor;
				half4 bClr = _BottomColor;

				float x = IN.texcoord.x;
				float y = IN.texcoord.y;

				float t = x * _LeftToRight + y * _BottomToTop;

				half4 color = lerp(bClr, tClr, t);

				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

#ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
#endif
				return color;
			}
			ENDCG
		}
	}
}