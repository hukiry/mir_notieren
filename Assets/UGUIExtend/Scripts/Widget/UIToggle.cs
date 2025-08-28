using System;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [DrawIcon(typeof(Toggle), null)]
    public class UIToggle : Toggle
    {
        public Action<int, bool> onChangeToggle;
        protected override void Awake()
        {
            this.onValueChanged.AddListener(OnToggle);
        }

        private void OnToggle(bool arg0)
        {
            int index = 0;
            if (this.group)
            {
                int childCount = this.group.transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    var tf = this.group.transform.GetChild(i);
                    if (tf == this.transform)
                    {
                        index = i;
                        break;
                    }
                }
            }

            onChangeToggle?.Invoke(index, arg0);
        }
    }
}
