﻿
using UnityEngine;
using UnityEngine.UI;

namespace Hukiry
{
    [RequireComponent(typeof(Hukiry.HukirySupperText), typeof(RectTransform))]
    [AddComponentMenu("UI/UI Curved Text")]
    public class UICurvedText : BaseMeshEffect
    {
        [SerializeField]
        private AnimationCurve _curveForText = AnimationCurve.Linear(0, 0, 1, 10);
        [SerializeField]
        private float _curveMultiplier = 1;
        private RectTransform rectTrans;


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (_curveForText[0].time != 0)
            {
                var tmpRect = _curveForText[0];
                tmpRect.time = 0;
                _curveForText.MoveKey(0, tmpRect);
            }
            if (rectTrans == null)
                rectTrans = GetComponent<RectTransform>();
            if (_curveForText[_curveForText.length - 1].time != rectTrans.rect.width)
                OnRectTransformDimensionsChange();
        }
#endif
        protected override void Awake()
        {
            base.Awake();
            rectTrans = GetComponent<RectTransform>();
            OnRectTransformDimensionsChange();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            rectTrans = GetComponent<RectTransform>();
            OnRectTransformDimensionsChange();
        }
        public override void ModifyMesh(VertexHelper vh)
        {
            int count = vh.currentVertCount;
            if (!IsActive() || count == 0)
            {
                return;
            }
            for (int index = 0; index < vh.currentVertCount; index++)
            {
                UIVertex uiVertex = new UIVertex();
                vh.PopulateUIVertex(ref uiVertex, index);
                uiVertex.position.y += _curveForText.Evaluate(rectTrans.rect.width * rectTrans.pivot.x + uiVertex.position.x) * _curveMultiplier;
                vh.SetUIVertex(uiVertex, index);
            }
        }
        protected override void OnRectTransformDimensionsChange()
        {
            if (rectTrans)
            {
                Keyframe tmpRect = _curveForText[_curveForText.length - 1];
                tmpRect.time = rectTrans.rect.width;
                _curveForText.MoveKey(_curveForText.length - 1, tmpRect);
            }
        }
    }
}
