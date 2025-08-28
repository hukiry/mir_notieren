---
---
--- Create Time:2023-03-17
--- Author: Hukiry
---

---@class PassActivity:BaseInfo
local PassActivity = Class(BaseInfo)

function PassActivity:ctor()

end

function PassActivity:InitData()
    ---月份大小，天数，信息
    ---@type table<number, PassInfo>
    self.infoList = {}
    local  tab = SingleConfig.Pass():GetTable()
    for _, v in pairs(tab) do
        ---@type PassInfo
        local info = require("Game.UI.Activity.Info.PassInfo").New(v.id)
        self.infoList[info.id] = info
    end

    self.lastFreeID = 1
    self.lastPayID = 1
end

function PassActivity:SyncRemoteData()
    ---,|,
    local freeId, freeState, payId, payState = 101, 0, 101, 0
    ---id, state
    local canFreeTab = {}
    if not IsEmptyString(self.strValue) then
        local tab = string.Split(self.strValue,'|')
        local len = #tab
        for i = 1, len-1 do
            local temp = string.Split(tab[i],',')
            local id, s = tonumber(temp[1]), tonumber(temp[2])
            canFreeTab[id] = s
        end

        local freeTab, payTab = string.Split(tab[len-1],','), string.Split(tab[len],',')
        freeId, freeState = tonumber(freeTab[1]), tonumber(freeTab[2])
        payId, payState = tonumber(payTab[1]), tonumber(payTab[2])
    end

    if freeState >= 1 then
        freeId = freeId + 1
        freeState = 0
    end

    if payState == 1 then
        payId = payId + 1
        payState = 0
    end

    for i, v in pairs(self.infoList) do
        if i < freeId then
            v.freeState = canFreeTab[v.id]~=nil and canFreeTab[v.id] or 2
        elseif i==freeId then
            v.freeState = freeState
        end

        if i < payId then
            v.payState = 1
        elseif i==payId then
            v.payState = payState
        end
    end

    self.lastFreeID = freeId
    self.lastPayID = payId
end

---@param freeState number 领取状态
---@param payState number 支付状态
function PassActivity:SendState(freeState, payState, callback, id)
    self.lastFreeID = Mathf.Clamp(self.lastFreeID,1, 128)
    self.lastPayID = Mathf.Clamp(self.lastPayID,1, 128)

    id = id or 0
    local str = ''
    freeState = freeState or self.infoList[self.lastFreeID].freeState
    payState = payState or  self.infoList[self.lastPayID].payState
    for i, v in pairs(self.infoList) do
        if v.freeState == 1 and v.id~=id then
            str = str .. v.id .. ',1|'
        end
    end

    if id>0 and id~=self.lastFreeID then
        str = str..id..','..freeState..'|'
    end

    if self.infoList[self.lastFreeID] then
        str = str..self.lastFreeID..','..self.infoList[self.lastFreeID].freeState..'|'
    end

    str = str..self.lastPayID..','..payState
    Single.Request().SendActivity(EActivityType.pass, EHttpActivityState.FinishProgressbar, str, callback)
end


---完成进度同步
function PassActivity:SyncProgress()
    self:SyncRemoteData()
end

---完成新关卡，获得一颗星
function PassActivity:FinishLevelStar()
    if self.infoList[self.lastFreeID] and self.infoList[self.lastFreeID]:IsLock() then
        self.infoList[self.lastFreeID].freeState = 1
        self:SendState(1, nil)
        self.lastFreeID = Mathf.Clamp(self.lastFreeID + 1 ,1, 128)
    end
end

---获取最近滚动索引
function PassActivity:GetPassIndex()
    return self.lastFreeID > self.lastPayID and self.lastFreeID%100 or self.lastPayID%100
end

---支付完成回调
function PassActivity:PayFinish(callback)
    local curId = self.lastPayID
    self.lastPayID = Mathf.Clamp(self.lastPayID + 1 ,1, 128)
    self:SendState(nil, curId==self.lastPayID and 1 or 0, callback)
end

---@param day number 一个月中的第几天
---@return PassInfo
function PassActivity:GetPassInfo(day)
    return self.infoList[day]
end

---@return PassInfo
function PassActivity:GetPayInfo()
    return self:GetPassInfo(self.lastPayID)
end

---@return table<number, PassInfo>
function PassActivity:GetPassArray()
    local temp = table.toArray(self.infoList)
    table.sort(temp, function(a, b) return a.day < b.day end)
    return temp
end

return PassActivity