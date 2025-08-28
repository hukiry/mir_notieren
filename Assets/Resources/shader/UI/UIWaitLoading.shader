
Shader "Custom/UIWaitLoading"
{
	Properties
	{
		[HideInInspector]
        _MainTex ("Texture", 2D) = "white" {}
		[Header(Setting Color)][HDR]
		_Color("内部颜色", Color) = (0.5, 0.5, 0.5, 1)
		[HDR]_OutColor("外边颜色", Color) = (1, 1, 1, 1)
		_OutlineWidth("外边颜色宽", Range(0.1, 0.5)) = 0.2
		[Header(Setting Attribute)]
		_Speed("转圈速度", Range(1, 10)) = 4
		_Radius("转圈半径", Range(0, 0.5)) = 0.4
		_DotCount("圆点数量", Range(6, 10)) = 9
		_Scale("圆点缩放", Range(0.003, 0.01)) = 0.006

		_StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
		 _ColorMask ("Color Mask", Float) = 15
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off

		Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

		ColorMask [_ColorMask]

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			#define PI 3.14159

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			fixed4 _Color;
			fixed4 _OutColor;
			half _Speed;
			fixed _Radius;
			half _DotCount;
			half _Scale;
			half _OutlineWidth;

			fixed4 circle(float2 uv, float2 center, float radius)
			{
				//if(pow(uv.x - center.x, 2) + pow(uv.y - center.y, 2) < pow(radius, 2)) return _Color;
				half rate = 1-_OutlineWidth;
				if (length(uv - center) < radius*rate) return _Color;
				else if (length(uv - center) < radius) return _OutColor;
				else return fixed4(0, 0, 0, 0);
			}

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 finalCol = (0, 0, 0, 0);

				for (int count = _DotCount + 1; count > 1; count--)
				{
					half radian = fmod(_Time.y * _Speed + count * 0.6, 2 * PI);//弧度
					half2 center = half2(0.5 - _Radius * cos(radian), 0.5 + _Radius * sin(radian));

					finalCol += circle(i.uv, center, count * _Scale);
				}

				return finalCol;
			}
			ENDCG
		}
	}
}