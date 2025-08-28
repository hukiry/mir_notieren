---
--- ChestActivity       
--- Author : hukiry     
--- DateTime 2023/7/21 21:26   
---

---@class ChestActivity:BaseInfo
local ChestActivity = Class(BaseInfo)
function ChestActivity:ctor()
    --3列集合配置：'101,1|109,200|103,1|202,3|305,1|306,1|308,1|2'
end

function ChestActivity:InitData()
    ---@type table<number, table<number, ChestInfo>>
    self.infoList = {}
    ---@type  table<number, number>
    self.buyStateList = {}
end

function ChestActivity:SyncRemoteData()
    ---物品类型1-10, 数量, -1=免费|...
    local tempTab = string.Split(self.paramsValue,'|')
    ---状态：3,2,2
    local strTab = string.Split(self.strValue,',')
    if not IsEmptyString(self.strValue) then
        for i, v in ipairs(strTab) do
            self.buyStateList[i] = tonumber(v)
        end
    end

    local len = #tempTab-1
    for i = 1, len do
        local tab = string.Split(tempTab[i],',')
        ---@type ChestInfo
        local info = require("Game.UI.Activity.Info.ChestInfo").New(tab, i)
        if self.infoList[info.sort]==nil then
            self.infoList[info.sort] = {}
        end

        if self.buyStateList[1] then
            if info.sort == 1 then
                info.state = 3
            else
                info.state = self.buyStateList[info.sort]
            end
        end
        self.infoList[info.sort][info.id] = info
    end
    ---价格部分
    self.payId = tonumber(tempTab[#tempTab])
end

---完成进度同步
function ChestActivity:SyncProgress()

end

---完成进度同步
---@param sort number
function ChestActivity:RequestProgressbar(sort)
    if self.buyStateList[sort] then
        self.buyStateList[sort] = sort==1 and 3 or 2
        local str = ''
        if sort == 1 then
            str = '3,1,1'
            self.buyStateList[sort] = 3
            for i = 2, 3 do
                self.buyStateList[sort] = 1
            end
        else
            self.buyStateList[sort] = 2
            str = '3,'..self.buyStateList[2]..','..self.buyStateList[3]
        end

        local isFinish = self.buyStateList[1]==3 and self.buyStateList[2]==2 and self.buyStateList[3]==2
        if isFinish then
            Single.Request().SendActivity(self.actType, EHttpActivityState.Finished, str)
        else
            Single.Request().SendActivity(self.actType, EHttpActivityState.FinishProgressbar, str)
        end
    end
end

function ChestActivity:RequestFinished()
    Single.Request().SendActivity(self.actType, EHttpActivityState.Finished)
end


---@return boolean
function ChestActivity:IsBuy()
    local state = self.buyStateList[1] or 0
    return state == 3
end

---@param indexSort number
function ChestActivity:GetState(indexSort)
    return self.buyStateList[indexSort] or 0
end

---@param indexSort number
---@return table<number, ChestInfo>
function ChestActivity:GetArrayInfo(indexSort)
    local temp = table.toArray(self.infoList[indexSort])
    table.sort(temp, function(a, b) return a.index < b.index end)
    return temp
end

function ChestActivity:GetShowPrice()
   return SingleConfig.Recharge():GetShowPrice(self.payId)
end

---@type TableRechargeItem
function ChestActivity:GetRechargeInfo()
    return SingleConfig.Recharge():GetKey(self.payId)
end

return ChestActivity