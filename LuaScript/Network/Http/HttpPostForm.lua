---
--- HttpPostForm
--- Author : hukiry     
--- DateTime 2024/7/12 17:19   
---

---@class HttpPostForm:HttpBase
local HttpPostForm = Class(HttpBase)
function HttpPostForm:ctor()
end

---@param url string 地址
---@param args table<...> 表单
---@param callback function<EHttpCode, string> 回调函数
function HttpPostForm:Post(url, args , callback)
    local form = UnityEngine.WWWForm.New();
    if args then
        for key, value in pairs(args) do
            self.form:AddField(tostring(key), tostring(value));
        end
    end
    self.form = form
    self:StartRequest(url, callback)
end

---@private
function HttpPostForm:StartOverrideSend()
    ---@type UnityEngine.Networking.UnityWebRequest
    local request = UnityWebRequest.Post(self.url, self.form);
    request.timeout =3;
    request.downloadHandler = DownloadHandlerBuffer.New();
    Yield(request:SendWebRequest());
    self:ActionResult(request);
end

return HttpPostForm