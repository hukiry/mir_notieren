using UnityEngine;
namespace Hukiry
{
    public static class GameObjectExtend
    {
		public static void ChangeLayer(this GameObject go, int layer)
		{
			if (go == null) return;

			var translist = go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < translist.Length; ++i)
			{
				translist[i].gameObject.layer = layer;
			}

			if (go.GetComponent<RectTransform>())
			{
				go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
			}
		}

	}
}
