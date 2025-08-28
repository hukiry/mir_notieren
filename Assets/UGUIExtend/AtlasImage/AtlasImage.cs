using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Hukiry.UI
{
    [AddComponentMenu("UI/Atlas Image", 10)]
    [DrawIcon(typeof(RawImage), null)]
    public class AtlasImage : Image
    {
		public static Material m_matGray = null;
		/// <summary>
		/// 是否水平翻转
		/// </summary>
		public bool FlipHorizontal {
			get { return flipHor; }
			set {
				if (flipHor != value)
				{
					flipHor = value;
					this.UpdateGeometry();
					this.SetVerticesDirty();
				}
			}
		}
		/// <summary>
		/// 是否垂直翻转
		/// </summary>
		public bool FlipVertical {
			get { return flipVer; }
            set { 
				if (flipVer != value) {
					flipVer = value;
					this.UpdateGeometry();
					this.SetVerticesDirty();
				}
			}
		}

		[SerializeField]
		protected bool flipHor;
		[SerializeField]
		protected bool flipVer;

		[SerializeField]
        private SpriteAtlas m_SpriteAtlas;
        public SpriteAtlas spriteAtlas
        {
            get
            {
                return m_SpriteAtlas;
            }
            set
            {
                if(m_SpriteAtlas != value)
                {
                    m_SpriteAtlas = value;
					ChangeSprite();
				}
            }
        }

        [SerializeField]
        private string m_SpriteName = "";
        public string spriteName
        {
            get
            {
                return m_SpriteName;
            }
            set
            {
                if(m_SpriteName != value)
                {
                    m_SpriteName = value;
					ChangeSprite();
				}
            }
        }


		[SerializeField]
		private bool m_IsGray;
		public bool IsGray {
			get
			{
				return m_IsGray;
			}
			set
			{
				if (m_IsGray != value)
				{
					m_IsGray = value;
					ChangeGray();
				}
			}
		}

		protected override void Awake()
        {
            base.Awake();
            if(sprite == null)
            {
                ChangeSprite();
            }
			else
            {
                if(sprite.name != spriteName)
                {
                    ChangeSprite();
                }
            }
			this.UpdateGeometry();
			this.ChangeGray();
		}

		private void ChangeGray()
		{
			if (AtlasImage.m_matGray == null)
			{
				AtlasImage.m_matGray = new Material(Shader.Find("Hukiry/UI/Gray"));
				AtlasImage.m_matGray.name = "SpriteToGray";
				AtlasImage.m_matGray.hideFlags = HideFlags.HideInInspector| HideFlags.DontSave;
			}

			this.material = m_IsGray? AtlasImage.m_matGray : this.material;
		}

		private void ChangeSprite() {
			sprite = spriteAtlas ? spriteAtlas.GetSprite(spriteName) : null;
#if UNITY_EDITOR
			if (Application.isPlaying) { 
#endif
				if (sprite == null) {
					spriteName = "none";
				}
#if UNITY_EDITOR
			}
#endif
		}

		protected override void OnPopulateMesh(VertexHelper toFill) {
			base.OnPopulateMesh(toFill);

			if (flipHor || flipVer) {
				Vector2 rectCenter = rectTransform.rect.center;

				int vertCount = toFill.currentVertCount;
				for (int i = 0; i < vertCount; i++) {
					UIVertex uiVertex = new UIVertex();
					toFill.PopulateUIVertex(ref uiVertex, i);

					Vector3 pos = uiVertex.position;
					uiVertex.position = new Vector3(
						flipHor ? (pos.x + (rectCenter.x - pos.x) * 2) : pos.x,
						flipVer ? (pos.y + (rectCenter.y - pos.y) * 2) : pos.y,
						pos.z);

					toFill.SetUIVertex(uiVertex, i);
				}
			}
		}



#if UNITY_EDITOR
#if ENABLE_LUA
        [LuaInterface.NoToLua]
#endif
        public void StartFind()
        {
            if (sprite == null)
            {
                var spAtlas = Hukiry.HukiryUtilEditor.FindSpriteAtlas(spriteName);
                if (spAtlas)
                {
                    spriteAtlas = spAtlas;
                    Hukiry.HukiryUtilEditor.LocationObject<SpriteAtlas>(spriteAtlas.name);
                    sprite = spriteAtlas ? spriteAtlas.GetSprite(spriteName) : null;
                }
                else
                {
                    Hukiry.HukiryUtilEditor.LocationObject<Texture2D>(spriteName);
                    Debug.LogError($"设置的Object：{this.gameObject.name} 未打图集，找不到精灵名：{spriteName}");
                }
            }
        }
#if ENABLE_LUA
        [LuaInterface.NoToLua]
#endif
        public void StartFindBySpriteName(Sprite lastSprite)
        {
            int id = lastSprite.GetInstanceID();
            string assetPath = AssetDatabase.GetAssetPath(id);
            System.IO.FileInfo fi = new System.IO.FileInfo(assetPath);
            string spriteAtlasName = fi.DirectoryName.Replace('\\', '/');
            spriteAtlasName = spriteAtlasName.Substring(spriteAtlasName.LastIndexOf('/') + 1);
            var curSpriteAtlas = Hukiry.HukiryUtilEditor.FindAssetObject<SpriteAtlas>(spriteAtlasName);
            if (curSpriteAtlas)
            {
                spriteAtlas = curSpriteAtlas;
                spriteName = lastSprite.name;
            }
            else
            {
                curSpriteAtlas = FindSpriteAtlas(lastSprite);
                if (curSpriteAtlas)
                {
                    spriteAtlas = curSpriteAtlas;
                    spriteName = lastSprite.name;
                }
                else
                {
                    Hukiry.HukiryUtilEditor.LocationObject<Texture2D>(lastSprite.name);
                    if (Hukiry.HukiryUtilEditor.FindAssetObject<Texture2D>(lastSprite.name) == null)
                    {
                        Debug.LogError($"Unity Editor Of Object ：{this.gameObject.name}。 文件夹 {spriteAtlasName} 未打图集，找不到精灵名：{spriteName}");
                    }
                    else
                    {
                        Debug.LogError($"编辑的Object：{this.gameObject.name}。 文件夹 {spriteAtlasName} 未打图集，找不到精灵名：{spriteName}");
                    }
                }
            }
        }

        private SpriteAtlas FindSpriteAtlas(Sprite lastSprite)
        {
            foreach (string path in AssetDatabase.FindAssets("t:" + typeof(SpriteAtlas).Name).Select(x => AssetDatabase.GUIDToAssetPath(x)))
            {
                SpriteAtlas spriteAtla = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
                var sprite = spriteAtla.GetSprite(lastSprite.name);
                if (sprite && sprite.rect == lastSprite.rect && sprite.border == lastSprite.border)
                {
                    return spriteAtla;
                }
            }
            return null;
        }


        protected override void OnValidate()
        {
            base.OnValidate();
            this.ChangeSprite();
            this.ChangeGray();
            this.UpdateGeometry();
            this.SetVerticesDirty();
        }

        [ContextMenu("Copy SpriteName")]
        private void CopySpriteName()
        {
            GUIUtility.systemCopyBuffer = spriteName;
        }

        [ContextMenu("Pase SpriteName")]
        private void PasteSpriteName()
        {
            this.spriteName = GUIUtility.systemCopyBuffer;
        }

        [ContextMenu("Ping SpriteAtlas")]
        private void PingAtlas()
        {
            Hukiry.HukiryUtilEditor.LocationObject(this.spriteAtlas);
        }

        [ContextMenu("Packing SpriteAtlas")]
        private void PackingAtlas()
        {
            Hukiry.HukiryUtilEditor.PackingAtlas(this.spriteAtlas);
        }


        private bool isEnableMaterial = false;
        [ContextMenu("Enable Material")]
        private void EnableMaterial()
        {
            isEnableMaterial = !isEnableMaterial;
            if (isEnableMaterial)
            {
                m_matGray.hideFlags = IsGray ? HideFlags.DontSave : HideFlags.HideInInspector;
            }
            else
            {
                m_matGray.hideFlags = HideFlags.HideInInspector;
            }
        }

        [ContextMenu("Enable Material", true)]
        private bool EnableMaterialCheck()
        {
            return IsGray;
        }
#endif
    }
}


