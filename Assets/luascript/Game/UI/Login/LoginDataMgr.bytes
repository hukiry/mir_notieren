---
---
--- Create Time:2021-9-21 17:20
--- Author: Hukiry
---

---@class EBill_info
EBill_info = {
    id = 1,
    title = 2,
    content = 3,
    creatTime = 4,
    expirateTime = 5
}

---@class GameVersion
local GameVersion = Class()
function GameVersion:ctor(msg)
    ---资源版本
    self.version = msg.version
    ---资源热更新地址，拼接地址
    self.webUrl = msg.webUrl
    ---从服务器下载ip地址
    self.jsonUrl = msg.jsonUrl
    ---出包模式
    self.workMode = msg.workMode
    ---强更地址
    self.strongerUrl = msg.strongerUrl
    ---App版本，累加且不可修改
    self.appVersion = msg.appVersion
end

---@class LoginDataMgr
local LoginDataMgr = Class()

function LoginDataMgr:ctor()

end

--清理数据
function LoginDataMgr:InitData()
    ---ip 地址
    ---@type string
    self.ip = ""
    ---端口
    ---@type number
    self.port = 0
    ---@type boolean
    self.isHaveAddress = false

    self.isLogin = true
    ---@type GameVersion
    self.gameVersion = nil
end


---@return GameVersion
function LoginDataMgr:GetGameVersion()
    if self.gameVersion == nil then
        local jsonTab = json.decode(MainGame.Instance.JsonGameVersion)
        self.gameVersion =  GameVersion.New(jsonTab)
    end
    return self.gameVersion
end

---@return boolean
function LoginDataMgr:IsHasAddress()
    return self.isHaveAddress
end

---获取ip地址
---@return string
function LoginDataMgr:GetIpAddress()
    return string.format("http://%s:%s/match",self.ip, tostring(self.port))
end

---获取oss上ip地址路径
---@return string
function LoginDataMgr:GetJsonUrl()
    local workMode = MainGame.Instance.WorkModeString
    return "http://match-oss.calf66.top/json/"..workMode..".ini"
end

---上传到oss资源路径
function LoginDataMgr:GetOssUrl(fileName, isHttp)
    local workMode = MainGame.Instance.WorkModeString
    local url = "game/match/"..workMode.."/"..fileName..".binary"
    if isHttp then
        ---match-three.oss-us-west-1.aliyuncs.com
        return "http://match-oss.calf66.top/"..url
    end
    return url
end

---登录成功和登出
function LoginDataMgr:Logout() self.isLogin = false end
function LoginDataMgr:LoginSuccessful() self.isLogin = true end
function LoginDataMgr:IsNeedLogin() return self.isLogin end

---公告提示
---公告测试ok
function LoginDataMgr:HttpBillboard()
    Single.Request().SendMailBoard(false, EHttpMailState.Request)
end

---1,链接服务器：登录界面调用
function LoginDataMgr:ConnectServer()
    local customFunction = function()
        PlayerPrefs.SetInt("USER_INFO",1)
        self:StartRequestLogin()
    end

    if self:CheckTermsPrivacy(customFunction) then
        customFunction()
    end
end

---2,检查用户协议
function LoginDataMgr:CheckTermsPrivacy(callback)
    local isFirst = PlayerPrefs.GetInt("USER_INFO",0) == 1
    if not isFirst then
        UIManager:OpenWindow(ViewID.User, callback)
        return false
    end
    return true
end

---3,请求IP地址
function LoginDataMgr:StartRequestLogin()
    if GameSymbols:IsEnableNetwork() then
        if self:IsHasAddress() then
            UIManager:OpenWindow(ViewID.ServerTip)
            NetSocket:Connect(self.ip, self.port)
        else
            if Single.Http():IsHaveNetwork() then
                UIManager:OpenWindow(ViewID.ServerTip)
                self:RequestAddress(function(isSuccesful)
                    if isSuccesful then
                        NetSocket:Connect(self.ip, self.port)
                    else
                        local content = Single.Http():IsHaveNetwork() and  10001 or 10002
                        UIManager:OpenWindow(ViewID.PromptBox, GetLanguageText(content),  Handle(self,  self.StartRequestLogin))
                    end
                end)
            else
                local content = Single.Http():IsHaveNetwork() and  10001 or 10002
                UIManager:OpenWindow(ViewID.PromptBox, GetLanguageText(content),  Handle(self,  self.StartRequestLogin))
            end
        end
    else
        self:EnterGame()--单击
    end
end

---4,请求地址
function LoginDataMgr:RequestAddress(backCall)
    local jsonUrl = self:GetJsonUrl()
    Single.Http():HttpGet(jsonUrl, nil, function(text, succ)
        if succ then
            local tab = json.decode(text)
            if tab.code == 0 then
                self.ip = tab.ip
                self.port = tab.port
                self.isHaveAddress = true

                if backCall then
                    backCall(true)
                end
            end
        else
            if backCall then
                backCall(false)
            end
        end
    end)
end


---5,登录成功，进入游戏：协议返回调用
function LoginDataMgr:EnterGame()
    Single.Player():ReadRoleData()
    self:LoginSuccessful()
    UIManager:CloseWindow(ViewID.ServerTip)
    SceneApplication.ChangeState(ViewScene)
end

return LoginDataMgr