Shader "Custom/GrayGrabPass"
{
    Properties
    {
		_R("R0.3",Range(0,1))=0.3
		_G("G0.59",Range(0,1))=0.59
		_B("B0.11",Range(0,1))=0.11
		 [Toggle]_IsGray ("开启灰度", Float) = 0
	}

    SubShader
    {
        // 在所有不透明几何体之后绘制自己
        Tags { "Queue" = "Transparent-100" }

        // 将对象后面的屏幕抓取到 _BackgroundTexture 中
        GrabPass
        {
            "_BackgroundTexture"
        }

        // 使用上面生成的纹理渲染对象，并灰度颜色
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 grabPos : TEXCOORD0;
                float4 pos : SV_POSITION;
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

            sampler2D _BackgroundTexture;
			half _R,_G,_B;
			half _IsGray;

            half4 frag(v2f i) : SV_Target
            {
                half4 bgcolor = tex2Dproj(_BackgroundTexture, i.grabPos);

				if (_IsGray)
				{	
					//屏幕纹理和灰度系数相乘
					half3 gray=dot(bgcolor.rgb,half3(_R,_G,_B));
					return half4(gray,bgcolor.a);
				}
				else
					return bgcolor;	
            }
            ENDCG
        }

    }

	//自定义 Shader 编辑器  
	//放在Editor目录下 ShaderViewEditor.cs 继承 ShaderGUI。并重新父类OnGUI
	CustomEditor "ShaderViewEditor"
}