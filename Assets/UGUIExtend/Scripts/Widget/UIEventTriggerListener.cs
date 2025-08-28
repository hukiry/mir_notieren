using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using Hukiry.UI;

[DisallowMultipleComponent]
[DrawIcon(typeof(EventTrigger), null)]
[RequireComponent(typeof(EventTrigger))][Obsolete("use UIEventListener replace", true)]
public class UIEventTriggerListener : MonoBehaviour
{
	public object paramsObject;
	public int paramInt;
	public int paramTwoInt;
	public bool paramBool;
	public string paramString;
	public float paramFloat;
	public GameObject paramGameObject;
	//同意按钮声音播放回调
	public static Action audioPlayAction = null;

	/// <summary>
	/// BaseEventData 受Unity版本限制
	/// </summary>
	/// <param name="data"></param>
	public delegate void EventListenerHandle(BaseEventData data);

	public delegate void EventGameObjectHandle(GameObject gameObject);
	/// <summary>
	/// 函数参数使用时，需要转换为子类 PointerEventData 
	/// </summary>
	public EventGameObjectHandle onClickEnter = null,
		onClickExit = null,
		onClickDown = null,
		onClickUp = null,
		onClick = null,

		onClickDrag = null,
		onClickDrop = null,
		onClickScroll = null,
		onClickUpdateSelected = null,
		onClickSelect = null,
		onClickDeselect = null,
		onClickMove = null,
		onClickInitializePotentialDrag = null,
		onClickBeginDrag = null,
		onClickEndDrag = null,
		onClickSubmit = null,
		onClickCancel = null;

	public EventGameObjectHandle onClickGameObject = null;
	public static UIEventTriggerListener Get(GameObject go)
	{
		UIEventTriggerListener _UGUIEventListener = go.GetComponent<UIEventTriggerListener>() ?? go.AddComponent<UIEventTriggerListener>();
		EventTrigger _trigger = go.GetComponent<EventTrigger>() ?? go.AddComponent<EventTrigger>();
		if (_trigger.triggers == null || _trigger.triggers.Count == 0)
		{
			_trigger.triggers = new List<EventTrigger.Entry>();
			_trigger.triggers.AddRange(_UGUIEventListener.RegisterEvent());
		}
		else
		{
			_trigger.triggers.Clear();
			_trigger.triggers.AddRange(_UGUIEventListener.RegisterEvent());
		}
		return _UGUIEventListener;
	}
	public static UIEventTriggerListener Get(Transform transform)=> Get(transform.gameObject);
	public static UIEventTriggerListener Get(MonoBehaviour monoBehaviour)=> Get(monoBehaviour.gameObject);
	public static UIEventTriggerListener Get(Toggle monoBehaviour)=> Get(monoBehaviour.gameObject);
	public static UIEventTriggerListener Get(AtlasImage monoBehaviour)=> Get(monoBehaviour.gameObject);
	public static UIEventTriggerListener Get(Image monoBehaviour)=> Get(monoBehaviour.gameObject);
	public static UIEventTriggerListener Get(Text monoBehaviour)=> Get(monoBehaviour.gameObject);
	private List<EventTrigger.Entry> RegisterEvent()
	{
		List<EventTrigger.Entry> entryList = new List<EventTrigger.Entry>();
		IList<UnityAction<BaseEventData>> funList = new List<UnityAction<BaseEventData>>(){
			PointerEnter,
			PointerExit,
			PointerDown,
			PointerUp,
			PointerClick,
			Drag,
			Drop,
			Scroll,
			UpdateSelected,
			Select,
			Deselect,
			Move,
			InitializePotentialDrag,
			BeginDrag,
			EndDrag,
			Submit,
			Cancel
		};
		for (int i = 0; i < funList.Count; i++)
		{
			EventTriggerType _eventTriggerType = (EventTriggerType)Enum.Parse(typeof(EventTriggerType), funList[i].Method.Name);
			EventTrigger.Entry _entry = new EventTrigger.Entry();
			_entry.callback.AddListener(funList[i]);
			_entry.eventID = _eventTriggerType;
			entryList.Add(_entry);
		}
		return entryList;

	}

	void PointerEnter(BaseEventData data)
	{
		onClickEnter?.Invoke(this.gameObject);
	}

	void PointerExit(BaseEventData data)
	{
		onClickExit?.Invoke(this.gameObject);
	}

	void PointerDown(BaseEventData data)
	{
		onClickDown?.Invoke(this.gameObject);
	}

	void PointerUp(BaseEventData data)
	{
		onClickUp?.Invoke(this.gameObject);
	}

	void PointerClick(BaseEventData data)
	{
		onClick?.Invoke(this.gameObject);
		audioPlayAction?.Invoke();
	}

	void Drag(BaseEventData data)
	{
		onClickDrag?.Invoke(this.gameObject);
	}

	void Drop(BaseEventData data)
	{
		onClickDrop?.Invoke(this.gameObject);
	}

	void Scroll(BaseEventData data)
	{
		onClickScroll?.Invoke(this.gameObject);
	}

	void UpdateSelected(BaseEventData data)
	{
		onClickUpdateSelected?.Invoke(this.gameObject);
	}

	void Select(BaseEventData data)
	{
		onClickSelect?.Invoke(this.gameObject);
	}

	void Deselect(BaseEventData data)
	{
		onClickDeselect?.Invoke(this.gameObject);
	}

	void Move(BaseEventData data)
	{
		onClickMove?.Invoke(this.gameObject);
	}

	void InitializePotentialDrag(BaseEventData data)
	{
		onClickInitializePotentialDrag?.Invoke(this.gameObject);
	}

	void EndDrag(BaseEventData data)
	{
		onClickEndDrag?.Invoke(this.gameObject);

	}

	void Submit(BaseEventData data)
	{
		onClickSubmit?.Invoke(this.gameObject);
	}

	void Cancel(BaseEventData data)
	{
		onClickCancel?.Invoke(this.gameObject);
	}

	void BeginDrag(BaseEventData data)
	{
		onClickBeginDrag?.Invoke(this.gameObject);
	}
}
