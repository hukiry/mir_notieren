using System;
using System.IO;

namespace Hukiry.Http
{
    //消息包
    public class HttpPacket
    {
        private const byte HEAD = 0x7c;

        //空包
        public static HttpPacket Empty = new HttpPacket(0, new byte[0]);

        //消息头大小
        public static short HEAD_LENGTH = 5;

        //包最大的大小
        public static int MSG_MAX_SIZE = 1024 * 1024;


        public HttpPacket(int type, byte[] msgData)
        {
            this.Type = type;

            this.mMsgData = msgData;
        }

        /**
         * 获取消息命令
         */
        public int Type { get; private set; }

        //消息体
        private byte[] mMsgData;

        /**
         * 获取数据
         */

        public byte[] GetData()
        {
            return mMsgData;
        }

        /**
         * 转换成字节码
         */

        public byte[] ToBytes()
        {
            //消息头+消息体
            byte[] msgdata = new byte[HEAD_LENGTH + mMsgData.Length];

            //加密
            byte[] flgbytes = { HttpPacket.HEAD };
            Buffer.BlockCopy(flgbytes, 0, msgdata, 0, flgbytes.Length);

            //消息cmd
            int cmd = System.Net.IPAddress.HostToNetworkOrder(Type);
            byte[] cmdbytes = BitConverter.GetBytes(cmd);
            Buffer.BlockCopy(cmdbytes, 0, msgdata, 1, cmdbytes.Length);

            //拷贝到消息体
            Buffer.BlockCopy(mMsgData, 0, msgdata, HEAD_LENGTH, mMsgData.Length);

            return msgdata;
        }

        /**
         * 加密消息包
         */

        public static HttpPacket Encode(int packType, byte[] msgData)
        {
            return new HttpPacket(packType, msgData);
        }

        /**
         * 消息包解密
         */

        public static HttpPacket Decode(byte[] msg)
        {
            //协议号
            int cmd = System.Net.IPAddress.HostToNetworkOrder(BitConverter.ToInt32(msg, 0));

            //消息体
            MemoryStream body = new MemoryStream(msg, HEAD_LENGTH, msg.Length - HEAD_LENGTH);
            if (body != null)
            {
                return new HttpPacket(cmd, body.ToArray());
            }
            return HttpPacket.Empty;
        }
    }
}
