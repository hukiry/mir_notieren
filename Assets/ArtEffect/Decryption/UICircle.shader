Shader "Decryption/UI/Circle"
{
    Properties
    {
        _EdgeCol ("EdgeCol", color) = (0, 0, 1, 1)
        _MainCol ("MainCol", color) = (1, 1, 1, 0)
        
        _MainTex ("MainTex", 2D) = "white" { }
        _Mainvalue ("Tile X  Tile Y  Disturb Z  DisturbPow W", vector) = (1, 1, 0.05, 3)
        _NoiseMask ("NoiseTex", 2D) = "gray" { }
        _EdgeRadius ("EdgeRadius", float) = 1
        _EdgeFade ("EdgeFade", float) = 1
        _smallCircle ("SmallCircle", float) = 0.8
        _Radius ("Glow01 Radius", float) = 1
        _CircleFade ("Glow01 CircleFade", float) = 1
        _RotaRange ("RotaRange", float) = 15
        _RotaSpeed ("RotaSpeed", float) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100
        
        Pass
        {
            Tags { "RenderType" = "Transparent+5" }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            
            #include "UnityCG.cginc"
            
            half _Radius, _CircleFade, _EdgeRadius, _EdgeFade, _smallCircle, _RotaRange, _RotaSpeed;
            sampler2D _MainTex, _NoiseMask;
            float4 _MainTex_ST, _NoiseMask_ST;
            fixed4 _MainCol, _EdgeCol;
            half4 _Mainvalue;
            struct appdata
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
                float2 maskuv: TEXCOORD1;
            };
            
            struct v2f
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                
                float2 uv: TEXCOORD0;
                float4 vertex: SV_POSITION;
                float2 nmuv: TEXCOORD1;
                float2 maskuv: TEXCOORD2;
                float2 edgeuv: TEXCOORD3;
                float2 Polaruv: TEXCOORD4;
            };
            
            #define Cycle 6.283185
            
            void Rotation(inout float3 vertex)
            {
                float angleY = _RotaRange * (_Time.y * _RotaSpeed) * Cycle;
                float radY = radians(angleY);
                float sinY, cosY = 0;
                sincos(radY, sinY, cosY);
                vertex.xz = float2(vertex.x * cosY - vertex.z * sinY, vertex.x * sinY + vertex.z * cosY);
            }
            
            v2f vert(appdata v)
            {
                v2f o;
                Rotation(v.vertex.xyz);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.maskuv = v.maskuv;
                o.edgeuv = v.maskuv;
                o.Polaruv = v.uv;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex) - float2(2.0, 2.0) * _Time.x * 2;
                o.nmuv = TRANSFORM_TEX(v.uv, _NoiseMask) + float2(0.0, 2.0) * frac(_Time.x * _Mainvalue.w) ;
                
                return o;
            }
            
            
            
            // 直角坐标转极坐标方法
            float2 RectToPolar(float2 uv, float2 centerUV)
            {
                uv = uv - centerUV;
                float theta = atan2(uv.y, uv.x) ;
                
                float r = length(uv);
                return float2(theta, r);
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                
                
                // 直角坐标转极坐标
                float2 thetaR = RectToPolar(i.Polaruv, float2(0.5, 0.5));
                // 极坐标转纹理采样UV
                float2 polarUV = float2(
                    thetaR.x / 3.141593 * 0.5 + 0.5, // θ映射到[0, 1]
                    thetaR.y - frac(_Time.x * 1.0)      // r随时间流动
                );
                
                
                // 采样噪波贴图
                fixed nm = tex2D(_NoiseMask, i.nmuv);
                
                // 圆形遮罩
                float small = 1 - step(_smallCircle, length(i.edgeuv * 2 - 1)); //UI内部遮罩区域 小圆
                
                float circle = 1 - smoothstep(_Radius, _Radius + _CircleFade, length(i.maskuv * 2 - 1)); //大圆形
                float yuan = circle - small;
                
                yuan *= nm.r ; //增加采样噪波纹理随机
                
                // 边缘光
                float Edgeglow = 1 - smoothstep(_EdgeRadius, _EdgeRadius + _EdgeFade, length(i.edgeuv * 2 - 1));//边缘光
                float Edge = saturate((Edgeglow - small) * (abs(sin(2 * _Time.y)) + 0.15));//边缘光圈减去内部遮罩区域 循环发光动画
                // return Edge;
                
                fixed4 col = tex2D(_MainTex, polarUV * float2(_Mainvalue.x, _Mainvalue.y) + nm.r * _Mainvalue.z) * 2; //采样主纹理贴图
                // return col;
                
                // 最终结果
                fixed4 final = saturate(col * 1.5 * yuan) * _MainCol * 1.65 + Edge * _EdgeCol;
                final.rgb *= _MainCol.a ;
                
                return final;
            }
            ENDCG
            
        }
    }
}

