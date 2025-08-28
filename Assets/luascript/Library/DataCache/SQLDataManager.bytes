---
---
--- Created by hukiry.
--- DateTime: 2022/4/5 10:42
---

---@class SQLFieldType
SQLFieldType = {
    --字符长度不受限制
    TypeTEXT = "TEXT",
    --限制长度 50个字符
    TypeString = "varchar(50)",
    --浮点类型
    TypeFloat = "FLOAT",
    --整数类型
    TypeInt = "INTEGER",
}

---地图表，功能表
---@class SQLDataManager
local SQLDataManager = Class()
local FIELD_KEY = "key"
local FIELD_Value = "value"

function SQLDataManager:ctor()
    ---@type --Hukiry.SQLAccess
    self.sqlAccess = nil;
end

---初始化本地玩家登录数据库
---@param roleID number
function SQLDataManager:InitLocalLogin(roleID)
    if roleID == nil then
        log("角色id 不能为空",roleID,"red")
        return
    end

    if self.sqlAccess == nil then return end
    --self.sqlAccess = Hukiry.SQLAccess.New()
    self.sqlAccess:OpenSQL(roleID)
end

---创建表
function SQLDataManager:CreateTable(tableName)
    if self.sqlAccess == nil then return end
    self.sqlAccess:CreateTableLua(tableName, FIELD_KEY.."#"..FIELD_Value, SQLFieldType.TypeString.."#"..SQLFieldType.TypeTEXT)
end

---返回json 字符串
---@return string
function SQLDataManager:GetTable(tableName)
    if self.sqlAccess == nil then return nil end
    return self.sqlAccess:GetTableDataLua(tableName)
end

---获取指定行
---@param keyName string key名
---@return string
function SQLDataManager:GetTableLine(tableName, keyName)
    if self.sqlAccess == nil then return nil end
    return self.sqlAccess:GetTableLine(tableName, FIELD_KEY, "'"..keyName.."'")
end

---删除表中所有行
function SQLDataManager:DeleteTableMulLine(tableName)
    if self.sqlAccess == nil then return end
    self.sqlAccess:DeleteTableLua(tableName)
end

---删除指定key的行
---@param keyName string key名
function SQLDataManager:DeleteLineIF(tableName, keyName)
    if self.sqlAccess == nil then return end
    self.sqlAccess:DeleteLua(tableName, FIELD_KEY, "'"..keyName.."'")
end

---添加新数据
---@param keyName string key名
---@param jsonValue string 字符串
function SQLDataManager:AddInfo(tableName,  keyName, jsonValue)
    if self.sqlAccess == nil then return end
    self.sqlAccess:InsertInfoLua(tableName, keyName, "'"..keyName.."'#'"..jsonValue.."'", FIELD_KEY)
end

---修改数据
---@param keyName string key名
---@param jsonValue string 字符串
function SQLDataManager:UpdateInfo(tableName, keyName,  jsonValue)
    if self.sqlAccess == nil then return end
    self.sqlAccess:UpdateInfoLua(tableName, FIELD_KEY, "'"..keyName.."'", FIELD_Value, "'"..jsonValue.."'")
end

---检查表是否存在
---@return boolean
function SQLDataManager:CheckHasTable(tableName)
    if self.sqlAccess == nil then return false end
    return self.sqlAccess:CheckHasTable(tableName)
end

---key值是否存在
---@param keyName string key名
---@return boolean
function SQLDataManager:IsHasKey(tableName, keyName)
    if self.sqlAccess == nil then return false end
    return self.sqlAccess:IsHasKey(tableName, keyName, FIELD_KEY)
end

return SQLDataManager


