---
--- AutoTaskManager       
--- Author : hukiry     
--- DateTime 2023/6/27 17:45   
---

---@class ETaskData
ETaskData = {
    itemId = 0,
    ---任务数量
    itemNum = 0,
    ---任务完成数量
    finishNum = 0,
    rewardType = 0,
    rewardNum = 0,
    totalTime = 0,
    timeId = 0
}
---@class ETask_Message_Data
ETask_Message_Data={
    ---@type table<number, ETaskData>
    tasks = {},
    finsheds = {}
}

---@class AutoTaskManager:DisplayObjectBase
local AutoTaskManager = Class(DisplayObjectBase)
local TASK_DATA = "TASK_DATA"
function AutoTaskManager:ctor()
    self.itemNumberTab = {
        [0] = {
            getNum = function()
                return math.random(1,5)*150
            end,
            getRewardNum = function()
                return 1
            end
        },
        [1] = {
            getNum = function()
                return math.random(1,3)*10
            end,
            getRewardNum = function()
                return  1
            end
        },

        [2] = {
            getNum = function()
                return math.random(3, 10) * 10
            end,
            getRewardNum = function()
                local tab = {50,100,150,200,250,300}
                return tab[math.random(1,#tab)]
            end
        } ,
    }
end
---@private
function AutoTaskManager:InitData()
    ---@type table<EItemType>
    self.itemList = {}

    ---当前任务数据
    ---@type table<number ETaskData>
    self.taskTab = {}

    ---当前任务数据
    ---@type number
    self.taskId = 0

    ---@type ETask_Message_Data
    self.message = nil

    ---@type table<number, TableItemItem>
    local idTab = {}--收集需要生成的id
    local tab = SingleConfig.Item():GetTable()
    for _, v in pairs(tab) do
        local indexId = math.floor(v.itemId/10)
        if indexId ~= 331 and indexId ~= 332 and indexId ~= 333 and indexId ~= 334  and indexId ~= 340 then
            idTab[v.itemId] = v
        end
    end

    ---分类处理
    for i, v in pairs(idTab) do
        if self.itemList[v.itemType] == nil then
            self.itemList[v.itemType] = {}
        end
        table.insert(self.itemList[v.itemType], v.itemId)
    end
end

---获取数据
---@param isFinishTask boolean 完成任务播放动画，创建并刷新——新数据
---@return ETaskData
function AutoTaskManager:GetTaskData(isFinishTask)
    ---首次读取本地数据
    if self.message == nil then
        ---@type ETaskData
        self.message = self:ReadBinaryTable(TASK_DATA, self:ToMessageBody())
        for i, v in ipairs(self.message.tasks) do
            if math.floor(v.itemId/10)==340 then--兼容前方版本
                v.itemId = 104
            end
            self.taskTab[v.timeId] = v
        end
    end

    ---本地数据不存在，或时间过期；则创建时间
    if self.message and #self.message.tasks == 0 then
        self:CreateTaskData()
    else
        local tab ,_ = self:GetArray()
        self.taskId = tab[1].timeId
    end

    return self:GetTaskInfo()
end

---数据发生改变时，保存数据
function AutoTaskManager:SaveData()
    if self.message then
        self.message.tasks = {}
        for i, v in pairs(self.taskTab) do
            local msg = {}--self.message["tasks_Message"]()
            msg.itemId = v.itemId
            msg.itemNum = v.itemNum
            msg.totalTime = v.totalTime
            msg.rewardNum = v.rewardNum
            msg.rewardType = v.rewardType
            msg.finishNum = v.finishNum
            msg.timeId = v.timeId
            msg.itemType = v.itemType
            table.insert(self.message.tasks, msg)
        end

        --大于10的全部移除
        local len = #self.message.finsheds
        if len > 10 then
            table.sort(self.message.finsheds, function(a, b) return a.timeId<b.timeId end)
            for i = len, 10, -1 do
                table.remove(self.message.finsheds, i)
            end
        end
        self:SaveBinaryTable(TASK_DATA, self.message)
    end
end

---记录任务
---@param itemId number
function AutoTaskManager:UpdateTaskTarget(itemId)
    local info = self:GetTaskInfo()
    if info and info.itemId == itemId then
        info.finishNum = info.finishNum + 1
        if info.finishNum>=info.itemNum then
            info.finishNum = info.itemNum
            local msg = {}--self.message["finsheds_Message"]()
            msg.timeId = info.timeId
            msg.state = 1
            msg.rewardType = info.rewardType
            msg.rewardNum = info.rewardNum
            table.insert(self.message.finsheds, msg)
            self.taskTab[self.taskId] = nil
            --todo 获奖动画
            Single.Player():SetMoneyNum(info.rewardType, info.rewardNum)
            self:SaveData()
        end
    end
end

---检查时间的倒计时，如果
function AutoTaskManager:UpdateTaskTime()
    local info = self:GetTaskInfo()
    if info and info.totalTime then
        info.totalTime = info.totalTime - 1
        EventDispatch:Broadcast(ViewID.Game, 2)
    end
end

---@return ETaskData
function AutoTaskManager:GetTaskInfo()
    return self.taskTab[self.taskId]
end

---@private
function AutoTaskManager:ToMessageBody()
    return {
        tasks_IsArray = true,
        tasks = {
            itemId = protobuf_type.int16,
            itemNum = protobuf_type.int16,
            finishNum = protobuf_type.int16,
            rewardType = protobuf_type.byte,
            rewardNum = protobuf_type.int16,
            totalTime = protobuf_type.uint32,
            timeId = protobuf_type.uint32,
            itemType = protobuf_type.byte,
        },

        finsheds_IsArray = true,
        finsheds = {
            timeId = protobuf_type.uint32,
            state  = protobuf_type.byte,
            rewardType = protobuf_type.byte,
            rewardNum = protobuf_type.int16
        }
    }
end

---@private
function AutoTaskManager:CreateTaskData()
    if self.message == nil then
        self.message = protobuf.ConvertMessage(self:ToMessageBody())
    end

    ---创建10个任务
    for i = 1, 10 do
        local itemId, itemNum, totalTime, rewardNum, rewardType, itemType = self:CreateOneTask()
        ---@type ETaskData
        local msg = {}--self.message["tasks_Message"]()
        msg.itemId = itemId
        msg.itemNum = itemNum
        msg.totalTime = totalTime
        msg.rewardNum = rewardNum
        msg.rewardType = rewardType
        msg.finishNum = 0
        msg.timeId = os.time() + i
        ---@type EItemType 普通物品=0，障碍物=2，道具=1
        msg.itemType = itemType
        self.taskTab[msg.timeId] =msg
        table.insert(self.message.tasks, msg)
    end

    local tab ,_ = self:GetArray()
    self.taskId = tab[1].timeId

    self:SaveData()
end

function AutoTaskManager:GetArray()
    local array = table.toArray(self.taskTab)
    table.sort(array, function(a, b)  return a.timeId<b.timeId end)
    return array, self.message.finsheds
end

function AutoTaskManager:CreateOneTask()
    local itemTy = math.random(0, 2)
    local array = self.itemList[itemTy]
    local itemId = array[math.random(1,#array)]
    local itemNum = self.itemNumberTab[itemTy].getNum()
    local rewardType = self:_GetRewardType(itemTy, itemId)
    local rewardNum = self.itemNumberTab[itemTy].getRewardNum()
    local totalTime = math.random(1, 5)*24*3600--任务天数内
    return itemId, itemNum, totalTime, rewardNum, rewardType, itemTy
end

---获取奖励类型
---@private
---@return EMoneyType
function AutoTaskManager:_GetRewardType(itemTy, itemId)
    if itemTy == 0 then
        return EMoneyType.life
    elseif itemTy == 1 then
        local  itemStyle = SingleConfig.Item():GetKey(itemId).itemStyle
        if itemStyle == EPropsType.Rocket then
            return EMoneyType.gun
        elseif itemStyle == EPropsType.Bomb then
            return EMoneyType.bows
        elseif itemStyle == EPropsType.Dragonfly then
            return EMoneyType.hammer
        else
            return EMoneyType.cap
        end
    else
        return EMoneyType.gold
    end
end

return AutoTaskManager