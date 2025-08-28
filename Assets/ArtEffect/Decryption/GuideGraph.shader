Shader "Decryption/GuideGraph"
{
    Properties
    {
        [HideInInspector]_MainTex ("MainTex", 2D) = "white" { }
        _offsetX ("OffsetX", float) = 0
        _offsetY ("OffsetY", float) = 0
        _Opacity ("Opacity", float) = 0.9
        _intensity ("Intensity", float) = 0.9
        _width ("Width", float) = 1
        _height ("Height", float) = 1
        _Fillet ("Fillet", float) = 2
        _fuzzy ("Fuzzy", Range(0, 1)) = 1
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
            float _width;
            float _height;
            float _Fillet;
            float _fuzzy;
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD;
            };
            
            
            struct v2f
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD;
            };
            
            
            
            v2f vert(appdata v)
            {
                float4 tmpvar_2 = UnityObjectToClipPos(v.vertex);
                float2 tmpvar_3 = (tmpvar_2.xy / tmpvar_2.w);
                
                float2 tmpvar_1;
                tmpvar_1.x = (tmpvar_3.x + _offsetX);
                tmpvar_1.y = (tmpvar_3.y + _offsetY) * 2.5;
                
                v2f o;
                o.vertex = tmpvar_2;
                o.uv = tmpvar_1 ;
                
                return o;
            }
            
            fixed4 frag(v2f i): SV_TARGET
            {
                fixed4 col;
                
                col.rgb = fixed3(0.0, 0.0, 0.0);
                float h = _width / 4;
                // col.a = clamp(pow(abs(i.uv.x / 0.5) * _width, _Fillet) + pow(abs(i.uv.y / 0.5) * h, _Fillet), 0.0, 1.0);
                col.a = clamp
                (
                    pow(abs(i.uv.x / 0.5) * _width, _Fillet)
                + pow(abs(i.uv.y / 0.5) * _height, _Fillet),
                0.0,
                1.0
                );
                
                
                // return col.a * 0.8;
                
                if (col.a < 1.0f)
                {
                    col.a *= _Opacity * _intensity * _fuzzy;
                }
                
                else
                {
                    col.a *= _Opacity * _intensity;
                }
                
                
                return col;
            }
            
            ENDCG
            
        }
    }
}
