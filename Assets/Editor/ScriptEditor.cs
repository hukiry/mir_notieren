using Hukiry;
using Hukiry.CurveTexture;
using Hukiry.Editor;
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Sprites;
using UnityEditor.U2D;
using UnityEditor.UI;

using UnityEngine;
using UnityEngine.U2D;

/*
 * Author:Alan
 * Date:2019/05/20 4:00pm
 * Descption:Batch Modification of Audio Files
 * &=Alt
 * %=Ctrl
 * #=Shift
  [MenuItem("Window/OpenAudioClipEditor %#h", false,-1)]
 */

//[RuntimeInitializeOnLoadMethod]
public class ScriptEditor
{

    private const string AssetsMenu = "Assets/Hukiry/Open AudioClip Editor";
	private const string AssetsMenu1 = "Assets/Hukiry/Open Texture Editor";

	#region Window
	//[MenuItem("Hukiry/Window/1，导入地图", false, -13)]
	//[MenuItem("GameObject/导入地图", false, -2)]
	//private static void LoadMapByTxt()
	//{
	//	ScriptableWizard.DisplayWizard<ImportGridMap>("导入地图");
	//}

	//[MenuItem("Hukiry/Window/2，导出地图", false, -12)]
	//[MenuItem("GameObject/导出地图", false, -1)]
	//private static void ExportMapByPrefab()
	//{
	//	ScriptableWizard.DisplayWizard<ExportMapText>("导出地图");
	//}
#if UNITY_IOS || UNITY_IPHONE
	[MenuItem("GameObject/iOS 重新导入SpriteAtlas", false, -1)]
#else
	[MenuItem("GameObject/Android 重新导入SpriteAtlas", false, -1)]
#endif
	public static void ImportSpriteAtlas()
	{
		UGUIImportAtlas.ImportSpriteAtlas();
	}

	[MenuItem(AssetsMenu, false, -11)]
	[MenuItem("Hukiry/Window/1-AudioClip Window %#h", false, -1)]
	private static void OpenAudioEditor()
	{
		AudioWindowEditor.OpenAudioEditor();
	}

	[MenuItem("Hukiry/Window/2-PackMaker Window", false, -1)]
	static public void ShowPackWindow()
	{
		Hukiry.Pack.PackToolWindow.ShowWindow();
	}

	[MenuItem("Hukiry/Window/3-UI View Window", false, -1)]
	static public void ShowUIViewWindow()
	{
		UIViewWindowEditor.ins.OpenWindowEditor("UI视图创建");
	}
	#endregion

	#region 图片部分
	[MenuItem(AssetsMenu1, false, -12)]
	[MenuItem("Hukiry/Texture2D/1，图片格式处理 %#t", false, -45)]
	private static void OpenTextureEditor()
	{
		TextureWindowEditor.OpenTextureEditor();
	}

	[MenuItem("Hukiry/Texture2D/2，图片切割", false, -44)]
	private static void OpenImageCut()
	{
		ImageCutWindowEditor.ins.OpenWindowEditor("贴图切割");
	}

	[MenuItem("Hukiry/Texture2D/3，图片多变形导出", false, -43)]
	private static void OpenImageDrawEditor()
	{
		DrawImageWindow.ins.OpenWindowEditor("多变形导出");
	}

	[MenuItem("Hukiry/Texture2D/4，图片颜色&缩放处理", false, -42)]
	private static void OpenTextureColorEditor()
	{
		ImageWindowEditor.ins.OpenWindowEditor("图片处理");
	}
	#endregion

	#region Unity 编辑工具
	[MenuItem("Hukiry/UnityEditor/Texture Maker &_F1", false, -40)]
	private static void Init()
	{
		TextureMaker.TextureMakerEditor.Init();

    }
	[MenuItem("Hukiry/UnityEditor/Texture Shader", false, -41)]
	private static void OpenCurveToTexture()
	{
		CurveToTexture.ShowCurveToTexture();
	}

	[MenuItem("Hukiry/UnityEditor/Unity Icon Window", false, -42)]
	private static void OpenUnityLookIconWindow()
	{
		UnityLookIconWindow.ShowWindow();
	}

	[MenuItem("Hukiry/UnityEditor/Editor Style Viewer", false, -43)]
	private static void OpenEditorStyleViewer()
	{
		EditorStyleViewer.ShowEditorStyleViewer();
	}
	#endregion

