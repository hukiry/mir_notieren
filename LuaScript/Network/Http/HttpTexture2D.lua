---
--- HttpTexture2D       
--- Author : hukiry     
--- DateTime 2024/7/12 20:12   
---

---@class HttpTexture2D:HttpBase
local HttpTexture2D = Class(HttpBase)
function HttpTexture2D:ctor()
end

---@param url string 地址
---@param callback function<EHttpCode, string, UnityEngine.Texture2D> 回调函数
function HttpTexture2D:Get(url, callback)
    self:StartRequest(url,  callback)
end

---@private
function HttpTexture2D:StartOverrideSend()
    ---@type UnityEngine.Networking.UnityWebRequest
    local request = UnityWebRequest.Get(self.url);
    request.timeout =3;
    request.downloadHandler = DownloadHandlerBuffer.New();
    Yield(request:SendWebRequest());
    if request.isNetworkError or request.isHttpError then
        self:Fail(request.url, request.error);
    else
        self:Success(request.downloadHandler.data);
    end
    request:Dispose();
end

---@private
function HttpTexture2D:Success(buffer)
    log(string.Format("HttpTexture2D下载成功, Url:{0}", self.url));
    local t2d = Texture2D.New(0, 0);
    try
    {
        function()
            t2d.LoadImage(buffer);
            if self.callback then
                self.callback(EHttpCode.Success, self.url, t2d)
            end
        end,
        catch = function(msg)
            self:Fail(self.url, msg);
        end
    }
end

---@private
function HttpTexture2D:Fail(url, error)
    StopCoroutine(self.coroutine)
    if self.retryTime > 0 then
        logError("HttpTexture2D连接错误", url, self.retryTime , error);
        self.coroutine = StartCoroutine(Handle(self, self.StartOverrideSend));
        self.retryTime=self.retryTime-1;
    else
        log(string.Format("HttpTexture2D下载失败, Url:{0}, error:{1}", self.url, error));
        if self.callback then
            self.callback(EHttpCode.Failure, self.url, nil)
        end
    end
end

function HttpTexture2D:StopDownload()
    if self.coroutine then
        StopCoroutine(self.coroutine)
    end
end

return HttpTexture2D