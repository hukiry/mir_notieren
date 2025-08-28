--- AB资源缓存管理
--- AssetBundleCache       
--- Author : hukiry     
--- DateTime 2024/8/8 14:31   
---

---@class AssetBundleCache
AssetBundleCache = {}
---已经加载完的缓存列表
---@type table<string, AssetBundleHolder>
local mLoadedList = {}
---添加加载完成的资源
---@param holder AssetBundleHolder
function AssetBundleCache:AddLoaded(holder)
    local assetName = holder.AssetName;
    if mLoadedList[assetName] then
        logError("已经存在缓存列表中，请检查逻辑" , assetName)
        return
    end
    mLoadedList[assetName] = holder
end

---从缓存中删除
function AssetBundleCache:DelLoaded(assetName)
    mLoadedList[assetName] = nil
end

---是否存在
---@param assetName string
function AssetBundleCache:IsExist(assetName)
    return mLoadedList[assetName]~=nil
end

---节点依赖是否所有的都已经完毕
---@return boolean
function AssetBundleCache:IsAllDependicesReady(name)
    local dependencies = SingleAssist.AssetBlock():GetDependencies(name);
    for i, v in ipairs(dependencies) do
        if not self:IsAllDependicesReady(v) then
            return false
        end

        if mLoadedList[v] == nil then
            return false
        end
    end
    return true
end

---获取缓存文件
---@return AssetBundleHolder
function AssetBundleCache:FindAssetBundle(assetName)
    return mLoadedList[assetName]
end

---释放资源
function AssetBundleCache:UnloadAsset(assetName)
    local holder = mLoadedList[assetName]
    if holder then
        holder:Release()
    end
end

---立即释放
function AssetBundleCache:UnloadImmediate(assetName)
    local holder = mLoadedList[assetName]
    if holder then
        holder:DelReference()
    end
end

---释放未使用资源
function AssetBundleCache:UnloadUnuseAsset()
    for i, v in pairs(mLoadedList) do
        v:UnLoadUnuseAssets()
    end
end

---释放所有资源
function AssetBundleCache:UnloadAllAsset()
    local removeList = table.toArrayKey(mLoadedList)
    for i, refname in ipairs(removeList) do
        if mLoadedList[refname] then
            mLoadedList[refname]:UnLoadUnuseAssets()
            self:UnloadImmediate(refname)
        end
    end

    for Key, Value in pairs(mLoadedList) do
        logError("还有资源未移除 name =",Key,"引用次数 =",  Value:ReferencedCount())
    end
end

---打印未释放资源
function AssetBundleCache:LogUnloadAllAsset()
    local isContains = function(key)
        if SingleAssist.LoaderMgr():IsCacheMark(key) then
            return true
        end

        if key == "ui/prefab/login/loginpanel" or key == "ui/prefab/loader/sceneloading" then
            return true
        end
        return false
    end

    local count = 0
    for key, Value in pairs(mLoadedList) do
        if not isContains(key) then
            local refParents = Value:GetRefParents()
            local str = "引用数："..#refParents..", 使用次数："..Value:ReferencedCount()..", 资源："..key;
            for i, v in ipairs(refParents) do
                str = str.."\n引用者：" .. v;
            end
            log(str)
            count = count+1
        end
    end
    log("未释放资源数量:", count)
end 

