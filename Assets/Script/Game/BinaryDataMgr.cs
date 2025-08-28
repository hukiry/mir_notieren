using Hukiry.Socket;
using System;
using System.IO;

namespace Hukiry
{
    public class BinaryDataMgr
    {
        private const string EXTEND_BIN = ".bin";
        private const string EXTEND_TXT = ".txt";
        private const string Dir_Name = "GameBinary";
        public static BinaryDataMgr instance { get; } = new BinaryDataMgr();
        private string GetFullPath(string fileName)
        {
            string dirPath = Path.Combine(AssetBundleConifg.AppCachePath, Dir_Name);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

            if (fileName.Contains("."))
            {
                fileName = fileName.Split('.')[0];
            }
            return Path.Combine(dirPath, fileName);
        }
        /// <summary>
        /// 删除游戏本地缓存-编辑器使用
        /// </summary>
        [LuaInterface.NoToLua]
        public void DeleteGameCache()
        {
            string dirPath = Path.Combine(AssetBundleConifg.AppCachePath, Dir_Name);
            Directory.Delete(dirPath, true);
        }

        //#region 发送和接收
        ////发送数据时处理
        //public byte[] DealBlock(Func<ByteBlock, byte[]> actionBack)
        //{
        //    ByteBlock byteBlock = new ByteBlock(Socket.Packet.MSG_MAX_SIZE);

        //    return actionBack(byteBlock);

        //}
        ////接收数据时处理
        //public void ReceiveDealBlock(byte[] buffer, Action<ByteBlock> actionBack)
        //{
        //    ByteBlock byteBlock = new ByteBlock(Socket.Packet.MSG_MAX_SIZE);

        //    byteBlock.Write(buffer);
        //    byteBlock.Pos = 0;
        //    actionBack(byteBlock);

        //}
        //#endregion

        #region 读写二进制
        /// <summary>
        /// 读取二进制
        /// </summary>
        /// <param name="fileName">文件名，不包含后缀</param>
        /// <returns>ByteBlock <see cref="Hukiry.Byte.ByteBlock"/></returns>
        public void ReadBinary(string fileName, Action<ByteBlock> actionBack)
        {
            string filePath = this.GetFullPath(fileName) + EXTEND_BIN;
            if (File.Exists(filePath))
            {
                ByteBlock byteBlock = new ByteBlock(Socket.Packet.MSG_MAX_SIZE);

                byteBlock.Write(Hukiry.HukiryUtil.ReadAllBytes(filePath));
                byteBlock.Pos = 0;
                actionBack?.Invoke(byteBlock);

            }
        }

        /// <summary>
        /// 保存二进制
        /// </summary>
        /// <param name="fileName">文件名，不包含后缀</param>
        /// <param name="byteBlock">字节处理 <see cref="Hukiry.Byte.ByteBlock"/></param>
        public void SaveBinary(string fileName, Action<ByteBlock> actionBack)
        {
            ByteBlock byteBlock = new ByteBlock(Socket.Packet.MSG_MAX_SIZE);

            actionBack?.Invoke(byteBlock);
            string filePath = this.GetFullPath(fileName) + EXTEND_BIN;
            Hukiry.HukiryUtil.WriteAllBytes(filePath, byteBlock.ToArray());

        }

        #endregion

        #region 读写文本
        public void SaveText(string fileName, string content)
        {
            string filePath = this.GetFullPath(fileName) + EXTEND_TXT;
            byte[] buffer = HukiryUtil.CodeByte(content, true);
            File.WriteAllBytes(filePath, buffer);
        }

        public void ReadText(string fileName, Action<string> actionBack)
        {
            string filePath = this.GetFullPath(fileName) + EXTEND_TXT;
            if (File.Exists(filePath))
            {
                byte[] buffer = Hukiry.HukiryUtil.ReadAllBytes(filePath);
                string content = HukiryUtil.DeCodeByte(buffer, true);
                actionBack?.Invoke(content);
            }
        }
        #endregion

        public void DeleteFile(string fileName)
        {
            string filePath = GetFullPath(fileName) + EXTEND_BIN;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        /// <summary>
        /// 删除账号时候
        /// </summary>
        public void DeleteDir()
        {
            string dirPath = Path.Combine(AssetBundleConifg.AppCachePath, Dir_Name);
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);
            }
        }
    }
}
