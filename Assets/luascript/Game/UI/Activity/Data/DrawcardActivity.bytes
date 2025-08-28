---
--- DrawcardActivity       
--- Author : hukiry     
--- DateTime 2023/7/21 21:24   
---

---@class DrawcardActivity:BaseInfo
local DrawcardActivity = Class(BaseInfo)
function DrawcardActivity:ctor()

end

function DrawcardActivity:InitData()
    ---类型，数量
    ---@type table<number,{..}>
    self.itemTab = {}
end

function DrawcardActivity:SyncRemoteData()
    self.timeStemp = tonumber(self.strValue) or 0
    if self.timeStemp > 0 then
       self.state = Util.Time().IsSameDay(Util.Time().GetServerTime(), self.timeStemp) and 2 or 0
    end
end

---完成进度同步
function DrawcardActivity:SyncProgress()
    self:SyncRemoteData()
end

function DrawcardActivity:RequestFinishProgressbar()
    local t = Util.Time().GetServerTime()
    Single.Request().SendActivity(self.actType, EHttpActivityState.FinishProgressbar, tostring(t))
end

function DrawcardActivity:GetArrayInfo()
    self.itemTab = {}
    local temp = {}
    for i = 1, 9 do
        table.insert(temp, i)
    end

    for i = 1, 9 do
       local pos = math.random(1, #temp)
       local key = table.remove(temp, pos)
       local number = key == EMoneyType.gold and 200 or 1
       table.insert(self.itemTab, {itemType = key, itemNum = number})
    end

    return self.itemTab
end

return DrawcardActivity