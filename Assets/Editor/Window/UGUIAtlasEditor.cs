using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;
using UnityEngine.U2D;
using UnityEditor.U2D;
using System;
using System.IO;
using System.Reflection;

public class UGUIAtlasEditor : CreateWindowEditor<UGUIAtlasEditor>
{
	public class atlas_data
	{
		
		public SpriteAtlas atlas;
		public bool isExport = true;
	}

	private List<atlas_data> m_atlas = new List<atlas_data>();
	private string assetPath = "Assets/ResourceData/Atlas";

	private static MethodInfo GetPreviewTexturesMethod = null, GetPackedSpritesMethod=null;
	private string saveAtlasDir = "Assets/ResourcesEx/atlas";

	public override void DrawGUI()
	{
		LoadFile();

		int len = m_atlas.Count;
		if (len > 0)
		{
			for (int i = 0; i < len; i++)
			{
				var d = m_atlas[i];
				EditorGUILayout.BeginHorizontal();
				d.atlas=(SpriteAtlas)EditorGUILayout.ObjectField(d.atlas?.name, d.atlas, typeof(SpriteAtlas), true);
				d.isExport=EditorGUILayout.ToggleLeft(d.isExport?"取消":"导出", d.isExport);
				EditorGUILayout.EndHorizontal();
			}
		}

		if (GUILayout.Button("导出Atlas"))   //保存每一个精灵的数据
		{
			for (int i = 0; i < len; i++)
			{
				var ad = m_atlas[i];
				string atlasName = ad.atlas.name;
				int spriteCount = ad.atlas.spriteCount;
				Sprite[] array = new Sprite[spriteCount];
				ad.atlas.GetSprites(array);
				//List<AtlasImageData> arraySpriteData = new List<AtlasImageData>();


				var arraySprite = AccessPackedSprites(ad.atlas);
				foreach (var item in arraySprite)
				{

					var sp = item;
					LogManager.Log(sp.rect, sp.border,sp.name);
					//arraySpriteData.Add(new AtlasImageData()
					//{
					//	name = sp.name,
					//});

				}

				//保存建筑图集数据
				var tex = AccessPackedTextureEditor(ad.atlas);
				if (tex != null)
				{
					byte[] buffer = tex.EncodeToPNG();
					if(buffer!=null)
					File.WriteAllBytes(string.Format("{0}/Garden_{1}.png", saveAtlasDir, atlasName), buffer);
				}

				
			}
			AssetDatabase.Refresh();
		}
	}

	private void LoadFile()
	{
		if (m_atlas.Count == 0)
		{
			if (Directory.Exists(assetPath))
			{
				var array=Directory.GetFiles(assetPath);
				foreach (var item in array)
				{
					if (Path.GetExtension(item) == ".spriteatlas")
					{
						var path = item.Replace(Application.dataPath, "Assets");

						atlas_data ad = new atlas_data();

						ad.atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
						ad.isExport = true;
						m_atlas.Add(ad);
					}
				}	
			}
		}
	}



	//图集转贴图
	public static Texture2D AccessPackedTextureEditor(SpriteAtlas spriteAtlas)
	{
#if EXPOSES_SPRITE_ATLAS_UTILITIES
	UnityEditor.U2D.SpriteAtlasUtility.PackAtlases(new SpriteAtlas[] { spriteAtlas }, EditorUserBuildSettings.activeBuildTarget);
#else
		/*if (PackAtlasesMethod == null) {
			System.Type T = Type.GetType("UnityEditor.U2D.SpriteAtlasUtility,UnityEditor");
			PackAtlasesMethod = T.GetMethod("PackAtlases", BindingFlags.NonPublic | BindingFlags.Static);
		}
		if (PackAtlasesMethod != null) {
			PackAtlasesMethod.Invoke(null, new object[] { new SpriteAtlas[] { spriteAtlas }, EditorUserBuildSettings.activeBuildTarget });
		}*/
#endif
		if (GetPreviewTexturesMethod == null)
		{
			System.Type T = Type.GetType("UnityEditor.U2D.SpriteAtlasExtensions,UnityEditor");
			GetPreviewTexturesMethod = T.GetMethod("GetPreviewTextures", BindingFlags.NonPublic | BindingFlags.Static);
		}
		if (GetPreviewTexturesMethod != null)
		{
			object retval = GetPreviewTexturesMethod.Invoke(null, new object[] { spriteAtlas });
			var textures = retval as Texture2D[];
			if (textures.Length > 0)
				return textures[0];
		}
		return null;
	}

	//图集转精灵集合
	public static Sprite[] AccessPackedSprites(UnityEngine.U2D.SpriteAtlas spriteAtlas)
	{
		Sprite[] sprites = null;
#if UNITY_EDITOR
		if (!Application.isPlaying)
		{

			if (GetPackedSpritesMethod == null)
			{
				System.Type T = Type.GetType("UnityEditor.U2D.SpriteAtlasExtensions,UnityEditor");
				GetPackedSpritesMethod = T.GetMethod("GetPackedSprites", BindingFlags.NonPublic | BindingFlags.Static);
			}
			if (GetPackedSpritesMethod != null)
			{
				object retval = GetPackedSpritesMethod.Invoke(null, new object[] { spriteAtlas });
				var spritesArray = retval as Sprite[];
				if (spritesArray != null && spritesArray.Length > 0)
				{
					sprites = spritesArray;
				}
			}
		}
#endif
		if (sprites == null)
		{
			sprites = new UnityEngine.Sprite[spriteAtlas.spriteCount];
			spriteAtlas.GetSprites(sprites);
			if (sprites.Length == 0)
				return null;
		}
		return sprites;
	}
}
