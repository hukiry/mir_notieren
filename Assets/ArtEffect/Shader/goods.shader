Shader "Custom/Goods"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_RadiusDown("RadiusDown",Range(0.0,1)) = 0
		_RadiusUp("RadiusUp",Range(0.0,1)) = 0
		_RadiusLeft("RadiusLeft",Range(0.0,1)) = 0
		_RadiusRight("RadiusRight",Range(0.0,1)) = 0

		[Space(10)]
		_XAxis("_XAxis",Range(0.0,0.5)) = 0
		_YAxis("_YAxis",Range(0.0,0.5)) = 0
		//_IsPadding 1=由外向内隐藏，2=由内向外隐藏，3=由内向外正边隐藏
		_IsPadding("IsPadding",int)=1


    }

    SubShader
    {
        // No culling or depth

		LOD 200

		Tags { 
			"QUEUE"="Transparent" 
			"IgnoreProjector"="true"  
			"PreviewType"="Plane" 
			//"RenderType"="Transparent" 
			"CanUseSpriteAtlas"="True"
		}

        Pass
        {
			ZWrite Off
			Cull Back
			Blend SrcAlpha OneMinusSrcAlpha
			

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

			 float _RadiusDown;
			 float _RadiusUp;
			 float _RadiusLeft;
			 float _RadiusRight;
			 float _IsPadding;

			 float _XAxis;
			 float _YAxis;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

           

            fixed4 frag (v2f i) : SV_Target
            {
				
                 fixed4 col = tex2D(_MainTex, i.uv) ;
                 half y = i.uv.y;
				 half x = i.uv.x;
 
                fixed cy = 0;
				if(_IsPadding==1){
					if (y < _RadiusDown)//由下至上
					{ 
						cy = _RadiusDown;
					}
					else if (y > 1-_RadiusUp)//由上至下  
					{  
						cy = 1-_RadiusUp;
					}
					else if (x > 1-_RadiusRight)//由右至左
					{  
						cy = 1-_RadiusRight;
					}
					else if (x <_RadiusLeft )//由左至右 
					{  
						cy =_RadiusLeft ;
					}
				}else if(_IsPadding==2){
					 
					if (y < 0.5 && y > 0.5 - _RadiusDown)//由内至下
					{ 
						cy = 0.5 - _RadiusDown;
					}
					else if ( y > 0.5 && y < 0.5 + _RadiusUp)//由内至上 
					{  
						cy = 0.5 + _RadiusUp;
					}
					else if (x > 0.5 && x < 0.5 + _RadiusRight)//由内至右  
					{  
						cy = 0.5 + _RadiusRight;
					}
					else if (x < 0.5 && x > 0.5 - _RadiusLeft )//由内至左 
					{  
						cy = 0.5 - _RadiusLeft;
					}
				}
				else if(_IsPadding==3)
				{
					bool isFullX=x>0.5-_XAxis&&x <0.5+ _XAxis;//由内向外/**/
					bool isFullY=y>0.5-_YAxis&&y <0.5+ _YAxis;
					if(isFullX&&isFullY)
					{
						cy =_XAxis*2+_YAxis*2;
					}
				}
				else{
					bool isFullX=x<_XAxis||x >1- _XAxis;
					bool isFullY=y<_YAxis||y >1- _YAxis;//由外向内
					if(isFullX||isFullY)
					{
						cy =_XAxis*2+_YAxis*2;
					}
				}

                if ( cy != 0)
                {
                    col.a = 0;
                }
                return col;
            }
            ENDCG
        }
    }

	CustomEditor "ShaderGoodsEditor"
}
