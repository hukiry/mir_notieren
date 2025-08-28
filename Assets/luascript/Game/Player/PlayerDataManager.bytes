---
--- 玩家基本属性
--- Created by Administrator.
--- DateTime: 2022/5/3 17:00
---




---@class PlayerDataManager:DisplayObjectBase
local PlayerDataManager = Class(DisplayObjectBase)
local ROLE_INFO = "ROLE_INFO"

function PlayerDataManager:ctor()

end

function PlayerDataManager:InitData()
    ---角色ID
    self.roleId = 0
    ---角色昵称：3-15
    self.roleNick = "Match Meta"
    ---角色头像Id
    self.headId = 1
    ---社团iD
    self.gassId = 0

    self.loginTime = os.time()

    ---当前生命剩余时间
    self.curLifeTime = 0
    ---绑定了facebook
    ---@type boolean
    self.isBindLogin = false
    ---绑定的类型
    self.bindAccount = PlayerPrefs.GetInt("BINDACCOUNT", 0)
    ---@type table<EMoneyType, number> 字典
    self.items = {}

    for i = EMoneyType.gun, EMoneyType.integral do
        self.items[i] = (i==EMoneyType.level or i==EMoneyType.metaExpendNum) and 1 or 0
    end

    ---地图id集合，不可超过30个，state=》0 设计中，1=更新中，2= 已上传 { numberId = 1, state = 0 }
    ---@type table<number, number>
    self.metaMapIds = {}

    self.reqTypeList = {}

    ---@type table<ELogin,string>
    self.itemsPack = {}
end

---@param msg MSG_1002
function PlayerDataManager:InitMsg(msg)
    self:ReadRoleData();
    self.roleId = msg.roleId
    self.isBindLogin = string.len(msg.token) > 0

    ---@param v ITEMSRESOURCE
    for _, v in ipairs(msg.items) do
        self:SetItem(v.type, v.number)
    end

    ---@param v ITEMSPACK
    for _, v in ipairs(msg.itemsPacks) do
        self:SetPackValue(v.type, v.value)
    end

    self.roleNick = self:GetPackValue(ELogin.Nick)
    self.gassId = self:GetPackValue(ELogin.GassID)
    self.headId = self:GetPackValue(ELogin.HeadID)
    self.curLifeTime = self:GetMoneyNum(EMoneyType.life)
end

function PlayerDataManager:SetPackValue(eLogin, value)
    self.itemsPack[eLogin] = value
end

function PlayerDataManager:GetPackValue(eLogin)
    return self.itemsPack[eLogin]
end

---添加编辑id
---@param mapId number 地图编号
function PlayerDataManager:AddMetaMap(mapId)
    local isContains = false
    for _, v in ipairs(self.metaMapIds) do
        if v.numberId == mapId then
            isContains = true
            break
        end
    end

    if not isContains then
        table.insert(self.metaMapIds, { numberId = mapId, state = 0  })
    end
end

---货币是否足够
---@param moneyType EMoneyType 货币类型
---@param costNum number 货币数量
---@param isOpen boolean 货币不足时，是否开口货币弹框
function PlayerDataManager:IsEnough(moneyType, costNum, isOpen, isPopRecharge)
    local curNum =  self.items[moneyType] or 0
    local isEnough =  curNum >= costNum
    if not isEnough and isOpen then
        if moneyType == EMoneyType.life then
            EventDispatch:Broadcast(ViewID.Game, 1, 2)
        elseif moneyType == EMoneyType.gold  then
            if SceneRule.CurSceneType == SceneType.ViewCity and isPopRecharge == nil then
                EventDispatch:Broadcast(ViewID.Game, 1, 1)
            else
                UIManager:OpenWindow(ViewID.Recharge)
            end
        end
    end
    return isEnough
end

---获取货币值
---@param moneyType EMoneyType 货币类型
---@return number
function PlayerDataManager:GetMoneyNum(moneyType)
    local curNum = self.items[moneyType]
    return curNum or 0
end

---更新道具的使用
---@param props EPropsView
function PlayerDataManager:UpdatePropsNum(props)
    local moneyType = EPropsToMoneyType[props]
    self:SetMoneyNum(moneyType, 1, true)
end

---设置货币值：赋值
---@param moneyType EMoneyType 货币类型
---@param num number 货币数量
function PlayerDataManager:SetItem(moneyType, num)
    if self.items[moneyType] == nil then
        self.items[moneyType] = 0
    end

    self.items[moneyType] = num
    EventDispatch:Broadcast(ViewID.Money, moneyType, self.items[moneyType])
