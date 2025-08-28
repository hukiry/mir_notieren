--- 元世界活动
--- MetaActivity       
--- Author : hukiry     
--- DateTime 2023/9/13 11:28   
---

---@class MetaActivity:BaseInfo
local MetaActivity = Class(BaseInfo)
function MetaActivity:ctor()

end

function MetaActivity:InitData()

end

function MetaActivity:SyncRemoteData()

end

---完成进度同步
function MetaActivity:SyncProgress()

end

function MetaActivity:RequestFinished()
    Single.Request().SendActivity(self.actType, EHttpActivityState.Finished)
end

return MetaActivity