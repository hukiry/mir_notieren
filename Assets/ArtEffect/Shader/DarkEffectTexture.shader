Shader "Hidden/DarkEffectTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_SmoothLength("SmoothLength",Range(1,20))=20
		_DarkColor("DarkColor",Color)=(1,1,1,1)
		 [Toggle] _IsUseTextrue ("开启纹理", Float) = 0
		_DarkColorTex ("DarkColorTex", 2D) = "white" {}

		_x("x",float)=1
		_y("y",float)=1
		_Radius("Radius",Range(1,3000))=20
    }
	 SubShader
    {

		LOD 200
		Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true"  "PreviewType"="Plane" }

		Cull Off
		ZWrite Off
		ZTest Always

        GrabPass
        {
            "_GrabPassTexture"
        }

		Pass
		{
		  CGPROGRAM
		  #pragma vertex vert
		  #pragma fragment frag
 
		  #include "UnityCG.cginc"
 
		  //追踪物体最多个数
		  #define ItemSize 1
 
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
			
		
		  //渐变颜色
		  float _SmoothLength;
		  //设置的数量
		  fixed _ItemCnt;
		  //数组
		  float4 _Item;

		 
		  float _IsUseTextrue;
		  sampler2D _DarkColorTex;
		  //颜色
		  fixed4 _DarkColor;
		  float _x,_y;
		  float _Radius;
		 
		  sampler2D _GrabPassTexture,_GrabPassTextureTwo;
 
		  v2f vert (appdata v)
		  {
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			return o;
		  }
 
		  fixed CalcAlpha(float4 vt, float4 pt)
		  {
			if(pt.z < 0)
			{
			  return 1;
			}
 
			float distPow2 = pow(vt.x - pt.x, 2) + pow(vt.y - pt.y, 2);
			float dist = (distPow2 > 0) ? sqrt(distPow2) : 0;
 
			float smoothLength = _SmoothLength;
			if(smoothLength < 0)
			{
			  smoothLength = 0;
			}
 
			float maxValue = pt.z;
			float minValue = pt.z - smoothLength;
			if(minValue < 0)
			{
			  minValue = 0;
			  smoothLength = pt.z;
			}
 
			if(dist <= minValue)
			{
			  return 0;
			}
			else if (dist > maxValue)
			{
			  return 1;
			}
 
			fixed retVal = (dist - minValue) / smoothLength;
 
			return retVal;
		  }
 
		  fixed4 frag (v2f i) : SV_Target
		  {
			fixed alphaVal = 1;
			fixed tmpVal = 1;
			_Item=float4(_x,_SmoothLength-_y,_Radius,0);
			fixed4 col=tex2D(_DarkColorTex,i.uv);

			tmpVal = CalcAlpha(i.vertex, _Item);
			if(tmpVal < alphaVal)
			{
				alphaVal = tmpVal;
			}

			
			if(_IsUseTextrue){
				alphaVal *= col.a;
				return tex2D(_GrabPassTexture,float2( i.uv.x,1-i.uv.y)) * ( 1 - alphaVal) + col * alphaVal;
			}
			else
			{
				alphaVal *= _DarkColor.a;
				return tex2D(_MainTex, i.uv) * ( 1 - alphaVal) + _DarkColor * alphaVal;
			}
		  }
 
		  ENDCG
		}
	}
}
