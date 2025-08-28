

using System;

namespace Hukiry.Socket
{
    /// <summary>
    /// 字节块流
    /// </summary>
    public class ByteBlock
    {
        private int m_length;
        private byte[] m_buffer;
        private int m_position;

        /// <summary>
        ///  构造函数 
        /// </summary>
        /// <param name="byteSize">Socket.Packet.MSG_MAX_SIZE</param>
        public ByteBlock(int byteSize = 1024 * 32)
        {
            this.m_buffer = new byte[byteSize];
        }
        /// <summary>
        /// 字节实例
        /// </summary>
        public byte[] Buffer => this.m_buffer;
        /// <summary>
        /// 真实长度
        /// </summary>
        public int Length => this.m_length;

        /// <summary>
        /// int型流位置
        /// </summary>
        public int Pos
        {
            get => (int)this.m_position;
            set => this.m_position = value;
        }

        /// <summary>
        /// 重新设置容量
        /// </summary>
        /// <param name="size">新尺寸</param>
        private void SetCapacity(int size)
        {
            byte[] newBytes = new byte[size];
            Array.Copy(this.m_buffer, 0, newBytes, 0, this.m_buffer.Length);
            this.m_buffer = newBytes;
        }

        /// <summary>
        /// 从指定位置转化到指定长度的有效内存
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] ToArray(int offset, int length)
        {
            byte[] buffer = new byte[length];
            Array.Copy(this.m_buffer, offset, buffer, 0, buffer.Length);
            return buffer;
        }
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void Write(byte[] buffer, int offset, int count)
        {
            if (count == 0)
            {
                return;
            }

            if (this.m_buffer.Length - this.m_position < count)
            {
                int need = this.m_buffer.Length + count;
                this.SetCapacity(need);
            }
            Array.Copy(buffer, offset, this.m_buffer, this.m_position, count);
            this.m_position += count;
            this.m_length = Math.Max(this.m_position, this.Length);
        }
        public void Write(byte[] buffer)
        {
            this.Write(buffer, 0, buffer.Length);
        }

    }
}