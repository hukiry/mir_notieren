--- 计算视图
--- MatchUtil       
--- Author : hukiry     
--- DateTime 2024/6/28 14:50   
---

---@class MatchUtil
local MatchUtil = Class()
function MatchUtil:ctor(matchView)
    ---@type MatchView
    self.matchView = matchView

    ---@type table<number, UnityEngine.Vector2Int>
    self.shapePinList = {
        {
            Vector2Int.New(1, 1),Vector2Int.New(0, 1),
            Vector2Int.New(1, 0)
        },
        {
            Vector2Int.New(-1, -1),Vector2Int.New(0, -1),
            Vector2Int.New(-1, 0)
        },
        {
            Vector2Int.New(1, -1),Vector2Int.New(0, -1),
            Vector2Int.New(1, 0)
        },
        {
            Vector2Int.New(-1, 1),Vector2Int.New(0, 1),
            Vector2Int.New(-1, 0)
        }
    }
end

---检查是颜色物品
---@param item MatchBase
function MatchUtil:IsColorItem(item, color)
    if item==nil or item.info == nil then
        return false
    end

    local info = item.info:GetCfgInfo()
    if info.color >  0 and info.itemType == EItemType.Normal  then
        return info.color == color
    end
    return false
end

---检查垂直方向
---@return table<number, UnityEngine.Vector2Int>
function MatchUtil:CheckVertical(cx, cy, color)
    local tabVertical = {}
    for i = cy + 1, cy + 4 do
        local item = self.matchView:GetMatchItem(cx, i)
        if self:IsColorItem(item, color) then
            table.insert(tabVertical, Vector2Int.New(cx, i))
        else
            break
        end
    end

    for i = cy - 1, cy - 4, -1 do
        local item = self.matchView:GetMatchItem(cx, i)
        if self:IsColorItem(item, color) then
            table.insert(tabVertical, Vector2Int.New(cx, i))
        else
            break
        end
    end
    return tabVertical
end

---检查水平方向
---@return table<number, UnityEngine.Vector2Int>
function MatchUtil:CheckHorizontal(cx, cy, color)
    local tabHorizontal = {}
    for i = cx + 1, cx + 4 do
        local item = self.matchView:GetMatchItem(i, cy)
        if self:IsColorItem(item, color) then
            table.insert(tabHorizontal, Vector2Int.New(i, cy))
        else
            break
        end
    end

    for i = cx - 1, cx - 4, -1 do
        local item = self.matchView:GetMatchItem(i, cy)
        if self:IsColorItem(item, color) then
            table.insert(tabHorizontal, Vector2Int.New(i, cy))
        else
            break
        end
    end
    return tabHorizontal
end

function MatchUtil:CheckdDragonfly(mx, my, color)
    for _, vTab in ipairs(self.shapePinList) do
        local index = 0
        for i, v in ipairs(vTab) do
            local x, y = mx+v.x, my+v.y
            local item = self.matchView:GetMatchItem(x, y)
            if self:IsColorItem(item, color) then
                index = index + 1
            end
        end

        if index == #vTab then
            local v = vTab[1]
            return  Vector2Int.New(mx+v.x, my+v.y)
        end
    end
    return nil
end

---@param color EColorItem
---@return EPropsType, table<number, UnityEngine.Vector2Int>
function MatchUtil:SearchSameColor(mx, my, color)
    --水平方向:火箭和彩虹球
    local tabHorizontal = self:CheckHorizontal(mx, my, color)
    if #tabHorizontal >= 4 then
        return EPropsType.RainbowBall, tabHorizontal
    end

    --垂直方向:火箭和彩虹球
    local tabVertical = self:CheckVertical(mx, my, color)
    if #tabVertical >= 4 then
        return EPropsType.RainbowBall,tabVertical
    end

    --炸弹方向
    if #tabVertical >= 2 and #tabHorizontal >= 2 then
        local temp = table.addArryTable(tabVertical, tabHorizontal)
        return EPropsType.Bomb, temp
    elseif #tabVertical >= 3 then--垂直火箭
        return EPropsType.Rocket,tabVertical
    elseif #tabHorizontal>=3 then--水平火箭
        return EPropsType.Rocket_Horizontal, tabHorizontal
    elseif #tabVertical > 0 and #tabHorizontal>0 then--竹蜻蜓
        local coordPos = self:CheckdDragonfly(mx, my, color)
        if coordPos then
            local temp = table.addArryTable(tabVertical, tabHorizontal)
            table.insert(temp, coordPos)
            return EPropsType.Dragonfly, temp
        end
    end

    if #tabVertical == 2 then
        return EPropsType.None, tabVertical
    else
        return EPropsType.None, tabHorizontal
    end
