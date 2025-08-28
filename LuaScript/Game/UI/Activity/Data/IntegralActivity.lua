---
--- IntegralActivity       
--- Author : hukiry     
--- DateTime 2023/7/21 21:26   
---

---@class IntegralActivity:BaseInfo
local IntegralActivity = Class(BaseInfo)
function IntegralActivity:ctor()

end


function IntegralActivity:InitData()

end

function IntegralActivity:SyncRemoteData()

end

---完成进度同步
function IntegralActivity:SyncProgress()

end

function IntegralActivity:RequestFinished()
    Single.Request().SendActivity(self.actType, EHttpActivityState.Finished)
end

return IntegralActivity