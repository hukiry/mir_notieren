--- 操作编辑视图
--- MetaOperate       
--- Author : hukiry     
--- DateTime 2023/9/13 20:46   
---

---操作视图 MatchOperate
---@class MetaOperate:DisplayObjectBase
local MetaOperate = Class(DisplayObjectBase)
function MetaOperate:ctor(gameObject, metaEditorView)
    ---@type MetaView
    self.metaView = metaEditorView
    ---改变选择框
    ---@type RendererGrid
    self.rendererGrid = self.gameObject:GetComponent(typeof(RendererGrid))
end

---初始化子视图， 注册事件，计时器
function MetaOperate:OnStart()
    EventDispatch:Register(self, UIEvent.Meta_Operate_Move, self.OnMouseMove)
    EventDispatch:Register(self, UIEvent.Meta_Operate_Click, self.OnMouseClick)
    EventDispatch:Register(self, UIEvent.Meta_Operate_ResetDo, self.ResetDoItem)
    EventDispatch:Register(self, UIEvent.Meta_Operate_LayerView, self.ChangeLayerView)
end

function MetaOperate:OnMouseClick(x, y)
    self:ChangeView(x, y)
end

function MetaOperate:OnMouseMove(x, y)
    self:ChangeView(x, y)
end

---修改坐标视图
function MetaOperate:ChangeView(x, y)
    ---绘制格子
    self.rendererGrid:SetPositionTile(x, y)

    if Single.Meta().selectItemId > 0  then
        local selectMode = Single.Meta().selectMode
        --涂上物品
        if selectMode == ESelectState.AddItem then
            local infoDel = Single.Meta():GetInfo(x, y)
            if infoDel and Single.Meta().selectItemId == infoDel.itemId then
                if not (infoDel.itemId==203 and infoDel.isHorizontal == Single.Meta().isVertical) then
                    EventDispatch:Broadcast(ViewID.MetaEditor, 4)
                    return
                end
            end

            self:DeleteItem(x, y)
            ---@type TileInfo
            local info = Single.Meta():CreateInfo(Single.Meta().selectItemId, x, y)
            info.isHorizontal = not Single.Meta().isVertical
            self.metaView:CreateItem(info, true)
            EventDispatch:Broadcast(ViewID.MetaEditor, infoDel~=nil and 3 or 1, info)

            if infoDel then --有数据
                Single.Meta():SetResetdoInfo(infoDel.itemId, x, y, infoDel.isHorizontal, true)
            else
                Single.Meta():SetResetdoInfo(0, x, y, false, false)
            end
        elseif selectMode == ESelectState.Delete then
            --擦除物品
            self:DeleteItem(x, y, true)
        end
    else
        TipMessageBox.ShowUI(GetLanguageText(16006))
    end
end

---移除坐标视图
function MetaOperate:DeleteItem(x, y, isHandle)
    local info = Single.Meta():RemoveInfo(x, y, Single.Meta().selectLayer)

    if isHandle then
        if info then
            Single.Meta():SetResetdoInfo(info.itemId, x, y, info.isHorizontal, true)
        end
        EventDispatch:Broadcast(ViewID.MetaEditor, 2, info)
    end

    if info then
        self.metaView:RemoveItem(x, y, Single.Meta().selectLayer, true)
    end
end

---撤销上一步骤
function MetaOperate:ResetDoItem()
    local doInfo = Single.Meta():GetResetdoInfo()
    Single.Meta():RemoveInfo(doInfo.x, doInfo.y, doInfo.selectLayer)
    self.metaView:RemoveItem(doInfo.x, doInfo.y, doInfo.selectLayer)
    if doInfo.isHave then
        ---@type TileInfo
        local info = Single.Meta():CreateInfo(doInfo.itemId, doInfo.x, doInfo.y)
        info.isHorizontal = doInfo.isHorizontal
        self.metaView:CreateItem(info)
    end
    EventDispatch:Broadcast(ViewID.MetaEditor, 5)
end

---场景层透明
---@param layerView EMapLayerView
function MetaOperate:ChangeLayerView(layerView)
    for i = 0, EMapLayerView.Float do
        local col = i == layerView and Color.New(1,1,1,1) or Color.New(1,1,1,0.3)
        local arrayMap = self.metaView:GetViewTable(i)
        for _, vTab in pairs(arrayMap) do
            for _, v in pairs(vTab) do
                v:SetHalfAlpha(col)
            end
        end
    end
end

---每次隐藏视图：撤销事件，回收对象池，撤销计时器
function MetaOperate:OnDisable()
    EventDispatch:UnRegister(self)
end

return MetaOperate