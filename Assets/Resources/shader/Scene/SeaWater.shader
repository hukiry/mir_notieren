Shader "Hukiry/SeaWater"
{
    Properties
    {
        _LightDir ("方向 (X Y Z)", vector) = (0, 0, 0, 0)
        _MainCol ("Main Color", color) = (0.5, 0.5, 0.5, 1)
        _DeepCol ("深度颜色", color) = (0.12, 0.25, 0.32, 1)
        _MainTex ("MainTex", 2D) = "white" { }
        _RipTex ("波纹", 2D) = "white" { }
        _WaveTex ("波纹法线", 2D) = "bump" { }
        _WaveSpeed ("波纹速度", vector) = (1, 1, 1, 1)
        _WarpInt ("WarpInt", Range(0.2,1.0)) = 0.2
        _Opacity ("Opacity", Range(0.5,1.0)) = 0.5
        _Shininess ("亮度",Range(0.1,0.8))=0.6
        [HDR]_SpecularCol("高亮颜色", color) = (1,1,1,1)
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
            half _Shininess;
            fixed4 _SpecularCol;
            fixed4 _MainCol, _DeepCol;
            half _Opacity;
            
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
                float2 waveuv: TEXCOORD1;
                float2 MaintexUV: TEXCOORD2;
                float2 RipUV: TEXCOORD3;
                half3 worldNormal: TEXCOORD4;
                float4 worldPos: TEXCOORD5;
                float3 dirn: TEXCOORD6;
                float3 dirt: TEXCOORD7;
                float3 dirb: TEXCOORD8;
                fixed4 color: TEXCOORD9;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.dirn = UnityObjectToWorldNormal(v.normal);
                o.dirt = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
                o.dirb = normalize(cross(o.dirn, o.dirt) * v.tangent.w);
                
                o.pos = UnityObjectToClipPos(v.vertex);
                o.MaintexUV = v.uv;
                o.RipUV = TRANSFORM_TEX(v.uv, _RipTex) - frac(_Time.x * 0.95);
                o.uv.xy = TRANSFORM_TEX(v.uv, _WaveTex) * 10 - frac(float2(_Time.x * _WaveSpeed.xy));
                o.uv.zw = TRANSFORM_TEX(v.uv, _WaveTex) * 8 + frac(float2(_Time.x * _WaveSpeed.zw));
                o.waveuv = TRANSFORM_TEX(v.uv, _WaveTex) * float2(1, 2) - frac(_Time.x * 0.75);
                o.color = v.color;
                return o;
            }

            fixed4 circle(float2 uv, float2 center, float radius)
			{
                if (length(uv - center) < radius) return _SpecularCol;
				else return fixed4(0, 0, 0, 0);
			}
            
            fixed4 frag(v2f i): SV_Target
            {
                // sample the texture
                fixed3 wavetex1 = tex2D(_WaveTex, i.uv.xy);
                fixed3 wavetex2 = tex2D(_WaveTex, i.uv.zw);
                fixed3 wave = tex2D(_WaveTex, i.waveuv);
                float2 NormalMap =( wavetex1 + wavetex2+ wave ) * 0.5;
                float3x3 dirws = float3x3(i.dirt, i.dirb, i.dirn);
                float3 nDir = normalize(mul(dirws, NormalMap));
                float lambert = cos(dot(nDir, normalize(_LightDir))) * 0.5 + 0.5;
                
                float2 cuv = (i.MaintexUV + NormalMap * _WarpInt)*0.5 + 0.5;
                fixed rip = tex2D(_RipTex, i.RipUV + NormalMap*0.2) * 0.3;
                fixed4 c = tex2D(_MainTex, cuv) *1.25;
                c += rip ;

                fixed4 decol = _DeepCol * i.color.r;
                fixed4 mcol = _MainCol * i.color.b ;
                c.rgb *= lambert * (decol + mcol);
                float Opacity = i.color * _Opacity;

				half2 center = half2(wavetex1.r, wavetex1.r)*0.7;
                fixed4 cir = circle(wavetex1.xy, center, frac(rip*_Shininess));
                c.rgb +=cir.rgb;
                
                return fixed4(c.rgb, Opacity);
            }
            ENDCG
            
        }
    }
}
