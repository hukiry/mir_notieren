
---
--- 商店->Shop
--- Created by Hukiry 工具自动生成.

---@class TableShopItem
local TableShopItem = Class()
function TableShopItem:ctor(data)

	---货品ID
	---@type number
	self.productId = data[1]

	---商品名称
	---@type number
	self.name = data[2]

	---商品标记名
	---@type number
	self.tag = data[3]

	---商品id
	---@type number
	self.payId = data[4]

	---大图标
	---@type string
	self.icon = data[5]

	---获得金币奖励
	---@type number
	self.coin = data[6]

	---获得道具id
	---@type table<number>
	self.rewardIds = {}
	local arr_list = string.Split(data[7], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.rewardIds, tonumber(v))
		end
	end

	---获得道具数量
	---@type table<number>
	self.rewardNums = {}
	local arr_list = string.Split(data[8], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.rewardNums, tonumber(v))
		end
	end

	data = nil

end

---@class TableShopHelp:ConfigurationBase
local TableShopHelp = Class(ConfigurationBase)

function TableShopHelp:ctor()
	self.TableItem = TableShopItem
	self.sourceTable = require('LuaConfig.Shop.TableShop')
end

---@param productId number 主键
---@param faultTolerance boolean 默认为true:容错，如果未找到则返回第一条数据，false:不容错
---@return TableShopItem
function TableShopHelp:GetKey(productId, faultTolerance)
	return self:_GetKey(productId, faultTolerance)
end

--Custom_Code_Begin



--Custom_Code_End

return TableShopHelp
