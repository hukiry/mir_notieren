---
---
--- Create Time:2023-03-10
--- Author: Hukiry
---

require("Game.UI.Guide.GuideRule")
---@class GuideDataMgr
local GuideDataMgr = Class()

function GuideDataMgr:ctor()
    ---初始化属性
    self:InitData()
end

--清理数据
function GuideDataMgr:InitData()
    ---引导组，引导步骤，信息
	---@type table<number, table<number, GuideInfo>>
    self.infoList = {}
    local  tab = SingleConfig.Guide():GetTable()
    for _, v in pairs(tab) do
        ---@type GuideInfo
        local info = require("Game.UI.Guide.Data.GuideInfo").New(v.id)
        if  self.infoList[info.groupId] == nil then
            self.infoList[info.groupId] ={}
        end
        self.infoList[info.groupId][info.step] = info
    end

    ---引导组索引
    ---@type number
    self.curGroupIndex = 1
    ---引导步骤索引
    ---@type number
    self.stepIndex = 1
    ---引导状态
    ---@type number
    self.groupState = 0
    ---启动引导：用于调试
    ---@type boolean
    self.isEnableGuide = true

    ---是正在播放引导
    ---@type boolean
    self.isPlayGuiding = false

    ---引导界面步骤对象
    ---@type table<number, UnityEngine.GameObject>
    self.stepObjectList = {}

    ---完成的合并物品
    self.finishItemID = 0
    ---操作的索引坐标
    self.coordIndex = 1
end

---@param groupIndex number
function GuideDataMgr:SyncServerData(groupIndex)
    self.curGroupIndex = groupIndex%100
    self.groupState = Mathf.Floor(groupIndex/100)
end

---关闭引导
function GuideDataMgr:CloseGuide()
    Single.TimerManger():RemoveHandler(self)
    self.isPlayGuiding = false

    if self.arrowItem then
        SceneItemPool.Put(SceneItemType.SceneArrowItem, self.arrowItem)
        self.arrowItem = nil
    end
end

function GuideDataMgr:CheckGuide()
    ---家园检查，关卡检查，
    if self.isEnableGuide then
        self:StartGuide()
    end
end

function GuideDataMgr:StartGuide()
    if self.isPlayGuiding then
        return
    end
    self.isPlayGuiding = true

    if self.groupState == 1 then
        self.curGroupIndex = self.curGroupIndex + 1
        self.groupState = 0
        self.stepIndex = 1
    end

    ---取引导数据
    if self.infoList[self.curGroupIndex] then
        self:PlayGuideStep(self:GetGuideArray(self.curGroupIndex))
    end
end

---开始播放引导步骤
---@param groupTab table<number, GuideInfo>
function GuideDataMgr:PlayGuideStep(groupTab)
    local info = groupTab[self.stepIndex]
    if info == nil then
        self:GuideDone()
        return
    end

    local isPass = true
    if info:GetCfg().operateType == 1 then
        isPass = self:GetClickHandler(self.stepIndex)~=nil
    end
    ---满足条件执行引导
    if info:IsFullCondition() and isPass then
        UIManager:OpenWindow(ViewID.Guide, self.stepIndex,  info, Handle(self, self.NextStep, groupTab))
    else
        self:CreateArrow(info:IsFullLevel())
        ---等待循环执行下一次引导
        self:ContineGuide()
    end
end

function GuideDataMgr:NextStep(groupTab)
    self.stepIndex = self.stepIndex + 1
    self:PlayGuideStep(groupTab)
end

---引导组完成：执行下一组引导
function GuideDataMgr:GuideDone()
    self.groupState = 1
    self.coordIndex = 1
    SingleNet.Login().SendGuide(self.curGroupIndex)
    self:ContineGuide()
end

function GuideDataMgr:ContineGuide()
    self.isPlayGuiding = false
    Single.TimerManger():DoTime(self, self.StartGuide, 1, 1)
end

---添加可以点击的按钮：ui
---@param step number 执行步骤
---@param gameObject UnityEngine.GameObject 按钮对象
function GuideDataMgr:AddClickHandler(groupIndex, step, gameObject)
    if self.stepObjectList[groupIndex] == nil  then
        self.stepObjectList[groupIndex] = {}
    end
    self.stepObjectList[groupIndex][step] = gameObject
    if not self.isPlayGuiding then
        Single.TimerManger():RemoveHandler(self, self.StartGuide)
        self:StartGuide()
    end
end

---回调对象：ui
---@param step number 步骤id
---@return UnityEngine.GameObject
function GuideDataMgr:GetClickHandler(step)
    if self.stepObjectList[self.curGroupIndex] then
        return self.stepObjectList[self.curGroupIndex][step]
    end
    return nil
end

---设置完成目标:场景
function GuideDataMgr:FinishTarget(itemId, isClick)
    if self:IsMapOperate() then
        if not self.isPlayGuiding then
            if self.coordIndex >= 4 or isClick then
                Single.TimerManger():RemoveHandler(self)
                self.finishItemID = itemId
                self.isPlayGuiding = false
                self.stepIndex = self.stepIndex + 1
                self:StartGuide()
            else
                self.coordIndex = self.coordIndex + 2
            end

            if self.arrowItem then
                SceneItemPool.Put(SceneItemType.SceneArrowItem, self.arrowItem)
                self.arrowItem = nil
            end
        end
    end
end

---初始化引导的创建:场景
function GuideDataMgr:CreateArrow(isFull)
    local info = self.infoList[self.curGroupIndex][self.stepIndex]
    if self.arrowItem==nil and info and isFull then
        if self:IsMapOperate() and info.targetId > 0 then
            local x, y =self:GetItemCoord()
            if self.arrowItem == nil and x~=1000 then
                ---@type SceneArrowItem
                self.arrowItem = SceneItemPool.Get(SceneItemType.SceneArrowItem, UIManager:GetRootGameObject())
                self.arrowItem:OnEnable(Util.Map().IndexCoordToWorld(x, y), self.curGroupIndex == 2)
            end
        end
    end
end

---地图一操作:场景
function GuideDataMgr:IsMapOperate()
    return self.curGroupIndex <=2 and self.groupState == 0 and SceneRule.CurSceneType == SceneType.LevelCity
end

---获取坐标:场景
---@return number,  number
function GuideDataMgr:GetItemCoord()
    if self.infoList[self.curGroupIndex] then
        local info = self.infoList[self.curGroupIndex][self.stepIndex]
        if info then
            local posTab = info:GetCfg().arrowPos
            return posTab[self.coordIndex] or 1000 , posTab[self.coordIndex + 1] or 1000
        end
    end
    return 0, 0
end

function GuideDataMgr:IsMoveToCoord(x, y)
    if SingleData.Guide():IsMapOperate() then
        local info = self.infoList[self.curGroupIndex][self.stepIndex]
        if info then
            local coords = info:GetMoveToTargetPos()
            if coords[self.coordIndex]==x and coords[self.coordIndex + 1]==y or #coords == 0 then
                return true
            else
                return false
            end
        end
    end
    return true
end

---获取组的步骤集合
---@param groupIndex number
---@return table<number, GuideInfo>
function GuideDataMgr:GetGuideArray(groupIndex)
    local temp = table.toArray(self.infoList[groupIndex])
    table.sort(temp, function(a, b) return a.step < b.step end)
    return temp
end

return GuideDataMgr