end

---设置货币值:添加或扣除
---@param moneyType EMoneyType 货币类型
---@param num number 货币数量
---@param isDiscount boolean 是扣除
function PlayerDataManager:SetMoneyNum(moneyType, num, isDiscount, isReq)
    if self.items[moneyType]==nil then
        self.items[moneyType] = 0
    end

    if isDiscount then
        self.items[moneyType] = self.items[moneyType] - num
        if self.items[moneyType] < 0 then
            self.items[moneyType] = 0
        end
        if moneyType == EMoneyType.life then
            self.curLifeTime = self.curLifeTime - self.items[EMoneyType.lifehour]*3600
        end
    else
        self.items[moneyType] = self.items[moneyType] + num

        if moneyType == EMoneyType.lifehour then
            local h = math.floor(self.curLifeTime/(self.items[moneyType]*3600))
            local addTime = h*num*3600
            self.curLifeTime = self.curLifeTime + addTime
        elseif moneyType == EMoneyType.life then
            self.curLifeTime = self.curLifeTime + self.items[EMoneyType.lifehour]*3600
        elseif moneyType ==  EMoneyType.gold then
            EventDispatch:Broadcast(ViewID.Game, 4)
        end
    end

    if isReq == nil or isReq == true then
        Single.Request().SendMoney(moneyType, self.items[moneyType])
    end
end

---获取货币图标
---@param moneyType EMoneyType
function PlayerDataManager:GetMoneyIcon(moneyType)
    local tabIcon = {  [EMoneyType.life] = 9, [EMoneyType.gold] = 10 }
    local iconIndex = tabIcon[moneyType] or 0
    local info = SingleConfig.Currency():GetKey(iconIndex, false);
    if info==nil then
        return nil
    end
    return 'icon_' .. info.itemId
end

---无网络时：读取数据
function PlayerDataManager:ReadRoleData()
    local roleMsg = self:ReadBinaryTable(ROLE_INFO, self:ToMessageBody())
    if roleMsg and roleMsg.roleId > 0 then
        self.roleId = roleMsg.roleId
        self.roleNick = roleMsg.roleNick
        self.curLifeTime = roleMsg.curLifeTime
        self.isBindLogin = roleMsg.isBindLogin
        self.bindAccount = roleMsg.bindAccount
        self.metaMapIds = roleMsg.metaMapIds
        for _, v in ipairs(roleMsg.items) do
            self.items[v.type] = v.number
        end
        return true
    end
    return false
end

---保存数据：有网络时，上传数据,每隔10秒
---充值成功，需要立马保存
function PlayerDataManager:SaveRoleData(isPause)
    if self.roleId > 0 then
        local roleMsg = protobuf.ConvertMessage(self:ToMessageBody())
        roleMsg.roleId = self.roleId
        roleMsg.roleNick = self.roleNick
        roleMsg.curLifeTime = self.curLifeTime
        roleMsg.isBindLogin = self.isBindLogin
        roleMsg.bindAccount = self.bindAccount
        roleMsg.metaMapIds = self.metaMapIds
        for key, num in pairs(self.items) do
            table.insert(roleMsg.items, {type = key, number = num})
        end

        for key, v in pairs(self.itemsPack) do
            table.insert(roleMsg.itemsPack, {type = key, value = v})
        end

        self:SaveBinaryTable(ROLE_INFO, roleMsg)
        if not isPause then
            Single.Request().SendRoleData()
        end
    end
end

---@private
function PlayerDataManager:ToMessageBody()
    return {
        ---角色ID
        roleId = protobuf_type.int32,
        ---角色昵称：3-15
        roleNick = protobuf_type.string,
        items_IsArray = true,
        items = {
            type = protobuf_type.byte,
            number = protobuf_type.int32,
        },
        ---当前生命剩余时间
        curLifeTime = protobuf_type.int32,
        isBindLogin = protobuf_type.bool,
        ---登录类型
        bindAccount = protobuf_type.byte,
        metaMapIds_IsArray = true,
        metaMapIds = {
            numberId = protobuf_type.byte,
            state = protobuf_type.byte,
        },
        itemsPack_IsArray = true,
        itemsPack = {
            type = protobuf_type.byte,
            value = protobuf_type.string,
        }
    }
end

return PlayerDataManager