Shader"Decryption/ParticlesAddStencil"
{
    Properties
    {
        [Header(RenderingMode)]
        [Space(5)]
        [Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend ("Src Blend", int) = 0
        [Enum(UnityEngine.Rendering.BlendMode)]_DstBlend ("Dst Blend", int) = 0
        [Enum(UnityEngine.Rendering.CullMode)]_Cull ("Cull", int) = 0
        
        [Header(Base)]
        [Space(5)]
        _MainColor ("MainColor", color) = (0.8, 0.8, 0.8, 0.5)
        _Intensity ("Intensity", Range(0, 10)) = 1
        _MainTex ("MainTex", 2D) = "white" { }

         _StencilRef("Stencil Ref", Float) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilCompare("Stencil Compare", Float) = 0
    }
    
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        Blend [_SrcBlend] [_DstBlend]
        Cull [_Cull]
        ColorMask RGBA
        ZWrite Off
        ZTest LEqual
        

        Stencil
        {
            Ref[_StencilRef]
            Comp[_StencilCompare]
        }
        
        pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            
            
            
            
            sampler2D _MainTex; float4 _MainTex_ST;
            fixed4 _MainColor;
            fixed _Intensity;
            fixed4 _color;
            
            struct appdata
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex: POSITION;
                float2 uv: TEXCOORD;
                fixed4 color: COLOR;
            };
            
            struct v2f
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 pos: SV_POSITION;
                float2 uv: TEXCOORD;
                fixed4 color: COLOR;
            };
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag(v2f i): SV_TARGET
            {
                fixed4 c;
                fixed4 tex = tex2D(_MainTex, i.uv);
                c = tex;
                c *= (_MainColor * _Intensity) * _MainColor.a * tex.a * i.color * i.color.a;
                return c;
            }
            
            ENDCG
            
        }
    }
}