
Shader "Custom/UITextureBlur" 
{

	Properties
	{
		[HideInInspector]_MainTex("Base (RGB)", 2D) = "white" {}

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
		[HideInInspector][Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}
 
	SubShader
	{
		Pass
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

			ZTest Always
			Cull Off
			ZWrite Off
			Fog{ Mode Off }
			ColorMask[_ColorMask]

			CGPROGRAM
			#include "UnityCG.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			#pragma vertex vert
			#pragma fragment frag
			//blur结构体，从blur的vert函数传递到frag函数的参数
			struct v2f
			{
				float4 pos : SV_POSITION; //顶点位置
				float2 uv  : TEXCOORD0;	  //纹理坐标
				float2 uv1 : TEXCOORD1;  //周围纹理1
				float2 uv2 : TEXCOORD2;  //周围纹理2
				float2 uv3 : TEXCOORD3;  //周围纹理3
				float2 uv4 : TEXCOORD4;  //周围纹理4
			};
 
			//用到的变量
			sampler2D _MainTex;
			//XX_TexelSize，XX纹理的像素相关大小width，height对应纹理的分辨率，x = 1/width, y = 1/height, z = width, w = height
			float4 _MainTex_TexelSize;
			float _BlurRadius = 2;//模糊半径
			v2f vert(appdata_img v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				//计算uv上下左右四个点对于blur半径下的uv坐标
				o.uv1 = v.texcoord.xy + _BlurRadius * _MainTex_TexelSize * float2( 1,  1);
				o.uv2 = v.texcoord.xy + _BlurRadius * _MainTex_TexelSize * float2(-1,  1);
				o.uv3 = v.texcoord.xy + _BlurRadius * _MainTex_TexelSize * float2(-1, -1);
				o.uv4 = v.texcoord.xy + _BlurRadius * _MainTex_TexelSize * float2( 1, -1);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 color = fixed4(0,0,0,0);
				color += tex2D(_MainTex, i.uv );
				color += tex2D(_MainTex, i.uv1);
				color += tex2D(_MainTex, i.uv2);
				color += tex2D(_MainTex, i.uv3);
				color += tex2D(_MainTex, i.uv4);
				#ifdef UNITY_UI_ALPHACLIP
					clip(color.a - 0.001);
				#endif
				return color/5;
			}
			ENDCG
		}
	}
}