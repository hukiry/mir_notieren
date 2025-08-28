using System;
using System.Net;
using System.Net.Sockets;

namespace Hukiry.Socket
{
    //网络事件
    public enum NetError
    {
        CONNECT_ERROR, SEND_ERROR, RECV_ERROR, REMOTE_CLOSE, GAME_REACTIVATION,
    }

    /// <summary>
    /// 网络客户端
    /// </summary>
    public class Connection
    {

        //连接成功回调
        public Action OnConnectedCallback;

        //网络发生错误
        public Action<NetError, SocketError, string> OnErrorCallback;

        //包体解析
        public Action<byte[], int> OnRecvDataCallback;
        public LoomQueueOnMainThread QueueOnMainThreadCallback;

        public const int BUFF_SIZE = 32 * 1024;
        private byte[] buffer = new byte[BUFF_SIZE];

        private System.Net.Sockets.Socket mSocket;

        public Connection() { }

        /// <summary>
        ///  网络是否连接
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return mSocket != null && mSocket.Connected;
        }

        /// <summary>
        /// 处理网络事件
        /// </summary>
        public void ProcessTcpEvent(NetError ret, Exception e)
        {
            SocketError code = SocketError.SocketError;
            if (e.GetType() == typeof(SocketException))
            {
                code = ((SocketException)e).SocketErrorCode;
            }

            //网络发生错误
            OnErrorCallback?.Invoke(ret, code, e.Message);
        }

        #region 打开网络连接

        /// <summary>
        ///  添加网络事件监听
        /// </summary>
        public void OpenConnection(string hostAddress, int port)
        {
            UnityEngine.Debug.Log("Open Connection host = " + hostAddress + " port = " + port);
            try
            {
                IPAddress[] address = Dns.GetHostAddresses(hostAddress);
                foreach (var info in address)
                {
                    UnityEngine.Debug.Log("查看连接地址" + info);
                }

                IPAddress mIpAddress = address[0];
                IPEndPoint mIpEndPoint = new IPEndPoint(mIpAddress, port);

                mSocket = new System.Net.Sockets.Socket(mIpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                mSocket.Blocking = false;
                mSocket.ReceiveTimeout = 3000;

                IAsyncResult result = mSocket.BeginConnect(mIpEndPoint, new AsyncCallback(OnConnectCallback), mSocket);
                //3秒后
                QueueOnMainThreadCallback?.Invoke(3, () =>
                {
                    if (!result.IsCompleted) { this.CloseSocket(); }
                });

            }
            catch (Exception e)
            {
                ProcessTcpEvent(NetError.CONNECT_ERROR, e);
            }
        }

        /// <summary>
        /// 连接完成
        /// </summary>
        void OnConnectCallback(IAsyncResult async)
        {
            try
            {
                System.Net.Sockets.Socket handler = (System.Net.Sockets.Socket)async.AsyncState;
                handler.EndConnect(async);

                if (mSocket == null)
                {
                    throw new SocketException((int)SocketError.TimedOut);
                }

                //设置timeout
                this.mSocket.SendTimeout = 30;
                this.mSocket.ReceiveTimeout = 30;

                //连接成功
                OnConnectedCallback?.Invoke();

                //开始监听
                mSocket.BeginReceive(buffer, 0, BUFF_SIZE, SocketFlags.None, new AsyncCallback(RecvCallback), null);
            }
            catch (Exception e)
            {
                ProcessTcpEvent(NetError.CONNECT_ERROR, e);
            }
        }

        #endregion

        /// <summary>
        /// 关闭网络
        /// </summary>
        public void CloseSocket()
        {
            try
            {
                if (mSocket != null)
                {
                    if (mSocket.Connected)
                    {
                        mSocket.Shutdown(SocketShutdown.Both);
                    }

                    mSocket.Close();
                    mSocket = null;
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning(e);
            }
        }

        #region 发送网络数据

        /// <summary>
        /// 发送消息包
        /// </summary>
        public void SendPacket(byte[] msgdata)
        {
            //网络还未连接,提示警告
            if (!IsConnected()) { UnityEngine.Debug.LogWarning("conection does't work now!"); return; }

            mSocket.BeginSend(msgdata, 0, msgdata.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
        }

        /// <summary>
        /// 发送等待
        /// </summary>
        void SendCallback(IAsyncResult async)
        {
            try
            {
                mSocket.EndSend(async);
            }
            catch (Exception e)
            {
                ProcessTcpEvent(NetError.SEND_ERROR, e);
            }
        }

        #endregion

        #region 接收网络数据

        //远程服务器已经关闭
        private Exception RemoteException = new Exception("Remote server is closed!");

        /// <summary>
        /// 接收等待
        /// </summary>
        void RecvCallback(IAsyncResult async)
        {
            if (mSocket == null || !mSocket.Connected) return;
            try
            {
                int recvLength = mSocket.EndReceive(async);
                if (recvLength > 0)
                {
                    //解析数据包
                     OnRecvDataCallback?.Invoke(buffer, recvLength);
                }
                else if (recvLength < 0)
                {
                    ProcessTcpEvent(NetError.REMOTE_CLOSE, RemoteException); return;
                }

                mSocket.BeginReceive(buffer, 0, BUFF_SIZE, SocketFlags.None, new AsyncCallback(RecvCallback), null);
            }
            catch (Exception e)
            {
                ProcessTcpEvent(NetError.RECV_ERROR, e);
            }
        }

        #endregion
    }
}
