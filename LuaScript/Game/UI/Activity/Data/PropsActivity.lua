---
--- PropsActivity
--- Author : hukiry     
--- DateTime 2023/7/21 21:27   
---

---@class PropsActivity:BaseInfo
local PropsActivity = Class(BaseInfo)
function PropsActivity:ctor()

end

function PropsActivity:InitData()
    ---参数配置：id， 目标数量，奖励数量
end

function PropsActivity:SyncRemoteData()
    ---道具id-物品表,数量， 金币
    local array = string.Split(self.paramsValue, ',')
    self.itemId, self.itemNum, self.coinNum = tonumber(array[1]), tonumber(array[2]), tonumber(array[3])
    self.curNum = IsEmptyString(self.strValue) and 0 or tonumber(self.strValue)
end

---完成进度同步
function PropsActivity:SyncProgress()

end

function PropsActivity:IsCanGet()
    return self.curNum >= self.itemNum
end

function PropsActivity:AddBornItem(id)
    if self.itemId == id then
        self.curNum = self.curNum + 1
        if self.curNum >= self.itemNum then
            self.curNum = self.itemNum
        end
        local str = tostring(self.curNum)
        Single.Request().SendActivity(self.actType, EHttpActivityState.FinishProgressbar, str)
    end
end

function PropsActivity:RequestFinished()
    Single.Request().SendActivity(self.actType, EHttpActivityState.Finished)
end

function PropsActivity:GetItemIcon()
    local info = Single.Match():GetMapConifg():GetItemInfo(self.itemId)
    if info then
        return info.icon
    end
    return ''
end

return PropsActivity