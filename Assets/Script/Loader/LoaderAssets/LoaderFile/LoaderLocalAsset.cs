using UnityEngine;

namespace Hukiry
{
    /// <summary>
    /// 资源同步加载
    /// </summary>
    public class LoaderLocalAsset
    {

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <returns></returns>
        internal static UnityEngine.Object LoadAssets(string assetName, string abName)
        {
            AssetBundleObject objInfo = LoadAssetHolder(abName);

            if (objInfo != null)
            {
                objInfo.AddRefParent(objInfo.AssetName);
                var obj = objInfo.LoadAsset<UnityEngine.Object>(assetName);
                if (obj == null)
                {
                    AssetBundleList.ins.LogError(AssetsLoaderMgr.LogLoadInfo, "同步加载资源失败:" + assetName);
                }
                return obj;
            }
            AssetBundleList.ins.LogError("red", $"同步加载资源{assetName}失败!");

            //释放已经加载的
            AssetBundleLoader.FailReleaseLoaded(assetName, abName); return null;
        }

        /// <summary>
        /// 同步加载资源列表
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <returns></returns>
        internal static UnityEngine.Object[] LoadAssetsList(string assetName, string abName)
        {
            //从缓存中查找
            AssetBundleObject obj = LoadAssetHolder(abName);

            //资源加载失败
            if (obj != null)
            {
                obj.AddRefParent(obj.AssetName);
                AssetBundleList.ins.LogColor("red", AssetsLoaderMgr.LogLoadInfo, "同步加载资源列表完成" + assetName);
                return obj.LoadAllAssets<UnityEngine.Object>();
            }

            AssetBundleList.ins.LogColor("red", $"同步加载资源{assetName}失败!");

            //释放已经加载的
            AssetBundleLoader.FailReleaseLoaded(assetName, abName); 
            
            return null;
        }

        /// <summary>
        /// 递归同步加载子节点
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private static AssetBundleObject LoadAssetHolder(string abName)
        {
            HotAssetInfo mData = AssetBundleLoader.GetManifestData(abName);
            if (mData != null)
            {
                AssetBundleObject obj = AssetBundleCache.FindAssetBundle(abName);
                if (obj == null)
                {
                    //先加载依赖节点
                    for (int i = 0; i < mData.ds.Count; ++i)
                    {
                        var dsAB = AssetBundleCache.FindAssetBundle(abName);

                        if (dsAB == null)
                        {
                            var child = LoadAssetHolder(mData.ds[i]);
                            if (child == null)
                                return null;
                            child.AddRefParent(abName);
                        }
                    }

                    string path = mData.GetPath();

                    AssetBundleList.ins.Log(nameof(LoadAssetHolder), "递归同步加载, path:" + path);
                    //后加载主资源
                    AssetBundle bundle = AssetBundle.LoadFromFile(path);
                    if (bundle != null)
                    {
                        obj = AssetBundleObject.Create(abName, bundle);
                        AssetBundleCache.AddLoaded(obj);
                        return obj;
                    }

                }
                return obj;
            }
            return null;
        }
    }
}
