Shader "Decryption/UI/Default"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" { }
        _Color ("Tint", Color) = (1, 1, 1, 1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15
        [Space(10)]
        [Toggle]_SweepEnabled ("Sweep Enabled", Int) = 0
        _Speed ("Speed", float) = 2
        _MoveX ("Move X", float) = 0
        _MoveY ("Move Y", float) = 0
        _ColIntensity ("ColIntensity", Range(0, 3)) = 1
        _GlowCol ("Glow Color", color) = (1, 1, 1, 1)
        _guangTex ("GuangTex", 2D) = "back" { }
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" "CanUseSpriteAtlas" = "True" }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
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
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
            #pragma multi_compile _ _SWEEPENABLED_ON

            sampler2D _MainTex, _guangTex;
            fixed4 _Color, _GlowCol;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST, _guangTex_ST;
            half _ColIntensity;
            half _MoveX, _MoveY, _Speed;
            struct appdata_t
            {
                float4 vertex: POSITION;
                float4 color: COLOR;
                float2 texcoord: TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex: SV_POSITION;
                fixed4 color: COLOR;
                float2 texcoord: TEXCOORD0;
                float4 worldPosition: TEXCOORD1;
                float2 lineuv: TEXCOORD2;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.lineuv = TRANSFORM_TEX(v.texcoord, _guangTex) - frac(float2(_MoveX, _MoveY) * _Time.x * _Speed);

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN): SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
                fixed4 gt = (tex2D(_guangTex, IN.lineuv) + _TextureSampleAdd);

                #if _SWEEPENABLED_ON

                    color.rgb += gt.r * _ColIntensity * _GlowCol;
                #endif

                #ifdef UNITY_UI_CLIP_RECT
                    color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect)  ;
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                    clip(color.a - 0.001)  ;
                #endif

                return color   ;
            }
            ENDCG
            
        }
    }
}
