#if UNITY_EDITOR
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hukiry
{
    /// <summary>
    /// 预制体加载
    /// </summary>
    public class EditorLoader : MonoBehaviour, IBaseLoader
    {
        /// <summary>
        /// 创建加载对象
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IBaseLoader Create(Transform parent)
        {
            GameObject go = new GameObject("EditorLoader");
            DontDestroyOnLoad(go);

            return go.AddComponent<EditorLoader>();
        }

        /// <summary>
        /// 获取资源目录
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public static string GetAssetPath(string abName)
        {
            string fileName = Path.GetFileName(abName);

            string fullPath = "Assets/" + Path.GetDirectoryName(abName) + "/";
            try
            {
				string[] files = Directory.GetFiles(fullPath, "*.*", SearchOption.TopDirectoryOnly);
				// 加载本地资源
				for (int i = 0, UPPER = files.Length; i < UPPER; i++)
                {
                    if (Path.GetExtension(files[i]) == ".meta") continue;

                    string temp = Path.GetFileNameWithoutExtension(files[i]);

                    if (fileName.Equals(temp, StringComparison.OrdinalIgnoreCase))
                    {
                        return files[i];
                    }
                }
            }
            catch (Exception e)
            {
                AssetBundleList.ins.LogException(e);
            }

            return abName;
        }

        public AssetBundleObject LoadAssetHolder(string abName)
        {
            AssetBundleObject obj = AssetBundleCache.FindAssetBundle(abName);
            if (obj == null) {
                obj = AssetBundleObject.Create(abName, null);
                obj.AddRefParent(abName);

                AssetBundleCache.AddLoaded(obj);
            }

            return obj;
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string assetName, string abName)
        {
            var holder = LoadAssetHolder(abName);

            var mainAsset = holder.LoadAsset<UnityEngine.Object>(abName);
            if (mainAsset == null)
            {
                AssetBundleList.ins.LogColor("red", "加载资源失败 => " + assetName);
            }

            return mainAsset;
        }

        /// <summary>
        /// 同步加载资源列表
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <returns></returns>
        public object[] LoadAllAsset(string assetName, string abName)
        {
            var holder = LoadAssetHolder(abName);

            var allAssets = holder.LoadAllAssets<UnityEngine.Object>();
            if (allAssets == null) {
                AssetBundleList.ins.LogColor("red", "加载资源失败 => " + assetName);
            }

            return allAssets;
        }

        /// <summary>
        /// 模拟异步加载
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <param name="OnLoadedCallback"></param>
        public void LoadAsync(string assetName, string abName, OnAssetLoadedCallback<UnityEngine.Object> OnLoadedCallback, bool loadObject)
        {
            StartCoroutine(_LoadAsync(assetName, abName, OnLoadedCallback, loadObject));
        }

        //模拟异步加载列表
        IEnumerator _LoadAsync(string assetName, string abName, OnAssetLoadedCallback<UnityEngine.Object> OnLoadedCallback, bool loadObject)
        {
            yield return null; //慢一帧回调

            if (OnLoadedCallback != null) {
                var asset = loadObject ? LoadAsset(assetName, abName) : null;

                OnLoadedCallback(assetName, asset as UnityEngine.Object); 
            }
        }

        /// <summary>
        /// 模拟异步加载列表
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <param name="OnLoadedCallback"></param>
        public void LoadAllAsync(string assetName, string abName, OnAssetListLoadedCallback<UnityEngine.Object> OnLoadedCallback)
        {
            StartCoroutine(_LoadAllAsync(assetName, abName, OnLoadedCallback));
        }

        //模拟异步加载列表
        IEnumerator _LoadAllAsync(string assetName, string abName, OnAssetListLoadedCallback<UnityEngine.Object> OnLoadedCallback)
        {
            var allAsset = LoadAllAsset(assetName, abName);
            if (OnLoadedCallback != null)
            {
                OnLoadedCallback(assetName, allAsset as UnityEngine.Object[]);
            }
            yield return null;
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="levelName"></param>
        /// <param name="onProgress"></param>
        public void LoadSceneAsync(string levelName, string abName, Action<float> onProgress, Action onFinish)
        {
            StartCoroutine(_LoadSceneAsync(levelName, abName, onProgress, onFinish));
        }

        IEnumerator _LoadSceneAsync(string levelName, string abName, Action<float> onProgress, Action onFinish)
        {
            //从内存加载
            AsyncOperation op = SceneManager.LoadSceneAsync(levelName);

            while (!op.isDone) {
                onProgress(op.progress);
                yield return new WaitForEndOfFrame();
            }
            onFinish?.Invoke();

            yield return null;
        }
        /// <summary>
        /// 释放未使用的资源
        /// </summary>
        /// <param name="gc"></param>
        public void UnloadUnusedAssets(bool gc)
        {
            Resources.UnloadUnusedAssets();
            
            if (gc) { GC.Collect(); }
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public void UnLoadAllAssets()
        {
            AssetBundleCache.UnloadAllAsset();
        }

        void OnApplicationQuit()
        {
            this.UnLoadAllAssets();
        }
    }
}

#endif