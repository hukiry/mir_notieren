using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Hukiry.Editor;

public class ImageWindowEditor : CreateWindowEditor<ImageWindowEditor>
{
    public class ImageTex
    {
        [FieldName("纹理贴图")]
        public Texture2D tex;

        [FieldName("选择颜色叠加")]
        public Color selectChangeColor=Color.gray;

        [FieldName("保存尺寸")]
        public int texSize=64;

        [FieldName("保存副本图片的格式")]
        [Space]
        public ImageFormatState imageFormatState = ImageFormatState.TGA;

        [FieldName("生成Alpha图片尺寸")][Space(20)]
        public int AlphaSize = 100;

        [FieldName("生成Alpha图片名称")]
        public string AlphaFilename="Background";

       
    }


    /// <summary>
    /// 图片批量处理 缩放并处理成灰色图片
    /// </summary>
    public class ImageBatching
    {
        [FieldName("目录路径")]
        public string DirPath="Assets/ResourcesEx/garden/sprite";
		[FieldName("是否子目录导出")]
		public bool isExportSubDirByParentDir = true;

        [FieldName("缩放到指定比例的尺寸")]
        public int texSize = 256;

        [FieldName("保存副本图片的格式")]
        [Space]
        public ImageFormatState imageFormatState = ImageFormatState.PNG;

        [FieldName("转换为灰色")]
        public bool isGetGray = false;

        [FieldName("保存目录路径")]
        public string SaveDirPath = "Assets/ResourcesEx/garden/";

    }

    public enum ImageFormatState
    {
        PNG,
        JPG, 
        TGA
    }

    public void OpenTextureColorEditor(string str)
    {
        OpenWindowEditor(str);
    }
    [SerializeField]
    private ImageTex imageTex=new ImageTex ();
    private string saveFilePath = null;
    private string saveAlphaFilePath = null;

    private bool m_isBatch = false;
    private ImageBatching imageBatching = new ImageBatching();
    public override void DrawGUI()
    {
        m_isBatch = EditorGUILayout.ToggleLeft("是否批量处理缩放贴图", m_isBatch);
        if (m_isBatch)
        {
            imageBatching = HukiryGUIEditor.DrawType(imageBatching);
            if (GUILayout.Button("导出图片"))
            {
                if (Directory.Exists(imageBatching.DirPath) && Directory.Exists(imageBatching.SaveDirPath))
                {
                    ExportImageGray(imageBatching);
                }
                else
                {
                    LogManager.LogColor("red", "目录不存在：",imageBatching.SaveDirPath);
                }
            }

			if (imageBatching.isExportSubDirByParentDir)
			{
				imageBatching.DirPath = "Assets/ResourcesEx/garden";
			}
			
        }
        else { 
            imageTex = HukiryGUIEditor.DrawType<ImageTex>(imageTex, true);
            GUI.skin.label.fontSize = 12;
            if (imageTex.tex != null)
            {
                saveFilePath = AssetDatabase.GetAssetPath(imageTex.tex);
                GUILayout.Label("保存图片路径：" + saveFilePath);
                GUILayout.Label("原图尺寸[w:" + imageTex.tex.width + ",h:" + imageTex.tex.height + "]");
            }

            GUILayout.Space(5);
            if (!string.IsNullOrEmpty(imageTex.AlphaFilename))
            {
                saveAlphaFilePath = "Assets/Editor Default Resources/" + imageTex.AlphaFilename.Split('.')[0] + ".png";
                GUILayout.Label("save Alpha图片路径：" + saveAlphaFilePath);
            }


            if (imageTex.tex != null && imageTex.tex.isReadable == false)
            {
                TextureImporter texImpor = TextureImporter.GetAtPath(saveFilePath) as TextureImporter;
                texImpor.allowAlphaSplitting = true;
                texImpor.alphaIsTransparency = true;
                texImpor.isReadable = true;
                texImpor.sRGBTexture = true;
                texImpor.SaveAndReimport();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("缩放并保存为副本", GUILayout.Height(60)))
            {
                if (imageTex.tex != null)
                {
                    ChangeTextureColor(imageTex, saveFilePath);

                    AssetDatabase.Refresh();
                }
                else
                {
                    this.ShowNotification(new GUIContent("纹理资源不能为空！"));
                }
            }

            if (GUILayout.Button("创建Alpha格子图片", GUILayout.Height(60)))
            {
                ProduceAlphaTexture(saveAlphaFilePath, imageTex.selectChangeColor, imageTex.AlphaSize);
                AssetDatabase.Refresh();
            }
        }
    }

