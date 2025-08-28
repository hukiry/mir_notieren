using Hukiry.Editor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
namespace UnityEditor.UI
{
    public class DrawImageWindow : CreateWindowEditor<DrawImageWindow>
    {
        public class DrawImageEditorData
        {
            [FieldName("保存路径")]
            public string saveFilePath= "ResourceData/Mesh";
            [FieldName("文件名后缀")]
            public string nameExt = string.Empty;
            [FieldName("绘制正多边形")]
            public bool isquadrilate = true;
            [FieldName("单位像素")]
            public int piexl =100;
            [FieldName("导出等放大数量")]
            public int num=4;
            [FieldName("导出图片颜色", "textureExternal")]
            public Color color= Color.green;

            [FieldName("输入要导出的模型")][Space]
            public GameObject objectGame;
            [FieldName("导出模型的缩放比例")]
            public Vector3 scale = Vector3.one;
        }

        DrawImageEditorData drawImageEditorData = new DrawImageEditorData();
        public override void DrawGUI()
        {
            drawImageEditorData =HukiryGUIEditor.DrawType(drawImageEditorData);
            if (drawImageEditorData.isquadrilate)
            {
                string savePath = Application.dataPath + "/" + drawImageEditorData.saveFilePath.Replace('\\', '/').TrimStart('/');
                EditorGUILayout.HelpBox("导出目录路径：" + savePath, MessageType.Info);
                if (GUILayout.Button("导出多边形"))
                {
                    if (drawImageEditorData.num > 0)
                    {
                        for (int i = 1; i <= drawImageEditorData.num; i++)
                        {
                            string name = i + "x" + i + drawImageEditorData.nameExt + ".png";
                            int piexl = drawImageEditorData.piexl * i;
                            //CreateImageGrid(piexl, name, drawImageEditorData.color, savePath);
                        }
                    }
                    this.ShowNotification(new GUIContent("导出完成"));
                }
            }
            else
            {
                string savePath = Application.dataPath + "/" + drawImageEditorData.saveFilePath.Replace('\\', '/').TrimStart('/');
                EditorGUILayout.HelpBox("导出目录路径：" + savePath, MessageType.Info);
                if (GUILayout.Button("导出模型数据"))
                {
                    if (drawImageEditorData.objectGame)
                    {
                        string objText=MeshToString(drawImageEditorData.objectGame.GetComponent<MeshFilter>(), drawImageEditorData.scale);
                        File.WriteAllText(savePath +"/"+ drawImageEditorData.objectGame.name + ".obj", objText + "\n" + objText, Encoding.UTF8);
                    }
                    this.ShowNotification(new GUIContent("导出完成"));
                }
            }
        }

        public override Color TitleColor()
        {
            return new Color(0, 0, 1, 0.3f);
        }

        //private void CreateImageGrid(int piexl, string name, Color color, string savePath)
        //{
        //    Vector2[] array = HukiryEditor.GetQuadrilatePoint(piexl);
        //    int w = (int)Mathf.Abs(array[0].x - array[2].x);
        //    int h = (int)Mathf.Abs(array[0].y - array[3].y);

        //    int x = (int)Mathf.Abs(array[0].x - array[3].x);

        //    //高度比宽度大时，需要计算，w/h的比率
        //    float delta = Mathf.Abs(array[0].x - array[3].x) / Mathf.Abs(array[0].y - array[3].y);

        //    Texture2D texOut = new Texture2D(w, h);
        //    for (int j = 0; j < h; j++)
        //    {
        //        for (int i = 0; i < w; i++)
        //        {
        //            texOut.SetPixel(i, j, color);

        //            if (i < x && i < j * delta)//判断纹理水平移动的x与y等量运行。
        //            {
        //                texOut.SetPixel(i, j, new Color(0, 0, 0, 0));
        //            }

        //            if (i > piexl && i > (j * delta + piexl))
        //            {
        //                texOut.SetPixel(i, j, new Color(0, 0, 0, 0));
        //            }
        //        }
        //    }
        //    texOut.Apply();
           
        //    byte[] buffer = texOut.EncodeToPNG();

        //    File.WriteAllBytes(savePath+"/" + name, buffer);

        //    AssetDatabase.Refresh();
        //}

        /*
 * 保存json数据
 */

        private string MeshToString(MeshFilter mf, Vector3 scale)
        {
            Mesh mesh = mf.mesh;
            Material[] sharedMaterials = mf.GetComponent<Renderer>().sharedMaterials;
            Vector2 textureOffset = mf.GetComponent<Renderer>().material.GetTextureOffset("_MainTex");
            Vector2 textureScale = mf.GetComponent<Renderer>().material.GetTextureScale("_MainTex");

            StringBuilder stringBuilder = new StringBuilder().Append("mtllib design.mtl")
                .Append("\n")
                .Append("g ")
                .Append(mf.name)
                .Append("\n");

            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 vector = vertices[i];
                stringBuilder.Append(string.Format("v {0} {1} {2}\n", vector.x * scale.x, vector.y * scale.y, vector.z * scale.z));
            }

            stringBuilder.Append("\n");

            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            if (mesh.subMeshCount > 1)
            {
                int[] triangles = mesh.GetTriangles(1);

                for (int j = 0; j < triangles.Length; j += 3)
                {
                    if (!dictionary.ContainsKey(triangles[j]))
                    {
                        dictionary.Add(triangles[j], 1);
                    }

                    if (!dictionary.ContainsKey(triangles[j + 1]))
                    {
                        dictionary.Add(triangles[j + 1], 1);
                    }

                    if (!dictionary.ContainsKey(triangles[j + 2]))
                    {
                        dictionary.Add(triangles[j + 2], 1);
                    }
                }
            }

            for (int num = 0; num != mesh.uv.Length; num++)
            {
                Vector2 vector2 = Vector2.Scale(mesh.uv[num], textureScale) + textureOffset;

                if (dictionary.ContainsKey(num))
                {
                    stringBuilder.Append(string.Format("vt {0} {1}\n", mesh.uv[num].x, mesh.uv[num].y));
                }
                else
                {
                    stringBuilder.Append(string.Format("vt {0} {1}\n", vector2.x, vector2.y));
                }
            }

            for (int k = 0; k < mesh.subMeshCount; k++)
            {
                stringBuilder.Append("\n");

                if (k == 0)
                {
                    stringBuilder.Append("usemtl ").Append("Material_design").Append("\n");
                }

                if (k == 1)
                {
                    stringBuilder.Append("usemtl ").Append("Material_logo").Append("\n");
                }

                int[] triangles2 = mesh.GetTriangles(k);

                for (int l = 0; l < triangles2.Length; l += 3)
                {
                    stringBuilder.Append(string.Format("f {0}/{0} {1}/{1} {2}/{2}\n", triangles2[l] + 1, triangles2[l + 2] + 1, triangles2[l + 1] + 1));
                }
            }

            return stringBuilder.ToString();
        }
    }
}
