using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hukiry.UI
{
    /// <summary>
    /// mask进度条遮罩，可以降底draw call
    /// </summary>
    [AddComponentMenu("UI/UI Progressbar Mask", 11)]
    [DrawIcon(typeof(SliderJoint2D), null, HierarchyIconLayout.After)]
    public class UIProgressbarMask : RectMask2D
    {
        [SerializeField, Range(0.0F, 1.0F)]
        private float m_fillAmount;
        [SerializeField, Tooltip("仅支持水平和垂直进度条")]
        private FillMethod m_fillMethod;
        [SerializeField, Tooltip("游标，必须向左对齐")]
        private RectTransform m_handle;
        [SerializeField, Tooltip("百分比文本")]
        private Text m_percentText;
        [SerializeField, Tooltip("百分比展示方式"), Description("Percent Method", typeof(PercentMethod))]
        private PercentMethod m_PercentMethod;
        [SerializeField, Tooltip("游标百分比最大数字"), Range(1, 100)]
        private int m_handlePercent = 100;

        /// <summary>
        /// 设置最大值，用于控制文本的显示
        /// </summary>
        public int ProgresssMax { get => m_handlePercent; set => m_handlePercent = value; }
        [System.Serializable]
        public class OnChangeEvent : UnityEvent<string> { }
        [SerializeField, Tooltip("支持游标数字变化")]
        private OnChangeEvent m_OnValueChanged = new OnChangeEvent();
        public float fillAmount
        {
            get => this.m_fillAmount; set
            {
                this.m_fillAmount = Mathf.Clamp(value, 0, 1);
                this.ChageFillRate();
            }
        }

        public FillMethod fillMethod
        {
            get => this.m_fillMethod; set
            {
                this.m_fillMethod = value;
                this.ChageFillRate();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (gameObject.GetComponent<Graphic>() == null)
            {
                gameObject.AddComponent<UIBoxCollider>();
            }

        }
        // Start is called before the first frame update
        protected override void OnEnable()
        {
            base.OnEnable();
        }

#if UNITY_EDITOR

        private int GetPersistentEventCount()
        {
            return this.m_OnValueChanged.GetPersistentEventCount();
        }
        protected override void OnValidate()
        {
            base.OnValidate();
            this.ChageFillRate();
        }
#endif

        private void ChageFillRate()
        {
            RectTransform rectTransform = transform as RectTransform;
            var paddingT = Vector4.zero;
            (float handleX, float handleY) = (m_handle ? m_handle.anchoredPosition.x : 0, m_handle ? m_handle.anchoredPosition.y : 0);
            switch (m_fillMethod)
            {
                case FillMethod.Horizontal:
                    paddingT.z = rectTransform.rect.width * (1 - m_fillAmount);//水平方向
                    handleX = rectTransform.rect.width * m_fillAmount;
                    break;
                case FillMethod.Vertical:
                    paddingT.w = rectTransform.rect.height * (1 - m_fillAmount);//垂直方向
                    handleY = rectTransform.rect.height * m_fillAmount;
                    break;
                case FillMethod.LeftHorizontal:
                    paddingT.x = rectTransform.rect.width * (1 - m_fillAmount);//水平反方向
                    handleX = rectTransform.rect.width * (1 - m_fillAmount);
                    break;
                case FillMethod.BottomVertical:
                    paddingT.y = rectTransform.rect.height * (1 - m_fillAmount);//垂直反方向
                    handleY = rectTransform.rect.height * (1 - m_fillAmount);
                    break;
                default:
                    break;
            }

            if (m_handle)
            {
                var handleoffset = this.GetOffsetHandle();
                m_handle.anchoredPosition = new Vector3(handleX + handleoffset.handleOffsetX, handleY + handleoffset.handleOffsetY, 0);
            }
            this.padding = paddingT;

            int percent = (int)(this.m_fillAmount * m_handlePercent);
            string result = $"{percent}/{m_handlePercent}";
            if (m_PercentMethod == PercentMethod.Number)
                result = percent.ToString();
            else if (m_PercentMethod == PercentMethod.Perscent_Number)
                result = $"{percent}%";

            m_OnValueChanged?.Invoke(result);
            if (m_percentText)
            {
                m_percentText.text = result;
            }
        }

        private (float handleOffsetX, float handleOffsetY) GetOffsetHandle()
        {
            var handleParent = this.m_handle.parent ? (this.m_handle.parent as RectTransform).rect : rectTransform.rect;
            var isHorizontal = (m_fillMethod == FillMethod.Horizontal || m_fillMethod == FillMethod.LeftHorizontal);
            var diff = isHorizontal ? (handleParent.width - rectTransform.rect.width) / 2 : (handleParent.height - rectTransform.rect.height) / 2;
            if (isHorizontal)
            {
                return (diff, 0);
            }
            return (0, diff);
        }

        /// X = Left
        /// Y = Bottom
        /// Z = Right
        /// W = Top
        public enum FillMethod
        {
            Horizontal = 0,
            Vertical = 1,

            LeftHorizontal = 2,
            BottomVertical = 3,
        }

        public enum PercentMethod
        {
            [Description("大小分割[0_100]", typeof(PercentMethod))]
            Size = 0,
            [Description("纯数字[50]", typeof(PercentMethod))]
            Number = 1,
            [Description("百分号[50%]", typeof(PercentMethod))]
            Perscent_Number = 2,
        }
    }
}
