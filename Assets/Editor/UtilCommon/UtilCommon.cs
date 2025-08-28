using System;
using System.Collections.Generic;
using UnityEditor;


public class UtilCommon
{
	public class ResMethod
	{
		public string btnName;
		public string methodName;
	}

	private Dictionary<int, ResMethod> m_resMethod = new Dictionary<int, ResMethod>();
	public void AddResMethod(int id,string methodName,string btnName)
	{
		m_resMethod[id] = new ResMethod()
		{
			methodName = methodName,
			btnName=btnName
		};
	}

	public void AddResMethod(int id, ResMethod res)
	{
		m_resMethod[id] = res;
	}

	public ResMethod GetResMethod(int id)
	{
		if (!m_resMethod.ContainsKey(id))
		{
			return null;
		}
		return m_resMethod[id];
	}

	public void CallInstance<T>(T obj, string methodName, params object[] objs)
	{
		Hukiry.HukiryUtilEditor.InvokeInstance<T>(obj, methodName, objs);
	}

	public void CallStatic<T>(string methodName, params object[] objs)
	{
		Hukiry.HukiryUtilEditor.InvokeStatic<T>(methodName, objs);
	}

	public static int GetTotalSeconds()
	{
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return (int)ts.TotalSeconds;
	}

	public static long GetTotalMilliseconds()
	{
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return (long)ts.TotalMilliseconds;
	}

	public static string ToTimeStringMS(float time_seconds)
	{
		int m = (int)(time_seconds) / 60;
		int s = (int)(time_seconds) % 60;
		return string.Format("{0:D2}:{1:D2}", m, s);
	}

}
