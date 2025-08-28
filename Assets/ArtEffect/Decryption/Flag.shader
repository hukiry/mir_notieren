Shader "Decryption/Flag"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "gray" { }
        _SwingXPara ("SwingXPara X:Intsity Y:Speed Z:Wave", vector) = (1, 1, 1, 0)
        _SwingZPara ("SwingZPara X:Intsity Y:Speed Z:Wave", vector) = (1, 1, 1, 0)
        _SwingYPara ("SwingYPara X:Intsity Y:Speed Z:Wave", vector) = (1, 1, 1, 0)
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        LOD 100
        
        Pass
        {
            Blend One OneMinusSrcAlpha
            Cull off
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            half3 _SwingXPara, _SwingZPara, _SwingYPara;
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
                float4 color: COLOR;
            };
            
            struct v2f
            {
                float2 uv: TEXCOORD0;
                float4 vertex: SV_POSITION;
                float4 color: COLOR;
            };
            
            #define PI2 6.283185
            
            void flag(inout float3 vertex, inout float3 color)
            {
                
                // 摆动
                float swingX = _SwingXPara.x * sin(frac(_Time.z * _SwingXPara.y + vertex.y * _SwingXPara.z) * PI2);
                float swingZ = _SwingZPara.x * sin(frac(_Time.z * _SwingZPara.y + vertex.y * _SwingZPara.z) * PI2);
                vertex.xz += float2(swingX, swingZ) * (color.r);
                
                
                // // 处理顶点色
                float lightness = color * 1.0 ;
                color = float3(lightness, lightness, lightness);
            }
            
            
            v2f vert(appdata v)
            {
                v2f o;
                flag(v.vertex.xyz, v.color.rgb);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                fixed4 coltex = tex2D(_MainTex, i.uv);
                // fixed3 col = coltex.rgb * i.color.rgb;
                return coltex ;
            }
            ENDCG
            
        }
    }
}
