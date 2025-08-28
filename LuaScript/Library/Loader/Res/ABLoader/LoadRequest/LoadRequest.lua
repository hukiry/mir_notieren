--- 加载请求
--- LoadRequest       
--- Author : hukiry     
--- DateTime 2024/8/8 14:32   
---

---@class LoadRequest
LoadRequest = Class()
---初始化字段
function LoadRequest:ctor()
    ---资源名称
    self.abName = ''
    ---加载是否完成
    self.m_IsDone = false
    ---请求错误
    self.m_Error = ''
    ---@type table<string> 集合
    self.m_RefParentList = {}
    ---@type table<LoadRequest> 集合
    self.m_LoadingList = {}
    ---加载的assetbunlde
    ---@type UnityEngine.AssetBundle
    self.assetBundle = nil
end

function LoadRequest:Error()
    return self.m_Error
end

---是否加载完成
function LoadRequest:IsDone()
    return self.m_IsDone
end

function LoadRequest:CreateRequest()

end

---是否加载中
function LoadRequest:IsLoading()
    return true
end

---加载完成
function LoadRequest:OnAssetsLoaded()
    self.m_IsDone = true
    if #self.m_RefParentList==0 then
        if not IsNil(self.assetBundle) then
            self.assetBundle:Unload(false)
        end
    elseif not IsNil(self.assetBundle) then
        local holder = require("Library.Loader.Res.ABLoader.AssetBundleHolder").New(self.abName, self.assetBundle)
        for _, refname in ipairs(self.m_RefParentList) do
            holder:AddRefParent(refname)
        end
        AssetBundleCache:AddLoaded(holder)
    end
end

---添加其他的引用
function LoadRequest:AddRefParent(abname)
    table.insert(self.m_RefParentList, abname)
end

---移除引用
function LoadRequest:DelReference(abname)
    local index = table.findIndex(self.m_RefParentList, abname)
    if index > 0 then
        table.remove(self.m_RefParentList, index)
        if #self.m_RefParentList>0 then
            self:DeleteSelf()
        end
    end
end

---删除自己
---@private
function LoadRequest:DeleteSelf()
    SingleAssist.AssetAsync():RemoveLoading(self.abName)
    local dependencies = SingleAssist.AssetBlock():GetDependencies(self.abName)
    for i, onlyname in ipairs(dependencies) do
        local loading = SingleAssist.AssetAsync():FindLoading(onlyname);
        if loading then
            loading:DelReference(self.abName)
        end
    end
end
