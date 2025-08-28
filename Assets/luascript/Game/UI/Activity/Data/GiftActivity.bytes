---
--- GiftActivity       
--- Author : hukiry     
--- DateTime 2023/3/14 16:59   
---

---@class GiftActivity:BaseInfo
local GiftActivity = Class(BaseInfo)

function GiftActivity:ctor()

end

function GiftActivity:InitData()
    ---3个礼包的结束时间
    ---@type table<number, number>
    self.gitTimeList = {}
end

function GiftActivity:SyncRemoteData()
    ---1,2,3 天数
    local array = string.Split(self.paramsValue, ',')
    for i, v in ipairs(array) do
        self.gitTimeList[i] = self.expirateTime - tonumber(v)*3600*24
    end

    local strTab =  string.Split(self.strValue, ',')
    self.index1, self.index2, self.index3 = tonumber(strTab[1] or 0), tonumber(strTab[2] or 0), tonumber(strTab[3] or 0)
end

---完成进度同步
function GiftActivity:SyncProgress()

end

function GiftActivity:RequestFinishProgressbar(index)
    self["index"..index] = index
    local str = self.index1 .. ','..self.index2 .. ','..self.index3
    Single.Request().SendActivity(self.actType, EHttpActivityState.FinishProgressbar, str)
    local isFinish = true
    for i = 1, 3 do
        if not self:IsBuy(i) then
            isFinish = false
            break
        end
    end

    if isFinish then
        self:RequestFinished()
    end
end

function GiftActivity:RequestFinished()
    Single.Request().SendActivity(self.actType, EHttpActivityState.Finished)
end

----@return boolean
function GiftActivity:IsBuy(index)
    local indexV = self["index"..index]
    return indexV == index
end

---@param index number 礼包索引
function GiftActivity:_GiftRemaintime(index)
    local t = self.gitTimeList[index]
    return self.expirateTime - t
end

---获取礼包卡剩余时间
---@param index number 礼包索引
---@return string
function GiftActivity:GetGiftRemaintime(index)
    local t = self:_GiftRemaintime(index)
    return Util.Time().GetTimeStringBySecond(t)
end

---是礼包卡结束
---@param index number 礼包索引
---@return boolean
function GiftActivity:IsEndGift(index)
    local t =  self:_GiftRemaintime(index)
    return t <= 0
end

---@return table<number, ShopInfo>
function GiftActivity:GetGiftArray()
    return SingleData.Shop():GetGiftArray()
end


return GiftActivity