
---
--- Sheet1->Sign
--- Created by Hukiry 工具自动生成.

---@class TableSignItem
local TableSignItem = Class()
function TableSignItem:ctor(data)

	---签到id
	---@type number
	self.id = data[1]

	---签到天数
	---@type number
	self.day = data[2]

	---奖励类型
	---@type number
	self.rewardType = data[3]

	---道具类型
	---@type number
	self.propType = data[4]

	---奖励数量
	---@type number
	self.rewardNum = data[5]

	data = nil

end

---@class TableSignHelp:ConfigurationBase
local TableSignHelp = Class(ConfigurationBase)

function TableSignHelp:ctor()
	self.TableItem = TableSignItem
	self.sourceTable = require('LuaConfig.Sign.TableSign')
end

---@param id number 主键
---@param faultTolerance boolean 默认为true:容错，如果未找到则返回第一条数据，false:不容错
---@return TableSignItem
function TableSignHelp:GetKey(id, faultTolerance)
	return self:_GetKey(id, faultTolerance)
end

--Custom_Code_Begin



--Custom_Code_End

return TableSignHelp
