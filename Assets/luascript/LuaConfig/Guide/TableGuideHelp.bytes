
---
--- Sheet1->Guide
--- Created by Hukiry 工具自动生成.

---@class TableGuideItem
local TableGuideItem = Class()
function TableGuideItem:ctor(data)

	---引导id
	---@type number
	self.id = data[1]

	---场景类型
	---@type number
	self.type = data[2]

	---鼠标操作类型
	---@type number
	self.operateType = data[3]

	---触发限制条件
	---@type number
	self.lv = data[4]

	---触发目标条件
	---@type number
	self.targetId = data[5]

	---引导语言文本
	---@type number
	self.lanContent = data[6]

	---界面引导物品文字
	---@type table<number>
	self.iconIds = {}
	local arr_list = string.Split(data[7], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.iconIds, tonumber(v))
		end
	end

	---箭头指引坐标
	---@type table<number>
	self.arrowPos = {}
	local arr_list = string.Split(data[8], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.arrowPos, tonumber(v))
		end
	end

	---箭头指引坐标
	---@type table<number>
	self.targetPos = {}
	local arr_list = string.Split(data[9], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.targetPos, tonumber(v))
		end
	end

	---显示跳过时间
	---@type number
	self.delayTime = data[10]

	---是跳转
	---@type boolean
	self.isSkip = data[11] == 1

	---遮罩区域
	---@type table<number>
	self.areaMask = {}
	local arr_list = string.Split(data[12], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.areaMask, tonumber(v))
		end
	end

	data = nil

end

---@class TableGuideHelp:ConfigurationBase
local TableGuideHelp = Class(ConfigurationBase)

function TableGuideHelp:ctor()
	self.TableItem = TableGuideItem
	self.sourceTable = require('LuaConfig.Guide.TableGuide')
end

---@param id number 主键
---@param faultTolerance boolean 默认为true:容错，如果未找到则返回第一条数据，false:不容错
---@return TableGuideItem
function TableGuideHelp:GetKey(id, faultTolerance)
	return self:_GetKey(id, faultTolerance)
end

--Custom_Code_Begin



--Custom_Code_End

return TableGuideHelp
