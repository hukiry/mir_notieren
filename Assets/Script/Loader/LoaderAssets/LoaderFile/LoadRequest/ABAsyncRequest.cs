using UnityEngine;

namespace Hukiry
{
    /// <summary>
    /// ab文件Async请求
    /// </summary>
    public class ABAsyncRequest : BaseRequest
    {
        protected AssetBundleCreateRequest request;

        internal ABAsyncRequest(string abName)
        {
            this.abName = abName;
        }

        public override bool IsLoading()
        {
            if (request != null && request.isDone)
            {
                if (!AssetBundleCache.IsExist(abName))
                {
                    //LogManager.LogError(request.assetBundle == null, $"{abName}加载资源失败");
					assetBundle = request.assetBundle; 
                    return false;
                }
            }
            return true;
        }

        public override void CreateRequest()
        {
            string path = AssetBundleLoader.FindPath(abName);
            //LogManager.Log("===========================启动加载请求:" + path, abName);
            request = AssetBundle.LoadFromFileAsync(path);
        }
    }
}
