
---
--- Sheet1->MultiLanguage
--- Created by Hukiry 工具自动生成.

---@class TableMultiLanguageItem
local TableMultiLanguageItem = Class()
function TableMultiLanguageItem:ctor(data)

	---语言序号
	---@type number
	self.lanId = data[1]

	---显示语言名称
	---@type string
	self.name = data[2]

	---语言代码
	---@type string
	self.code = data[3]

	---游戏名称
	---@type string
	self.gameName = data[4]

	data = nil

end

---@class TableMultiLanguageHelp:ConfigurationBase
local TableMultiLanguageHelp = Class(ConfigurationBase)

function TableMultiLanguageHelp:ctor()
	self.TableItem = TableMultiLanguageItem
	self.sourceTable = require('LuaConfig.MultiLanguage.TableMultiLanguage')
end

---@param lanId number 主键
---@param faultTolerance boolean 默认为true:容错，如果未找到则返回第一条数据，false:不容错
---@return TableMultiLanguageItem
function TableMultiLanguageHelp:GetKey(lanId, faultTolerance)
	return self:_GetKey(lanId, faultTolerance)
end

--Custom_Code_Begin

local _lanCodeTab = {}
---@return TableMultiLanguageItem
function TableMultiLanguageHelp:GetInfoByCode(code)
	if _lanCodeTab[code]  then
		return _lanCodeTab[code]
	end

	self:InitCode()

	if _lanCodeTab[code] == nil then
		return _lanCodeTab['en']
	end

	return _lanCodeTab[code]
end

function TableMultiLanguageHelp:InitCode()
	local tab = self:GetTable()
	for i, v in pairs(tab) do
		if _lanCodeTab[v.code] == nil then
			_lanCodeTab[v.code] = v
		end
	end
end

---@return boolean
function TableMultiLanguageHelp:IsExit(code)
	if _lanCodeTab[code]  then
		return true
	end
	self:InitCode()
	return _lanCodeTab[code]~=nil
end

--Custom_Code_End

return TableMultiLanguageHelp
