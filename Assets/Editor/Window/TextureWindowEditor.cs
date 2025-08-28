using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class TextureWindowEditor : ResourcesWindowEditor<TextureWindowEditor>
{
	public override bool IsCloseWindow { get { return false; } }
	public enum FileType
	{
#if NGUI
        PrefabFixMaterial,//预制件材质修复
        TextureFormatChange  
#endif
		TextureTypeChange,//图片修改
	}

	public enum FileImageSaveType
	{
		Jpg,
		Png
	}

	public class TextureData : BaseData
	{
		public FileType fileType;
		[FieldName("贴图类型：")]
		public TextureImporterType textureImporterType;
		//public FileImageSaveType fileImageSaveType;
		[FieldName("图片压缩格式：")]
		public TextureImporterFormat textureFormat = TextureImporterFormat.ARGB16;
		[FieldName("打包的图集名：")]
		public string spritePackingTag;
	}
	public class FileExt
	{
		public string ext;
		private bool _isFilter;
		private static bool isChange;
		public static Action callCollectFile = null;

		public bool isOpen
		{
			get { return _isFilter; }
			set
			{
				if (_isFilter != value)
				{
					isChange = true;
				}
				_isFilter = value;
			}
		}
		public static void OnCheckChange()
		{
			if (isChange)
			{
				isChange = false;
				if (callCollectFile != null)
				{
					callCollectFile();
				}
			}
		}

		public FileExt(string v1, bool v2)
		{
			this.ext = v1;
			this.isOpen = v2;
		}
		public static bool IsContainUnityExt(string ext)
		{
			return ".tga" == ext || ".jpg" == ext || ".png" == ext || ".dds" == ext || ".bmp" == ext
				|| ".psd" == ext || ".gif" == ext;
		}
	}

	private TextureData textureData;
	//[MenuItem("Lsj/Hukiry_Unity Upgrade/Reover Tga Texture", false)]
	//[MenuItem("Assets/Hukiry_Unity Upgrade/Reover Tga Texture", false, 1)]
	private static void ReleaseAtlas()
	{
		string dir = Application.persistentDataPath + "/Hukiry_Unity Upgrade";
		string texDir = Path.Combine(ATLAS_ROOT, "MaterialAndTexture");
		if (Directory.Exists(dir))
		{
			string[] sf = Directory.GetFiles(dir);
			int length = sf.Length;
			for (int i = 0; i < length; i++)
			{
				File.Copy(sf[i], texDir + "/" + Path.GetFileName(sf[i]));
			}

			sf = Directory.GetFiles(dir, "*.png");
			length = sf.Length;
			for (int i = 0; i < length; i++)
			{
				File.Delete(sf[i]);
			}
		}
		AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
		//_Instance.ImportTextureData(".tga");

	}
	//[MenuItem("Lsj/Hukiry_Unity Upgrade/Open Reover Folder", false)]
	//[MenuItem("Assets/Hukiry_Unity Upgrade/Open Reover Folder", false, 2)]
	private static void OpenReoverFolder()
	{
		Application.OpenURL(Application.persistentDataPath + "/Hukiry_Unity Upgrade");
	}

	public override void EndSave()
	{
#if NGUI
        if (textureData.fileType == FileType.PrefabFixMaterial)
        {
            //ImportTextureData(".png");
        }
#endif
	}
#if NGUI
    private void ImportTextureData(string extFile)
    {
        var texDir = Path.Combine(ATLAS_ROOT, "MaterialAndTexture");
        try
        {
            EditorUtility.ClearProgressBar();
            // 看看是否有新建图集
            int progressCount = 0;

            var files = Directory.GetFiles(texDir, "*.txt", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                string msg = string.Format("正在检测Prefab:{0}", file);
                EditorUtility.DisplayProgressBar("检测TP中...", msg, progressCount / (float)files.Length);
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);

                var matFileName = string.Format("{0}.mat", fileNameWithoutExt);
                var matPath = Path.Combine(texDir, matFileName);
                Material mat = null;
                if (!File.Exists(matPath))
                {
                    // If the material doesn't exist, create it
                    Shader shader = Shader.Find("Unlit/Transparent Colored");
                    mat = new Material(shader);

                    // Save the material
                    AssetDatabase.CreateAsset(mat, matPath);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

                    string texturePath = matFileName.Replace(".mat", extFile);
                    mat.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture));
                }

                var prefabFileName = string.Format("{0}.prefab", fileNameWithoutExt);
                var atlasPath = Path.Combine(ATLAS_ROOT, prefabFileName);
                if (!File.Exists(atlasPath))
                {
                    bool success = false;
                    var go = new GameObject(fileNameWithoutExt);
                    var atlas = go.AddComponent<UIAtlas>();
                    atlas.spriteMaterial = mat;

                    PrefabUtility.SaveAsPrefabAsset(go, atlasPath, out success);
                    if (success)
                    {
                        GameObject.DestroyImmediate(go);
                    }
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
                ++progressCount;
            }

            AssetDatabase.SaveAssets();

            // 重新给图集赋值
            progressCount = 0;
            files = Directory.GetFiles(ATLAS_ROOT, "*.prefab", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                string msg = string.Format("正在导入:{0}", file);
                EditorUtility.DisplayProgressBar("导入TP中...", msg, progressCount / (float)files.Length);

                EditorHelper.LoadAndProcessPrefab(file, (go, customData) =>
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    var txtFileName = string.Format("{0}.txt", fileName);
                    var txtFilePath = Path.Combine(texDir, txtFileName);
                    if (!File.Exists(txtFilePath))
                        return false;

                    var atlas = go.GetComponent<UIAtlas>();
                    if (atlas == null)
                        return false;

                    var meterialPath = Path.Combine(texDir, string.Format("{0}.mat", fileName));
                    var mat = atlas.spriteMaterial;
                    if (mat == null || !AssetDatabase.GetAssetPath(mat).Replace('\\', '/').Equals(meterialPath))
                    {
                        mat = (Material)AssetDatabase.LoadAssetAtPath(meterialPath, typeof(Material));
                        if (mat != null)
                        {
                            atlas.spriteMaterial = mat;
                        }
                    }

                    if (mat != null)
                    {
                        var texturePath = Path.Combine(texDir, string.Format("{0}" + extFile, fileName));
                        var tex = mat.mainTexture;
                        if (tex == null || !AssetDatabase.GetAssetPath(tex).Replace('\\', '/').Equals(texturePath))
                        {
                            tex = (Texture)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture));
                            if (tex != null)
                            {
                                mat.mainTexture = tex;
                            }
                            else
                            {
                                throw new System.Exception(string.Format("文理不存在:{0}", texturePath));
                            }
                        }

                        var texImporter = (TextureImporter)AssetImporter.GetAtPath(texturePath);
                        var texSetting = new TextureImporterSettings();
                        texImporter.ReadTextureSettings(texSetting);
                        if (texSetting.maxTextureSize < 2048)
                        {
                            texSetting.maxTextureSize = 2048;
                            texImporter.SetTextureSettings(texSetting);
                            AssetDatabase.ImportAsset(texturePath);
                        }
                    }

                    string data = File.ReadAllText(txtFilePath);
                    NGUIJson.LoadSpriteData(atlas, data);

                    return true;
                });
                ++progressCount;
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();

            AssetDatabase.Refresh();
        }

    }
    private string TextureFormatChange(TextureData data, string filePath)
    {
        string tagFile = filePath.Split('.')[0] + ".tga";
        string pngFile = filePath.Split('.')[0] + ".png";
        string jpgFile = filePath.Split('.')[0] + ".jpg";
        bool isTga = File.Exists(tagFile);
        bool isPng = File.Exists(pngFile);
        bool isJpg = File.Exists(jpgFile);
        if (isTga)
        {
            if (File.Exists(pngFile))
                File.Delete(pngFile);

            if (File.Exists(jpgFile))
                File.Delete(jpgFile);

            isPng = isJpg = false;
            filePath = tagFile;
        }

        //NGUIEditorTools.MakeTextureReadable(filePath, false);
        Texture2D texture2D = AssetDatabase.LoadAssetAtPath(filePath, typeof(Texture2D)) as Texture2D;
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        try
        {

            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            if (data.fileImageSaveType == FileImageSaveType.Png)
            {
                if (!isPng && texture2D != null)
                {
                    File.WriteAllBytes(pngFile, texture2D.EncodeToPNG());
                }

                if (File.Exists(jpgFile))
                {
                    File.Delete(jpgFile);
                }
            }
            else
            {
                if (!isJpg && texture2D != null)
                {
                    File.WriteAllBytes(jpgFile, texture2D.EncodeToJPG());
                }

                if (File.Exists(pngFile))
                {
                    File.Delete(pngFile);
                }
            }

            if (isTga)
            {
                string dir = Application.persistentDataPath + "/Hukiry_Unity Upgrade";
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string fileName = Path.GetFileName(tagFile);
                Log(dir);

                if (File.Exists(dir + "/" + fileName))
                    File.Delete(dir + "/" + fileName);

                File.Move(tagFile, dir + "/" + fileName);
            }

        }
        catch (System.Exception ex)
        {
            LogError("此文件不是规范的Image:" + ex.ToString());
        }
        Log(filePath);
        return AssetDatabase.GetAssetPath(texture2D.GetInstanceID());
    }
