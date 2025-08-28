// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "bin/ui2"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_Textureadd("Texture add", 2D) = "black" {}
		_noise2("noise2", 2D) = "white" {}
		_texsudu("texsudu", Vector) = (0,2,0,0)
		_noisesudu("noisesudu", Vector) = (0.1,0.1,0,0)
		_zhuan("zhuan", Float) = 1.02
		_noisepower("noisepower", Range( 0 , 1)) = 0.3907282
		[HDR]_Color0("Color 0", Color) = (1,1,1,1)
		_mask2("mask2", 2D) = "white" {}
		[Toggle(_MASKON_ON)] _maskon("maskon", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			CompFront [_StencilComp]
			PassFront [_StencilOp]
			FailFront Keep
			ZFailFront Keep
			CompBack Always
			PassBack Keep
			FailBack Keep
			ZFailBack Keep
		}


		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _MASKON_ON

			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform sampler2D _Textureadd;
			uniform float2 _texsudu;
			uniform float4 _Textureadd_ST;
			uniform sampler2D _noise2;
			uniform float2 _noisesudu;
			uniform float4 _noise2_ST;
			uniform float _noisepower;
			uniform float _zhuan;
			uniform float4 _Color0;
			uniform sampler2D _mask2;
			uniform float4 _mask2_ST;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode3 = tex2D( _MainTex, uv_MainTex );
				float2 uv_Textureadd = IN.texcoord.xy * _Textureadd_ST.xy + _Textureadd_ST.zw;
				float2 panner6 = ( 1.0 * _Time.y * _texsudu + uv_Textureadd);
				float2 uv_noise2 = IN.texcoord.xy * _noise2_ST.xy + _noise2_ST.zw;
				float2 panner29 = ( 1.0 * _Time.y * _noisesudu + uv_noise2);
				float2 temp_cast_0 = (tex2D( _noise2, panner29 ).r).xx;
				float2 lerpResult10 = lerp( panner6 , temp_cast_0 , _noisepower);
				float cos36 = cos( _zhuan );
				float sin36 = sin( _zhuan );
				float2 rotator36 = mul( lerpResult10 - float2( 0.5,0.5 ) , float2x2( cos36 , -sin36 , sin36 , cos36 )) + float2( 0.5,0.5 );
				float2 uv_mask2 = IN.texcoord.xy * _mask2_ST.xy + _mask2_ST.zw;
				#ifdef _MASKON_ON
				float4 staticSwitch43 = tex2D( _MainTex, uv_MainTex );
				#else
				float4 staticSwitch43 = tex2D( _mask2, uv_mask2 );
				#endif
				
				half4 color = ( ( tex2DNode3 + ( tex2DNode3.a * tex2D( _Textureadd, rotator36 ) * _Color0 * staticSwitch43 ) ) * IN.color );
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
2040;30;1799;989;1829.797;475.9862;2.283755;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-1306.64,944.1059;Inherit;False;0;16;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;28;-1244.883,1176.147;Inherit;False;Property;_noisesudu;noisesudu;3;0;Create;True;0;0;0;False;0;False;0.1,0.1;0.1,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-942.5306,274.5215;Inherit;False;0;5;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;8;-817.931,528.5211;Inherit;True;Property;_texsudu;texsudu;2;0;Create;True;0;0;0;False;0;False;0,2;0,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;29;-1049.765,1041.413;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;16;-822.1973,939.9521;Inherit;True;Property;_noise2;noise2;1;0;Create;True;0;0;0;False;0;False;-1;5328ec2bdf6176d47922414eed896fe8;5328ec2bdf6176d47922414eed896fe8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-739.7894,1185.937;Inherit;True;Property;_noisepower;noisepower;5;0;Create;True;0;0;0;False;0;False;0.3907282;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;6;-660.2411,657.9729;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-438.6945,440.909;Inherit;False;Property;_zhuan;zhuan;4;0;Create;True;0;0;0;False;0;False;1.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;10;-359.7117,664.3658;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;37;-592.469,216.3396;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;39;-379.4555,942.4651;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;35;-42.88885,1204.75;Inherit;True;Property;_mask2;mask2;7;0;Create;True;0;0;0;False;0;False;-1;a3d9da1479e677d4fb969286f7754116;a3d9da1479e677d4fb969286f7754116;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;36;-254.2503,296.7188;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;41;-149.8855,939.4692;Inherit;True;Property;_mask1;mask;7;0;Create;True;0;0;0;False;0;False;-1;a3d9da1479e677d4fb969286f7754116;a3d9da1479e677d4fb969286f7754116;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;-359.7546,-15.3761;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;13.43108,250.7968;Inherit;True;Property;_Textureadd;Texture add;0;0;Create;True;0;0;0;False;0;False;-1;86a0bc076f1a57048855a7d74c7212a3;86a0bc076f1a57048855a7d74c7212a3;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;32;9.367811,520.9388;Inherit;False;Property;_Color0;Color 0;6;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-69.72784,-14.04745;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;43;249.2711,796.3944;Inherit;False;Property;_maskon;maskon;8;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;396.5823,233.3078;Inherit;False;4;4;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;33;633.6411,526.1387;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;4;658.4072,115.036;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;2;139,-224;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;960.5087,139.61;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1246.116,258.9077;Float;False;True;-1;2;ASEMaterialInspector;0;4;bin/ui2;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;True;0;True;-9;False;False;False;False;False;False;False;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;29;0;27;0
WireConnection;29;2;28;0
WireConnection;16;1;29;0
WireConnection;6;0;7;0
WireConnection;6;2;8;0
WireConnection;10;0;6;0
WireConnection;10;1;16;1
WireConnection;10;2;14;0
WireConnection;36;0;10;0
WireConnection;36;1;37;0
WireConnection;36;2;38;0
WireConnection;41;0;39;0
WireConnection;5;1;36;0
WireConnection;3;0;1;0
WireConnection;43;1;35;0
WireConnection;43;0;41;0
WireConnection;30;0;3;4
WireConnection;30;1;5;0
WireConnection;30;2;32;0
WireConnection;30;3;43;0
WireConnection;4;0;3;0
WireConnection;4;1;30;0
WireConnection;31;0;4;0
WireConnection;31;1;33;0
WireConnection;0;0;31;0
ASEEND*/
//CHKSM=C9DF1938659050DCF2B65CFE37F42EFF032CF543