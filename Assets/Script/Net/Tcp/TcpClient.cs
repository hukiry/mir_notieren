using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Hukiry.Socket
{
	/// <summary>
	/// Tcp客户端 2021/05/15
	/// 抽象类
	/// </summary>
	public class TcpClient : ISocektConnect
	{
		//网络连接者
		private Connection mConnectoin = new Connection();
		private int msgLength = 0;
		private byte[] msgbuff = new byte[Packet.MSG_MAX_SIZE];
		//接收队列
		private Queue mRecvQueue = Queue.Synchronized(new Queue());
		//延迟处理
		private Queue<DelayEventData> DelayQueueList = new Queue<DelayEventData>();

		#region 回调注册
		/// <summary>
		/// 连接成功回调
		/// </summary>
		private SocketConnectedDelegate OnSocketConnectedCallback;
		/// <summary>
		/// 网络发生错误回调
		/// </summary>
		private OnSocketError OnSocketPostErrorCallback;
		/// <summary>
		/// 接收到网络数据包
		/// </summary>
		private OnDataReceived OnRecvPacketCallback;

		private PlatformIPHandle GePlatformIPCallback;

		private LoomQueueOnMainThread QueueOnMainThreadCallback;

		void ISocektConnect.SetPlatformIPFunction(PlatformIPHandle callback)
		{
			this.GePlatformIPCallback = callback;
		}

		void ISocektConnect.SetOnSocketConnectedFunction(SocketConnectedDelegate callback)
		{
			this.OnSocketConnectedCallback = callback;
		}

		void ISocektConnect.SetOnSocketErrorFunction(OnSocketError callback)
		{
			this.OnSocketPostErrorCallback = callback;
		}

		void ISocektConnect.SetOnRecvPacketFunction(OnDataReceived callback)
		{
			this.OnRecvPacketCallback = callback;
		}

		void ISocektConnect.SetQueueOnMainThreadFunction(LoomQueueOnMainThread callback)
		{
			this.QueueOnMainThreadCallback = callback;
		}

		#endregion

		/// <summary>
		/// 网络连接成功, not main thread callback
		/// </summary>
		private void OnSocketConnected()
		{
			DelayQueueList.Enqueue(new DelayEventData(OnSocketConnected, new object[] { }));
		}

		private void OnSocketConnected(params object[] args)
		{
			int index = args.Length > 0 ? (int)args[0] : 0;

			if (OnSocketConnectedCallback != null) { OnSocketConnectedCallback(index); }
		}
		/// <summary>
		/// 网络发生错误, not main thread callback
		/// </summary>
		private void OnSocketError(NetError ret, SocketError code, string msg)
		{
			this.CloseConnection("网络发生错误, code:" + code);
			DelayQueueList.Enqueue(new DelayEventData(OnSocketPostError, new object[] { ret, code, msg }));
		}

		private void OnSocketPostError(params object[] args)
		{
			NetError error = (NetError)args[0];
			SocketError code = (SocketError)args[1];
			string msg = (string)args[2];

			Debug.LogError(string.Format("OnSocketPostError({0}, {1}, {2}", error, code, msg));

			if (OnSocketPostErrorCallback != null) { OnSocketPostErrorCallback(error, code, msg); }
		}

		/// <summary>
		/// 接收网络数据
		/// </summary>
		private void OnRecvPacketData(byte[] recv, int recvLength)
		{
			if (msgLength + recvLength <= Packet.MSG_MAX_SIZE)
			{
				Buffer.BlockCopy(recv, 0, msgbuff, msgLength, recvLength);//收到的数据添加到末尾
				msgLength += recvLength;

				while (msgLength >= Packet.GetPacketSize(msgbuff, msgLength))
				{
					Packet newPacket = Packet.Decode(msgbuff, msgLength);
					if (newPacket == null || newPacket == Packet.Empty)
					{
						Debug.LogError("接收到非法消息  cmd:未知");
						break;
					}
					else
					{
						mRecvQueue.Enqueue(newPacket);
					}

					int packLen = newPacket.Size + Packet.HEAD_LENGTH;
					msgLength = msgLength - packLen;//剩下的消息长度
					if (msgLength > 0)
					{
						Buffer.BlockCopy(msgbuff, packLen, msgbuff, 0, msgLength);//重新定位剩下的数据
					}
					else if (msgLength < 0)
					{
						Debug.LogError("msgLength not equip packet.size" + msgLength + ", " + newPacket.Size);
					}
				}
			}
			else
			{
				if (msgLength >= Packet.HEAD_LENGTH) //非法包丢掉
				{
					//body长度
					int msglength = BitConverter.ToInt16(msgbuff, 0);
					//协议号
					int cmdId = BitConverter.ToInt16(msgbuff, 2);
					Debug.LogError(string.Format("接收到非法消息 ,包体长度大于限定长度 cmd:{0}, bodylen:{1}", cmdId, msglength));
					msgLength -= msglength - recvLength;
				}
			}
		}

		public TcpClient()
		{
			this.mConnectoin.OnConnectedCallback = this.OnSocketConnected;
			this.mConnectoin.OnErrorCallback = this.OnSocketError;
			this.mConnectoin.OnRecvDataCallback = this.OnRecvPacketData;
			this.mConnectoin.QueueOnMainThreadCallback = this.QueueOnMainThreadCallback;
		}


		/// <summary>
		/// 打开网络连接
		/// </summary>
		void ISocektConnect.OpenConnection(string hostAddress, int port)
		{
			hostAddress = this.GePlatformIPCallback(hostAddress, port.ToString());
			if (mConnectoin.IsConnected())
			{
				this.CloseConnection("重新连接Socket时");
				//晚一帧时间连接
				this.QueueOnMainThreadCallback?.Invoke(UnityEngine.Time.deltaTime, () =>
				{
					this.mConnectoin.OpenConnection(hostAddress, port);
				});
			}
			else
			{
				this.mConnectoin.OpenConnection(hostAddress, port);
			}
		}


		public bool IsConnected() => mConnectoin.IsConnected();

		//发送
		void ISocektConnect.SendPacket(short cmd, byte[] body)
		{
			Packet packet = new Packet(cmd, body);//发送数据包
			this.SendPacket(packet);
		}
		/// <summary>
		/// 发送网络包
		/// </summary>
		public void SendPacket(Packet packet)
		{
			if (!IsConnected())
			{
				Debug.Log("the current is not send message-> cmd:" + packet.cmd);
				return;
			}
			mConnectoin.SendPacket(packet.ToBytes());
		}

		//每帧更新
		void ISocektConnect.OnLoopUpdate()
		{
			//安全事件
			while (DelayQueueList.Count > 0)
			{
				var delay = DelayQueueList.Dequeue();
				delay.handler(delay.paramlist);
			}

			int len = mRecvQueue.Count;
			//消息队列
			while (len > 0)
			{
				len--;
				Packet pk = mRecvQueue.Dequeue() as Packet;
				if (pk == null)
				{
					break;
				}
				OnRecvPacketCallback?.Invoke(pk.MsgBody, pk.cmd);
			}
		}

		/// <summary>
		///  关闭网络连接
		/// </summary>
		public void CloseConnection(string code, bool exception = false)
		{
			Debug.Log(code + " -- Close Connection! 原状态：" + this.mConnectoin.IsConnected());
			this.msgLength = 0;
			this.mRecvQueue.Clear();
			this.mConnectoin.CloseSocket();
			if (exception)
			{
				this.mConnectoin.ProcessTcpEvent(NetError.GAME_REACTIVATION, new Exception("Game reactivation!"));
			}
		}

		/// <summary>
		/// 应用程序退出
		/// </summary>
		void ISocektConnect.OnApplicationQuit()
		{
			this.CloseConnection("Application Quit");
		}
	}
}
