using Hukiry;
using Hukiry.UI;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class WidgetMenu
{

	[MenuItem("GameObject/UI/Hukiry Text", false, 1001)]
	static public void AddHukirySupperText(MenuCommand menuCommand)
	{
		CreateComponent<HukirySupperText>(menuCommand, hukiry => {
			hukiry.SetShadowStyle();
			hukiry.raycastTarget = false;
			hukiry.text = "New Text";
		});
	}
	[MenuItem("GameObject/UI/UI Progressbar Mask", false, 1003)]
	static public void AddUIProgressbarMask(MenuCommand menuCommand)
	{
		CreateComponent<UIProgressbarMask>(menuCommand, mask => {
			mask.fillAmount = 1;
		});
	}

	[MenuItem("GameObject/UI/Atlas Image", false, 1000)]
	static public void AddAtlasImage(MenuCommand menuCommand)
	{
		CreateComponent<AtlasImage>(menuCommand, img=> {
			img.raycastTarget = false;
			string atlasPath = AtlasImageAssetSetting.Instance.lastAtlasPath;
			if (string.IsNullOrEmpty(atlasPath))
			{
				img.spriteAtlas = null;
			}
			else
			{
				img.spriteAtlas = AssetDatabase.LoadAssetAtPath(atlasPath, typeof(SpriteAtlas)) as SpriteAtlas;
			}

			img.spriteName = AtlasImageAssetSetting.Instance.lastSpriteName;
			img.SetNativeSize();
		});
	}

	private static void CreateComponent<T>( MenuCommand menuCommand, System.Action<T> actionCall) where T :Component
	{
		var canvas = Object.FindObjectsOfType(typeof(Canvas)).Cast<Canvas>().FirstOrDefault();
		var goName = $"New {typeof(T).Name}";
		if (canvas)
		{
			// Create a custom game object
			GameObject go = new GameObject(goName);
			T pi = go.AddComponent<T>();
			actionCall?.Invoke(pi);
			if (menuCommand.context)
			{
				GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
			}
			else
			{
				GameObjectUtility.SetParentAndAlign(go, canvas.gameObject);
			}

			Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
			Selection.activeObject = go;
		}
		else
		{
			GameObject newCanvas = new GameObject("Canvas");
			Canvas c = newCanvas.AddComponent<Canvas>();
			c.renderMode = RenderMode.ScreenSpaceOverlay;
			newCanvas.AddComponent<CanvasScaler>();
			newCanvas.AddComponent<GraphicRaycaster>();

			// Create a custom game object
			GameObject go = new GameObject(goName);
			T pi = go.AddComponent<T>();
			actionCall?.Invoke(pi);
			GameObjectUtility.SetParentAndAlign(go, newCanvas);

			Undo.RegisterCreatedObjectUndo(newCanvas, "Create " + go.name);
			Selection.activeObject = go;
		}

		var eventSystem = Object.FindObjectsOfType(typeof(EventSystem)).Cast<EventSystem>().FirstOrDefault();

		if (eventSystem == null)
		{
			GameObject eSystem = new GameObject("EventSystem");
			EventSystem e = eSystem.AddComponent<EventSystem>();
			eSystem.AddComponent<StandaloneInputModule>();
		}
	}

}
