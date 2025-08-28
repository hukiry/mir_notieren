---
---
--- Create Time:2023-10-17
--- Author: Hukiry
---

---@class MetaHomeDataMgr
local MetaHomeDataMgr = Class()

function MetaHomeDataMgr:ctor()
    ---初始化属性

end

--清理数据
function MetaHomeDataMgr:InitData()
	---@type table<number, MetaFightInfo>
    self.infoList = {}
    self.isHave = false

    self.fightState = EMetaFightState.Level
    self.playTime = 0

    self.numberId = 0
end

---@param metaInfos table<number, METAINFO>
function MetaHomeDataMgr:SyncServerData(metaInfos)
    for _, v in ipairs(metaInfos) do
        self.infoList[v.numberId] = require("Game.UI.Metauniverse.Data.MetaFightInfo").New()
        self.infoList[v.numberId]:UpdateInfo(v)
    end

    self.isHave = true
end

function MetaHomeDataMgr:IsFight()
    return self.fightState == EMetaFightState.Fight
end

---退出
function MetaHomeDataMgr:StartQuitFight()
    local fightState = self.fightState
    self.fightState = EMetaFightState.Quit
    return fightState == EMetaFightState.Fight
end

---开始测试
function MetaHomeDataMgr:StartTest(State)
    self.fightState = State or EMetaFightState.Test
end

---开始对战
function MetaHomeDataMgr:StartFight()
    if self.fightState ~= EMetaFightState.Test then
        self.fightState = EMetaFightState.Fight
    end

    Single.Meta().numberId = self.numberId
    local dataTab = Single.Meta():GetMapInfo()
    self.isFlightWin = false
    self.isTestWin = false
    Single.Match():LoadMetaMap(self.fightState, dataTab)
end

---@param metaState EMetaFightState
---@param isWin boolean 胜利
function MetaHomeDataMgr:UpdateResult(metaState, isWin)
    self.fightState = metaState
    local isFight = metaState == EMetaFightState.FightFinish
    local winID = isWin and ViewID.MetaWin or ViewID.MetaFail
    UIManager:OpenWindow(winID, isFight)

    if metaState == EMetaFightState.TestFinish then
        self.isTestWin = isWin
    elseif metaState == EMetaFightState.FightFinish then
        self.isFlightWin = isWin
    end
end

---@param numberId number
---@return MetaFightInfo
function MetaHomeDataMgr:GetMetaHomeInfo(numberId)
    return self.infoList[numberId]
end

function MetaHomeDataMgr:IsHave()
    return  self.isHave
end

---@return table<number, MetaFightInfo>
function MetaHomeDataMgr:GetMetaHomeArray()
    local temp = table.toArray(self.infoList)
    if #temp>1 then
        table.sort(temp, function(a, b) return a.numberId < b.numberId end)
    end
    return temp
end

return MetaHomeDataMgr