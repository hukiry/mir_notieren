Shader "Decryption/WaveSpray"
{
    Properties
    {
        [Header(Render)]
        [Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend ("Src Blend", int) = 0
        [Enum(UnityEngine.Rendering.BlendMode)]_DstBlend ("Dst Blend", int) = 0
        [Space(10)]
        _WaterCol ("WaterCol", color) = (1, 1, 1, 1)
        _NoiseTex ("NoiseTex", 2D) = "white" { }
        _WaveTex ("WaveTex", 2D) = "white" { }
        _Wetedge ("watedge", color) = (0.5, 0.5, 0.5, 0.5)
        _uv ("UV", float) = 1
        _Speed ("Speed", float) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100
        
        Pass
        {
            Tags { "RenderType" = "Transparent" }
            Blend [_SrcBlend] [_DstBlend]
            ZWrite Off
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            //--------------------------------------------------------------------
            sampler2D _NoiseTex, _WaveTex;
            float4 _NoiseTex_ST, _WaveTex_ST;
            fixed4 _WaterCol, _Wetedge;
            half _uv, _Speed;
            //--------------------------------------------------------------------
            
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
                fixed4 color: COLOR;
            };
            
            struct v2f
            {
                float2 uv: TEXCOORD0;
                float4 pos: SV_POSITION;
                fixed4 color: TEXCOORD1;
                float2 waveuv: TEXCOORD2;
            };
            
            
            
            v2f vert(appdata v)
            {
                v2f o;
                o.color = v.color;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _NoiseTex) * float2(1, 1) - float2(1, 1) * (_Time.y * 0.125) ;
                o.waveuv = TRANSFORM_TEX(v.uv, _WaveTex) - float2(0, _uv) * sin(_Time.y * _Speed);
                
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                // sample the texture
                fixed col = tex2D(_NoiseTex, i.uv)  ;
                fixed4 wavemask = tex2D(_WaveTex, i.waveuv);
                
                fixed4 water = _WaterCol * wavemask.r * 2;
                float c = col * wavemask.g * 1.25 ;
                fixed4 wet = _Wetedge * wavemask.b * 0.35;
                fixed4 final = ((c * 3 + water) + wet) * i.color.r;
                
                return final;
            }
            ENDCG
            
        }
    }
}
