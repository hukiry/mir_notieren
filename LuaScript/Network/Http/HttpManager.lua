---
--- HttpManager
--- Created by hukiry.
--- DateTime: 2022/11/17 17:55
---


require('Network.Http.HttpBase')
---@class HttpManager
local HttpManager = {}

---上传数据
---@param postUrl string
---@param dicTable table<string,string> 字典
---@param callbackFunc function<string>
function HttpManager:HttpPost(postUrl, dicTable, callbackFunc)
    local receiveFunc = function(callback, url, code, text)
        if code == EHttpCode.Success then
            callback(text)
        else
            callback(nil)
            logError("失败：", url, text)
        end
    end
    ---@type HttpPostForm
    local postBinary = require("Network.Http.HttpPostForm").New()
    postBinary:Post(postUrl, dicTable, HandleParams(receiveFunc, callbackFunc, postUrl))
end

---获取数据
---@param getUrl string
---@param dicTable table<string,string> 字典
---@param callbackFunc function<string, boolean>
function HttpManager:HttpGet(getUrl, dicTable, callbackFunc)
    local receiveFunc = function(callback, url, code, text)
        if code == EHttpCode.Success then
            callback(text, true)
        else
            callback(text, false)
            logError("失败：", url, text)
        end
    end
    ---@type HttpGet
    local postBinary = require("Network.Http.HttpGet").New()
    postBinary:Get(getUrl, dicTable, HandleParams(receiveFunc, callbackFunc, getUrl))
end

---获取字节数据
---@param getUrl string
---@param callbackFunc function<boolean, Hukiry.Socket.ByteBlock>
function HttpManager:HttpGetBytes(getUrl, callbackFunc)
    local receiveFunc = function(callback, url, code, buffer)
        if code == EHttpCode.Success then
            Hukiry.BinaryDataMgr.instance:ReceiveDealBlock(buffer, function(block)
                callback(true, block)
            end)
        else
            callback(true, nil)
            logError("失败：", url)
        end
    end
    ---@type HttpGet
    local postBinary = require("Network.Http.HttpGet").New()
    postBinary:Get(getUrl,nil, nil, HandleParams(receiveFunc, callbackFunc, getUrl))
end

---获取字节数据
---@param postUrl string
---@param postData LuaInterface.LuaByteBuffer
---@param callbackFunc function<boolean, Hukiry.Socket.ByteBlock>
function HttpManager:HttpPostBinary(postUrl, postData, callbackFunc)
    local receiveFunc = function(callback, url, code, block)
        if code == EHttpCode.Success then
            callback(code, block)
        else
            callback(code, nil)
            logError("失败：", url)
        end
    end
    ---@type HttpPostBinary
    local postBinary = require("Network.Http.HttpPostBinary").New()
    postBinary:Post(postUrl, postData, HandleParams(receiveFunc, callbackFunc, postUrl))
end

---是否链接到网络
---@return boolean
function HttpManager:IsHaveNetwork()
    local state = Hukiry.HukiryUtil.GetNetworkState()
    return state > 0
end

return HttpManager