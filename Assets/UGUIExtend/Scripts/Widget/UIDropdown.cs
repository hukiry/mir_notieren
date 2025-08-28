using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hukiry.UI
{
    [DrawIcon(typeof(Dropdown), null)]
    public class UIDropdown : Dropdown
    {
        private UnityAction clickCallBack;
        private string sortingLayerName = string.Empty;
        public void AddListener(UnityAction clickCallBack)
        {
            this.clickCallBack = clickCallBack;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            this.clickCallBack?.Invoke();
            base.OnPointerClick(eventData);
        }

        protected override GameObject CreateDropdownList(GameObject template)
        {
            template.GetComponent<Canvas>().sortingLayerName = GetComponentInParent<Canvas>().sortingLayerName;
            return base.CreateDropdownList(template);
        }

        protected override GameObject CreateBlocker(Canvas rootCanvas)
        {
            rootCanvas.sortingLayerName = sortingLayerName;
            return base.CreateBlocker(rootCanvas);
        }
    }
}
