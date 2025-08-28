---
--- AssetBundleManager       
--- Author : hukiry     
--- DateTime 2024/8/8 14:30   
---

---@class HotAssetInfo
local HotAssetInfo = Class()
function HotAssetInfo:ctor(info)
    ---@type string
    self.ab = info.ab
    ---@type string
    self.md5 = info.md5
    ---@type number
    self.size = info.size
    ---@type table<number, string>
    self.ds = info.ds
    ---是否边下载边加载
    self.isRemote = false
end

function HotAssetInfo:SetRemote(isRemote)
    self.isRemote = isRemote
end

function HotAssetInfo:IsRemote()
    return self.isRemote
end

---获取下载路径
function HotAssetInfo:GetAbFullPath()
    return self.ab
end

---AssetBundle ： 路径前不需要加 "file：//"
---@return string
function HotAssetInfo:GetPath()
    return Hukiry.ResLoadUtil.GetPath(self.ab)
end


---@class AssetBundleManager
local AssetBundleManager = Class()

---初始化字段
function AssetBundleManager:ctor()
    ---@type table<string, HotAssetInfo>
    self.manifestDatas = {}
end

---游戏初始化时，初始化ManifestData数据
---@param manifestText number
function AssetBundleManager:RecoverManifestData(manifestText)
    if manifestText == '' then
        logError("Error:", "需要初始化 LuaManager.cs => EnableGame() ")
        return
    end
    try {
        function()
            local tempList = json.decode(manifestText)
            for _, v in ipairs(tempList) do
                self.manifestDatas[v.ab] = HotAssetInfo.New(v)
            end
        end,
        catch = function(error)
            logError("解析异常：", manifestText, error)
        end
    }
end

---根据引用路径获取AssetBundleData的数据：打包之后，ab路径加载
function AssetBundleManager:GetAssetBundleDataAtPath(abName)
    if abName == nil or abName == '' then
        return nil
    end

    --if string.StartsWith(abName,"test") then
    --    --todo 设置远程下载
    --end

    if self.manifestDatas[abName] then
        return self.manifestDatas[abName]
    end
    return nil
end

return AssetBundleManager