--- 资源加载管理
--- AssetsLoaderMgr       
--- Author : hukiry     
--- DateTime 2024/8/8 14:25   
---

---@class AssetsLoaderMgr
local AssetsLoaderMgr = Class()
function AssetsLoaderMgr:ctor()

end

---初始化字段
function AssetsLoaderMgr:Initialize()
    ---标识缓存 字典
    self.CacheMarkList = {}
    if UNITY_EDITOR and not ASSETBUNDLE_TEST then
        self.instance = require("Library.Loader.Res.Weg.EditorLoader").New()
    else
        ---@type AssetBundleLoader
        self.instance = require("Library.Loader.Res.Weg.AssetBundleLoader").New()
    end
end

---@return boolean
function AssetsLoaderMgr:IsCacheMark(key)
    return self.CacheMarkList[key]~=nil
end

function AssetsLoaderMgr:CheckAssetName(assetName)
    if UNITY_EDITOR then
        if assetName == nil then
            logError("AssetBundle Name 不能为空!")
        end
    end
end

---转换路径
---@return string
function AssetsLoaderMgr:SwitchingPath(assetName)
    return SingleAssist.AssetsLoader():SwitchingPath(assetName)
end

---@return UnityEngine.Object
function AssetsLoaderMgr:LoadAsset(assetName)
    self:CheckAssetName(assetName)
    assetName = string.lower(assetName)
    local abName = self:SwitchingPath(assetName)
    return self.instance:LoadAsset(assetName, abName)
end

---@return UnityEngine.Object[]
function AssetsLoaderMgr:LoadAllAsset(assetName)
    self:CheckAssetName(assetName)
    assetName = string.lower(assetName)
    local abName = self:SwitchingPath(assetName)
    return self.instance:LoadAllAsset(assetName, abName)
end

function AssetsLoaderMgr:LoadAsync(assetName, OnLoadedCallback, loadObject)
    loadObject  = loadObject or true
    self:CheckAssetName(assetName)
    assetName = string.lower(assetName)
    local abName = self:SwitchingPath(assetName)
    self.instance:LoadAsync(assetName, abName, OnLoadedCallback, loadObject)
end

function AssetsLoaderMgr:LoadAllAsync(assetName, OnLoadedCallback)
    self:CheckAssetName(assetName)
    assetName = string.lower(assetName)
    local abName = self:SwitchingPath(assetName)
    self.instance:LoadAllAsync(assetName, abName, OnLoadedCallback)
end

---异步加载场景
function AssetsLoaderMgr:LoadSceneAsync(levelName, onProgress, onFinish)
    self:CheckAssetName(levelName)
    levelName = string.lower(levelName)
    local abName = self:SwitchingPath("scenes/"..levelName)
    self.instance:LoadSceneAsync(levelName, abName, onProgress, onFinish)
end

---标识某个资源缓存
function AssetsLoaderMgr:ToCacheAssets(assetName)
    assetName = string.lower(assetName)
    local abName = self:SwitchingPath(assetName)
    self.CacheMarkList[abName] = abName
end

---检查是否已加载过
function AssetsLoaderMgr:IsLoad(assetName)
    assetName = string.lower(assetName)
    local abName = self:SwitchingPath(assetName)
    return AssetBundleCache:FindAssetBundle(abName) ~=nil
end

---释放资源
function AssetsLoaderMgr:Unload(assetName, delay, force)
    delay = delay or 0
    force = force or false
    assetName = string.lower(assetName)
    if delay<=0  then
        self:UnloadImmediate(assetName, force)
        return
    end

    Single.TimerManger():DoTime(self, Handle(self, self.UnloadImmediate, assetName, force), delay, 1)
end

---立即移除
function AssetsLoaderMgr:UnloadImmediate(assetName, force, _self)
    assetName = string.lower(assetName)
    force = force or false
    local abName = self:SwitchingPath(assetName)
    if self.CacheMarkList[abName] then
        if not force then
            return
        end
        self.CacheMarkList[abName] = nil
    end

    if force then
        AssetBundleCache:UnloadImmediate(assetName)
    else
        AssetBundleCache:UnloadAsset(assetName)
    end
end

---卸载未使用的资源
function AssetsLoaderMgr:UnloadUnusedAssets(gc)
    self.instance:UnloadUnusedAssets(gc)
end

function AssetsLoaderMgr:UnLoadAllAssets()
    AssetBundleCache:UnloadAllAsset();
end

function AssetsLoaderMgr:Destroy()
    self.instance:Destroy()
end

return AssetsLoaderMgr