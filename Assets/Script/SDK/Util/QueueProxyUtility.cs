using System.Collections.Generic;
using UnityEngine;

namespace Hukiry.SDK
{
    public class QueueProxyUtility : MonoBehaviour
    {
        private Queue<ProxyUnit> QueueMsg = new Queue<ProxyUnit>();
        private ProxyUnit unit;

        public void Add(ProxyUnit item)
        {
            QueueMsg.Enqueue(item);
        }

        private void Update()
        {
            if (unit == null)
            {
                if (QueueMsg.Count > 0)
                {
                    unit = QueueMsg.Dequeue();
                }
            }

            if (unit != null)
            {
                if (unit.frameCount <= 0)
                {
                    unit?.Runable();
                    unit = null;
                }
                else
                {
                    unit.frameCount--;
                }
            }
        }
    }
}