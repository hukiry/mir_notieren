---
--- HttpPostBinary       
--- Author : hukiry     
--- DateTime 2024/7/13 13:54   
---

---@class HttpPostBinary:HttpBase
local HttpPostBinary = Class(HttpBase)
function HttpPostBinary:ctor()
end

---@param url string 地址
---@param postData string
---@param callback function<EHttpCode, string> 回调函数
function HttpPostBinary:Post(url, postData , callback)
    self.postData = postData
    self:StartRequest(url,  callback)
end

---@private
function HttpPostBinary:StartOverrideSend()
    ---@type UnityEngine.Networking.UnityWebRequest
    local request = UnityWebRequest.New(self.url, UnityWebRequest.kHttpVerbPOST);
    local postBytes = Hukiry.HukiryUtil.GetBytes(self.postData);
    request.uploadHandler = UploadHandlerRaw.New(postBytes);
    request:SetRequestHeader("Content-Type", "application/binary");

    request.timeout =3;
    request.downloadHandler = DownloadHandlerBuffer.New();
    Yield(request:SendWebRequest());

    ---@type EHttpCode
    local code = 0;
    ---@type System.Byte[]
    local content = nil
    if request.isNetworkError or request.isHttpError then
        code = EHttpCode.Failure;
        content = request.error;
    else
        code = EHttpCode.Success;
        content = request.downloadHandler.text;
    end
    request:Dispose();

    if not Single.Http():IsHaveNetwork() and code == EHttpCode.Failure then
        self:DealResult(code, content)
    else
        self:ActionResult(code, content);
    end
end

---@private
function HttpPostBinary:ActionResult(code, content)
    if code == EHttpCode.Success then
        self.callback(code, content);
    else
        if self.retryTime > 0 then
            StopCoroutine(self.coroutine)
            self.coroutine = StartCoroutine(Handle(self, self.StartOverrideSend));
            self.retryTime = self.retryTime-1;
        else
            self.callback(code, content);
        end
    end
end

return HttpPostBinary