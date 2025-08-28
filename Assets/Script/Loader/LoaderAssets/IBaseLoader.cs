using System;

namespace Hukiry
{
    /// <summary>
    /// 资源加载接口
    /// </summary>
    public interface IBaseLoader
    {
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <returns>资源对象</returns>
        UnityEngine.Object LoadAsset(string assetName, string abName);

        /// <summary>
        /// 同步加载列表资源
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <returns></returns>
        object[] LoadAllAsset(string assetName, string abName);

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <param name="callback">加载完成回调</param>
        void LoadAsync(string assetName, string abName, OnAssetLoadedCallback<UnityEngine.Object> OnLoadedCallback, bool loadObject);

        /// <summary>
        /// 异步加载资源列表
        /// </summary>
        /// <param name="assetName">资源路径</param>
        /// <param name="abName">ab路径</param>
        /// <param name="OnLoadedCallback"></param>
        void LoadAllAsync(string assetName, string abName, OnAssetListLoadedCallback<UnityEngine.Object> OnLoadedCallback);

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="levelName"></param>
        /// <param name="onProgress"></param>
        void LoadSceneAsync(string levelName, string abName, Action<float> onProgress, Action onFinish);

        /// <summary>
        /// 卸载未使用的资源
        /// </summary>
        void UnloadUnusedAssets(bool gc);

        /// <summary>
        /// 清理所有资源
        /// </summary>
        void UnLoadAllAssets();
    }
}
