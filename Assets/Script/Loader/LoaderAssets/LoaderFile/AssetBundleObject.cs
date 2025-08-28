using System.Collections.Generic;
using UnityEngine;

namespace Hukiry
{
	/// <summary>
	/// 资源信息
	/// </summary>
	public class AssetBundleObject : BaseRefObject
    {
        /// <summary>
        /// assetBundle的名字
        /// </summary>
        public string AssetName { get; private set; }

		/// <summary>
		/// true 加载完成自动释放ab  false由引用计数管理
		/// </summary>
		public bool isMem { get; private set; }

		/// <summary>
		/// 关联的AssetBundle
		/// </summary>
		public AssetBundle m_AssetBundle = null;

        /// <summary>
        /// 当前引用者列表
        /// </summary>
        protected List<string> m_RefParentList = new List<string>();

		/// <summary>
		/// 主实体，如果是单一资源
		/// </summary>
		public UnityEngine.Object mainAsset = null;

		/// <summary>
		/// 所有实体
		/// </summary>
		public UnityEngine.Object[] allAssets = null;

		/// <summary>
		/// 已加载的实体
		/// </summary>
		private Dictionary<string, UnityEngine.Object> assetsDic = new Dictionary<string, UnityEngine.Object>();

		public static AssetBundleObject Create(string name, AssetBundle bundle)
        {
            return new AssetBundleObject(name, bundle);
        }

        private AssetBundleObject(string name, AssetBundle bundle)
        {
            this.AssetName = name;

			this.m_AssetBundle = bundle;

            this.IsStreamed = bundle ? bundle.isStreamedSceneAssetBundle : false;

			HotAssetInfo mData = AssetBundleLoader.GetManifestData(name);
			if (mData == null)
			{
				this.isMem = false;
			}

		}

        /// <summary>
        /// 是否场景资源
        /// </summary>
        public bool IsStreamed { get; private set; }

		/// <summary>
		/// 是否是单一资源
		/// </summary>
		public bool IsSingle = true;

		/// <summary>
		/// 是否已经加载过
		/// </summary>
		public bool HasLoaded(string name) {
			if (IsSingle) {
				return mainAsset != null;
			}else {
				return assetsDic.ContainsKey(name);
			}
		}

		/// <summary>
		/// 是否已经加载过
		/// </summary>
		public bool HasLoaded() {
			return allAssets != null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T LoadAsset<T>(string name) where T : UnityEngine.Object
        {
#if UNITY_EDITOR && !ASSETBUNDLE_TEST
			if (allAssets == null|| allAssets.Length==0)
			{
				var path = EditorLoader.GetAssetPath(AssetName);
				//LogManager.Log(AssetsLoaderMgr.LogLoadInfo, "编辑器同步加载,path:" + path);

				mainAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
				allAssets = new Object[] { mainAsset };
				if (mainAsset == null)
				{
					allAssets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);
					mainAsset = allAssets!=null&& allAssets.Length>0 ?allAssets[0]:null;
				}
				
			}

			this.Record(); 
			return mainAsset as T;
#else
			if (IsSingle) {
				if (mainAsset == null) {
					mainAsset = m_AssetBundle.LoadAsset(this.GetFileName(name));
				}
				if (isMem) this.UnLoadAssetBundle(false);

				this.Record(); 
				return mainAsset as T;
			}else {
				string fileName = this.GetFileName(name);
				if (!assetsDic.ContainsKey(name)) {
					assetsDic[name] = m_AssetBundle.LoadAsset(this.GetFileName(name));
				}
				this.Record();
				return assetsDic[name] as T;
			}
#endif
		}

		/// <summary>
		/// 加载所有
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T[] LoadAllAssets<T>() where T : UnityEngine.Object
        {
            if (allAssets == null)
            {
#if UNITY_EDITOR && !ASSETBUNDLE_TEST
				var assetPath = EditorLoader.GetAssetPath(AssetName);
				allAssets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetPath);
#else
				allAssets = m_AssetBundle.LoadAllAssets<T>();
				if (isMem) 
				this.UnLoadAssetBundle(false);
#endif
			}
			this.Record(); return allAssets as T[];
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public UnityEngine.AssetBundleRequest LoadAsync<T>(string name)
        {
            return m_AssetBundle.LoadAssetAsync<T>(this.GetFileName(name));
        }

