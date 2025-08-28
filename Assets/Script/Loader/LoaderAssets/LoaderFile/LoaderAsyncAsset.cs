using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hukiry
{
    /// <summary>
    /// 资源加载  管理LoadRequest，异步加载资源
    /// </summary>
    public class LoaderAsyncAsset : MonoBehaviour
    {
		private static Queue<LoadWaitingInfo> waitingQueue = new Queue<LoadWaitingInfo>();
        internal static void AddOneLoadAsync(string assetName, string abName,OnAssetLoadedCallback<UnityEngine.Object> onLoadedCallback, bool loadObject)
		{
			bool has = false;
			waitingQueue.ToList().ForEach(info => {
				if (info.assetName == assetName)
				{
					info.onLoadedCallback.Add(onLoadedCallback);
					has = true;
				}
			});
			if (!has)
			{
				LoadWaitingInfo info = new LoadWaitingInfo();
				info.assetName = assetName;
				info.abName = abName;
				info.onLoadedCallback = new List<OnAssetLoadedCallback<UnityEngine.Object>> { onLoadedCallback };
				info.loadObject = loadObject;
				waitingQueue.Enqueue(info);
			}
		}

		private void Update() {
			if (m_WaitingRequest.Count == 0 && waitingQueue.Count > 0) {
				LoadWaitingInfo info = waitingQueue.Dequeue();
				StartCoroutine(LoadAssetAsync(info.assetName, info.abName, info.onLoadedCallback, info.loadObject));
			}
		}

        #region 异步加载资源

        /// <summary>
        /// 异步加载资源
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <param name="onLoadedCallback"></param>
        /// </summary>
        internal void LoadAssets(string assetName, string abName, OnAssetLoadedCallback<UnityEngine.Object> onLoadedCallback, bool loadObject)
        {
            AssetBundleList.ins.Log(AssetsLoaderMgr.LogLoadInfo, "异步加载资源：" + "当前等待数：" + waitingQueue.Count + "正在加载数：" + m_WaitingRequest.Count);
			AddOneLoadAsync(assetName, abName, onLoadedCallback, loadObject);
		}

        //加载资源
        IEnumerator LoadAssetAsync<T>(string assetName, string abName, List<OnAssetLoadedCallback<T>> onLoadedCallback, bool loadObject) where T : UnityEngine.Object
        {
			//从缓存中查找
			AssetBundleObject obj = AssetBundleCache.FindAssetBundle(abName);
            if (obj == null) {
                BaseRequest request = CreateRequest(abName);
                yield return request;

				obj = AssetBundleCache.FindAssetBundle(abName);

                request.DelReference(abName);
            }
			if (obj != null) {
				//加载完成
				if (onLoadedCallback != null) {
					//场景对象
					if (obj.IsStreamed) { onLoadedCallback.ForEach(fun => fun(assetName, null)); yield break; }

					//不加载预制体
					if (!loadObject) { onLoadedCallback.ForEach(fun => fun(assetName, null)); yield break; }

					//启动异步加载
					if (!obj.HasLoaded(assetName)) {
						AssetBundleRequest abReq = obj.LoadAsync<T>(assetName);
						while (!abReq.isDone) {
							yield return abReq;
						}
						obj.SetAsyncLoad(assetName, abReq.asset);
					}
                    AssetBundleList.ins.Log(AssetsLoaderMgr.LogLoadInfo, "异步加载资源完成" + assetName);
					//加载完成
					onLoadedCallback.ForEach(fun => fun(assetName, obj.LoadAsset<T>(assetName)));
				}

                yield break;
            }

            AssetBundleList.ins.LogError($"manifestmap文件清单中找不到， 异步资源加载失败! {assetName}");

            //对空的进行回调
            if (onLoadedCallback != null) onLoadedCallback.ForEach(fun => fun(assetName, null));

            //释放已经加载的
            AssetBundleLoader.FailReleaseLoaded(assetName, abName);
        }

        #endregion

        #region 异步加载资源列表

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <param name="onLoadedCallback"></param>
        internal void LoadAssets<T>(string assetName, string abName,OnAssetListLoadedCallback<T> onLoadedCallback) where T : UnityEngine.Object
        {
            StartCoroutine(LoadAssetListAsync<T>(assetName, abName, onLoadedCallback));
        }

        //加载资源列表
        IEnumerator LoadAssetListAsync<T>(string assetName, string abName,OnAssetListLoadedCallback<T> onLoadedCallback) where T : UnityEngine.Object
        {
			//从缓存中查找
			AssetBundleObject obj = AssetBundleCache.FindAssetBundle(abName);
            if (obj == null)
            {
                BaseRequest request = CreateRequest(abName);
                yield return request;

                request.DelReference(abName);
                obj = AssetBundleCache.FindAssetBundle(abName);
            }

            if (obj != null)
            {
                //场景对象
                if (obj.IsStreamed) { 
                    onLoadedCallback(assetName, null); 
                    yield break; 
                }

				//启动异步加载
				if (!obj.HasLoaded()) {
					AssetBundleRequest abReq = obj.LoadAllAsync<T>();
					while (!abReq.isDone) {
						yield return abReq;
					}
					obj.SetAllAsyncLoad(assetName, abReq.allAssets);
				}

                //加载完成
                if (onLoadedCallback != null) {
                    onLoadedCallback(assetName, obj.LoadAllAssets<T>());
                }

                yield break;
            }

            AssetBundleList.ins.LogColor("red",$"异步加载资源列表加载失败! {assetName}");

            //对空的进行回调
            if (onLoadedCallback != null) onLoadedCallback(assetName, null);

            //释放已经加载的
            AssetBundleLoader.FailReleaseLoaded(assetName, abName); 
        }

        #endregion

        #region 异步加载逻辑处理

        /// <summary>
        /// 正在加载的请求
        /// </summary>
        static Dictionary<string, BaseRequest> m_WaitingRequest = new Dictionary<string, BaseRequest>();

		/// <summary>
		/// 查找加载中的
		/// </summary>
		/// <param abName="abName"></param>
		/// <returns></returns>
		internal static BaseRequest FindLoading(string abName)
        {
            BaseRequest request = null;
            m_WaitingRequest.TryGetValue(abName, out request); return request;
        }

		/// <summary>
		/// 移除加载中的
		/// </summary>
		/// <param abName="abName"></param>
		internal static void RemoveLoading(string abName)
        {
            m_WaitingRequest.Remove(abName);
        }

        /// <summary>
        /// 最大同时处理个数
        /// </summary>
        public const int MAX_PROGRESS = 5;

        /// <summary>
        /// 当前等待处理列表
        /// </summary>
        static List<BaseRequest> mWaitingRequest = new List<BaseRequest>();

        /// <summary>
        /// 当前处理列表
        /// </summary>
        static List<BaseRequest> mProgressRequest = new List<BaseRequest>();

        /// <summary>
        /// 是否运行更新
        /// </summary>
        static bool KeepRunning;

		/// <summary>
		/// 是否有子节点加载失败
		/// </summary>
		/// <param abName="abName"></param>
		/// <returns></returns>
		internal static bool HasChildErrorOrCancel(string abName)
        {
            string[] dependencies = AssetBundleLoader.GetDependencies(abName);

            BaseRequest request;
            for (int i = 0; i < dependencies.Length; ++i)
            {
                string childname = dependencies[i];

                if (LoaderAsyncAsset.HasChildErrorOrCancel(childname))
                    return true;

                if(m_WaitingRequest.TryGetValue(childname, out request)) {
                    if(!string.IsNullOrEmpty(request.Error)) return true;
                }
            }

            return false;
        }

		#region 创建异步请求

		/// <summary>
		/// 创建请求
		/// </summary>
		/// <param abName="abName"></param>
		/// <returns></returns>
		BaseRequest CreateRequest(string abName, bool root = true)
        {
            HotAssetInfo md = AssetBundleLoader.GetManifestData(abName);

            //在加载列表中查找
            BaseRequest request = null;
            if (m_WaitingRequest.TryGetValue(abName, out request))
            {
                if (root) request.AddRefParent(abName); return request;
            }

            BaseRequest loading = new ABAsyncRequest(abName);
            if (md.IsWebRequest())
            {   //UnityWebRequest下载
                loading = new ABHttpRequest(md);
            }

            if (root) loading.AddRefParent(abName);

            m_WaitingRequest.Add(abName, loading);
            mWaitingRequest.Insert(0, loading);

            //依赖关系
            string[] dependencies = AssetBundleLoader.GetDependencies(abName);
            for (int i = 0; i < dependencies.Length; ++i)
            {
                var abname = dependencies[i];

                //添加一次引用
                AssetBundleObject obj = AssetBundleCache.FindAssetBundle(abname);
                if (obj != null) { 
                    obj.AddRefParent(abName); continue; 
                }

                var newReq = CreateRequest(abname, false);
                newReq.AddRefParent(abName);
            }

            //启动加载
            StartLoopUpdate(); 
            return loading;
        }

        #endregion

        #region 异步处理逻辑

        /// <summary>
        /// 启动更新
        /// </summary>
        void StartLoopUpdate()
        {
            if (KeepRunning)
                return;

            KeepRunning = true; 

            StartCoroutine(OnLoopUpdate());
        }

        /// <summary>
        /// 更新处理
        /// </summary>
        IEnumerator OnLoopUpdate()
        {
            while (KeepRunning)
            {
                //处理加载中的
                int execTaskIndex = mProgressRequest.Count;
                while (execTaskIndex > 0)
                {
                    var curRequest = mProgressRequest[--execTaskIndex];
                    if (curRequest.IsLoading()) continue;

                    mProgressRequest.RemoveAt(execTaskIndex);

                    curRequest.OnAssetsLoaded();
                }

                //处理未加载的
                int waitingIndex = mWaitingRequest.Count;
                while (mProgressRequest.Count < MAX_PROGRESS && waitingIndex > 0)
                {
                    var curRequest = mWaitingRequest[--waitingIndex];

                    if (AssetBundleCache.IsAllDependicesReady(curRequest.abName))
                    {
                        if (!AssetBundleCache.IsExist(curRequest.abName))
                        {
                            curRequest.CreateRequest();

                            mProgressRequest.Add(curRequest);

                            mWaitingRequest.RemoveAt(waitingIndex);

						}
                        else
                        {
                            curRequest.OnAssetsLoaded();

                            mWaitingRequest.RemoveAt(waitingIndex);
                        }
                    }
                    else if (HasChildErrorOrCancel(curRequest.abName))
                    {
                        curRequest.OnAssetsLoaded();

                        mWaitingRequest.RemoveAt(waitingIndex);
					}
                }

                KeepRunning = mWaitingRequest.Count > 0 || mProgressRequest.Count > 0;

                if (KeepRunning) 
                    yield return null; 
                else 
                    yield break;
            }

        }

        #endregion

        #endregion

        private class LoadWaitingInfo
        {
            public string assetName;
            public string abName;
            public List<OnAssetLoadedCallback<UnityEngine.Object>> onLoadedCallback;
            public bool loadObject;
        }
    }
}