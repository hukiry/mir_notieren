using Hukiry;
using Hukiry.UI;
using LuaInterface;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using static UnityEngine.EventSystems.PointerEventData;
using BindType = ToLuaMenu.BindType;
using Hukiry.Socket;

public static class CustomSettings
{
    public static string saveDir = Application.dataPath + "/Lua/Source/Generate/";  //生成的文件目录 
    public static string toluaBaseType = Application.dataPath + "/Lua/ToLua/BaseType/";
    public static string baseLuaDir = Application.dataPath + "/Lua/ToLua/Lua/";
    public static string injectionFilesPath = Application.dataPath + "/Lua/ToLua/Injection/";

    //导出时强制做为静态类的类型(注意customTypeList 还要添加这个类型才能导出)
    //unity 有些类作为sealed class, 其实完全等价于静态类
    public static List<Type> staticClassTypes = new List<Type>
    {        
        typeof(UnityEngine.Application),
        typeof(UnityEngine.Time),
        typeof(UnityEngine.Screen),
        typeof(UnityEngine.SleepTimeout),
        typeof(UnityEngine.Input),
		typeof(UnityEngine.Resources),
        typeof(UnityEngine.Physics),
        typeof(UnityEngine.RenderSettings),
        typeof(UnityEngine.QualitySettings),
        typeof(UnityEngine.GL),
        typeof(UnityEngine.Graphics),
    };

    //附加导出委托类型(在导出委托时, customTypeList 中牵扯的委托类型都会导出， 无需写在这里)
    public static DelegateType[] customDelegateList = 
    {        
        _DT(typeof(Action)),
		_DT(typeof(Action<ByteBlock>)),
		_DT(typeof(Func<ByteBlock,byte[]>)),
		
		_DT(typeof(Action<GameObject>)),
		_DT(typeof(UnityEngine.Events.UnityAction)),
		_DT(typeof(System.Predicate<int>)),
        _DT(typeof(System.Action<int>)),
		_DT(typeof(System.Action<int, string>)),
		_DT(typeof(System.Comparison<int>)),
        _DT(typeof(System.Func<int, int>)),
		_DT(typeof(Action<bool, string>))
	};

