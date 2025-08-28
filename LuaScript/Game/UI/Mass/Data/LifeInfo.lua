---
--- LifeInfo       
--- Author : hukiry     
--- DateTime 2024/7/20 15:21   
---


---@class LifeInfo
local LifeInfo = Class()
---@param msg GLIFEINFO
function LifeInfo:ctor(msg)
    ---聊天角色id
    self.Id = msg.Id
    ---状态：请求帮助，帮助其他
    self.state = msg.state
    ---帮助进度
    self.count = msg.count
    ---寻求帮助时间
    self.time = msg.time
    ---昵称
    self.nick = msg.nick
end

---@return boolean
function LifeInfo:IsFinish()
    return self.count>=5
end

---@return boolean
function LifeInfo:IsMe()
    return self.Id == Single.Player().roleId
end

function LifeInfo:GetHelpText()
    if self.state == ELifeState.AskHelp then
        return GetLanguageText(14008)
    elseif self.state == ELifeState.Help then
        return GetLanguageText(14018)
    else
        return GetLanguageText(14026)
    end
end

return LifeInfo