#endif
	private string TextureTypeChange(TextureData data, string filePath)
	{
		TextureImporter textureImporter = AssetImporter.GetAtPath(filePath) as TextureImporter;
		TextureImporterPlatformSettings setting = textureImporter.GetDefaultPlatformTextureSettings();
		textureImporter.textureType = data.textureImporterType;
		setting.format = data.textureFormat;
		if (data.textureImporterType == TextureImporterType.Sprite)
		{
			textureImporter.mipmapEnabled = false;
			textureImporter.alphaIsTransparency = true;
			textureImporter.sRGBTexture = false;
			textureImporter.alphaSource = TextureImporterAlphaSource.None;
			textureImporter.mipmapEnabled = false;
			textureImporter.spritePackingTag = data.spritePackingTag;
		}
		textureImporter.SetPlatformTextureSettings(setting);
		Log(filePath);
		return textureImporter.assetPath;
	}

	private string PrefabFixMaterial(string filePath)
	{
		string dir = Path.GetDirectoryName(filePath);
		dir = dir.Substring(0, dir.LastIndexOf("\\")) + "\\" + Path.GetFileNameWithoutExtension(filePath) + ".prefab";
		Material mat = AssetDatabase.LoadAssetAtPath(filePath, typeof(Material)) as Material;
#if NGUI
        UIAtlas atlas = AssetDatabase.LoadAssetAtPath(dir, typeof(UIAtlas)) as UIAtlas;

        if (atlas != null)
            atlas.spriteMaterial = mat;

        Texture2D textureImporter = AssetDatabase.LoadAssetAtPath(filePath.Split('.')[0] + ".png", typeof(Texture2D)) as Texture2D;
        if (textureImporter == null)
        {
            textureImporter = AssetDatabase.LoadAssetAtPath(filePath.Split('.')[0] + ".tga", typeof(Texture2D)) as Texture2D;
        }
        mat.SetTexture("_MainTex", textureImporter);
#endif

		Log(filePath);
		return AssetDatabase.GetAssetPath(mat.GetInstanceID());
	}

	private static List<FileExt> extList = new List<FileExt>();

	public static void OpenTextureEditor()
	{
		TextureData tex = new TextureData();
		tex.textureFormat = TextureImporterFormat.DXT1;
#if NGUI
        tex.textureImporterType = TextureImporterType.Default;
#else
		tex.textureImporterType = TextureImporterType.Sprite;
#endif

		_Instance.OpenWindowEditor<TextureData>(Selection.assetGUIDs, tex);
		FileExt.callCollectFile = _Instance.ReCollectFile;
		_Instance.ReInitFileFilterCall = (isChange) =>
		{
			if (isChange)
			{
				extList.Clear();
			}
		};

		string filePath = _Instance.GetEditorBaseData()?.filePath;
		if (!string.IsNullOrEmpty(filePath))
		{
			TextureImporter textureImporter = AssetImporter.GetAtPath(filePath) as TextureImporter;
			TextureImporterPlatformSettings setting = textureImporter.GetDefaultPlatformTextureSettings();
			tex.textureFormat = setting.format;
			tex.textureImporterType = textureImporter.textureType;
			tex.spritePackingTag = textureImporter.spritePackingTag;
			_Instance.SetBaseData(tex);
		}
	}

	public override string DrawGUI(BaseData data)
	{
		textureData = data as TextureData;
		GUILayout.Space(10);
		GUILayout.BeginHorizontal();
		GUILayout.Label("图片格式过滤:");
		if (extList.Count > 0)
		{
			for (int i = 0; i < extList.Count; i++)
			{
				FileExt fileExt = extList[i];
				fileExt.isOpen = EditorGUILayout.ToggleLeft(new GUIContent(fileExt.ext), fileExt.isOpen);
			}
		}
		FileExt.OnCheckChange();
		GUILayout.EndHorizontal();



		//textureData.fileType = (FileType)EditorGUILayout.EnumPopup("图片优化操作：", textureData.fileType);
		string btnName = "保存修改";

		//textureData = ExtendMothedClass.DrawType(textureData);

		//        switch (textureData.fileType)
		//        {
		//#if NGUI
		//            case FileType.PrefabFixMaterial:
		//                btnName = "修复预制件中的材质球";
		//                break;
		//#endif
		//            case FileType.TextureTypeChange:
		//                textureData.textureImporterType = (TextureImporterType)EditorGUILayout.EnumPopup("转换的贴图压缩类型：", textureData.textureImporterType);
		//                textureData.textureFormat = (TextureImporterFormat)EditorGUILayout.EnumPopup("ImporterFormat：", textureData.textureFormat);
		//                btnName = "贴图类型转换";
		//                break;
		//            case FileType.TextureFormatChange:
		//                textureData.fileImageSaveType = (FileImageSaveType)EditorGUILayout.EnumPopup("图片保存格式：", textureData.fileImageSaveType);
		//                btnName = "图片压缩格式转换";
		//                break;
		//        }
		return btnName;
	}

	public override string WindowTitileName()
	{
		return "图片";
	}

	public override bool FileFilterExtension(string extName)
	{
		if (FileExt.IsContainUnityExt(extName))
		{
			if (extList.Count == 0 || extList.Find(p => p.ext == extName) == null)
			{
				extList.Add(new FileExt(extName, true));
			}
		}

		foreach (var ext in extList)
		{
			if (ext.ext == extName && ext.isOpen)
			{
				return true;
			}
		}
		return false;
	}

	public override string ExcuteData(BaseData data, EditorBaseData filePath)
	{
		var textureData = data as TextureData;
		bool isMat = Path.GetExtension(filePath.filePath).ToLower() == ".mat";

		string assetPath = "";
		switch (textureData.fileType)
		{
#if NGUI
            case FileType.PrefabFixMaterial:
                if (isMat)
                {
                    assetPath = this.PrefabFixMaterial(filePath.filePath);
                }
                break;
#endif
			case FileType.TextureTypeChange:
				if (!isMat)
				{

					assetPath = this.TextureTypeChange(textureData, filePath.filePath);

				}
				break;
				//case FileType.TextureFormatChange:
				//    if (!isMat)
				//    {
				//        assetPath = this.TextureFormatChange(textureData, filePath.filePath);
				//    }
				//    break;
		}
		Log(filePath.filePath);
		return assetPath;
	}
}
