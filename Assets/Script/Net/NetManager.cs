using UnityEngine;
// 工作模式
public enum EWorkMode
{
    /// <summary>
    /// 无热更:本地白包 =内网测试服
    /// </summary>
    Debug = 0,
    /// <summary>
    /// 开发版 热更  =外网测试服
    /// </summary>
    Develop = 1,   //
    /// <summary>
    /// 正式版 热更  =正式测试服
    /// </summary>
    Release = 2,
}

namespace Hukiry.Socket
{
    public class NetManager : MonoBehaviour
    {
        public static NetManager ins;
        private ISocektConnect tcpClient = null;

        private void Awake()
        {
            ins = this;

#if ENABLE_SOCKET
            tcpClient = new TcpClient();
        }

        private void Start()
        {
            if (ins == null) ins = this;
            if (tcpClient == null) tcpClient = new TcpClient();

            this.SetQueueOnMainThread(Loom.QueueOnMainThread);
            this.SetPlatformIPFunction(SDK.SdkManager.ins.GetPlatformIP);
        }

        private void Update()
        {
            tcpClient?.OnLoopUpdate();
        }


        private void OnApplicationQuit()
        {
            tcpClient?.OnApplicationQuit();
        }
#else
        }
#endif
        /// <summary>
        /// 注册网络连接回调
        /// </summary>
        public void SetOnSocketConnectedFunction(SocketConnectedDelegate callback) => tcpClient?.SetOnSocketConnectedFunction(callback);

        /// <summary>
        /// 注册网络错误
        /// </summary>
        public void SetOnSocketErrorFunction(OnSocketError callback) => tcpClient?.SetOnSocketErrorFunction(callback);
        /// <summary>
        /// 注册消息接受
        /// </summary>
        public void SetOnRecvPacketFunction(OnDataReceived callback) => tcpClient?.SetOnRecvPacketFunction(callback);
        /// <summary>
        /// 设置SDK函数注册
        /// </summary>
        public void SetPlatformIPFunction(PlatformIPHandle callback) => tcpClient?.SetPlatformIPFunction(callback);

        /// <summary>
        /// 设置 Loom 
        /// </summary>
        /// <param name="callback"></param>
        public void SetQueueOnMainThread(LoomQueueOnMainThread callback) => tcpClient?.SetQueueOnMainThreadFunction(callback);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="body"></param>
        public void SendPacket(short cmd, byte[] body) => tcpClient?.SendPacket(cmd, body);

        /// <summary>
        /// 打开ip地址
        /// </summary>
        /// <param name="hostAddress"></param>
        /// <param name="port"></param>
        public void OpenConnection(string hostAddress, int port) => tcpClient?.OpenConnection(hostAddress, port);

        /// <summary>
        /// 关闭网络
        /// </summary>
        /// <param name="code"></param>
        /// <param name="exception"></param>
        public void CloseConnection(string code, bool exception = false) => tcpClient?.CloseConnection(code, exception);

        /// <summary>
        /// 是否连接成功
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            if (tcpClient == null) return false;
            return tcpClient.IsConnected();
        }
    }
}