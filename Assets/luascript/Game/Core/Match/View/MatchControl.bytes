--- 控制视图下落，补充
--- MatchControl       
--- Author : hukiry     
--- DateTime 2024/6/29 15:25   
---

---@class MatchControl
local MatchControl = Class()
function MatchControl:ctor(matchView, matchOperate)
    ---操作视图
    ---@type MatchView
    self.matchView = matchView
    ---操作视图
    ---@type MatchOperate
    self.matchOp = matchOperate

    self.fourCoord = {Vector2.New(0,0),Vector2.New(-1,0),
                      Vector2.New(0,1),Vector2.New(-1,1)}
end

---是否为4个格子
---@return boolean
function MatchControl:IsFourItem(cx, cy)
    for i, v in ipairs(self.fourCoord) do
        local x, y = v.x + cx, v.y + cy
        local item = self.matchView:GetMatchItem(x, y)
        if item then
            if item.info:IsBowl() or item.info:IsJar() then
                return true
            end
        end
    end
    return false
end

---获取下落坐标
---@return number, number
function MatchControl:GetFallCoord(tx, ty)
    local x, y = tx, ty
    for i = 1, 8 do
        if ty-i >= -4 then
            if self:IsFourItem(x, ty-i) then
                break
            end

            local itemBefore = self.matchView:GetMatchItem(x, ty-i)
            if itemBefore == nil then
                y = ty-i
            else
                break
            end
        else
            break
        end
    end
    return x, y
end

---获取可以下落的物品
---@return number, number
function MatchControl:GetFallItem(tx, ty)
    for i = 1, 8 do
        if ty+i <= 4 then
            local item = self.matchView:GetMatchItem(tx, ty+i)
            if item and item.info:IsCanMove() then
                return item
            end
        else
            break
        end
    end
    return nil
end

---下移动完成，补充数据
---@param item MatchBase
function MatchControl:MoveDown(item, tx, ty)
    local ox, oy = item.info.x, item.info.y
    local x, y = self:GetFallCoord(tx, ty)--下落的目标x坐标

    local nextStepPos = Util.Map().IndexCoordToWorld(x, y)
    self.matchView:ChangeMapItem(ox, oy, x, y, item)
    item.info:SetCoord(x, y)

    local nextItem = self:GetFallItem(ox, oy)
    --todo 数据坐标
    item.transform:DOKill()
    item.transform:DOLocalMove(nextStepPos,0.06):OnComplete(Handle(self, self.FinishDown, item,  x, nextItem))
end

---@param item MatchBase
---@param nextItem MatchBase
function MatchControl:FinishDown(item, x, nextItem)
    if item.info:GetCfgInfo().barrierType == EObstacleType.Animal then
        local isFinish = false
        local bornItem = self.matchView:GetMatchItem(item.info.x, -MatchRule.Size)
        if bornItem and bornItem.info:GetCfgInfo().barrierType == EObstacleType.Born then
            if item.info.y == 1-MatchRule.Size then
                isFinish = true
            end
        elseif item.info.y == -MatchRule.Size then
            isFinish = true
        end

        if isFinish then
            self.matchView.itemList[item.info.x][item.info.y] = nil
            item:FinishAnimal()
        end
    end

    if nextItem then
        local nx, ny = nextItem.info.x, nextItem.info.y-1
        local spaceItem = self.matchView:GetMatchItem(nx, ny)
        if spaceItem == nil and not self:IsFourItem(nx, ny) then--下一个是否为空格
            self:MoveDown(nextItem, nx, ny)
        else
            self.matchView:RemoveDownX(x)
        end
    else
        self.matchView:RemoveDownX(x)
        --补充数据
        self:StartRepair(x)
    end
end

function MatchControl:StartRepair(x, isCheck)
    --补充数据
    local yTab = self:GetRepairCell(x)
    for _, _y in ipairs(yTab) do
        self:RepairView(x, _y)
    end

    if isCheck then
        self.matchView:RemoveDownX(x)
    end
end

---获取需要补充的x列数据
---@param x number 补充x列的数据
---@return table<number>
function MatchControl:GetRepairCell(x)
    local yTab = {}
    for y = MatchRule.Size, -MatchRule.Size, -1 do
        local item = self.matchView:GetMatchItem(x, y)
        --可以补充的坐标, 补充数据时，是否为4个格子区域
        if item == nil and not self:IsFourItem(x, y) then
            table.insert(yTab, 1, y)
        else
            break
        end
    end
    return yTab
end

---补充目标数据
function MatchControl:RepairView(x, y)
    local info = Single.Match():CreateOnly(x, y)
    local startPos = Util.Map().IndexCoordToWorld(x, 5)
    local item = self.matchView:CreateGo(info)
    self.matchView:ChangeMapItem(nil,nil, x, y, item)
    item.transform:SetPosition(startPos)
    --todo 数据坐标
    item.transform:DOKill()
    item.transform:DOLocalMove(info:GetWorldPos(),0.06)
end

return MatchControl