---
--- 数学工具类
--- Created huiry
--- DateTime: 2021/2/6 20:02

---@class MathUtil
local MathUtil = {}

---获取两向量的夹角2D
---@param curent UnityEngine.Vector3
---@param target UnityEngine.Vector3
---@return number
function MathUtil:GetTwoVectorAnagle(curent, target)
    local dir = (curent - target).normalized
    local angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg
    return angle
end

---获取两向量的夹角z方向旋转的四元素2D
---@param curent UnityEngine.Vector3
---@param target UnityEngine.Vector3
---@return UnityEngine.Quaternion
function MathUtil:GetTwoVectorQuaternion(curent, target)
    local angle = self:GetTwoVectorAnagle(curent, target)
    return Quaternion.AngleAxis(angle, Vector3.forward)
end

---获取向前一步移动2D
---@param curent UnityEngine.Vector3
---@param target UnityEngine.Vector3
---@return UnityEngine.Vector3
function MathUtil:MoveStepNormalized(curent, target)
    local dir = (curent - target).normalized
    return curent + dir
end

---获取新的概率
---@param rateArray table<number> 概率数组集合，整数
---@return number 返回概率数组索引 ,-1 表示没有随机到
function MathUtil:GetRandom(rateArray)
    local reward_pool, com_weight = {}, 0
    -- 根据概率数组构建奖励池
    for _, v in ipairs(rateArray) do
        com_weight = com_weight + v
        table.insert(reward_pool, com_weight)
    end

    --math.randomseed(os.time())
    local value = math.random(1, com_weight)
    local left, right = 1, #reward_pool
    while right >= left do
        local mid = math.floor((left + right) / 2)
        local mid_weight = reward_pool[mid]
        if value == mid_weight then
            right = right - 1
            break
        elseif value < mid_weight then
            right = mid - 1
        else
            left = mid + 1
        end
    end

    local index = right + 1 --此时right为reward_pool中抽到的索引
    return index
end

---@private
---@param randomValue number 随机的值
---@param weights table<number,number> 增量权重
function MathUtil:BisectSearch(weights, randomValue)
    local startPos = 1
    local endPos = #weights
    while startPos<endPos do
        local mid = math.floor((startPos+endPos)/2)
        if randomValue < weights[mid] then
            endPos = mid
        else
            startPos = mid + 1
        end
    end
    return startPos
end

---选中随机值
---@param num number 随机次数
---@return table<number> 权重索引
function MathUtil:ChoicesRandom(rateArray, num)
    local cum_weights = {}
    local count = #rateArray
    local sum = 0
    for i=1, count do
        sum = sum + rateArray[i]
        table.insert(cum_weights, sum)
    end

    local result = {}
    for i=1, num do
        local r = math.random(0, math.floor(sum*10000))/10000.0
        local rIndex =  self:BisectSearch(cum_weights, r)
        table.insert(result, rIndex)
    end
    return result
end

return MathUtil