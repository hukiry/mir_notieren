//边框流动:不可使用图集
Shader "Hukiry/BorderFlow"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("使用透明度裁剪", Float) = 0
		_FlowPos("流光位置", Range(0, 1)) = 0
		_FlowStartColor("流光开始颜色", Color) = (1,0.4,0)
		_FlowColor("流光结束颜色", Color) = (1,0.7,0)
		_FlowWidth("流光区域宽度", Range(0, 1)) = 0.3
		_FlowThickness("流光区域厚度", Range(0, 1)) = 0.1
		_FlowBrightness("流光亮度", Range(0, 1)) = 1
		_FlowSpeed("流光速度", Range(1, 10)) = 5
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			Name "Default"

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			fixed4 _TextureSampleAdd;
			half _FlowPos;
			fixed3 _FlowColor;
			fixed3 _FlowStartColor;
			half _FlowWidth;
			half _FlowThickness;
			half _FlowBrightness;
			half _FlowSpeed;

			//片元处理输入数据（标准）
			struct FragData
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			//顶点处理输入数据（标准）
			struct VertData
			{
				float4 vertex   : POSITION;
				fixed4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};


			//求一个点是否在指定方形区域内，是则返回1，否则返回0
			fixed IsInRect(half4 rect, half2 point2)
			{
				half width = rect.z * 0.5;
				half height = rect.w * 0.5;
				fixed left = step(rect.x - width, point2.x);
				fixed right = step(point2.x, rect.x + width);
				fixed up = step(rect.y - height, point2.y);
				fixed down = step(point2.y, rect.y + height);
				return left * right * up * down;
			}

			//如果条件 condition == 1，返回 trueValue，如果 condition == 0，返回 falseValue
			half If(fixed condition, half trueValue, half falseValue)
			{
				return trueValue * condition + falseValue * (1 - condition);
			}

			//为一个uv区域应用边框流动
			half4 ApplyBorderFlow(half4 color, float2 uv, half flowPos, half flowWidth, half flowThickness, half flowBrightness, fixed3 flowColor, float2 texelSize)
			{
				//计算上下边框的宽、高
				half width = flowWidth * 0.5;
				half height = flowThickness * 0.5;

				//绘制上边框
				//计算当前流光位置
				half ratio = smoothstep(-width, 0.5, If(step(flowPos, 0.5), flowPos, flowPos - 1));
				//将流光映射到图像上的真实位置
				half realPos = lerp(width * -1, 1 + width, ratio);
				//计算当前流光强度
				half brightness = IsInRect(half4(realPos, 1 - height, width * 2, height * 2), uv) * flowBrightness;
				//将流光区域平滑（使得越靠近区域右侧，流光强度越接近1，越靠近区域左侧，流光强度越接近0）
				brightness *= smoothstep(0, width * 2, uv.x - realPos + width);
				//将流光颜色叠加到主颜色
				color.rgb += color.a * brightness * flowColor;

				//绘制下边框（原理同上边框）
				realPos = lerp(width * -1, 1 + width, 1 - ratio);
				brightness = IsInRect(half4(realPos, height, width * 2, height * 2), uv) * flowBrightness;
				brightness *= smoothstep(0, width * 2, realPos - uv.x + width);
				color.rgb += color.a * brightness * flowColor;

				//计算左右边框的宽、高（保证在图像的宽、高不等时，流光的宽、高值保持一致）
				width = width * texelSize.x / texelSize.y;
				height = height * texelSize.y / texelSize.x;

				//绘制左边框（原理同上边框）
				ratio = smoothstep(0.5 - width, 1, flowPos);
				realPos = lerp(width * -1, 1 + width, ratio);
				brightness = IsInRect(half4(height, realPos, height * 2, width * 2), uv) * flowBrightness;
				brightness *= smoothstep(0, width * 2, uv.y - realPos + width);
				color.rgb += color.a * brightness * flowColor;

				//绘制右边框（原理同上边框）
				realPos = lerp(width * -1, 1 + width, 1 - ratio);
				brightness = IsInRect(half4(1 - height, realPos, height * 2, width * 2), uv) * flowBrightness;
				brightness *= smoothstep(0, width * 2, realPos - uv.y + width);
				color.rgb += color.a * brightness * flowColor;

				return color;
			}

			//顶点处理方法（标准）
			FragData vert(VertData IN)
			{
				FragData OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				return OUT;
			}

			fixed4 frag(FragData IN) : SV_Target
			{
				
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

				_FlowPos = fmod(_Time.x*_FlowSpeed, 1.0);
				fixed3 newColor = lerp(_FlowStartColor, _FlowColor, _FlowPos);
				//应用边框流动特效
				color = ApplyBorderFlow(color, IN.texcoord, _FlowPos, _FlowWidth, _FlowThickness, _FlowBrightness, newColor, _MainTex_TexelSize.zw);

				#ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
				#endif
				
				return color;
			}


			ENDCG
		}
	}
}