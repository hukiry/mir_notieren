using System.Net.Sockets;

namespace Hukiry.Socket
{
    public delegate void SocketConnectedDelegate(int index);
	/// <summary>
	/// 网络错误
	/// </summary>
	public delegate void OnSocketError(NetError ret, SocketError code, string msg);
	/// <summary>
	/// 网络数据接收
	/// </summary>
	public delegate void OnDataReceived(byte[] data, int length);

	public delegate string PlatformIPHandle(string hostAddress, string port);

	public delegate void LoomQueueOnMainThread(float time, System.Action action);

	public interface ISocektConnect
	{
		/// <summary>
		/// 设置连接成功回调
		/// </summary>
		void SetOnSocketConnectedFunction(SocketConnectedDelegate callback);
		/// <summary>
		/// 设置网络发生错误回调
		/// </summary>
		void SetOnSocketErrorFunction(OnSocketError callback);
		/// <summary>
		/// 设置接收到网络数据包
		/// </summary>
		void SetOnRecvPacketFunction(OnDataReceived callback);
		/// <summary>
		/// 设置平台IP获取
		/// </summary>
		/// <param name="callback"></param>
		void SetPlatformIPFunction(PlatformIPHandle callback);
		/// <summary>
		/// Loom.QueueOnMainThread()
		/// </summary>
		/// <param name="callback"></param>
		void SetQueueOnMainThreadFunction(LoomQueueOnMainThread callback);

		/// <summary>
		/// 发生数据包
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="body"></param>
		void SendPacket(short cmd, byte[] body);
		/// <summary>
		/// 开始连接
		/// </summary>
		/// <param name="hostAddress"></param>
		/// <param name="port"></param>
		void OpenConnection(string hostAddress, int port);
		/// <summary>
		/// 关闭链接
		/// </summary>
		/// <param name="code"></param>
		/// <param name="exception"></param>
		void CloseConnection(string code, bool exception = false);

		/// <summary>
		/// 是否链接成功
		/// </summary>
		/// <returns></returns>
		bool IsConnected();

		/// <summary>
		/// 每帧更新
		/// </summary>
		void OnLoopUpdate();
		/// <summary>
		/// 退出
		/// </summary>
		void OnApplicationQuit();
	}

	public struct DelayEventData
	{
		public delegate void EventHandler(params object[] args);

		public DelayEventData(EventHandler h, object[] l)
		{
			handler = h;
			paramlist = l;
		}

		public EventHandler handler;

		public object[] paramlist;
	}

	[System.Serializable]
	public enum NetState
	{
		/// <summary>
		/// 单机游戏
		/// </summary>
		SingleGame,
		/// <summary>
		/// 网络游戏
		/// </summary>
		NetworkGame,
		/// <summary>
		/// HttpGame
		/// </summary>
		HttpGame
	}
}