	#region 预制件处理
	[MenuItem("Hukiry/Component/Excel Text Editor  %#m", false, -40)]
	private static void ExcelTextEditor()
	{
		ScriptableWizard.DisplayWizard<ExcelWizardEditor>("Excel字符分割编辑");
	}

	//[MenuItem("Hukiry/Window/Open to Create Font", false, -3)]
	//[MenuItem("Assets/Hukiry/Window/Open to Create FontWindow", false, -3)]
	private static void OpenFontWindowEditor()
	{
		FontWindowEditor.ins.OpenWindowEditor("创建图片字体");
	}

	[MenuItem("Hukiry/Component/创建Unity自定义字体", false, -41)]
	static void CreateBitmapFontGUITools()
	{
		BitmapFontCreateEditorWindow.Open();
	}

	[MenuItem("Hukiry/Component/Prefab Replace Editor  %#F", false, -42)]
	private static void FontReplaceEditor()
	{
		ScriptableWizard.DisplayWizard<FontWizardEditor>("预制件组件替换");
	}

	#endregion

	#region Scene
	//[MenuItem("Hukiry/Scene/Open 副本关卡【编辑】", false, -14)]
	//private static void OpenEditorScane2()
	//{
	//	string path = HukiryUtilEditor.GetAssetPath<SceneAsset>("MapEditor2D");
	//	EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
	//}

	//[MenuItem("Hukiry/Scene/Open 庄园地图【编辑】", false, -15)]
	//private static void OpenEditorScane3()
	//{
	//	string path = HukiryUtilEditor.GetAssetPath<SceneAsset>("MapEditor3D");
	//	EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
	//}

	//[MenuItem("Hukiry/Scene/Open 开始游戏", false, -16)]
	//private static void OpenGameScane()
	//{
	//	string path = HukiryUtilEditor.GetAssetPath<SceneAsset>("main");
	//	EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
	//	EditorApplication.isPlaying = true;
	//}

	//[MenuItem("Hukiry/Scene/Open 建筑位置【编辑】", false, -13)]
	//private static void OpenEditorScane4()
	//{
	//	string path = HukiryUtilEditor.GetAssetPath<SceneAsset>("BuildingSpriteEditor3D");
	//	EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
	//}

	//[MenuItem("GameObject/Preview/查看地图预览格子", false, 1)]
	//private static void AddRuntimePreview()
	//{
	//	var go = GameObject.Find(nameof(Hukiry.Runtime.RuntimePreviewGrid));
	//	if (go == null)
	//	{
	//		go = new GameObject(nameof(Hukiry.Runtime.RuntimePreviewGrid), typeof(Hukiry.Runtime.RuntimePreviewGrid));
	//	}
	//	go.GetComponent<Hukiry.Runtime.RuntimePreviewGrid>().isPreviewGrid = true;
	//}

	//[MenuItem("GameObject/Preview/查看当前物品格子", false, 2)]
	//private static void AddItemPreview()
	//{
	//	var activeTransform = Selection.activeTransform;
	//	if (activeTransform)
	//	{
	//		var go = activeTransform.GetComponent<Hukiry.Runtime.RuntimePreviewPositionIndex>() ?? activeTransform.gameObject.AddComponent<Hukiry.Runtime.RuntimePreviewPositionIndex>();
	//	}
	//}

	//[MenuItem("GameObject/Preview/配置预览物品", false, 3)]
	//private static void ConfigurePreviewItem()
	//{
	//	var go = GameObject.Find(nameof(Hukiry.Runtime.RuntimePreviewPositionIndex));
	//	if (go == null)
	//	{
	//		go = new GameObject(nameof(Hukiry.Runtime.RuntimePreviewPositionIndex), typeof(Hukiry.Runtime.RuntimePreviewPositionIndex));
	//		go.transform.position = new Vector3(0, -1.56F / 4, 0);
	//	}
	//	Selection.activeGameObject = go;
	//}
	#endregion

	#region Tool
	//[MenuItem("Assets/合并游戏/重命名目录下的所有文件")]
	//private static void ReNameFile()
	//{
	//	SceneTextureMenu.ReNameFile();
	//}

	//[MenuItem("Assets/合并游戏/导出精灵")]
	private static void TestSprite()
	{
		UnityEngine.U2D.SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<UnityEngine.U2D.SpriteAtlas>("Assets/Prefabs/Garden/Atlas/decoration.spriteatlas");
		var sp = spriteAtlas.GetSprite("cityhighrail");
		var tex=SpriteUtility.GetSpriteTexture(sp, true);
	}

