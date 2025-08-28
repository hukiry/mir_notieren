using GameServer.MySQL;
using Hukiry.Protobuf;
using System;
using System.Collections.Generic;

namespace GameServer.TcpServer
{
    //用于模板生成
    public class {PROTO_CLASS}_message_Pb: MessageProto
    {
        public override MessageProto GetMessagePb() => this;

        {PROTO_Method}
    }
}