end

---转换为边界坐标
---@param tab table<UnityEngine.Vector3>
---@return table<number, UnityEngine.Vector3>
function MatchUtil:ConvertEdge(tab)
    local temp = {}
    local closeList ={} --关闭列表
    local addListFunc = function(x, y, list, state)
        if math.abs(x)>4 or math.abs(y)>4 then
            return
        end

        local key = x ..'_'..y
        if not closeList[key] then
            closeList[key] = true
            table.insert(list, Vector3.New(x, y, state))
        end
    end

    for i, v in ipairs(tab) do
        addListFunc(v.x, v.y, temp, 1)
    end

    for i, v in ipairs(tab) do
        addListFunc(v.x+1, v.y, temp, 0)
        addListFunc(v.x-1, v.y, temp, 0)
        addListFunc(v.x, v.y+1, temp, 0)
        addListFunc(v.x, v.y-1, temp, 0)
    end
    return temp
end


-------------------------道具部分-------------------------------
---彩虹球:一个触发的
---@param color EColorItem 被交换的物品
---@return table<number, UnityEngine.Vector2Int> 所有坐标点集合
---@param matchView MatchView
---@param props EPropsType
function MatchUtil:Props_RainbowBall(color,  matchView, props)
    if color == EColorItem.none then
        local v = Single.Match():GetColorId() % 100
        color = v>6 and v-6 or v
    end

    local temp, nextColor, propsTab = {}, 0, {}
    for x, vtab in pairs(matchView.itemList) do
        for y, item in pairs(vtab) do
            if item and item.info:IsNormal() then
                if item.info:GetCfgInfo().color == color then
                    table.insert(temp, Vector2Int.New(x, y))
                else
                    nextColor = item.info:GetCfgInfo().color
                end
            end

            if props and item then
                if item.info:GetCfgInfo().itemProps == props and props>0 then
                    table.insert(propsTab, Vector2Int.New(x, y))
                end
            end
        end
    end

    if #temp == 0 then
        temp = self:Props_RainbowBall(nextColor,  matchView)
    end

    for i, v in ipairs(propsTab) do
        table.insert(temp, v)
    end
    return temp
end

