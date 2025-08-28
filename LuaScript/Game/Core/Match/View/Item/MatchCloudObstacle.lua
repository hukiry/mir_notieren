--- 漂浮物固定障碍物
--- MatchCloudObstacle       
--- Author : hukiry     
--- DateTime 2024/6/25 16:18   
---


---@class MatchCloudObstacle:MatchObstacleBase
local MatchCloudObstacle = Class(MatchObstacleBase)

function MatchCloudObstacle:ctor(gameObject)
    ---@type MatchInfo
    self.info = nil
end

---更新视图
---@param info MatchInfo
function MatchCloudObstacle:UpdateAfter(info)
    self.info = info

    if self.info.sortLayer ~= EMapLayerView.None then
        self.iconSprite.container.sortingOrder = self.info.sortLayer == EMapLayerView.Bottom and -2 or 2
    else
        self.iconSprite.container.sortingOrder = 0
    end

    ---云层
    if  self.iconSprite then
        self.iconSprite.container.color = self.info.itemId == 3602 and Color.New(1,1,1,0.7) or Color.New(1,1,1,1)
    end
end

---播放振动
function MatchCloudObstacle:PlayShake(x, y)
    Single.Sound():PlaySound(ESoundResType.ClickBubble)
    self.transform:DOShakeScale(0.2, 0.3, 4)
end

---播放特效
function MatchCloudObstacle:PlayEffect(x, y, col)
    if self.info then
        if self.info:IsFinish() then
            self:FinishObstacle(self.info)
            EventDispatch:Broadcast( UIEvent.Match_RemoveFloat_View, x, y)
        else
            if self.iconSprite then
                self:PlaySound()
                self:PlayExplosion(self.iconSprite.spriteName, self.info:GetWorldPos())
            end
            self:UpdateInfo(self.info)
        end
    end
end


return MatchCloudObstacle