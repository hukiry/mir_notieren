---
--- AssetsLoaderAssist       
--- Author : hukiry     
--- DateTime 2024/8/8 14:30   
---

---@class AssetsLoaderAssist
local AssetsLoaderAssist = Class()
---初始化字段
function AssetsLoaderAssist:ctor()
    ---@type table<string,string>
    self.assetBundleNameCache = {}
end

---转换路径
function AssetsLoaderAssist:SwitchingPath(assetName)
    local  temp = "resourcesex/" .. assetName;
    if not UNITY_EDITOR or ASSETBUNDLE_TEST then
        temp = self:GetAssetBundleName(temp)
    end
    return temp
end

---根据加载的名字，获取assetBundle名字
function AssetsLoaderAssist:GetAssetBundleName(abName)
    local fullpath = self.assetBundleNameCache[abName]
    if fullpath == nil then
        fullpath = abName .. ".ab"
        self.assetBundleNameCache[abName] = fullpath
        return fullpath
    end
    return fullpath
end

return AssetsLoaderAssist