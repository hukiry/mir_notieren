using Hukiry.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIButtonListener : UIButton
{
	public static UIButtonListener Get(GameObject go)
	{
		if (go.GetComponent<Graphic>() == null)
		{
			go.AddComponent<UIBoxCollider>().raycastTarget = true;
		}
		else
		{
			go.GetComponent<Graphic>().raycastTarget = true;
		}
		UIButtonListener btn = go.GetComponent<UIButtonListener>() ?? go.AddComponent<UIButtonListener>();
		return btn;
	}
	public static UIButtonListener Get(Transform go) => Get(go.gameObject);
	public static UIButtonListener Get(MonoBehaviour go) => Get(go.gameObject);
	public static UIButtonListener Get(UIBehaviour go) => Get(go.gameObject);
	public static UIButtonListener Get(Behaviour go) => Get(go.gameObject);
	public static UIButtonListener Get(Component go) => Get(go.gameObject);
	public static UIButtonListener Get(AtlasImage go) => Get(go.gameObject);
	public static UIButtonListener Get(Text go) => Get(go.gameObject);
	public static UIButtonListener Get(Image go) => Get(go.gameObject);
	public static UIButtonListener Get(RawImage go) => Get(go.gameObject);
}
