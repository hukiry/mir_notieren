Shader "LeBook/MainCloud"
{
    Properties
    {
        _LightIntensity ("光照强度", Range(0, 3)) = 1.0
        _MainColor ("MainCol", Color) = (1, 1, 1, 1)
        _MainTex ("MainTex", 2D) = "white" { }
        _AisleTex ("通道偏移纹理", 2D) = "white" { }
        _AisleValue ("通道R X:SpeedX Y:SpeedY
        通道G Z:SpeedX W:SpeedY", vector) = (1, 1, 1, 1)
        _MaskTex ("MaskTex", 2D) = "white" { }
        _Alpha ("云层透明度", Range(0, 1)) = 0.5
        _Height ("云凹凸高度", range(0, 1)) = 0.15
        _HeightAmount ("湍流量", range(0, 1)) = 0.5
        _HeightTileSpeed ("XY：湍流纹理平铺& Z：湍流纹理流速 W：主纹理流速", Vector) = (1.0, 1.0, 0.65, 0.7)
        [Toggle]_CustomLight ("自定义灯光", Int) = 1
        _CustomLightDir ("Light Direction", Vector) = (0.981, 0.122, -0.148, 0.0)
        _Radius ("边缘遮罩范围", float) = 0.5
        _CircleFade ("边缘遮罩羽化", Range(0, 1)) = 1
        _Width ("Width", float) = 0.25
        _Fade ("Fade", float) = 0.25
        _H ("Height", float) = 0.25
    }
    
    
    SubShader
    {
        LOD 300
        Tags { "IgnoreProjector" = "True" "Queue" = "Transparent-50" "RenderType" = "Transparent" }
        
        Pass
        {
            Name "FORWARD"
            Tags { "LightMode" = "ForwardBase" }
            Blend One OneMinusSrcAlpha
            ZWrite Off
            Cull back
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            
            #pragma target 3.0
            
            sampler2D _MainTex, _AisleTex, _MaskTex;
            float4 _MainTex_ST, _AisleTex_ST;
            half _Height;
            float4 _HeightTileSpeed;
            half _HeightAmount;
            fixed4 _MainColor;
            half _Alpha;
            half _LightIntensity;
            
            half4 _LightingColor;
            half4 _CustomLightDir;
            half _CustomLight;
            half _Radius, _CircleFade;
            half4 _AisleValue;
            half _H, _Fade, _Width;
            
            struct v2f
            {
                float4 pos: SV_POSITION;
                float2 uv: TEXCOORD0;
                float3 normalDir: TEXCOORD1;
                float3 viewDir: TEXCOORD2;
                float4 posWorld: TEXCOORD3;
                float2 uv2: TEXCOORD4;
                float4 color: TEXCOORD5;
                float2 uv3: TEXCOORD6;
                float4 uv4: TEXCOORD7;
                float2 maskuv: TEXCOORD8;
            };
            
            v2f vert(appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex) + frac(_Time.x * _HeightTileSpeed.zw);
                o.uv2 = v.texcoord * _HeightTileSpeed.xy;
                o.uv3 = v.texcoord;
                o.uv4.xy = TRANSFORM_TEX(v.texcoord, _AisleTex) + frac(_Time.x * _AisleValue.xy);
                o.uv4.zw = o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.maskuv = v.texcoord;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                TANGENT_SPACE_ROTATION;
                o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex));
                
                
                o.color = v.color;
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                
                
                float3 viewRay = normalize(-i.viewDir);
                viewRay.z = abs(viewRay.z) + 0.2;
                viewRay.xy *= _Height;
                
                float3 shadeP = float3(i.uv, 0);
                float3 shadeP2 = float3(i.uv2, 0);
                
                
                float linearStep = 16;
                
                float4 T = tex2D(_MainTex, shadeP2.xy);
                float h2 = T.a * _HeightAmount;
                
                float3 lioffset = viewRay / (viewRay.z * linearStep);
                float d = 1.0 - tex2Dlod(_MainTex, float4(shadeP.xy, 0, 0)).a * h2;
                float3 prev_d = d;
                float3 prev_shadeP = shadeP;
                while(d > shadeP.z)
                {
                    prev_shadeP = shadeP;
                    shadeP += lioffset;
                    prev_d = d;
                    d = 1.0 - tex2Dlod(_MainTex, float4(shadeP.xy, 0, 0)).a * h2;
                }
                float d1 = d - shadeP.z;
                float d2 = prev_d - prev_shadeP.z;
                float w = d1 / (d1 - d2);
                shadeP = lerp(shadeP, prev_shadeP, w);
                
                // 采样纹理
                half4 c = tex2D(_MainTex, shadeP.xy) * T * _MainColor + 0.1;
                half4 AT = tex2D(_AisleTex, i.uv4.xy) ;
                
                // // 边缘遮罩
                // float circle = 1 - (smoothstep(_Radius, (_Radius + _CircleFade), length(i.uv3 * 2 - 1)));
                // // return circle;
                fixed4 maskTex = tex2D(_MaskTex, i.maskuv);
                
                float2 centerUV = abs(i.uv3 * 2 - 1);
                float rectangleX = smoothstep(_Width, (_Width + _Fade), centerUV.x);
                float rectangleY = smoothstep(_H, (_H + _Fade), centerUV.y);
                float rectangleClamp = 1 - clamp((rectangleX + rectangleY), 0.0, 1.0);
                
                half Opacity = lerp(AT.g, 1, _Alpha) * i.color.r * maskTex.r * rectangleClamp;
                // return Opacity;
                float3 normal = normalize(i.normalDir);
                half3 lightDir1 = normalize(_CustomLightDir.xyz);
                half3 lightDir2 = UnityWorldSpaceLightDir(i.posWorld);
                half3 lightDir = lerp(lightDir2, lightDir1, _CustomLight);
                float NdotL = max(0, dot(normal, lightDir)) * 0.5 + 0.5;
                fixed3 finalColor = c.rgb * (NdotL + 1) * _LightIntensity;
                return half4(finalColor * Opacity, Opacity);
            }
            ENDCG
            
        }
    }
}
