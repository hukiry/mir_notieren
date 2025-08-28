
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace Hukiry
{
    /// <summary>
    /// 仅编辑器使用
    /// </summary>
    public static class HukiryUtilEditor
	{
		/// <summary>
		/// 查找脚本及资源路径
		/// </summary>
		/// <typeparam name="T">资源类型</typeparam>
		/// <param name="fileName">文件名，无后缀</param>
		/// <returns></returns>
		public static string FindAssetPath<T>(string fileName) where T : UnityEngine.Object
		{
			var assetPathArray = AssetDatabase.FindAssets($"t:{typeof(T).Name}").Select(x => AssetDatabase.GUIDToAssetPath(x));
			foreach (var path in assetPathArray)
			{
				var assetName = Path.GetFileNameWithoutExtension(path);
				if (assetName == fileName)
				{
					return path;
				}
			}
			return string.Empty;
		}

		/// <summary>
		/// 查找脚本及资源对象
		/// </summary>
		/// <param name="fileName">文件名，无后缀</param>
		public static T FindAssetObject<T>(string fileName) where T : UnityEngine.Object
		{
			var assetPathArray = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(T).Name}").Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x));
			foreach (var path in assetPathArray)
			{
				var assetName = Path.GetFileNameWithoutExtension(path);
				if (assetName == fileName)
				{
					return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
				}
			}
			return default;
		}

		/// <summary>
		/// 根据精灵名 查找图集
		/// </summary>
		public static SpriteAtlas FindSpriteAtlas(string spriteName)
		{
			foreach (string path in AssetDatabase.FindAssets("t:" + typeof(SpriteAtlas).Name).Select(x => AssetDatabase.GUIDToAssetPath(x)))
			{
				SpriteAtlas spriteAtla = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
				if (spriteAtla.GetSprite(spriteName) != null)
				{
					return spriteAtla;
				}
			}
			return null;
		}

		/// <summary>
		/// 根据精灵对象， 查找图集
		/// </summary>
		public static SpriteAtlas FindSpriteAtlas(Sprite sprite)
		{
			foreach (string path in AssetDatabase.FindAssets("t:" + typeof(SpriteAtlas).Name).Select(x => AssetDatabase.GUIDToAssetPath(x)))
			{
				SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
				var findSprite = spriteAtlas.GetSprite(sprite.name);
				if (findSprite && findSprite.rect == sprite.rect && findSprite.border == sprite.border)
				{
					return spriteAtlas;
				}
			}
			return null;
		}

		public static void IfNotDirCreate(string dirPath)
		{
			if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
		}

		/// <summary>
		/// 根据资源类型查找所有路径
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static List<string> GetAssetsPath<T>() where T : UnityEngine.Object
		{
			List<string> temp = new List<string>();
			var assetPathArray = AssetDatabase.FindAssets($"t:{typeof(T).Name}").Select(x => AssetDatabase.GUIDToAssetPath(x));
			foreach (var path in assetPathArray)
			{
				temp.Add(path);
			}
			return temp;
		}


		/// <summary>
		/// 根据资源类型查找所有路径
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static List<T> GetAssetObjects<T>() where T : UnityEngine.Object
		{
			List<T> temp = new List<T>();
			var assetPathArray = AssetDatabase.FindAssets($"t:{typeof(T).Name}").Select(x => AssetDatabase.GUIDToAssetPath(x));
			foreach (var path in assetPathArray)
			{
				temp.Add(AssetDatabase.LoadAssetAtPath<T>(path));
			}
			return temp;
		}

		/// <summary>
		/// 清理控制台
		/// </summary>
		public static void ClearUnityConsole()
		{
			//Clear控制台
			var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
			var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
			clearMethod.Invoke(null, null);
		}

		#region Reflection Call

		public static object CreateInstanceUnityEditor(string className, params object[] objParams)
		{
			var type = GetUnityEditor(className);
			return Activator.CreateInstance(type, objParams);
		}

		public static Type GetUnityEditor(string className)
		{
			var type = Type.GetType($"UnityEditor.{className},UnityEditor", false);
			if (type == null)
			{
				type = Type.GetType($"UnityEditor.U2D.{className},UnityEditor", false);
			}
			return type;
		}

		/// <summary>
		/// 类实例方法
		/// </summary>
		/// <typeparam name="T">任意类</typeparam>
		/// <param name="methodName">类方法名</param>
		/// <param name="objs">参数</param>
		public static void InvokeInstance<T>(T obj, string methodName, params object[] objs)
		{
			try
			{
				var type = obj.GetType();
				MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
				if (methodInfo == null)
				{
					methodInfo = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
				}
				methodInfo?.Invoke(obj, objs);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex);
			}
		}

		public static TResult FieldInstance<T, TResult>(T obj, string methodName, params object[] objs)
		{
			try
			{
				var type = obj.GetType();
				FieldInfo methodInfo = type.GetField(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
				if (methodInfo == null)
				{
					methodInfo = type.GetField(methodName, BindingFlags.Instance | BindingFlags.Public);
				}
				return (TResult)methodInfo?.GetValue(obj);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex);
			}

			return default(TResult);
		}

		public static TResult InvokeInstance<T, TResult>(T obj, string methodName, params object[] objs)
		{
			try
			{
				var type = obj.GetType();
				MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
				if (methodInfo == null)
				{
					methodInfo = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
				}

				return (TResult)methodInfo?.Invoke(obj, objs);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex);
			}

			return default(TResult);
		}

		/// <summary>
		/// 类静态方法
		/// </summary>
		/// <typeparam name="T">任意类</typeparam>
		/// <param name="methodName">类方法名</param>
		/// <param name="objs">参数</param>
		public static void InvokeStatic<T>(string methodName, params object[] objs)
		{
			try
			{
				var type = typeof(T).GetType();
				MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
				if (methodInfo == null)
				{
					methodInfo = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
				}
				methodInfo?.Invoke(type, objs);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex);
			}
		}

		public static TResult InvokeStatic<T, TResult>(string methodName, params object[] objs)
		{
			try
			{
				var type = typeof(T).GetType();
				MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
				if (methodInfo == null)
				{
					methodInfo = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
				}
				return (TResult)methodInfo?.Invoke(type, objs);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex);
			}
			return default(TResult);
		}

		public static TResult InvokeStatic<TResult>(Type staticType, string methodName, params object[] objs)
		{
			try
			{
				MethodInfo methodInfo = staticType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
				if (methodInfo == null)
				{
					methodInfo = staticType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
				}
				return (TResult)methodInfo?.Invoke(staticType, objs);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex);
			}
			return default(TResult);
		}
		#endregion

		#region Unity Window

		private static Dictionary<string, EditorWindow> m_dicEditorWindow = new Dictionary<string, EditorWindow>();
		private static EditorWindow GetUnityEditorWindow(string ClassName)
		{
			if (m_dicEditorWindow.ContainsKey(ClassName))
			{
				EditorWindow win;
				if ((win = m_dicEditorWindow[ClassName]) == null)
				{
					win = (m_dicEditorWindow[ClassName] = GetWindow(ClassName));
				}
				return win;
			}
			return m_dicEditorWindow[ClassName] = GetWindow(ClassName);
		}

		private readonly static string[] namespaceEditor = {nameof(UnityEditor), nameof(UnityEditor.AI) };
		private static EditorWindow GetWindow(string ClassName)
		{
			Type ty = null;
			int Length = namespaceEditor.Length;
            for (int i = 0; i < Length; i++)
            {
				ty = Type.GetType($"{namespaceEditor[i]}.{ClassName},{namespaceEditor[i]}", false);
				if (ty != null) break;
			}

			if (ty == null)
				ty = Type.GetType(ClassName + ",Assembly-CSharp-Editor", false);
			
			if (ty != null)
			{
				return EditorWindow.GetWindow(ty);
			}
			else
			{
				if (EditorApplication.isCompiling) return null;
				var types = typeof(SceneView).Assembly.GetTypes();
				foreach (var item in types)
				{
					if (item.Name == ClassName)
					{
						ty = item;
						break;
					}
				}
				if (ty != null)
				{
					return EditorWindow.GetWindow(ty);
				}
			}

			UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll<SearchableEditorWindow>();
			if (array != null && array.Length > 0)
			{
				foreach (var item in array)
				{
					if (ClassName == item.name)
					{
						return item as EditorWindow;
					}
				}
			}


			array = Resources.FindObjectsOfTypeAll<EditorWindow>();
			if (array != null && array.Length > 0)
			{
				foreach (var item in array)
				{
					if (ClassName == item.name)
					{
						return item as EditorWindow;
					}
				}
			}

			return null;
		}
		/// <summary>
		/// 监测窗口
		/// </summary>
		public static EditorWindow InspectorWindow => GetUnityEditorWindow(nameof(InspectorWindow));

		/// <summary>
		/// 项目工程窗口
		/// </summary>
		public static EditorWindow ProjectBrowser => GetUnityEditorWindow(nameof(ProjectBrowser));

		/// <summary>
		/// 场景监测窗口
		/// </summary>
		public static EditorWindow HierarchyWindow => GetUnityEditorWindow(nameof(HierarchyWindow));

		/// <summary>
		/// 场景监测窗口
		/// </summary>
		public static EditorWindow SceneView => GetUnityEditorWindow(nameof(SceneView));

		/// <summary>
		/// 控制台窗口
		/// </summary>
		public static EditorWindow ConsoleWindow => GetUnityEditorWindow(nameof(ConsoleWindow));

		/// <summary>
		/// 首页
		/// </summary>
		public static EditorWindow HomeWindow => GetUnityEditorWindow(nameof(HomeWindow));

		/// <summary>
		/// 关于窗口
		/// </summary>
		public static EditorWindow AboutWindow => GetUnityEditorWindow(nameof(AboutWindow));

		/// <summary>
		/// 保存对话框窗口
		/// </summary>
		public static EditorWindow AssetSaveDialog => GetUnityEditorWindow(nameof(AssetSaveDialog));

		/// <summary>
		/// 资源商店窗口
		/// </summary>
		public static EditorWindow AssetStoreWindow => GetUnityEditorWindow(nameof(AssetStoreWindow));

		/// <summary>
		/// 打包窗口 
		/// </summary>
		public static EditorWindow BuildPlayerWindow => GetUnityEditorWindow(nameof(BuildPlayerWindow));
		
		/// <summary>
		/// 曲线编辑窗口 
		/// </summary>
		public static EditorWindow CurveEditorWindow => GetUnityEditorWindow(nameof(CurveEditorWindow));
		/// <summary>
		/// 颜色拾取窗口 
		/// </summary>
		public static EditorWindow ColorPicker => GetUnityEditorWindow(nameof(ColorPicker));

		/// <summary>
		/// 渐变拾取窗口
		/// </summary>
		public static EditorWindow GradientPicker => GetUnityEditorWindow(nameof(GradientPicker));

		/// <summary>
		/// 图标选择窗口
		/// </summary>
		public static EditorWindow IconSelector => GetUnityEditorWindow(nameof(IconSelector));

		/// <summary>
		/// 灯光资源管理窗口
		/// </summary>
		public static EditorWindow LightingExplorerWindow => GetUnityEditorWindow(nameof(LightingExplorerWindow));

		/// <summary>
		/// 灯光窗口
		/// </summary>
		public static EditorWindow LightingWindow => GetUnityEditorWindow(nameof(LightingWindow));
		/// <summary>
		/// 网格导航窗口
		/// </summary>
		public static EditorWindow NavMeshEditorWindow => GetUnityEditorWindow(nameof(NavMeshEditorWindow));


		/// <summary>
		/// 监控窗口
		/// </summary>
		public static EditorWindow ProfilerWindow => GetUnityEditorWindow(nameof(ProfilerWindow));

		/// <summary>
		/// 项目设置窗口
		/// </summary>
		public static EditorWindow OpenProjectSettings => SettingsService.OpenProjectSettings();
		/// <summary>
		/// 用户首选窗口
		/// </summary>
		public static EditorWindow OpenUserPreferences => SettingsService.OpenUserPreferences();


		#endregion

		#region Unity Undo

		public static void UndoRecordObject(UnityEngine.Object obj,string name)
		{
			Undo.RecordObject(obj, name);
		}
		#endregion

		#region Unity EditorUtility
		/// <summary>
		/// 加载此目录下的资源 Editor Default Resources 文件名需要加后缀 (或者 unity内置的图标：不加后缀)
		/// </summary>
		public static Texture2D GetTexture2D(string fileName)
			=> EditorGUIUtility.Load(fileName) as Texture2D;

		/// <summary>
		/// 获取系统脚本的图标
		/// </summary>
		public static Texture2D GetTexture2D(Type type)
			=>EditorGUIUtility.ObjectContent(null, type).image as Texture2D;

		public static MonoScript GetMonoScript<T>(T target) where T : UnityEngine.MonoBehaviour
			=> MonoScript.FromMonoBehaviour(target);

		/// <summary>
		/// 设置脚本图标
		/// </summary>
		/// <typeparam name="T">继承 Monobehaver</typeparam>
		/// <param name="iconName">资源图片</param>
		public static void SetScriptIcon<T>(string iconName) where T : UnityEngine.Object
		{
			var tex = EditorGUIUtility.FindTexture(iconName);
			if (tex)
			{
				var obj = HukiryUtilEditor.FindAssetObject<MonoScript>(typeof(T).Name);
#if UNITY_2021_1_OR_NEWER
				EditorGUIUtility.SetIconForObject(obj, tex);
#else
				typeof(EditorGUIUtility).InvokeMember("SetIconForObject", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic, null, null, new object[2] {
					obj,
					tex
				});
#endif
			}
		}

		/// <summary>
		/// 资源定位
		/// </summary>
		/// <typeparam name="T">脚本类型</typeparam>
		/// <param name="fileName">文件名：无后缀</param>
		public static void LocationObject<T>(string fileName) where T : UnityEngine.Object
		{
			var obj = FindAssetObject<T>(fileName);
			EditorGUIUtility.PingObject(obj);

		}

		public static void LocationObject<T>(T t) where T : UnityEngine.Object
		{
			EditorGUIUtility.PingObject(t.GetInstanceID());
		}

		/// <summary>
		/// 定位脚本
		/// </summary>
		/// <param name="scriptName"></param>
		/// <param name="EditorDefaultResources"></param>
		public static void LocationMonoScript(string scriptName,string EditorDefaultResources = "Arrow.tga")
		{
			GUI.contentColor = Color.green;
			if (GUILayout.Button(new GUIContent("  SDK：", Hukiry.HukiryUtilEditor.GetTexture2D(EditorDefaultResources), ""), GUI.skin.textArea))
			{
				Hukiry.HukiryUtilEditor.LocationObject<MonoScript>(scriptName);
				GUI.changed = false;
			}
			GUI.contentColor = Color.white;
		}

		/// <summary>
		/// 打开Assets 目录下的文件
		/// </summary>
		/// <param name="fileName">文件名，不包含后缀</param>
		public static void OpenFile<T>(string fileName) where T : UnityEngine.Object
		{
			string assetpath = FindAssetPath<T>(fileName);
			EditorUtility.OpenWithDefaultApp(assetpath);
		}

		/// <summary>
		/// 在场景里，绘制窗口，右下角对齐
		/// </summary>
		/// <param name="callFunc">回调函数</param>
		/// <param name="winSize">窗口大小</param>
		/// <param name="space">右下角缝隙</param>
		/// <returns></returns>
		public static Vector2 DrawWindowOnScene(Func<Rect, Rect> callFunc, Vector2 winSize, float space = 5)
		{
			var viewSize = SceneView.position.size;
			Rect rect = new Rect(viewSize.x - winSize.x - space, viewSize.y - winSize.y - space, winSize.x, winSize.y);
			Rect lastRect = callFunc(rect);
			return lastRect.size;
		}

		/// <summary>
		/// 场景里编辑对象布局
		/// </summary>
		/// <param name="userAction">用户编辑回调</param>
		public static void LayoutHandleOnScene(Action userAction)
		{
			EventType eventType = Event.current.type;
			using (var check = new EditorGUI.ChangeCheckScope())
			{
				userAction?.Invoke();
				if (eventType == EventType.Layout)
				{
					HandleUtility.AddDefaultControl(0);
				}
				if (check.changed)
				{
					EditorApplication.QueuePlayerLoopUpdate();
				}
			}
		}

		public static void GetShowObjectPicker(string btnName,Sprite sprite ,Action<Sprite> callBack)
		{
			string commandName = Event.current.commandName;
			if (GUILayout.Button(btnName))
			{
				int _controlID = EditorGUIUtility.GetControlID(FocusType.Passive);
				EditorGUIUtility.ShowObjectPicker<Sprite>(sprite, false, "", _controlID);
			}

			if (commandName == "ObjectSelectorUpdated")
			{

				var selectSprite = EditorGUIUtility.GetObjectPickerObject() as Sprite;
				callBack?.Invoke(selectSprite);
			}

		}

		public static void SearchBarPreview(ref string mSearchText)
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(84f);

				mSearchText = EditorGUILayout.TextField("", mSearchText, "SearchTextField");
				if (GUILayout.Button("", "SearchCancelButton", GUILayout.Width(18f)))
				{
					mSearchText = string.Empty;
					GUIUtility.keyboardControl = 0;
				}
				GUILayout.Space(84f);
			}
			GUILayout.EndHorizontal();
			
		}
		#endregion

		#region Unity SpriteAtlas
		private static MethodInfo m_GetPreviewTexturesMethod = null, m_GetPackedSpritesMethod = null, m_PackAtlasesMethod = null;
		public static bool IsEnablePacker => EditorSettings.spritePackerMode == SpritePackerMode.BuildTimeOnlyAtlas || EditorSettings.spritePackerMode == SpritePackerMode.AlwaysOnAtlas
