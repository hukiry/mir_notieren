
/*模型视图映射到UI界面
*/
Shader "Hidden/RawViewTexture"
{
    Properties
    {
       _Color("Color",color)=(.4,.4,.4,.5)
    }
    SubShader
    {  
		LOD 100  
        // No culling or depth
        Cull Off ZWrite Off 
		Blend SrcAlpha OneMinusSrcAlpha  

		 Tags  
        {  

            "Queue" ="Overlay"  

            "IgnoreProjector" ="True"  

            "RenderType" ="Transparent"  

        }  

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 _Color;
            fixed4 frag (v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}
