---
--- HttpGet       
--- Author : hukiry     
--- DateTime 2024/7/13 16:01   
---

---@class HttpGet:HttpBase
local HttpGet = Class(HttpBase)
function HttpGet:ctor()
end

---@param url string 地址
---@param args table<...> 表
---@param callback function<EHttpCode, string> 文本回调函数
---@param callbackBuffer function<EHttpCode, System.Byte[]> 字节回调函数
function HttpGet:Get(url, args, callback, callbackBuffer)
    local address = url
    if args then
        local buildArgs = ''
        local len, index = table.length(args), 0
        for key, value in pairs(args) do
            index = index + 1
            if len == index then
                buildArgs = buildArgs ..string.format("%s=%s",key,value)
            else
                buildArgs = buildArgs ..string.format("%s=%s&",key,value)
            end
        end
        address = url.."?".. buildArgs
    end

    self:StartRequest(address,  callback, callbackBuffer)
end

---@private
function HttpGet:StartOverrideSend()
    ---@type UnityEngine.Networking.UnityWebRequest
    local request = UnityWebRequest.Get(self.url);
    request.timeout =3;
    request.downloadHandler = DownloadHandlerBuffer.New();
    Yield(request:SendWebRequest());
    self:ActionResult(request);
end

return HttpGet