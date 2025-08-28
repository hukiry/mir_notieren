using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UI;
using UnityEngine.Events;
using UnityEngine.U2D;
using UIIMageType = UnityEngine.UI.Image.Type;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine;

namespace Hukiry.UI {
	[CanEditMultipleObjects]
    [CustomEditor(typeof(AtlasImage), true)]
    public class AtlasImageEditor : ImageEditor
    {
		SpritePreview preview = new SpritePreview();

		private SerializedProperty m_SpriteAtlas;
        private SerializedProperty m_SpriteName;

        private AnimBool animShowType;
        private SerializedProperty m_PreserveAspect;
        private SerializedProperty m_Type;

		private SerializedProperty _spFlipHor;
		private SerializedProperty _spFlipVer;

		private SerializedProperty _isGray;
		private GUIContent _gcFlipHor;
		private GUIContent _gcFlipVer;
		private GUIContent _gcIsGray;

		private const string ATLAS_LIST = "Loading;Item;Scene";
		private const string PRE_ATLAS_NAME = "UI";

		protected override void OnEnable()
        {
            base.OnEnable();
            m_SpriteAtlas = serializedObject.FindProperty("m_SpriteAtlas");
            m_SpriteName = serializedObject.FindProperty("m_SpriteName");
            m_Type = serializedObject.FindProperty("m_Type");
            m_PreserveAspect = serializedObject.FindProperty("m_PreserveAspect");

            animShowType = new AnimBool(m_SpriteAtlas.objectReferenceValue && !string.IsNullOrEmpty(m_SpriteName.stringValue));
            animShowType.valueChanged.AddListener(new UnityAction(base.Repaint));

			preview.m_AtlasImage = target as AtlasImage;
			preview.onApplyBorder = () => {
				PackAtlas(m_SpriteAtlas.objectReferenceValue as SpriteAtlas);
				(target as AtlasImage).sprite = (m_SpriteAtlas.objectReferenceValue as SpriteAtlas).GetSprite(m_SpriteName.stringValue);
			};

			_spFlipHor = serializedObject.FindProperty("flipHor");
			_spFlipVer = serializedObject.FindProperty("flipVer");
			_isGray = serializedObject.FindProperty("m_IsGray");
			_gcFlipHor = EditorGUIUtility.TrTextContent("X", null);//Hukiry.HukiryUtilEditor.GetTexture2D("d_VerticalLayoutGroup Icon")
			_gcFlipVer = EditorGUIUtility.TrTextContent("Y", null);//Hukiry.HukiryUtilEditor.GetTexture2D("d_HorizontalLayoutGroup Icon")
			_gcIsGray = EditorGUIUtility.TrTextContent("IsGray", Hukiry.HukiryUtilEditor.GetTexture2D("SceneviewFx"));//
		}

		protected override void OnDisable() {
			base.OnDisable();
			preview.onApplyBorder = null;
		}

		public override void OnInspectorGUI()
		{

			serializedObject.Update();

			DrawAtlasPopupLayout(new GUIContent("Sprite Atlas"), new GUIContent("--"), m_SpriteAtlas);
			EditorGUI.indentLevel++;
			{
				DrawSpritePopup(m_SpriteAtlas.objectReferenceValue as SpriteAtlas, m_SpriteName, (selectedSpriteName) =>
				{
					if (selectedSpriteName == null)
						return;

					AtlasImageAssetSetting.Instance.lastSpriteName = selectedSpriteName;
					serializedObject.FindProperty("m_SpriteName").stringValue = selectedSpriteName;
					serializedObject.FindProperty("m_SpriteName").serializedObject.ApplyModifiedProperties();

				});
			}EditorGUI.indentLevel--;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			Hukiry.HukiryUtilEditor.GetShowObjectPicker("Select All", preview.sprite, spriteSelected => {
				
				m_SpriteName.stringValue = spriteSelected.name;
				m_SpriteName.serializedObject.ApplyModifiedProperties();
				(target as AtlasImage).StartFindBySpriteName(spriteSelected);
			});
			EditorGUILayout.EndHorizontal();

			AppearanceControlsGUI();
			RaycastControlsGUI();

			animShowType.target = m_SpriteAtlas.objectReferenceValue && !string.IsNullOrEmpty(m_SpriteName.stringValue);
			if (EditorGUILayout.BeginFadeGroup(animShowType.faded))
				this.TypeGUI();
			EditorGUILayout.EndFadeGroup();

			UIIMageType imageType = (UIIMageType)m_Type.intValue;
			base.SetShowNativeSize(imageType == UIIMageType.Simple || imageType == UIIMageType.Filled, false);

			if (EditorGUILayout.BeginFadeGroup(m_ShowNativeSize.faded))
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(m_PreserveAspect);
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup();

			this.FlipToggles();

			EditorGUILayout.PropertyField(_isGray, _gcIsGray);
			base.NativeSizeButtonGUI();



			if (GUI.changed)
			{
				EditorUtility.SetDirty(target);
			}

			serializedObject.ApplyModifiedProperties();


			////精灵更新
			AtlasImage image = target as AtlasImage;
			preview.sprite = GetOriginalSprite(image.spriteAtlas, image.spriteName);
			preview.color = image ? image.canvasRenderer.GetColor() : Color.white;


			AtlasImageUtility.DrawBorder(preview, () =>
			{
				string spriteName = string.IsNullOrEmpty(m_SpriteName.stringValue) ? "----" : m_SpriteName.stringValue;
				AtlasSpriteSelector.Show(m_SpriteAtlas.objectReferenceValue as SpriteAtlas, spriteName, (selectedSpriteName) =>
				{
					if (selectedSpriteName == null)
						return;

					AtlasImageAssetSetting.Instance.lastSpriteName = selectedSpriteName;
					m_SpriteName.stringValue = selectedSpriteName;
					m_SpriteName.serializedObject.ApplyModifiedProperties();
				});
                this.Repaint();
            },
			spriteName => {
				m_SpriteName.stringValue = spriteName;
				m_SpriteName.serializedObject.ApplyModifiedProperties();
			});

			if (preview.sprite == null)
			{
				GUI.contentColor = Color.yellow;
				if (GUILayout.Button("修复精灵显示"))
				{
					image.StartFind();
				}
				GUI.contentColor = Color.white;
			}
		}



