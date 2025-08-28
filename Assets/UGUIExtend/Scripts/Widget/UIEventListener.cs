using Hukiry;
using Hukiry.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 支持所有的UI事件
/// </summary>
[DrawIcon("Listener", null)]
public class UIEventListener : UIBehaviour, IPointerDownHandler,IPointerUpHandler,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,
	IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler,IMoveHandler,ISelectHandler,IScrollHandler,ICancelHandler, IEventSystemHandler
{
	public delegate void UIHukiryHandler(GameObject pointer);
	public delegate void UIHukiryEventHandler(PointerEventData pointer);
	[SerializeField]
	public UIHukiryHandler onClick = null,onDoubleClick = null,onClickUp = null,onClickDown = null,onClickExit = null,
		onClickEnter = null,onClickDrop = null,onMove = null,onCancel = null, onSelect = null;
	public UIHukiryEventHandler onBeginDrag = null, onDrag = null, onEndDrag = null, onScroll = null;


	/// <summary>
	/// 拖拽结束和抬起按钮
	/// </summary>
	public static UIHukiryHandler onScrollEndUp = null;
	/// <summary>
	/// 声音名字
	/// </summary>
	[HideInInspector]
	public string soundId = "";

	/// <summary>
	/// 延时点击
	/// </summary>
	[HideInInspector]
	public float delayClickTime = 0.35f;

	/// <summary>
	/// 时间内双击有效
	/// </summary>
	[HideInInspector]
	public float doubleClickTime = 0.35f;

	/// <summary>
	/// 长按生效时间
	/// </summary>
	[HideInInspector]
	public float pressTime = 0.5f;

	private const float maxDoubleClickTime = 10;
	private const float maxcurrentPressTime = 1000;
	private float currentDoubleClickTime = -maxDoubleClickTime; 
	private float currentPressTime = maxcurrentPressTime * 10;  
	private float currentDelayClickTime = 0;

	private object paramObject;
	public void Set<T>(T param) => this.paramObject = param;
	public T Get<T>() => (T)this.paramObject;

	public static UIEventListener Get(GameObject go)
	{
		if (go.GetComponent<Graphic>() == null)
		{
			go.AddComponent<UIBoxCollider>().raycastTarget = true;
		}
		else
		{
			go.GetComponent<Graphic>().raycastTarget = true;
		}

		UIEventListener listener = go.GetComponent<UIEventListener>() ?? go.AddComponent<UIEventListener>();
		return listener;
	}

	public static UIEventListener Get(Transform go)=> Get(go.gameObject);
	public static UIEventListener Get(MonoBehaviour go) => Get(go.gameObject);
	public static UIEventListener Get(UIBehaviour go) => Get(go.gameObject);
	public static UIEventListener Get(Behaviour go) => Get(go.gameObject);
	public static UIEventListener Get(Component go) => Get(go.gameObject);
	public static UIEventListener Get(AtlasImage go) => Get(go.gameObject);
	public static UIEventListener Get(Text go) => Get(go.gameObject);
	public static UIEventListener Get(Image go) => Get(go.gameObject);
	public static UIEventListener Get(RawImage go) => Get(go.gameObject);
	public static UIEventListener Get(HukirySupperText go) => Get(go.gameObject);

	/// <summary>
	/// 发生事件回调
	/// </summary>
	/// <param name="go"></param>
	/// <param name="pEventType"></param>
	/// <returns></returns>
	public static bool ExecuteEvent(GameObject go, int pEventType, PointerEventData pointerEventData=null)
	{
		if(pointerEventData==null) pointerEventData = new PointerEventData(EventSystem.current);

		EventTriggerType eventType = (EventTriggerType)pEventType;
        switch (eventType)
		{
			case EventTriggerType.PointerUp:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.pointerUpHandler);
			case EventTriggerType.PointerClick:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.pointerClickHandler);
			case EventTriggerType.PointerDown:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.pointerDownHandler);
			case EventTriggerType.BeginDrag:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.beginDragHandler);
			case EventTriggerType.Drag:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.dragHandler);
			case EventTriggerType.EndDrag:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.endDragHandler);
			case EventTriggerType.PointerExit:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.pointerExitHandler);
			case EventTriggerType.PointerEnter:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.pointerEnterHandler);
			case EventTriggerType.Drop:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.dropHandler);
			case EventTriggerType.Scroll:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.scrollHandler);
			case EventTriggerType.UpdateSelected:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.updateSelectedHandler);
			case EventTriggerType.Select:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.selectHandler);
			case EventTriggerType.Deselect:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.deselectHandler);
			case EventTriggerType.Move:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.moveHandler);
			case EventTriggerType.InitializePotentialDrag:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.initializePotentialDrag);
			case EventTriggerType.Submit:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.submitHandler);
			case EventTriggerType.Cancel:
				return ExecuteEvents.Execute(go, pointerEventData, ExecuteEvents.cancelHandler);
			default:
				return false;
		}
	}

	public void AddUIScale()
	{
		if (this.gameObject.GetComponent<UIClickScale>() == null)
		{
			this.gameObject.AddComponent<UIClickScale>();
		}
	}

	private ScrollRect scrollRect;

	protected override void Start()
	{
		scrollRect = transform.GetComponentInParent<ScrollRect>();
	}

	#region 事件接口类实现

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		onClickExit?.Invoke(this.gameObject);
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		onClickEnter?.Invoke(this.gameObject);
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		if (Time.realtimeSinceStartup - currentDelayClickTime >= delayClickTime)
		{
			onClick?.Invoke(this.gameObject);
			//声音播放
			currentDelayClickTime = Time.realtimeSinceStartup;
		}
	}

	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		onClickUp?.Invoke(this.gameObject);
		if (scrollRect)
		{
			onScrollEndUp?.Invoke(this.gameObject);
		}
	}

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		onClickDown?.Invoke(this.gameObject);
		currentPressTime = Time.realtimeSinceStartup;
		if (Time.realtimeSinceStartup - currentDoubleClickTime < doubleClickTime)
		{
			onDoubleClick?.Invoke(this.gameObject);
			currentDoubleClickTime = -maxDoubleClickTime;
		}
		else
		{
			currentDoubleClickTime = Time.realtimeSinceStartup;
		}
	}

	void IDropHandler.OnDrop(PointerEventData eventData)
	{
		onClickDrop?.Invoke(this.gameObject);

	}

	void IMoveHandler.OnMove(AxisEventData eventData)
	{
		onMove?.Invoke(this.gameObject);
	}

	void ISelectHandler.OnSelect(BaseEventData eventData)
	{
		onSelect?.Invoke(this.gameObject);
	}

	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
	{
		onBeginDrag?.Invoke(eventData);
		if (scrollRect) scrollRect.OnBeginDrag(eventData);
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		currentDelayClickTime = Time.realtimeSinceStartup;
		onDrag?.Invoke(eventData);
        if (scrollRect) scrollRect.OnDrag(eventData);
    }

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		onEndDrag?.Invoke(eventData);
		if (scrollRect) scrollRect.OnEndDrag(eventData);
	}

	void IScrollHandler.OnScroll(PointerEventData eventData)
	{
		onScroll?.Invoke(eventData);
		if (scrollRect) scrollRect.OnScroll(eventData);
	}

	void ICancelHandler.OnCancel(BaseEventData eventData)
	{
		onCancel?.Invoke(this.gameObject);
	}


	#endregion 事件接口类实现
}
