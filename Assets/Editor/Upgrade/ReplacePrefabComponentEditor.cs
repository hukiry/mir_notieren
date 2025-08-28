using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using Hukiry.UI;
using System;
using System.Reflection;
using UnityEngine.U2D;
using System.Linq;

namespace Hukiry.Prefab
{
    /*
     * 1,项目升级，批量或多个替换预制件组件，只需要更换类型即可
     * 例如：Image -》AtlasImage
     */
    public class ReplacePrefabComponentEditor
    {
        [MenuItem("Assets/Hukiry/Replace Component/Image To AtlasImage", false, 1)]
        private static void ReplaceImage()
        {
            StartReplace<Image, AtlasImage>((atlasImage, atlas, spriteName) =>
            {
                atlasImage.spriteAtlas = atlas;
                atlasImage.spriteName = spriteName;
            });
        }

        [MenuItem("Assets/Hukiry/Replace Component/AtlasImage To Image", false, 2)]
        private static void ReplaceAtlasImage()
        {
            StartReplace<AtlasImage, Image>((atlasImage, atlas, spriteName) =>
            {
               int id= atlas.GetSprite(spriteName).GetInstanceID();
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(id));
                if (sprite == null) sprite = atlas.GetSprite(spriteName);
                atlasImage.sprite = sprite;
            });
        }

        private static void StartReplace<T, K>(Action<K, SpriteAtlas, string> actionCall) where T : Image where K : Image
        {
            var guids = Selection.instanceIDs;
            int index = 0;
            foreach (var id in guids)
            {
                index++;
                string assetPath = AssetDatabase.GetAssetPath(id);
                if (Directory.Exists(assetPath))
                {
                    string[] files = Directory.GetFiles(assetPath, "*.prefab", SearchOption.AllDirectories);
                    for (int i = 0; i < files.Length; i++)
                    {
                        string path = files[i].Replace('\\', '/').Replace(Application.dataPath, "Assets");
                        ReplaceComponent<T, K>(path, actionCall);
                        EditorUtility.DisplayProgressBar("替换预制件组件", path, i / (float)files.Length);
                    }
                }
                else
                {
                    ReplaceComponent<T, K>(assetPath, actionCall);
                    EditorUtility.DisplayProgressBar("替换预制件组件", assetPath, index / (float)guids.Length);
                }
            }
            EditorUtility.ClearProgressBar();
        }

        //替换图片组件
        private static void ReplaceComponent<T, K>(string assetPath, Action<K, SpriteAtlas, string> actionCall) where T : Image where K : Image
        {
            GameObject go = PrefabUtility.LoadPrefabContents(assetPath);
            if (go)
            {
                T[] images = go.GetComponentsInChildren<T>(true);
                if (images != null && images.Length > 0)
                {
                    for (int i = 0; i < images.Length; i++)
                    {
                        if (images[i].GetType().Name == typeof(K).Name) continue;
                        ReplaceComponent<T, K>(images[i], actionCall);
                    }
                    PrefabUtility.SaveAsPrefabAsset(go, assetPath);
                }
            }
        }

        static public void ReplaceComponent<T, K>(T image, Action<K, SpriteAtlas, string> actionCall) where T : Image where K : Image
        {
            Dictionary<string, System.Object> tempImage = new Dictionary<string, System.Object>();
            GameObject imgGo = image.gameObject;
            string spriteName = image.sprite ? image.sprite.name : null;
            SpriteAtlas atlas = Hukiry.HukiryUtilEditor.FindSpriteAtlas(image.sprite);

            //获取所有属性
            PropertyInfo[] pInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance);
            foreach (var pInfo in pInfos)
            {
                if (pInfo.GetGetMethod() == null || pInfo.GetSetMethod() == null) continue;
                if (pInfo.Name == "sprite" || pInfo.Name == "overrideSprite") continue;
                tempImage[pInfo.Name] = pInfo.GetValue(image);
            }

            //Unity 自带的精灵名
            if (spriteName == "UIMask" || spriteName == "UISprite" || spriteName == "Background" || spriteName == "Knob" || spriteName == "DropdownArrow" || spriteName == "Checkmark" || spriteName == "InputFieldBackground")
            {
                return;
            }

