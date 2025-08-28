Shader "Decryption/MaskFlow"
{
    Properties
    {
        
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [PerRendererData]_MainTex ("Texture", 2D) = "white" { }
        [Toggle] _SweepEnabled ("SweepEnabled", float) = 0
        _Intensity ("Intensity", float) = 1
        _SweepCol ("SweepCol", color) = (1, 1, 1, 1)
        _SweepTex ("SweepTex", 2D) = "white" { }
        _Speed ("Speed", float) = 0
        _MaskTex ("MaskTex", 2D) = "black" { }
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" "CanUseSpriteAtlas" = "True" }
        LOD 100
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
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
            #pragma multi_compile _ _SWEEPENABLED_ON
            #include "UnityCG.cginc"
            
            sampler2D _MainTex, _SweepTex, _MaskTex;
            float4 _MainTex_ST, _SweepTex_ST, _MaskTex_ST;
            fixed4 _SweepCol;
            half _Speed, _Intensity, _ClipRect;
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv: TEXCOORD0;
                float4 vertex: SV_POSITION;
                float2 sweepuv: TEXCOORD1;
                float2 maskuv: TEXCOORD2;
                float4 worldPosition: TEXCOORD3;
            };
            
            inline float UnityGet2DClipping(in float2 position, in float4 clipRect)
            {
                float2 inside = step(clipRect.xy, position.xy) * step(position.xy, clipRect.zw);
                return inside.x*inside.y;
            }
            
            v2f vert(appdata v)
            {
                v2f o;
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(o.worldPosition);
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.sweepuv = TRANSFORM_TEX(v.uv, _SweepTex) - frac(float2(1, 0) * _Time.y * _Speed);
                o.maskuv = v.uv;
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 sweep = tex2D(_SweepTex, i.sweepuv);
                fixed mask = tex2D(_MaskTex, i.maskuv);
                
                #if _SWEEPENABLED_ON
                    sweep *= mask;
                    col.rgba += saturate(sweep * _SweepCol) * _Intensity * 10;
                #endif
                
                #ifdef UNITY_UI_CLIP_RECT
                    col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect)  ;
                #endif
                
                #ifdef UNITY_UI_ALPHACLIP
                    clip(col.a - 0.001)  ;
                #endif
                
                return col;
            }
            ENDCG
            
        }
    }
}
