using Hukiry.Http;
using System.Collections;
using System.IO;
using UnityEngine;

using PathConifg = Hukiry.AssetBundleConifg;

namespace HukiryInitialize
{
    /// <summary>
    /// 版本比对
    /// </summary>
    public class VersionComparison : IGameInitialize
	{
		GameVersion cacheVersion = null;
		/// <summary>
		/// 包内版本：不是缓存版本
		/// </summary>
		private int appVervsion = 0;
		private bool isCancel;

		//private bool isCancel = false;
		protected override IEnumerator Coroutines()
		{
			UpdateProgress(0f, "check_assets_update");
			//加载版本号
			yield return LoadLocalVersions();

#if !UNITY_EDITOR || HOTUPDATE_TEST
			UpdateProgress(1f, "check_assets_update");
			yield return DownloadServerVersions(); 
#endif
			FinishTask();
		}

		/// <summary>
		/// 第一步 先比对本地包版本号
		/// </summary>
		/// <returns></returns>
		private IEnumerator LoadLocalVersions()
		{
			//先拿出随包版本号
			GameVersion packInfo = MainGame.gameVersion;
			this.appVervsion = MainGame.gameVersion.appVersion;
			//再拿出缓存版号
			GameVersion cacheInfo = null;
			string localPath = Path.Combine(PathConifg.AppCachePath, PathConifg.VersionName);
			if (File.Exists(localPath))
			{
				string str = File.ReadAllText(localPath);
				cacheInfo = JsonUtil.ToObject<GameVersion>(str);
				LogManager.Log("从本地缓存获取版本号文件成功，版本号：" + cacheInfo.version);
				yield return null;
			}

			if (cacheInfo != null)
			{
				//覆盖安装新包后，删除旧的缓存数据
				if (GameVersion.IsUpdatePackVersion(packInfo.version, cacheInfo.version))
				{
					//如果包里的大于缓存的，为新包
					GIController.newPack = true;
					FileHelper.DeleteDirectory(PathConifg.CacheAbPath);//删除本地资源缓存文件
					LogManager.Log("为新包，删除本地abfile缓存文件(保留玩家设置)");
					cacheVersion = packInfo;
				}
				else
				{
					cacheVersion = cacheInfo;
				}
			}
			else
			{
				//无缓存，为首包
				GIController.newPack = true;
				cacheVersion = packInfo;
			}
			//取本地缓存的数据
			MainGame.gameVersion = cacheVersion;
		}


		/// <summary>
		/// 第二步 从服务端下载版本号
		/// </summary>
		/// <returns></returns>
		private IEnumerator DownloadServerVersions()
		{
			bool loadFinish = false;
			HttpDownload hd = new HttpDownload(PathConifg.AppTempCachePath, null, (_path, _relativePath, _msg, isSuccess) =>
			{
				if (isSuccess)
				{
					string str = File.ReadAllText(Path.Combine(PathConifg.AppTempCachePath, PathConifg.VersionName));
					LogManager.Log("从服务端下载版本号：", str, PathConifg.AppTempCachePath, PathConifg.VersionName);
					GIController.serverVersions = JsonUtil.ToObjectUnity<GameVersion>(str);
					loadFinish = true;
				}
				else
				{
					FailureTask("下载 服务端版本 文件", _msg, _path);
				}
			});
			//获取服务器的数据
			hd.StartDownload(AssetsUrlPathVersion, PathConifg.VersionName);
			while (!loadFinish)
			{
				yield return new WaitForEndOfFrame();
			}

			GIController.isAppUpdate  = GameVersion.IsUpdateApp(this.appVervsion, GIController.serverVersions.appVersion);
			//中版本号上升 或 大版本号上升  强更包：重启动游戏时发现有新包更新
			if (GameVersion.IsUpdatePackVersion(GIController.serverVersions?.version, cacheVersion?.version))
			{
				//isCancel = false;
				GIController.ShowPanelMsgBox?.Invoke(3, () =>
				{
					Application.OpenURL(GIController.serverVersions.strongerUrl);
					Application.Quit();
				}, () =>
				{
					//取消更新包，继续游戏，不更新资源
					GIController.serverVersions = cacheVersion;
					this.isCancel = true;
				});

				while (true)
				{
					if (this.isCancel)
					{
						yield break;
					}
					else
					{
						yield return new WaitForEndOfFrame();
					}
				}
			}
			else
			{
				GIController.isNeedUpdate = GameVersion.IsUpdateResourceVersion(GIController.serverVersions?.version, cacheVersion?.version);
			}


			//取服务器的数据
			MainGame.gameVersion = GIController.serverVersions;
			LogManager.Log("从服务端下载版本号：", GIController.serverVersions.version, " 是否需要更新：", GIController.isNeedUpdate);
			yield return null;
		}
	}
}