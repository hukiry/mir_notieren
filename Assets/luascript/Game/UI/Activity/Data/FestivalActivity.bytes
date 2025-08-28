---
--- FestivalActivity       
--- Author : hukiry     
--- DateTime 2023/7/21 21:26   
---

---@class FestivalActivity:BaseInfo
local FestivalActivity = Class(BaseInfo)
function FestivalActivity:ctor()

end

function FestivalActivity:InitData()

end

function FestivalActivity:SyncRemoteData()

end

---完成进度同步
function FestivalActivity:SyncProgress()

end

function FestivalActivity:RequestFinished()
    Single.Request().SendActivity(self.actType, EHttpActivityState.Finished)
end

return FestivalActivity