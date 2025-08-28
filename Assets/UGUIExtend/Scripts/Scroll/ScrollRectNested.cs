using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine
{
    //嵌套循环滚动视图
    [DrawIcon(typeof(ScrollRect), null)]
    public class ScrollRectNested : ScrollRect
    {
        /// <summary>
        /// 滑动方向
        /// </summary>
        enum EScrollDir { Horizontal, Vertical, }
        ScrollRectNested m_ScrollParent;//不同滑动方向的滑动框
        EScrollDir m_ScrollDir;//组件滑动方向
        EScrollDir m_CurScrollDir;//当前滑动方向
        protected override void Awake()
        {
            base.Awake();
            Transform parent = transform.parent;
            if (parent != null)
            {
                m_ScrollParent = parent.GetComponentInParent<ScrollRectNested>();
            }
            m_ScrollDir = horizontal ? EScrollDir.Horizontal : EScrollDir.Vertical;
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (m_ScrollParent != null)
            {
                m_CurScrollDir = Mathf.Abs(eventData.delta.x) >= Mathf.Abs(eventData.delta.y)
                ? EScrollDir.Horizontal
                : EScrollDir.Vertical;
                if (m_CurScrollDir != m_ScrollDir)
                {
                    ExecuteEvents.Execute(m_ScrollParent.gameObject, eventData, ExecuteEvents.beginDragHandler);
                    return;
                }
            }
            base.OnBeginDrag(eventData);
        }
        public override void OnDrag(PointerEventData eventData)
        {
            if (m_ScrollParent != null)
            {
                if (m_CurScrollDir != m_ScrollDir)
                {
                    ExecuteEvents.Execute(m_ScrollParent.gameObject, eventData, ExecuteEvents.dragHandler);
                    return;
                }
            }
            base.OnDrag(eventData);
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            if (m_ScrollParent != null)
            {
                if (m_CurScrollDir != m_ScrollDir)
                {
                    ExecuteEvents.Execute(m_ScrollParent.gameObject, eventData, ExecuteEvents.endDragHandler);
                    return;
                }
            }
            base.OnEndDrag(eventData);
            UIEventListener.onScrollEndUp?.Invoke(null);
        }
        public override void OnScroll(PointerEventData data)
        {
            if (m_ScrollParent != null)
            {
                if (m_CurScrollDir != m_ScrollDir)
                {
                    ExecuteEvents.Execute(m_ScrollParent.gameObject, data, ExecuteEvents.scrollHandler);
                    return;
                }
            }
            base.OnScroll(data);


        }
    }

}