---
--- MetaFightInfo
--- Author : hukiry     
--- DateTime 2023/10/17 17:58   
---

---@class MetaFightInfo
local MetaFightInfo = Class()

function MetaFightInfo:ctor()
    ---发起挑战者id
    self.id = 0
    ---地图点赞数量
    self.likeNum = 0
    ---地图id
    self.numberId = 0
    ---挑战状态，0=未接受挑战，1=接受过挑战
    self.state = 0
end

---@param msg METAINFO
function MetaFightInfo:UpdateInfo(msg)
    for key, v in pairs(msg) do
        if self[key] then
            self[key] = v
        end
    end
end

return MetaFightInfo