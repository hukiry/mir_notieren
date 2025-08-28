
---
--- 掉落表->DropArray
--- Created by Hukiry 工具自动生成.

---@class TableDropArrayItem
local TableDropArrayItem = Class()
function TableDropArrayItem:ctor(data)

	---掉落id
	---@type number
	self.dropId = data[1]

	---固定掉落
	---@type table<number, number>
	self.dropFix = {}
	local arr_dic = string.Split(data[2], '|')
	for i, v in ipairs(arr_dic) do
		local temp_dic = string.Split(v, ',')
		if #temp_dic == 2 then
			self.dropFix[tonumber(temp_dic[1])] = tonumber(temp_dic[2])
		end
	end

	---组1抽取数量
	---@type table<number>
	self.Counts1 = {}
	local arr_list = string.Split(data[3], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.Counts1, tonumber(v))
		end
	end

	---组1权重
	---@type table<number>
	self.Rates1 = {}
	local arr_list = string.Split(data[4], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.Rates1, tonumber(v))
		end
	end

	---奖励库1
	---@type table<table<number, number>>
	self.Group1 = {}
	local arr_dic = string.Split(data[5], '|')
	for i, v in ipairs(arr_dic) do
		local temp_dic = string.Split(v, ',')
		if #temp_dic == 2 then
			table.insert(self.Group1, {tonumber(temp_dic[1]), tonumber(temp_dic[2])})
		end
	end

	---组2抽取数量
	---@type table<number>
	self.Counts2 = {}
	local arr_list = string.Split(data[6], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.Counts2, tonumber(v))
		end
	end

	---组2权重
	---@type table<number>
	self.Rates2 = {}
	local arr_list = string.Split(data[7], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.Rates2, tonumber(v))
		end
	end

	---奖励库2
	---@type table<table<number, number>>
	self.Group2 = {}
	local arr_dic = string.Split(data[8], '|')
	for i, v in ipairs(arr_dic) do
		local temp_dic = string.Split(v, ',')
		if #temp_dic == 2 then
			table.insert(self.Group2, {tonumber(temp_dic[1]), tonumber(temp_dic[2])})
		end
	end

	---组3抽取数量
	---@type table<number>
	self.Counts3 = {}
	local arr_list = string.Split(data[9], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.Counts3, tonumber(v))
		end
	end

	---组3权重
	---@type table<number>
	self.Rates3 = {}
	local arr_list = string.Split(data[10], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.Rates3, tonumber(v))
		end
	end

	---奖励库3
	---@type table<table<number, number>>
	self.Group3 = {}
	local arr_dic = string.Split(data[11], '|')
	for i, v in ipairs(arr_dic) do
		local temp_dic = string.Split(v, ',')
		if #temp_dic == 2 then
			table.insert(self.Group3, {tonumber(temp_dic[1]), tonumber(temp_dic[2])})
		end
	end

	---组4抽取数量
	---@type table<number>
	self.Counts4 = {}
	local arr_list = string.Split(data[12], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.Counts4, tonumber(v))
		end
	end

	---组4权重
	---@type table<number>
	self.Rates4 = {}
	local arr_list = string.Split(data[13], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.Rates4, tonumber(v))
		end
	end

	---奖励库4
	---@type table<table<number, number>>
	self.Group4 = {}
	local arr_dic = string.Split(data[14], '|')
	for i, v in ipairs(arr_dic) do
		local temp_dic = string.Split(v, ',')
		if #temp_dic == 2 then
			table.insert(self.Group4, {tonumber(temp_dic[1]), tonumber(temp_dic[2])})
		end
	end

	---组5抽取数量
	---@type table<number>
	self.Counts5 = {}
	local arr_list = string.Split(data[15], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.Counts5, tonumber(v))
		end
	end

	---组5权重
	---@type table<number>
	self.Rates5 = {}
	local arr_list = string.Split(data[16], ',')
	for i, v in ipairs(arr_list) do
		if v ~= '' then
			table.insert(self.Rates5, tonumber(v))
		end
	end

	---奖励库5
	---@type table<table<number, number>>
	self.Group5 = {}
	local arr_dic = string.Split(data[17], '|')
	for i, v in ipairs(arr_dic) do
		local temp_dic = string.Split(v, ',')
		if #temp_dic == 2 then
			table.insert(self.Group5, {tonumber(temp_dic[1]), tonumber(temp_dic[2])})
		end
	end

	data = nil

end

---@class TableDropArrayHelp:ConfigurationBase
local TableDropArrayHelp = Class(ConfigurationBase)

function TableDropArrayHelp:ctor()
	self.TableItem = TableDropArrayItem
	self.sourceTable = require('LuaConfig.DropArray.TableDropArray')
end

---@param dropId number 主键
---@param faultTolerance boolean 默认为true:容错，如果未找到则返回第一条数据，false:不容错
---@return TableDropArrayItem
function TableDropArrayHelp:GetKey(dropId, faultTolerance)
	return self:_GetKey(dropId, faultTolerance)
end

--Custom_Code_Begin

---获取掉落的物品集合
---@param dropID number 掉落id
---@return table<number, number> 字典集合，掉落物品ID和数量，否则返回空
function TableDropArrayHelp:GetDropItem(dropID)
	local info = self:GetKey(dropID, false)
	if info == nil then return nil end

	local temp = {}
	---必掉落的物品
	for id, num in pairs(info.dropFix) do
		temp[id] = num
	end

	----随机组掉落物品
	for i = 1, 5 do
		local dropNums = info["Counts"..i]
		if #dropNums == 0 then break end
		local len = 0
		if #dropNums==2 and dropNums[1] >= dropNums[2] then
			len = dropNums[2]
		else
			len = #dropNums==1 and dropNums[1] or math.random(dropNums[1], dropNums[2])
		end

		for _ = 1, len do
			local Rates = info["Rates" .. i]
			if #Rates == 1 then
				table.insert(Rates, 10 - Rates[1])
			end
			local index = Util.Math():GetRandom(Rates)
			if index > 0 then
				local tab = info["Group" .. i][index]
				if tab then
					local id ,num =  tab[1], tab[2]
					if temp[id] == nil then temp[id] = 0 end
					temp[id] = temp[id] + num
				end
			end
		end
	end
	return temp
end

--Custom_Code_End

return TableDropArrayHelp
