
---
--- 道具表->Item
--- Created by Hukiry 工具自动生成.

---@class TableItemItem
local TableItemItem = Class()
function TableItemItem:ctor(data)

	---道具id
	---@type number
	self.itemId = data[1]

	---道具名称
	---@type number
	self.name = data[2]

	---对应颜色
	---@type number
	self.color = data[3]

	---消除次数
	---@type number
	self.count = data[4]

	---物品类型
	---@type number
	self.itemType = data[5]

	---道具类型
	---@type number
	self.itemStyle = data[6]

	---障碍物类型
	---@type number
	self.barrierType = data[7]

	---图标
	---@type string
	self.icon = data[8]

	---罐子颜色集合
	---@type table<number>
	self.jarColors = {}
	local arr_list = string.Split(data[9], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.jarColors, tonumber(v))
		end
	end

	---目标显示图标
	---@type string
	self.targetIcon = data[10]

	---元图标
	---@type string
	self.metaIcon = data[11]

	---分类页签类型
	---@type number
	self.metaPage = data[12]

	---脚本分类
	---@type number
	self.scriptType = data[13]

	data = nil

end

---@class TableItemHelp:ConfigurationBase
local TableItemHelp = Class(ConfigurationBase)

function TableItemHelp:ctor()
	self.TableItem = TableItemItem
	self.sourceTable = require('LuaConfig.Item.TableItem')
end

---@param itemId number 主键
---@param faultTolerance boolean 默认为true:容错，如果未找到则返回第一条数据，false:不容错
---@return TableItemItem
function TableItemHelp:GetKey(itemId, faultTolerance)
	return self:_GetKey(itemId, faultTolerance)
end

--Custom_Code_Begin

---获取工人的名称和描述
---@param itemId number
---@return string, string
function TableItemHelp:GetItemNameAndDesc(itemId)

end

--Custom_Code_End

return TableItemHelp
