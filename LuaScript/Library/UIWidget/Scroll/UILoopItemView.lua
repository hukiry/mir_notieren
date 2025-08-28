---
--- 无限滚动item列表
--- Created huiry
--- DateTime: 2021/1/30 下午 2:27

---滚动视图必须添加 GridLayoutGroup，每个界面只能实例化一次，不可多次
---@class UILoopItemView:UIItemViewBase
UILoopItemView = Class(UIItemViewBase)

---@param go UnityEngine.GameObject
---@param pItemType UIItemType
function UILoopItemView:ctor(go, pItemType)
    local sizeFitter = self.transform:GetComponent("ContentSizeFitter")
    if sizeFitter then sizeFitter.enabled = false end

    ---@type UnityEngine.UI.GridLayoutGroup
    local layoutGroup = self.transform:GetComponent("GridLayoutGroup")
    if layoutGroup then layoutGroup.enabled = false end
end

---设置渲染列表的尺寸
---private
function UILoopItemView:_SetListRenderSize()
    if self.mIsVertical then
        local y = math.max(self.mMaskSize.y, math.ceil((self.mInfoCount / self.mColumnCount)) * self.mBlockSize.y + self.mPaddingRect.top + self.mPaddingRect.bottom);
        self.mRectTransform.sizeDelta = Vector2.New(0, y);
    else
        local x = math.max(self.mMaskSize.x, math.ceil((self.mInfoCount / self.mColumnCount)) * self.mBlockSize.x + self.mPaddingRect.left + self.mPaddingRect.right);
        self.mRectTransform.sizeDelta = Vector2.New(x, 0);
    end
end

---private
---是否强制刷新所有数据
---@param recalculate boolean 重新计算一次
---@param updateItem boolean 是否强制刷新所有Item
function UILoopItemView:_UpdateRender(recalculate, updateItem)
    if self.mInfoList == nil then return end

    local gridPositionXY = math.ceil(self.mIsVertical and self.mRectTransform.anchoredPosition.y or -self.mRectTransform.anchoredPosition.x);
    local viewCount = math.min(self.mRendererCount, self.mInfoCount);
    local minIndex = math.max(1, math.floor(gridPositionXY / self.mBlockXY) * self.mColumnCount);
    minIndex = math.min(minIndex, (self.mInfoCount - viewCount) + 1);--当拖动到最底时，还在继续往下拉(保持当前视图最小显示量)
    local maxIndex = Mathf.Clamp(math.floor((gridPositionXY + self.mMaskSizeXY) / self.mBlockXY + 1) * self.mColumnCount, 1, self.mInfoCount);
    maxIndex = math.max(maxIndex, viewCount);--当拖动到最顶时，还在继续往上拉(保持当前视图最小显示量)

    if self.lastMinIndex == minIndex and self.lastMaxIndex == maxIndex and recalculate ~= true and updateItem ~= true then return; end
    self.lastMinIndex = minIndex; self.lastMaxIndex = maxIndex

    --取出显示Item中最大索引
    local len = 1
    for k, v in pairs(self.mItemDic) do
        if k > len then
            len = k
        end
    end

    --先移除不在范围内的索引对象
    for i = 1, len do
        if (i < minIndex or i > maxIndex) and self.mItemDic[i] then
            UIItemPool.Put(self.itemType, self.mItemDic[i])
            self.mItemDic[i] = nil
        end
    end

    --补齐数据，并刷新		，，已经存在的不用再处理
    for i = minIndex, maxIndex do
        if self.mItemDic[i] == nil then
            ---对象池中取数据
            self.mItemDic[i] = self:_SetItemAndPosition(UIItemPool.Get(self.itemType, self.gameObject, self.initArgs), i)
        elseif updateItem == true then
            self:_SetItemAndPosition(self.mItemDic[i], i)
        end
    end
end

---private
---更新各个渲染格子的位置
---@param item IUIItem
function UILoopItemView:_SetItemAndPosition(item, index)
    ---根据索引的值，计算出位置
    local pos = self:_CalculateItemPosition(index)
    item.transform.anchoredPosition3D = Vector3.zero
    item.transform.anchoredPosition = pos

    try{
        function()
            item.numberId = index
            item:OnEnable(self.mInfoList[index])
            if self.RandererItemFunc then
                self.RandererItemFunc(index , item)
            end
        end,
        catch = function(error)
            log(error, "red")
        end
    }
    return item
end
