Shader "Custom/UI/UISeqAnimate" {
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Rows ("rows", Float) = 3
        _Cols ("cols", Float) = 4
        _FrameCount ("frame count", Float) = 12
        _Speed ("speed", Float) = 100

        _IsEnabel ("IsEnabel", int) = 1

        _Color("Tint", Color) = (1,1,1,1)
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
        Tags { 
            "RenderType"="Opaque"
            "Queue" = "Transparent"
			"IgnoreProjector" = "True"
			//"RenderType" = "Transparent"
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

        LOD 200

        CGPROGRAM
        #pragma surface surf NoLighting alpha

        sampler2D _MainTex;
        fixed4 _Color;

        uniform fixed _Rows;
        uniform fixed _Cols;
        uniform int _FrameCount;
        uniform fixed _Speed;

        uniform int _IsEnabel;

        struct Input
        {
            UNITY_VERTEX_INPUT_INSTANCE_ID
            float2 uv_MainTex;
        };
        
        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            fixed4 c;
            c.rgb = s.Albedo;
            c.a = s.Alpha;
            return c;
        }
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            uint index = floor(_Time.z * _Speed);
            if(_IsEnabel == 1)
            {
                index = 5;
            }
            index = index % _FrameCount;
            int indexY = index / _Cols;
            int indexX = index - indexY * _Cols;
            
            float2 uv = float2(IN.uv_MainTex.x /_Cols, IN.uv_MainTex.y /_Rows);
            uv.x += indexX / _Cols;
            uv.y += indexY / _Rows;
            
            //o.Albedo = float3(floor(_Time .x * _Speed) , 0, 1);
            fixed4 c = tex2D(_MainTex, uv) * _Color;
            //o.Albedo = c.rgb;
            o.Emission = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Transparent/VertexLit"
}
