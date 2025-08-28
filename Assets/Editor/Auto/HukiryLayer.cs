using System.Reflection;
using UnityEditorInternal;
using UnityEngine;

namespace Hukiry
{
    public class HukiryLayer
	{
		public static string[] GetSortingLayerNames()
		{
			return BindingCallProperty<string[]>("sortingLayerNames", new object[0]);
		}

		public static void AddSortingLayer(string sortingLayerName, int index)
		{
			var item = IsContainSortingLayerName(sortingLayerName, index);
			if (!item.isExit)
			{
				if(item.isUpdate) HukiryLayer.AddSortingLayer();
				HukiryLayer.SetSortingLayerName(sortingLayerName, index);
				HukiryLayer.UpdateSortingLayersOrder();
			}
		}

		public static string GetSortingLayerNameFromUniqueID(int uniqueID)
		{
			return BindingCallFunction<string>("GetSortingLayerNameFromUniqueID", uniqueID);
		}

		public static (bool isExit, bool isUpdate) IsContainSortingLayerName(string sortingLayerName, int index)
		{
			int length = GetSortingLayerCount();
			for (int i = 0; i < length; i++)
			{
				if (GetSortingLayerName(i) == sortingLayerName)
				{
					return (i == index, false);
				}
			}
			return (false, true);
		}

		public static int GetSortingLayerUniqueID(int index)
		{
			return BindingCallFunction<int>("GetSortingLayerUniqueID", index);
		}

		private static string GetSortingLayerName(int index)
		{
			return BindingCallFunction<string>("GetSortingLayerName", index);
		}

		private static bool IsSortingLayerDefault(int index)
		{
			return BindingCallFunction<bool>("IsSortingLayerDefault", index);
		}

		private static int GetSortingLayerCount()
		{
			return BindingCallFunction<int>("GetSortingLayerCount");
		}
		
		private static void SetSortingLayerLocked(bool locked, int index=-1)
		{
			int max = GetSortingLayerCount();
			index = Mathf.Clamp(index, 0, max - 1);
			if(index==-1)
			{
				index = max - 1;
			}
			BindingCallFunction("SetSortingLayerLocked", index, locked);
		}

		//设置已经纯在的
		private static void SetSortingLayerName(string name, int index)
		{
			BindingCallFunction("SetSortingLayerName", index, name);
		}

		private static void AddSortingLayer()
		{
			BindingCallFunction("AddSortingLayer");
		}

		private static void UpdateSortingLayersOrder()
		{
			BindingCallFunction("UpdateSortingLayersOrder");
		}

		private static T BindingCallFunction<T>(string methodName, params object[] objects)
		{
			var internalEditorUtilityType = typeof(InternalEditorUtility);
			var methodInfo = internalEditorUtilityType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
			return (T)methodInfo.Invoke(internalEditorUtilityType, objects);
		}

		private static T BindingCallProperty<T>(string methodName, params object[] objects)
		{
			var internalEditorUtilityType = typeof(InternalEditorUtility);
			var methodInfo = internalEditorUtilityType.GetProperty(methodName, BindingFlags.Static | BindingFlags.NonPublic);
			return (T)methodInfo.GetValue(internalEditorUtilityType, objects);
		}

		private static void BindingCallFunction(string methodName, params object[] objects)
		{
			var internalEditorUtilityType = typeof(InternalEditorUtility);
			var methodInfo = internalEditorUtilityType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
			methodInfo.Invoke(internalEditorUtilityType, objects);
		}
	}
}
