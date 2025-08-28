
---
--- Sheet1->Global
--- Created by Hukiry 工具自动生成.

---@class TableGlobalItem
local TableGlobalItem = Class()
function TableGlobalItem:ctor(data)

	---配置名
	---@type string
	self.itemKey = data[1]

	---
	---@type number
	self.id = data[2]

	---配置内容
	---@type string
	self.itemValue = data[3]

	data = nil

end

---@class TableGlobalHelp:ConfigurationBase
local TableGlobalHelp = Class(ConfigurationBase)

function TableGlobalHelp:ctor()
	self.TableItem = TableGlobalItem
	self.sourceTable = require('LuaConfig.Global.TableGlobal')
end

---@param itemKey number 主键
---@param faultTolerance boolean 默认为true:容错，如果未找到则返回第一条数据，false:不容错
---@return TableGlobalItem
function TableGlobalHelp:GetKey(itemKey, faultTolerance)
	return self:_GetKey(itemKey, faultTolerance)
end

--Custom_Code_Begin

---字符串公式转换为函数
---@param key string
---@return function<number, number, number, ...>
function TableGlobalHelp:GetFormula(key)
	local params = "params"
	local expression = self:GetKey(key).itemValue
	for i = 1, 5 do
		---替换字符串
		expression = string.gsub(expression,"#"..i, params.."["..i.."]")
	end
	---将字符串封装成函数
	local strfunc = loadstring("return function(...) local params = { ... } return " .. string.lower(expression) .." end")
	local func = strfunc()
	return func
end

--Custom_Code_End

return TableGlobalHelp
