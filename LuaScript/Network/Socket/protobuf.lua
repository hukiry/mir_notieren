---
--- 网络协议字节处理
--- Created by huxiongkun.
--- DateTime: 2022/10/18 18:40

--region 声明协议结构
    --todo 定义字段： test(字段名) = protobuf_type.byte(字段类型)
    --todo 定义字段数组： test_IsArray = true(数组标识), test(字段名) = protobuf_type.byte(字段类型)
    --todo 定义结构体： test(结构名) = { protobuf_type.byte(字段类型)，... }
    --todo 定义结构体数组： test_IsArray = true(数组标识), test(结构名) = { protobuf_type.byte(字段类型)，... }
--endregion

--region 使用例子
    --todo local block = {
    --todo     roleId = protobuf_type.int32,
    --todo     roleNick = protobuf_type.string,
    --todo     items_IsArray = true,
    --todo     items = {
    --todo             type = protobuf_type.byte,
    --todo             number = protobuf_type.int32,
    --todo     }
    --todo }

    --todo local msg = protobuf.ConvertMessage(block)
    --todo msg.roleId = 122;
    --todo msg.roleNick = "test"
    --todo msg.items = {}
    --todo table.insert(msg.items, {type=1, number = 2})
    --todo ---发送数据
    --todo local buffer = Single.Binary():SendDealBlock(function(byteBlock)
    --todo     local data = msg:SerializeToString(byteBlock);
    --todo     return data:ToArray()
    --todo end)

    --todo ---接受数据
    --todo msg = protobuf.ConvertMessage(block)
    --todo Single.Binary():ReceiveDealBlock(buffer, function(byteBlock)
    --todo     msg:ParseFromString(byteBlock)
    --todo end)
    --todo log(msg.roleId,msg.roleNick,#msg.items)
--endregion

---协议数据类型，表结构用消息体
---@class protobuf_type
protobuf_type = {
    byte =1,
    int16 = 2,
    int32 = 3,
    int64 = 4,
    float = 5,
    double = 6,
    bool = 7,
    char = 8,
    string = 9,
    ---嵌套结构数据类型
    message = 10,
    uint16 = 11,
    uint32 = 12,
    uint64 = 13,
}

---@private
---@class protobuf_defaultValue
protobuf_defaultValue = {
    [protobuf_type.byte] = 0,
    [protobuf_type.int16] = 0,
    [protobuf_type.int32] = 0,
    [protobuf_type.int64] = 0,
    [protobuf_type.float] = 0,
    [protobuf_type.double] = 0,
    [protobuf_type.bool] = false,
    [protobuf_type.char] = '',
    [protobuf_type.string] = "",
    [protobuf_type.uint16] = 0,
    [protobuf_type.uint32] = 0,
    [protobuf_type.uint64] = 0
}

---与C#函数一一对应
---@private
---@class protobuf
protobuf = {
    ---255 2^8
    ---@private
    WriteByte = function(block, v)
        block:Write(v)
    end,
    ---65536 2^16
    ---@private
    WriteInt16 = function(block, v)
        block:WriteInt16(v)
    end,
    ---4294967296 2^32
    ---@private
    WriteInt32 = function(block, v)
        block:WriteInt32(v)
    end,
    ---2^64
    ---@private
    WriteInt64 = function(block, v)
        block:WriteInt64(v)
    end,
    ---2^16
    ---@private
    WriteFloat = function(block, v)
        block:WriteFloat(v)
    end,
    ---2^32
    ---@private
    WriteDouble = function(block, v)
        block:WriteDouble(v)
    end,

    ---bool 值
    ---@private
    WriteBoolean = function(block, v)
        block:WriteBoolean(v)
    end,
    ---@private
    WriteChar = function(block, v)
        block:WriteChar(v)
    end,
    ---@private
    WriteString = function(block, v)
        block:WriteString(v)
    end,
    ---@private
    WriteUInt16 = function(block, v)
        block:WriteUInt16(v)
    end,
    ---@private
    WriteUInt32 = function(block, v)
        block:WriteUInt32(v)
    end,
    ---@private
    WriteUInt64 = function(block, v)
        block:WriteUInt64(v)
    end,

    ---@private
    ReadByte = function(block)
        return block:ReadByte()
    end,
    ---@private
    ReadInt16 = function(block)
        return block:ReadInt16()
    end,
    ---@private
    ReadInt32 = function(block)
        return block:ReadInt32()
    end,
    ---@private
    ReadInt64 = function(block)
        return block:ReadInt64()
    end,
    ---@private
    ReadFloat = function(block)
        return block:ReadFloat()
    end,
    ---@private
    ReadDouble = function(block)
        return block:ReadDouble()
    end,
    ---@private
    ReadBoolean = function(block)
        return block:ReadBoolean()
    end,
    ---@private
    ReadChar = function(block)
        return block:ReadChar()
    end,
    ---@private
    ReadString = function(block)
        return block:ReadString()
    end,
    ---@private
    ReadUInt16 = function(block)
        return block:ReadUInt16()
    end,
    ---@private
    ReadUInt32 = function(block)
        return block:ReadUInt32()
    end,
    ---@private
    ReadUInt64 = function(block)
        return block:ReadUInt64()
    end,
}

---@private
protobuf_write_function = {
    [protobuf_type.byte] = protobuf.WriteByte,
    [protobuf_type.int16] = protobuf.WriteInt16,
    [protobuf_type.int32] = protobuf.WriteInt32,
    [protobuf_type.int64] = protobuf.WriteInt64,
    [protobuf_type.float] = protobuf.WriteFloat,
    [protobuf_type.double] = protobuf.WriteDouble,
    [protobuf_type.bool] = protobuf.WriteBoolean,
    [protobuf_type.char] = protobuf.WriteChar,
    [protobuf_type.string] = protobuf.WriteString,
    [protobuf_type.uint16] = protobuf.WriteUInt16,
    [protobuf_type.uint32] = protobuf.WriteUInt32,
    [protobuf_type.uint64] = protobuf.WriteUInt64
}
---@private
protobuf_read_function = {
    [protobuf_type.byte] = protobuf.ReadByte,
    [protobuf_type.int16] = protobuf.ReadInt16,
    [protobuf_type.int32] = protobuf.ReadInt32,
    [protobuf_type.int64] = protobuf.ReadInt64,
    [protobuf_type.float] = protobuf.ReadFloat,
    [protobuf_type.double] = protobuf.ReadDouble,
    [protobuf_type.bool]  = protobuf.ReadBoolean,
    [protobuf_type.char] = protobuf.ReadChar,
    [protobuf_type.string] = protobuf.ReadString,
    [protobuf_type.uint16] = protobuf.ReadUInt16,
    [protobuf_type.uint32] = protobuf.ReadUInt32,
    [protobuf_type.uint64] = protobuf.ReadUInt64
}

---@class BaseProtobuf:protobuf.Message
protobuf_base_index = {
    ---反序列化
    ParseFromString = 1,
    ---序列化字符串
    ---@type Hukiry.Socket.ByteBlock
    SerializeToString = 2
}

---@private
---@param buf protobuf_type
protobuf.read = function(block, buf)
    if protobuf_read_function[buf] then
        return protobuf_read_function[buf](block)
    else
        logError("读取类型不存在 " .. buf)
        return 0
    end
end

---@private
---@param buf protobuf_type
protobuf.write = function(block, v, buf)
    if protobuf_write_function[buf] then
        protobuf_write_function[buf](block, v)
    else
        logError("写入类型不存在 ".. buf)
    end
end

---转换为副本
---@param t table 集合
protobuf.toArray = function(t)
    local _result = {}
    for _, v in ipairs(t) do
        table.insert(_result, v)
    end
    return _result
end

---消息类
---@return table
protobuf.Message = function(message_meta)
    local _member = {}
    ---过滤堆栈中不显示的成员
    local filterMemberCall = function(_self, meta)
        local strTab = {"fields","fieldType","fieldTag","Message"}
        for key, v in pairs(meta) do
            local isFind = false
            for _, strValue in ipairs(strTab) do
                if string.find(key, strValue)  then
                    isFind = true
                    break
                end
            end
            ---堆栈中不显示函数，和部分字段
            if not isFind and type(v)~="function" then
                _self[key] = v
            end
        end
    end

    ---@return BaseProtobuf
    local callFunction = function(__self)
        if __self._ctor then __self:_ctor() end
        local objTab = {}
        filterMemberCall(objTab, __self)
        return setmetatable(objTab, {__index = __self})
    end

    ---转化消息元
    ---@param key string
    message_meta.ToMessageMeta = function(_self, key)
        local array = protobuf.toArray(_self[key])
        _self[key] = {}
        for _, vTab in ipairs(array) do
            local msg = _self[key.."_Message"]()
            for i, v in pairs(vTab) do
                msg[i] = v
            end
            table.insert(_self[key], msg)
        end
    end

    ---解析读取数据
    ---@param pb Hukiry.Socket.ByteBlock
    message_meta.ParseFromString = function(_self, pb)
        if #_self.fields > 0 then
            for i, v in ipairs(_self.fields) do
                if _self.fieldTag[i] == 1 then
                    local len = protobuf.read(pb, protobuf_type.int16)
                    for _ = 1, len do
                        if _self.fieldType[i] == protobuf_type.message then
                            local _Message = _self[v.."_Message"]()
                            _Message:ParseFromString(pb)
                            table.insert(_self[v], _Message)
                        else
                            table.insert(_self[v], protobuf.read(pb, _self.fieldType[i]))
                        end
                    end
                else
                    if _self.fieldType[i] == protobuf_type.message then
                        local _Message = _self[v.."_Message"]()
                        _Message:ParseFromString(pb)
                        _self[v] = _Message
                    else
                        _self[v] = protobuf.read(pb, _self.fieldType[i])
                    end
                end
            end
        end
    end

    ---序列化字符串，发送
    ---@param pb Hukiry.Socket.ByteBlock
    message_meta.SerializeToString = function(_self, pb)
        if #_self.fields > 0 then
            for i, v in ipairs(_self.fields) do
                ---数组
                if _self.fieldTag[i] == 1 then
                    local len = #_self[v]
                    protobuf.write(pb, len, protobuf_type.int16)
                    for k = 1, len do
                        if _self.fieldType[i] == protobuf_type.message then
                            if not _self[v][k].SerializeToString then
                                _self:ToMessageMeta(v)
                            end
                            _self[v][k]:SerializeToString(pb)
                        else
                            protobuf.write(pb, _self[v][k], _self.fieldType[i])
                        end
                    end
                else
                    ---不是数组
                    if _self.fieldType[i] == protobuf_type.message then
                        if _self[v] == nil then
                            _self[v]= _self[v.."_Message"]()
                        end
                        _self[v]:SerializeToString(pb)
                    else
                        protobuf.write(pb, _self[v], _self.fieldType[i])
                    end
                end
            end
        end
        return pb
    end

    filterMemberCall(_member, message_meta)
    ---__index 访问表中元素索引，message_meta=成员在堆栈中不可见，访问时才可见；__call 当函数调用时，访问自定义的函数，
    setmetatable(_member, {__index = message_meta, __call = callFunction})
    return _member
end

--region 动态构建
---生成动态实例化表
---@param _self table
---@param fieldName string 字段名称
---@param type protobuf_type 字段类型
---@param isArray boolean 是集合
---@param childMessageTable table 消息子类
---@private
protobuf.GenerateBuf = function(_self, fieldName, type, isArray, childMessageTable)
    isArray = isArray or false
    if _self.fields == nil then _self.fields = {} end
    if _self.fieldType == nil then _self.fieldType = {} end
    if _self.fieldTag == nil then _self.fieldTag = {} end
    table.insert(_self.fields, fieldName)
    table.insert(_self.fieldType, type)
    table.insert(_self.fieldTag, isArray and 1 or 0)
    if protobuf_defaultValue[type] or type == protobuf_type.bool then
        if isArray then
            _self[fieldName] = {}
        else
            _self[fieldName] = protobuf_defaultValue[type]
        end
    else
        _self[fieldName.."_Message"] = protobuf.Message(childMessageTable)
        _self[fieldName] = {}
    end
end

---转换表为消息体
---@param tabData table 表数据结构
---@return BaseProtobuf
protobuf.ConvertMessage = function(tabData, isChildMessage)
    isChildMessage = isChildMessage or false
    local tempTab = {}
    for keyName, v in pairs(tabData) do
        ---数据结构，由字段名和类型组成；v值 是bool值时，用于数组区分
        if type(v) ~= "boolean" then
            local isMessage = type(v) == "table"
            local vType, isArray, childMessage = v, false, nil
            if tabData[keyName.."_IsArray"] or tabData[keyName.."_isArray"] then
                isArray = true
            end

            if isMessage then
                vType = protobuf_type.message
                ---递归收集子表数据
                childMessage = protobuf.ConvertMessage(v, true)
            end
            ---往表中添加字段
            protobuf.GenerateBuf(tempTab, keyName, vType, isArray, childMessage)
        end
    end

    if isChildMessage then
        return tempTab
    end
    return protobuf.Message(tempTab)()
end

--endregion