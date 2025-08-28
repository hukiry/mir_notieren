Shader"Tom/EffectSweep"
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
        _Rota ("Rota", float) = 0
        _MainUVSpeedX ("MainUVSpeed X", float) = 0
        _MainUVSpeedY ("MainUVSpeed Y", float) = 0
        [Space(10)]
        
        [Header(Mask)]
        [Space(5)]
        [Toggle]_MaskEnabled ("MaskEnabled", int) = 0
        [NoScaleOffset]_MaskTex ("MaskTex", 2D) = "white" { }
        [HideInInspector]_MaskUVSpeedX ("MaskUVSpeed X", float) = 0
        [HideInInspector]_MaskUVSpeedY ("MaskUVSpeed Y", float) = 0
    }
    
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        Blend [_SrcBlend] [_DstBlend]
        Cull [_Cull]
        ColorMask RGBA
        ZWrite Off
        ZTest LEqual
        Lighting Off
        
        
        
        pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MASKENABLED_ON
            #include "UnityCG.cginc"
            
            fixed4 _MainColor;
            fixed _Intensity;
            
            sampler2D _MainTex; float4 _MainTex_ST;
            float _MainUVSpeedX, _MainUVSpeedY;
            float _Rota;
            
            sampler2D _MaskTex; float4 _MaskTex_ST;
            float _MaskUVSpeedX, _MaskUVSpeedY;
            
            
            
            struct appdata
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex: POSITION;
                float4 uv: TEXCOORD;
                fixed4 color: COLOR;
            };
            
            struct v2f
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 pos: SV_POSITION;
                float4 uv: TEXCOORD;
                fixed4 color: COLOR;
            };
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex) + float2(_MainUVSpeedX, _MainUVSpeedY) * _Time.y;
                o.uv.zw = TRANSFORM_TEX(v.uv, _MaskTex) + float2(_MaskUVSpeedX, _MaskUVSpeedY) * _Time.y ;
                
                
                return o;
            }
            
            fixed4 frag(v2f i): SV_TARGET
            {
                
                
                fixed4 c;
                c = _MainColor * _Intensity * _MainColor.a * i.color*i.color.a;
                
                
                float Rote = (_Rota * 3.1415926) / 180;
                float sinNum = sin(Rote);
                float cosNum = cos(Rote);
                
                float2 rotator = mul(i.uv.xy - float2(0.5, 0.5), float2x2(cosNum, -sinNum, sinNum, cosNum)) + float2(0.5, 0.5);
                
                
                fixed4 MainTex = tex2D(_MainTex, rotator);
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