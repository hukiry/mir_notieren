using System.Collections.Generic;
using System.Linq;

namespace Hukiry
{
    /// <summary>
    /// 资源缓存管理:统计数据
    /// </summary>
    public class AssetBundleCache
    {
        /// <summary>
        /// 已经加载完的缓存列表
        /// </summary>
        static Dictionary<string, AssetBundleObject> mLoadedList = new Dictionary<string, AssetBundleObject>();

        /// <summary>
        /// 添加加载完成的资源
        /// </summary>
        /// <param name="assets"></param>
        internal static void AddLoaded(AssetBundleObject holder)
        {
            string assetName = holder.AssetName;

            if (mLoadedList.ContainsKey(assetName))
            {
                AssetBundleList.ins.LogError("已经存在缓存列表中，请检查逻辑" + assetName); return;
            }
          
            mLoadedList.Add(assetName, holder);
        }

        /// <summary>
        /// 从缓存中删除
        /// </summary>
        /// <param name="name"></param>
        internal static void DelLoaded(string name)
        {
            mLoadedList.Remove(name);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static bool IsExist(string name)
        {
            return mLoadedList.ContainsKey(name);
        }

        /// <summary>
        /// 节点依赖是否所有的都已经完毕
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static bool IsAllDependicesReady(string name)
        {
            string[] dependencies = AssetBundleLoader.GetDependencies(name);

            for (int i = 0; i < dependencies.Length; ++i)
            {
                if (!IsAllDependicesReady(dependencies[i]))
                    return false;

                if (!mLoadedList.ContainsKey(dependencies[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 获取缓存文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static AssetBundleObject FindAssetBundle(string name)
        {
			AssetBundleObject assets = null;
            mLoadedList.TryGetValue(name, out assets); return assets;
        }

        #region

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="path">资源目录</param>
        /// <param name="time">释放时间</param>
        internal static void UnloadAsset(string name)
        {
            AssetBundleObject obj;
            if (mLoadedList.TryGetValue(name, out obj))
            {
                obj.Release(); 
            }
        }

        /// <summary>
        /// 立即释放
        /// </summary>
        /// <param name="path"></param>
        internal static void UnloadImmediate(string name)
        {
            AssetBundleObject obj;
            if (mLoadedList.TryGetValue(name, out obj)) {
                obj.DelReference(name);
            }
        }

        /// <summary>
        /// 释放未使用资源
        /// </summary>
        /// <param name="gc">是否调用gc</param>
        internal static void UnloadUnuseAsset(bool gc)
        {
            List<string> removeList = mLoadedList.Keys.ToList<string>();
            for (int i = 0; i < removeList.Count; ++i)
            {
                mLoadedList[removeList[i]].UnLoadUnuseAssets();
            }
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        internal static void UnloadAllAsset()
        {
            List<string> removeList = new List<string>();
            foreach (var iter in mLoadedList) {
                removeList.Add(iter.Key);
            }

            //移除并做释放
            for (int i = 0; i < removeList.Count; ++i) {
                string refname = removeList[i];

                if (mLoadedList.ContainsKey(refname))
                {
                    mLoadedList[refname].UnLoadUnuseAssets();
                    UnloadImmediate(refname);
                }
            }

			//未移除列表
			foreach (var iter in mLoadedList) {
                AssetBundleList.ins.LogError(string.Format("还有资源未移除 name = {0}, 引用次数 = {1}", iter.Key, iter.Value.ReferencedCount));
			}
		}

        /// <summary>
        /// 打印未释放资源
        /// </summary>
        internal static void LogUnloadAllAsset() {
			int count = 0;
			foreach(KeyValuePair<string, AssetBundleObject> kv in mLoadedList) {
				if (AssetsLoaderMgr.CacheMarkList.Contains(kv.Key)) {
					continue;
				}
				if (kv.Key == "ui/prefab/login/loginpanel" || kv.Key == "ui/prefab/loader/sceneloading") {
					continue;
				}
				string str = string.Format("引用数：{0}, 使用次数：{1}, 资源：{2}", kv.Value.GetRefParents().Count, kv.Value.ReferencedCount, kv.Key);
				for (int i = 0; i < kv.Value.GetRefParents().Count; i++) {
					str += "\n引用者：" + kv.Value.GetRefParents()[i];
				}
                AssetBundleList.ins.Log(str);
				count++;
			}
            AssetBundleList.ins.Log((object)"未释放资源数量：{0}", count);
		}

        #endregion
    }
}
