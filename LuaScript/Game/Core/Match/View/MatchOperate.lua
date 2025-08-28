---
--- MatchOperate       
--- Author : hukiry     
--- DateTime 2023/5/9 21:35   
---

---操作视图 MatchOperate
---@class MatchOperate:DisplayObjectBase
local MatchOperate = Class(DisplayObjectBase)
function MatchOperate:ctor(gameObject, matchView)
    ---@type MatchView
    self.matchView = matchView
    ---道具释放控制
    ---@type MatchPropsControl
    self.propsCtrol = require('Game.Core.Match.View.MatchPropsControl').New(self)
    self.moveCoordTab = {}
    self.isCanMove = true
    ---@type MatchUtil
    self.matchUtil = require("Game.Core.Match.View.MatchUtil").New(matchView)
end

---初始化子视图，执行一次
function MatchOperate:OnStart()
    EventDispatch:Register(self, UIEvent.Match_Operate_Move, self.OnMouseMove)
    EventDispatch:Register(self, UIEvent.Match_Operate_Click, self.OnMouseClick)
    EventDispatch:Register(self, UIEvent.Match_Create_GM, self.CreatePropOrItem)
    self.propsCtrol:OnStart()
end

function MatchOperate:OnMouseMove(bx, by, mx, my)
    ---处理是否结束
    if self.isCanMove == false then
        return
    end

    if self.matchView.isLoadAnimation then
        return
    end

    --在使用道具 和 总移动次数为0
    if MatchRule.isUseProp or Single.Match().totalMove <= 0 then
        log("你fail",Single.Match().totalMove, "red")
        return
    end
    EventDispatch:Broadcast(UIEvent.Match_AI_Tip_View)
    --todo 记录全局x坐标集合，完成动画，移除此坐标
    if bx==mx and by==my then return end
    if bx~=mx and by~=my then return end
    local item1 =  self.matchView:GetMatchItem(bx, by)
    local item2 =  self.matchView:GetMatchItem(mx, my)
    if item1 == nil or item2 == nil then return end
    if item1.info == nil or item2.info == nil then return end
    local moveInfo, changeInfo = item1.info, item2.info
    ---可以移动的物品
    if moveInfo:IsCanMove() and changeInfo:IsCanMove() then
        local col1, col2 = changeInfo:GetCfgInfo().color, moveInfo:GetCfgInfo().color
        local props1, props2 = changeInfo:GetCfgInfo().itemProps, moveInfo:GetCfgInfo().itemProps
        ---相同颜色不可交换
        if col1==col2 and col1 > 0 then
            return
        end

        if props2 ~= EPropsType.None and changeInfo:GetCfgInfo():IsBarrier() then
            return
        end

        if props1 ~= EPropsType.None and moveInfo:GetCfgInfo():IsBarrier() then
            return
        end

        if changeInfo:GetCfgInfo():IsBarrier() and moveInfo:GetCfgInfo():IsBarrier() then
            return
        end

        --开始交换
        self.matchView:OnRestorePosition(bx, by, mx, my)
        --local col1, col2 = changeInfo:GetCfgInfo().color, moveInfo:GetCfgInfo().color
        --local props1, props2 = changeInfo:GetCfgInfo().itemProps, moveInfo:GetCfgInfo().itemProps
        if moveInfo:IsProps() and changeInfo:IsProps() then--2个都是道具
            self.isCanMove = false
            Single.Match():UpdateMoveCount()
            local isHorizontal = moveInfo.isHorizontal or changeInfo.isHorizontal
            self.propsCtrol:ChangeProps(Vector2Int.New(bx, by), Vector2Int.New(mx, my), props1, props2, isHorizontal)
            return
        elseif moveInfo:IsProps() or changeInfo:IsProps() then--其中一个是道具
            self.isCanMove = false
            Single.Match():UpdateMoveCount()
            local props, tab = 0 ,{}
            local rx,ry,px,py = 0,0,0,0
            local pros, col, isHorizontal = EPropsType.none,EColorItem.none, false
            if changeInfo:IsProps() then
                props, tab =  self.matchUtil:SearchSameColor(mx, my, col2)
                rx, ry, px, py, col = mx, my, bx, by, col2
                pros, isHorizontal = props1, changeInfo.isHorizontal
            else
                props, tab =  self.matchUtil:SearchSameColor(bx, by, col1)
                rx, ry, px, py, col= bx, by, mx, my, col1
                pros, isHorizontal  = props2, moveInfo.isHorizontal
            end

            if props ~= EPropsType.None then--生成道具
                self.matchView:UpdateView(bx, by, tab, props, col1)
            elseif #tab>=2 then
                table.insert(tab, Vector2Int.New(rx,ry))
                for _, v in ipairs(tab) do
                    self.matchView:RemoveMatchItem(v.x, v.y)
                end
            end
            self.propsCtrol:ClickProps(px, py, pros, isHorizontal, col, tab)
            return
        else--无道具
            local props1, tab1 =  self.matchUtil:SearchSameColor(bx, by, col1)
            local props2, tab2 =  self.matchUtil:SearchSameColor(mx, my, col2)
            if #tab1>=2 or #tab2>=2  then
                self.isCanMove = false
                Single.Match():UpdateMoveCount()
                ---生成物品
                if props1 ~= EPropsType.None or props2 ~= EPropsType.None then
                    if props1 ~= EPropsType.None then--左边生成道具
                        self.matchView:UpdateView(bx, by, tab1, props1, col1)
                    end

                    if props2 ~= EPropsType.None then--右边生成道具
                        self.matchView:UpdateView(mx, my, tab2, props2, col2)
                    end
                else
                    if #tab1>=2 then--左边消除
                        table.insert(tab1, Vector2Int.New(bx, by))
                        for i, v in ipairs(tab1) do
                            self.matchView:RemoveMatchItem(v.x, v.y)
                        end

                        local temp = self.matchUtil:ConvertEdge(tab1)
                        for _, v in ipairs(temp) do
                            self.matchView:RefreshNear(v.x, v.y, col1, v.z == 1)
                        end
                    end

                    if #tab2>=2 then--右边消除
                        table.insert(tab2, Vector2Int.New(mx, my))
                        for i, v in ipairs(tab2) do
                            self.matchView:RemoveMatchItem(v.x, v.y)
                        end

                        local temp = self.matchUtil:ConvertEdge(tab2)
                        for _, v in ipairs(temp) do
                            self.matchView:RefreshNear(v.x, v.y, col2, v.z == 1)
                        end
                    end
                end

                StartCoroutine(function()
                    WaitForSeconds(0.25)
                    local resultTAB = tab1
                    if #tab1>2 and #tab2>2 then
                        resultTAB = table.addArryTable(tab1, tab2)
                    elseif #tab2>2 then
                        resultTAB = tab2
                    end

                    local xtab = {}
                    for i, v in ipairs(resultTAB) do
                        xtab[v.x] = 1
                        --边缘消除下落
                        if v.x+1<=MatchRule.Size then xtab[v.x+1] = 1 end
                        if v.x-1>=-MatchRule.Size then xtab[v.x-1] = 1 end
                        if v.x+2<=MatchRule.Size then xtab[v.x+2] = 1 end
                        if v.x-2>=-MatchRule.Size then xtab[v.x-2] = 1 end
                    end
                    self.matchView:DownRepairView(table.toArrayKey(xtab))--手动下落
                end)
            else
                StartCoroutine(function()
                    WaitForSeconds(0.25)
                    self.matchView:OnRestorePosition(bx, by, mx, my)
                end)
            end
        end
    end
