Shader "Decryption/MainCloud"
{
    Properties
    {
        
        _LightIntensity ("光照强度", Range(0.75, 1)) = 1
        _MainColor ("MainCol", Color) = (0.6, 0.6, 0.6, 1)
        _MainTex ("MainTex", 2D) = "white" { }
        _AisleTex ("通道偏移纹理", 2D) = "white" { }
        [Space(10)]
        _AisleValueX ("Xspeed ", Range(0, -2.5)) = 0
        _AisleValueY ("Yspeed", Range(0, -0.5)) = 0
    }
    
    
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        
        Pass
        {
            Tags { "RenderType" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite off
            Cull back
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _MainTex, _AisleTex;
            float4 _MainTex_ST, _AisleTex_ST;
            fixed4 _MainColor;
            half _LightIntensity;
            
            half4 _LightingColor;
            half _AisleValueX, _AisleValueY;
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD;
            };
            
            struct v2f
            {
                float4 pos: SV_POSITION;
                float2 uv: TEXCOORD0;
                float2 uv2: TEXCOORD4;
                float2 uv3: TEXCOORD5;
                float4 uv4: TEXCOORD6;
            };
            
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv3 = v.uv;
                o.uv4.xy = TRANSFORM_TEX(v.uv, _AisleTex) + frac(float2(_AisleValueX, _AisleValueY) * _Time.x);
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                
                
                
                
                
                half4 AT = tex2D(_AisleTex, i.uv4.xy) ;
                
                // 采样纹理
                half4 c = tex2D(_MainTex, i.uv + AT.b * 0.12) * _MainColor;
                half4 d = tex2D(_MainTex, i.uv - AT.b * 0.1) * _MainColor;
                fixed4 cloud = c + d;
                // return c + d;
                cloud *= _LightIntensity;
                return cloud;
            }
            ENDCG
            
        }
    }
}
