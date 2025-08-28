---
--- AssetBundleHolder       
--- Author : hukiry     
--- DateTime 2024/8/8 14:30   
---

---@class AssetBundleHolder:RefObject
local AssetBundleHolder = Class(RefObject)
---初始化字段
function AssetBundleHolder:ctor(name, bundle)
    ---assetBundle的名字
    self.AssetName = name;
    ---true 加载完成自动释放ab  false由引用计数管理
    self.isMem = false
    ---@type UnityEngine.AssetBundle
    self.m_AssetBundle = bundle;
    ---当前引用者列表 集合
    ---@type table<string>
    self.m_RefParentList = {}
    ---主实体，如果是单一资源
    ---@type UnityEngine.Object
    self.mainAsset = nil
    ---@type UnityEngine.Object[] 数组
    self.allAssets = nil
    ---已加载的实体
    ---@type table<string, UnityEngine.Object>
    self.assetsDic = {}
    ---是否场景资源
    self.IsStreamed = false
    if bundle then
        self.IsStreamed = bundle.isStreamedSceneAssetBundle
    end
    local mData = SingleAssist.AssetBlock():GetManifestData(name);
    if mData == nil then
        self.isMem = false
    end
    ---是否是单一资源
    self.IsSingle = true
end

---是否已经加载过
function AssetBundleHolder:HasLoaded(name)
    if self.IsSingle then
        return self.mainAsset~=nil
    else
        return self.assetsDic[name]~=nil
    end
end

---是否已经加载过
function AssetBundleHolder:HasLoaded()
    return self.allAssets~=nil
end

function AssetBundleHolder:LoadAsset(name)
    if UNITY_EDITOR and not ASSETBUNDLE_TEST then
        self.mainAsset = Hukiry.ResLoadUtil.LoadAsset(self.allAssets, self.AssetName)
        self:Retain()
        return self.mainAsset
    else
        if self.IsSingle then
            if self.mainAsset == nil then
                self.mainAsset = self.m_AssetBundle:LoadAsset(Hukiry.ResLoadUtil.GetFileName(name))
            end

            if self.isMem then
                self:UnLoadAssetBundle(false)
            end
            self:Retain()
            --log("name=", name, self.mainAsset,"yellow")
            return self.mainAsset
        else
            local fileName = Hukiry.ResLoadUtil.GetFileName(name)
            if not self.assetsDic[name] then
                self.assetsDic[name] = self.m_AssetBundle:LoadAsset(fileName)
            end
            self:Retain()
            return self.assetsDic[name]
        end
    end
end

---加载所有
function AssetBundleHolder:LoadAllAssets()
    if self.allAssets == nil then
        if UNITY_EDITOR and not ASSETBUNDLE_TEST then
            self.allAssets = Hukiry.ResLoadUtil.LoadAllAssets(self.AssetName)
        else
            self.allAssets =  self.m_AssetBundle:LoadAllAssets();
            if self.isMem then
                self:UnLoadAssetBundle(false);
            end
        end
    end
    self:Retain()

    return self.allAssets
end

---异步加载
function AssetBundleHolder:LoadAsync(name)
   return self.m_AssetBundle:LoadAssetAsync(Hukiry.ResLoadUtil.GetFileName(name))
end

---设置异步加载完的实体对象
---@param asset UnityEngine.Object
function AssetBundleHolder:SetAsyncLoad(name,asset)
    if self.IsSingle then
        self.mainAsset = asset
    else
        self.assetsDic[name] = asset
    end
end

---异步加载所有
---@return UnityEngine.AssetBundleRequest
function AssetBundleHolder:LoadAllAsync()
    return self.m_AssetBundle:LoadAllAssetsAsync()
end

---设置异步加载完的实体对象
---@param assets UnityEngine.Object[]
function AssetBundleHolder:SetAllAsyncLoad(name, assets)
    self.allAssets = assets
end

---获取引用父节点
---@return table<number,string>
function AssetBundleHolder:GetRefParents()
    return self.m_RefParentList
end

---添加引用
function AssetBundleHolder:AddRefParent(name)
    local index = table.findIndex(self.m_RefParentList, name)
    if index > 0 then
        return
    end
    table.insert(self.m_RefParentList, name)
end

---移除引用
function AssetBundleHolder:DelReference(abname)
    local index = table.findIndex(self.m_RefParentList, abname)
    if index>0 then
        table.remove(self.m_RefParentList, index)
        if #self.m_RefParentList == 0 then
            self:DeleteSelf()
        end
    end
end

---释放使用的
function AssetBundleHolder:UnLoadUnuseAssets()
    local unuseList = {}
    for i, refparent in ipairs(self.m_RefParentList) do
        if not AssetBundleCache:IsExist(refparent) then
            table.insert(unuseList,refparent)
        end
    end

    for i, v in ipairs(unuseList) do
        self:DelReference(v)
    end
end

function AssetBundleHolder:Release()
    self.m_ReferencedCount = self.m_ReferencedCount-1
    if self.m_ReferencedCount<=0 then
        self:DelReference(self.AssetName)
    end
end

---释放AssetsBundle
function AssetBundleHolder:UnLoadAssetBundle(flag)
    if self.m_AssetBundle~=nil then
        self.m_AssetBundle:Unload(flag)
    end
end

---删除自己
function AssetBundleHolder:DeleteSelf()
    self.m_ReferencedCount = 0
    self:UnLoadAssetBundle(true)
    AssetBundleCache:DelLoaded(self.AssetName)

    local dependencies = SingleAssist.AssetBlock():GetDependencies(self.AssetName);
    for i, onlyname in ipairs(dependencies) do
        local assets = AssetBundleCache:FindAssetBundle(onlyname);
        if assets then
            assets:DelReference(self.AssetName)
        end
    end
end

return AssetBundleHolder