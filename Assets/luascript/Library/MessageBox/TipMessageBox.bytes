---
--- TipMessageBox       
--- Author : hukiry     
--- DateTime 2023/10/16 13:20   
---

---@class TipMessageBox
TipMessageBox = {}

---显示窗口:初次打开
---@param tip string
---@param isFixed boolean 是取消动画:固定不移动
function TipMessageBox.ShowUI(tip, isFixed)
    isFixed = isFixed or false
    TipMessageBox._Show(tip, isFixed, true)
end

---显示窗口:初次打开
function TipMessageBox.ShowScene(tip, isFixed)
    isFixed = isFixed or false
    --TipMessageBox._Show(tip, isFixed)
end


---通用弹框
---@param tip string
---@param isFixed boolean 是取消动画:固定不移动
function TipMessageBox.ShowBox(tip, isFixed)

end

---@private
function TipMessageBox._Show(tip, isFixed, isUI)
    local gameObject = UIManager:GetCanvasParent().gameObject
    ---@type TipItem
    local tipItem = nil
    if isUI then
        ---@type TipItem
        tipItem = UIItemPool.Get(UIItemType.TipItem, gameObject, tip)
        tipItem.transform:SetParent(UIManager:GetLayerParent(ViewLayer.Tips), false)
    else
        tipItem = SceneItemPool.Get(SceneItemType.SceneFlyItem, gameObject, tip)
    end

    tipItem:OnEnable(isFixed)
end
