using Hukiry;
using Hukiry.Editor;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 图片切割
/// </summary>
public class ImageCutWindowEditor : CreateWindowEditor<ImageCutWindowEditor>
{
    public enum ImageSizeState
    {
        Pixel128_128,
        Pixel256_256,
        Pixel512_512
    }
    
    public class ImageCutData
    {
        [FieldName("切割的贴图")]
        public Texture2D tex;
        [FieldName("切割图片边长")]
        public ImageSizeState imageSizeState= ImageSizeState.Pixel512_512;
        [FieldName("切割导出的图片路径")]
        public string saveFilePath;
        [FieldName("导出Png格式")]
        public bool isExportPng;

    }

    ImageCutData imageCutData = new ImageCutData();
    public override void DrawGUI()
    {
        imageCutData = HukiryGUIEditor.DrawType(imageCutData);
        if (imageCutData.tex != null)
        {
            imageCutData.saveFilePath = Application.dataPath + "/ResourcesEx/garden/backgroundisland/" + imageCutData.tex.name;

            if (GUILayout.Button("切割"))
            {
                StartCutTexture2D(imageCutData);
                this.ShowNotification(new GUIContent("切割完成!"));
            }
        }
    }

    private void StartCutTexture2D(ImageCutData imageCutData)
    {
        string dirPath = imageCutData.saveFilePath;
		HukiryUtilEditor.IfNotDirCreate(dirPath);
        int n = int.Parse(imageCutData.imageSizeState.ToString().Split('_')[1]);

        if (imageCutData.tex.isReadable)
        {
            int width = imageCutData.tex.width;
            int height = imageCutData.tex.height;

            int cell = (width /n);//切割个数
            if (width % n != 0)
            {
                cell += 1;
            }
            int row = (height / n);//切割个数

            if (height % n != 0)
            {
                row += 1;
            }

            float sum = cell * row;
            int index = 0;
            int posx = 0, posy = 0;
            for (int i = 0; i < cell; i++)
            {
                posx = i * n;
                for (int j = 0; j < row; j++)
                {
                    posy = j * n;
                    index++;

                    string preName = imageCutData.tex.name + "_" + i + "_" + j;
                    string filePath = dirPath + "/" + preName + ".jpg";
                    if (imageCutData.isExportPng)
                    {
                        filePath = dirPath + "/" + preName + ".png";
                    }

                    int w = n;
                    int h = n;

                    if (posx + n >= width)
                    {
                        w = width - posx;
                    }

                    if (posy + n >= height)
                    {
                        h = height - posy;
                    }

                    CutTexture2D(imageCutData.tex, posx, posy, w,h, filePath, preName, imageCutData.isExportPng);

                    EditorUtility.DisplayProgressBar("图片切割开始...", preName, (float)index / sum);
                }
            }
            EditorUtility.ClearProgressBar();
        }
        else
        {
            //CommonMethod.TextureImport(imageCutData.tex.GetInstanceID());
        }
    }

    private void CutTexture2D(Texture2D tex, int x, int y, int w, int h, string filePath, string name, bool isExportPng)
    {
        Texture2D texOut = new Texture2D(w, h);

        var colArray = tex.GetPixels(x, y, w, h);
        texOut.SetPixels(0, 0, w, h, colArray);
        texOut.name = name;
        texOut.Apply();

        byte[] buffer = isExportPng? texOut.EncodeToPNG():texOut.EncodeToJPG();
        File.WriteAllBytes(filePath, buffer);


        //CommonMethod.TextureImport(filePath.Replace('\\','/').Replace(Application.dataPath,"Assets"),true);
    }

    public override Color TitleColor()
    {
        return new Color(255, 0, 255, 0.4f);
    }
}