	//在这里添加你要导出注册到lua的类型列表
	public static BindType[] customTypeList =
	{
		_GT(typeof(LuaInjectionStation)),
		_GT(typeof(InjectType)),     

		//DOTWEEN
		_GT(typeof(DG.Tweening.DOTween)),
		_GT(typeof(DG.Tweening.DOTweenAnimation)),
		_GT(typeof(DG.Tweening.DOTweenAnimation.AnimationType)),

		_GT(typeof(DG.Tweening.Tween)).SetBaseType(typeof(System.Object)).AddExtendType(typeof(DG.Tweening.TweenExtensions)),
		_GT(typeof(DG.Tweening.Sequence)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
		_GT(typeof(DG.Tweening.Tweener)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
		_GT(typeof(DG.Tweening.LoopType)),
		_GT(typeof(DG.Tweening.PathMode)),
		_GT(typeof(DG.Tweening.PathType)),
		_GT(typeof(DG.Tweening.RotateMode)),
		_GT(typeof(DG.Tweening.Ease)),
		_GT(typeof(DG.Tweening.AxisConstraint)),
		_GT(typeof(Component)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
		_GT(typeof(GameObject)).AddExtendType(typeof(GameObjectExtend)),
		_GT(typeof(Transform)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)).AddExtendType(typeof(TransformExtend)),
		_GT(typeof(Light)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
		_GT(typeof(Material)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
		_GT(typeof(Rigidbody)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
		_GT(typeof(Camera)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
		_GT(typeof(AudioSource)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
		_GT(typeof(SpriteRenderer)).AddExtendType(typeof(DG.Tweening.DOTweenModuleSprite)),
		_GT(typeof(RectTransform)).AddExtendType(typeof(DG.Tweening.DOTweenModuleUI)),
		_GT(typeof(RawImage)).AddExtendType(typeof(DG.Tweening.DOTweenModuleUI)),
		_GT(typeof(CanvasGroup)).AddExtendType(typeof(DG.Tweening.DOTweenModuleUI)),
		_GT(typeof(Text)).AddExtendType(typeof(DG.Tweening.DOTweenModuleUI)),
		_GT(typeof(TextMesh)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
		
		//C#
		_GT(typeof(System.Type)),
		_GT(typeof(SocketError)),
		_GT(typeof(NetworkReachability)),

		
		//UNITY
		//------------------------UNITY static
		_GT(typeof(Application)),
		_GT(typeof(Time)),
		_GT(typeof(Screen)),
		_GT(typeof(SleepTimeout)),
		_GT(typeof(Input)),
		_GT(typeof(Resources)),
		_GT(typeof(Physics)),
		_GT(typeof(RenderSettings)),
		_GT(typeof(QualitySettings)),
		_GT(typeof(GL)),
		_GT(typeof(Graphics)),
		_GT(typeof(LuaProfiler)),

		_GT(typeof(UnityEngine.Random)),
		_GT(typeof(Behaviour)),
		_GT(typeof(MonoBehaviour)),
		_GT(typeof(TrackedReference)),
		_GT(typeof(Texture)),
		_GT(typeof(Texture2D)),
		_GT(typeof(Sprite)),
		_GT(typeof(Shader)),
		_GT(typeof(Renderer)),
		_GT(typeof(CameraClearFlags)),
		_GT(typeof(AudioClip)),
		_GT(typeof(AssetBundle)),
		_GT(typeof(AsyncOperation)).SetBaseType(typeof(System.Object)),
		_GT(typeof(LightType)),
		_GT(typeof(Animator)),
		_GT(typeof(KeyCode)),
		_GT(typeof(SkinnedMeshRenderer)),
		_GT(typeof(Space)),
		_GT(typeof(ColorUtility)),

		//物理网格部分
		_GT(typeof(Ray)),
		_GT(typeof(RaycastHit)),
		_GT(typeof(MeshFilter)),
		_GT(typeof(Quaternion)),
		_GT(typeof(Ray2D)),
		_GT(typeof(Physics2D)),
		_GT(typeof(Collider2D)),
		_GT(typeof(BoxCollider2D)),
		_GT(typeof(ColliderDistance2D)),
		_GT(typeof(Rigidbody2D)),
		_GT(typeof(RaycastHit2D)),
		_GT(typeof(EdgeCollider2D)),
		_GT(typeof(CircleCollider2D)),
		_GT(typeof(CapsuleCollider2D)),
		_GT(typeof(SpringJoint2D )),
		_GT(typeof(PolygonCollider2D)) ,

		_GT(typeof(AnimationClip)).SetBaseType(typeof(UnityEngine.Object)),
		_GT(typeof(AnimationBlendMode)),
		_GT(typeof(QueueMode)),
		_GT(typeof(PlayMode)),
		_GT(typeof(WrapMode)),
		_GT(typeof(AnimationCurve)),
		_GT(typeof(VideoPlayer)),
		_GT(typeof(SystemInfo)),
		_GT(typeof(Vector2Int)) ,
		
		//UnityWeb
		_GT(typeof(UnityWebRequest)),
		_GT(typeof(WWWForm)),
		_GT(typeof(UnityWebRequestAsyncOperation)),
		_GT(typeof(UploadHandlerRaw)),
		_GT(typeof(DownloadHandlerBuffer)),
		//精灵部分
		_GT(typeof(SpriteMask)),
		_GT(typeof(SpriteTileMode )),
		_GT(typeof(SpriteSortPoint)),
		_GT(typeof(AnimationEvent)),
		_GT(typeof(UnityEngine.U2D.SpriteAtlas)),
		_GT(typeof(UnityEngine.Sprites.DataUtility)),
		_GT(typeof(UnityEngine.Rendering.SortingGroup)),

		//Unity-UGUI
		_GT(typeof(UnityEngine.Events.UnityEvent)),
		_GT(typeof(Rect)),
		_GT(typeof(RectTransform.Edge)),
		_GT(typeof(RectTransformUtility)),
		_GT(typeof(Mask)),
		_GT(typeof(RectMask2D)),
		_GT(typeof(EventSystem)),
		_GT(typeof(StandaloneInputModule)),
		_GT(typeof(EventTrigger)),
		_GT(typeof(EventTriggerType)),
		_GT(typeof(Canvas)),
		_GT(typeof(CanvasRenderer)),
		_GT(typeof(CanvasScaler)),
		_GT(typeof(CanvasScaler.ScaleMode)),
		_GT(typeof(CanvasScaler.ScreenMatchMode)),
		_GT(typeof(RenderMode)),
		//_GT(typeof(GraphicRaycaster)),
		_GT(typeof(Gradient)),
		_GT(typeof(MaskableGraphic)),
		_GT(typeof(Graphic)),
		_GT(typeof(Image)),
		_GT(typeof(Image.Type)),
		_GT(typeof(Selectable)),
		_GT(typeof(Button)),
		_GT(typeof(TextAlignment)),
		_GT(typeof(TextAnchor)),
		_GT(typeof(Toggle)),
		_GT(typeof(Toggle.ToggleEvent)),
		_GT(typeof(ToggleGroup)),
		_GT(typeof(Slider)),
		_GT(typeof(Slider.SliderEvent)),
		_GT(typeof(Scrollbar)),
		_GT(typeof(ScrollRect)).AddExtendType(typeof(DG.Tweening.DOTweenModuleUI)),
		_GT(typeof(ScrollRect.ScrollRectEvent)),
		_GT(typeof(UIDropdown)),
		_GT(typeof(Dropdown.OptionData)),
		_GT(typeof(Dropdown.DropdownEvent)),
		_GT(typeof(InputField)),
		_GT(typeof(InputField.OnChangeEvent)),
		_GT(typeof(InputField.ContentType)),
		_GT(typeof(InputField.LineType)),
		_GT(typeof(InputField.InputType)),
		_GT(typeof(InputField.CharacterValidation)),
		_GT(typeof(TouchScreenKeyboardType)),
		_GT(typeof(LayoutElement)),
		_GT(typeof(HorizontalLayoutGroup)),
		_GT(typeof(VerticalLayoutGroup)),
		_GT(typeof(GridLayoutGroup)),
		_GT(typeof(GridLayoutGroup.Axis)),
		_GT(typeof(RectOffset)),
		_GT(typeof(AspectRatioFitter)),
		_GT(typeof(AspectRatioFitter.AspectMode)),
		_GT(typeof(ContentSizeFitter)),
		_GT(typeof(ContentSizeFitter.FitMode)),
		_GT(typeof(EventTrigger.Entry)),
		_GT(typeof(InputButton)),
		_GT(typeof(PointerEventData)),
		_GT(typeof(FontData)),
		_GT(typeof(Shadow)),
		_GT(typeof(Outline)),
		

		////=========================================== 客户自定义 ==============================================
		
		//UI事件+控件
		_GT(typeof(HukirySupperText)),
		_GT(typeof(UIBoxCollider)),
		_GT(typeof(UIProgressbarMask)).AddExtendType(typeof(DoTweenUIExtend)),//自定义扩展类
		_GT(typeof(UIEventListener)),
		_GT(typeof(UIButtonListener)),
		_GT(typeof(AtlasImage)).AddExtendType(typeof(DG.Tweening.DOTweenModuleUI)),
		//粒子插件
		//_GT(typeof(UIParticleScale)),
		//碎片爆炸插件_GT(typeof(ExplosionForce)),
		//游戏工具 
		_GT(typeof(HukiryUtil)),
		_GT(typeof(PlayerPrefs)),//永久缓存数据

		//资源管理    
		_GT(typeof(RootCanvas)), //UI加载
		_GT(typeof(MainGame)),//游戏
		_GT(typeof(UnityEngine.SceneManagement.SceneManager)),//场景
		//_GT(typeof(UnityEngine.AssetBundleCreateRequest)),
		_GT(typeof(SceneLoader)),
        _GT(typeof(AssetsLoaderMgr)),
		

		//SDK部分
		_GT(typeof(Hukiry.SDK.SdkManager)),
		_GT(typeof(Hukiry.SDK.AppleLoginVerify)),
		_GT(typeof(Hukiry.SDK.UnityParam)),
		_GT(typeof(Hukiry.SDK.MobileNotification)),//本地推送

		//网络部分
		_GT(typeof(NetManager)),  //客户端链接
		_GT(typeof(BinaryDataMgr)),//二进制数据缓存
		_GT(typeof(ByteBlock)).AddExtendType(typeof(ByteBlockExtensions)),
		_GT(typeof(PutAliyunSdk)),

		//地图编辑-元宇宙
		_GT(typeof(DrawRendererGrid)),
		_GT(typeof(MeshGraphic)),
		_GT(typeof(SpriteMeshInfo)),

	};

    public static List<Type> dynamicList = new List<Type>()
    {
        typeof(MeshRenderer),
#if !UNITY_5_4_OR_NEWER
        typeof(ParticleEmitter),
        typeof(ParticleRenderer),
        typeof(ParticleAnimator),
#endif

        typeof(BoxCollider),
        typeof(MeshCollider),
        typeof(SphereCollider),
        typeof(CharacterController),
        typeof(CapsuleCollider),

        typeof(Animation),
        typeof(AnimationClip),
        typeof(AnimationState),

#if UNITY_2019_1_OR_NEWER
        typeof(SkinWeights),
#else
		typeof(BlendWeights),
#endif
        typeof(RenderTexture),
        typeof(Rigidbody),
    };

	//黑名单类移除
	public static List<Type> BlackList = new List<Type>()
	{
		typeof(System.IO.Stream),
	};

	//重载函数，相同参数个数，相同位置out参数匹配出问题时, 需要强制匹配解决
	//使用方法参见例子14
	public static List<Type> outList = new List<Type>(){};
        
    //ngui优化，下面的类没有派生类，可以作为sealed class
    public static List<Type> sealedList = new List<Type>(){};

    public static BindType _GT(Type t)
    {
        return new BindType(t);
    }

    public static DelegateType _DT(Type t)
    {
        return new DelegateType(t);
    }    


    [MenuItem("Lua/Attach Profiler 附加 到Unity性能中", false, 151)]
    static void AttachProfiler()
    {
        if (!Application.isPlaying)
        {
            EditorUtility.DisplayDialog("警告", "请在运行时执行此功能", "确定");
            return;
        }

        LuaClient.Instance.AttachProfiler();
    }

    [MenuItem("Lua/Detach Profiler 从Unity性能中 移除", false, 152)]
    static void DetachProfiler()
    {
        if (!Application.isPlaying)
        {            
            return;
        }

        LuaClient.Instance.DetachProfiler();
    }
}
