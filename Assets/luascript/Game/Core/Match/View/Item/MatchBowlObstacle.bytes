--- 固定4个格子障碍物
--- MatchBowlObstacle       
--- Author : hukiry     
--- DateTime 2024/6/25 16:19   
---


---@class MatchBowlObstacle:MatchBase
local MatchBowlObstacle = Class(MatchBase)

function MatchBowlObstacle:ctor(gameObject)
    ---@type MatchInfo
    self.info = nil
end

---初始化
function MatchBowlObstacle:OnEnable(itemResPath)
    self.itemResPath = itemResPath
    ---@type CommonSprite
    self.iconSprite = CommonSprite.New(self:FindSpriteRenderer("icon"), self.Size)

    for i = 1, 5 do
        self["iconSprite"..i] = CommonSprite.New(self:FindSpriteRenderer("icon"..i), Vector2.New(140,140))
    end

    for i, _ in ipairs(self.fourCoord) do
        self.fourView[i] = self["iconSprite"..i]
    end
end

---更新视图
---@param info MatchInfo
function MatchBowlObstacle:UpdateInfo(info, oldPos)
    self.info = info
    if self.info == nil then --障碍物下更新数据
        self:OnDisable()
        return
    end

    self.iconSprite:LoadSprite(info:GetIcon(), nil , Handle(self, self.UpdateBefore))

    if oldPos then
        self.transform:SetPosition(oldPos)
        self.transform:DOLocalMove(info:GetWorldPos(), 0.2):SetEase(Ease.Line)
    else
        self.transform:SetPosition(info:GetFourPos())
    end

    self:UpdateAfter(info)

    self:_UpdateBowl()
end

---更新四个格子的视图：碗
function MatchBowlObstacle:_UpdateBowl()
    if self.info.count ~= self.info:GetCfgInfo().count then
        if self.info.count < self.info:GetCfgInfo().count-1  then
            for index, v in ipairs(self.fourCoord) do
                if self.info:IsHasCoord(v.x, v.y) and self.fourView[index] then
                    self.fourView[index]:SetToEmpty()
                    self.fourView[index]=nil
                end
            end
        end
        self["iconSprite5"]:SetToEmpty()
    else--完整加载
        for i = 1, self.info.count-1 do
            self["iconSprite"..i]:LoadSprite("bowl")
            self["iconSprite"..i].transform:SetPosition(SingleConfig.ItemCoord():GetCoord(self.info.itemId, i))
        end

        self["iconSprite5"]:LoadSprite("bowl_door", nil , function()
            self["iconSprite5"].transform:SetPosition(Vector3.zero)
            self["iconSprite5"].transform:SetScale(Vector3.one)
        end)
    end
end

---四个格子
---@return number
function MatchBowlObstacle:_GetFourIndex(x, y)
    for i, v in ipairs(self.fourCoord) do
        if v.x==x and v.y==y then
            return i
        end
    end
    return -1
end

---播放振动
function MatchBowlObstacle:PlayShake(x, y)
    local index = self:_GetFourIndex(x-self.info.x, y-self.info.y)
    ---@type UnityEngine.GameObject
    local item =  self["iconSprite"..index].gameObject
    if self.info.count == self.info:GetCfgInfo().count then
        item = self.gameObject
    end

    if item then
        item.transform:DOShakeScale(0.2, 0.3, 4)
    end
end


---播放特效
---@param x number 播放的x坐标
---@param y number 播放的y坐标
function MatchBowlObstacle:PlayEffect(x, y, col)
    local sprite = nil
    if self.info.count<=0 then
        sprite = self.iconSprite
    elseif not self["iconSprite5"]:IsDestroy() then
        sprite = self["iconSprite5"]
    else
        local index = self:_GetFourIndex(x-self.info.x, y-self.info.y)
        if index>0 then
            sprite = self["iconSprite"..index]
        end
    end

    if self.info:IsNeedUpdate(x-self.info.x, y-self.info.y)  then
        self:FinishObstacle()
    else
        sprite = nil
    end

    if sprite and sprite.spriteNameCopy then
        local pos = Util.Map().IndexCoordToWorld(x, y)
        if self.info.count == self.info:GetCfgInfo().count-1 then
            pos = self.info:GetWorldPos()
        end
        self:PlaySound()
        self:PlayExplosion(sprite.spriteNameCopy, pos,
                HandleParams(function(itemV1, info1)  itemV1:UpdateInfo(info1) end, self, self.info))
    end
end

return MatchBowlObstacle