		private void FlipToggles()
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, EditorGUIUtility.fieldWidth, 16f, 16f, EditorStyles.numberField);
			int controlID = GUIUtility.GetControlID("FlipToggleHash".GetHashCode(), FocusType.Keyboard, rect);
			rect = EditorGUI.PrefixLabel(rect, controlID, new GUIContent("Flip"));
			rect.width = 30f;
			this.FlipToggle(rect, _gcFlipHor, this._spFlipHor);
			rect.x += 30f;
			this.FlipToggle(rect, _gcFlipVer, this._spFlipVer);
			GUILayout.EndHorizontal();
		}


		private void FlipToggle(Rect r, GUIContent label, SerializedProperty property)
		{
			EditorGUI.BeginProperty(r, label, property);
			bool flag = property.boolValue;
			EditorGUI.BeginChangeCheck();
			int indentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			flag = EditorGUI.ToggleLeft(r, label, flag);
			EditorGUI.indentLevel = indentLevel;
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObjects(base.targets, "Edit Constraints");
				property.boolValue = flag;
			}
			EditorGUI.EndProperty();
		}

		/// <summary>
		/// 标题预览
		/// </summary>
		public override GUIContent GetPreviewTitle() {
			return preview.GetPreviewTitle();
		}

		/// <summary>
		/// 图片预览
		/// </summary>
		public override void OnPreviewGUI(Rect rect, GUIStyle background) {
			preview.OnPreviewGUI(rect);
		}

		/// <summary>
		/// 信息显示
		/// </summary>
		public override string GetInfoString() {
			return preview.GetInfoString();
		}

		/// <summary>
		/// 预览设置
		/// </summary>
		public override void OnPreviewSettings() {
			preview.OnPreviewSettings();
		}

		public static void DrawAtlasPopupLayout(GUIContent label, GUIContent nullLabel, SerializedProperty atlas, UnityAction<SpriteAtlas> onChange = null, params GUILayoutOption[] option)
        {
            DrawAtlasPopup(GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.popup, option), label, nullLabel, atlas, onChange);
        }

        public static void DrawAtlasPopup(Rect rect, GUIContent label, GUIContent nullLabel, SerializedProperty atlas, UnityAction<SpriteAtlas> onSelect = null)
        {
            DrawAtlasPopup(rect, label, nullLabel, atlas.objectReferenceValue as SpriteAtlas, obj =>
            {
				if (atlas!=null)
				{
					atlas.objectReferenceValue = obj;
					onSelect?.Invoke(obj as SpriteAtlas);
					atlas.serializedObject.ApplyModifiedProperties();
				}
            });
        }


        public static void DrawAtlasPopup(Rect rect, GUIContent label, GUIContent nullLabel, SpriteAtlas atlas, UnityAction<SpriteAtlas> onSelect = null)
        {
            rect = EditorGUI.PrefixLabel(rect, label);
            
            if (GUI.Button(rect, atlas ? new GUIContent(atlas.name) : nullLabel, EditorStyles.popup))
            {
                GenericMenu gm = new GenericMenu();
                gm.AddItem(nullLabel, !atlas, () => onSelect(null));

                foreach (string path in AssetDatabase.FindAssets("t:" + typeof(SpriteAtlas).Name).Select(x => AssetDatabase.GUIDToAssetPath(x)))
                {
                    string displayName = Path.GetFileNameWithoutExtension(path);
					if (ATLAS_LIST.Contains(displayName)|| displayName.ToUpper().StartsWith(PRE_ATLAS_NAME))
					{
						gm.AddItem(
						  new GUIContent(displayName),
						  atlas && (atlas.name == displayName),
						  x =>
						  {
							  AtlasImageAssetSetting.Instance.lastAtlasPath = (string)x;
							  onSelect(x == null ? null : AssetDatabase.LoadAssetAtPath((string)x, typeof(SpriteAtlas)) as SpriteAtlas);
						  },
						  path
					  );
					}
                }

                gm.DropDown(rect);
            }
        }

        public static void DrawSpritePopup(SpriteAtlas atlas, SerializedProperty spriteProperty, OnSpriteSelected callback)
        {
            GUIContent label = new GUIContent(spriteProperty.displayName, spriteProperty.tooltip);
            string spriteName = string.IsNullOrEmpty(spriteProperty.stringValue) ? "----" : spriteProperty.stringValue;

            using (new EditorGUI.DisabledGroupScope(!atlas))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.PrefixLabel(label);
                    if (GUILayout.Button(string.IsNullOrEmpty(spriteName) ? "-" : spriteName, "minipopup") && atlas)
                    {
                        AtlasSpriteSelector.Show(atlas, spriteName, callback);
                    }
                }
            }
        }

		/// <summary>
		/// Packs the atlas.
		/// </summary>
		/// <param name="atlas">Atlas.</param>
		static void PackAtlas(SpriteAtlas atlas)
		{
			UnityEditor.U2D.SpriteAtlasUtility.PackAtlases(new[] { atlas }, EditorUserBuildSettings.activeBuildTarget);
		}

		static Sprite GetOriginalSprite(SpriteAtlas atlas, string name) {
			if (!atlas || string.IsNullOrEmpty(name)) {
				return null;
			}

			SerializedProperty spPackedSprites = new SerializedObject(atlas).FindProperty("m_PackedSprites");
			return Enumerable.Range(0, spPackedSprites.arraySize)
				.Select(index => spPackedSprites.GetArrayElementAtIndex(index).objectReferenceValue)
				.OfType<Sprite>()
				.FirstOrDefault(s => s.name == name);
		}

		//[MenuItem("Game/Tools/UI/Change Image To Atlas", false, 1)]
		/// <summary>
		/// 在层次面板生效
		/// </summary>
		static public void AtlasImageReplaceImage()
        {
            GameObject[] selectedGOs = Selection.gameObjects;
            if (selectedGOs != null && selectedGOs.Length > 0)
            {
                for (int i = 0; i < selectedGOs.Length; i++)
                {
                    Image image = selectedGOs[i].GetComponent<Image>();
                    Dictionary<string, System.Object> tempImage = new Dictionary<string, System.Object>();
                    GameObject imgGo = image.gameObject;
                    Sprite sprite = image.sprite;
                    string spriteName = sprite ? sprite.name : null;
                    SpriteAtlas atlas = FindSpriteAtlas(image.sprite);

                    //获取所有属性
                    PropertyInfo[] pInfos = typeof(Image).GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance);
                    foreach (var pInfo in pInfos)
                    {
                        if (pInfo.GetGetMethod() == null || pInfo.GetSetMethod() == null) continue;
                        if (pInfo.Name == "sprite" || pInfo.Name == "overrideSprite") continue;
                        tempImage[pInfo.Name] = pInfo.GetValue(image);
                    }

                    if (spriteName == "UIMask" || spriteName == "UISprite" || spriteName == "Background" || spriteName == "Knob" || spriteName == "DropdownArrow" || spriteName == "Checkmark")
                    {
                        return;
                    }

                    GameObject.DestroyImmediate(image);
                    AtlasImage atlasImage = imgGo.AddComponent<AtlasImage>();

                    foreach (var pObj in tempImage)
                    {
                        PropertyInfo pInfo = typeof(AtlasImage).GetProperty(pObj.Key, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance);
                        if (pInfo != null)
                        {
                            pInfo.SetValue(atlasImage, pObj.Value);
                        }
                    }

                    if (atlas != null || spriteName != null)
                    {
                        atlasImage.spriteAtlas = atlas;
                        atlasImage.spriteName = spriteName;
                    }
                    EditorUtility.SetDirty(imgGo);
                }
            }
        }

        static private SpriteAtlas FindSpriteAtlas(Sprite lastSprite)
        {
            foreach (string path in AssetDatabase.FindAssets("t:" + typeof(SpriteAtlas).Name).Select(x => AssetDatabase.GUIDToAssetPath(x)))
            {
                SpriteAtlas spriteAtla = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
                var sprite = spriteAtla.GetSprite(lastSprite.name);
                if (sprite && sprite.rect == lastSprite.rect && sprite.border == lastSprite.border&& sprite.bounds == lastSprite.bounds)
                {
                    return spriteAtla;
                }
            }
            return null;
        }
        

	}
}
