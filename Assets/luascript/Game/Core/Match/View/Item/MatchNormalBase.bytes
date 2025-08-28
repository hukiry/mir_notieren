---
--- MatchNormalBase       
--- Author : hukiry     
--- DateTime 2024/6/26 13:37   
---

---@class MatchNormalBase:MatchBase
MatchNormalBase = Class(MatchBase)

function MatchNormalBase:ctor(gameObject)
    ---@type MatchInfo
    self.info = nil
end

---初始化
---@param itemResPath string
function MatchNormalBase:OnEnable(itemResPath)
    self.itemResPath = itemResPath
    ---@type CommonSprite
    self.iconSprite = CommonSprite.New(self:FindSpriteRenderer("icon"), self.Size)
end

---更新视图 info=nil 数据为空时，移除视图
---@param oldPos UnityEngine.Vector3 原始位置
---@param info MatchInfo
function MatchNormalBase:UpdateInfo(info, oldPos)
    self.info = info
    if self.info == nil then --障碍物下更新数据
        self:OnDisable()
        return
    end

    self:UpdateBefore()
    self.iconSprite:OnDestroy()
    self.iconSprite:LoadSprite(info:GetIcon())
    if oldPos then
        self.transform:SetPosition(oldPos)
        self.transform:DOComplete()
        self.transform:DOLocalMove(info:GetWorldPos(), 0.1):SetEase(Ease.Line)
    else
        --设置新位置
        self.transform:SetPosition(info:GetWorldPos())
    end

    self:UpdateAfter(info)

end

---点击播放振动
function MatchNormalBase:PlayShake(x, y)
    self.transform:DOShakeScale(0.2, 0.3, 4)
end

---AI提示振动
function MatchNormalBase:TipShake(x, y)
    local pos = self.transform.localPosition
    local newPos = Vector3.New(pos.x + 0.04,pos.y - 0.04, pos.z)
    self.transform:DOKill()
    self.transform:DOLocalMove(newPos,0.2):SetEase(Ease.InCubic):OnComplete(HandleParams(function(item, x1, y1)
        item.transform:DOLocalMove(Util.Map().IndexCoordToWorld(x1, y1), 0.2):SetEase(Ease.OutCubic)
    end, self, x, y))
end

---播放特效
function MatchNormalBase:PlayEffect(x, y, col)
    local pos = Util.Map().IndexCoordToWorld(self.info.x, self.info.y)
    self:PlayExplosion(self.info:GetIcon(), pos)
    return true
end