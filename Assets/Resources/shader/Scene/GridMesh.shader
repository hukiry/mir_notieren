Shader "Custom/GridMesh"
{
	Properties
	{
		_MainTex("Base (RGB), Alpha (A)", 2D) = "black" {}
	}
	
	SubShader
	{
		LOD 200
		Tags { 
			"Queue" = "Transparent" "IgnoreProjector" = "True" "PreviewType" = "Plane" //"RenderType" = "Transparent"
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


			v2f o;

			v2f vert (appdata_t v)
			{
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}

			fixed4 frag (v2f IN) : SV_Target
			{
				half4 color = tex2D(_MainTex, IN.texcoord);
				half3 tmp = lerp(dot(color.rgb, IN.color.rgb), color.rgb, step(1, IN.color.r * IN.color.g * IN.color.b));
				return half4(tmp, IN.color.a<1?0.7*color.a:color.a);
			}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" }
		Pass
		{
			Cull Back
			Lighting Off
			ZWrite Off
			ZTest Off
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
