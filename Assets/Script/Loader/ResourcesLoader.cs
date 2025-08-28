using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hukiry {

    public class ResourcesLoader {

        public static void LoadAsync(string assetName, OnAssetLoadedCallback<UnityEngine.Object> OnLoadedCallback) {
            AssetBundleLoader.Instance.StartCoroutine(LoadAssetAsync(assetName, OnLoadedCallback));
        }

        //加载资源
        static IEnumerator LoadAssetAsync(string assetName, OnAssetLoadedCallback<UnityEngine.Object> onLoadedCallback) {
            ResourceRequest rr = Resources.LoadAsync(assetName);
            while (!rr.isDone) {
                yield return new WaitForEndOfFrame();
            }
            onLoadedCallback(assetName, rr.asset);
            yield return null;
        }

        public static Object LoadAsset(string assetName)
        {
           return Resources.Load(assetName);
        }

        public static void Unload(UnityEngine.Object obj)
        {
            Resources.UnloadAsset(obj);
        }
    }
}