	//导出指定的缩放图片
    private void ExportImageGray(ImageBatching imageBatching)
    {
		if (imageBatching.isExportSubDirByParentDir) //是否批量导出
		{
			string[] dirs=Directory.GetDirectories(imageBatching.DirPath);
			for (int i = 0; i < dirs.Length; i++)
			{
				string dirName=dirs[i].Substring(dirs[i].LastIndexOf('\\')+1);
				if (dirName .Length>=6&& dirName.Length<=7&& dirName.Substring(0,6)=="sprite")
				{
					string saveDirPath = Application.dataPath+ "/../UIAtlasPacking/gray" + dirName;
					if (!Directory.Exists(saveDirPath))
					{
						Directory.CreateDirectory(saveDirPath);
					}

					//导出图片
					StartExportImaging(dirs[i],
						saveDirPath,
						imageBatching.isGetGray,
						imageBatching.texSize,
						imageBatching.imageFormatState);
				}
			}
		}
		else
		{	
			StartExportImaging(imageBatching.DirPath,
				imageBatching.SaveDirPath,
				imageBatching.isGetGray,
				imageBatching.texSize,
				imageBatching.imageFormatState);
		}

        EditorUtility.ClearProgressBar();
        this.ShowNotification(new GUIContent("Finish!"));
        AssetDatabase.Refresh();
    }

	private void StartExportImaging(string DirPath, string SaveDirPath,
		bool isGetGray,int texSize, ImageFormatState imageFormatState)
	{
		string[] files = Directory.GetFiles(DirPath);
		int length = files.Length;
		for (int i = 0; i < length; i++)
		{
			string[] fsarray = files[i].Split('.')[0].Split('_');
			if (fsarray[fsarray.Length - 1].ToLower().Trim() == "shadow")
			{
				continue;
			}

			string assetPath = files[i].Replace('\\', '/').Replace(Application.dataPath, "Assets");
			var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
			if (tex != null /*&& tex.isReadable == false*/)
			{
				TextureImporter texImpor = TextureImporter.GetAtPath(assetPath) as TextureImporter;

				texImpor.allowAlphaSplitting = true;
				texImpor.alphaIsTransparency = true;
				texImpor.isReadable = true;
				texImpor.sRGBTexture = true;
				texImpor.SaveAndReimport();
				AssetDatabase.Refresh();

			}

			if (tex != null)
			{
				ChangeTextureGrayColor(tex, isGetGray,
				  SaveDirPath + "/" + tex.name,
				  texSize,
				  imageFormatState);

				EditorUtility.DisplayProgressBar("转换UI贴图", assetPath, (float)i / (float)length);
			}
		}
	}

    public override Color TitleColor()
    {
        return new Color (255f,255,255f,0.5f);
    }

    private void ChangeTextureColor(ImageTex imageTex, string assetPath)
    {
        ChangeTextureColor(imageTex.tex, imageTex.selectChangeColor, assetPath, imageTex.texSize,imageTex.imageFormatState);
    }

    private void ChangeTextureColor(Texture2D tex,Color c,string assetPath,int size,ImageFormatState formatState)
    {
        if (tex.isReadable)
        {
            int w = tex.width;
            int h = tex.height;
            Texture2D texOut = tex;
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Color b = tex.GetPixel(i, j);
                    texOut.SetPixel(i, j, new Color(b.r * c.r, b.g * c.g, b.b * c.b, b.a));
                }
            }
            texOut.Apply();
            texOut = ScaleTextureBilinear(texOut, (float)size / (float)texOut.width);

            
            byte[] buffer = texOut.EncodeToPNG();
            switch (formatState)
            {
                case ImageFormatState.PNG:
                    buffer = texOut.EncodeToPNG();
                    assetPath = assetPath.Split('.')[0] + ".png";
                    break;
                case ImageFormatState.JPG:
                    buffer = texOut.EncodeToJPG();
                    assetPath = assetPath.Split('.')[0] + ".jpg";
                    break;
                case ImageFormatState.TGA:
                    buffer = texOut.EncodeToTGA();
                    assetPath = assetPath.Split('.')[0] + ".tga";
                    break;
            }

