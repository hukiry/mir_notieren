---
--- GuideInfo       
--- Author : hukiry     
--- DateTime 2023/3/15 14:24   
---

---@class GuideInfo
local GuideInfo = Class()

function GuideInfo:ctor(guideId)
    ---引导id
    ---@type number
    self.id = guideId
    ---引导步骤
    ---@type number
    self.step = guideId%100
    ---引导组id
    ---@type number
    self.groupId = Mathf.Floor(guideId/100)
    ---延时时间
    ---@type number
    self.delayTime = self:GetCfg().delayTime

    self.sceneType = self:GetCfg().type

    self.gameName = SingleConfig.MultiLanguage():GetInfoByCode(Single.SdkPlatform():GetLanguageCode()).gameName

    ---操作要合成的目标道具
    self.targetId = self:GetCfg().targetId
end

---是显示跳过按钮
---@return boolean
function GuideInfo:IsShowSkip()
    return self:GetCfg().delayTime > 0 and self:GetCfg().isSkip
end

---获取当前指引id物品
---@return number, number
function GuideInfo:GetItemiconIds()
    local iconIds = self:GetCfg().iconIds
    return iconIds[1] or 0, iconIds[2] or 0
end

---@return string
function GuideInfo:GetTitle()
    local lanid = EGuideScene[SceneRule.CurSceneType]+23000
    return GetLanguageText(lanid)
end

---是点击类型
---@return boolean
function GuideInfo:IsClickType()
    return self:GetCfg().operateType == EGuideOperate.Click
end

---是地图操作
---@return boolean
function GuideInfo:IsMapOperate()
    return self:GetCfg().operateType == EGuideOperate.Map
end

---@return string
function GuideInfo:GetContent()
    local item = self:GetCfg()
    if item.lanContent==0 then
        return ''
    end

    local startId, endId = self:GetItemiconIds()
    local desc = GetLanguageText(item.lanContent)
    if startId>0 and endId>0 then
        local start,_ = SingleConfig.Item():GetItemNameAndDesc(startId)
        local trail,_ = SingleConfig.Item():GetItemNameAndDesc(endId)
        local startDesc, tralDesc = "<color=green>"..GetLanguageText(start).."</color>","<color=red>"..GetLanguageText(trail).."</color>"
        return GetReplaceFormat(desc, startDesc, tralDesc)
    elseif startId>0 then
        local start,_ = SingleConfig.Item():GetItemNameAndDesc(startId)
        local startDesc = "<color=green>"..GetLanguageText(start).."</color>"
        return GetReplaceFormat(desc, startDesc)
    elseif self.groupId==1 and self.step == 1 then
        local startDesc = "<color=green>"..self.gameName.."</color>"
        return GetReplaceFormat(desc, startDesc)
    end
    return desc
end

---是满足条件
---@return boolean
function GuideInfo:IsFullCondition()
    local isFullLevel = SceneRule.CurSceneType==SceneType.LevelCity and SingleData.Level().selectInfo.levelIndex >= self:GetCfg().lv
    ---关卡条件，合成目标条件，场景条件
    return (SingleData.Level().lastLevelIndex >= self:GetCfg().lv or isFullLevel) and
            EGuideScene[SceneRule.CurSceneType] == self.sceneType and (SingleData.Guide().finishItemID == self:GetCfg().targetId or self:GetCfg().targetId==0)
end

function GuideInfo:IsFullLevel()
    if SceneRule.CurSceneType==SceneType.LevelCity then
        return SingleData.Level().selectInfo.levelIndex == self:GetCfg().lv
    end
    return true
end

---@return table<number>
function GuideInfo:GetMoveToTargetPos()
    return self:GetCfg().targetPos
end

---获取箭头指引坐标
---@return UnityEngine.Vector3, number
function GuideInfo:GetArrowPos()
    local tab = self:GetCfg().arrowPos
    return Vector3.New(tab[1] or 0, tab[2] or 0), tab[3]
end

---@return table<number>
function GuideInfo:GetAreaMask()
    return self:GetCfg().areaMask
end

---@return TableGuideItem
function GuideInfo:GetCfg()
    return SingleConfig.Guide():GetKey(self.id)
end

return GuideInfo