---
--- 滚动item列表
--- Created huiry
--- DateTime: 2021/1/30 下午 2:27

---滚动视图必须添加 GridLayoutGroup
---@class UIItemView:UIItemViewBase
UIItemView = Class(UIItemViewBase)

---@param parentGo UnityEngine.GameObject
---@param pItemType UIItemType
function UIItemView:ctor(parentGo, pItemType)
end

---private
---是否强制刷新所有数据
---@param recalculate boolean 重新计算一次
---@param updateItem boolean 是否强制刷新所有Item
function UIItemView:_UpdateRender(recalculate, updateItem)
    if self.mInfoList == nil then return end

    local minIndex = 1
    local maxIndex = self.mInfoCount
    local itemLen = #self.mItemDic

    --先移除不在范围内的
    for i = 1, itemLen do
        if (i < minIndex or i > maxIndex) and self.mItemDic[i] then
            UIItemPool.Put(self.itemType, self.mItemDic[i])
            self.mItemDic[i] = nil
        end
    end

    --补齐数据，并刷新		，，已经存在的不用再处理
    for i = minIndex, maxIndex do
        if self.mItemDic[i] == nil then
            self.mItemDic[i] = self:_SetItemAndPosition(UIItemPool.Get(self.itemType, self.gameObject, self.initArgs), i)
        elseif updateItem == true then
            self:_SetItemAndPosition(self.mItemDic[i], i)
        end
    end
end

---private
---更新各个渲染格子的位置
---@param item IUIItem
function UIItemView:_SetItemAndPosition(item, index)
    try{
        function()
            item.numberId = index
            item:OnEnable(self.mInfoList[index])
        end,
        catch = function(error)
            log(error, "red")
        end
    }
    return item
end