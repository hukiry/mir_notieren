--- 同步加载本地资源
--- AssetBlockLoader       
--- Author : hukiry     
--- DateTime 2024/8/8 14:31   
---

---@class AssetBlockAssist
local AssetBlockAssist = Class()
---初始化字段
function AssetBlockAssist:ctor()

end

---同步加载资源
---@param assetName string 资源路径
---@param abName string ab路径
function AssetBlockAssist:LoadAssets(assetName, abName)
    local holder = self:LoadAssetHolder(abName)
    if holder then
        holder:AddRefParent(holder.AssetName);
        local obj = holder:LoadAsset(assetName)
        if obj==nil then
            logError("AssetBlockAssist 同步加载资源失败:" , holder.AssetName, assetName);
        end
        return obj;
    end
    logError(assetName, "同步加载资源失败!");
    self:FailReleaseLoaded(assetName, abName)
    return nil
end

---同步加载资源列表
---@param assetName string 资源路径
---@param abName string ab路径
function AssetBlockAssist:LoadAssetsList(assetName, abName)
    local holder = self:LoadAssetHolder(abName)
    if holder then
        holder:AddRefParent(holder.AssetName);
        return holder:LoadAllAssets()
    end
    logError(assetName, "同步加载资源失败!");
    self:FailReleaseLoaded(assetName, abName)
    return nil
end

---@private
---递归同步加载子节点
---@param abName string ab路径
function AssetBlockAssist:LoadAssetHolder(abName)
    ---@type HotAssetInfo
    local mData = self:GetManifestData(abName)
    if mData then
        local holder = AssetBundleCache:FindAssetBundle(abName);
        if holder==nil then
            for _, name in ipairs(mData.ds) do
                local dsAB = AssetBundleCache:FindAssetBundle(abName);
                if dsAB == nil then
                    local nextAbName = name
                    ---@type AssetBundleHolder
                    local child = self:LoadAssetHolder(nextAbName)
                    if child == nil then
                        return nil
                    end
                    child:AddRefParent(abName)
                end
            end
            local path = mData:GetPath()
            log("LoadAssetHolder", "递归同步加载, path:" , path);
            local bundle = UnityEngine.AssetBundle.LoadFromFile(path);
            if bundle then
                holder = require("Library.Loader.Res.ABLoader.AssetBundleHolder").New(abName, bundle)
                AssetBundleCache:AddLoaded(holder)
                return holder
            end
        end
        return holder
    end
    return nil
end

------------------------------资源依赖相关------------------

---获取AssetBundle的依赖，会缓存
---@return table<number, string>
function AssetBlockAssist:GetDependencies( name)
    local mData = self:GetManifestData(name)
    if mData then
        return mData.ds
    end
    return {}
end

---获取AssetBundle的依赖，会缓存
function AssetBlockAssist:GetManifestData( abName)
    return SingleAssist.AssetBundle():GetAssetBundleDataAtPath(abName)
end

---查找路径
function AssetBlockAssist:FindPath(resName)
    local mData = self:GetManifestData(resName);
    if mData then
        return mData:GetPath()
    end
    return resName
end

---加载失败,释放已经加载的资源
function AssetBlockAssist:FailReleaseLoaded(path, parent)
    local dependices = self:GetDependencies(path)
    for i, v in ipairs(dependices) do
        self:FailReleaseLoaded(v, path)
    end

    local holder = AssetBundleCache:FindAssetBundle(path)
    if holder then
        holder:DelReference(parent)
    end
end

return AssetBlockAssist