            File.WriteAllBytes(assetPath, buffer);
            this.ShowNotification(new GUIContent("修改成功！"));
        }
        else
        {
            this.ShowNotification(new GUIContent("纹理"+ tex.name + "不可读"));
        }
    }
    private void ChangeTextureGrayColor(Texture2D tex, bool isGray, string assetPath, int size, ImageFormatState formatState)
    {
        if (tex.isReadable)
        {
            Color gray = new Color(0.3f, 0.59f, 0.11f);
            Texture2D texOut = tex;
            if (isGray)
            {
                int w = tex.width;
                int h = tex.height;
                texOut = new Texture2D(w, h);
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        Color b = tex.GetPixel(i, j);
                        float grayValue = b.r * gray.r + b.g * gray.g + b.b * gray.b;
                        texOut.SetPixel(i, j, new Color (grayValue, grayValue, grayValue,b.a));
                    }
                }
                texOut.Apply();
            }

            if (texOut.width > size)
            {
                texOut = ScaleTextureBilinear(texOut, (float)size / (float)texOut.width);
            }

            byte[] buffer = texOut.EncodeToPNG();
            switch (formatState)
            {
                case ImageFormatState.PNG:
                    buffer = texOut.EncodeToPNG();
                    assetPath = assetPath + ".png";
                    break;
                case ImageFormatState.JPG:
                    buffer = texOut.EncodeToJPG();
                    assetPath = assetPath + ".jpg";
                    break;
                case ImageFormatState.TGA:
                    buffer = texOut.EncodeToTGA();
                    assetPath = assetPath + ".tga";
                    break;
            }

            File.WriteAllBytes(assetPath, buffer);
            this.ShowNotification(new GUIContent("修改成功！"));
        }
        else
        {
            this.ShowNotification(new GUIContent("纹理" + tex.name + "不可读"));
        }
    }

   private void ProduceAlphaTexture(string saveAssetPath, Color col, int size)
    {
        Texture2D tex = GetTexture2D(col,size, size);
        byte[] buffer = tex.EncodeToPNG();
        File.WriteAllBytes(saveAssetPath, buffer);
    }

    Texture2D ScaleTextureBilinear(Texture2D originalTexture, float scaleFactor)
    {
        Texture2D newTexture = new Texture2D(Mathf.CeilToInt(originalTexture.width * scaleFactor), Mathf.CeilToInt(originalTexture.height * scaleFactor));
        float scale = 1.0f / scaleFactor;
        int maxX = originalTexture.width - 1;
        int maxY = originalTexture.height - 1;
        for (int y = 0; y < newTexture.height; y++)
        {
            for (int x = 0; x < newTexture.width; x++)
            {
                // Bilinear Interpolation
                float targetX = x * scale;
                float targetY = y * scale;
                int x1 = Mathf.Min(maxX, Mathf.FloorToInt(targetX));
                int y1 = Mathf.Min(maxY, Mathf.FloorToInt(targetY));
                int x2 = Mathf.Min(maxX, x1 + 1);
                int y2 = Mathf.Min(maxY, y1 + 1);

                float u = targetX - x1;
                float v = targetY - y1;
                float w1 = (1 - u) * (1 - v);
                float w2 = u * (1 - v);
                float w3 = (1 - u) * v;
                float w4 = u * v;
                Color color1 = originalTexture.GetPixel(x1, y1);
                Color color2 = originalTexture.GetPixel(x2, y1);
                Color color3 = originalTexture.GetPixel(x1, y2);
                Color color4 = originalTexture.GetPixel(x2, y2);
                Color color = new Color(Mathf.Clamp01(color1.r * w1 + color2.r * w2 + color3.r * w3 + color4.r * w4),
                    Mathf.Clamp01(color1.g * w1 + color2.g * w2 + color3.g * w3 + color4.g * w4),
                    Mathf.Clamp01(color1.b * w1 + color2.b * w2 + color3.b * w3 + color4.b * w4),
                    Mathf.Clamp01(color1.a * w1 + color2.a * w2 + color3.a * w3 + color4.a * w4)
                    );
                newTexture.SetPixel(x, y, color);
            }
        }

        return newTexture;
    }

    private Texture2D GetTexture2D(Color col, int Width = 100, int Hight = 100, int WArea = 10, int HArea = 10)
    {
        Texture2D tex = new Texture2D(Width, Hight, TextureFormat.ARGB32, false);
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Hight; j++)
            {
                tex.SetPixel(i, j, new Color(255,255,255,0.2f));
                if ((j / WArea + 1) % 2 == 0 && (i / HArea + 1) % 2 != 0)
                {
                    tex.SetPixel(i, j, col);
                }

                if ((j / WArea + 1) % 2 != 0 && (i / HArea + 1) % 2 == 0)
                {
                    tex.SetPixel(i, j, col);
                }
            }
        }
        tex.Apply();
        return tex;
    }

}
