Shader "Custom/DiffuseBump" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		 _BumpMap ("Bumpmap", 2D) = "bump" {}
		 _FlowTex("Light Texture(A)",2D)="black"{}//流光贴图
		_uvaddspeed("",float)=1//流光uv改变速度
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
      _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
      _Detail ("Detail", 2D) = "gray" {}//细节纹理
       _Cube ("Cubemap", CUBE) = "" {}//立方图反射
       _Amount ("Extrusion Amount", Range(-1,1)) = 0//法线挤压定点修改编成Q版
       _ColorTint ("ColorTint", Color) = (1.0, 0.6, 0.6, 1.0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		BLend SrcAlpha OneMinusSrcAlpha		
		CGPROGRAM
		//增加方法 vertex:vert    继承定点 
		#pragma surface surf Lambert  vertex:vert finalcolor:mycolor

		 sampler2D _MainTex;
		 sampler2D _BumpMap;
		 
		 sampler2D _FlowTex;
		 float _uvaddspeed;   //流光 
		 float4 _RimColor;
     	 float _RimPower;
     	 sampler2D _Detail;
     	 
     	 samplerCUBE _Cube;
     	 float _Amount;
     	 fixed4 _ColorTint;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
		    float4 screenPos;
			float3 worldRefl;
			 float3 worldPos;
			  float3 customColor;
			//可计算复杂的反射效果
			 INTERNAL_DATA
		};
		
		void vert (inout appdata_full v, out Input o) 
		 {
           	 //多个参数接口需要用到这个初始化
           	   UNITY_INITIALIZE_OUTPUT(Input, o);
           	   //挤压变形
           	//  v.vertex.xyz += v.normal * _Amount;
          	 o.customColor = abs(_RimColor.rgb);
      	}
      	
  	 void mycolor (Input IN, SurfaceOutput o, inout fixed4 color)
      {
          color *= _ColorTint;
      }

		void surf (Input IN, inout SurfaceOutput o) {
			 //切割效果
			 //clip (frac((IN.worldPos.y+IN.worldPos.z*0.1) * 5) - 0.5);
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			float2 uv=IN.uv_MainTex;//计算流量光
			uv.x/=2;//取一半
			//流光亮自动加上
			uv.x+=_Time.y*_uvaddspeed;
			//取流光亮度
			float flow=tex2D(_FlowTex,uv).a;
			o.Albedo = c.rgb+float3(flow,flow,flow);
			//o.Albedo = c.rgb;
			
			//===顶点输出===
			// o.Albedo *= IN.customColor;
			
			//法线贴图效果
			//o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
			
			//=====添加方式的细节纹理=====
			 float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
          	 screenUV *= float2(8,6);
         	 o.Albedo *= tex2D (_Detail, screenUV).rgb * 2;
			
			//=====边缘光照跟透明效果有冲突======
			 // half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
           	// o.Emission = _RimColor.rgb * pow (rim, _RimPower);
         	    //立方图反射
         	   // o.Emission = texCUBE (_Cube, IN.worldRefl).rgb;
               // o.Emission = texCUBE (_Cube, WorldReflectionVector (IN, o.Normal)).rgb;//带有光泽的立面反射
               
			o.Alpha = _ColorTint.a;
		}

      
		ENDCG
	} 
	FallBack "Diffuse"
}