---竹蜻蜓：一起飞, 两个交换调用2次
---@param x number 所在x坐标
---@param y number 所在y坐标
---@return table<number, UnityEngine.Vector2Int>, UnityEngine.Vector2Int 所在坐标点，目标坐标点
---@param matchView MatchView
function MatchUtil:Props_Dragonfly(x, y, matchView)
    local posTab,coord = {}, nil
    local posFunc = function(_x, _y)
        if _x>MatchRule.Size or _x<-MatchRule.Size then  return end
        if _y>MatchRule.Size or _y<-MatchRule.Size then  return end
        table.insert(posTab, Vector2Int.New(_x, _y))
    end
    posFunc(x+1, y)
    posFunc(x-1, y)
    posFunc(x, y+1)
    posFunc(x, y-1)

    --收集障碍物的集合
    local tabBarrier = {}
    for i, vtab in pairs(matchView.itemList) do
        for j, item in pairs(vtab) do
            if item and item.info:GetCfgInfo():IsBarrier() then
                local barrierType = item.info:GetCfgInfo().barrierType
                if barrierType ~= EObstacleType.Animal and not Single.Match():IsFinishTarget(item.info.itemId)  then
                    table.insert(tabBarrier, Vector2Int.New(i, j))
                end
            end
        end
    end

    for i, vtab in pairs(matchView.itemFloatList) do
        for j, item in pairs(vtab) do
            if item then
                table.insert(tabBarrier, Vector2Int.New(i, j))
            end
        end
    end

    if #tabBarrier == 0 then
        for i, vtab in pairs(matchView.itemBottomList) do
            for j, item in pairs(vtab) do
                if item then
                    table.insert(tabBarrier, Vector2Int.New(i, j))
                end
            end
        end
    end

    --todo 后面修改为回形针检查
    if #tabBarrier > 0  then
        local pos = tabBarrier[math.random(1,#tabBarrier)]
        local item = matchView:GetMatchItem(pos.x, pos.y)
        if item then
            if item.info:IsBowl() or item.info:IsJar() then
                local defaultCoord = {Vector2.New(0,0),Vector2.New(1,0), Vector2.New(0,-1),Vector2.New(1,-1)}
                for i, v in ipairs(defaultCoord) do
                    if  item.info:IsHasWipe(v.x, v.y) then
                        coord = Vector2Int.New(item.info.x+v.x, item.info.y+v.y)
                        break
                    end
                end
            end

            if coord == nil then
                coord = Vector2Int.New(item.info.x, item.info.y)
            end
        end
    else
        coord = Vector2Int.New(math.random(-MatchRule.Size,MatchRule.Size), math.random(-MatchRule.Size,MatchRule.Size))
    end

    if coord == nil then
        coord = Vector2Int.New(x, y)
    end
    return posTab, coord
end

---炸弹：范围坐标集合
---@param isTwo boolean 是2个炸弹交换
---@return table<number, UnityEngine.Vector2Int> 炸弹范围集合
function MatchUtil:Props_Bomb(x, y, isTwo)
    local temp = {}
    local num = isTwo and 2 or 1
    num = num*2
    local cellMax, cellMin = self:_GetClamp(x+num) ,self:_GetClamp(x-num)
    local rowMax, rowMin = self:_GetClamp(y+num) ,self:_GetClamp(y-num)
    for _x = cellMin, cellMax do
        for _y = rowMin, rowMax do
            if x~=_x or y~=_y then
                table.insert(temp, Vector2Int.New(_x, _y))
            end
        end
    end
    return temp
end

---火箭：水平和垂直-收集消除的坐标集合, 不包含道具自己坐标
---@param isHorizontal boolean
---@return table<number, UnityEngine.Vector2Int>,table<number, UnityEngine.Vector2Int> 正集合，负集合
function MatchUtil:Collect_Rocket(x, y, isHorizontal)
    if self:IsOverMap(x, y) then
        return nil, nil
    end

    local temp1, temp2 = {}, {}
    local t = isHorizontal==true and x or y
    for k = t, MatchRule.Size do
        local ve = isHorizontal==true and Vector2Int.New(k, y) or Vector2Int.New(x, k)
        if k ~= t then
            table.insert(temp1, ve)
        end

    end

    for k = t, -MatchRule.Size, -1 do
        local ve = isHorizontal==true and Vector2Int.New(k, y) or Vector2Int.New(x, k)
        if  k ~= t then
            table.insert(temp2, ve)
        end
    end

    return temp1, temp2
end


---获取限制范围
---@param v number
function MatchUtil:_GetClamp(v)
    return Mathf.Clamp(v,-MatchRule.Size, MatchRule.Size)
end

---是否超出坐标
---@param x number
---@param y number
function MatchUtil:IsOverMap(x, y)
    if self:_IsOverRange(x) or self:_IsOverRange(y) then
        return true
    end
    return false
end

---获取限制范围
---@param v number
function MatchUtil:_IsOverRange(v)
    if v > MatchRule.Size or v<-MatchRule.Size then
        return true
    end
    return false
end

return MatchUtil