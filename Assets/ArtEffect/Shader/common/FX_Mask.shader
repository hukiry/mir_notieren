Shader "FX/Hukiry_Mask" {
    Properties {
        [Header(_________________________________________________________________)]
        [Space(19)]
        [Enum(UnityEngine.Rendering.BlendMode)] SrcBlend("SrcBlend", Float) = 5//SrcAlpha
        [Enum(UnityEngine.Rendering.BlendMode)] DstBlend("DstBlend", Float) = 10//One
        [Header(_________________________________________________________________)]
        [Header(___________________ Main Texture __________________________________)]
        [HDR]_TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("MainTex", 2D) = "white" {}
        Speed_MainTex_U ("Speed_MainTex_U", Float ) = 0
        Speed_MainTex_V ("Speed_MainTex_V", Float ) = 0
        [Header(___________________ Mask Texture ________________________________)]
        MaskTex ("MaskTex", 2D) = "white" {}
        Speed_MaskTex_U ("Speed_MaskTex_U", Float ) = 0
        Speed_MaskTex_V ("Speed_MaskTex_V", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
            Blend[SrcBlend][DstBlend]
            AlphaTest Greater .01
            ColorMask RGB
            Cull Off Lighting Off ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            uniform sampler2D MaskTex; uniform float4 MaskTex_ST;
            uniform float Speed_MainTex_U;
            uniform float Speed_MainTex_V;
            uniform float Speed_MaskTex_U;
            uniform float Speed_MaskTex_V;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 time_MainTex = _Time;
                float2 UVmove_MainTex = (i.uv0+(time_MainTex.g*float2(Speed_MainTex_U,Speed_MainTex_V)));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(UVmove_MainTex, _MainTex));
                float3 emissive = (_MainTex_var.rgb*i.vertexColor.rgb*_TintColor.rgb*2.0);
                float3 finalColor = emissive;
                float4 time_MaskTex = _Time;
                float2 UVmove_MaskTex = (i.uv0+(time_MaskTex.g*float2(Speed_MaskTex_U,Speed_MaskTex_V)));
                float4 MaskTex_var = tex2D(MaskTex,TRANSFORM_TEX(UVmove_MaskTex, MaskTex));
                fixed4 finalRGBA = fixed4(finalColor,(_MainTex_var.a*i.vertexColor.a*MaskTex_var.r));
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
}
