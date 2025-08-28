using Hukiry.Protobuf;
using System.Linq;
using System.Collections.Generic;

namespace Game.Http
{
    //用于模板生成
    public class {PROTO_CLASS}_message_Pb: MessageProto
    {
        public override MessageProto GetMessagePb() => this;

        {PROTO_Method}
    }
}