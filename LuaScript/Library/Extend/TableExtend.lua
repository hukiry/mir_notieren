
--region string 扩展类
---分割字符
function string.Split(str, sep)
	local fields = {}
	if IsEmptyString(str) or IsEmptyString(sep) then
		return fields
	end

	str:gsub("[^" .. sep .. "]+", function(c) fields[#fields + 1] = c end)
	return fields
end

---判断 是否以sep开头
function string.StartsWith(str, sep)
	if IsEmptyString(str) or IsEmptyString(sep) then
		return false
	end

	if string.find(str, sep) ~= 1 then
		return false
	else
		return true
	end
end

---去掉两边空格
function string.Trim(s)
	return (string.gsub(s, "^%s*(.-)%s*$", "%1"))
end
--endregion

--region table 扩展类
function table.unique(t, bArray)
	local check = { }
	local n = { }
	local idx = 1
	for k, v in pairs(t) do
		if bArray then
			n[idx] = v
			idx = idx + 1
		else
			n[k] = v
		end
	end
	return n
end

---判断是否存在
---@param t table 对象
---@param value userdata 子对象
---@return boolean
function table.contains(t, value)
	local len = #t
	for i = 1, len do
		if t[i] == value then
			return true
		end
	end
	return false
end

---判断字典中是否含有key值数据
---@param t table 对象
---@param key userdata key值
---@return boolean
function table.Iscontains(t, key)
	for k,v in pairs(t) do
		if k==key then
			return true
		end
	end
	return false
end

---返回指定值在数组中的索引
---@param t table 对象
---@param value userdata 子对象
---@param iBegin number 超始坐标
---@return number 返回找到的索引值，如果未找到，返回-1
function table.indexOf(t, value, iBegin)
    for i = iBegin or 1, #t do
        if t[i] == value then
            return i
        end
    end
    return -1
end

---根据key返回指定值在数组中的索引
---@param t table 对象
---@param key string 字段名
---@param value 值
---@param iBegin number 超始坐标
---@return number 返回找到的索引值，如果未找到，返回-1
function table.indexOfByKey(t, key, value, iBegin)
	for i = iBegin or 1, #t do
		if t[i][key] == value then
			return i
		end
	end
	return -1
end

---返回table长度
---@return number
function table.length(t)
	if t == nil then return 0 end
	local _len = 0
	for k, v in pairs(t) do
		_len = _len + 1
	end
	return _len
end

---返回第一个数据
function table.first(t)
	for k, v in pairs(t) do
		return v, k
	end
	return nil
end

---返回最后一个数据
function table.last(t)
	local len = #t
	if len > 0 then
		return t[len]
	end
	return nil
end

---删除并返回第一个数据 数组数据
function table.dequeueFirstArr(t)
	local len = #t
	if len > 0 then
		local v = t[1]
		table.remove(t, 1)
		return v
	end
	return nil
end

---删除并返回最后一个数据 数组数据
function table.dequeueLastArr(t)
	local len = #t
	if len > 0 then
		local v = t[len]
		table.remove(t, len)
		return v
	end
	return nil
end

---删除并返回  对象
---@param {}
---@param key number
function table.dequeueDic(t, key)
	if t == nil then return nil end
	local value = t[key]
	table.remove(t, key)
	return value
end

---清理table
function table.clear(t)
	if t == nil then
		return
	end
	for k, v in pairs(t) do
		t[k] = nil
	end
end

---数组拼接，并返回新的数组
function table.addRange(t,t1)
	local result = {}
	local index = 1
	for i = 1, #t do
		result[index] = t[i]
		index = index +1
	end
	for i = 1, #t1 do
		result[index] = t1[i]
		index = index +1
	end
	return result
end

---获取范围
---@param t table<>
---@param startIndex number 开始索引(含起点)
---@param len number 获取长度
function table.getRange(t, startIndex, len)
	local result = {}
	local endIndex = startIndex + len;
	for i = 1, #t do
		if i >= startIndex and i < endIndex then
			table.insert(result, t[i])
		end
	end
	return result
end

---集合拼接2个表，并返回新的table
function table.addArryTable(t,t1)
	local result = {}
	for key, v in ipairs(t) do
		table.insert(result, v)
	end
	for key, v in ipairs(t1) do
		table.insert(result, v)
	end
	return result
end

---字典拼接2个表，并返回新的table
function table.addTable(t,t1)
	local result = {}
	for key, v in pairs(t) do
		result[key] = v
	end
	for key, v in pairs(t1) do
		result[key] = v
	end
	return result
end

---字典拼接，在原table上拼接
function table.addTable2(t,t1)
	for key, v in pairs(t1) do
		t[key] = v
	end
end

---字典值转换为数组
function table.toArray(t)
	local _result = {}
	for _, v in pairs(t) do
		table.insert(_result, v)
	end
	return _result
end

---字典键转换为数组
function table.toArrayKey(t)
	local _result = {}
	for k, v in pairs(t) do
		if v then
			table.insert(_result, k)
		end
	end
	return _result
end

---克隆table(但数组内还是引用)
function table.clone(tab)
	local tmp = {}
	for k, v in pairs(tab) do
		tmp[k] = v
	end
	return tmp
end

---克隆table(完全克隆，非引用，除函数体)	不支持protobuf数据
function table.clone2(tab)
	if tab == nil or type(tab) ~= "table" then
		return tab
	end
	local result = {}
	local function clone_fun(obj, source)
		for k, v in pairs(source) do
			if type(v) == "table" then
				obj[k] = {}
				clone_fun(obj[k], v)
			else
				obj[k] = v
			end
		end
	end
	clone_fun(result, tab)
	return result
end

---反转数组，只能翻转数组
---@return table<>
function table.reverse(tab)
	local tmp = {}
	local len = #tab
	for i = 1, len do
		tmp[i] = tab[len - i + 1]
	end
	return tmp
end

---装入并去重，如果数组中有，不放入
function table.insertUnique(t, v)
	if not table.contains(t, v) then
		table.insert(t, v)
	end
end

---检测是否存在，存在则删除
function table.removeContains(t, value, iBegin)
    if t == nil or value == nil then return false end
    for i = iBegin or 1, #t do
        if t[i] == value then
            table.remove(t, i)
            return true
        end
    end
    return false
end

---判断表中是否存在符合条件的对象
---@param t table
---@param func function
function table.Find(t, func)
	for i, v in pairs(t) do
		if func(v) then
			return v
		end
	end
	return nil
end

function table.findIndex(t, value)
	for i, v in ipairs(t) do
		if v==value then
			return i
		end
	end
	return 0
end
--endregion
