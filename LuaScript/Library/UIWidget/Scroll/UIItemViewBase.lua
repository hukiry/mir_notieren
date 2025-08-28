---
--- 无限滚动 与 普通滚动 item列表 基类
--- Created huiry
--- DateTime: 2021/1/30 下午 2:27

---@class UIItemViewBase:DisplayObjectBase
UIItemViewBase = Class(DisplayObjectBase)

---@param go UnityEngine.GameObject
---@param pItemType UIItemType
function UIItemViewBase:ctor(go, pItemType)
    ---private
    ---@type table<number, IUIItem> <index, Item>
    self.mItemDic = {}

    ---@type UIItemType 对象池类型
    self.itemType = pItemType

    ---private
    self.mRectTransform = self.rectTransform
    ---获得蒙版尺寸
    ---@type UnityEngine.Rect
    local maskRect = self.transform.parent:GetComponent("RectTransform").rect
    ---private
    self.mMaskSize = Vector2.New(maskRect.width, maskRect.height)

    ---注册拖动视图时，刷新
    ---@type UnityEngine.UI.ScrollRect
    self.mScrollRect = self.transform.parent.parent:GetComponent("ScrollRect")
    if self.mScrollRect then
        self.mScrollRect.onValueChanged:AddListener(function() self:_UpdateRender(false, false) end);
    end

    ---禁用 GridLayoutGroup 与 ContentSizeFitter
    ---@type UnityEngine.UI.GridLayoutGroup
    local layoutGroup = self.transform:GetComponent("GridLayoutGroup")
    if layoutGroup == nil then
        log("使用UIItemViewBase必须包含<GridLayoutGroup>组件","red")
    end

    ---private
    self.mIsVertical = self.mScrollRect.vertical
    ---private
    ---@type UnityEngine.Vector2 格子大小
    self.mCellSize = layoutGroup.cellSize;
    ---private
    ---@type UnityEngine.Vector2 Grid填充大小
    self.mPaddingRect = layoutGroup.padding
    ---private
    ---@type UnityEngine.Vector2 实际每个格子大小(加上间距)
    self.mBlockSize = self.mCellSize + layoutGroup.spacing
    ---private
    ---@type number 根据水平或垂直 取出格子高度
    self.mBlockXY = self.mIsVertical and self.mBlockSize.y or self.mBlockSize.x
    ---private
    ---@type number 根据水平或垂直 取出渲染高度
    self.mMaskSizeXY = self.mIsVertical and self.mMaskSize.y or self.mMaskSize.x

    --蒙版尺寸下可以显示多少列
    ---@type UnityEngine.Rect
    local gridRect = self.mRectTransform.rect
    if self.mIsVertical then
        self.mColumnCount = Mathf.Round((gridRect.width - (layoutGroup.padding.left + layoutGroup.padding.right + layoutGroup.spacing.x)) / self.mBlockSize.x)
    else
        self.mColumnCount = Mathf.Round((gridRect.height - (layoutGroup.padding.top + layoutGroup.padding.bottom + layoutGroup.spacing.y)) / self.mBlockSize.y)
    end

    --通过蒙版尺寸和格子尺寸计算需要的渲染器个数
    if self.mIsVertical then
        self.mRendererCount = self.mColumnCount * (Mathf.Round(self.mMaskSize.y / self.mBlockSize.y) + 1)
    else
        self.mRendererCount = self.mColumnCount * (Mathf.Round(self.mMaskSize.x / self.mBlockSize.x) + 1)
    end

    self.RandererItemFunc = nil ---设置格子表现状态
end

---设置渲染列表的尺寸
---private
function UIItemViewBase:_SetListRenderSize()
end

---更新列表
---@param infoList table<number, {}> 数据集合 OnEable方法
---@param resetTop boolean 是否重置到顶部 默认:true
---@param tween boolean 是否需要渐入动画 默认:true
---@param args table 传给Start方法接收的(只有第一次初始化才有用)
function UIItemViewBase:UpdateList(infoList, resetTop, tween, args)
    self.mInfoList = table.clone(infoList or {})--这里把数据克隆出来用，防止外部引用修改
    self.mInfoCount = #self.mInfoList
    self.initArgs = args
    self:_SetListRenderSize();
    if resetTop then
        self:GotoLocationByIndex(1);
    end

    self:_UpdateRender(true, true);
    if tween then
        Single.TimerManger():RemoveHandler(self, self.PlayGraduallyTween)
        Single.TimerManger():DoFrame(self, self.PlayGraduallyTween, 2, 1)
    end
end

---根据Info更新Item
function UIItemViewBase:UpdateItemByInfo(info)
    local index = table.indexOf(self.mInfoList, info)
    if index ~= -1 then
        self.mInfoList[index] = info
        self:UpdateItemByIndex(index)
    end
end

---根据索引更新Item
function UIItemViewBase:UpdateItemByIndex(index)
    if self.mItemDic[index] then
        try{
            function()
                self.mItemDic[index]:OnEnable(self.mInfoList[index])
            end,
            catch = function(error)
                log(error, "red")
            end
        }
    end
end

---按数据移除元素
---@param removeTween boolean 是否有移除动画
function UIItemViewBase:RemoveAtInfo(info, removeTween)
    self:RemoveAtIndex(table.indexOf(self.mInfoList, info))
end

