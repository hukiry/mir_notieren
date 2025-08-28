Shader"Decryption/UVDistort"
{
    Properties
    {
        [Header(RenderingMode)]
        [Space(5)]
        [Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend ("Src Blend", int) = 0
        [Enum(UnityEngine.Rendering.BlendMode)]_DstBlend ("Dst Blend", int) = 0
        [Enum(UnityEngine.Rendering.CullMode)]_Cull ("Cull", int) = 0
        [Space(10)]
        
        [Header(Base)]
        [Space(5)]
        _MainColor ("MainColor", color) = (0.5, 0.5, 0.5, 0.5)
        _Intensity ("Intensity", Range(0, 10)) = 1
        _MainTex ("MainTex", 2D) = "white" { }
        _MainUVSpeedX ("MainUVSpeed X", float) = 0
        _MainUVSpeedY ("MainUVSpeed Y", float) = 0
        [Space(10)]
        
        [Header(Mask)]
        [Space(5)]
        [Toggle]_MaskEnabled ("MaskEnabled", int) = 0
        [NoScaleOffset]_MaskTex ("MaskTex", 2D) = "white" { }
        _MaskUVSpeedX ("MaskUVSpeed X", float) = 0
        _MaskUVSpeedY ("MaskUVSpeed Y", float) = 0
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
        Blend [_SrcBlend] [_DstBlend]
        Cull [_Cull]
        ColorMask RGBA
        ZWrite Off
        ZTest LEqual
        
        
        
        pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _ _MASKENABLED_ON
            #pragma shader_feature _ _DISTORTENABLED_ON
            #include "UnityCG.cginc"
            
            
            sampler2D _MainTex; float4 _MainTex_ST;
            fixed4 _MainColor;
            fixed _Intensity;
            float _MainUVSpeedX, _MainUVSpeedY;
            
            sampler2D _MaskTex; float4 _MaskTex_ST;
            float _MaskUVSpeedX, _MaskUVSpeedY;
            
            sampler2D _DistortTex; float4 _DistortTex_ST;
            float _Distort, _DistortUVSpeedX, _DistortUVSpeedY;
            
            
            struct appdata
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex: POSITION;
                float4 uv: TEXCOORD;
                float2 uv2: TEXCOORD1;
                fixed4 color: COLOR;
            };
            
            struct v2f
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 pos: SV_POSITION;
                float4 uv: TEXCOORD;
                float2 uv2: TEXCOORD1;
                fixed4 color: COLOR;
            };
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex) + frac(float2(_MainUVSpeedX, _MainUVSpeedY) * _Time.y);
                o.uv.zw = TRANSFORM_TEX(v.uv, _MaskTex) + float2(_MaskUVSpeedX, _MaskUVSpeedY) * _Time.y ;
                o.uv2 = TRANSFORM_TEX(v.uv, _DistortTex) + float2(_DistortUVSpeedX, _DistortUVSpeedY) * _Time.y  ;
                
                
                return o;
            }
            
            fixed4 frag(v2f i): SV_TARGET
            {
                
                fixed4 c;
                c = _MainColor * _Intensity * _MainColor.a * i.color * i.color.a;
                
                float2 distort = i.uv.xy;
                
                #if _DISTORTENABLED_ON
                    fixed4 distortTex = tex2D(_DistortTex, i.uv2);
                    distort = lerp(i.uv.xy, distortTex, _Distort);
                #endif
                
                fixed4 MainTex = tex2D(_MainTex, distort);
                c *= MainTex * MainTex.a;
                
                
                #if _MASKENABLED_ON
                    fixed4 mask = tex2D(_MaskTex, i.uv.zw);
                    c *= mask;
                #endif
                
                
                return c;
            }
            
            ENDCG
            
        }
    }
}