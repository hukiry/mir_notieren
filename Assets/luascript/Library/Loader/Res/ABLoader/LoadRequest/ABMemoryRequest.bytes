--- 远程下载，内存加载
--- ABMemoryRequest       
--- Author : hukiry     
--- DateTime 2024/8/8 14:31   
---

---@class ABMemoryRequest:LoadRequest
local ABMemoryRequest = Class(LoadRequest)
---初始化字段
function ABMemoryRequest:ctor(manifestData)
    ---@type UnityEngine.AssetBundleCreateRequest
    self.request = nil
    ---@type HotAssetInfo
    self.manifestData = manifestData
    self.abName = manifestData.ab
    ---@type boolean
    self.isWwwFailure = false
end

function ABMemoryRequest:IsLoading()
    if self.request~=nil and self.request.isDone then
        if not AssetBundleCache:IsExist(self.abName) then
            logError(self.request.assetBundle == nil, self.abName, "加载资源失败")
            self.assetBundle = self.request.assetBundle
            return false
        end
    end
    return not self.isWwwFailure
end

function ABMemoryRequest:CreateRequest()
    logError("启动加载请求" , self.manifestData:GetPath())
    local path = self:GetWWWPrefix() .. self.manifestData:GetPath()
    StartCoroutine(Handle(self, self.OnLoadStream, path))
end

---@private
function ABMemoryRequest:OnLoadStream(path)
    ---@type UnityEngine.Networking.UnityWebRequest
    local request = UnityWebRequest.Get(path);
    request.downloadHandler = DownloadHandlerBuffer.New();
    Yield(request:SendWebRequest());

    if request.isNetworkError or request.isHttpError then
        self.isWwwFailure = true;
        logError("WWW加载资源失败, path:" , path , ",   error:" , request.error);
    else
        local buffer = request.downloadHandler.data;
        --buffer = EncryptionStream:Decrypt(buffer);
        self.request = UnityEngine.AssetBundle.LoadFromMemoryAsync(buffer);
    end
    request:Dispose();
end

function ABMemoryRequest:GetWWWPrefix()
    if UNITY_ANDROID then
        return ""
    elseif UNITY_IOS then
        return "file://"
    else
        return ""
    end
end

return ABMemoryRequest