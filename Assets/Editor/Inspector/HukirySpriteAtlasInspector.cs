
using Hukiry.UI;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

[CustomEditor(typeof(SpriteAtlas))]
[CanEditMultipleObjects]
internal sealed class HukirySpriteAtlasInspector : Editor, IHasCustomMenu
{
	private	Editor editor;
	private Editor editorSpriteAtlas;
	private Hukiry.PackageSpriteAtlasManger atlasManger;
    private static Material s_PreviewSpriteDefaultMaterial;

	private Texture2D[] texArray;
    private string spriteName;
	SpritePreview preview = new SpritePreview();
	private string projectPath => Application.dataPath.Replace("Assets", "");
	private GUIContent disabledPackLabel => EditorGUIUtility.TrTextContent("UV数据导出时，需要开启【SpritePacker】模式", null, Hukiry.HukiryUtilEditor.GetTexture2D("console.warnicon"));
	private void OnEnable()
	{
		editor = Editor.CreateEditor(target, Type.GetType("UnityEditor.U2D.SpriteAtlasInspector, UnityEditor"));
		Hukiry.HukiryUtilEditor.InvokeInstance(editor, nameof(OnEnable));
		if (EditorApplication.isCompiling) return;
		atlasManger = Hukiry.HukiryUtilEditor.FindAssetObject<Hukiry.PackageSpriteAtlasManger>("PackageSpriteAtlasMangerAsset");
		editorSpriteAtlas = Editor.CreateEditor(atlasManger);
		Hukiry.HukiryUtilEditor.InvokeInstance(editorSpriteAtlas, nameof(OnEnable));

		spriteName = AtlasImageAssetSetting.Instance.lastSpriteName;

	}

	public override void OnInspectorGUI()
	{
		editor?.OnInspectorGUI();

		if (EditorApplication.isCompiling) return;
		EditorGUILayout.Space();
		if (Hukiry.UI.AtlasImageUtility.DrawHeader("当前图集"))
		{
			if (!EditorApplication.isPlaying)
			{
				if(GUILayout.Button(disabledPackLabel, EditorStyles.helpBox))
				{
					SettingsService.OpenProjectSettings("Project/Editor");
				}
			}

			using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
			{
				var icon = EditorApplication.isPlaying ? "d_PauseButton" : "d_PlayButton";
				if (GUILayout.Button(Hukiry.HukiryUtilEditor.GetTexture2D(icon), GUILayout.Height(30), GUILayout.Width(30)))
				{
					EditorApplication.isPlaying = !EditorApplication.isPlaying;
				}

				if (GUILayout.Button("Save as UV", GUILayout.Width(100), GUILayout.Height(30)))
				{
					this.ExportUVData(target as SpriteAtlas, false);
				}
				
				using (new EditorGUILayout.VerticalScope())
				{
					if (Hukiry.UI.AtlasImageUtility.DrawHeader("Extand UV Dir to Save"))
					{
						EditorGUILayout.LabelField("UV数据目录");
						using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
						{
							EditorGUILayout.TextField(atlasManger.uvDataDirPath);
							if (GUILayout.Button("Browse", GUILayout.Width(100)))
							{
								string folder = Application.dataPath;
								if (!string.IsNullOrEmpty(atlasManger.uvDataDirPath))
								{
									folder = atlasManger.uvDataDirPath.Replace(projectPath, "");
								}

								string dirPath = EditorUtility.OpenFolderPanel("Unity", folder, "");
								if (!string.IsNullOrEmpty(dirPath))
								{
									atlasManger.uvDataDirPath = dirPath.Replace(projectPath, "");
								}
								else
								{
									atlasManger.uvDataDirPath = folder;
								}
							}
						}

						if (!string.IsNullOrEmpty(atlasManger.uvDataDirPath) && System.IO.Directory.Exists(atlasManger.uvDataDirPath) )
						{
							if (GUILayout.Button("Save"))
							{
								this.ExportUVData(target as SpriteAtlas, true, $"{atlasManger.uvDataDirPath}/{target.name}.json");
							}
						}
					}
				}
			}

			using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
			{
				if (texArray != null && texArray.Length > 0)
				{
					for (int i = 0; i < texArray.Length; i++)
					{
						var tex = texArray[i];
						GUILayout.Label(tex, GUILayout.Height(40), GUILayout.Width(tex.width / (float)tex.height * 40));
					}
				}
				else
				{
					texArray = Hukiry.HukiryUtilEditor.SpriteAtlasToTexture(target as SpriteAtlas);
				}
			}
		}

		EditorGUILayout.Space();
		if (Hukiry.UI.AtlasImageUtility.DrawHeader("自定义设置"))
		{
			editorSpriteAtlas?.OnInspectorGUI();
		}


		DrawBorder(target as SpriteAtlas, this.spriteName);
	}