---按索引移除元素
---@param removeTween boolean 是否有移除动画
function UIItemViewBase:RemoveAtIndex(index, removeTween)
    if self.mItemDic[index] then
        local item = self.mItemDic[index]
        if removeTween then
            local call = function()
                self:_RemoveItemTweenFinish(item, index);
                Single.TimerManger():DoFrame(self, function() self:PlayGraduallyTween(index); end,1 ,1)--延时一帧，但在C#中没问题，后面可检查
            end
            if self.mIsVertical then
                item.transform:DOLocalMoveX(self.mMaskSize.x, 0.3):SetEase(Ease.InBack).onComplete = call;
            else
                item.transform:DOLocalMoveY(self.mMaskSize.y, 0.3):SetEase(Ease.InBack).onComplete = call;
            end
        else
            self:_RemoveItemTweenFinish(item, index)
        end
    elseif self.mInfoCount > index then
        table.remove(self.mInfoList, index)
        self:_SetListRenderSize()
        self:_UpdateRender(true, true)
    end
end

---根据数据跳转到相应位置
function UIItemViewBase:GotoLocationByInfo(info)
    self:GotoLocationByIndex(table.indexOf(self.mInfoList, info))
end

---根据索引跳转到相应位置
function UIItemViewBase:GotoLocationByIndex(index)
    if index < 1 or index > self.mInfoCount then
        return
    end
    for k, v in pairs(self.mItemDic) do
        v.transform:DOKill(true)
    end
    if self.mScrollRect then self.mScrollRect:StopMovement(); end    --停止正在滑动的ScrollRect

    local pos = self.mRectTransform.anchoredPosition
    local v2Pos = self:_CalculateItemPosition(index)
    if self.mIsVertical then
        v2Pos = Vector2.New(pos.x, math.min(-(v2Pos.y + self.mPaddingRect.top), self.mRectTransform.rect.height - self.mMaskSize.y));
    else
        v2Pos = Vector2.New(-math.min((v2Pos.x - self.mPaddingRect.left), self.mRectTransform.rect.width - self.mMaskSize.x), pos.y);
    end
    self.mRectTransform.anchoredPosition = v2Pos
    self:_UpdateRender(false, false)
end

---播放渐入动画
---@param index number 从第几个索引开始播
function UIItemViewBase:PlayGraduallyTween(index)
    if index == nil then index = 1 end

    for k, v in pairs(self.mItemDic) do
        v.transform:DOKill(true)
    end

    for k, v in pairs(self.mItemDic) do
        --if v.index >= index and v.index <= self.mRendererCount then
            local rt = v.transform
            local targetPos = rt.anchoredPosition

            if self.mIsVertical then
                rt.anchoredPosition = Vector2.New(targetPos.x, targetPos.y - (self.mCellSize.y + math.floor((v.numberId - 1) / self.mColumnCount) * 500));
                rt.transform:DOLocalMoveY(targetPos.y, 0.5 + math.floor((v.numberId - 1) / self.mColumnCount) * 0.05):SetEase(Ease.OutBack, 0.6);
            else
                rt.anchoredPosition = Vector2.New(targetPos.x + (self.mCellSize.x + math.floor((v.numberId - 1) / self.mColumnCount) * 500), targetPos.y);
                rt.transform:DOLocalMoveX(targetPos.x, 0.5 + math.floor((v.numberId - 1) / self.mColumnCount) * 0.05):SetEase(Ease.OutBack, 0.6);
            end
        --end
    end
end

---在移除动画完成后操作
---private
---@param item IUIItem
---@param index number 刚才移除的是哪个索引
function UIItemViewBase:_RemoveItemTweenFinish(item, index)
    self.mItemDic[index] = nil
    table.remove(self.mInfoList, index)
    self.mInfoCount = #self.mInfoList
    UIItemPool.Put(self.itemType, item)

    --后面的需要重置索引
    for k, v in pairs(self.mItemDic) do
        if v.numberId > index then
            v.numberId = v.numberId - 1
            self:_SetItemAndPosition(v, v.numberId)
        end
    end

    local items = table.toArray(self.mItemDic)
    table.clear(self.mItemDic)
    for i = 1, #items do
        self.mItemDic[items[i].index] = items[i]
    end

    self:_SetListRenderSize();
    self:_UpdateRender(true, false);
end

---private
---是否强制刷新所有数据
---@param recalculate boolean 重新计算一次
---@param updateItem boolean 是否强制刷新所有Item
function UIItemViewBase:_UpdateRender(recalculate, updateItem)
end

---private
---更新各个渲染格子的位置
---@param item IUIItem
function UIItemViewBase:_SetItemAndPosition(item, index)
end

---private
---计算坐标
function UIItemViewBase:_CalculateItemPosition(index)
    local row = math.floor((index - 1) / self.mColumnCount);
    local column = (index - 1) % self.mColumnCount;
    if self.mIsVertical then
        return Vector2.New(self.mPaddingRect.left + column * self.mBlockSize.x, -(self.mPaddingRect.top + row * self.mBlockSize.y));
    else
        return Vector2.New(self.mPaddingRect.left + row * self.mBlockSize.x, -(self.mPaddingRect.top + column * self.mBlockSize.y));
    end
end

---获取数据列表
---@return table<number, IUIItem>
function UIItemViewBase:GetInfoList()
    return self.mInfoList
end

---获取当前显示的Item列表
---@return table<number, IUIItem>
function UIItemViewBase:GetItemList()
    return self.mItemDic
end

---获取对应index显示的Item
---@return IUIItem
function UIItemViewBase:GetItemByIndex(index)
    return self.mItemDic[index]
end

---刷新视图
function UIItemViewBase:RefreshRender(...)
    local itemList = self:GetItemList()
    for _, v in pairs(itemList) do
        if v.OnRefresh then
            v:OnRefresh(...)
        end
    end
end

function UIItemViewBase:OnDisable()
    UIItemPool.PutTable(self.itemType, self.mItemDic)
end

---@param isDestroyType boolean 是否把该类型的对象从对象池中销毁 默认true
function UIItemViewBase:OnDestroy(isDestroyType)
    if isDestroyType ~= false then
        UIItemPool.Destory(self.itemType)
    end
end