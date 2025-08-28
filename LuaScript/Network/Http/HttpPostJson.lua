---
--- HttpPostJson       
--- Author : hukiry     
--- DateTime 2024/7/12 18:02   
---

---@class HttpPostJson:HttpBase
local HttpPostJson = Class(HttpBase)
function HttpPostJson:ctor()
end

---@param url string 地址
---@param postData string
---@param callback function<EHttpCode, string> 回调函数
function HttpPostJson:Post(url, postData , callback)
    self.postData = postData
    self:StartRequest(url,  callback)
end

---@private
function HttpPostJson:StartOverrideSend()
    ---@type UnityEngine.Networking.UnityWebRequest
    local request = UnityWebRequest.New(self.url, UnityWebRequest.kHttpVerbPOST);
    local postBytes = Hukiry.HukiryUtil.GetBytes(self.postData);
    request.uploadHandler = UploadHandlerRaw.New(postBytes);
    request:SetRequestHeader("Content-Type", "application/json");

    request.timeout =3;
    request.downloadHandler = DownloadHandlerBuffer.New();
    Yield(request:SendWebRequest());

    self:ActionResult(request);
end

return HttpPostJson