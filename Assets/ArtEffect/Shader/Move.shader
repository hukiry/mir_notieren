Shader "Tom/Move"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _RotaRange ("RotaRange", float) = 10
        _RotaSpeed ("RotaSpeed", float) = 0.2
        _MoveDir ("DirX DirY", vector) = (0, 0, 0, 0)
        _ShakeRange ("ShakeRange", Range(0, 1)) = 0.02
        _ShakePow ("ShakePow", Range(0, 2)) = 2
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100
        
        Pass
        {
            Tags { "RenderType" = "Transparent" }
            Blend srcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            
            // ------------------------------
            sampler2D _MainTex;
            float4 _MainTex_ST;
            half _RotaRange, _RotaSpeed;
            half _ShakeRange, _ShakePow;
            half4 _MoveDir;
            
            //-------------------------------
            
            struct appdata
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
            };
            
            struct v2f
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv: TEXCOORD0;
                float4 pos: SV_POSITION;
            };
            
            #define Cycle 6.283185
            
            void Rotation(inout float3 vertex)
            {
                float angleY = _RotaRange * sin(frac(_Time.y * _RotaSpeed) * Cycle);
                float radY = radians(angleY);
                float sinY, cosY = 0;
                sincos(radY, sinY, cosY);
                vertex.yz = float2(vertex.y * cosY - vertex.z * sinY, vertex.y * sinY + vertex.z * cosY);
            }
            
            void Translation(inout float3 vertex)
            {
                vertex.xyz += float3(_MoveDir.x, _MoveDir.y, 0) * _ShakeRange * sin(frac(_Time.z * _ShakePow) * Cycle);
            }
            
            v2f vert(appdata v)
            {
                v2f o;
                Rotation(v.vertex.xyz);
                Translation(v.vertex.xyz);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
            
        }
    }
}
