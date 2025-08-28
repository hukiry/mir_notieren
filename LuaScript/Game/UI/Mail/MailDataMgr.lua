---
---
--- Create Time:2023-03-10
--- Author: Hukiry
---

---@class MailDataMgr:DisplayObjectBase
local MailDataMgr = Class(DisplayObjectBase)

local MAIL_DATA = 'MAIL_DATA'
function MailDataMgr:ctor()

end

--清理数据
function MailDataMgr:InitData()
	---@type table<number, MailInfo>
    self.infoList = {}
    ---@type table<number, number>
    self.lifeList = {}

    self:ReadLifeData()
end

---服务器推送的数据
---@param mails table<number, MAILDATA>
function MailDataMgr:SyncMail(mails)
    for i, v in ipairs(mails) do
        ---@type MailInfo
        local info = require("Game.UI.Mail.Data.MailInfo").New(v)
        self.infoList[info.mailId] = info
    end
    --EventDispatchLua:Broadcast(ViewID.Game, 3, EGamePage.MailView)
end

---@param mail MAILDATA
function MailDataMgr:SyncBillBoard(mail)
    UIManager:OpenWindow(ViewID.CommonTip, mail.content, function()  end, function()  end, false, mail.title)
end

---@param id number
---@return MailInfo
function MailDataMgr:GetMailInfo(id)
    return self.infoList[id]
end

---移除数据
---@param isLife boolean
---@param id number
function MailDataMgr:RemoveData(isLife, id)
    if isLife then
        self.lifeList[id] = nil
        self:SaveLifeData()
    else
        self.infoList[id] = nil
    end
end

---@return table<number, MailInfo>
function MailDataMgr:GetMailArray()
    ---@type table<number, MailInfo>
    local temp = table.toArray(self.infoList)
    table.sort(temp, function(a, b)
        return a.mailId<b.mailId
    end)
    return temp
end

---@return table<number, number>
function MailDataMgr:GetLifeArray()
    local temp = table.toArrayKey(self.lifeList)
    return temp
end

function MailDataMgr:ReadLifeData()
    local msgData = self:ReadBinaryTable(MAIL_DATA, self:ToMessageBody())
    local isCreate = false
    if msgData and msgData.curDayTime and msgData.curDayTime > 0 then
        for _, v in ipairs(msgData.creatTimes) do
            self.lifeList[v] = 1
        end
        isCreate = Util.Time().IsDifferentDay(msgData.curDayTime, os.time(), 1)
    else
        self:SaveLifeData()
    end

    if isCreate and table.length(self.lifeList) < 5 then
        self.lifeList[os.time()] = 1
        self:SaveLifeData()
    end
end

---创建和领取时
function MailDataMgr:SaveLifeData()
    local msgData = protobuf.ConvertMessage(self:ToMessageBody())
    msgData.curDayTime = os.time()
    for key, _ in pairs(self.lifeList) do
        table.insert(msgData.creatTimes, key)
    end
    self:SaveBinaryTable(MAIL_DATA, msgData)
end

---@private
function MailDataMgr:ToMessageBody()
    return {
        curDayTime = protobuf_type.uint32,
        creatTimes_IsArray = true,
        creatTimes = protobuf_type.uint32,
    }
end

return MailDataMgr