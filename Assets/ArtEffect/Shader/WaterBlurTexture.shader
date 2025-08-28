
//毛玻璃模糊效果
//附加到纹理上模型
Shader "Custom/WaterBlurTexture" {
    Properties {
		_blurSizeXY("BlurSize_XY", Range(0,10)) = 2  //2为正常模糊 
		_GlassAlpha("GlassAlpha",Range(0.02,0.1))=0.02//0.1完全透明
		//_Color("Color",Color)=(1,1,1,1)//0.1完全透明
	}
    SubShader 
	{
        Tags { "Queue" = "Transparent" }

        // Grab the screen behind the object into _GrabTexture
        GrabPass {
			"_BackgroundTexture"
		}//对屏幕采样，用于制作模糊，玻璃效果

        Pass {
			CGPROGRAM
			#pragma debug
			#pragma vertex vert
			#pragma fragment frag 
			#pragma target 3.0
			#include "UnityCG.cginc"
 
			float _blurSizeXY;//模糊xy的大小
			float _GlassAlpha;//玻璃透明值
			sampler2D _BackgroundTexture;
			//float3 _Color;

			struct v2f {
				float4 pos : SV_POSITION;
				float4 grabPos : TEXCOORD0;
			};

			

            v2f vert(appdata_base v) {
                v2f o;
                // 使用 UnityCG.cginc 中的 UnityObjectToClipPos 来计算
                // 顶点的裁剪空间
                o.pos = UnityObjectToClipPos(v.vertex);
                // 使用 UnityCG.cginc 中的 ComputeGrabScreenPos 函数
                // 获得正确的纹理坐标
                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }
			     

			half4 frag( v2f i ) : COLOR
			{
 
				float2 screenPos = i.grabPos.xy / i.grabPos.w;//屏幕位置

				half4 sum;// = tex2D( _BackgroundTexture, screenPos) * _GlassAlpha; //不够模糊，只是适合做水的波动
				//改成如下
				for(int i = 0; i <5; ++i)
				{
					float depthOffset=i*_blurSizeXY*0.0005;//微量偏移
					sum += tex2D( _BackgroundTexture, screenPos-depthOffset) * _GlassAlpha; //左偏移纹理采样
					sum += tex2D( _BackgroundTexture, screenPos+depthOffset) * _GlassAlpha; //右偏移纹理采样
				}
				//float3 sumLerp=lerp(sum.rgb,_Color.rgb,0.5);
				//return half4(sumLerp,sum.a);
				return sum;
			}
			ENDCG
        }
    }
	Fallback Off
}