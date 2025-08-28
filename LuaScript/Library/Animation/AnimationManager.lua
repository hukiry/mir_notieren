--- 场景到UI，UI到UI， ui到场景， 场景到场景
--- AnimationManager       
--- Author : hukiry     
--- DateTime 2022/12/15 13:37   
---

---@class Eitem_fly_data
Eitem_fly_data = {
    ---播放飞起的图标次数
    count = 1,
    ---图标尺寸
    size = 2,
    ---图标名
    iconName = 3,
}
---@class AnimationManager
local AnimationManager = Class()

function AnimationManager:ctor()
    ---@type table<number, function>
    self.funcList = {}
end


---物品播放动画
---@param iconName string 图标名
---@param pos UnityEngine.Vector3 世界坐标位置(场景和UGUI)
---@param targetPos UnityEngine.Vector3 世界坐标位置(场景和UGUI)
---@param targetPos UnityEngine.GameObject 目标transform
---@param animationFly EAnimationFly 视图类型
---@param finishCall function
---@param size UnityEngine.Vector2
function AnimationManager:PlayItemToScene(pos, targetPos ,targetGo, iconName, animationFly, size, finishCall)
    local startPos = pos
    if animationFly == EAnimationFly.SceneToView then
        startPos = Util.Camera().WorldToUGUIPoint(UIManager:GetCanvasParent(), pos)
    elseif animationFly == EAnimationFly.ViewToScene then
        startPos = Util.Camera().UGUIPointToWorld(pos)
    end

    if iconName and targetGo then
        if animationFly == EAnimationFly.ViewToScene or animationFly == EAnimationFly.SceneToScene then
            ---@type SceneFlyItem
            local flyItem = SceneItemPool.Get(SceneItemType.SceneFlyItem, targetGo, iconName, size)
            flyItem.finishCall = finishCall
            flyItem:OnEnable(startPos, targetPos, EMoneyType.None)
        else
            ---@type ViewFlyItem
            local flyItem = UIItemPool.Get(UIItemType.ViewFlyItem, targetGo, iconName, size)
            flyItem.finishCall = finishCall
            flyItem:OnEnable(startPos, targetPos)
        end
    else
        logError("resource is not exit：", iconName, animationFly)
    end
end

---物品播放动画
---@param iconName string 图标名
---@param pos UnityEngine.Vector3 世界坐标位置(场景和UGUI)
---@param targetTf UnityEngine.Transform 目标transform
---@param animationFly EAnimationFly 视图类型
---@param finishCall function
---@param size UnityEngine.Vector2
---@param isCard boolean 是卡片
function AnimationManager:PlayItem(pos, targetTf , iconName, animationFly, size, finishCall, isCard)
    isCard = isCard or false
    local startPos = pos
    if animationFly == EAnimationFly.SceneToView then
        startPos = Util.Camera().WorldToUGUIPoint(UIManager:GetCanvasParent(), pos)
    elseif animationFly == EAnimationFly.ViewToScene then
        startPos = Util.Camera().UGUIPointToWorld(pos)
    end

    if iconName and targetTf then
        local targetPos = targetTf.position
        if animationFly == EAnimationFly.ViewToScene or animationFly == EAnimationFly.SceneToScene then
            ---@type SceneFlyItem
            local flyItem = SceneItemPool.Get(SceneItemType.SceneFlyItem, targetTf.gameObject, iconName, size)
            flyItem.finishCall = finishCall
            flyItem:OnEnable(startPos, targetPos,EMoneyType.None)
        else
            local parentGo = UIManager:GetCanvasParent().gameObject
            ---@type ViewFlyItem
            local flyItem = UIItemPool.Get(UIItemType.ViewFlyItem, parentGo, iconName, size)
            flyItem.finishCall = finishCall
            flyItem:OnEnable(startPos, targetPos)
        end
    else
        logError("resource is not exit：", iconName, animationFly)
    end
end

---{len, addNum, modNum index}
---@return table {len, addNum, modNum index}
function AnimationManager:CaculateCount(count)
    local len, addNum, modNum = 0, 0, 0
    if count <= 1 then
        len, addNum, modNum = 1, 1, 0
    else
        addNum = math.floor(count/10)---每次多少数量
        len = math.floor(count/addNum) + (count%addNum>0 and 1 or 0)
        modNum = count%addNum
        if count < 10 then
            len, addNum, modNum = count, 1, 0
        end
    end
    return {len=len, addNum=addNum, modNum=modNum, index = 0}
end

---播放多个动画：飞往物品
---@param targetTf UnityEngine.Transform 目标位置
---@param startPos UnityEngine.Vector3 起始位置
---@param data Eitem_fly_data {次数， 物品数量}
---@param animationFly EAnimationFly 场景到UI的动画
---@param isCard boolean 是卡片
---@param viewId ViewID 是卡片
function AnimationManager:PlayMultipleItem(startPos, targetTf, data, animationFly, finishCall, isCard)
    isCard = isCard or false
    local count, iconName = data.count or 0,  data.iconName
    if count <= 1 then
        self:PlayItem( startPos, targetTf, iconName, animationFly, data.size, finishCall, isCard)
    else
        StartCoroutine(function()
            for _ = 1, count do
                self:PlayItem( startPos, targetTf, iconName, animationFly, data.size, finishCall, isCard)
                WaitForSeconds(0.2)
            end
        end)
    end
end

---@private
---@param childTf UnityEngine.Transform
function AnimationManager:_PlayItemFinish(isDisableCall, childTf, disableCall)
    local pos =  childTf.localPosition
    if isDisableCall then
        disableCall()
    else
        local endPos = Vector3.New(pos.x + Mathf.Random(-12,12.0),pos.y + Mathf.Random(-12,12.0),0)
        childTf:DOLocalMove(endPos, 1):OnComplete(function()
            table.insert(self.funcList, disableCall)
        end)
    end
end

---释放
function AnimationManager:OnDisable()
    for _, vFunc in ipairs(self.funcList) do
        vFunc()
    end
    table.clear(self.funcList)
end

return AnimationManager