		/// <summary>
		/// 获取文件名
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private  string GetFileName(string path)
		{
			string file = path;
			if (file.LastIndexOf('.') >= 0)
				file = file.Substring(0, file.LastIndexOf('.'));
			if (file.LastIndexOf('\\') >= 0)
				file = file.Substring(file.LastIndexOf('\\') + 1);
			if (file.LastIndexOf('/') >= 0)
				file = file.Substring(file.LastIndexOf('/') + 1);
			return file;
		}

		/// <summary>
		/// 设置异步加载完的实体对象
		/// </summary>
		/// <param name="name"></param>
		/// <param name="asset"></param>
		public void SetAsyncLoad(string name, UnityEngine.Object asset) {
			if (IsSingle) {
				mainAsset = asset;
			} else {
				assetsDic[name] = asset;
			}
		}

		/// <summary>
		/// 异步加载所有
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public UnityEngine.AssetBundleRequest LoadAllAsync<T>()
        {
            return m_AssetBundle.LoadAllAssetsAsync<T>();
        }

		/// <summary>
		/// 设置异步加载完的实体对象
		/// </summary>
		/// <param name="name"></param>
		/// <param name="asset"></param>
		public void SetAllAsyncLoad(string name, UnityEngine.Object[] assets) {
			allAssets = assets;
		}

		/// <summary>
		/// 获取引用父节点
		/// </summary>
		/// <returns></returns>
		public List<string> GetRefParents()
        {
            return m_RefParentList;
        }

        /// <summary>
        /// 添加引用
        /// </summary>
        /// <param name="name"></param>
        public void AddRefParent(string name)
        {
			if (m_RefParentList.Contains(name)) {
				return;
			}
            m_RefParentList.Add(name);
        }

        /// <summary>
        /// 移除引用
        /// </summary>
        /// <param name="name"></param>
        public void DelReference(string abname)
        {
			//LogManager.Log(AssetsLoaderMgr.LogUnloadInfo,string.Format( "删除父节点引用{0}, self:{1}, 剩余引用数：{2}", abname, AssetName, (m_RefParentList.Count - 1)));

			if (m_RefParentList.Contains(abname))
            {
                this.m_RefParentList.Remove(abname);

                if (m_RefParentList.Count == 0) 
					this.DeleteSelf();
            }
        }

        /// <summary>
        /// 释放使用的
        /// </summary>
        public void UnLoadUnuseAssets()
        {
            List<string> unuseList = new List<string>();

            foreach (string refparent in m_RefParentList)
            {
                if (!AssetBundleCache.IsExist(refparent)) {
                    unuseList.Add(refparent);
                }
            }

            for (int i = 0; i < unuseList.Count; ++i)
            {
                this.DelReference(unuseList[i]);
            }
        }

		public override void Release()
		{
			m_ReferencedCount--;

			//LogManager.LogError(AssetsLoaderMgr.LogUnloadInfo, string.Format("Release释放资源，剩余计数：{0}, ab:{1}", m_ReferencedCount, AssetName));
			//移除自己
			if (m_ReferencedCount <= 0)
				DelReference(this.AssetName);
		}

        /// <summary>
        /// 释放AssetsBundle
        /// </summary>
        /// <param name="flag"></param>
        protected void UnLoadAssetBundle(bool flag)
        {
            if (m_AssetBundle != null) { m_AssetBundle.Unload(flag); }
        }

        //删除自己
        protected override void DeleteSelf()
        {
			//LogManager.LogError(AssetsLoaderMgr.LogUnloadInfo, "从缓存列表中删除自己" + AssetName);

			this.m_ReferencedCount = 0;

            this.UnLoadAssetBundle(true);

            AssetBundleCache.DelLoaded(AssetName);

            //移除引用的ab
            string[] dependencies = AssetBundleLoader.GetDependencies(AssetName);
            foreach (string onlyname in dependencies)
            {
                var assets = AssetBundleCache.FindAssetBundle(onlyname);
                if (assets != null) {
                    assets.DelReference(AssetName); //移除对自己的引用
                }
            }
        }
    }
}