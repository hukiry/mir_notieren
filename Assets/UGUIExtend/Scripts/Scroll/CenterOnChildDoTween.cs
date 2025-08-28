using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Hukiry
{
	public enum ScrollDir
	{
		Horizontal,
		Vertical
	}

	/*1,需要DoTween插件
	 *2,需要附加到手动创建的Scroll View 上，设置对应的参数即可
	 */
	public class CenterOnChildDoTween : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
	{
		[SerializeField]
		private ScrollDir Dir = ScrollDir.Horizontal;

		[Header("对齐中点速度")]
		[SerializeField] public float ToCenterTime = 0.3f;
		[Header("中心点放大倍数")]
		[SerializeField] public float CenterScale = 1f;
		[Header("靠近中心点放大倍数")]
		[SerializeField] public float UnCenterScale = 0.9f;
		[Header("远离中心点放大倍数")]
		[SerializeField] public float farCenterScale = 0.8f;

		private ScrollRect _scrollView;
		private Transform _content;
		private RectTransform _content_recttsf;
		private List<float> _childrenPos = new List<float>();
		private float _targetPos;

		/// <summary>
		/// 当前中心child索引
		/// </summary>
		private int _curCenterChildIndex = -1;

		public int CenterChildIndex => _curCenterChildIndex;

		/// <summary>
		/// 当前中心ChildItem
		/// </summary>
		public GameObject CurCenterChildItem
		{
			get
			{
				GameObject centerChild = null;
				if (_content != null && _curCenterChildIndex >= 0 && _curCenterChildIndex < _content.childCount)
				{
					centerChild = _content.GetChild(_curCenterChildIndex).gameObject;
				}
				return centerChild;
			}
		}
		/// <summary>
		/// only call by lua 
		/// </summary>
		public Action<Transform, int> OnSetItemScale = null;

		/// <summary>
		/// 滚动结束回调
		/// </summary>
		public Action<int> OnScrollCall = null;

		private LayoutGroup layoutGroup = null;
		private HorizontalLayoutGroup layoutGroupHorizontal = null;
		private int enableState = 0;

		void Start()
		{
			StartExecute(true);
		}
		[ContextMenu("RePosition")]
		private void ReStartPosition()
		{
			StartExecute(false);
		}
		public void StartExecute(bool isEveryExecute, int jumpIndex = 0)
		{
			int index_1 = 0;
			var layoutView = transform.GetComponentInChildren<HorizontalOrVerticalLayoutGroup>();
			if (layoutView)
			{
				for (int i = layoutView.transform.childCount - 1; i >= 0; i--)
				{
					if (layoutView.transform.GetChild(i).gameObject.activeSelf == false)
					{
						index_1++;
					}
				}
			}


			_childrenPos.Clear();

			_scrollView = GetComponent<ScrollRect>();
			if (_scrollView == null)
			{
				Debug.LogError("ScrollRect is null");
				return;
			}
			_content = _scrollView.content;
			Dir = _scrollView.horizontal ? ScrollDir.Horizontal : ScrollDir.Vertical;

			layoutGroup = _content.GetComponent<LayoutGroup>();
			_content_recttsf = _content.GetComponent<RectTransform>();
			if (layoutGroup == null)
			{
				Debug.LogError("LayoutGroup component is null");
			}
			_scrollView.movementType = ScrollRect.MovementType.Unrestricted;
			float spacing = 0f;
			//根据dir计算坐标，Horizontal：存x，Vertical：存y
			switch (Dir)
			{
				case ScrollDir.Horizontal:
					if (layoutGroup is HorizontalLayoutGroup)
					{
						float childPosX = _scrollView.GetComponent<RectTransform>().rect.width * 0.5f - GetChildItemWidth(0) * 0.5f;
						spacing = (layoutGroup as HorizontalLayoutGroup).spacing;
						_childrenPos.Add(childPosX);     //添加第一个
						for (int i = 1; i < _content.childCount - index_1; i++)    //添加多个
						{
							if (_content.GetChild(i).gameObject.activeSelf)
							{
								childPosX -= GetChildItemWidth(i) * 0.5f + GetChildItemWidth(i - 1) * 0.5f + spacing;
								_childrenPos.Add(childPosX);
							}
						}
					}
					else
					{
						Debug.LogError("Horizontal ScrollView is using VerticalLayoutGroup");
					}
					break;
				case ScrollDir.Vertical:
					if (layoutGroup is VerticalLayoutGroup)
					{
						float childPosY = -_scrollView.GetComponent<RectTransform>().rect.height * 0.5f + GetChildItemHeight(0) * 0.5f;
						spacing = (layoutGroup as VerticalLayoutGroup).spacing;
						_childrenPos.Add(childPosY);
						for (int i = 1; i < _content.childCount - index_1; i++)
						{
							childPosY += GetChildItemHeight(i) * 0.5f + GetChildItemHeight(i - 1) * 0.5f + spacing;
							_childrenPos.Add(childPosY);
						}
					}
					else
					{
						Debug.LogError("Vertical ScrollView is using HorizontalLayoutGroup");
					}
					break;
			}
			ScrollTo(jumpIndex);

		}


		public void StartExecuteTwo()
		{
			_childrenPos.Clear();
			if (_scrollView == null)
			{
				_scrollView = GetComponent<ScrollRect>() ?? gameObject.AddComponent<ScrollRect>();
			}

			_content = _scrollView.content;
			_content_recttsf = _content.GetComponent<RectTransform>();
			if (layoutGroupHorizontal == null)
			{
				layoutGroupHorizontal = _content.GetComponent<HorizontalLayoutGroup>() ?? _content.gameObject.AddComponent<HorizontalLayoutGroup>();
			}
			_scrollView.movementType = ScrollRect.MovementType.Unrestricted;
		}

		public void AddGameobject(GameObject go, int index)
		{
			if (index > 0)
			{
				float w = GetChildItemWidth(0);
				float spacing = layoutGroupHorizontal.spacing;
				if (index > 1)
				{
					float childPosX = _childrenPos[_childrenPos.Count - 1] - w - spacing;
					_childrenPos.Add(childPosX);
				}
				else
				{
					float childPosX = _scrollView.GetComponent<RectTransform>().rect.width * 0.5f - w * 0.5f;
					_childrenPos.Add(childPosX);
				}
			}

		}

		public void ScrollTo(int index)
		{
			int curIndex = index < 0 ? 0 : index;
			_targetPos = FindClosestChildPos(_childrenPos[curIndex], out _curCenterChildIndex);

#if DOTWEEN
		_content.DOLocalMoveX(_targetPos, ToCenterTime).OnComplete(()=> {
			SetCellScale();
		});
#endif
		}

		/// <summary>
		/// 根据拖动来改变每一个子物体的缩放
		/// </summary>
		private void SetCellScale()
		{
			GameObject centerChild = null;
			for (int i = 0; i < _content.childCount; i++)
			{
				centerChild = _content.GetChild(i).gameObject;
				if (OnSetItemScale != null)
				{
					OnSetItemScale(centerChild.transform, Mathf.Abs(_curCenterChildIndex - i));
				}
				else
				{
					if (i == _curCenterChildIndex)
					{
						centerChild.transform.localScale = CenterScale * Vector3.one;
					}
					else if (Mathf.Abs(_curCenterChildIndex - i) == 1)
					{
						centerChild.transform.localScale = UnCenterScale * Vector3.one;
					}
					else if (Mathf.Abs(_curCenterChildIndex - i) == 2)
					{
						centerChild.transform.localScale = farCenterScale * Vector3.one;
					}
					else
					{
						centerChild.transform.localScale = (farCenterScale - 0.1F) * Vector3.one;
					}
				}
			}
		}
		private float GetChildItemWidth(int index)
		{
			return (_content.GetChild(index) as RectTransform).sizeDelta.x;
		}

		private float GetChildItemHeight(int index)
		{
			return (_content.GetChild(index) as RectTransform).sizeDelta.y;
		}

		void IDragHandler.OnDrag(PointerEventData eventData)
		{
            //这里会一直调用 实时更新中心点的下标 并作出缩放改变 所以如果需求是拖动结束的时候 这里不调用FindClosestChildPos
            switch (Dir)
            {
                case ScrollDir.Horizontal:
                    _targetPos = FindClosestChildPos(_content.localPosition.x, out _curCenterChildIndex);
                    break;
                case ScrollDir.Vertical:
                    _targetPos = FindClosestChildPos(_content.localPosition.y, out _curCenterChildIndex);
                    break;
            }
            SetCellScale();
		}
		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			_scrollView.StopMovement();
			switch (Dir)
			{
				case ScrollDir.Horizontal:
					_targetPos = FindClosestChildPos(_content.localPosition.x, out _curCenterChildIndex);
					_content.DOLocalMoveX(_targetPos, ToCenterTime);
					break;
				case ScrollDir.Vertical:
					_targetPos = FindClosestChildPos(_content.localPosition.y, out _curCenterChildIndex);
					_content.DOLocalMoveY(_targetPos, ToCenterTime);
					break;
			}

			OnScrollCall?.Invoke(_curCenterChildIndex);
			SetCellScale();

		}
		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			_content.DOKill();
			_curCenterChildIndex = -1;
		}

		private float FindClosestChildPos(float currentPos, out int curCenterChildIndex)
		{
			float closest = 0;
			float distance = Mathf.Infinity;
			curCenterChildIndex = -1;
			for (int i = 0; i < _childrenPos.Count; i++)
			{
				float pos = _childrenPos[i];
				float tempDistance = Mathf.Abs(pos - currentPos); //计算出与当前位置距离最小的索引
				if (tempDistance < distance)
				{
					distance = tempDistance;
					closest = pos;
					curCenterChildIndex = i;
				}
				else
					break;
			}
			return closest;
		}
	}
}