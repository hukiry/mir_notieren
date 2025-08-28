---
--- ProtoTest       
--- Author : hukiry     
--- DateTime 2024/8/5 21:03   
---

local ProtoTest = {}

function ProtoTest:Start()
    local body = {
        id= protobuf_type.uint16,
        numbers_IsArray = true,
        numbers = protobuf_type.byte,
        tels_IsArray = true,
        tels = {
            id = protobuf_type.int16,
            age = protobuf_type.int32,
            isMale = protobuf_type.bool
        }
    }

    local msg = protobuf.ConvertMessage(body)
    msg.id =255
    msg.numbers = {31,32,34,35}
    msg.tels = {}
    table.insert(msg.tels, {id = 101, age=18,isMale =true })
    table.insert(msg.tels, {id = 102, age=17,isMale =false })

    --local buffer = Single.Binary():SendDealBlock(function(byteBlock)
    --    local data = msg:SerializeToString(byteBlock);
    --    return data:ToArray()
    --end)
    log("buffer size:", buffer.Length, "green")

    local msgResult = protobuf.ConvertMessage(body)
    --Single.Binary():ReceiveDealBlock(buffer, function(byteBlock)
    --    msgResult:ParseFromString(byteBlock)
    --end)
    log("id:",msgResult.id)
    for i, v in ipairs(msgResult.numbers) do
        log("numbers:", v, "yellow")
    end

    for i, v in ipairs(msgResult.tels) do
        log("id:", v.id, "age=",v.age,"isMale=", v.isMale , "yellow")
    end

end

return ProtoTest