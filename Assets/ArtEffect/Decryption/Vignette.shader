Shader "Decryption/Vignette"
{
    Properties
    {
        [HideInInspector]_MainTex ("MainTex", 2D) = "white" { }
        _offsetX ("OffsetX", float) = 0
        _offsetY ("OffsetY", float) = 0
        _Opacity ("Opacity", Range(0, 1)) = 0.9
        [Toggle]_Conver ("ConverCircle(换圆形)", int) = 0
        
        [Space(5)]
        [Header(Rectangle)]
        _Width ("Width", float) = 0.8
        _Heigh ("Heigh", float) = 0.2
        _Fillet ("Fillet", float) = 1
        _Wfade ("WFade", float) = 0.15
        [Space(5)]
        [Header(Circle)]
        _Radius ("Radius", float) = 0.25
        _Fade ("Fade", float) = 0.5
    }
    
    SubShader
    {
        Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
        Pass
        {
            ZWrite Off
            Blend One OneMinusSrcAlpha
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            // sampler2D _MainTex;
            float _offsetX;
            float _offsetY;
            float _Opacity ;
            float _intensity;
            float _Width;
            float _Heigh;
            float _Fillet;
            float _fuzzy;
            float _Conver;
            float _Wfade, _Radius, _Fade;
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD;
            };
            
            
            struct v2f
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD;
                float2 maskuv: TEXCOORD1;
                float2 ciruv: TEXCOORD2;
            };
            
            
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.maskuv = v.uv + float2(_offsetX, _offsetY);
                o.ciruv = v.uv + float2(_offsetX, _offsetY) ;
                // float2 tmpvar = lerp(o.maskuv, o.ciruv, _Conver);
                return o;
            }
            
            fixed4 frag(v2f i): SV_TARGET
            {
                fixed4 col;
                
                col.rgb = fixed3(0.0, 0.0, 0.0);
                
                float cir = smoothstep(_Radius * 0.1, (_Radius * 0.1 + _Fade * 0.1), length(i.maskuv * 2 - 1));
                
                // return col * 0.8 ;
                float2 centerUV = clamp(pow(abs(i.maskuv.xy * 2 - 1), _Fillet), 0, 1);
                // float2 centerUV = clamp(pow(abs(i.uv.x * 2 - 1), _Fillet) + pow(abs(i.uv.y * 2 - 1), _Fillet), 0, 1);
                float rectangleX = smoothstep(_Width * 0.1, (_Width * 0.1 + _Wfade * 0.1), centerUV.x);
                float rectangleY = smoothstep(_Heigh * 0.1, (_Heigh * 0.1 + _Wfade * 0.1), centerUV.y);
                float squ = clamp((rectangleX + rectangleY), 0.0, 1.0);
                col.a = lerp(squ, cir, _Conver) * _Opacity;
                return col;
            }
            
            ENDCG
            
        }
    }
}
