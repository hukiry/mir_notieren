--- 固定障碍物
--- MatchFixedObstacle       
--- Author : hukiry     
--- DateTime 2024/6/25 16:18   
---

---@class MatchFixedObstacle:MatchObstacleBase
local MatchFixedObstacle = Class(MatchObstacleBase)

function MatchFixedObstacle:ctor(gameObject)
    ---@type MatchInfo
    self.info = nil
end

return MatchFixedObstacle