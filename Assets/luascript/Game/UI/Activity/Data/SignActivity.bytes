---
--- SignActivity
--- Author : hukiry     
--- DateTime 2023/7/21 21:26   
---

---@class SignActivity:BaseInfo
local SignActivity = Class(BaseInfo)
function SignActivity:ctor()

end

function SignActivity:InitData()
    ---@type table<number, SignInfo>
    self.infoList = {}

    self.dayId = 101
    ---0 =未签到， 1=已经签到
    self.dayState = 0
    self.curDay = 1
    local tab = SingleConfig.Sign():GetTable()
    for _, v in pairs(tab) do
        ---@type SignInfo
        local info = require("Game.UI.Activity.Info.SignInfo").New(v.id)
        self.infoList[info.dayId] = info
    end
end

function SignActivity:SyncRemoteData()
    local tab = string.Split(self.strValue, '|')
    self.dayId = tonumber(tab[1] or 100)
    self.curDay = self.dayId%100
    if tab[2] == nil or  not Util.Time().IsSameDay(tonumber(tab[2]), os.time()) then
        self.dayId = self.dayId + 1
        self.curDay = self.dayId%100
        self.dayState = 0
    else
        self.dayState = 1
    end

    for i, v in pairs(self.infoList) do
        if self.curDay > v.day then
            v.state = 2
        elseif self.curDay == v.day then
            v.state = self.dayState==1 and 2 or 1
        end
    end
end

---完成进度同步
function SignActivity:SyncProgress()
    self.dayState = 1
    self.infoList[self.dayId].state = 2
    UIManager:CloseWindow(ViewID.SignMonth)

    local data = self.infoList[self.dayId]
    SingleData.Activity():PlayReward(EActivityType.sign, data:GetRewardType(),  data:GetCfg().rewardNum, HandleParams(function(ty1, num)
        Single.Player():SetMoneyNum(ty1, num)
    end, data:GetRewardType()))
end

---可以签到
function SignActivity:IsCanSign()
    return self.dayState == 0
end

function SignActivity:IsHasRedpoint()
    return self:IsCanSign()
end

---@return table<number, SignInfo>
function SignActivity:GetSignArray()
    local temp, dicTab ={}, self.infoList
    local weekDay = self.curDay - 1
    local minID, maxID = 100+ Mathf.Floor(weekDay/7)+1, 100+Mathf.Floor(weekDay/7)+7
    for i = minID, maxID do
        table.insert(temp, dicTab[i])
    end
    table.sort(temp, function(a, b) return a.day < b.day end)
    return temp
end

return SignActivity