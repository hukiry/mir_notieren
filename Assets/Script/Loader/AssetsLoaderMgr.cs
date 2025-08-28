using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hukiry
{
    //单个加载回调
    public delegate void OnAssetLoadedCallback<T>(string name, T asset) where T : UnityEngine.Object;

    //多个加载回调
    public delegate void OnAssetListLoadedCallback<T>(string name, T[] assetlist) where T : UnityEngine.Object;

    /// <summary>
    /// 资源加载
    /// </summary>
    public class AssetsLoaderMgr
    {
        
        public static bool LogLoadInfo = false;
        
        public static bool LogUnloadInfo = false;

        
        public static IBaseLoader Instance { get; private set; }

        private static IBaseLoader _Instance;


        /// <summary>
        /// 创建一个文件夹
        /// </summary>
        /// <param name="folder"></param>
         static void CreateDirectory(string folder)
        {
            if (folder.LastIndexOf('.') != -1)
            {
                folder = folder.Substring(0, folder.LastIndexOf('/'));
            }
            folder = folder.Replace("\\", "/");
            if (folder[folder.Length - 1] != Path.DirectorySeparatorChar)
                folder += Path.DirectorySeparatorChar;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>

        public static bool Initialize(Transform parent)
        {
            if (Instance != null)
                return true;
            CreateDirectory(AssetBundleConifg.CacheAbPath);

#if UNITY_EDITOR && !ASSETBUNDLE_TEST
            _Instance = EditorLoader.Create(parent);
#else
            _Instance = AssetBundleLoader.Create(parent);
#endif
            return true;
		}

		public static UnityEngine.Object LoadAsset(string assetName)
		{
			assetName = assetName.ToLower();
#if UNITY_EDITOR
			//LogManager.Log(LogLoadInfo, "加载资源" + assetName);

            if (assetName == null)
            {
                throw new Exception("AssetBundle Name 不能为空!");
            }
#endif
            return _Instance.LoadAsset(assetName, AssetBundleList.SwitchingPath(assetName));
        }

        public static object[] LoadAllAsset(string assetName)
        {
            assetName = assetName.ToLower();
#if UNITY_EDITOR
			//LogManager.Log(LogLoadInfo, "加载资源" + assetName);

            if (assetName == null)
            {
                throw new Exception("AssetBundle Name 不能为空!");
            }
#endif
            return _Instance.LoadAllAsset(assetName, AssetBundleList.SwitchingPath(assetName));
        }

        public static void LoadAsync(string assetName, 
            OnAssetLoadedCallback<UnityEngine.Object> OnLoadedCallback, bool loadObject = true)
        {
			assetName = assetName.ToLower();
#if UNITY_EDITOR
			//LogManager.Log(LogLoadInfo, "异步加载资源" + assetName);

            if (assetName == null)
            {
                throw new Exception("AssetBundle Name 不能为空!");
            }
#endif
            _Instance.LoadAsync(assetName, AssetBundleList.SwitchingPath(assetName), OnLoadedCallback, loadObject);
        }

        public static void LoadAllAsync(string assetName, OnAssetListLoadedCallback<UnityEngine.Object> OnLoadedCallback)
        {
            assetName = assetName.ToLower();
#if UNITY_EDITOR
			//LogManager.LogError(LogLoadInfo, "异步加载资源" + assetName);

            if (assetName == null)
            {
                throw new Exception("AssetBundle Name 不能为空!");
            }
#endif
            _Instance.LoadAllAsync(assetName, AssetBundleList.SwitchingPath(assetName), OnLoadedCallback);
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="onProgress"></param>
        public static void LoadSceneAsync(string levelName, Action<float> onProgress, Action onFinish)
        {
			_Instance.LoadSceneAsync(levelName, AssetBundleList.SwitchingPath("scenes/" + levelName.ToLower()), onProgress, onFinish);
        }

        /// <summary>
        /// 标识缓存列表
        /// </summary>
        
        public static HashSet<string> CacheMarkList = new HashSet<string>();

        /// <summary>
        /// 标识某个资源缓存
        /// </summary>
        /// <param name="path"></param>
        public static void ToCacheAssets(string assetName)
        {
            assetName = assetName.ToLower();
            CacheMarkList.Add(AssetBundleList.SwitchingPath(assetName));
        }

		/// <summary>
		/// 检查是否已加载过
		/// </summary>
		/// <param name="assetName"></param>
		/// <returns></returns>
		public static bool IsLoad(string assetName) {
            assetName = assetName.ToLower();
            assetName = AssetBundleList.SwitchingPath(assetName);

			return AssetBundleCache.FindAssetBundle(assetName) != null;
		}

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="delay"></param>
        public static void Unload(string assetName, int delay = 0, bool force = false)
        {
            assetName = assetName.ToLower();
            if (delay <= 0) {
                UnloadImmediate(assetName, force);
                return;
            }

            //LogManager.LogColor("red",LogUnloadInfo, "Unload释放资源" + assetName);

            AssetBundleList.AddTimer(delay, 0, UnloadImmediate, assetName, force);
        }

        /// <summary>
        /// 立即移除
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="force"></param>
        public static void UnloadImmediate(string assetName, bool force = false)
        {
            assetName = assetName.ToLower();
            assetName = AssetBundleList.SwitchingPath(assetName);

            //如果是缓存的,不做删除
            if (CacheMarkList.Contains(assetName))
            {
                if (!force)
                    return;
                CacheMarkList.Remove(assetName);
            }

            //释放资源
            if (force)
                AssetBundleCache.UnloadImmediate(assetName);
            else
                AssetBundleCache.UnloadAsset(assetName);
        }

        /// <summary>
        /// 卸载未使用的资源
        /// </summary>
        /// <param name="gc"></param>
        public static void UnloadUnusedAssets(bool gc)
        {
            _Instance.UnloadUnusedAssets(gc);
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public static void UnLoadAllAssets()
        {
            AssetBundleCache.UnloadAllAsset();
        }
    }
}
