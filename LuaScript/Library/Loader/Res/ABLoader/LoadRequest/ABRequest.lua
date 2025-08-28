--- 本地异步加载
--- ABRequest       
--- Author : hukiry     
--- DateTime 2024/8/8 14:32   
---

---@class ABRequest:LoadRequest
local ABRequest = Class(LoadRequest)
---初始化字段
function ABRequest:ctor(abName)
    ---@type UnityEngine.AssetBundleCreateRequest
    self.request = nil
    self.abName = abName
end

function ABRequest:IsLoading()
    if self.request~=nil and self.request.isDone then
        if not AssetBundleCache:IsExist(self.abName) then
            self.assetBundle = self.request.assetBundle
            return false
        end
    end
    return true
end

---下载Ab
function ABRequest:CreateRequest()
    local path = SingleAssist.AssetBlock():FindPath(self.abName)
    self.request = UnityEngine.AssetBundle.LoadFromFileAsync(path)
    --log("ABRequest 加载的路径：", path, "pink")
end

return ABRequest