Shader "Hukiry/UI/Gray"
{
    Properties
    {
		[HideInInspector]
        _MainTex ("Texture", 2D) = "white" {}

		//UI部分
		_StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
		 _ColorMask ("Color Mask", Float) = 15

		_R("R0.3",Range(0,1))=0.3
		_G("G0.59",Range(0,1))=0.59
		_B("B0.11",Range(0,1))=0.11

		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend Mode", Float) = 5
        [HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend Mode", Float) = 10
		[HideInInspector][Enum(Off, 0, On, 1)] _ZWrite ("ZWrite", Float) = 0
		//预处理值无法在编辑器上动态修改
		 [Toggle]_IsGray ("开启灰度", Float) = 1

		 [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		// [PowerSlider(0.2)] _Shininess ("Shininess", Range (0.2, 0.7)) = 0.08
    }

    SubShader
    {
        // No culling or depth
		Tags {     

            "IgnoreProjector" ="True"  
			 "Queue"="Transparent" 
			 "RenderType"="Transparent"
			 "PreviewType" = "Plane"
			 "CanUseSpriteAtlas"="True"
		}

		Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

		LOD 100
		//标准透明通道 SrcAlpha OneMinusSrcAlpha 
		Blend SrcAlpha OneMinusSrcAlpha //[_SrcBlend] [_DstBlend] 
        Cull Off
		ZWrite [_ZWrite]
		Lighting Off 
		ZTest Always  
		//颜色遮罩，UI滚动时可以帮助遮挡
		ColorMask [_ColorMask]

        Pass
        {
			Name "Gray"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			//#pragma shader_feature _ISGRAY_ON

            #include "UnityCG.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				 UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

			sampler2D _MainTex;
            float4 _MainTex_ST;

			float _R,_G,_B;
			float _IsGray;

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				#ifdef UNITY_UI_ALPHACLIP
				clip(col.a - 0.001);
				#endif

				float3 gray=dot(col.rgb,float3(_R,_G,_B));
				if (_IsGray)
					return float4(gray,col.a);
				else
					return col;				
            }
            ENDCG
        }
    }

	CustomEditor "ShaderViewEditor"
}
