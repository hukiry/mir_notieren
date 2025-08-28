Shader "Decryption/Sequence"
{
    Properties
    {
        _Opacity ("Opacity", range(0, 1)) = 0.5
        _MainCol ("MainCol", color) = (1, 1, 1, 1)
        _Intensity ("Intensity", float) = 1
        _Sequence ("Sequence Tex", 2d) = "gray" { }
        _ColCount ("Tiles X", int) = 1
        _RowCount ("Tiles Y", int) = 1
        _Speed ("Speed", range(0.0, 3.0)) = 1
        [Space(10)]
        
        [Header(Distort)]
        [Toggle]_DistortEnabled ("DistortEnabled", int) = 0
        [Space(5)]
        _DistortTex ("DistortTex", 2D) = "white" { }
        _Distort ("Distort", Range(0, 1)) = 0
        _DistortUVSpeedX ("DistortUVSpeed X", float) = 0
        _DistortUVSpeedY ("DistortUVSpeed Y", float) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }

        

        Pass
        {
            Tags { "RenderType" = "Transparent" }
            Blend One One          // 混合方式
            ZWrite Off             //关闭深度
            Cull off
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma shader_feature _ _DISTORTENABLED_ON

            #include "UnityCG.cginc"

            sampler2D _Sequence;  float4 _Sequence_ST;
            sampler2D _DistortTex; float4 _DistortTex_ST;
            half _Opacity;
            half _RowCount;
            half _ColCount;
            half _Speed;
            half _Intensity;
            half _Distort, _DistortUVSpeedX, _DistortUVSpeedY;
            fixed4 _MainCol;

            struct appdata
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
                float2 duv: TEXCOORD1;
                fixed4 color: COLOR;
            };
            
            struct v2f
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 pos: SV_POSITION;
                float2 uv: TEXCOORD0;
                fixed4 color: TEXCOORD1;
                float2 uv2: TEXCOORD2;
                float2 duv: TEXCOORD3;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Sequence);      // 前置UV ST操作
                float id = floor(_Time.z * _Speed);         // 计算序列id
                float idV = floor(id / _ColCount);          // 计算V轴id
                float idU = id - idV * _ColCount;           // 计算U轴id
                float stepU = 1.0 / _ColCount;              // 计算U轴步幅
                float stepV = 1.0 / _RowCount;              // 计算V轴步幅
                float2 initUV = o.uv * float2(stepU, stepV) + float2(0.0, stepV * (_ColCount - 1.0));   // 计算初始UV
                o.uv = initUV + float2(idU * stepU, idV * stepV);   // 计算序列帧UV
                o.uv2 = TRANSFORM_TEX(v.uv, _DistortTex) + float2(_DistortUVSpeedX, _DistortUVSpeedY) * _Time.y ;
                o.color = v.color;

                return o;
            }

            fixed4 frag(v2f i): SV_TARGET
            {

                float2 distort = i.uv;
                
                #if _DISTORTENABLED_ON
                    fixed4 distortTex = tex2D(_DistortTex, i.uv2);
                    distort = lerp(i.uv, i.uv - distortTex, _Distort * 0.1);
                #endif

                half4 var_Sequence = tex2D(_Sequence, distort);

                
                half3 finalRGB = var_Sequence.rgb * _MainCol.rgb * _Intensity * 2;
                half opacity = var_Sequence.a * _Opacity * i.color * _MainCol.a;
                return fixed4(finalRGB * opacity, opacity);
            }
            ENDCG
            
        }
    }
}