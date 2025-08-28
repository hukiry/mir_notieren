Shader "Decryption/SeaWater"
{
    Properties
    {
        _LightDir ("Light Direction", Vector) = (0.981, 0.122, -0.148, 0.0)
        _ShallowCol ("ShallowCol", color) = (0.16, 0.5, 0.69, 1)
        _DeepCol ("DeepCol", color) = (0.12, 0.25, 0.32, 1)
        _Opacity ("Opacity", Range(0, 1)) = 0.65
        _SpecularColor ("SpecularColor", color) = (0.65, 0.65, 0.65, 1)
        _ReflectPow ("reflectPow", float) = 1
        _Shininess ("Shininess", float) = 1
        _WaveTex ("Texture", 2D) = "bump" { }
        _QiantanIntensity ("QiantanIntensity", float) = 1
        _qiantanCol ("qiantanCol", color) = (1, 1, 1, 1)
        
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
            
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            //--------------------------------------------------------------------------------
            sampler2D _WaveTex;
            float4 _WaveTex_ST;
            half4 _WaveSpeed;
            half4 _LightDir;
            fixed4 _ShallowCol, _DeepCol, _SpecularColor, _qiantanCol;
            half _Shininess, _reflectPow, _Opacity, _QiantanIntensity;
            
            //--------------------------------------------------------------------------------
            struct appdata
            {
                
                float4 vertex: POSITION;
                float4 uv: TEXCOORD0;
                half3 normal: NORMAL;
                half4 tangent: TANGENT;
                fixed3 color: COLOR;
            };
            
            struct v2f
            {
                
                float4 pos: SV_POSITION;
                float4 uv: TEXCOORD0;
                half3 WorldNormal: TEXCOORD1;
                float3 nDirWS: TEXCOORD2;
                float3 tDirWS: TEXCOORD3;
                float3 bDirWS: TEXCOORD4;
                fixed3 color: TEXCOORD5;
                half3 worldPos: TEXCOORD6;
                float2 waveuv: TEXCOORD7;
            };
            
            
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                
                o.uv.xy = TRANSFORM_TEX(v.uv, _WaveTex) * 100 - frac(float2(_Time.x * _WaveSpeed.xy));
                o.uv.zw = TRANSFORM_TEX(v.uv, _WaveTex) * 80 + frac(float2(_Time.x * _WaveSpeed.zw));
                // o.waveuv = TRANSFORM_TEX(v.uv, _WaveTex) * float2(15, 25) - frac(float2(_Time.x * _WaveSpeed.xy));
                o.waveuv = TRANSFORM_TEX(v.uv, _WaveTex) * float2(10, 15) - frac(_Time.x * 0.5);
                
                
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
                
                
                
                float3x3 TBN = float3x3(i.tDirWS, i.bDirWS, i.nDirWS);
                float3 nDir = normalize(mul(TBN, NormalMap));
                
                float3 lDir = normalize(_LightDir);
                float nDotl = dot(nDir, lDir);
                
                float3 R = reflect(-lDir, nDir);
                float3 V = UnityWorldSpaceViewDir(i.worldPos);
                
                float3 Specular = _SpecularColor * pow(max(0, dot(R, V)), _Shininess) * _reflectPow ;
                float lambert = max(0.0, nDotl) * 0.5 + 0.15;
                
                
                float lg = 1 - i.color.r ;
                float l = max(lg * _QiantanIntensity, 0) ;
                // return l;
                
                
                
                fixed3 c = (lambert + Specular) * ((_ShallowCol * i.color.b) + (_DeepCol * i.color.r) + l * _qiantanCol) * 1.5  ;
                float opacity = i.color * _Opacity;
                return fixed4(c, opacity);
            }
            ENDCG
            
        }
    }
}
