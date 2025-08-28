
---
--- 物品-17->Language
--- Created by Hukiry 工具自动生成.

---@class TableLanguageItem
local TableLanguageItem = Class()
function TableLanguageItem:ctor(data)

	---语言表ID
	---@type number
	self.id = data[1]

	---中文
	---@type string
	self.cn = data[2]

	---繁体
	---@type string
	self.hk = data[3]

	---英语
	---@type string
	self.en = data[4]

	---西班牙语
	---@type string
	self.es = data[5]

	---葡萄牙语
	---@type string
	self.pt = data[6]

	---法语
	---@type string
	self.fr = data[7]

	---德语
	---@type string
	self.de = data[8]

	---俄语
	---@type string
	self.ru = data[9]

	---意大利语
	---@type string
	self.it = data[10]

	---日语
	---@type string
	self.ja = data[11]

	---韩语
	---@type string
	self.ko = data[12]

	---越南语
	---@type string
	self.vi = data[13]

	---波兰语
	---@type string
	self.pl = data[14]

	---印尼语
	---@type string
	self.id = data[15]

	---泰语
	---@type string
	self.th = data[16]

	---马来语
	---@type string
	self.ms = data[17]

	---土耳其语
	---@type string
	self.tr = data[18]

	---荷兰语
	---@type string
	self.nl = data[19]

	---希腊语
	---@type string
	self.el = data[20]

	---捷克语
	---@type string
	self.cs = data[21]

	data = nil

end

---@class TableLanguageHelp:ConfigurationBase
local TableLanguageHelp = Class(ConfigurationBase)

function TableLanguageHelp:ctor()
	self.TableItem = TableLanguageItem
	self.sourceTable = require('LuaConfig.Language.TableLanguage')
end

---@param id number 主键
---@param faultTolerance boolean 默认为true:容错，如果未找到则返回第一条数据，false:不容错
---@return TableLanguageItem
function TableLanguageHelp:GetKey(id, faultTolerance)
	return self:_GetKey(id, faultTolerance)
end

--Custom_Code_Begin

---@param id number
---@return string
function TableLanguageHelp:GetLanguage(id)
	local info = self:GetKey(id, false)
	local lanageCode = Single.SdkPlatform():GetLanguageCode()
	if info then
		return info[lanageCode]
	else
		return tostring(id)
	end
end

--Custom_Code_End

return TableLanguageHelp
