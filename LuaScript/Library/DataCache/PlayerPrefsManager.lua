---
--- PlayerPrefsManager       
--- Author : hukiry     
--- DateTime 2022/12/14 14:34   
---

---永久缓存数据
---@class PlayerPrefsManager
local PlayerPrefsManager = Class()

function PlayerPrefsManager:ctor()
    ---使用账号创建时间
    ---@type boolean
    self.isUseCreateAccountTime = true
end

---@param keyName string
---@param value number
function PlayerPrefsManager:SetInt(keyName, value)
    PlayerPrefs.SetInt(self:GetRoleKey(keyName), value)
end

---@param keyName string
---@param value number
function PlayerPrefsManager:SetFloat(keyName, value)
    PlayerPrefs.SetFloat(self:GetRoleKey(keyName), value)
end

---设置值
---@param keyName string
---@param value string
function PlayerPrefsManager:SetString(keyName, value)
    PlayerPrefs.SetString(self:GetRoleKey(keyName), value)
end

---@param keyName string
---@param value boolean
function PlayerPrefsManager:SetBool(keyName, value)
    local result = value == true and 1 or 0
    self:SetInt(keyName, result)
end

---@param keyName string
---@return boolean
function PlayerPrefsManager:GetBool(keyName, defaultValue)
    defaultValue = defaultValue or false
    local result = self:GetInt(keyName, defaultValue== true and 1 or 0)
    return result == 1
end

---@param keyName string
---@return number
function PlayerPrefsManager:GetInt(keyName, defaultValue)
    defaultValue = defaultValue or 0
    return PlayerPrefs.GetInt(self:GetRoleKey(keyName), defaultValue)
end

---@param keyName string
---@return string
function PlayerPrefsManager:GetString(keyName, defaultValue)
    defaultValue = defaultValue or ''
    return PlayerPrefs.GetString(self:GetRoleKey(keyName), defaultValue)
end

---@param keyName string
---@return number
function PlayerPrefsManager:GetFloat(keyName, defaultValue)
    defaultValue = defaultValue or ''
    return PlayerPrefs.GetFloat(self:GetRoleKey(keyName), defaultValue)
end

function PlayerPrefsManager:GetRoleKey(keyName)
    if self.isUseCreateAccountTime  then
        if keyName == nil then
            logError("keyName is not empty to store")
            return Single.Player().roleId
        end
        return keyName .. Single.Player().roleId
    end
    return keyName
end

---@param keyName string
---@return boolean
function PlayerPrefsManager:HasKey(keyName)
    return PlayerPrefs.HasKey(self:GetRoleKey(keyName))
end

---@param keyName string
function PlayerPrefsManager:DeleteKey(keyName)
    PlayerPrefs.DeleteKey(self:GetRoleKey(keyName))
end

function PlayerPrefsManager:DeleteAll()
    PlayerPrefs.DeleteAll()
end

return PlayerPrefsManager

