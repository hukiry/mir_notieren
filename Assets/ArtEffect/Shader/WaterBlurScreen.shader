// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//毛玻璃模糊效果
//整个屏幕的模糊效果
Shader "Custom/WaterBlurScreen" {
    Properties {
		_blurSizeXY("BlurSize_XY", Range(0,10)) = 1.5  //2为正常模糊 
		_GlassAlpha("GlassAlpha",Range(0.01,0.1))=0.05//0.1完全透明
	}
    SubShader 
	{
        Tags { "Queue" = "Transparent" }

        // Grab the screen behind the object into _GrabTexture
        GrabPass { }//对屏幕采样，用于制作模糊，玻璃效果
        Pass {
			CGPROGRAM
			#pragma debug
			#pragma vertex vert
			#pragma fragment frag 
			#pragma target 3.0
 
			sampler2D _GrabTexture : register(s0);
			float _blurSizeXY;//模糊xy的大小
			float _GlassAlpha;//玻璃透明值
 
			struct data {
				float4 vertex : POSITION;
			};
 
			struct v2f {
				float4 position : SV_POSITION;
				float4 screenPos : TEXCOORD0;
			};
 
			v2f vert(data i){
 
				v2f o;
 
				o.position = UnityObjectToClipPos(i.vertex);
 
				o.screenPos = o.position;
 
				return o;
 
			}
			     float3 _Color;

			half4 frag( v2f i ) : COLOR
			{
 
				float2 screenPos = i.screenPos.xy / i.screenPos.w;//屏幕位置

				screenPos.x = (screenPos.x + 1) * 0.5;
 				screenPos.y = 1-(screenPos.y + 1) * 0.5;

				half4 sum;  
				// sum = tex2D( _GrabTexture, screenPos+1* depth) * _GlassAlpha; //不够模糊，只是适合做水的波动
				//改成如下
				for(int i = 0; i <5; ++i)
				{
					float depthOffset=i*_blurSizeXY*0.0005;//微量偏移
					sum += tex2D( _GrabTexture, screenPos-depthOffset) * _GlassAlpha; //左偏移纹理采样
					sum += tex2D( _GrabTexture, screenPos+depthOffset) * _GlassAlpha; //右偏移纹理采样
				}

				return sum;
 
			}
			ENDCG
        }
    }
	Fallback Off
}