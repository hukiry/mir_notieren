using Hukiry;
using Hukiry.Http;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HukiryInitialize
{
    /// <summary>
    /// 版本记录比对
    /// </summary>
    public class VersionRecordComparison : IGameInitialize {
		/// <summary>
		/// 本地版本比对文件
		/// </summary>
		public static List<HotAssetInfo> localVersionRecord;

		/// <summary>
		/// 远程版本比对文件
		/// </summary>
		public static List<HotAssetInfo> serverVersionRecord;

		protected override IEnumerator Coroutines() {
			//如果需要更新：版本对比
			if (!GIController.isNeedUpdate) {
				FinishTask();
				yield break;
			}
			UpdateProgress(0f, "check_assets_update");
			yield return GetServerVersionRecord();

			UpdateProgress(0.5f, "check_assets_update");
			yield return GetLocalVersionRecord();

			UpdateProgress(1f, "check_assets_update");
			yield return CompareVersionRecord();

			FinishTask();
		}

		/// <summary>
		/// 第一步 获取到服务端版本记录文件清单
		/// </summary>
		/// <returns></returns>
		private IEnumerator GetServerVersionRecord() {
			bool loading = true;
			HttpDownload hd = new HttpDownload(AssetBundleConifg.AppTempCachePath, null, (_path, _relativePath, _msg, isSuccess) => {
				if (isSuccess) {
					string str = File.ReadAllText(Path.Combine(AssetBundleConifg.AppTempCachePath, AssetBundleConifg.HashmapFileName));
					serverVersionRecord = JsonUtil.ToObject<List<HotAssetInfo>>(str);
					loading = false;

					LogManager.Log("获取 服务端版本记录 文件成功，长度：{0}", serverVersionRecord.Count);
				} else {
					//下载失败回调
					FailureTask("载 服务端版本记录 文件", _msg, _path);
				}
			});
			hd.StartDownload(AssetsUrlPath, AssetBundleConifg.HashmapFileName);
			while (loading) {
				yield return new WaitForEndOfFrame();
			}
			yield return null;
		}

		/// <summary>
		/// 第二步 获取到本地版本记录文件清单
		/// </summary>
		/// <returns></returns>
		private IEnumerator GetLocalVersionRecord() {
			if (GIController.newPack) {
				//新包或首包，从包里取出
				string path = AssetBundleConifg.WWWPrefix + Path.Combine(AssetBundleConifg.PacketAbPath, AssetBundleConifg.HashmapFileName);
				HttpLocalRequest uwrGet = new HttpLocalRequest(path, (isSuccess, msg,_) => {
					if (isSuccess) {
						localVersionRecord = JsonUtil.ToObject<List<HotAssetInfo>>(msg);
						LogManager.Log("从StreamingAssets获取 版本记录 文件成功，长度：" + localVersionRecord.Count);
					} else {
						FailureTask("从StreamingAssets获取 版本记录 文件", path, msg);
					}
				});
				while (!uwrGet.isDone) {
					yield return null;
				}
			} else {
				//热更新过的包，且有缓存
				string localPath = Path.Combine(AssetBundleConifg.AppCachePath, AssetBundleConifg.HashmapFileName);
				if (File.Exists(localPath)) {
					string str = File.ReadAllText(localPath);
					localVersionRecord = JsonUtil.ToObject<List<HotAssetInfo>>(str);
					LogManager.Log("获取 本地版本记录 文件成功，长度：" + localVersionRecord.Count);
				} else {
					//热更新过的包，在本地缓存被删除时
					localVersionRecord = new List<HotAssetInfo>();
				}
			}
			yield return null;
		}

		/// <summary>
		/// 第三步 比较版本记录文件 找出需要更新的文件
		/// </summary>
		/// <returns></returns>
		private IEnumerator CompareVersionRecord() {

			bool ishasDownload = this.IsHaveDownload(out List<string> downloadedList);
			//先把本地的记录文件转成字典这样获取快一些
			Dictionary<string, HotAssetInfo> localDic = new Dictionary<string, HotAssetInfo>();
			localVersionRecord.ForEach(vr => {
				localDic[vr.ab] = vr;
			});

			//再循环服务端的记录，与本地比较
			serverVersionRecord.ForEach(vr => {
				if (ishasDownload && downloadedList.Contains(vr.GetAbFullPath()))//断续已经下载过
				{
					return;
				}

				if (localDic.ContainsKey(vr.ab)) {
					//旧文件对比：对比MD5
					if (vr.md5 != localDic[vr.ab].md5) {
						GIController.updateVersionRecord.Enqueue(new HttpQueueInfo { filePath = vr.GetAbFullPath(), size = vr.size });
					}
				} else {
					//新文件
					GIController.updateVersionRecord.Enqueue(new HttpQueueInfo { filePath = vr.GetAbFullPath(), size = vr.size });
				}
			});

			LogManager.Log("比较版本记录文件完成，需要更新数量：" + GIController.updateVersionRecord.Count);
			yield return null;
		}

		private bool IsHaveDownload(out List<string> downloaded)
		{
			string tempFile = Path.Combine(AssetBundleConifg.AppCachePath, AssetBundleConifg.TempDownload);
			string version = string.Empty;
			downloaded = new List<string>();
			if (File.Exists(tempFile))
			{
				string[] lines = File.ReadAllLines(tempFile);
				if (lines != null && lines.Length > 0)
				{
					version = lines[0];
					for (int i = 1; i < lines.Length; i++)
					{
						downloaded.Add(lines[i]);
					}
				}
			}
			return MainGame.gameVersion.version.Equals(version);
		}
	}
}