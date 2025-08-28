Shader "Decryption/River"
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
        _NoiseTex ("NoiseTex", 2D) = "while" { }
        _WaveTex ("Texture", 2D) = "bump" { }
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
            sampler2D _WaveTex, _NoiseTex;
            float4 _WaveTex_ST, _NoiseTex_ST;
            half4 _WaveSpeed;
            half4 _LightDir;
            fixed4 _ShallowCol, _DeepCol, _SpecularColor;
            half _Shininess, _reflectPow, _Opacity;
            
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
                float2 noiseuv: TEXCOORD7;
            };
            
            
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                
                o.uv.xy = TRANSFORM_TEX(v.uv, _WaveTex) * 10 - frac(float2(_Time.x * _WaveSpeed.xy));
                o.uv.zw = TRANSFORM_TEX(v.uv, _WaveTex) * 8 + frac(float2(_Time.x * _WaveSpeed.zw));
                o.noiseuv = TRANSFORM_TEX(v.uv, _NoiseTex) + float2(0, 1) * _Time.x * 1 ;;
                o.nDirWS = UnityObjectToWorldNormal(v.normal);
                o.tDirWS = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
                o.bDirWS = normalize(cross(o.nDirWS, o.tDirWS) * v.tangent.w);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                // sample the texture
                float3 wavetex1 = tex2D(_WaveTex, i.uv.xy);
                float3 wavetex2 = tex2D(_WaveTex, i.uv.zw);
                float4 noisemap = tex2D(_NoiseTex, i.noiseuv);
                // return noisemap;
                float3 NormalMap = wavetex1 + wavetex2 - 1;
                // return fixed4(NormalMap, 1);
                float3x3 TBN = float3x3(i.tDirWS, i.bDirWS, i.nDirWS);
                float3 nDir = normalize(mul(NormalMap, TBN));
                
                float3 lDir = normalize(_LightDir);
                float nDotl = dot(nDir, lDir);
                
                float3 R = reflect(-lDir, nDir);
                float3 V = UnityWorldSpaceViewDir(i.worldPos);
                
                float3 Specular = _SpecularColor * pow(max(0, dot(R, V)), _Shininess) * _reflectPow ;
                
                float lambert = max(0.0, nDotl) * 0.5 + 0.15;
                
                float deepcol = pow(1 - i.color.g, 3) * noisemap * 8 ;
                // return deepcol;
                fixed4 col = _DeepCol * deepcol  ;
                
                fixed4 c;
                c.rgb = (lambert + Specular) * ((_ShallowCol * i.color.g) + col) * 2;
                c.a = i.color * _Opacity;
                return c;
            }
            ENDCG
            
        }
    }
}
