using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;
using Hukiry.UI;

public class MeshGridTool
{
	SceneViewGrid drawGridRect;
	List<string> resSpriteNames = new List<string>();
	private Vector2 _spritesScrollView = Vector2.zero;
	public  const int ICON_WIDTH = 160;
	private static int selectIndex = 0;
	GUIContent deleteContent;
	public MeshGridTool()
	{
		deleteContent = new GUIContent(Hukiry.HukiryUtilEditor.GetTexture2D("ClothInspector.PaintValue"), "橡皮擦");
		this.drawGridRect = GameObject.FindObjectOfType<SceneViewGrid>();
		resSpriteNames.Clear();
		selectIndex = 0;
	}

	public SceneViewGrid GetDrawGridRect() => this.drawGridRect;
	//绘制windon列表
	public void UpdateDraw(Rect rect, Action<string> callBack, SpriteAtlas spriteAtlas)
	{
		Vector2Int iconSize = new Vector2Int(100, 100);
		_spritesScrollView = GUILayout.BeginScrollView(_spritesScrollView, "HelpBox", GUILayout.Height(rect.height - 20));
		using (new EditorGUILayout.VerticalScope())
		{
			int length = spriteAtlas.spriteCount;
			Sprite[] sprites = new Sprite[length];
			spriteAtlas.GetSprites(sprites);
			for (int i = 0; i < length; i++)
			{
				var sprite = sprites[i];
				var spriteName = sprite.name.Replace("(Clone)", "");
				if (resSpriteNames.Contains(spriteName)) continue;

				if (GUILayout.Button(new GUIContent(i.ToString(), "移除精灵"), "OL Minus", GUILayout.Width(30)))
				{
					resSpriteNames.Add(spriteName);
				}
				DrawIconButton(sprite, i == selectIndex, iconSize, () =>
				{
					//if (MapMeshGridGlobalAsset.Instance.isDelete)
					//{
					//	Hukiry.HukiryUtilEditor.SceneView.ShowNotification(new GUIContent("正在编辑中，无法选择！"));
					//}
					//else
					{
						selectIndex = i;
						callBack(spriteName);
					}
				});

				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				EditorGUILayout.LabelField(spriteName, GUILayout.Width(iconSize.x));

				GUILayout.Space(10);
				GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			}

		}
		GUILayout.EndScrollView();

	}

	//绘制场景列表
	public void DrawList(Rect rect,Vector2Int size, Action<string> action, SpriteAtlas spriteAtlas)
	{
		
		_spritesScrollView = GUILayout.BeginScrollView(_spritesScrollView, "HelpBox", GUILayout.Height(rect.height - 50));
		if (spriteAtlas != null)
		{
			int length = spriteAtlas.spriteCount;
			Sprite[] sprites = new Sprite[length];
			spriteAtlas.GetSprites(sprites);
			for (int i = 0; i < length; i++)
			{
				var sprite = sprites[i];
				var spriteName = sprite.name.Replace("(Clone)", "");
				DrawIconButton(sprite, i==selectIndex, size,()=>
				{
					//if (MapMeshGridGlobalAsset.Instance.isDelete)
					//{
					//	Hukiry.HukiryUtilEditor.SceneView.ShowNotification(new GUIContent("橡皮擦模式下，无法选择！"));
					//}
					//else
					{
						selectIndex = i;
						action?.Invoke(spriteName);
					}
				});
				EditorGUILayout.TextField(spriteName, GUILayout.Width(size.x));
				GUILayout.Space(5);
			}
		}
		GUILayout.EndScrollView();
	}

	//绘制橡皮擦
	public void DrawEraser()
	{
		//deleteContent.text = MapMeshGridGlobalAsset.Instance.isDelete ? "正在擦除..." : "橡皮擦";
		//GUI.color = MapMeshGridGlobalAsset.Instance.isDelete ? new Color(1F,0.5f,0.5F,1) : Color.white;
		////5，橡皮擦+图片
		//if (GUILayout.Button(deleteContent))
		//{
		//	MapMeshGridGlobalAsset.Instance.isDelete = !MapMeshGridGlobalAsset.Instance.isDelete;
		//	this.drawGridRect.SelectSprite();
		//}
		//GUI.color = Color.white;
	}

	public static void DrawIconButton(Sprite sprite,bool isSelectIndex, Vector2Int size, Action action)
	{
		if (sprite)
		{
			
			GUI.skin.label.alignment = TextAnchor.LowerCenter;
			GUILayout.Label("", GUILayout.Width(size.x), GUILayout.Height(size.y));
			Rect rect = GUILayoutUtility.GetLastRect();
			if (isSelectIndex)
			{
				//AtlasSpriteSelector.DrawOutline(rect, MapMeshGridGlobalAsset.Instance.GetColor());
			}

			if (GUI.Button(rect,"", GUI.skin.box))
			{
				action?.Invoke();
			}
			DrawSprite( sprite, rect);
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		}
	}

	public static void DrawSprite(Sprite sprite, Rect drawArea)
	{
		if (sprite == null)
			return;

		Texture2D tex = sprite.texture;
		if (tex == null)
			return;

		Rect outer = sprite.rect;
		Rect inner = outer;
		inner.xMin += sprite.border.x;
		inner.yMin += sprite.border.y;
		inner.xMax -= sprite.border.z;
		inner.yMax -= sprite.border.w;

		Vector4 uv4 = UnityEngine.Sprites.DataUtility.GetOuterUV(sprite);
		Rect uv = new Rect(uv4.x, uv4.y, uv4.z - uv4.x, uv4.w - uv4.y);
		GUI.DrawTextureWithTexCoords(drawArea, tex, uv, true);
	}

}


public enum ExportType
{
	SaveMap,
	LoadMap,
	CreateMap,
}