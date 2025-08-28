---
--- EditorLoader       
--- Author : hukiry     
--- DateTime 2024/8/8 14:25   
---

---@class EditorLoader
local EditorLoader = Class()
---初始化字段
function EditorLoader:ctor()

end

function EditorLoader:LoadAssetHolder(abName)
    local holder = AssetBundleCache:FindAssetBundle(abName)
    if holder == nil then
        holder = require("Library.Loader.Res.ABLoader.AssetBundleHolder").New(abName, nil)
        holder:AddRefParent(abName)
        AssetBundleCache:AddLoaded(holder)
    end
    return holder
end

function EditorLoader:LoadAsset(assetName, abName)
    local holder = self:LoadAssetHolder(abName)
    local mainAsset = holder:LoadAsset(abName)
    if mainAsset == nil then
        logError("加载资源失败:", assetName)
    end
    return mainAsset
end

---同步加载资源列表
---@return UnityEngine.Object[]
function EditorLoader:LoadAllAsset(assetName, abName)
    local holder = self:LoadAssetHolder(abName)
    local allAssets = holder:LoadAllAssets()
    if allAssets == nil then
        logError("加载资源失败:", assetName)
    end
    return allAssets
end

---模拟异步加载
---@param OnLoadedCallback function<UnityEngine.Object>
function EditorLoader:LoadAsync(assetName, abName, OnLoadedCallback, loadObject)
    StartCoroutine(Handle(self, self._LoadAsync, assetName, abName, OnLoadedCallback, loadObject))
end

---@private
function EditorLoader:_LoadAsync(assetName, abName, OnLoadedCallback,loadObject)
    WaitForFixedUpdate()
    if OnLoadedCallback then
        local asset = nil
        if loadObject then
            asset = self:LoadAsset(assetName, abName)
        end
        OnLoadedCallback(assetName, asset)
    end
end

---模拟异步加载列表
---@param OnLoadedCallback function<UnityEngine.Object>
function EditorLoader:LoadAllAsync(assetName, abName, OnLoadedCallback)
    StartCoroutine(Handle(self, self._LoadAllAsync, assetName, abName, OnLoadedCallback))
end
---@private
function EditorLoader:_LoadAllAsync(assetName, abName, OnLoadedCallback)
    local allAsset = self:LoadAllAsset(assetName, abName)
    if OnLoadedCallback then
        OnLoadedCallback(assetName, allAsset)
    end
    WaitForEndOfFrame()
end

---异步加载场景
---@param onProgress function<number>
---@param onFinish function
function EditorLoader:LoadSceneAsync(levelName, abName, onProgress, onFinish)
    StartCoroutine(Handle(self, self._LoadSceneAsync, levelName, abName, onProgress, onFinish))
end
---@private
function EditorLoader:_LoadSceneAsync(levelName, abName, onProgress, onFinish)
    local op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelName)
    while (not op.isDone) do
        onProgress(op.progress)
        WaitForEndOfFrame()
    end

    if onFinish then
        onFinish()
    end
    WaitForEndOfFrame()
end


function EditorLoader:Unload(assetName, delay)
    self:UnloadImmediate(assetName)
end


function EditorLoader:UnloadImmediate(assetName, force)
    AssetBundleCache:UnloadImmediate(assetName);
end

---释放未使用的资源
function EditorLoader:UnloadUnusedAssets(gc)
    UnityEngine.Resources.UnloadUnusedAssets()
    if gc then
        Hukiry.ResLoadUtil.Collect()
    end
end

---释放所有资源
function EditorLoader:UnLoadAllAssets()
    AssetBundleCache:UnloadAllAsset()
end

function EditorLoader:Destroy() end

function EditorLoader:OnApplicationQuit()
    self:UnLoadAllAssets()
end

return EditorLoader