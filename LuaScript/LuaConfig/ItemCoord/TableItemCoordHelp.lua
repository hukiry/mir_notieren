
---
--- 消除坐标->ItemCoord
--- Created by Hukiry 工具自动生成.

---@class TableItemCoordItem
local TableItemCoordItem = Class()
function TableItemCoordItem:ctor(data)

	---道具id
	---@type number
	self.itemId = data[1]

	---坐标1
	---@type table<number>
	self.coord1 = {}
	local arr_list = string.Split(data[2], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.coord1, tonumber(v))
		end
	end

	---坐标2
	---@type table<number>
	self.coord2 = {}
	local arr_list = string.Split(data[3], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.coord2, tonumber(v))
		end
	end

	---坐标3
	---@type table<number>
	self.coord3 = {}
	local arr_list = string.Split(data[4], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.coord3, tonumber(v))
		end
	end

	---坐标4
	---@type table<number>
	self.coord4 = {}
	local arr_list = string.Split(data[5], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.coord4, tonumber(v))
		end
	end

	data = nil

end

---@class TableItemCoordHelp:ConfigurationBase
local TableItemCoordHelp = Class(ConfigurationBase)

function TableItemCoordHelp:ctor()
	self.TableItem = TableItemCoordItem
	self.sourceTable = require('LuaConfig.ItemCoord.TableItemCoord')
end

---@param itemId number 主键
---@param faultTolerance boolean 默认为true:容错，如果未找到则返回第一条数据，false:不容错
---@return TableItemCoordItem
function TableItemCoordHelp:GetKey(itemId, faultTolerance)
	return self:_GetKey(itemId, faultTolerance)
end

--Custom_Code_Begin

---@param itemId number 物品id
---@param count number 消除的剩余次数
---@return UnityEngine.Vector3
function TableItemCoordHelp:GetCoord(itemId, count)
	local data = self:GetKey(itemId)
	local tab = data["coord"..count]
	if tab then
		return Vector3.New(tab[1], tab[2],0)
	end
	return Vector3.zero
end

--Custom_Code_End

return TableItemCoordHelp
