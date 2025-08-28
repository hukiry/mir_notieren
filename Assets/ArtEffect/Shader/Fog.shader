Shader "Custom/Fog"
{
	Properties
	{
		[Header(Textures and color)]
		[Space][NoScaleOffset]
		_MainTex ("Fog texture", 2D) = "white" {}
		[NoScaleOffset] _Mask ("Mask", 2D) = "white" {}
		_Color ("Color", color) = (1., 1., 1., 1.)
		[Space(10)]

		[Header(Behaviour)]
		[Space]
		_ScrollDirX ("Scroll along X", Range(-1., 1.)) = 1.
		_ScrollDirY ("Scroll along Y", Range(-1., 1.)) = 1.
		_Speed ("Speed", float) = 1.
		_Distance ("Fading distance", Range(1., 10.)) = 1.

		[Toggle]_IsPlay ("IsPlay", int) = 1.
	}

	SubShader
	{
		Tags { "Queue"="Transparent+10" "RenderType"="Transparent+100" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct v2f {
				float4 poss : SV_POSITION;
				fixed4 vertCol : COLOR0;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata_full v)
			{
				v2f o;
				o.poss = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv2 = v.texcoord;
				o.vertCol = v.color;
				return o;
			}

			float _Distance;
			sampler2D _Mask;
			float _Speed;
			fixed _ScrollDirX;
			fixed _ScrollDirY;
			fixed4 _Color;

			int _IsPlay;

			fixed4 frag(v2f i) : SV_Target
			{
//				if(1 - ((i.poss.z / i.poss.w) * _Distance) > 1)  // i.pos.w 是从camera到自身的距离
//				return fixed4(1,0,0,1);
//				else return fixed4(0,1,0,1);

				float2 uv = i.uv;
				//云雾飘动
				if(_IsPlay)
				{
					uv = i.uv + fixed2(_ScrollDirX, _ScrollDirY) * _Speed * _Time.x;
				}

				
				fixed4 col = tex2D(_MainTex, uv) * _Color* i.vertCol;
				col.a *= tex2D(_Mask, i.uv2).r;
				col.a *= 1 - ((i.poss.z / i.poss.w) * _Distance);

				return col;
			}
			ENDCG
		}
	}
}