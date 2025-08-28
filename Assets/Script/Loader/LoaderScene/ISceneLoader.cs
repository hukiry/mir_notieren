

public interface ISceneLoader {

	/// <summary>
	/// 开始预加载
	/// </summary>
	void StartPreloaded();

	/// <summary>
	/// 是否全部完成
	/// </summary>
	/// <returns></returns>
	bool IsDone();
}