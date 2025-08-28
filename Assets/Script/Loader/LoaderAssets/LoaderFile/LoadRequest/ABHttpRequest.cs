using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Hukiry
{
    /// <summary>
    /// ab文件加载请求：内存流加载
    /// </summary>
    public class ABHttpRequest : BaseRequest
    {
        protected AssetBundleCreateRequest request;

        /// <summary>
        /// ab详细信息
        /// </summary>
        private HotAssetInfo manifestData;

        /// <summary>
        /// www加载是否失败
        /// </summary>
        private bool isWwwFailure;
        internal ABHttpRequest(HotAssetInfo manifestData)
        {
            this.abName = manifestData.ab;
            this.manifestData = manifestData;
        }

        public override bool IsLoading()
        {
            if (request != null && request.isDone)
            {
                if (!AssetBundleCache.IsExist(abName))
                {
                    AssetBundleList.ins.LogError(request.assetBundle == null, $"{abName}加载资源失败");

					assetBundle = request.assetBundle; return false;
                }
            }

            return !isWwwFailure;
        }

        public override void CreateRequest()
        {
            AssetBundleList.ins.LogError(AssetsLoaderMgr.LogLoadInfo, "启动加载请求" + manifestData.GetPath());

            AssetBundleLoader.Instance.StartCoroutine(OnLoadStream(AssetBundleConifg.WWWPrefix + manifestData.GetPath()));
        }

        IEnumerator OnLoadStream(string path) {
            UnityWebRequest uwr = UnityWebRequest.Get(path);
            uwr.downloadHandler = new DownloadHandlerBuffer();

            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError) {
                isWwwFailure = true;
                Debug.LogError("WWW加载资源失败, path:" + path + ",   error:" + uwr.error);
            } else {
				//解密
				byte[] data = uwr.downloadHandler.data;
                //float lastTime = Time.realtimeSinceStartup;
                //EncryptionStream.Decrypt(ref data);
                request = AssetBundle.LoadFromMemoryAsync(data);
            }
            uwr.Dispose();
        }
    }
}