end

---循环检查
function MatchOperate:CheckLoop()
    local temp = {}
    for i = -MatchRule.Size, MatchRule.Size do
        temp[i] = {}
        for j = -MatchRule.Size, MatchRule.Size do
            temp[i][j] = true
        end
    end

    local productTab = {}
    for x = -MatchRule.Size, MatchRule.Size do
        for y = -MatchRule.Size, MatchRule.Size do
            if temp[x][y] then
                local item =  self.matchView:GetMatchItem(x, y)
                if item == nil then
                    temp[x][y] = false
                end

                if temp[x][y] then
                    if not item.info:IsNormal() or item.info:GetCfgInfo().color == 0 then
                        temp[x][y] = false
                    end
                end

                if temp[x][y] then
                    local col = item.info:GetCfgInfo().color
                    local props, tab =  self.matchUtil:SearchSameColor(x, y, col)
                    if #tab>=2 then
                        for _, v in ipairs(tab) do
                            temp[v.x][v.y] = false
                        end
                        temp[x][y] = false
                        table.insert(productTab,{x=x, y=y, props=props, tab=tab, color = col})
                    end
                end
            end
        end
    end

    temp = {}
    for _, v in ipairs(productTab) do
        if v.props~= EPropsType.None then
            self.matchView:BornProp(v.x,v.y,v.tab,v.props)
        else
            table.insert(v.tab, Vector2Int.New(v.x,v.y))
            for _, vt in ipairs(v.tab) do
                self.matchView:RemoveMatchItem(vt.x, vt.y)
            end
        end

        ---消除边界障碍物
        local tempEdge = self.matchUtil:ConvertEdge(v.tab)
        for _, vl in ipairs(tempEdge) do
            self.matchView:RefreshNear(vl.x, vl.y, v.color,vl.z == 1)
        end
    end

    if #productTab > 0 then
        --自动下落
        StartCoroutine(function()
            WaitForSeconds(0.25)
            local xtab = {}
            for _, obj in ipairs(productTab) do
                for _, v in ipairs(obj.tab) do
                    xtab[v.x] = 1
                    --边缘消除下落
                    if v.x+1<=MatchRule.Size then xtab[v.x+1] = 1 end
                    if v.x-1>=-MatchRule.Size then xtab[v.x-1] = 1 end
                    if v.x+2<=MatchRule.Size then xtab[v.x+2] = 1 end
                    if v.x-2>=-MatchRule.Size then xtab[v.x-2] = 1 end
                end
            end
            self.matchView:DownRepairView(table.toArrayKey(xtab))--自动下落
        end)
    else
        --todo 结束，用户可以点击
        Single.Match():UpdateMoveCount(true)
        self.isCanMove = true
    end
