Shader "Tom/warp"
{
    Properties
    {
        _MainTex ("RGB：Color A：Alpha", 2d) = "white" { }
        _Opacity ("Opacity", range(0, 1)) = 0.5
        _WarpTex ("WarpTex", 2d) = "gray" { }
        _WarpInt ("WarpInt", range(0, 1)) = 0.1
        _NoiseInt ("NoiseInt", range(0, 5)) = 1
        _FlowSpeed ("FlowSpeed", range(-10, 10)) = 5
        _Radius ("Radius", float) = 0.2
        _Fade ("Fade", float) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        Pass
        {
            Tags { "RenderType" = "Transparent" }
            Blend One OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            half _Opacity;
            sampler2D _WarpTex;    float4 _WarpTex_ST;
            half _WarpInt;
            half _NoiseInt;
            half _FlowSpeed;
            half _Radius, _Fade;

            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
            };
            
            struct v2f
            {
                float4 pos: SV_POSITION;
                float2 uv0: TEXCOORD0;
                float2 uv1: TEXCOORD1;
                float2 maskuv: TEXCOORD2;
            };
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv0 = v.uv;
                o.maskuv = v.uv;
                o.uv1 = TRANSFORM_TEX(v.uv, _WarpTex);
                o.uv1.y = o.uv1.y + frac(_Time.x * _FlowSpeed);
                return o;
            }
            
            fixed4 frag(v2f i): SV_TARGET
            {
                half3 var_WarpTex = tex2D(_WarpTex, i.uv1);
                half2 uvBias = (var_WarpTex.rg) * _WarpInt * 0.1;
                half2 uv0 = i.uv0 + uvBias;

                half4 var_MainTex = tex2D(_MainTex, uv0);
                float circle = 1 - smoothstep(_Radius, (_Radius + _Fade), length(i.maskuv * 2 - 1));
                // return 1 - circle;
                
                half3 finalRGB = var_MainTex.rgb ;
                half noise = lerp(1, var_WarpTex.b * 2, _NoiseInt);
                noise = max(0.0, noise);

                half opacity = var_MainTex.a * _Opacity * noise * circle;

                return half4(finalRGB * opacity, opacity);
            }
            ENDCG
            
        }
    }
}