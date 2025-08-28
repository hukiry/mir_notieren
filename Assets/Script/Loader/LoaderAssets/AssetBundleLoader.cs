using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hukiry
{
    /// <summary>
    /// 资源加载管理
    /// </summary>
    public class AssetBundleLoader : MonoBehaviour, IBaseLoader {

		/// <summary>
		/// 当前的实体
		/// </summary>
		internal static LoaderAsyncAsset Instance { get; private set; }
        #region 资源依赖相关

        /// <summary>
        /// 获取AssetBundle的依赖，会缓存
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static string[] GetDependencies(string name) {
			HotAssetInfo mData = GetManifestData(name);
			if (mData != null) {
				return mData.ds.ToArray();
			}
			return new string[0] { };
		}

		/// <summary>
		/// 获取AssetBundle的依赖，会缓存
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static HotAssetInfo GetManifestData(string abName) {
			return AssetBundleList.ins.GetAssetBundleDataAtPath(abName);
		}

        /// <summary>
        /// 查找路径
        /// </summary>
        /// <param name="resName"></param>
        /// <returns></returns>
        public static string FindPath(string resName) {
			HotAssetInfo mData = GetManifestData(resName);
			if (mData != null) {
                return mData.GetPath();

            }
			return resName;
		}

		#endregion

		/// <summary>
		/// 创建加载器
		/// </summary>
		/// <param name="parent"></param>
		/// <returns></returns>
		public static AssetBundleLoader Create(Transform parent)
        {
            GameObject go = new GameObject("AssetBundleLoader");
            
            Instance = go.AddComponent<LoaderAsyncAsset>();

            DontDestroyOnLoad(go);

            return go.AddComponent<AssetBundleLoader>();
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <returns>资源对象</returns>
        public UnityEngine.Object LoadAsset(string assetName, string abName)
        {
            return LoaderLocalAsset.LoadAssets(assetName, abName);
        }

        /// <summary>
        /// 同步加载列表资源
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <returns></returns>
        public object[] LoadAllAsset(string assetName, string abName)
        {
            return LoaderLocalAsset.LoadAssetsList(assetName, abName);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <param name="OnLoadedCallback">加载完成回调</param>
        public void LoadAsync(string assetName, string abName, OnAssetLoadedCallback<UnityEngine.Object> OnLoadedCallback, bool loadObject)
        {
            AssetBundleList.ins.Log("LoadAsync",assetName, abName);
            //启动异步加载
            Instance.LoadAssets(assetName, abName, OnLoadedCallback, loadObject);
        }

        /// <summary>
        /// 异步加载资源列表
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <param name="OnLoadedCallback">加载完成回调</param>
        public void LoadAllAsync(string assetName, string abName, OnAssetListLoadedCallback<UnityEngine.Object> OnLoadedCallback)
        {
            //启动异步加载
            Instance.LoadAssets<UnityEngine.Object>(assetName, abName, OnLoadedCallback);
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
            float proportion = 0.5f;
            float displayProgress = 0;

			if (AssetBundleList.ins.GetAssetBundleDataAtPath(abName) != null) {
				//先加载ab文件
				bool abLoadFinish = false;
				LoadAsync(levelName, abName, (name, obj) => {
					abLoadFinish = true;
				}, true);

				while (!abLoadFinish) {
                    displayProgress = Mathf.Min(displayProgress + 0.01f, proportion);
                    onProgress(displayProgress);
					yield return new WaitForEndOfFrame();
				}
			}

            //从内存加载
            AsyncOperation op = SceneManager.LoadSceneAsync(levelName);

            while (!op.isDone) {
                onProgress(op.progress * proportion + proportion);
                yield return new WaitForEndOfFrame();
            }
			AssetsLoaderMgr.Unload(abName, 0, true);

            onFinish?.Invoke();
		}

        /// <summary>
        /// 加载失败,释放已经加载的资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        internal static void FailReleaseLoaded(string path, string parent)
        {
            //遍历子节点
            string[] dependices = GetDependencies(path);
            for (int i = 0; i < dependices.Length; ++i)
            {
                FailReleaseLoaded(dependices[i], path);
            }

            //如果存在释放parent的引用
            AssetBundleObject obj = AssetBundleCache.FindAssetBundle(path);
            if (obj != null)
            {
                obj.DelReference(parent);
            }
        }
        /// <summary>
        /// 卸载未使用的资源
        /// </summary>
        public void UnloadUnusedAssets(bool gc)
        {
            AssetBundleCache.UnloadUnuseAsset(gc);
        }

        /// <summary>
        /// 清理所有资源
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