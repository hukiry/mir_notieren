
---
--- Sheet1->Pass
--- Created by Hukiry 工具自动生成.

---@class TablePassItem
local TablePassItem = Class()
function TablePassItem:ctor(data)

	---通行证id
	---@type number
	self.id = data[1]

	---奖励类型图标
	---@type number
	self.type = data[2]

	---免费奖励
	---@type table<table<number, number>>
	self.freeRewards = {}
	local arr_dic = string.Split(data[3], '|')
	for i, v in ipairs(arr_dic) do
		local temp_dic = string.Split(v, ',')
		if #temp_dic == 2 then
			table.insert(self.freeRewards, {tonumber(temp_dic[1]), tonumber(temp_dic[2])})
		end
	end

	---支付奖励
	---@type table<table<number, number>>
	self.payRewards = {}
	local arr_dic = string.Split(data[4], '|')
	for i, v in ipairs(arr_dic) do
		local temp_dic = string.Split(v, ',')
		if #temp_dic == 2 then
			table.insert(self.payRewards, {tonumber(temp_dic[1]), tonumber(temp_dic[2])})
		end
	end

	---充值id
	---@type number
	self.shopId = data[5]

	data = nil

end

---@class TablePassHelp:ConfigurationBase
local TablePassHelp = Class(ConfigurationBase)

function TablePassHelp:ctor()
	self.TableItem = TablePassItem
	self.sourceTable = require('LuaConfig.Pass.TablePass')
end

---@param id number 主键
---@param faultTolerance boolean 默认为true:容错，如果未找到则返回第一条数据，false:不容错
---@return TablePassItem
function TablePassHelp:GetKey(id, faultTolerance)
	return self:_GetKey(id, faultTolerance)
end

--Custom_Code_Begin



--Custom_Code_End

return TablePassHelp
