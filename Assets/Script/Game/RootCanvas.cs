using Hukiry;
using HukiryInitialize;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI
/// </summary>
public class RootCanvas : MonoBehaviour
{
	public static RootCanvas Instance;
	public Camera UICamera { get; private set; }

	[SerializeField]
	private Transform loadingTF;
	[SerializeField] 
	private Transform promptBoxTF;
	void Awake()
	{
		Instance = this;
		UICamera = gameObject.GetComponent<Canvas>().worldCamera;

		var v= gameObject.GetComponent<CanvasScaler>().referenceResolution;
		LogManager.Log("UI Resolution:", v.x, v.y);
	}

    //[LuaInterface.NoToLua]
    public Transform GetTransformLayer<T>()
    {
        if (loadingTF == null || promptBoxTF == null)
            LogManager.LogError("C# 中脚本挂载 MainCanvas 没有赋值！");
        Transform tf = typeof(T) == typeof(Loading) ? loadingTF : promptBoxTF;
        if (tf)
        {
            tf.gameObject.SetActive(true);
            var rectTransform = tf as RectTransform;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
        }
        return tf;
    }

    /// <summary>
    /// 判断所有UGUI
    /// </summary>
    /// <returns></returns>
    public bool IsTouchAllUI()
	{
#if UNITY_EDITOR
		if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
		{
#else
		if (Input.touchCount > 0) {
#endif
			PointerEventData eventData = new PointerEventData(EventSystem.current);
#if UNITY_EDITOR
			eventData.position = Input.mousePosition;
#else
			eventData.position = Input.GetTouch(0).position;
#endif
			List<RaycastResult> raycastResults = new List<RaycastResult>();
			//向点击位置发射一条射线，检测是否点击UI
			EventSystem.current.RaycastAll(eventData, raycastResults);
			return raycastResults.Count > 0;
		}
		return false;
	}

	/// <summary>
	/// 指定GraphicRaycaster发出的射线
	/// </summary>
	/// <returns></returns>
	public bool IsTouchMainUI()
	{
#if UNITY_EDITOR
		if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
		{
#else
		if (Input.touchCount > 0) {
#endif
			PointerEventData eventData = new PointerEventData(EventSystem.current);
#if UNITY_EDITOR
			eventData.position = Input.mousePosition;
#else
			eventData.position = Input.GetTouch(0).position;
#endif
			List<RaycastResult> raycastResults = new List<RaycastResult>();
			//向点击位置发射一条射线，检测是否点击UI
			gameObject.GetComponent<GraphicRaycaster>().Raycast(eventData, raycastResults);
			return raycastResults.Count > 0;
		}
		return false;
	}

	/// <summary>
	/// 获取鼠标停留处UI对象 (慎用)
	/// </summary>
	/// <param name="canvas"></param>
	/// <returns></returns>
	public GameObject GetOverUI()
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = Input.mousePosition;
		GraphicRaycaster pr = gameObject.GetComponent<GraphicRaycaster>();
		List<RaycastResult> results = new List<RaycastResult>();
		pr.Raycast(pointerEventData, results);
		if (results.Count != 0)
		{
			return results[0].gameObject;
		}
		return null;
	}

	Dictionary<string, byte> UnitySymbols = new Dictionary<string, byte>();
	public bool GetSymbols(string symbolName)
	{
		if (UnitySymbols.ContainsKey(symbolName)) return true;

#if UNITY_EDITOR
		UnitySymbols[nameof(UnitySymbol.UNITY_EDITOR)] = 0;
#endif

#if UNITY_ANDROID
		UnitySymbols[nameof(UnitySymbol.UNITY_ANDROID)] = 0;
#endif

#if UNITY_IOS || UNITY_IPHONE
		UnitySymbols[nameof(UnitySymbol.UNITY_IOS)] = 0;
#endif

#if UNITY_STANDALONE
		UnitySymbols[nameof(UnitySymbol.UNITY_STANDALONE_OSX)] = 0;
#endif

#if UNITY_STANDALONE_WIN
		UnitySymbols[nameof(UnitySymbol.UNITY_STANDALONE_WIN)] = 0;
#endif

#if UNITY_STANDALONE_LINUX
		UnitySymbols[nameof(UnitySymbol.UNITY_STANDALONE_LINUX)] = 0;
#endif

#if UNITY_STANDALONE
		UnitySymbols[nameof(UnitySymbol.UNITY_STANDALONE)] = 0;
#endif

#if ASSETBUNDLE_TEST
		UnitySymbols[nameof(UnitySymbol.ASSETBUNDLE_TEST)] = 0;
#endif

#if RELEASE
		UnitySymbols[nameof(UnitySymbol.RELEASE)] = 0;
#endif

#if DEVELOP
		UnitySymbols[nameof(UnitySymbol.DEVELOP)] = 0;
#endif

#if DEBUG
		UnitySymbols[nameof(UnitySymbol.DEBUG)] = 0;
#endif

#if SYSTEM_INFO
		UnitySymbols[nameof(UnitySymbol.SYSTEM_INFO)] = 0;
#endif

#if ENABLE_SDK
		UnitySymbols[nameof(UnitySymbol.ENABLE_SDK)] = 0;
#endif

#if ENABLE_FPS
		UnitySymbols[nameof(UnitySymbol.ENABLE_FPS)] = 0;
#endif

#if ENABLE_SOCKET
		UnitySymbols[nameof(UnitySymbol.ENABLE_SOCKET)] = 0;
#endif


#if STRONG_SOCKET
		UnitySymbols[nameof(UnitySymbol.STRONG_SOCKET)] = 0;
#endif

#if USE_CCHARP
		UnitySymbols[nameof(UnitySymbol.USE_CCHARP)] = 0;
#endif

#if ENABLE_LUA
		UnitySymbols[nameof(UnitySymbol.ENABLE_LUA)] = 0;
#endif

		return UnitySymbols.ContainsKey(symbolName);
	}
}
