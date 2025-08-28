--- 可移动道具，使用和生成
--- MatchDragonflyItem       
--- Author : hukiry     
--- DateTime 2024/6/25 16:12   
---


---@class MatchDragonflyItem:MatchNormalBase
local MatchDragonflyItem = Class(MatchNormalBase)

function MatchDragonflyItem:ctor(gameObject)
    ---@type MatchInfo
    self.info = nil
end

---触发事件
function MatchDragonflyItem:TriggerEvent()

end

return MatchDragonflyItem