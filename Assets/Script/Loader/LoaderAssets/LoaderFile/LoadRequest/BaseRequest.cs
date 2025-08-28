using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Hukiry
{

    /// <summary>
    /// 加载基类迭代
    /// </summary>
    public class BaseRequest : IEnumerator
    {
        //资源名称
        public string abName { get; protected set; }
        //加载是否完成
        protected bool m_IsDone;
        //请求错误
        protected string m_Error;
        protected HashSet<string> m_MissDependencies;
        /// <summary>
        /// 他人对自己的引用列表
        /// </summary>
        protected HashSet<string> m_RefParentList = new HashSet<string>();
        /// <summary>
        /// 加载列表
        /// </summary>
        protected List<BaseRequest> m_LoadingList;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error => m_Error;
        /// <summary>
        /// 加载的assetbunlde
        /// </summary>
        public AssetBundle assetBundle { get; protected set; }
        public object Current => null;
        public bool MoveNext()
        {
            return !IsDone();
        }

        public void Reset(){}

        ~BaseRequest()
        {
            if (m_RefParentList.Count > 0)
                AssetBundleList.ins.LogError($"~LoadRequest() 还有{abName}未释放");
        }

        /// <summary>
        /// 资源是否准备完成
        /// </summary>
        public bool IsReady => assetBundle != null;
        /// <summary>
        /// 是否加载完成
        /// </summary>
        /// <returns></returns>
        public virtual bool IsDone() => m_IsDone;
        /// <summary>
        /// 创建请求
        /// </summary>
        public virtual void CreateRequest(){}
        /// <summary>
        /// 是否加载中
        /// </summary>
        public virtual bool IsLoading() => true;
        /// <summary>
        /// 加载完成
        /// </summary>
        public virtual void OnAssetsLoaded()
        {
			//LogManager.LogError(AssetsLoaderMgr.LogLoadInfo, $"{abName}资源加载完成, refcount = {m_RefParentList.Count}");
            this.m_IsDone = true;
            //加载完未发现有人引用
            if (m_RefParentList.Count == 0) {
                if (assetBundle != null) {
                    assetBundle.Unload(false);
                }
            }
            else if (assetBundle != null)
            {
                AssetBundleObject obj = AssetBundleObject.Create(abName, assetBundle);
                foreach (string refname in m_RefParentList) {
                    obj.AddRefParent(refname);
                }
                AssetBundleCache.AddLoaded(obj);
            }
        }
        /// <summary>
        /// 获取引用父节点
        /// </summary>
        public HashSet<string> GetRefParents() => m_RefParentList;
        /// <summary>
        /// 添加他人的引用
        /// </summary>
        /// <param name="name"></param>
        public void AddRefParent(string abname)
        {
            this.m_RefParentList.Add(abname);
        }
        /// <summary>
        /// 移除引用
        /// </summary>
        /// <param name="abname"></param>
        public void DelReference(string abname)
        {
            if (m_RefParentList.Contains(abname))
            {
                this.m_RefParentList.Remove(abname);

                if (m_RefParentList.Count <= 0) this.DeleteSelf();
            }
        }
        /// <summary>
        /// 删除自己
        /// </summary>
        protected virtual void DeleteSelf()
        {
			//LogManager.LogError(AssetsLoaderMgr.LogLoadInfo, "从Loading列表中删除自己" + abName);
			LoaderAsyncAsset.RemoveLoading(abName);
            //释放掉依赖的
            string[] dependencies = AssetBundleLoader.GetDependencies(abName);
            for (int i = 0; i < dependencies.Length; ++i)
            {
                var onlyname = dependencies[i];
                //从正在Loading中释放
                var loading = LoaderAsyncAsset.FindLoading(onlyname);
                if (loading != null) {
                    loading.DelReference(this.abName);
                }
            }
        }

    }

}