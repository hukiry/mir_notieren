using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Hukiry.UI
{
    /// <summary>
    /// 将世界网格转换到UI网格 
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
	[DrawIcon(typeof(Mesh), null)]
	public class MeshGraphic : MaskableGraphic
	{
		public Vector3 overridePostion;
		public Vector3 overrideRotation;
		public Vector3 overrideScale = new Vector3(100f, 100f, 100f);
		private Mesh mesh;

		//静态变量
		private static Texture textureGoble;
		private static SpriteAtlas spriteAtlasGoble;
		private static Material matGoble;

		public override Texture mainTexture
		{
			get
			{
				return this.IsActive()?textureGoble:null;
			}
		}

        protected override void OnPopulateMesh(VertexHelper vh)
		{
			if (mesh != null)
			{
				vh.Clear();
				//提取Mesh信息
				int[] triangles = mesh.triangles;
				Vector3[] vertices = mesh.vertices;
				Vector3[] normals = mesh.normals;
				Vector2[] UVs = mesh.uv;
				//Color32[] colors = mesh.colors32;
				//处理缩放矩阵
				Matrix4x4 matrix4X4 = Matrix4x4.identity;
				matrix4X4.m00 = overrideScale.x;
				matrix4X4.m11 = overrideScale.y;
				matrix4X4.m22 = overrideScale.z;

				for (int i = 0; i < vertices.Length; i++)
				{
					//组合UI顶点信息
					UIVertex temp = new UIVertex();
					temp.position = matrix4X4.MultiplyPoint3x4((Quaternion.Euler(overrideRotation) * vertices[i]) + overridePostion);
					temp.uv0 = UVs[i];
					if (normals.Length > i)
					{
						temp.normal = normals[i];
					}
					//temp.color = colors.Length>0?colors[i]:(Color32)color;//使用生成的网格颜色
					temp.color = color;
					vh.AddVert(temp);
				}

				//设置三角形索引
				for (int i = 0; i < triangles.Length; i += 3)
				{
					vh.AddTriangle(triangles[i], triangles[i + 1], triangles[i + 2]);
				}
			}
		}

		/// <summary>
		/// 加载网格
		/// </summary>
		/// <param name="spriteAtlas">图集</param>
		/// <param name="infoArray">精灵信息的集合</param>
		public void LoadMesh(SpriteAtlas spriteAtlas, SpriteMeshInfo[] infoArray)
		{
			//1，计算网格适配图片大小
			this.AdaptionGraphicSize();

			if (spriteAtlasGoble == null) spriteAtlasGoble = spriteAtlas;
			//2,创建全局材质球		
			if (matGoble == null) matGoble = new Material(Shader.Find("UI/Default"));
			//3,加载材质												 
			this.material = matGoble;

			//4,加载贴图
			if (textureGoble == null) textureGoble = this.LoadTextureAtlas(spriteAtlasGoble);

			List<SpriteMeshInfo> spriteInfoLst = new List<SpriteMeshInfo>();
			//5，添加背景数据
			this.AddBackgroud(spriteInfoLst);
			//6, 加载网格
			if (infoArray != null && infoArray.Length > 0)
			{
				spriteInfoLst.AddRange(infoArray);
				spriteInfoLst.Sort((n, m) => n.sort - m.sort);
			}
			this.mesh = MeshUtliMatch.CreateGeometryMesh(spriteAtlasGoble, spriteInfoLst);
		}

		/// <summary>
		/// 计算网格适配图片大小
		/// </summary>
		private void AdaptionGraphicSize()
		{
			var w = this.rectTransform.sizeDelta.x / 100F * 8;
			var h = this.rectTransform.sizeDelta.y / 100F * 8;
			var result = h > w ? w : h;
			this.overrideScale = new Vector3(w, h, 1);
		}

		private Texture2D LoadTextureAtlas(SpriteAtlas spriteAtlas)
		{
			Sprite[] sprites = new Sprite[spriteAtlas.spriteCount];
			spriteAtlas.GetSprites(sprites);
			return sprites[0].texture;
		}

		private void AddBackgroud(List<SpriteMeshInfo> spriteInfoLst)
		{
			for (int i = -4; i <= 4; i++)
			{
				for (int j = -4; j <= 4; j++)
				{
					var pos = MeshUtliMatch.CoordToWorldPoint(i, j);
					spriteInfoLst.Add(new SpriteMeshInfo() { sort = 0, spriteName = "baseboard", centerPos = pos });
				}
			}
		}

#if UNITY_EDITOR
		//只有编辑器才会执行
		//修改面板属性重新刷新
		protected override void OnValidate()
		{
			base.OnValidate();
			SetMaterialDirty();
			SetVerticesDirty();
		}
#endif
	}
}