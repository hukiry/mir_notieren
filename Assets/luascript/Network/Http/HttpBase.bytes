---
--- HttpBase       
--- Author : hukiry     
--- DateTime 2024/7/12 17:43   
---

require("Network.http.HttpRule")
---@class HttpBase
HttpBase = Class()
function HttpBase:ctor()
    --- 剩余尝试次数
    self.retryTime = 0;
    --- 回调函数
    ---@type function<EHttpCode, string>
    self.callback=nil;
    --- 回调函数
    ---@type function<EHttpCode, System.Byte[]>
    self.callbackBuffer=nil;
    --- 请求路径
    self.url = '';
    --- 全路径
    self.coroutine = nil;
    --- form参数
    ---@type UnityEngine.WWWForm
    self.form = nil;
    --- POST参数
    self.postData = '';
end

---@param url string 地址
---@param callback function<EHttpCode, string> 回调函数
function HttpBase:StartRequest(url, callback, callbackBuffer)
    self.retryTime = 3;
    self.callback = callback;
    self.callbackBuffer = callbackBuffer;
    self.url = url;
    self.coroutine = StartCoroutine(Handle(self, self.StartOverrideSend));
end

function HttpBase:StartOverrideSend()

end

---@private
---@param request UnityEngine.Networking.UnityWebRequest
function HttpBase:ActionResult(request)
    ---@type EHttpCode
    local code = 0;
    ---@type string
    local content = nil;
    ---@type System.Byte[]
    local buffer = nil
    if request.isNetworkError or request.isHttpError then
        code = EHttpCode.Failure;
        content = request.error;
    else
        code = EHttpCode.Success;
        content = request.downloadHandler.text;
        buffer = request.downloadHandler.data;
    end
    request:Dispose();

    if code == EHttpCode.Success then
        self:CallFunction(code,  content, buffer)
    else
        if self.retryTime > 0 then
            local tryCount = (3 - self.retryTime) + 2
            logError("requestPost请求错误", self.url, content, "第".. tryCount.."次尝试");
            StopCoroutine(self.coroutine)
            self.coroutine = StartCoroutine(Handle(self, self.StartOverrideSend));
            self.retryTime = self.retryTime-1;
        else
            self:CallFunction(code,  content, buffer)
        end
    end
end

---@private
function HttpBase:CallFunction(code,  content, buffer)
    StopCoroutine(self.coroutine)
    if self.callback then
        self.callback(code,  content)
    end

    if self.callbackBuffer then
        self.callbackBuffer(code,  buffer)
    end
end