---
---Update Time:2024-08-08
---Author:Hukiry
---

---@class SingleData
SingleData = {}

---@type table<string,table>
local list = {}
---导入lua
---@param className string
---@param classPath string
function SingleData.ImportClass(className, classPath)
    if list[className] == nil then
        list[className] = require(classPath).New()
        list[className]:InitData()
    end
    return list[className]
end

---切换账号时登录
function SingleData.InitData()
    for i, v in pairs(list) do
        v:InitData()
    end
end

---退出游戏时清空
function SingleData.ClearData()
    list = {}
end


---@return ActivityDataMgr
function SingleData.Activity()
     return SingleData.ImportClass('Activity','Game.UI.Activity.ActivityDataMgr')
end

---@return FriendDataMgr
function SingleData.Friend()
     return SingleData.ImportClass('Friend','Game.UI.Friend.FriendDataMgr')
end

---@return GuideDataMgr
function SingleData.Guide()
     return SingleData.ImportClass('Guide','Game.UI.Guide.GuideDataMgr')
end

---@return LoginDataMgr
function SingleData.Login()
     return SingleData.ImportClass('Login','Game.UI.Login.LoginDataMgr')
end

---@return MailDataMgr
function SingleData.Mail()
     return SingleData.ImportClass('Mail','Game.UI.Mail.MailDataMgr')
end

---@return MassDataMgr
function SingleData.Mass()
     return SingleData.ImportClass('Mass','Game.UI.Mass.MassDataMgr')
end

---@return MetaHomeDataMgr
function SingleData.Metauniverse()
     return SingleData.ImportClass('Metauniverse','Game.UI.Metauniverse.MetaHomeDataMgr')
end

---@return RankDataMgr
function SingleData.Rank()
     return SingleData.ImportClass('Rank','Game.UI.Rank.RankDataMgr')
end

---@return ShopDataMgr
function SingleData.Shop()
     return SingleData.ImportClass('Shop','Game.UI.Shop.ShopDataMgr')
end
