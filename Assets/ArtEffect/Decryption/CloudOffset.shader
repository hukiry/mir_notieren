Shader "Decryption/CloudOffset"
{
    Properties
    {
        _CustomLightDir ("自定义灯光", vector) = (0.5, 0.75, 0, 0)
        _MainCol ("云彩颜色", color) = (0.75, 0.75, 0.75, 1)
        _LightIntensity ("光照亮度", float) = 1
        _MainTex ("主纹理贴图", 2D) = "white" { }
        _OffsetTex ("通道偏移", 2D) = "white" { }
        _Mainvalue ("主图偏移值 X:SpeedX Y:SpeedY", vector) = (1.0, 0.5, 0.0, 0.0)
        _Offsetvalue ("通道偏移值 Tex1:SpeedX Y:SpeedY Tex2:SpeedX Y:SpeedY", vector) = (0.2, 0.25, 0.6, 0.67)
        _Radius ("边缘遮罩范围", float) = 0.5
        _CircleFade ("边缘遮罩羽化值", float) = 0.5
        _Opacity ("透明度", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent+20" "RenderType" = "Transparent" }
        LOD 100
        
        Pass
        {
            Name "FORWARD"
            Tags { "LightMode" = "ForwardBase" }
            Blend one OneMinusSrcAlpha
            ZWrite Off
            Cull back
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            
            #include "UnityCG.cginc"
            
            
            sampler2D _MainTex, _OffsetTex;
            float4 _MainTex_ST, _OffsetTex_ST;
            fixed4 _MainCol;
            half _CircleFade, _Radius, _LightIntensity, _Opacity;
            half4 _CustomLightDir, _Mainvalue, _Offsetvalue;
            
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
                half3 normal: NORMAL;
                float4 color: COLOR;
            };
            
            struct v2f
            {
                float4 pos: SV_POSITION;
                float2 uv: TEXCOORD0;
                float2 uv1: TEXCOORD1;
                float2 uv2: TEXCOORD2;
                float2 uv3: TEXCOORD3;
                half3 worldNormal: TEXCOORD4;
                float4 color: TEXCOORD5;
            };
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex) + frac(float2(_Mainvalue.xy) * _Time.x);
                o.uv1 = TRANSFORM_TEX(v.uv, _OffsetTex) + frac(float2(_Offsetvalue.xy) * _Time.x) ;
                o.uv2 = TRANSFORM_TEX(v.uv, _OffsetTex) + frac(float2(_Offsetvalue.zw) * _Time.x);
                o.uv3 = v.uv;
                o.color = v.color;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                // return col;
                fixed4 col = tex2D(_MainTex, i.uv);
                
                fixed4 ostex01 = tex2D(_OffsetTex, i.uv1);
                fixed4 ostex02 = tex2D(_OffsetTex, i.uv2);
                
                fixed4 cloud01 = fixed4(col.rgb, ostex01.r);
                // return cloud01;
                
                fixed4 cloud02 = fixed4(col.rgb, ostex02.g);
                // return cloud02;
                
                fixed4 cloudtex = cloud01 * cloud02 * _MainCol * _LightIntensity;
                
                //向量
                half3 N = normalize(i.worldNormal);
                half3 L = normalize(_CustomLightDir);
                
                half3 Ambient = max(dot(N, L), 0) * 0.5 + 0.5;
                
                cloudtex.rgb += Ambient;
                
                // 边缘遮罩
                float circle = 1 - (smoothstep(_Radius, (_Radius + _CircleFade), length(i.uv3 * 2 - 1)));
                
                fixed opacity = cloudtex.a * i.color.a * circle * _Opacity ;
                
                return fixed4(cloudtex.rgb * opacity, opacity);
            }
            ENDCG
            
        }
    }
}
