Shader "Tom/Sea"
{
    Properties
    {
        _LightDir ("LightDir (X Y Z)", vector) = (0, 0, 0, 0)
        _SpecularCol ("Specular Color", color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess Pow", float) = 0
        _reflectPow ("Reflect Pow", float) = 0
        [HideInInspector]_MainTex ("MainTex", 2D) = "white" { }
        _RipTex ("RipTex", 2D) = "white" { }
        _WaveTex ("Texture", 2D) = "bump" { }
        _WaveSpeed ("WaveSpeed", vector) = (1, 1, 1, 1)
        _WarpInt ("WarpInt", float) = 0.2
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
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _RipTex;
            float4 _RipTex_ST;
            sampler2D _WaveTex;
            float4 _WaveTex_ST;
            half4 _WaveSpeed;
            half _WarpInt;
            half4 _LightDir;
            half _Shininess, _reflectPow;
            fixed4 _SpecularCol;
            //--------------------------------------------------------------------------------
            struct appdata
            {
                
                float4 vertex: POSITION;
                float4 uv: TEXCOORD0;
                half3 normal: NORMAL;
                half4 tangent: TANGENT;
            };
            
            struct v2f
            {
                
                float4 pos: SV_POSITION;
                float4 uv: TEXCOORD0;
                float2 waveuv: TEXCOORD1;
                float2 MaintexUV: TEXCOORD2;
                float2 RipUV: TEXCOORD3;
                half3 worldNormal: TEXCOORD4;
                float4 worldPos: TEXCOORD5;
                float3 nDirWS: TEXCOORD6;
                float3 tDirWS: TEXCOORD7;
                float3 bDirWS: TEXCOORD8;
            };
            
            
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.MaintexUV = v.uv;
                o.RipUV = TRANSFORM_TEX(v.uv, _RipTex) + frac(_Time.x * 0.6);
                o.uv.xy = TRANSFORM_TEX(v.uv, _WaveTex) * 10 - frac(float2(_Time.x * _WaveSpeed.xy));
                o.uv.zw = TRANSFORM_TEX(v.uv, _WaveTex) * 8 + frac(float2(_Time.x * _WaveSpeed.zw));
                o.waveuv = TRANSFORM_TEX(v.uv, _WaveTex) * float2(1, 2) - frac(_Time.x * 1.2);
                
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
                // return fixed4(wavetex1, 1);
                float2 NormalMap = wavetex1 + wavetex2 + wave ;
                
                float3x3 TBN = float3x3(i.tDirWS, i.bDirWS, i.nDirWS);
                float3 nDir = normalize(mul(TBN, NormalMap));
                
                float3 lDir = normalize(_LightDir);
                float nDotl = dot(nDir, lDir);
                
                float3 R = reflect(-lDir, nDir);
                float3 V = UnityWorldSpaceViewDir(i.worldPos);
                
                float3 Specular = _SpecularCol * pow(max(0, dot(R, V)), _Shininess) * _reflectPow ;
                float lambert = max(0.0, nDotl) * 0.5 + 0.35;
                // return fixed4(lambert, lambert, lambert, lambert);
                
                float2 cuv = i.MaintexUV + NormalMap * _WarpInt * 0.02;
                fixed rip = tex2D(_RipTex, i.RipUV + NormalMap * 0.2) * 0.05;
                fixed4 c = tex2D(_MainTex, cuv);
                c += rip;
                fixed3 lcol = (Specular + lambert);
                c.rgb *= lcol;
                return c;
            }
            ENDCG
            
        }
    }
}
