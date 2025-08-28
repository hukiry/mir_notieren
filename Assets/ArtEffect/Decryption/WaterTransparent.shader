Shader "Decryption/WaterTransparent"
{
    Properties
    {
        _LightX ("_LightX", Range(-1, 1)) = 0.5
        _LightY ("_LightY", Range(-1, 1)) = 1
        _LightZ ("_LightZ", Range(-1, 1)) = -1
        _Specular ("_Specular", Color) = (1, 1, 1, 1)
        _Shininess ("_Shininess", float) = 192
        _SpecularIntensity ("_SpecularIntensity", float) = 0.75
        
        _Color1 ("_Color1", Color) = (0.1, 0.5, 0.3, 1)
        _Color2 ("_Color2", Color) = (0.3, 0.7, 0.6, 1)
        [HideInInspector]_ColorVaryK ("_ColorVaryK", float) = 0.8
        [HideInInspector]_ColorVaryB ("_ColorVaryB", float) = 0.3
        [HideInInspector]_TransparentDifference ("_TransparentDifference", float) = 0.2
        _TransparentVaryK ("_TransparentVaryK", Range(0.7, 1.1)) = 1
        
        _Reflect ("_Reflect", 2D) = "blue" { }
        _ReflectScale ("_ReflectScale", float) = 1
        _ReflectParallax ("_ReflectParallax", float) = 2
        _FnlPow ("_FnlPow", float) = 1.5
        _ReflectVaryK ("_ReflectVaryK", float) = 0.7
        [HideInInspector]_ReflectVaryB ("_ReflectVaryB", float) = 0
        
        _Flat ("_Flat", float) = 0
        _Wave ("_Wave", 2D) = "Bump" { }
        _ScaleX1 ("_ScaleX1", float) = 0.13
        _ScaleZ1 ("_ScaleZ1", float) = -0.13
        _SpeedX1 ("_SpeedX1", float) = 0.2
        _SpeedZ1 ("_SpeedZ1", float) = 0.6
        _ScaleX2 ("_ScaleX2", float) = -0.2
        _ScaleZ2 ("_ScaleZ2", float) = 0.2
        _SpeedX2 ("_SpeedX2", float) = -0.6
        _SpeedZ2 ("_SpeedZ2", float) = -0.2
        
        _ReflectWave ("_ReflectWav", float) = 0.025
        _LightmapWave ("_LightmapWave", float) = 0.01
        [HideInInspector]_LightmapVaryB ("_LightmapVaryB", float) = 0.1
        [HideInInspector]_LightmapVaryK ("_LightmapVaryK", float) = 0.75
    }
    
    SubShader
    {
        LOD 300
        
        Tags { "Queue" = "Transparent" }
        
        Pass
        {
            Lighting Off
            Fog
            {
                Mode Off
            }
            Cull Off
            ZTest LEqual
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ LIGHTNING_ON
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_fog
            #include "UnityCG.cginc"
            
            half _LightX;
            half _LightY;
            half _LightZ;
            half3 _Specular;
            half _Shininess;
            half _SpecularIntensity;
            half3 _Color1;
            half3 _Color2;
            half _ColorVaryK;
            half _ColorVaryB;
            half _TransparentDifference;
            half _TransparentVaryK;
            sampler2D _Reflect;
            half _ReflectScale;
            half _ReflectParallax;
            half _ReflectWave;
            half _FnlPow;
            half _ReflectVaryK;
            half _ReflectVaryB;
            half _LightmapVaryB;
            half _LightmapVaryK;
            half _LightmapWave;
            half _Flat;
            sampler2D _Wave;
            float _ScaleX1;
            float _ScaleZ1;
            float _SpeedX1;
            float _SpeedZ1;
            float _ScaleX2;
            float _ScaleZ2;
            float _SpeedX2;
            float _SpeedZ2;
            
            struct v2f
            {
                float4 pos: POSITION;
                float4 waveUV: TEXCOORD0;
                float3 view: TEXCOORD01;
                float3 light: TEXCOORD2;
                half depth: TEXCOORD3;
                #ifdef LIGHTMAP_ON
                    half2 lmuv: TEXCOORD4;
                #endif
                half4 fogParam: TEXCOORD5;
                #ifdef LIGHTNING_ON
                    half3 lightning: TEXCOORD6;
                #endif
                UNITY_FOG_COORDS(7)
            };
            
            v2f vert(appdata_full v)
            {
                v2f o;
                
                float3 wp = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.view = _WorldSpaceCameraPos - wp;
                o.light = normalize(float3(_LightX, _LightY, _LightZ));
                o.waveUV.x = (_SpeedX1 * _Time.y - wp.x) * _ScaleX1;
                o.waveUV.y = (_SpeedZ1 * _Time.y - wp.z) * _ScaleZ1;
                o.waveUV.z = (_SpeedX2 * _Time.y - wp.x) * _ScaleX2;
                o.waveUV.w = (_SpeedZ2 * _Time.y - wp.z) * _ScaleZ2;
                o.depth = v.color.a;
                // #ifdef LIGHTMAP_ON
                //     o.lmuv = LightMapUV(v.texcoord1);
                // #endif
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                UNITY_TRANSFER_FOG(o, o.pos);
                
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                half4 n1 = tex2D(_Wave, i.waveUV.xy);
                half4 n2 = tex2D(_Wave, i.waveUV.zw);
                half3 v = normalize(i.view);
                half f = saturate(pow(1 - v.y, _FnlPow) * _ReflectVaryK + _ReflectVaryB);
                half3 n = normalize((n1.xyz + n2.xyz - 1) * 2 + half3(0, 0, _Flat));
                n.xyz = n.yzx;
                half nv = dot(v, n);
                half3 lm = 1;
                // #ifdef LIGHTMAP_ON
                //     lm = max(LightMapColor(i.lmuv + _LightmapWave * n.xz) * _LightmapVaryK + _LightmapVaryB, 0);
                // #endif
                half4 r = tex2D(_Reflect, v.xz * (_ReflectParallax - v.y) * _ReflectScale + n.xz * _ReflectWave);
                half3 wc = lerp(_Color2, _Color1, saturate(max(nv, 0) * _ColorVaryK + _ColorVaryB)) * lm;
                half s = pow(max(dot(normalize(v + i.light), n), 0), _Shininess) * _SpecularIntensity;
                
                half4 c;
                c.rgb = max((r.rgb - wc) * f, 0) + wc + s * _Specular * lm;
                c.a = max(i.depth * _TransparentVaryK - nv * _TransparentDifference, s * i.depth);
                
                UNITY_APPLY_FOG(i.fogCoord, c);
                
                return c;
            }
            
            ENDCG
            
        }
    }
}
