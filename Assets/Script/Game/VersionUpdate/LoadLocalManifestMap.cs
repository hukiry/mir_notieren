
using Hukiry;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace HukiryInitialize
{
    /// <summary>
    /// 加载本地记录的资源引用关系
    /// </summary>
    public class LoadLocalManifestMap : IGameInitialize {
		protected override IEnumerator Coroutines() {
			string path = Path.Combine(AssetBundleConifg.AppCachePath, AssetBundleConifg.HashmapFileName);
			if (File.Exists(path)) {
				string str = File.ReadAllText(path);
                AssetBundleList.ins.RecoverManifestDataFromLocal(str);
            }
			else {
				path = AssetBundleConifg.WWWPrefix + Path.Combine(AssetBundleConifg.PacketAbPath, AssetBundleConifg.HashmapFileName);
				using (UnityWebRequest uwr = new UnityWebRequest(path)) {
					uwr.downloadHandler = new DownloadHandlerBuffer();
					yield return uwr.SendWebRequest();
					if (uwr.isNetworkError || uwr.isHttpError) {
						FailureTask("获取 资源引用关系 文件", uwr.url, uwr.error);
						yield return null;
					} else {
						string text = uwr.downloadHandler.text;
                        AssetBundleList.ins.RecoverManifestDataFromLocal(text);
					}
					uwr.Dispose();
				}
			}
			LogManager.Log("获取 资源引用关系 文件成功 Path:" + path);
			FinishTask();
		}
	}
}