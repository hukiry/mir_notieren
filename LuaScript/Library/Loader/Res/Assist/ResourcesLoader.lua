---
--- ResourcesLoader       
--- Author : hukiry     
--- DateTime 2024/8/8 14:26   
---

---@class ResourcesLoader
local ResourcesLoader = Class()
---初始化字段
function ResourcesLoader:ctor()
    self.OnLoadedCallback = nil
end

---@param OnLoadedCallback function<UnityEngine.Object>
function ResourcesLoader:LoadAsyncMp4(assetName, OnLoadedCallback)
    self.OnLoadedCallback = OnLoadedCallback
    StartCoroutine(Handle(self, self.LoadAssetAsync, assetName))
end

function ResourcesLoader:Unload(obj)
    UnityEngine.Resources.UnloadAsset(obj)
end

---@private
function ResourcesLoader:LoadAssetAsync(assetName)
    local rr = UnityEngine.Resources.LoadAsync(assetName);
    while (not rr.isDone) do
        WaitForEndOfFrame()
    end
    self.OnLoadedCallback(assetName, rr.asset)
    Yield(0)
end

return ResourcesLoader