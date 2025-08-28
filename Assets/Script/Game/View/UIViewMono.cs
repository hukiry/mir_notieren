using UnityEngine;
public abstract class UIViewMono<T> : MonoBehaviour where T : MonoBehaviour
{
	private  static T _intance;
	public static T Instance
	{
		get
		{
			if (_intance == null)
			{
				var layerTransform = RootCanvas.Instance.GetTransformLayer<T>();
				_intance = layerTransform.gameObject.AddComponent<T>();
			}
			return _intance;
		}
	}

	public abstract void HideView();
}
