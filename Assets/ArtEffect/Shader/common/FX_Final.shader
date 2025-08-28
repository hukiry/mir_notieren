Shader "FX/Hukiry_Final" {
	Properties {
		[HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5	
		
		[Enum(UnityEngine.Rendering.BlendMode)] SrcBlend("SrcBlend-----------------------------------------------------------------------------------", Float) = 5//SrcAlpha
		[Enum(UnityEngine.Rendering.BlendMode)] DstBlend("DstBlend-----------------------------------------------------------------------------------", Float) = 10//One
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("双面显示-----------------------------------------------------------------------------------", Float) = 0		
		[Enum(off,0,On,1)] _ZWrite ("ZWrite-----------------------------------------------------------------------------------", float) = 0
		[Space(30)]
		[Toggle(IsCoustom)]IsCoustom("【自定义开关（实现曲线控制动画）)】发射器Renderer面板Custom传入：'UV2'、'Custom1.xyzw'、'Custom2.xyzw'、'Tangent'",int) = 0
		[Header(____________________________________BaseMap_______________________________________________________)]
		[Space(10)]
		[HDR] _BaseColor ("颜色-----------------------------------------------------------------------------------", Color) = (1,1,1,1)
		_BaseMap ("主贴图---------------------------------------------------------------------------------", 2D) = "white" {}
		_BaseTexRotation ("旋转-----------------------------------------------------------------------------------", Float ) = 0	
		_DeBlackBG ("去黑底-----------------------------------------------------------------------------------", range (0,1) ) = 0
		_BaseMapBrightness ("贴图亮度-----------------------------------------------------------------------------------", Range(0.001,3) ) = 1
		_BaseMapPower ("贴图亮度-强度-----------------------------------------------------------------------------------", Range(0.001,30) ) = 1		
		_BaseMapPannerX ("UV流动-X轴-----------------------------------------------------------------------------------",Float) = 0 
		_BaseMapPannerY ("UV流动-Y轴-----------------------------------------------------------------------------------",Float) = 0
		_BaseIsPolarBlend ("极坐标-----------------------------------------------------------------------------------", range (0,1) ) = 0
		_BasePolarRadius ("半径大小-----------------------------------------------------------------------------------",Range(-1,1) )= 0.3
		[Space(30)]
		[Header(____________________________________Turbulence_______________________________________________________)]
		[Space(10)]
		_TurbulenceTex ("扰乱贴图-----------------------------------------------------------------------------------", 2D) = "black" {}
		_TurbulenceStrength ("【开关】扰乱强度【对应Custom Data面板的Z值(用曲线控制动画)】-----------------------------------------------------------------------------------", Float ) = 0.2
		_TurbulenceMulDis ("扰乱效果-溶解贴图-----------------------------------------------------------------------------------", range (-1,1) ) = 0
		_TurbulenceMulAllMask ("扰乱效果-遮罩贴图-----------------------------------------------------------------------------------", range (-1,1) ) = 0
		_TurbulenceMulAllTex ("扰乱效果-全部贴图-----------------------------------------------------------------------------------", range (-1,1) ) = 0
		_TurbulenceTexRotation ("旋转-----------------------------------------------------------------------------------", Float ) = 0
		_TurbulenceTexPannerX ("UV流动-X轴-----------------------------------------------------------------------------------", Float ) = 0
		_TurbulenceTexPannerY ("UV流动-Y轴-----------------------------------------------------------------------------------", Float ) = 0
		_TurbulenceIsPolarBlend ("扰乱效果-极坐标-----------------------------------------------------------------------------------", range (0,1) ) = 0
		_TurbulencePolarRadius ("扰乱效果-极坐标-半径大小-----------------------------------------------------------------------------------",Range(-1,1) )= 0.3
		[Space(30)]
		[Header(____________________________________Mask1_______________________________________________________)]
		[Space(10)]
		_SMaskTex ("遮罩1贴图-----------------------------------------------------------------------------------", 2D) = "white" {}
		_SMaskIntensity ("遮罩1强度-----------------------------------------------------------------------------------", range (0,1) ) = 1
		_SMaskTexRotation ("旋转-----------------------------------------------------------------------------------", Float ) = 0	
		_SMaskTexPannerX ("UV流动-X轴-----------------------------------------------------------------------------------", Float ) = 0
		_SMaskTexPannerY ("UV流动-Y轴-----------------------------------------------------------------------------------", Float ) = 0
		_SMaskIsPolarBlend ("遮罩1-极坐标-----------------------------------------------------------------------------------", range (0,1) ) = 0		
		_SMaskPolarRadius ("遮罩1-极坐标-半径大小-----------------------------------------------------------------------------------",Range(-1,1) )= 0.3
		[Space(30)]
		[Header(____________________________________Mask2_______________________________________________________)]
		[Space(10)]
		_MaskTex ("遮罩2贴图-----------------------------------------------------------------------------------", 2D) = "white" {}
		_MaskIntensity ("遮罩2强度-----------------------------------------------------------------------------------", range (0,1) ) = 1
		_MaskTexRotation ("旋转-----------------------------------------------------------------------------------", Float ) = 0	
		_MaskTexPannerX ("UV流动-X轴-----------------------------------------------------------------------------------", Float ) = 0
		_MaskTexPannerY ("UV流动-Y轴-----------------------------------------------------------------------------------", Float ) = 0		
		_MaskIsPolarBlend ("遮罩2-极坐标-----------------------------------------------------------------------------------", range (0,1) ) = 0
		_MaskPolarRadius ("遮罩2-极坐标-半径大小-----------------------------------------------------------------------------------",Range(-1,1) )= 0.3
		[Space(30)]
		[Header(____________________________________Dissolve_______________________________________________________)]
		[Space(10)]
		_DissolveTex ("溶解贴图-----------------------------------------------------------------------------------", 2D) = "white" {}
		_AnDissolve ("溶解强度【对应Custom Data面板的W值,需先设值-0.5(用曲线控制动画)】-----------------------------------------------------------------------------------",range (-2.5,2)  ) = -2
		_DissolveMultX ("X轴溶解-----------------------------------------------------------------------------------", range (-1,1) ) = 0
		_DissolveMultY ("Y轴溶解-----------------------------------------------------------------------------------", range (-1,1) ) = 0		
		_DisSoftness ("软边-----------------------------------------------------------------------------------",range (0.1,2)) = 0.1
		_DisRimWidth ("溶解描边-----------------------------------------------------------------------------------" , Range(0,1) ) = 0
		[HDR] _DisRimColr ("描边颜色-----------------------------------------------------------------------------------",Color) = (1,1,1,1)
		_DissolveTexRotation ("旋转-----------------------------------------------------------------------------------", Float ) = 0	
		_DissolvePannerX ("UV流动-X轴-----------------------------------------------------------------------------------", Float ) = 0
		_DissolvePannerY ("UV流动-Y轴-----------------------------------------------------------------------------------", Float ) = 0
		_DissolvePolarBlend ("溶解-极坐标-----------------------------------------------------------------------------------", range (0,1) ) = 0		
		_DissolvePolarRadius ("溶解-极坐标-半径大小-----------------------------------------------------------------------------------",Range(-1,1) )= 0.3
		[Space(30)]
		[Header(____________________________________OcclusionMap_______________________________________________________)]
		[Space(10)]
		_Occlusion ("细节贴图（亮亮相乘 暗暗相乘）-----------------------------------------------------------------------------------", 2D) = "white" {}
		_OcclusionBlend ("【开关】贴图混合-----------------------------------------------------------------------------------", range (0,1) ) = 0
		_OcclusionThrough ("混合强度-----------------------------------------------------------------------------------", range (0,1) ) = 0	
		_OcclusionPolarBlend ("极坐标-----------------------------------------------------------------------------------", range (0,1) ) = 0
		_OcclusionPolarRadius ("极坐标-半径大小-----------------------------------------------------------------------------------",Range(-1,1) )= 0.3
		_OcclusionRotation ("旋转-----------------------------------------------------------------------------------", Float ) = 0	
		_OccDeBlackBG ("去黑底-----------------------------------------------------------------------------------", range (0,1) ) = 0
		_OcclusionBrightness ("亮度-----------------------------------------------------------------------------------", Range(0.001,30) ) = 1
		_OcclusionPower ("OcclusionPower-----------------------------------------------------------------------------------", Range(0.001,30) ) = 1		
		_OcclusionPannerX ("UV流动-X轴-----------------------------------------------------------------------------------",Float) = 0 
		_OcclusionPannerY ("UV流动-Y轴-----------------------------------------------------------------------------------",Float) = 0  	
		[Space(30)]				
		[Header(____________________________________Vertex Animation_______________________________________________________)]
		[Space(10)]
		_PointMoveTex ("顶点动画贴图-----------------------------------------------------------------------------------", 2D) = "white" {}
		[Toggle(UseUV2)]UseUV2("UV2-是否使用第二套UV-----------------------------------------------------------------------------------",int) = 0
		_PointMove ("顶点移动距离【对应Custom Data面板Custom2的W值】-----------------------------------------------------------------------------------",vector ) =(0,0,0,0)
		_PointMoveRotation ("旋转-----------------------------------------------------------------------------------", Float ) = 0	
		_PointMoveX ("UV流动-X轴-----------------------------------------------------------------------------------",Float) = 0 
		_PointMoveY ("UV流动-Y轴-----------------------------------------------------------------------------------",Float) = 0
		_PointMoveUnNormal ("是否跟随法线方向（0=法线方向/1=非法线方向）-----------------------------------------------------------------------------------", range(0,1) ) = 0		
		_PointMoveMulSMask ("PointMoveMulSMask-----------------------------------------------------------------------------------", range(0,1) ) = 0	 
		[Space(30)]  						
		[Header(____________________________________NormalMap_______________________________________________________)]
		[Space(10)]
		_NormalTex ("法线贴图-----------------------------------------------------------------------------------", 2D) = "white" {}
		_NormalScale ("法线强度-----------------------------------------------------------------------------------", Float ) = 1
		_NormalPannerX ("NormalPannerX-----------------------------------------------------------------------------------",Float) = 0 
		_NormalPannerY ("NormalPannerY-----------------------------------------------------------------------------------",Float) = 0  
		_NormalZ ("NormalZ-----------------------------------------------------------------------------------", range (0,10) ) = 1 //提高暗部亮度	
		[Space(30)]
		[Header(____________________________________Lighting_______________________________________________________)]	
		[Space(10)]
		_IsLightingBlend ("是否受灯光影响-----------------------------------------------------------------------------------", range (0,1) ) = 0	
		[HDR] _LightColor ("灯光颜色-----------------------------------------------------------------------------------",Color) = (1,1,1,1)
		_LightIntensity ("灯光强度-----------------------------------------------------------------------------------", range (0,10) ) = 0.1
		_LightPow ("灯光边缘-----------------------------------------------------------------------------------", range (0.01,10) ) = 1
		[Space(30)]
		[Header(____________________________________Fresnel_______________________________________________________)]	
		[Space(10)]
		_FresnelBlend ("Fresnel-----------------------------------------------------------------------------------", range (0,1) ) = 0
		[HDR] _FColor ("FColor-----------------------------------------------------------------------------------",Color) = (1,1,1,1)
		_FScale ("FScale-----------------------------------------------------------------------------------",Float ) = 1
		[Space(30)]
		[Header(____________________________________Fade_______________________________________________________)]	
		[Space(10)]
		_Fade ("FadeSize(RimSize)-----------------------------------------------------------------------------------", range(0,10) ) = 0
		_FadePower ("Fade(Rim)Power-----------------------------------------------------------------------------------", range(0.1,10) ) = 1
		_IsRimLightBlend ("【IsRimLightBlend】-----------------------------------------------------------------------------------", range (0,1) ) = 0
		[HDR] _RimColor("RimColor-----------------------------------------------------------------------------------",Color)=(1,1,1,1)
		_MeshFade ("边缘羽化-----------------------------------------------------------------------------------", range(0,10) ) = 0
		_MeshFadePower ("边缘羽化强度-----------------------------------------------------------------------------------", range(0.1,10) ) = 1		
		[Space(30)]
		[Header(____________________________________DoubleFace_______________________________________________________)]	
		[Space(10)]
		_DoubleFaceBlend ("模型双面颜色-----------------------------------------------------------------------------------", range (0,1) ) = 0
		[HDR] _FaceInColor ("内面颜色-----------------------------------------------------------------------------------",Color) = (1,1,1,1)
		[HDR] _FaceOurColor ("外面颜色-----------------------------------------------------------------------------------",Color) = (1,1,1,1)
		[Space(30)]
		[Header(____________________________________Refract_______________________________________________________)]	
		[Space(10)]
		_RefractBlend ("折射-----------------------------------------------------------------------------------", range (0,1) ) = 0
		_RefractStrength ("折射强度-----------------------------------------------------------------------------------", range (-5,5) ) = 0.2
		_RefractAphaStrength ("折射透明度-----------------------------------------------------------------------------------", range (0,5) ) = 5	
		[Space(30)]	
		[Header(____________________________________BlackWhite_______________________________________________________)]	
		[Space(10)]
		_BlackWhiteBlend ("黑白遮罩-----------------------------------------------------------------------------------", range (0,1) ) = 0
		_BlackWhitePower ("黑白遮罩强度-----------------------------------------------------------------------------------", range(0.1,30) ) = 1	
		_BlackWhiteAphaStrength ("黑白遮罩透明度-----------------------------------------------------------------------------------", range (0,5) ) = 5	
		[Space(30)]
		[Header(____________________________________Squence_______________________________________________________)]	
		[Space(10)]
		_SquenceMoveBlend ("【开关】序列帧-----------------------------------------------------------------------------------", range (0,1) ) = 0
		_SquenceRow ("序列帧-行数-----------------------------------------------------------------------------------", float ) = 1	
		_SquenceColumn ("序列帧-列数-----------------------------------------------------------------------------------",float ) = 1
		_SquenceSpeed ("序列帧-速度-----------------------------------------------------------------------------------",float ) = 1			
		//[Space(10)]
		//Stencile	
		//[Header(Stencil)]
		//      [Space(5)]
		//      [IntRange] _Stencil         ("Stencil Reference", Range (0, 255)) = 0
		//      [IntRange] _ReadMask        ("     Read Mask", Range (0, 255)) = 255
		//      [IntRange] _WriteMask       ("     Write Mask", Range (0, 255)) = 255
		//      [Enum(UnityEngine.Rendering.StencilOp)]
		//      _StencilFail                ("Stencil Fail Op", Int) = 0           // 0 = keep
		//      [Enum(UnityEngine.Rendering.StencilOp)] 
		//      _StencilZFail               ("Stencil ZFail Op", Int) = 0          // 0 = keep	
		[Space(30)]
		[Header(____________________________________Stencil_______________________________________________________)]	
		[Space(10)]
		[IntRange] _StencilID         ("地面缝隙效果-----------------------------------------------------------------------------------", Range (0, 4)) = 4    
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp  ("Stencil Comparison-----------------------------------------------------------------------------------", Int) = 8		
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Operation-----------------------------------------------------------------------------------", Int) = 0      // 0 = keep, 2 = replace
		[Enum(UnityEngine.Rendering.CompareFunction)]  _ZTest ("深度测试-----------------------------------------------------------------------------------",float) = 4	
		//[Enum(UnityEngine.Rendering.StencilOp)] 
		//_StencilZFail               ("Stencil ZFail Op", Int) = 0          // 0 = keep
		[Space(30)]
		[Header(____________________________________ColorMask_______________________________________________________)]	
		[Space(10)]
		_ColorMask("ColorMask-----------------------------------------------------------------------------------", Range (0, 14)) = 14
	}
	SubShader {
		Tags {
			"IgnoreProjector"="True"
			"Queue"="Transparent"
			"RenderType"="Transparent"
		}
		//Stencil {
			//            Ref   [_Stencil]
			//            ReadMask [_ReadMask]
			//            WriteMask [_WriteMask]
			//            Fail  [_StencilFail]
			//            ZFail [_StencilZFail]
		//        }
		Stencil {
			Ref   [_StencilID]
			Comp  [_StencilComp]//Gequal
			Pass  [_StencilOp]//replace
		}
		GrabPass{
			//"_GrabTexture"//如果想使用自定义名称则需要把折射shader分开单独来，因为那样只会抓取一次屏幕，相对来说比较节省性能
		}		
		Pass {			
			Name "FORWARD"
			Tags {
				"LightMode"="ForwardBase"
			}
			Blend[SrcBlend][DstBlend]
			Cull [_Cull]
			ZWrite [_ZWrite]
			ZTest [_ZTest]
			ColorMask [_ColorMask]
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#pragma shader_feature __ IsCoustom
			#pragma shader_feature __ UseUV2
			CBUFFER_START(UnityPerMaterial)	
			float _BaseIsPolarBlend;			
			float _BasePolarRadius;
			float _BaseTexRotation;
			float4 _BaseColor;
			float4 _BaseMap_ST;				
			float _BaseMapBrightness;
			float _BaseMapPower;
			float _BaseMapPannerX;
			float _BaseMapPannerY;
			float _DeBlackBG;

			float _OcclusionPolarBlend;
			float _OcclusionPolarRadius;
			float _OcclusionRotation;
			float4 _Occlusion_ST;
			float _OccDeBlackBG;
			float _OcclusionBlend;
			float _OcclusionThrough;
			float _OcclusionBrightness;
			float _OcclusionPower;
			float _OcclusionPannerX;
			float _OcclusionPannerY;

			float _TurbulenceIsPolarBlend;
			float _TurbulencePolarRadius;
			float _TurbulenceTexRotation;
			float4 _TurbulenceTex_ST;
			float _TurbulenceTexPannerX;
			float _TurbulenceTexPannerY;
			float _TurbulenceStrength;
			float _TurbulenceMulDis;
			float _TurbulenceMulAllMask;
			float _TurbulenceMulAllTex;

			float _SMaskIsPolarBlend;
			float _SMaskIntensity;
			float _SMaskPolarRadius;
			float _SMaskTexRotation;
			float4 _SMaskTex_ST;
			float _SMaskTexPannerX;
			float _SMaskTexPannerY;

			float _MaskIsPolarBlend;
			float _MaskIntensity;
			float _MaskPolarRadius;
			float _MaskTexRotation;
			float4 _MaskTex_ST;
			float _MaskTexPannerX;
			float _MaskTexPannerY;

			float _DissolvePolarBlend;
			float _DissolveMultX;
			float _DissolveMultY;
			float _DissolvePolarRadius;
			float _DissolveTexRotation;
			float4 _DissolveTex_ST;
			float _DissolvePannerX;
			float _DissolvePannerY;
			float _DisSoftness;
			float _AnDissolve;
			float _DisRimWidth;
			float4 _DisRimColr;

			float _PointMoveRotation;
			float4 _PointMoveTex_ST;
			float _PointMoveX;
			float _PointMoveY;
			float4 _PointMove;
			float _PointMoveUnNormal;
			float _PointMoveMulSMask;

			float _DoubleFaceBlend;

			float _FresnelBlend;
			float _FScale;
			float4 _FColor;
			float4 _FaceInColor;
			float4 _FaceOurColor;
			float _Fade;
			float _MeshFade;
			float _MeshFadePower;
			float4 _RimColor;
			float _FadePower;

			float4 _NormalTex_ST;
			float _NormalPannerX;
			float _NormalPannerY;
			float _NormalScale;
			float _NormalZ;
			float  _IsLightingBlend;
			float  _IsRimLightBlend;
			float4 _LightColor;
			float _LightIntensity;
			float _LightPow;

			float _RefractStrength;
			float _RefractAphaStrength;
			float _RefractBlend;

			float _BlackWhiteBlend;
			float _BlackWhitePower;
			float _BlackWhiteAphaStrength;

			float _SquenceMoveBlend;
			float _SquenceRow;
			float _SquenceColumn;
			float _SquenceSpeed;
			
			CBUFFER_END
			sampler2D _PointMoveTex;
			sampler2D _MaskTex;
			sampler2D _SMaskTex;
			sampler2D _NormalTex;
			sampler2D _TurbulenceTex;
			sampler2D _BaseMap;
			sampler2D _Occlusion;
			sampler2D _DissolveTex;
			sampler2D _CameraDepthTexture;
			sampler2D _GrabTexture;
			

			struct VertexInput {
				float4 vertex : POSITION;
				float4 texcoord0 : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 tangent : TANGENT;
				float4 vertexColor : COLOR;
				float3 normal : NORMAL;
			};
			struct VertexOutput {
				float4 pos : SV_POSITION;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 custom2 : TEXCOORD2;
				float4 tangent : TEXCOORD3;
				float4 vertexColor : COLOR;
				float3 normal : TEXCOORD8;
				float3 posWS : TEXCOORD7;
				float4 positionScreen : TEXCOORD5;
			};

			float remap (float S,float Begin,float End,float TBegin,float TEnd)//区间映射
			{
				return (S - Begin )/(End - Begin)*(TEnd - TBegin) + TBegin;
			};

			float2 polaruv (float2 uv, float radius)//Polar
			{
				float2 Puv;
				float PolarRadius ;
				PolarRadius = radius;
				Puv = uv;
				Puv -= float2 (0.5,0.5);
				Puv = float2 (atan2(Puv.y,Puv.x)/3.141593*0.5+0.5,length(Puv)+PolarRadius);
				return Puv;
			};
			float2 SequenceMove(float2 uv )//Sequence move
			{
				float2 SUV ;				
				float time = floor(_Time.y*_SquenceSpeed);
				float row = floor(time/_SquenceColumn);
				float column = time - row *_SquenceRow;
				SUV= uv + float2(column,-row);
				SUV.x/=_SquenceColumn;
				SUV.y/=_SquenceRow;
				return SUV;
			}

			float2 rotation(float2 uv, float angle) //Rotation uv
			{
				float2 Ruv;
				float Rangle;
				Rangle = radians(angle);//角度转弧度
				Ruv = uv;
				Ruv -= float2(0.5, 0.5);
				Ruv = float2(Ruv.x * cos(Rangle) - Ruv.y * sin(Rangle), Ruv.x * sin(Rangle) + Ruv.y * cos(Rangle));
				Ruv += float2(0.5, 0.5);
				return Ruv;
			};

			VertexOutput vert (VertexInput v) {
				VertexOutput o = (VertexOutput)0;	
				o.uv0 = v.texcoord0;		
				o.uv1 = v.texcoord1;
				o.custom2 =  v.texcoord2;

				//SMASK Pointmove
				float2 smaskuv = lerp(o.uv0.xy,polaruv (o.uv0.xy,_SMaskPolarRadius ),_SMaskIsPolarBlend);
				float2 _SMaksUVCruve = 0 ;
				#if defined(IsCoustom)
					_SMaksUVCruve = float2(o.custom2.r  , o.custom2.g )  ;
					# endif 				       				      											
					#if defined(UseUV2)
						smaskuv = lerp(o.uv0.zw,polaruv (o.uv0.zw,_SMaskPolarRadius ),_SMaskIsPolarBlend);			       				      
					#endif 
					float2 _SMakUVSelfMove = float2(_SMaskTexPannerX , _SMaskTexPannerY) * _Time.g ;
					float2 _SMakUV =  (smaskuv + float2(_SMaksUVCruve + _SMakUVSelfMove));
					_SMakUV = rotation(_SMakUV,_SMaskTexRotation);	
					float4 _SMask_var = tex2Dlod(_SMaskTex,float4( float2(_SMakUV) ,0,0));
					float _SMaskOut = lerp(0,_SMask_var.r * _SMask_var.a,_PointMoveMulSMask) ;		
					
					//Point move 	
					float CustomCurve = 1 ;	
					#if defined(IsCoustom)
						CustomCurve =  o.custom2.w  ;				
						# endif					
						float4 move = 	float4 ((rotation(o.uv0.xy ,_PointMoveRotation) + float2(_Time.y * _PointMoveX, _Time.y * _PointMoveY)),0,0);
						#if defined(UseUV2)
							move = 	float4 ((rotation(o.uv0.zw ,_PointMoveRotation) + float2(_Time.y * _PointMoveX, _Time.y * _PointMoveY)),0,0);
						#endif 				
						float4 turCol = tex2Dlod(_PointMoveTex, move)  ;
						float turColOut = turCol.r*turCol.a;				
						v.vertex.xyz += float3((turColOut+_SMaskOut) * _PointMove.x,(turColOut+_SMaskOut) * _PointMove.y,(turColOut+_SMaskOut) * _PointMove.z)*lerp(v.normal,1,_PointMoveUnNormal)* (_PointMove.w + CustomCurve) ;
						
						
						o.vertexColor = v.vertexColor;
						o.pos = UnityObjectToClipPos( v.vertex.xyz);
						o.posWS =  mul(unity_ObjectToWorld, v.vertex.xyz);
						o.normal = v.normal;
						o.tangent = v.tangent;
						//Fade								
						o.positionScreen = ComputeScreenPos(o.pos);//z分量为裁剪空间的z值，范围[-Near,Far]
						COMPUTE_EYEDEPTH(o.positionScreen.z);//COMPUTE_EYEDEPTH函数，将z分量范围[-Near,Far]转换为[Near,Far]
						
						return o;
					}


					float4 frag(VertexOutput i , float facing : VFACE) : COLOR {	
						//SquenceUV
						i.uv0.xy = lerp(i.uv0.xy, SequenceMove(i.uv0.xy),_SquenceMoveBlend);
						//Tub11erlence
						float2 tuv =  lerp(i.uv0.xy,polaruv (i.uv0.xy,_TurbulencePolarRadius ),_TurbulenceIsPolarBlend);	
						float2 _TUVCruve = 0 ;
						#if defined(IsCoustom)
							_TUVCruve = float2( 0 , i.custom2.b)   ;
							# endif

							float2 _TurbulenceMove = float2(_TurbulenceTexPannerX , _TurbulenceTexPannerY) * _Time.g ;
							float2 _TurbulenceUV = (tuv+_TurbulenceMove );
							_TurbulenceUV = rotation(_TurbulenceUV,_TurbulenceTexRotation);			
							#if defined(IsCoustom)
								_TurbulenceUV = (tuv + float2(_TUVCruve + _TurbulenceMove));
								_TurbulenceUV = rotation(_TurbulenceUV,_TurbulenceTexRotation);	
								# endif 

								float4 _TurbulenceTex_var = tex2D(_TurbulenceTex,  TRANSFORM_TEX(_TurbulenceUV, _TurbulenceTex));
								float _AfTuv = _TurbulenceTex_var.r *_TurbulenceTex_var.a *_TurbulenceStrength;
								float _AfTuv1 = _TurbulenceTex_var.r* _TurbulenceTex_var.a  * (i.uv1.b+_TurbulenceStrength);
								float _AllTurbulence = lerp(0,_AfTuv1,_TurbulenceMulAllTex);     
								//SMASK
								float2 smaskuv = lerp(i.uv0.xy,polaruv (i.uv0.xy,_SMaskPolarRadius ),_SMaskIsPolarBlend);
								float2 _SMaksUVCruve = 0 ;
								#if defined(IsCoustom)
									_SMaksUVCruve = float2(i.custom2.r  , i.custom2.g )   ;
									# endif 
									
									float2 _SMakUVSelfMove = float2(_SMaskTexPannerX , _SMaskTexPannerY) * _Time.g ;
									float2 _SMakUV =   lerp(0,_AfTuv1,_TurbulenceMulAllMask) + _AllTurbulence + (smaskuv + float2(_SMaksUVCruve + _SMakUVSelfMove));
									_SMakUV = rotation(_SMakUV,_SMaskTexRotation);				
									
									float4 _SMask_var = tex2D(_SMaskTex,  TRANSFORM_TEX(_SMakUV, _SMaskTex));		
									float _SMaskOut = _SMask_var.r * _SMask_var.a ;	
									_SMaskOut=lerp(1,_SMaskOut, _SMaskIntensity);
									
									
									//MASK
									float2 maskuv = lerp(i.uv0.xy,polaruv (i.uv0.xy,_MaskPolarRadius ),_MaskIsPolarBlend);
									float2 _MaksUV = lerp(0,_AfTuv1,_TurbulenceMulAllMask) +_AllTurbulence+ (maskuv+(float2(_MaskTexPannerX,_MaskTexPannerY)*_Time.g) );
									_MaksUV = rotation(_MaksUV,_MaskTexRotation);
									float4 _MaskTex_var = tex2D(_MaskTex, TRANSFORM_TEX(_MaksUV, _MaskTex));
									float _MaskOut = _MaskTex_var.r * _MaskTex_var.a ;
									_MaskOut=lerp(1,_MaskOut, _MaskIntensity);
									
									//DoubleFace
									float DoubleFace = (facing >= 0 ?  1 : 0);
									float3 FaceColor =lerp( (1,1,1),( (DoubleFace * _FaceOurColor + (1 - DoubleFace) *_FaceInColor ).rgb),_DoubleFaceBlend) ;

									//Fresnel
									float3  N = normalize( UnityObjectToWorldNormal(i.normal));
									float3  V = normalize(_WorldSpaceCameraPos.xyz - i.posWS);
									float4  F = pow(( 1 - saturate( dot( N , V))),_FScale) * _FColor ;		
									
									//MeshFade
									float MeshF =saturate(lerp(1, pow(saturate(dot( N,V )),_MeshFadePower),_MeshFade));
									
									//Lighting				
									float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);						
									float3 Lightdiff = saturate(dot(lightDirection,N));	
									
									//Normal
									float3 T = normalize(mul(unity_ObjectToWorld,i.tangent.xyz));
									float3 BT = normalize(cross(N,T)*i.tangent.w);
									float3x3 RotaTangent = float3x3 (T,BT,N) ;	//转切线空间矩阵				
									float3 Tlight = mul(RotaTangent,lightDirection);
									float2 _bumpUV =_AllTurbulence + (i.uv0.xy+(float2(_NormalPannerX,_NormalPannerY)*_Time.g) );			
									float3 bump = UnpackNormal(tex2D(_NormalTex,TRANSFORM_TEX(_bumpUV, _NormalTex)));
									bump.xy *=  _NormalScale;	
									bump.z = sqrt(1-saturate(dot(bump.xy,bump.xy)));
									float3 lightN = saturate(dot(Tlight,bump));
									
									//Base
									float2 baseuv =  lerp(i.uv0.xy,polaruv (i.uv0.xy,_BasePolarRadius ),_BaseIsPolarBlend)	;			
									float2 _BaseUVCruve = 0 ;
									#if defined(IsCoustom)
										_BaseUVCruve = float2(i.uv1.r  , i.uv1.g )   ;
										# endif 
										
										float2 _BaseUVSelfMove = float2(_BaseMapPannerX , _BaseMapPannerY) * _Time.g ;
										float2 _BaseUV = (_TurbulenceTex_var.r *_TurbulenceTex_var.a *_TurbulenceStrength + (baseuv + float2(_BaseUVCruve + _BaseUVSelfMove)));
										_BaseUV = rotation(_BaseUV,_BaseTexRotation);				
										#if defined(IsCoustom)
											_BaseUV = (_TurbulenceTex_var.r* _TurbulenceTex_var.a  * (i.uv1.b+_TurbulenceStrength)  + (baseuv + float2(_BaseUVCruve + _BaseUVSelfMove)));
											_BaseUV = rotation(_BaseUV,_BaseTexRotation);
											# endif
											
											float4 _BaseMap_var = tex2D(_BaseMap,  TRANSFORM_TEX(_BaseUV, _BaseMap));
											
											//Occlusion  
											float2 occuv = lerp(i.uv0.xy,polaruv (i.uv0.xy,_OcclusionPolarRadius ),_OcclusionPolarBlend);			      
											float2 _OcclusionUVSelfMove = float2(_OcclusionPannerX , _OcclusionPannerY) * _Time.g ;
											float2 _OcclusionUV = (_AllTurbulence + (occuv + _OcclusionUVSelfMove));
											_OcclusionUV = rotation(_OcclusionUV,_OcclusionRotation);
											float4 _Occlusion_var = tex2D(_Occlusion, TRANSFORM_TEX(_OcclusionUV, _Occlusion));
											
											//Disslove
											float2 dissolveUV1 = lerp(i.uv0.xy,polaruv (i.uv0.xy,_DissolvePolarRadius ),_DissolvePolarBlend);
											float2 dissolveUV2 = lerp(i.uv0.zw,polaruv (i.uv0.zw,_DissolvePolarRadius ),_DissolvePolarBlend);
											float2 _DissolveUV = lerp(0,_AfTuv1,_TurbulenceMulDis)+_AllTurbulence + (dissolveUV1+float2(_DissolvePannerX * _Time.g ,_DissolvePannerY * _Time.g));
											#if defined(UseUV2)
												_DissolveUV = lerp(0,_AfTuv1,_TurbulenceMulDis)+_AllTurbulence + (dissolveUV2 +float2(_DissolvePannerX * _Time.g ,_DissolvePannerY * _Time.g));
											#endif 					
											_DissolveUV = rotation(_DissolveUV,_DissolveTexRotation);				
											float4 _DissolveTex_var = tex2D(_DissolveTex,  TRANSFORM_TEX(_DissolveUV, _DissolveTex));		
											float uvdisX = lerp(0,i.uv0.x*ceil(_DissolveMultX),abs(_DissolveMultX));//UVX定向溶解	
											float uvdisY = lerp(0,i.uv0.y*ceil(_DissolveMultY),abs(_DissolveMultY));//UVY定向溶解				
											float _DissolveOut = pow (( saturate (_DissolveTex_var.r*_DissolveTex_var.a - uvdisX - uvdisY -  _AnDissolve ) ), _DisSoftness ) ;	
											#if defined(IsCoustom)
												_DissolveOut = pow (( saturate (_DissolveTex_var.r*_DissolveTex_var.a  - uvdisX - uvdisY - saturate(i.uv1.a)*2 -  _AnDissolve ) ), _DisSoftness ) ;	
												# endif 
												
												//dissloverim					
												clip(_DissolveOut - _DisRimWidth);
												float lightrim  = ( _DissolveOut  - _DisRimWidth < _DisRimWidth ? 1 : 0 );
												float4 lightrimcolor = float4(1,1,1,lightrim)*_DisRimColr;

												//Out
												float _AphaOut = 1;
												float3 _ColorOut = (1,1,1);	
												float3 _ColorOutA = _BaseColor.rgb * (pow((_BaseMapBrightness * _BaseMap_var.rgb), _BaseMapPower) * i.vertexColor.rgb)* FaceColor.rgb + F.rgb ;
												float3 _ColorOutAO = _BaseColor.rgb * (pow((_OcclusionBrightness * _Occlusion_var.rgb), _OcclusionPower) * i.vertexColor.rgb)* FaceColor.rgb + F.rgb ;
												_ColorOutA = lerp(lerp(_ColorOutA,_ColorOutA*_Occlusion_var.rgb,_OcclusionThrough),_ColorOutAO,_OcclusionBlend);

												float3 _ColorOutB =_BaseColor.rgb * (pow((_BaseMapBrightness * _BaseMap_var.rgb), _BaseMapPower) * i.vertexColor.rgb) * FaceColor.rgb;
												float3 _ColorOutBO = _BaseColor.rgb * (pow((_OcclusionBrightness * _Occlusion_var.rgb),_OcclusionPower) * i.vertexColor.rgb) * FaceColor.rgb;
												_ColorOutB = lerp(lerp(_ColorOutB,_ColorOutB*_Occlusion_var.rgb,_OcclusionThrough),_ColorOutBO,_OcclusionBlend);

												_ColorOut = lerp(_ColorOutB,_ColorOutA,_FresnelBlend);																	                 				
												_ColorOut *= lerp((saturate( dot(_ColorOut,bump))+_NormalZ),(saturate( dot(_ColorOut,lightN))+_NormalZ+pow( Lightdiff ,_LightPow)*_LightIntensity*_LightColor.rgb),_IsLightingBlend);			
												_AphaOut = lerp( _BaseColor.a  * _BaseMap_var.a * _SMaskOut * _MaskOut  * _DissolveOut * i.vertexColor.a * MeshF,_BaseColor.a * _BaseMap_var.r * _BaseMap_var.a * _SMaskOut * _MaskOut  * _DissolveOut * i.vertexColor.a * MeshF,_DeBlackBG);
												float _AphaOutO = lerp( _BaseColor.a  * _Occlusion_var.a * _SMaskOut * _MaskOut  * _DissolveOut * i.vertexColor.a * MeshF,_BaseColor.a * _Occlusion_var.r * _Occlusion_var.a * _SMaskOut * _MaskOut  * _DissolveOut * i.vertexColor.a * MeshF,_OccDeBlackBG);
												_AphaOut = lerp (_AphaOut, _AphaOutO, _OcclusionBlend)	;	
												_ColorOut = lerp (_ColorOut,lightrimcolor.rgb,step(_DissolveOut,lightrim) );//增加亮边颜色
												float4 col = float4(_ColorOut,_AphaOut);

												//Fade																											
												float screenZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture,i.positionScreen));//负责把深度纹理的采样转到视角空间下的深度值。				
												float diff = saturate(abs(screenZ - i.positionScreen.z));
												float4 colend = 1 - saturate( lerp (_Fade ,0,diff));				
												colend =saturate(pow(colend,_FadePower));
												col.a = col.a*colend.a;
												float4 rim =lerp( 0,_AphaOut *(pow((1-colend),_FadePower))*_RimColor,_IsRimLightBlend);

												//Refract
												float2 ScreenUV = (i.pos.xy /  _ScreenParams.xy) + col.a *_RefractStrength ;
												#if !UNITY_UV_STARTS_AT_TOP
													ScreenUV.y =  1 - ScreenUV.y;
												#endif
												float4 refracttex = tex2D(_GrabTexture,ScreenUV);
												refracttex.a = saturate( col.a * _RefractAphaStrength);
												float4 finalout = lerp((col+rim),refracttex,_RefractBlend);//混合折射

												//BlackWhite
												float2 BlackWhiteUV = lerp((i.pos.xy /  _ScreenParams.xy),ScreenUV,_RefractBlend);
												float4 blackwhite = tex2D(_GrabTexture,BlackWhiteUV);
												float grey = dot(blackwhite.rgb, fixed3(0.22, 0.707, 0.071));
												blackwhite.rgb = saturate( 1 - pow( float3(grey, grey, grey),_BlackWhitePower));  
												blackwhite.a = saturate( col.a * _BlackWhiteAphaStrength);								
												finalout = lerp(finalout,blackwhite,_BlackWhiteBlend);

												return finalout;

											}
											ENDHLSL
										}
									}

								}

