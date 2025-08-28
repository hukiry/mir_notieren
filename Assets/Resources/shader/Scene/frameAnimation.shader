// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//图片序列帧播放
Shader "Custom/Circle"
{
    Properties
    {
        //[KeywordEnum(X, Y,Z,W)] _FadeWith ("Fade With", Float) = 0 ;类似于枚举[KeywordEnum(X, Y,Z,W)]
         _MainTex ("MainTex", 2D) =""{}
       // [PowerSlider(1)] _Weights ("Weights", Range (0.0, 3.0)) = 2.0
		_h("x",float)=4//列
		_v("y",float)=4//行
		_speed("speed", float )=1
    }
    SubShader
    {
		Tags{
		"Queue"="Transparent" "IgnoreProject"="True" "RenderTyep"="Transparent"
		}
		Cull Off //关闭剔除，正反都可以看见
		ZWrite Off//关闭深度，不写入缓存区
		blend SrcAlpha OneMinusSrcAlpha//开启透明通道
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

            sampler2D _MainTex;
			float4 _MainTex_ST;
			float _h;
			float _v;
			float _speed;
			float _Amount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv,_MainTex);
                return o;
            }

            //注意，uv的最大值是(1, 1)
            fixed4 frag (v2f i) : SV_Target
            {
				float t=floor(_Time.y*_speed);//播放的速度

				float row=floor(t/_v);	//行

				float cell=t-row*_v;	//列

				i.uv+=float2(-cell,row);	//方向偏移

				i.uv=float2(i.uv.x/_h,i.uv.y/_v);//取左上角偏移后的图片

				float4 col=tex2D(_MainTex,i.uv);

				return col;
            }
            ENDCG

        }
    }
}
