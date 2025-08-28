using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.TcpServer
{
    /// <summary>
    /// 协议定义和反射类型
    /// </summary>
    [Flags]
    public enum ProtoType
    {
        /// <summary>
        /// 默认保存到数据库
        /// </summary>
        None = 1,
        /// <summary>
        /// 用来定义文本数据的长度:VARCHAR
        /// </summary>
        Text = 2,
        /// <summary>
        /// 用来定义文本数据的长度:TEXT
        /// </summary>
        FullText = 4,
        /// <summary>
        /// 声明字段为唯一键
        /// </summary>
        PrimaryKey = 8,
        /// <summary>
        /// 是将此类生成为表，字段和协议无效：新建立表
        /// </summary>
        SqlTable = 16,
        /// <summary>
        /// 客户端协议定义发来的数据:每次需要更新
        /// </summary>
        Client = 32,
        /// <summary>
        /// 服务器协议定义响应的数据
        /// </summary>
        Server = 64,
        /// <summary>
        /// 自定义字段声明：每次需要更新
        /// </summary>
        Custome = 128,
    }

    [AttributeUsageAttribute(AttributeTargets.Class| AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class TextLengthAttribute : Attribute
    {
        
        public int size;
        public ProtoType protoType;
        public string tableName;
        /// <summary>
        /// 特性反射
        /// </summary>
        /// <param name="protoType">协议定义和反射类型</param>
        /// <param name="size">仅字符串长度声明</param>
        public TextLengthAttribute(ProtoType protoType, int size = 0)
        {
            this.protoType = protoType;
            this.size = size;
        }

        public bool IsContain(ProtoType proto) => (this.protoType & proto) == proto;
    }
}