	//[MenuItem("Hukiry/Tool/Export all file name", false, -1)]
	[MenuItem("Assets/Hukiry/导出目录所有文件名:有后缀", false, -1)]
	private static void ExportFileName()
	{
		HukiryGUIEditor.ExportFileName();
	}

	[MenuItem("Assets/Hukiry/导出目录所有文件名:无后缀", false, -1)]
	private static void ExportFileName2()
	{
		HukiryGUIEditor.ExportFileName(true);
	}

	[MenuItem("Assets/Hukiry/图集：选择目录导出映射", false, -1)]
	private static void ExportFileNameSpriteAtlas()
	{
		HukiryGUIEditor.ExportSpriteAtlasFile();
	}

	[MenuItem("Assets/Hukiry/SetAssetBundleName", false, 1)]
	private static void SetAssetBundleName()
	{
		var ids = Selection.instanceIDs;
		foreach (var item in ids)
		{
			string filePath = AssetDatabase.GetAssetPath(item);
			if (Directory.Exists(filePath))
			{
				var all = Directory.GetFiles(filePath, "*.lua", SearchOption.AllDirectories);
				for (int i = 0; i < all.Length; i++)
				{
					AssetImporter importer = AssetImporter.GetAtPath(all[i]);
					importer.assetBundleName = all[i].Replace("Assets/", "");
					importer.assetBundleVariant = "lua";
					UnityEditor.EditorUtility.DisplayProgressBar("assetBundleName", all[i], (float)i / (float)all.Length);
				}
			}
		}
		UnityEditor.EditorUtility.ClearProgressBar();
		AssetDatabase.Refresh();
	}

	[MenuItem("Tools/Screen TakePhoto", false, 1)]
	private static void TakePotos()
	{
		string dir = Application.dataPath + "/TakePhoto";
		if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
		AssetDatabase.Refresh();

		int count = 1;
		if (Directory.GetFiles(dir) != null)
		{
			count = Directory.GetFiles(dir).Length / 2 + 1;
		}
		
		ScreenCapture.CaptureScreenshot(dir + "/" + count + ".png");
		AssetDatabase.Refresh();
	}

	[MenuItem("Assets/移除字体阴影+碰撞", false, 1)]
	private static void FixComponent()
	{
		Hukiry.HukiryUtilEditor.RepairPrefabAssets<HukirySupperText>(item => {
			item.raycastTarget = false;
			item.SetShadowStyle();
		});
	}

	/// <summary>
	/// 修复预制件资源
	/// </summary>
	/// <param name="extension">扩展名</param>
	public static void RepairPrefabAssets<T>(Action<T> callBack,string extension= "*.prefab") where T:UnityEngine.Object
	{
		if (callBack == null)
		{
			Debug.LogError("回调函数不能为空");
			return;
		}

		Action<string,string> actionCall = (assetPath, ext) => {
			if (Path.GetExtension(assetPath) == ext)
			{
				var go = PrefabUtility.LoadPrefabContents(assetPath);
				var objs = go.GetComponentsInChildren<T>(true);
				foreach (var item in objs)
				{
					callBack(item);
					EditorUtility.SetDirty(item);
				}
				PrefabUtility.SaveAsPrefabAsset(go, assetPath);
			}
		};

		string[] ids = Selection.assetGUIDs;
		int index = 0;
		foreach (string id in ids)
		{
			index++;
			string dirPath = AssetDatabase.GUIDToAssetPath(id);
			if (Directory.Exists(dirPath))
			{
				string[] array = Directory.GetFiles(dirPath, extension, SearchOption.AllDirectories);
				for (int i = 0; i < array.Length; i++)
				{
					var strPath = array[i].Replace('\\', '/').Replace(Application.dataPath, "Assets");
					actionCall(strPath, extension);
					EditorUtility.DisplayProgressBar("移除字体阴影...", strPath, i / (float)array.Length);

				}
			}
			else
			{
				actionCall(dirPath, extension);
				EditorUtility.DisplayProgressBar("移除字体阴影...", dirPath, index / (float)ids.Length);
			}
			EditorUtility.ClearProgressBar();

		}
	}
	#endregion

