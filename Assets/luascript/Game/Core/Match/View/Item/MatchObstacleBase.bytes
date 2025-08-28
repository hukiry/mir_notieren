---
--- MatchObstacleBase       
--- Author : hukiry     
--- DateTime 2024/6/26 13:37   
---

---@class MatchObstacleBase:MatchBase
MatchObstacleBase = Class(MatchBase)

function MatchObstacleBase:ctor(gameObject)
    ---@type MatchInfo
    self.info = nil
end

---初始化
function MatchObstacleBase:OnEnable(itemResPath)
    self.itemResPath = itemResPath
    ---@type CommonSprite
    self.iconSprite = CommonSprite.New(self:FindSpriteRenderer("icon"), self.Size)

    for i, _ in ipairs(self.fourCoord) do
        self.fourView[i] = self["iconSprite"..i]
    end
end

---更新视图
---@param info MatchInfo
function MatchObstacleBase:UpdateInfo(info, oldPos)
    self.info = info
    if self.info == nil then --障碍物下更新数据
        self:OnDisable()
        return
    end

    self:UpdateBefore()

    self.iconSprite:LoadSprite(info:GetIcon())
    if oldPos then
        self.transform:SetPosition(oldPos)
        self.transform:DOLocalMove(info:GetWorldPos(), 0.2):SetEase(Ease.Line)
    else
        --设置新位置
        self.transform:SetPosition(info:GetWorldPos())
    end

    self:UpdateAfter(info)
end

---点击播放振动
function MatchObstacleBase:PlayShake(x, y)
    self.transform:DOShakeScale(0.2, 0.3, 4)
end

---播放特效
function MatchObstacleBase:PlayEffect(x, y, col)
    local iconGo =  nil
    local barrierType = self.info:GetCfgInfo().barrierType
    if barrierType == EObstacleType.Fixed then
        if self.info.count <= 0 then
            iconGo =  self.iconSprite
        else
            iconGo =  self["iconSprite"..self.info.count]
        end
    elseif barrierType == EObstacleType.Move then
        iconGo =  self.iconSprite
    end

    if iconGo and iconGo.spriteNameCopy then
        local pos = Util.Map().IndexCoordToWorld(self.info.x, self.info.y)
        self:PlaySound()
        self:PlayExplosion(iconGo.spriteNameCopy, pos, HandleParams(function(itemV, info)  itemV:UpdateInfo(info) end, self, self.info))
    end
end