	private void DrawBorder(SpriteAtlas target, string spriteName)
	{
		preview.sprite = GetOriginalSprite(target, spriteName);
		preview.color = Color.white;

		bool isExpand = AtlasImageUtility.DrawBorder(preview, () =>
		{
			AtlasSpriteSelector.Show(target, spriteName, (selectedSpriteName) =>
			{
				if (selectedSpriteName == null)
					return;
				this.spriteName = selectedSpriteName;
				AtlasImageAssetSetting.Instance.lastSpriteName = selectedSpriteName;
				this.Repaint();
			});
		}, sName => {
			this.spriteName = sName;
			AtlasImageAssetSetting.Instance.lastSpriteName = sName;
		}, $"Sprite Border [{target.name}]");

		if (isExpand)
		{

			var rect = GUILayoutUtility.GetLastRect();
			rect.Set(rect.x, rect.y + 100, 150, 150);
			rect.x = (Hukiry.HukiryUtilEditor.InspectorWindow.position.width - rect.width) / 2;
			//AtlasSpriteSelector.DrawTiledTexture(rect);
			preview.OnPreviewGUI(rect, false);
			EditorGUILayout.Space(rect.height + 30);
		}
	}

	static Sprite GetOriginalSprite(SpriteAtlas atlas, string name)
	{
		if (!atlas || string.IsNullOrEmpty(name))
		{
			return null;
		}

		SerializedProperty spPackedSprites = new SerializedObject(atlas).FindProperty("m_PackedSprites");
		return Enumerable.Range(0, spPackedSprites.arraySize)
			.Select(index => spPackedSprites.GetArrayElementAtIndex(index).objectReferenceValue)
			.OfType<Sprite>()
			.FirstOrDefault(s => s.name == name);
	}


	private void ExportUVData(SpriteAtlas spriteAtlas, bool isCustom, string filePath=null)
	{
		string texturePath = Path.ChangeExtension(AssetDatabase.GetAssetPath(spriteAtlas), ".png");
		if (isCustom)
			this.ExportTexture2D(spriteAtlas, filePath.Replace(".json", ".png"));
		else
			this.ExportTexture2D(spriteAtlas, texturePath);


		if (Hukiry.HukiryUtilEditor.IsEnablePacker && EditorApplication.isPlaying)
		{
			if (isCustom)
				Texture2DExportEditor.ExportCustomSpriteAtlasUV(spriteAtlas, filePath);
			else
				Texture2DExportEditor.ExportCurrentSpriteAtlasUV(spriteAtlas);
		}
		else
		{
			string contentError = Hukiry.HukiryUtilEditor.IsEnablePacker ? "You must enable Sprite Packer and Editor is currently in play mode" :
				"Editor is currently in play mode";
			LogManager.LogColor("red", contentError);
		}
		AssetDatabase.Refresh();
	}

	private void ExportTexture2D(SpriteAtlas spriteAtlas, string texturePath)
	{
		var texture2D = Hukiry.HukiryUtilEditor.SpriteAtlasToStaticTexture(spriteAtlas); 
        if (texture2D != null)
        {
            if (texture2D.isReadable)
				File.WriteAllBytes(texturePath, texture2D.EncodeToPNG());
			else
				LogManager.LogColor("red", "You can make the texture readable in the Texture Import Settings.");
        }
    }

	//返回预览标题
	public override string GetInfoString()
	{
	
		return editor?.GetInfoString();
	}

	//是否预览
    public override bool HasPreviewGUI()
    {
		if (editor == null) return false;
		return editor.HasPreviewGUI();
	}

	//预览设置
    public override void OnPreviewSettings()
	{
		editor?.OnPreviewSettings();
	}

	//预览GUI
	public override void OnPreviewGUI(Rect r, GUIStyle background)
	{
		editor?.OnPreviewGUI(r,background);
	}