end


---点击了道具，或振动
---@param bx number 点击的x坐标
---@param by number 点击的y坐标
function MatchOperate:OnMouseClick(bx, by)
    ---处理未结束 或 移动次数为0
    if self.isCanMove == false or Single.Match().totalMove <= 0 then
        return
    end

    --使用道具时，点击没有超出地图范围
    if MatchRule.isUseProp and not self.matchUtil:IsOverMap(bx, by) then
        MatchRule.isUseProp = false
        EventDispatch:Broadcast(ViewID.LevelMain, 3, 0 , bx, by)
        return
    end

    --道具被点击
    local clickItem = self.matchView:GetMatchItem(bx, by)
    if clickItem and clickItem.info:IsProps() then
        EventDispatch:Broadcast(UIEvent.Match_AI_Tip_View)
        Single.Match():UpdateMoveCount()
        ----使用道具
        self.isCanMove = false
        local props = clickItem.info:GetCfgInfo().itemProps
        self.propsCtrol:ClickProps(bx, by, props, clickItem.info.isHorizontal)
    else
        --播放物品振动
        EventDispatch:Broadcast(UIEvent.Match_Shake_View, bx, by)
        if UNITY_EDITOR then
            EventDispatch:Broadcast(ViewID.Gm, bx, by)
        end
    end
end

---获取主视图
---@return MatchView
function MatchOperate:GetMtchView()
    return self.matchView
end

---更加Gm命令创建物品
---@param props EPropsType
---@param x number
---@param y number
---@param isHorizontal boolean
function MatchOperate:CreatePropOrItem(props, x, y, isHorizontal)
    if props ~= EPropsType.None then
        self.matchView:RemoveMatchItem(x, y)
        local info = Single.Match():CreateProps(props,x, y)
        local itemV = self.matchView:CreateMergeItem(info)
        if itemV then
            if props <= EPropsType.Rocket_Horizontal then
                Single.Sound():PlaySound(ESoundResType.MergeNewItem)
                itemV:GetAnimation():PlayEffect(info:GetWorldPos(), ESceneEffectRes.Born_props)
            end
        end
    else
        self.matchView:RemoveMatchItem(x, y)
        local  info = Single.Match():CreateOnly(x, y)
        local itemV = self.matchView:CreateMergeItem(info)
        self.matchView:ChangeMapItem(nil,nil,x,y,itemV)
    end
end

---每次隐藏视图：撤销事件，回收对象池，撤销计时器
function MatchOperate:OnDisable()
    EventDispatch:UnRegister(self)
    self.propsCtrol:OnDisable()
end

---消耗资源
function MatchOperate:OnDestroy()
end

return MatchOperate