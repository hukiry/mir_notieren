using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace UnityEngine
{
    /// <summary>
    /// 滚动视图 点击事件
    /// </summary>
    [DrawIcon(typeof(Button), null)]
	public class UIButton : Button
    {
		public Action<GameObject> onClickExtand = null;
		private const float MaxWaitTime = 0.5f;
		private float m_waitSecond = 1;
		public override void OnPointerClick(PointerEventData eventData)
        {
			if (Time.realtimeSinceStartup - m_waitSecond > MaxWaitTime)
			{
				base.OnPointerClick(eventData);

				onClickExtand?.Invoke(this.gameObject);
				//AudioManager.Instance.PlaySound(1000);
				m_waitSecond = Time.realtimeSinceStartup;
			}
		}

        //手动调用
        public void ClickHand()
        {
			onClickExtand?.Invoke(this.gameObject);
		}
	}
}
