Shader "Decryption/GuideVignette"
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
        _Width ("Width", float) = 0.06
        _Heigh ("Heigh", float) = 0.03
        _Fillet ("Fillet", float) = 3
        _Wfade ("WFade", float) = 0.15
        _Hfade ("Hfade", float) = 0.25
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
            float _Hfade;
            
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
            };
            
            
            
            v2f vert(appdata v)
            {
                float4 tmpvar_2 = UnityObjectToClipPos(v.vertex);
                float2 tmpvar_3 = (tmpvar_2.xy / tmpvar_2.w);
                
                float2 tmpvar_Cir;
                tmpvar_Cir.x = (tmpvar_3.x + _offsetX);
                tmpvar_Cir.y = (tmpvar_3.y + _offsetY) * 0.58;
                
                float2 tmpvar_squ;
                tmpvar_squ.x = (tmpvar_3.x + _offsetX) ;
                tmpvar_squ.y = (tmpvar_3.y + _offsetY) * 2.5 - 1.5;
                float2 tmpvar = lerp(tmpvar_squ, tmpvar_Cir, _Conver);
                
                
                v2f o;
                o.vertex = tmpvar_2;
                o.uv = tmpvar;
                o.maskuv = v.uv;
                return o;
            }
            
            fixed4 frag(v2f i): SV_TARGET
            {
                fixed4 col;
                
                col.rgb = fixed3(0.0, 0.0, 0.0);
                
                
                //圆
                // smoothstep(_Radius, (_Radius + _Fade), length(i.uv * 2 - 1));
                
                //方
                // float2 centerUV = clamp(pow(abs(i.maskuv.x * 2 - 1), 5) + pow(abs(i.maskuv.y * 2 - 1), 5), 0, 1);
                // float rectangleX = smoothstep(_Width, (_Width + _RectangleFade), centerUV.x);
                // float rectangleY = smoothstep(_Heigth, (_Heigth + _RectangleFade), centerUV.y);
                // float rectangleClamp = clamp((rectangleX + rectangleY), 0.0, 1.0);
                
                
                float cir = smoothstep(_Radius, (_Radius + _Fade), length(i.maskuv * 2 - 1));
                
                // return col * 0.8 ;
                float2 centerUV = clamp(pow(abs(i.maskuv.xy * 2 - 1), _Fillet), 0, 1);
                // float2 centerUV = clamp(pow(abs(i.uv.x * 2 - 1), _Fillet) + pow(abs(i.uv.y * 2 - 1), _Fillet), 0, 1);
                float rectangleX = smoothstep(_Width, (_Width + _Wfade), centerUV.x);
                float rectangleY = smoothstep(_Heigh, (_Heigh + _Hfade), centerUV.y);
                float squ = clamp((rectangleX + rectangleY), 0.0, 1.0);
                col.a = lerp(squ, cir, _Conver) * _Opacity;
                return col;
            }
            
            ENDCG
            
        }
    }
}