	//预览图片
	public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
		return editor?.RenderStaticPreview(assetPath, subAssets,width,height);
	}




	///////////////////////////////////////////////////////////////Sprite////////////////////////////////////////////////////////////////////////

	#region 渲染精灵部分

	private void PackageButton()
	{
		if (!Hukiry.HukiryUtilEditor.IsEnablePacker)
		{
			if (GUILayout.Button(EditorGUIUtility.TrTextContent($"{target.name} Pack Preview", "Pack this atlas."), GUILayout.ExpandWidth(expand: false)))
			{
				SpriteAtlas[] array = new SpriteAtlas[] { target as SpriteAtlas };
				SpriteAtlasUtility.PackAtlases(array, EditorUserBuildSettings.activeBuildTarget);
				Hukiry.HukiryUtilEditor.InvokeInstance(editor, "SyncPlatformSettings");
				GUIUtility.ExitGUI();
			}

			EditorGUILayout.Space();
			Hukiry.HukiryUtilEditor.DrawLine(Color.green, Color.gray, 4, 4);
			EditorGUILayout.Space();

		}
	}
	private static Texture2D CustomRenderStaticPreview(Sprite sprite, Color color, int width, int height, Matrix4x4 transform)
    {
        if (sprite == null)
        {
            return null;
        }
        AdjustWidthAndHeightForStaticPreview((int)sprite.rect.width, (int)sprite.rect.height, ref width, ref height);
        var savedRenderTargetState = CreateInstanceUnityEditor("SavedRenderTargetState");
        RenderTexture temp = (RenderTexture.active = RenderTexture.GetTemporary(width, height, 0, SystemInfo.GetGraphicsFormat(0)));
        GL.Clear(clearDepth: true, clearColor: true, new Color(0f, 0f, 0f, 0.1f));
        previewSpriteDefaultMaterial.mainTexture = sprite.texture;
        previewSpriteDefaultMaterial.SetPass(0);
        RenderSpriteImmediate(sprite, color, transform);
        Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: false);
        texture2D.hideFlags = HideFlags.HideAndDontSave;
        texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
        texture2D.Apply();
        RenderTexture.ReleaseTemporary(temp);
        Hukiry.HukiryUtilEditor.InvokeInstance(savedRenderTargetState, "Restore");
        return texture2D;
    }

    private static object CreateInstanceUnityEditor(string className, params object[] objs)
    {
        string classPath = $"UnityEditor.{className},UnityEditor";
        var type = Type.GetType(classPath, false);
        return Activator.CreateInstance(type, objs);
    }


    private static void AdjustWidthAndHeightForStaticPreview(int textureWidth, int textureHeight, ref int width, ref int height)
    {
        int max = width;
        int max2 = height;
        if (textureWidth <= width && textureHeight <= height)
        {
            width = textureWidth;
            height = textureHeight;
        }
        else
        {
            float b = (float)height / (float)textureWidth;
            float a = (float)width / (float)textureHeight;
            float num = Mathf.Min(a, b);
            width = Mathf.RoundToInt((float)textureWidth * num);
            height = Mathf.RoundToInt((float)textureHeight * num);
        }
        width = Mathf.Clamp(width, 2, max);
        height = Mathf.Clamp(height, 2, max2);
    }
    private static Material previewSpriteDefaultMaterial
    {
        get
        {
            if (s_PreviewSpriteDefaultMaterial == null)
            {
                Shader shader = Shader.Find("Sprites/Default");
                s_PreviewSpriteDefaultMaterial = new Material(shader);
            }
            return s_PreviewSpriteDefaultMaterial;
        }
    }

    private static void RenderSpriteImmediate(Sprite sprite, Color color, Matrix4x4 transform)
    {
        float width = sprite.rect.width;
        float height = sprite.rect.height;
        float num = sprite.rect.width / sprite.bounds.size.x;
        Vector2[] vertices = sprite.vertices;
        Vector2[] uv = sprite.uv;
        ushort[] triangles = sprite.triangles;
        Vector2 pivot = sprite.pivot;
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(4);
        for (int i = 0; i < sprite.triangles.Length; i++)
        {
            ushort num2 = triangles[i];
            Vector2 vector = vertices[num2];
            Vector2 vector2 = uv[num2];
            Vector3 point = new Vector3(vector.x, vector.y, 0f);
            point = transform.MultiplyPoint(point);
            point.x = (point.x * num + pivot.x) / width;
            point.y = (point.y * num + pivot.y) / height;
            GL.Color(color);
            GL.TexCoord(new Vector3(vector2.x, vector2.y, 0f));
            GL.Vertex3(point.x, point.y, point.z);
        }
        GL.End();
        GL.PopMatrix();
    }

	public void AddItemsToMenu(GenericMenu menu)
	{
		menu.AddItem(new GUIContent("Location Script"), false, () => {
			Hukiry.HukiryUtilEditor.LocationObject<MonoScript>("HukirySpriteAtlasInspector");
		});
	}

	#endregion

}
