---
--- MatchBase 基类提供方法
--- Created by Administrator.
--- DateTime: 2024/6/25 15:29
---


---@class MatchBase:DisplayObjectBase
MatchBase = Class(DisplayObjectBase)

function MatchBase:ctor(gameObject)
    ---@type MatchInfo
    self.info = nil
    ---四个格子的坐标
    ---@type table<number, UnityEngine.Vector2>
    self.fourCoord = {
        Vector2.New(0,0),
      Vector2.New(1,0),
      Vector2.New(0,-1),
      Vector2.New(1,-1)
    }
    ---@type MatchAnimation
    self.animation = require('Game.Core.Match.View.Effect.MatchAnimation').New(gameObject, self)

    ---四个格子
    ---@type table<number, CommonSprite>
    self.fourView = {}

    self.Size =Vector2.New(136,136)
    ---@type CommonSprite
    self.iconSprite = nil
    ---@type string
    self.itemResPath = nil
end

---初始化
function MatchBase:OnEnable()
end

function MatchBase:UpdateBefore()
    self.transform.localScale = Vector3.one
    self.transform:SetRotation(Vector3.zero)
    self.iconSprite.transform:SetScale(Vector3.one)
end

---更新视图
---@param info MatchInfo
function MatchBase:UpdateAfter(info)
    self.info = info

    if self.info.sortLayer ~= EMapLayerView.None then
        self.iconSprite.container.sortingOrder = self.info.sortLayer == EMapLayerView.Bottom and -2 or 2
    else
        self.iconSprite.container.sortingOrder = 0
    end
end

---设置物品层级
function MatchBase:SetSortingOrder()
    self.iconSprite.container.sortingOrder = 100
end

function MatchBase:UpdateInfo(info, oldPos)  end
---点击振动
function MatchBase:PlayShake(x, y) end
---播放特效
function MatchBase:PlayEffect(x, y, col) end
---触发事件
function MatchBase:TriggerEvent() end
---合成新道具的动画
---@param pos UnityEngine.Vector3
function MatchBase:MoveAnimaton(pos, len, info, itemTarget)
    self.transform:DOLocalMove(pos,0.14)
    self.transform:DOScale(Vector3.one*0.9,0.14):OnComplete(function()
        self:_FinishMoveGen( len, info, itemTarget)
    end)
end

---@param item MatchBase
---@param info MatchInfo
function MatchBase:_FinishMoveGen(len, info, item)
    item.index = item.index + 1
    if item.index == len-1 then
        item:PlaySound()
        item:UpdateInfo(info)
        item:GetAnimation():PlayEffect(info:GetWorldPos(), ESceneEffectRes.Born_props)
        Single.Match():GetMapTarget():UpdateTask(ETaskType.Props, info:GetCfgInfo().itemProps)
    end
end

---爆炸效果
---@param spriteName string 精灵图标
---@param pos UnityEngine.Vector3 指定当前位置
---@param finishCall function
function MatchBase:PlayExplosion(spriteName, pos, finishCall)
    local resPath = "prefab/Scene/ExplosionItem"
    ---@type UnityEngine.GameObject
    local go = GameObjectPool.Get(resPath, self.gameObject)
    go.transform:SetPosition(pos, false)

    local paramsAction = HandleParams(function(goParams, finishCallParams)
        local forceCall = function() return math.random(20,100)  end
        local radiusCall = function() return math.random(1,5)  end
        GameObject.Destroy(goParams)
        --ExplosionForce.Instance:PlayExplosion(goParams, forceCall, radiusCall, math.random(6,8))
        if finishCallParams then
            finishCallParams()
        end
        StartCoroutine(function()
            WaitForFixedUpdate()
            ResManager:Unload(resPath)
        end)
    end, go, finishCall)

    ---@type CommonSprite
    local common = CommonSprite.New(go:GetComponent("SpriteRenderer"), nil, true)
    common.resetBoxCollider2D = true
    common:LoadSprite(spriteName, nil, paramsAction)
end

---播放动物动画：更新目标数量
function MatchBase:FinishAnimal()
    ---@param target TargetItem
    local finishCall = function(target)
        target:Update()
    end
    local startPos, iconName = self.transform.position, self.iconSprite.spriteName
    ---@type TargetItem
    local target = UIManager:GetActiveWindow(ViewID.LevelMain):GetTargetItem(self.info.itemId)
    if not target:IsFinish() then
        Single.Animation():PlayItem(startPos, target.transform, iconName, EAnimationFly.SceneToView,nil,
                HandleParams(finishCall, target))
    end
    self:OnDisable()
end

---更新目标数量
---@param targetIcon string 精灵图标
---@param isFly boolean 是飞起动画
---@param isCard boolean
function MatchBase:FinishObstacle(targetIcon, isFly, isCard)
    ---@type TargetItem
    local target = UIManager:GetActiveWindow(ViewID.LevelMain):GetTargetItem(self.info.itemId)
    local finishCall = function(target1)
        target1:Update()
    end

    local pos = Util.Map().IndexCoordToWorld(self.info.x, self.info.y)
    if not target:IsFinish() then
        if isFly then
            Single.Animation():PlayItem(pos, target.transform, targetIcon, EAnimationFly.SceneToView,nil,
                    HandleParams(finishCall, target), isCard)
        else
            finishCall(target)
        end
    end
end

---播放声音
function MatchBase:PlaySound()
    if self.info then
        if self.info:IsNormal() then
            Single.Sound():PlaySound(ESoundResType.item_poshui)
        elseif self.info:IsJar() or self.info:IsBowl() then
            Single.Sound():PlaySound(ESoundResType.Bottle_Poshui)
        elseif self.info:GetCfgInfo():IsBarrier() then
            local barrierType = self.info:GetCfgInfo().barrierType
            if barrierType == EObstacleType.Bubble then
                Single.Sound():PlaySound(ESoundResType.ClickBubble)
            elseif barrierType == EObstacleType.Born then
                Single.Sound():PlaySound(ESoundResType.Card)
            else
                Single.Sound():PlaySound(ESoundResType.Obstacle_poshui)
            end
        end
    end
end

---移除视图 并回收对象
function MatchBase:OnRemoveView()
    self:PlaySound()
    local pos = Util.Map().IndexCoordToWorld(self.info.x, self.info.y)
    self.transform:DOScale(Vector3.zero,0.2):OnComplete(function()
        self:PlayExplosion(self.iconSprite.spriteName, pos)
        self.animation:PlayEffect(pos, ESceneEffectRes.Match_wipe)
        self:OnDisable()
    end)
end


---释放或回收对象
function MatchBase:OnDisable()
    if self.iconSprite then
        self.iconSprite:OnDestroy()
        for i = 1, 5 do
            if self["iconSprite"..i] then
                self["iconSprite"..i]:SetToEmpty()
            end
        end
        self.fourView = {}
        self.transform.localScale = Vector3.one
        GameObjectPool.Put(self.itemResPath, self.gameObject)
        self.iconSprite = nil
    end
end

---@return MatchAnimation
function MatchBase:GetAnimation() return self.animation end

---消耗资源
function MatchBase:OnDestroy()
    self:OnDisable()
end