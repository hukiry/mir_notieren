---
--- MatchEffectItem       
--- Author : hukiry     
--- DateTime 2023/5/21 17:46   
---

---@class MatchEffectItem:DisplayObjectBase
local MatchEffectItem = Class(DisplayObjectBase)

function MatchEffectItem:ctor(gameObject, resItemPath, _self)
    self.resItemPath = resItemPath
    ---@type PropsRainbowBall
    self.propsView = _self
end

---彩虹特效移动
---@param finishCall function 回调
---@param endPos UnityEngine.Vector3
---@param resEffectPath string 资源路径
function MatchEffectItem:PlayEffectFly(finishCall, endPos, resEffectPath)
    self.resEffectPath = resEffectPath
    self.finishCall = finishCall
    self.transform:DOLocalMove(endPos,0.5):OnComplete(function()
        GameObjectPool.Put(self.resEffectPath, self.gameObject)
        self.finishCall()
    end)
end


---特效移动
---@param resEffectPath string 资源路径
function MatchEffectItem:PlayEffect(resEffectPath, duration)
    self.resEffectPath = resEffectPath
    self.transform:DOScale(Vector3.one, duration):OnComplete(function()
        GameObjectPool.Put(self.resEffectPath, self.gameObject)
    end)
end

---物品移动
---@param index string 图标
function MatchEffectItem:PlayRocketItem(coordTab, index)
    if self.iconSprite == nil then
        ---@type CommonSprite
        self.iconSprite = CommonSprite.New(self:FindSpriteRenderer("icon"), Vector2.New(140,140))
        self.iconSprite:LoadSprite("rocket")
    end

    local coord = coordTab[index]
    if coord then
        local endPos = Util.Map().Vector2IntCoordToWorld(coord)
        self.transform:DOLocalMove(endPos,0.1):OnComplete(Handle(self,
                self.FinishRocketStep, coordTab, index, coord))
    else
        self:OnDisable()
    end
end

function MatchEffectItem:FinishRocketStep(coordTab, index, coord)
    self.nextStepCallBack(coord.x, coord.y, nil)
    self:PlayRocketItem(coordTab, index + 1)
end

function MatchEffectItem:OnDisable()
    if self.iconSprite then
        self.iconSprite:OnDestroy()
        self.iconSprite = nil
    end
    self.nextStepCallBack = nil
    self.transform:SetRotation(Vector3.zero)
    GameObjectPool.Put(self.resItemPath, self.gameObject)
end

return MatchEffectItem
