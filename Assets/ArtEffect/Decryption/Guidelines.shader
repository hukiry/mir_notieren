Shader "Decryption/UI/Guidelines"
{
    Properties
    {
        _Maincol ("MainCol", color) = (1, 1, 1, 1)
        _MainTex ("MainTex", 2D) = "white" { }
        _Opacity ("Opacity", Range(0, 1)) = 0.5
        _Radius ("Radius", float) = 0.5
        _Fade ("Fade", float) = 0.25
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100
        
        Pass
        {
            Blend one OneMinusSrcAlpha
            ZWrite Off
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            fixed4 _Maincol;
            half _Fade, _Radius, _Opacity;
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv: TEXCOORD0;
                float4 vertex: SV_POSITION;
                float2 texuv: TEXCOORD1;
            };
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.texuv = v.uv;
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.texuv);
                // return tex;
                float y = smoothstep(_Radius, (_Radius + _Fade), length(i.uv * 2 - 1));
                c *= y * _Maincol;// sample the texture
                
                return c * _Opacity;
            }
            ENDCG
            
        }
    }
}
