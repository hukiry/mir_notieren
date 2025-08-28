Shader "Decryption/SeaRipples"
{
    Properties
    {
        _LightDir ("Light Direction", Vector) = (0.981, 0.122, -0.148, 0.0)
        
        _Opacity ("Opacity", Range(0, 1)) = 0.65
        
        _WaveTex ("Texture", 2D) = "bump" { }
        _qiantanIntensity ("QiantanIntensity", float) = 3.5
        _qiantanCol ("qiantanCol", color) = (1, 1, 1, 1)
        _RipplesIntensity ("RipplesIntensity", float) = 1
        _Ripples ("RipplesTex", 2D) = "white" { }
        _WaveSpeed ("WaveSpeed", vector) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100
        
        Pass
        {
            Tags { "RenderType" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            //--------------------------------------------------------------------------------
            sampler2D _WaveTex, _Ripples;
            float4 _WaveTex_ST, _Ripples_ST;
            half4 _WaveSpeed;
            half4 _LightDir;
            fixed4 _qiantanCol;
            half _Opacity, _RipplesIntensity, _qiantanIntensity;
            
            //--------------------------------------------------------------------------------
            struct appdata
            {
                
                float4 vertex: POSITION;
                float4 uv: TEXCOORD0;
                half3 normal: NORMAL;
                half4 tangent: TANGENT;
                fixed4 color: COLOR;
            };
            
            struct v2f
            {
                
                float4 pos: SV_POSITION;
                float4 uv: TEXCOORD0;
                half3 WorldNormal: TEXCOORD1;
                float3 nDirWS: TEXCOORD2;
                float3 tDirWS: TEXCOORD3;
                float3 bDirWS: TEXCOORD4;
                fixed4 color: TEXCOORD5;
                half3 worldPos: TEXCOORD6;
                float2 waveuv: TEXCOORD7;
                float2 lyuv1: TEXCOORD8;
            };
            
            
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                
                o.uv.xy = TRANSFORM_TEX(v.uv, _WaveTex) * 100 - frac(float2(_Time.x * _WaveSpeed.xy));
                o.uv.zw = TRANSFORM_TEX(v.uv, _WaveTex) * 80 + frac(float2(_Time.x * _WaveSpeed.zw));
                o.waveuv = TRANSFORM_TEX(v.uv, _WaveTex) * float2(15, 25) - frac(float2(_Time.x * _WaveSpeed.xy));
                // o.waveuv = TRANSFORM_TEX(v.uv, _WaveTex) * float2(10, 20) ;
                o.lyuv1 = TRANSFORM_TEX(v.uv, _Ripples) * float2(60, 60) - frac(float2(0.85, 1) * _Time.x * 0.8);
                
                o.nDirWS = UnityObjectToWorldNormal(v.normal);
                o.tDirWS = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
                o.bDirWS = normalize(cross(o.nDirWS, o.tDirWS) * v.tangent.w);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                // sample the texture
                fixed3 wavetex1 = tex2D(_WaveTex, i.uv.xy);
                fixed3 wavetex2 = tex2D(_WaveTex, i.uv.zw);
                fixed3 wave = tex2D(_WaveTex, i.waveuv);
                
                
                
                float3 NormalMap = wavetex1 + wavetex2 + wave - 1.5 ;
                // return fixed4(NormalMap, 1);
                fixed4 lianyi1 = tex2D(_Ripples, i.lyuv1 - NormalMap * 0.08);
                
                
                
                float3x3 TBN = float3x3(i.tDirWS, i.bDirWS, i.nDirWS);
                float3 nDir = normalize(mul(NormalMap, TBN));
                
                float3 lDir = normalize(_LightDir);
                float nDotl = dot(nDir, lDir);
                
                float3 R = reflect(-lDir, nDir);
                float3 V = UnityWorldSpaceViewDir(i.worldPos);
                
                
                float lambert = max(nDotl, 0)  ;
                
                float lg = i.color.r;
                float l = max(lianyi1 * _RipplesIntensity, 0.0);
                // return lg;
                
                
                
                fixed3 c = (lambert + l) * _qiantanCol * _qiantanIntensity;
                float opacity = pow(i.color, 1.5) * _Opacity;
                // return opacity;
                return fixed4(c, opacity);
            }
            ENDCG
            
        }
    }
}
