Shader "Decryption/MixStandard"
{
    Properties
    {
        
        _LightDir ("Light Dir", vector) = (0.5, 1, 0.5, 0)
        _LightIntensity ("LightIntensity", float) = 1
        _MainCol ("MainColor", color) = (0.5, 0.5, 0.5, 1)
        [NoScaleOffset]_MainTex ("MainTex", 2D) = "white" { }
        _SpecularColor ("SpecularColor", color) = (1, 1, 1, 1)
        _SpecularPow01 ("SpecularPow01", range(1, 90)) = 30
        _SpecularPow02 ("SpecularPow02", range(1, 90)) = 30
        _Refvalue ("Refvalue", range(0, 2)) = 1
        
        _Phong ("Phong", Range(0, 1)) = 1
        _Dkness ("DKness", float) = 0.5
        
        _rimColor ("rimColor", color) = (1, 1, 1, 1)
        _fresnelPow ("fresnelPow", float) = 3
        
        
        [Space(10)]
        [Header(Env)]
        [Space(5)]
        _EnvIntsity ("EnvIntsity", float) = 1
        _EnvTCol ("EnvTCol", color) = (1, 1, 1, 1)
        _EnvMCol ("EnvMCol", color) = (0.5, 0.5, 0.5, 1)
        _EnvDCol ("EnvDCol", color) = (0, 0, 0, 1)
        [NoScaleOffset]_AOTex ("AOTex", 2D) = "white" { }
        _AOControl ("AOControl", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Geometry" }
        LOD 100
        
        Pass
        {
            Tags { "RenderType" = "Opaque" }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull back
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            
            fixed4 _MainCol, _SpecularColor, _EnvTCol, _EnvMCol, _EnvDCol, _rimColor;
            half _SpecularPow01, _SpecularPow02, _LightIntensity, _Refvalue, _EnvIntsity, _Dkness, _AOControl, _fresnelPow, _Phong;
            sampler2D _MainTex, _AOTex;
            float4 _MainTex_ST, _AOTex_ST;
            float4 _LightDir;
            
            
            struct appdata
            {
                float4 vertex: POSITION;
                float4 uv: TEXCOORD0;
                half3 normal: NORMAL;
            };
            
            struct v2f
            {
                float4 pos: SV_POSITION;
                float4 uv: TEXCOORD0;
                float4 WorldPos: TEXCOORD1;
                half3 WorldNorm: NORMAL;
            };
            
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.WorldNorm = UnityObjectToWorldNormal(v.normal);
                o.WorldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv, _AOTex);
                
                
                
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                
                
                
                fixed4 mt = tex2D(_MainTex, i.uv.xy);
                
                // lambient
                fixed4 diffuse;
                fixed4 Ambient = unity_AmbientSky;
                half Kd = _LightIntensity;
                half3 N = normalize(i.WorldNorm);
                half3 L = normalize(_LightDir);
                
                
                diffuse = Ambient + Kd * max(0, (dot(N, L) * 0.5 + _Dkness)) * _MainCol;
                
                // Phong
                fixed4 specular;
                fixed4 SpecularColor = _SpecularColor;
                half Ks = _Refvalue;
                half3 R = reflect(-L, N);
                half3 V = normalize(_WorldSpaceCameraPos.xyz - i.WorldPos.xyz);
                
                specular = SpecularColor * Ks * pow(max(0, dot(R, V)), lerp(_SpecularPow01, _SpecularPow02, mt.g));
                
                // 环境光模拟
                half3 Ndir = i.WorldNorm.y;
                fixed4 aotex = tex2D(_AOTex, i.uv.zw);
                // return half4 (Ndir, 1);
                half3 NdirT = max(0, Ndir);
                half3 NdirD = max(0, -Ndir);
                half3 NdirM = max(0, 1 - NdirT - NdirD);
                
                half3 aocol = lerp(1, aotex, _AOControl) * (_EnvTCol * NdirT + _EnvMCol * NdirM + _EnvDCol * NdirD) * _EnvIntsity ;
                half3 col = aocol;
                
                // Fresnel
                fixed4 rimCol = _rimColor;
                half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.WorldPos));
                float ndotv = dot(i.WorldNorm, worldViewDir);
                float fresnel = (0.2 + 2.0 * pow(1.0 - ndotv, _fresnelPow));
                fixed4 rf = rimCol * fresnel;
                
                
                // Ambient + phong
                col *= (diffuse + lerp(0, specular, _Phong)) * mt + rf ;
                
                return fixed4(col, mt.a);
            }
            ENDCG
            
        }
    }
}
