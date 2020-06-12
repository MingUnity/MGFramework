Shader "MGFramework/UIRoundRect"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0

		_RoundedRadius("Rounded Radius", Float) = 8
		_Width("Width" , Float) = 100
		_Height("Height" , Float) = 100
		_LeftTop("LeftTop" , int) = 1
		_RightTop("RightTop" , int) = 1
		_LeftBottom("LeftBottom" , int) = 1
		_RightBottom("RightBottom" , int) = 1
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
					float4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					half2 texcoord : TEXCOORD0;
					float4 worldPosition : TEXCOORD1;
				};

				fixed4 _Color;
				fixed4 _TextureSampleAdd;
				float4 _ClipRect;
				float _RoundedRadius;
				float4 _MainTex_TexelSize;
				float _Width;
				float _Height;
				int _LeftTop;
				int _RightTop;
				int	_LeftBottom;
				int	_RightBottom;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.worldPosition = IN.vertex;
					OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

					OUT.texcoord = IN.texcoord;

	#ifdef UNITY_HALF_TEXEL_OFFSET
					OUT.vertex.xy += (_ScreenParams.zw - 1.0) * float2(-1 , 1);
	#endif

					OUT.color = IN.color * _Color;
					return OUT;
				}

				sampler2D _MainTex;

				fixed4 frag(v2f IN) : SV_Target
				{
					half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

					color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

	#ifdef UNITY_UI_ALPHACLIP
					clip(color.a - 0.001);
	#endif

					//切圆角+抗锯齿
					float width = _Width;
					float height = _Height;

					float x = IN.texcoord.x * width;
					float y = IN.texcoord.y * height;

					float r = _RoundedRadius;

					float antiAliasingVal = _RoundedRadius * 1.2;

					//水平垂直坐标对比半径
					float xrVal = step(x , r);
					float yrVal = step(y , r);
					float yhrVal = step(y , height - r);
					float xwrVal = step(x , width - r);

					//左下角距离参数
					float lb_dis = (x - r) * (x - r) + (y - r) * (y - r);

					//左上角距离参数
					float lt_dis = (x - r) * (x - r) + (y - (height - r)) * (y - (height - r));

					//右下角距离参数
					float rb_dis = (x - (width - r)) * (x - (width - r)) + (y - r) * (y - r);

					//右上角距离参数
					float rt_dis = (x - (width - r)) * (x - (width - r)) + (y - (height - r)) * (y - (height - r));

					//像素点所在位置距离
					float dis = _LeftBottom * xrVal * yrVal * lb_dis +
								_LeftTop * xrVal * (1 - yhrVal) * lt_dis +
								_RightBottom * (1 - xwrVal) * yrVal * rb_dis +
								_RightTop * (1 - xwrVal) * (1 - yhrVal) * rt_dis;

					//标准边缘的距离对比
					float low = step(dis, r * r - 3 * antiAliasingVal);
					float nor = step(dis, r * r - 2 * antiAliasingVal);
					float high = step(dis, r * r - antiAliasingVal);
					float max = step(dis, r * r);

					//赋值透明度渐变像素(圆角+抗锯齿)
					color.a = (nor * (1 - low) * 0.8 + (1 - nor) * high * 0.5 + (1 - high) * max * 0.2 + (1 - max) * 0 + low) * color.a;

					return color;
				}
				ENDCG
			}
		}
}