Shader "Decryption/Rotation"
{
    Properties
    {
        _MainCol ("MainCol", color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" { }
        _RotaRange ("RotaRange", float) = 30
        _RotaSpeed ("RotaSpeed", float) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100
        
        Pass
        {
            Tags { "RenderType" = "Transparent" }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            half _RotaRange, _RotaSpeed;
            fixed4 _MainCol;
            
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
                fixed4 color: COLOR;
            };
            
            struct v2f
            {
                float2 uv: TEXCOORD0;
                float4 pos: SV_POSITION;
                fixed4 color: TEXCOORD1;
            };
            
            #define Cycle 6.283185
            
            void Rotation(inout float3 vertex)
            {
                float angleY = _RotaRange * (_Time.y * _RotaSpeed) * Cycle;
                float radY = radians(angleY);
                float sinY, cosY = 0;
                sincos(radY, sinY, cosY);
                vertex.xz = float2(vertex.x * cosY - vertex.z * sinY, vertex.x * sinY + vertex.z * cosY);
            }
            
            
            v2f vert(appdata v)
            {
                v2f o;
                Rotation(v.vertex.xyz);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _MainCol;
                return col;
            }
            ENDCG
            
        }
    }
}