	#region SpriteAtlas
	[MenuItem("Assets/Packing SpriteAtlas", false, 1)]
	private static void PackingAtlas()
	{
		var ids = Selection.instanceIDs;
		for (int i = 0; i < ids.Length; i++)
		{
			string path = AssetDatabase.GetAssetPath(ids[i]);
			if (Directory.Exists(path))
			{
				string[] assetPaths = Directory.GetFiles(AssetDatabase.GetAssetPath(ids[i]), "*.spriteatlas", SearchOption.AllDirectories);
				for (int j = 0; j < assetPaths.Length; j++)
				{
					var spriteAtlas = AssetDatabase.LoadAssetAtPath<UnityEngine.U2D.SpriteAtlas>(assetPaths[j]);
					Hukiry.HukiryUtilEditor.PackingAtlas(spriteAtlas);
					EditorUtility.DisplayProgressBar("Packing SpriteAtlas", j.ToString(), j / (float)assetPaths.Length);
				}
			}
			else
			{
				Hukiry.HukiryUtilEditor.PackingAtlas(ids[i]);
				EditorUtility.DisplayProgressBar("Packing SpriteAtlas", i.ToString(), i / (float)ids.Length);
			}
		}
		EditorUtility.ClearProgressBar();
	}


	//[MenuItem("Assets/Export Texture2D Of SpriteAtlas", false, 2)]
	private static void ExportSpriteAtlasTexture2D()
	{
        //var ids = Selection.instanceIDs;
        //foreach (var item in ids)
        //{
        //    string assetPath = AssetDatabase.GetAssetPath(item);
        //    if (Path.GetExtension(assetPath) == ".spriteatlas")
        //    {
        //        var spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);
        //        string path = GeneratePngFromSpriteAtlas(spriteAtlas);
        //        Texture2DExportEditor.ExportSpriteAtlas(spriteAtlas, path);
        //        break;
        //    }
        //}
    }

	//[MenuItem("Assets/Split Texture to Sprite", false, 2)]
	private static void SplitTextureToMulSprite()
	{
		//string texturePath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
		//string assetPath = Path.ChangeExtension(texturePath, ".asset");
		//if (File.Exists(texturePath) && File.Exists(assetPath) && Path.GetExtension(texturePath) == ".png")
		//{
		//	var atlasAsset = AssetDatabase.LoadAssetAtPath<SpriteAtlasAsset>(assetPath);
		//	Texture2DExportEditor.SplitTexture(texturePath, atlasAsset);
		//}
		//else
		//{
		//	LogManager.LogError("路径错误：", texturePath);
		//}
	}


	private static string GeneratePngFromSpriteAtlas(SpriteAtlas spriteAtlas)
	{
		string texturePath = Path.ChangeExtension(AssetDatabase.GetAssetPath(spriteAtlas), ".png");
		if (spriteAtlas == null)
			return null;

		Texture2D[] tempTexture = AccessPackedTextureEditor(spriteAtlas);
		if (tempTexture == null)
			return null;

		byte[] bytes = null;
		for (int i = 0; i < tempTexture.Length; i++)
		{
			try
			{
				bytes = tempTexture[i].EncodeToPNG();
			}
			catch (Exception)
			{
				// handled below
			}
			if (bytes == null || bytes.Length == 0)
			{
				LogManager.LogColor("red", "不能读取压缩过的（SpriteAtlas）图集，需要启动可读可写并在Inspector里确保压缩设置成 None", "Could not read Compressed SpriteAtlas. Please enable 'Read/Write Enabled' and ensure 'Compression' is set to 'None' in Inspector.",i, spriteAtlas);
				continue;
			}
			if (i > 0)
			{
				File.WriteAllBytes(texturePath.Replace(".png", i + ".png"), bytes);
			}
			else
			{
				File.WriteAllBytes(texturePath, bytes);
			}
		}
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		return texturePath;
	}
	private static Texture2D[] AccessPackedTextureEditor(SpriteAtlas spriteAtlas)
	{
		SpriteAtlasUtility.PackAtlases(new SpriteAtlas[] { spriteAtlas }, EditorUserBuildSettings.activeBuildTarget);
		Type T = Type.GetType("UnityEditor.U2D.SpriteAtlasExtensions,UnityEditor");
		MethodInfo GetPreviewTexturesMethod = T.GetMethod("GetPreviewTextures", BindingFlags.NonPublic | BindingFlags.Static);
		if (GetPreviewTexturesMethod != null)
		{
			object retval = GetPreviewTexturesMethod.Invoke(null, new object[] { spriteAtlas });
			var textures = retval as Texture2D[];
			if (textures.Length > 0)
				return textures;
		}
		return null;
	} 
	#endregion
}

