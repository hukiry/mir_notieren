---
--- MailInfo       
--- Author : hukiry     
--- DateTime 2023/3/14 10:12   
---

---@class MailInfo
local MailInfo = Class()

---@param v MAILDATA
function MailInfo:ctor(v)
    ---邮件id
    self.mailId = v.id
    ---奖励id- {{}}
    self.rewards = v.rewards
    ---邮件id
    self.title = v.title
    ---邮件内容
    self.content = v.content

    ---是生命
    self.isLife = v.isLife

    if string.len(v.rewards) > 0 then
        ---格式：[[moneyType, num],...] 如：[[1,2],[3,2],...]
        self.rewardList = json.decode(v.rewards)
    end

    ---创建时间
    self.creatTime = 0
end


---是有奖励
---@return boolean
function MailInfo:IsHasReward()
    return self.rewardList~=nil and #self.rewardList>0
end

---@return table<{number, number}>
function MailInfo:GetRewardArray()
    return self.rewardList
end

function MailInfo:GetTitleText()
    if string.len(self.title) > 0 then
        return self.title
    end
    return GetLanguageText(10022)
end

return MailInfo