
---
--- 充值->Recharge
--- Created by Hukiry 工具自动生成.

---@class TableRechargeItem
local TableRechargeItem = Class()
function TableRechargeItem:ctor(data)

	---货品ID
	---@type number
	self.payId = data[1]

	---充值id
	---@type string
	self.googlePayId = data[2]

	---充值价格
	---@type number
	self.price = data[3]

	---充值显示
	---@type string
	self.priceShow = data[4]

	data = nil

end

---@class TableRechargeHelp:ConfigurationBase
local TableRechargeHelp = Class(ConfigurationBase)

function TableRechargeHelp:ctor()
	self.TableItem = TableRechargeItem
	self.sourceTable = require('LuaConfig.Recharge.TableRecharge')
end

---@param payId number 主键
---@param faultTolerance boolean 默认为true:容错，如果未找到则返回第一条数据，false:不容错
---@return TableRechargeItem
function TableRechargeHelp:GetKey(payId, faultTolerance)
	return self:_GetKey(payId, faultTolerance)
end

--Custom_Code_Begin

---@param key number 货品id
---@return number
function TableRechargeHelp:GetShowPrice(key)
	local info = self:GetKey(tonumber(key))
	return info.priceShow
end

---充值id，对应商店配置表
---@type table<string, TableShopItem>
local _rechargeGoods = {}
---@param rechargeId string 充值id
---@return string
function TableRechargeHelp:GetShopName(rechargeId)
	if _rechargeGoods[rechargeId] then
		return _rechargeGoods[rechargeId].name
	else
		---@type table<number,TableRechargeItem>
		local tab = self:GetTable()
		for i, v in pairs(tab) do
			if v.googlePayId == rechargeId then
				_rechargeGoods[rechargeId] = SingleConfig.Shop():GetKey(v.payId)
				break;
			end
		end

		if _rechargeGoods[rechargeId] then
			return _rechargeGoods[rechargeId].name
		end
		return nil
	end
end

--Custom_Code_End

return TableRechargeHelp
