using Hukiry;
using System;
using System.Linq;

/// <summary>
/// 场景预加载管理
/// </summary>
public class SceneLoader : ISceneLoader {
	public Action<float> progressCallback;
	public Action finishCallback;
	protected string[] paths = new string[0];
	private bool cacheAssets = false;
	protected int doneCount = 0;

	/// <summary>
	/// 初始化预加载队列
	/// </summary>
	
	public void InitPreloadedQueue(string[] paths, bool cacheAssets = false) {
		this.paths = paths;
		this.cacheAssets = cacheAssets;
	}

	/// <summary>
	/// 开始预加载
	/// </summary>
	public void StartPreloaded() {
		paths.ToList().ForEach(path => {
			AssetsLoaderMgr.LoadAsync(path, LoadFinish);
		});
	}

	/// <summary>
	/// 是否全部完成
	/// </summary>
	/// <returns></returns>
	public bool IsDone() {
		return doneCount == this.paths.Length;
	}

	private void LoadFinish(string path, UnityEngine.Object prefab) {
		if (this.cacheAssets) {
			AssetsLoaderMgr.ToCacheAssets(path);
		}
		doneCount++;
		this.progressCallback?.Invoke((float)doneCount / this.paths.Length);
		if (doneCount == this.paths.Length) {
			this.finishCallback?.Invoke();
		}
	}

	/// <summary>
	/// 清理
	/// </summary>
	public void UnloadAll() {
		for (int i = 0; i < paths.Length; i++) {
			AssetsLoaderMgr.Unload(paths[i]);
		}
	}
}