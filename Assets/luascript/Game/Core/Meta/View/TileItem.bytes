---
--- 编辑格子
--- TileItem
--- Author : hukiry
--- DateTime 2023/5/9 21:42
---


---管理：item 特效，头顶UI，物品掉落
---@class TileItem:DisplayObjectBase
local TileItem = Class(DisplayObjectBase)

function TileItem:ctor(gameObject)
    ---@type MatchAnimation
    self.animation = require('Game.Core.Match.View.Effect.MatchAnimation').New(gameObject, self)
end

function TileItem:OnEnable(itemResPath)
    self.itemResPath = itemResPath
    ---@type CommonSprite
    self.iconSprite = CommonSprite.New(self:FindSpriteRenderer("icon"), Vector2.New(136,136))
end

---颜色盒子
---@param info TileInfo
function TileItem:UpdateInfo(info)
    self.info = info

    self.iconSprite:LoadSprite(info:GetIcon())
    self.transform:SetPosition(info:GetWorldPos())
    if info.sortLayer ~= EMapLayerView.None then
        self.iconSprite.container.sortingOrder = info.sortLayer == EMapLayerView.Bottom and -2 or 2
    else
        self.iconSprite.container.sortingOrder = 0
    end
end

---设置为半透明
---@param col UnityEngine.Color
function TileItem:SetHalfAlpha(col)
    self.iconSprite.container.color = col
end

---@return MatchAnimation
function TileItem:GetAnimation() return self.animation end

function TileItem:OnDisable()
    if self.iconSprite then
        self.iconSprite:OnDestroy()
        self.transform.localScale = Vector3.one
        GameObjectPool.Put(self.itemResPath, self.gameObject)
        self.iconSprite = nil
    end
end

return TileItem