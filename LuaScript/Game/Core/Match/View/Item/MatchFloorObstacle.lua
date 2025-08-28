--- 草地固定障碍物
--- MatchFloorObstacle       
--- Author : hukiry     
--- DateTime 2024/6/25 16:18   
---


---@class MatchFloorObstacle:MatchObstacleBase
local MatchFloorObstacle = Class(MatchObstacleBase)

function MatchFloorObstacle:ctor(gameObject)
    ---@type MatchInfo
    self.info = nil
end

function MatchFloorObstacle:PlayEffect(x, y, col)
    if self.info then
        if self.info:IsFinish() then
            self:FinishObstacle()
            EventDispatch:Broadcast(UIEvent.Match_RemoveBottom_View, x, y)
        else
            if self.iconSprite then
                self:PlaySound()
                self:PlayExplosion(self.iconSprite.spriteName, self.info:GetWorldPos())
            end
            self:UpdateInfo(self.info)
        end
    end
end

return MatchFloorObstacle