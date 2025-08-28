
---
--- 货币资源->Currency
--- Created by Hukiry 工具自动生成.

---@class TableCurrencyItem
local TableCurrencyItem = Class()
function TableCurrencyItem:ctor(data)

	---资源id
	---@type number
	self.key = data[1]

	---图标
	---@type string
	self.icon = data[2]

	---名称
	---@type number
	self.languageName = data[3]

	---描述名称
	---@type number
	self.languageDesc = data[4]

	---出售价格
	---@type number
	self.sellPrice = data[5]

	---出售数量
	---@type number
	self.sellNum = data[6]

	data = nil

end

---@class TableCurrencyHelp:ConfigurationBase
local TableCurrencyHelp = Class(ConfigurationBase)

function TableCurrencyHelp:ctor()
	self.TableItem = TableCurrencyItem
	self.sourceTable = require('LuaConfig.Currency.TableCurrency')
end

---@param key number 主键
---@param faultTolerance boolean 默认为true:容错，如果未找到则返回第一条数据，false:不容错
---@return TableCurrencyItem
function TableCurrencyHelp:GetKey(key, faultTolerance)
	return self:_GetKey(key, faultTolerance)
end

--Custom_Code_Begin



--Custom_Code_End

return TableCurrencyHelp
