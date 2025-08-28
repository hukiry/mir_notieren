Shader "Tom/GlowEdge"
{
    Properties
    {
        [HideInInspector]_MainTex ("Texture", 2D) = "white" { }
        [Space(10)]
        [Toggle]_GlowEnabled ("GlowEnabled", int) = 0
        [Space(5)]
        _GlowFre ("Glow Frequency", float) = 0.25
        _GlowCol ("Glow Color", color) = (0.8, 0.8, 0.8, 1)
        [Space(5)]
        [Toggle]_ShakeEnabled ("ShaKeEnabled", int) = 0
        _MoveDir ("DirX DirY", vector) = (0, 0, 0, 0)
        _ShakeRange ("ShakeRange", Range(0, 1)) = 0.02
        _ShakePow ("ShakePow", Range(0, 2)) = 2
        
        [Space(5)]
        [Toggle]_JumpEnabled ("JumpEnabled", int) = 0
        _JumpCount ("JumpCount", Range(0, 1)) = 0.25
        
        [Space(10)]
        [Toggle]_RotaEnabled ("RotationEnabled", Int) = 0
        _RotaRange ("Rota Range", Range(-30, 30)) = 10
        _RotaSpeed ("RotaSpeed", float) = 10
    }
    SubShader
    {
        
        
        Pass
        {
            Tags { "RenderType" = "TranSparent" }
            Blend SrcAlpha One
            ZTest Always
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _GLOWENABLED_ON
            #pragma multi_compile _ _SHAKEENABLED_ON
            #pragma multi_compile _ _JUMPENABLED_ON
            #pragma multi_compile _ _ROTAENABLED_ON
            
            
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _GlowCol;
            float _GlowFre;
            half _ShakeRange, _ShakePow;
            half4 _MoveDir;
            half _JumpCount;
            half _RotaRange, _RotaSpeed;
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex: SV_POSITION;
                float2 uv: TEXCOORD0;
            };
            
            #define PI 6.283185
            
            void Translation(inout float3 vertex)
            {
                vertex.xyz += float3(_MoveDir.x, 0, 0) * _ShakeRange * sin(frac(_Time.z * _ShakePow) * PI);
            }
            
            void Scale(inout float vertex)
            {
                
                vertex += abs(sin(frac(_Time.y * 1.5) * PI)) * _JumpCount ;
            }
            
            void Rotation(inout float3 vertex)
            {
                float angleY = _RotaRange * sin(_Time.y * _RotaSpeed) ;
                float radY = radians(angleY);
                float sinY, cosY = 0;
                sincos(radY, sinY, cosY);
                vertex.xy = float2(vertex.x * cosY - vertex.y * sinY, vertex.x * sinY + vertex.y * cosY);
            }
            
            v2f vert(appdata v)
            {
                v2f o;
                #if _SHAKEENABLED_ON
                    Translation(v.vertex.xyz);
                #endif
                
                #if _JUMPENABLED_ON
                    Scale(v.vertex.y);
                #endif
                
                #if _ROTAENABLED_ON
                    Rotation(v.vertex.xyz);
                #endif
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                #if _GLOWENABLED_ON
                    fixed4 gcol = _GlowCol * (abs(sin(_Time.y * _GlowFre)) + 0.5);
                    col *= gcol;
                    return col;
                #endif
                
                return col * 0;
            }
            ENDCG
            
        }
    }
}
