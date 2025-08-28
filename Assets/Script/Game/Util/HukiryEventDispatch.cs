using System;
using System.Collections.Generic;

namespace Hukiry
{
    /// <summary>
    /// 事件派发 仅C#使用
    /// </summary>
    public class HukiryEventDispatch : Singleton<HukiryEventDispatch>
	{

		public delegate void EventHandle(params object[] obj);
		private IDictionary<int, List<EventHandle>> handleDic;
		public void RegisterEvent(int eventId, EventHandle function)
		{
			if (handleDic == null)
				handleDic = new Dictionary<int, List<EventHandle>>();

			if (handleDic.TryGetValue(eventId, out List<EventHandle> eventHandle))
			{
				for (int i = 0; i < eventHandle.Count; i++)
				{
					if (eventHandle[i].Equals(function))
					{
						eventHandle.Remove(function);
						break;
					}
				}
			}
			else
			{
				handleDic[eventId] = new List<EventHandle>();
			}

			handleDic[eventId].Add(function);
		}

		public void RegisterEvent(Enum typeID, EventHandle function)
		{

			int eventId = typeID.GetHashCode();
			this.RegisterEvent(eventId, function);
		}

		public void RegisterEvent(string typeID, EventHandle function)
		{
			int eventId = typeID.GetHashCode();
			this.RegisterEvent(eventId, function);
		}

		/// <summary>
		/// 调用注册的函数
		/// </summary>
		/// <param name="eventId"></param>
		/// <param name="obj"></param>
		public void FireEvent(int eventId, params object[] obj)
		{
			if (handleDic != null && handleDic.TryGetValue(eventId, out List<EventHandle> eventHandle))
			{
				foreach (var handle in eventHandle)
				{
					handle?.Invoke(obj);
				}
			}
		}

		public void FireEvent(Enum typeID, params object[] obj)
		{
			int eventId = typeID.GetHashCode();
			this.FireEvent(eventId, obj);
		}

		public void FireEvent(string typeID, params object[] obj)
		{
			int eventId = typeID.GetHashCode();
			this.FireEvent(eventId, obj);
		}

		public void UnRegister(int eventId)
		{
			if (handleDic != null && handleDic.ContainsKey(eventId))
			{
				handleDic.Remove(eventId);
			}
		}

		public void UnAllRegister()
		{
			handleDic.Clear();
		}
	}
}