---场景数据控制
--- MatchInfo       
--- Author : hukiry     
--- DateTime 2023/5/9 21:37   
---

---@class MatchInfo
local MatchInfo = Class()
---罐子和碗生效
---@type table<number, UnityEngine.Vector2>
local DEFAULT_COORD = {
    Vector2.New(0,0),
    Vector2.New(1,0),
    Vector2.New(0,-1),
    Vector2.New(1,-1)
}

function MatchInfo:ctor(itemId, matchData)
    ---@type MatchConfig
    self.matchData = matchData

    ---生成的唯一ID
    ---@type number
    self.onlyID = MatchRule.Match_CreateCount
    MatchRule.Match_CreateCount = MatchRule.Match_CreateCount + 1

    self.itemId = itemId
    ---当前消除次数
    self.count = self:GetCfgInfo().count

    ---用于id区分
    self.indexId = math.floor(itemId/10)

    ---新的x坐标
    self.x = 0
    ---新的y坐标
    self.y = 0
    ---@type EMapLayerView
    self.sortLayer = EMapLayerView.None
    ---仅使用于溜溜球：水平或垂直消除
    self.isHorizontal = false
    ---颜色标记
    ---@type table<number, string>
    self.colorTab = {"red", "yellow", "blue", "green"}
    ---坐标消除记录 x,y,index
    ---@type table<number,table<number,EColorItem>>
    self.fourCoord = {}
    ---消除标记
    self.wipeTag = {}
    if self:IsJar() or self:IsBowl() then
        for _, v in ipairs(DEFAULT_COORD) do
            if self.wipeTag[v.x] == nil then
                self.wipeTag[v.x] = {}
            end
            self.wipeTag[v.x][v.y] = true
        end
    end
end

---消除一次
function MatchInfo:WipeOnce(col, x, y)
    if self:GetCfgInfo():IsWipeBarrier() then
        if self:IsJar() or self:IsBowl() then
            if self:IsBowl() and self.count == self:GetCfgInfo().count then
                self.count = self.count-1
            else
                local nx, ny = x-self.x, y-self.y
                for index, v in ipairs(DEFAULT_COORD) do
                    if self.fourCoord[v.x] == nil then
                        self.fourCoord[v.x] = {}
                    end

                    if v.x == nx and v.y==ny and not self.fourCoord[v.x][v.y] then
                        if self:IsJar()  then
                            local color = self:GetCfgInfo().jarColors[index]
                            if color == col or col == EColorItem.all  then
                                self.fourCoord[v.x][v.y] = col
                                self.count = self.count-1
                            end
                        else
                            self.fourCoord[v.x][v.y] = index
                            self.count = self.count-1
                        end
                        break
                    end
                end
            end
        else
            local color = self:GetCfgInfo().color
            if color ~= EColorItem.none then--盒子颜色值
                if color == col or col == EColorItem.all then
                    self.count = self.count-1
                end
            else
                self.count = self.count-1
            end
        end

        if self.count < 0 then
            self.count = 0
        end
    end
end

function MatchInfo:IsNeedUpdate(x, y, col)
    if self:IsBowl() and self.count == self:GetCfgInfo().count-1 then
        return true
    end

    if self.fourCoord[x] == nil or self.fourCoord[x][y]==nil then
        return false
    end

    local indexCol = self.fourCoord[x][y]
    if col then
        if indexCol>0 and indexCol == col then
            self.fourCoord[x][y] = 0
            self.wipeTag[x][y] = false
            return true
        end
        return false
    elseif indexCol and indexCol > 0 then
        self.fourCoord[x][y] = 0
        self.wipeTag[x][y] = false
        return true
    end
    return false
end

---视图更新消除
---@return boolean
function MatchInfo:IsHasCoord(x, y)
    if self.fourCoord[x] and self.fourCoord[x][y] then
        return true
    end
    return false
end

---道具检查四个格子位置
---@return boolean
function MatchInfo:IsHasWipe(x, y)
    if self.wipeTag[x] then
        return self.wipeTag[x][y] == true
    end
    return false
end

---是碗
---@return boolean
function MatchInfo:IsBowl()
    return self.indexId == 332
end

---是罐子
---@return boolean
function MatchInfo:IsJar()
    return self.indexId == 333
end

---是消除障碍物完成:移除障碍物
---@return boolean
function MatchInfo:IsFinish()
    return self:GetCfgInfo():IsWipeBarrier() and self.count <= 0
end

---@return ItemCfgInfo
function MatchInfo:GetCfgInfo()
    return self.matchData:GetItemInfo(self.itemId)
end

---@return boolean
function MatchInfo:IsProps()
    return self:GetCfgInfo().itemProps > 0
end

---是物品类型
---@return boolean
function MatchInfo:IsNormal()
    return self:GetCfgInfo().itemType == EItemType.Normal
end

---是固定物品
---@return boolean
function MatchInfo:IsFixed()
    return self:GetCfgInfo().barrierType == EObstacleType.Fixed or self:GetCfgInfo().barrierType == EObstacleType.Born
end

---可移动物品，交换物品判断
function MatchInfo:IsCanMove()
    local ty = self:GetCfgInfo().barrierType
    return ty == EObstacleType.None or ty == EObstacleType.Move or ty == EObstacleType.Animal
end

---@param x number
---@param y number
function MatchInfo:SetCoord(x, y)
    self.x = x
    self.y = y
end

---获取罐子图标
---@param index number
---@return string
function MatchInfo:GetJarIcon(index)
    local colorIndex = self:GetCfgInfo().jarColors[index]
    return "jar_"..self.colorTab[colorIndex]
end

---获取世界坐标位置
---@return UnityEngine.Vector3
function MatchInfo:GetWorldPos()
   return Util.Map().IndexCoordToWorld(self.x, self.y)
end

---获取世界坐标位置
---@return UnityEngine.Vector3
function MatchInfo:GetFourPos()
    local v = self:GetWorldPos()
    return Vector3.New(v.x+Grid_WIDTH/2,v.y-Grid_HIGHT/2)
end

---@return string
function MatchInfo:GetIcon()
    if self:GetCfgInfo().barrierType == EObstacleType.Fixed then
        return self:GetCfgInfo().icon
    end
    
    if self:GetCfgInfo():IsWipeBarrier() and self:GetCfgInfo().count>1  then
        return self:GetCfgInfo().icon .. "_"..(self.count-1)
    end

    local spriteName = self.isHorizontal and "horizontal_rocket" or "vertical_rocket"
    if self:GetCfgInfo().itemProps == EPropsType.Rocket then
        return spriteName
    end
    return self:GetCfgInfo().icon
end

---@return string
function MatchInfo:GetFlyIcon()
   return self:GetCfgInfo().targetIcon
end

return MatchInfo