;
		//打包图集
		public static void PackingAtlas(SpriteAtlas spriteAtlas)
        {
            if (spriteAtlas)
                UnityEditor.U2D.SpriteAtlasUtility.PackAtlases(new SpriteAtlas[] { spriteAtlas }, EditorUserBuildSettings.activeBuildTarget);
        }

        public static void PackingAtlas(string assetPath)
        {
            var spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);
            PackingAtlas(spriteAtlas);
        }

        public static void PackingAtlas(int instanceID)
        {
            string AssetPath = AssetDatabase.GetAssetPath(instanceID);
            if (File.Exists(AssetPath))
            {
                var spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(AssetPath);
                PackingAtlas(spriteAtlas);
            }
        }

        //图集转贴图
        public static Texture2D[] SpriteAtlasToTexture(SpriteAtlas spriteAtlas)
        {
            try
            {
                if (m_GetPreviewTexturesMethod == null)
                {
					System.Type t = GetUnityEditor("SpriteAtlasExtensions");
					m_GetPreviewTexturesMethod = t.GetMethod("GetPreviewTextures", BindingFlags.NonPublic | BindingFlags.Static);
                }

                if (m_GetPreviewTexturesMethod != null)
                {
                    object textures = m_GetPreviewTexturesMethod.Invoke(null, new object[] { spriteAtlas });
                    return textures as Texture2D[];
				}
            }
            catch{}
            return null;
        }

		/// <summary>
		/// 图集转贴图：静态
		/// </summary>
		/// <param name="spriteAtlas"></param>
		/// <returns></returns>
		public static Texture2D SpriteAtlasToStaticTexture(SpriteAtlas spriteAtlas)
		{
			Texture2D[] previewTextures = SpriteAtlasToTexture(spriteAtlas);
			if (previewTextures == null || previewTextures.Length == 0)
			{
				return null;
			}
			Texture2D texture2D = previewTextures[0];

            var type = GetUnityEditor("SpriteUtility");
            return InvokeStatic<Texture2D>(type, "CreateTemporaryDuplicate", texture2D, texture2D.width, texture2D.height);
        }

		//图集转精灵集合
		public static Sprite[] SpriteAtlasToSprites(SpriteAtlas spriteAtlas)
        {
            Sprite[] sprites = null;
            if (!Application.isPlaying)
            {

                if (m_GetPackedSpritesMethod == null)
                {
                    System.Type t = GetUnityEditor("SpriteAtlasExtensions");
                    m_GetPackedSpritesMethod = t.GetMethod("GetPackedSprites", BindingFlags.NonPublic | BindingFlags.Static);
                }
                if (m_GetPackedSpritesMethod != null)
                {
                    object retval = m_GetPackedSpritesMethod.Invoke(null, new object[] { spriteAtlas });
                    var spritesArray = retval as Sprite[];
                    if (spritesArray != null && spritesArray.Length > 0)
                    {
                        sprites = spritesArray;
                    }
                }
            }
            if (sprites == null)
            {
                sprites = new Sprite[spriteAtlas.spriteCount];
                spriteAtlas.GetSprites(sprites);
                if (sprites.Length == 0)
                    return null;
            }
            return sprites;
        }

		#endregion

		#region Texture Color
		private static Dictionary<int, Texture2D> contrastTexture;
		private static Texture2D _CreateCheckerTex(Color c0, Color c1)
		{
			Texture2D texture2D = new Texture2D(16, 16, TextureFormat.ARGB32, false);
			texture2D.name = "CreateCheckerTex";
			texture2D.hideFlags = HideFlags.DontSave;
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					texture2D.SetPixel(j, i, c1);
				}
			}
			for (int k = 8; k < 16; k++)
			{
				for (int l = 0; l < 8; l++)
				{
					texture2D.SetPixel(l, k, c0);
				}
			}
			for (int m = 0; m < 8; m++)
			{
				for (int n = 8; n < 16; n++)
				{
					texture2D.SetPixel(n, m, c0);
				}
			}
			for (int num = 8; num < 16; num++)
			{
				for (int num2 = 8; num2 < 16; num2++)
				{
					texture2D.SetPixel(num2, num, c1);
				}
			}
			texture2D.Apply();
			texture2D.filterMode = FilterMode.Point;
			return texture2D;
		}
		public static Texture2D GetColorTexture(Color col1, Color col2)
		{
			if (contrastTexture == null) contrastTexture = new Dictionary<int, Texture2D>();
			int hash = col1.GetHashCode() + col2.GetHashCode();
			if (!contrastTexture.ContainsKey(hash))
			{
				var tex = _CreateCheckerTex(col1, col2);
				contrastTexture[hash] = tex;
			}
			return contrastTexture[hash];
		}
		/// <summary>
		/// 获取布局矩形框
		/// </summary>
		/// <returns></returns>
		public static Rect GetLayoutRect(float w=8, float h=8)
		{
			Rect rect;
			//最新矩形大小 8*8
			rect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, EditorGUIUtility.fieldWidth, w, h);
			return rect;
		}

		/// <summary>
		/// 绘制瓦片贴图
		/// </summary>
		/// <param name="rect"> 矩形</param>
		/// <param name="tex"> 贴图</param>
		public static void DrawTiledTexture(Rect rect, Texture tex)
		{
			Rect texCoords = new Rect(0f, 0f, rect.width / (float)tex.width, rect.height / (float)tex.height);
			TextureWrapMode wrapMode;
			wrapMode = tex.wrapMode;
			tex.wrapMode = TextureWrapMode.Repeat;
			GUI.DrawTextureWithTexCoords(rect, tex, texCoords);
			tex.wrapMode = wrapMode;
		}

		/// <summary>
		/// 绘制直线
		/// </summary>
		/// <param name="col1"></param>
		/// <param name="col2"></param>
		public static void DrawLine(Color col1, Color col2 )
		{
			var contrastTexture = GetColorTexture(col1, col2);
			Rect rect = GetLayoutRect();
			DrawTiledTexture(rect, contrastTexture);
		}

		public static void DrawLine(Color col1, Color col2, float wLine , float hLine )
		{
			var contrastTexture = GetColorTexture(col1, col2);
			Rect rect = GetLayoutRect(wLine,  hLine);
			DrawTiledTexture(rect, contrastTexture);
		}

		/// <summary>
		/// 绘制贴图
		/// </summary>
		public static Texture2D DrawTexture2D(Color col, int Width = 110, int Hight = 110, int WArea = 10, int HArea = 10)
		{
			Texture2D tex = new Texture2D(Width, Hight, TextureFormat.ARGB32, false);
			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Hight; j++)
				{
					tex.SetPixel(i, j, Color.clear);
					if (WArea != 0 && HArea != 0)
					{
						if ((j / WArea + 1) % 2 == 0 && (i / HArea + 1) % 2 != 0)
						{
							tex.SetPixel(i, j, col);
						}

						if ((j / WArea + 1) % 2 != 0 && (i / HArea + 1) % 2 == 0)
						{
							tex.SetPixel(i, j, col);
						}
					}
					else
					{
						tex.SetPixel(i, j, col);
					}
				}
			}
			tex.Apply();
			return tex;
		}

		#endregion

		#region Unity Assets Repair
		/// <summary>
		/// 修复目录下预制件资源
		/// </summary>
		/// <param name="extension">扩展名</param>
		public static void RepairPrefabAssets<T>(Action<T> callBack, string extension = "*.prefab", string desc = "移除字体阴影...") where T : UnityEngine.Object
		{
			if (callBack == null)
			{
				Debug.LogError("回调函数不能为空");
				return;
			}

			Action<string, string> actionCall = (assetPath, ext) => {
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
						EditorUtility.DisplayProgressBar(desc, strPath, i / (float)array.Length);

					}
				}
				else
				{
					actionCall(dirPath, extension);
					EditorUtility.DisplayProgressBar(desc, dirPath, index / (float)ids.Length);
				}
				EditorUtility.ClearProgressBar();

			}
		}
		#endregion

	}
}



/*  
GUILayoutUtility
GameObjectUtility
GUIUtility
ColorUtility
HandleUtility 
MeshCombineUtility
MeshUtility
PrefabUtility
RectTransformUtility
SceneModeUtility
StackTraceUtility
StaticBatchingUtility
EditorApplication

Event  鼠标事件：用于场景编辑
Selection 选择对象，用于场景，各种面板
ScriptableWizard 小窗口选择,不可悬浮

GenericMenu 鼠标右键菜单：用于场景编辑
SceneView 场景：用于场景对象选择回调
	OnSceneGUI
SerializedProperty 序列化属性：用于检测面板
Handles 场景绘制编辑工具：用于场景对象绘制

特性使用
InitializeOnLoadMethod 标记在方法上
CustomEditor 标记在类上：用于检测面板编辑
 */
