--- 固定障碍物盒子
--- MatchBoxObstacle       
--- Author : hukiry     
--- DateTime 2024/6/25 16:19   
--- 331 334

---@class MatchBoxObstacle:MatchBase
local MatchBoxObstacle = Class(MatchObstacleBase)

function MatchBoxObstacle:ctor(gameObject)
    ---@type MatchInfo
    self.info = nil

    self.boxTab = {"horizontal","horizontal","xie"}
end

---初始化
function MatchBoxObstacle:OnEnable(itemResPath)
    self.itemResPath = itemResPath
    ---@type CommonSprite
    self.iconSprite = CommonSprite.New(self:FindSpriteRenderer("icon"), self.Size)
    for i = 1, 3 do
        self["iconSprite"..i] = CommonSprite.New(self:FindSpriteRenderer("icon"..i), Vector2.New(140,140))
    end

end

---更新视图
---@param info MatchInfo
function MatchBoxObstacle:UpdateInfo(info, oldPos)
    self.info = info
    if self.info == nil then --障碍物下更新数据
        self:OnDisable()
        return
    end

    ---盒子
    self:_UpdateBox(info)

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

---颜色盒子
---@param info MatchInfo
function MatchBoxObstacle:_UpdateBox(info)
    for i = 1, 3 do
        self["iconSprite"..i]:SetToEmpty()
    end

    local v = info.count-1
    for i = 1, v do
        local spName = info:GetIcon().."_"..self.boxTab[i]
        self["iconSprite"..i]:LoadSprite(spName)
        self["iconSprite"..i].transform:SetPosition(SingleConfig.ItemCoord():GetCoord(info.itemId, i))
    end
end


return MatchBoxObstacle