            GameObject.DestroyImmediate(image);
            K atlasImage = imgGo.AddComponent<K>();

            foreach (var pObj in tempImage)
            {
                PropertyInfo pInfo = typeof(K).GetProperty(pObj.Key, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance);
                if (pInfo != null)
                {
                    pInfo.SetValue(atlasImage, pObj.Value);
                }
            }

            if (atlas != null || spriteName != null)
            {
                actionCall(atlasImage, atlas, spriteName);
            }

            EditorUtility.SetDirty(imgGo);

        }

        /// <summary>
        /// 组件替换方法
        /// </summary>
        /// <typeparam name="T">旧组件类：被替换的目标</typeparam>
        /// <typeparam name="K">新组件类</typeparam>
        /// <typeparam name="L">数据类</typeparam>
        /// <param name="component">组件类</param>
        /// <param name="filterCall">过滤无需拷贝的类，成功返回Null，否则返回数据</param>
        /// <param name="actionCall">拷贝组件类后的处理</param>
        static public void ReplaceComponent<T, K, L>(T component, Func<GameObject, L> filterCall, Action<K, L> actionCall) where T : MonoBehaviour where K : MonoBehaviour where L : UnityEngine.Object
        {
            Dictionary<string, System.Object> tempImage = new Dictionary<string, System.Object>();
            GameObject imgGo = component.gameObject;
            //获取所有属性
            PropertyInfo[] pInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance);
            foreach (var pInfo in pInfos)
            {
                if (pInfo.GetGetMethod() == null || pInfo.GetSetMethod() == null) continue;
                if (pInfo.Name == "sprite" || pInfo.Name == "overrideSprite") continue;
                tempImage[pInfo.Name] = pInfo.GetValue(component);
            }

            L atlas = filterCall(imgGo);
            if (atlas == null) return;

            GameObject.DestroyImmediate(component);
            K atlasImage = imgGo.AddComponent<K>();

            foreach (var pObj in tempImage)
            {
                PropertyInfo pInfo = typeof(K).GetProperty(pObj.Key, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Instance);
                if (pInfo != null)
                {
                    pInfo.SetValue(atlasImage, pObj.Value);
                }
            }

            if (atlas != null)
            {
                actionCall(atlasImage, atlas);
            }

            EditorUtility.SetDirty(imgGo);

        }


        [MenuItem("Assets/Hukiry/Texture2D To Sprite")]
        static void ExportTexture2DToSprite()
        {
            Texture2D image = Selection.activeObject as Texture2D; //获取选择的对象
            if (Selection.activeObject == null || image == null) return;

            string imagePath = AssetDatabase.GetAssetPath(image);
            string rootPath = System.IO.Path.GetDirectoryName(imagePath); //获取路径名称
            TextureImporter texImp = AssetImporter.GetAtPath(imagePath) as TextureImporter; //获取图片的AssetImporter
            texImp.isReadable = true;
            texImp.SaveAndReimport();
            AssetDatabase.CreateFolder(rootPath, image.name); //创建文件夹
            foreach (SpriteMetaData metaData in texImp.spritesheet) //遍历小图集
            {
                Texture2D myimage = new Texture2D((int)metaData.rect.width, (int)metaData.rect.height);

                for (int y = (int)metaData.rect.y; y < metaData.rect.y + metaData.rect.height; y++) //Y轴像素
                {
                    for (int x = (int)metaData.rect.x; x < metaData.rect.x + metaData.rect.width; x++)
                        myimage.SetPixel(x - (int)metaData.rect.x, y - (int)metaData.rect.y, image.GetPixel(x, y));
                }

                if (myimage.format != TextureFormat.ARGB32 && myimage.format != TextureFormat.RGB24)
                {
                    Texture2D newTexture = new Texture2D(myimage.width, myimage.height);
                    newTexture.SetPixels(myimage.GetPixels(0), 0);
                    myimage = newTexture;
                }

                File.WriteAllBytes($"{rootPath}/{image.name }/{metaData.name}.png", myimage.EncodeToPNG());
            }

            AssetDatabase.Refresh();
        }
    }
}