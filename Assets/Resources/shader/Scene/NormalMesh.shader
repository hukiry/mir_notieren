Shader "Custom/NormalMesh"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		_R("R",Range(0,1)) = 0.8
		_G("G",Range(0,1)) = 0.1
		[Toggle] _OpenGray("Open", float) = 0
	}
	
	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			//"RenderType" = "Transparent"
			//"DisableBatching" = "True"
		}
		
		Pass
		{
			Cull Back
			Lighting Off
			ZWrite Off
			ZTest Off
			Fog { Mode Off }
			Offset -1, -1
		
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _OpenGray;
			float _R;
			float _G;
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				UNITY_VERTEX_OUTPUT_STEREO
			};
	
			

			v2f vert (appdata_t v)
			{	v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}

			float4 _Color;
			
			//最终颜色：将基础纹理叠加变亮返回	
			fixed4 frag(v2f IN) : SV_Target
			{
				if(_OpenGray)
				{
					half4 color = tex2D(_MainTex, IN.texcoord);// *IN.color;
					half3 tmp = dot(color.rgb, fixed3(.222,.707,.071));
					tmp = tmp * _R + color.rgb * _G;
					//float alpha = IN.color.a < 1?color.a*0.7:color.a;
					return half4(tmp.r,tmp.g,tmp.b, color.a);
				}
				else{
					half4 color = tex2D(_MainTex, IN.texcoord);// *IN.color;
					half3 temp = (color.rgb * IN.color.r + color.rgb * IN.color.g + color.rgb * IN.color.b)/3;
					float alpha = IN.color.a < 1?color.a*0.7:color.a;
					return fixed4(temp, alpha);
				}
			}
			ENDCG
		}

	
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			//"RenderType" = "Transparent"
			//"DisableBatching" = "True"
		}
		
		Pass
		{
			Cull Back
			Lighting Off
			/*ZWrite Off
			ZTest Off*/
			Fog { Mode Off }
			Offset -1, -1
			//ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
