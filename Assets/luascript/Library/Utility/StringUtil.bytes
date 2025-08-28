---@class StringUtil
local StringUtil = {}

---将字符串拆成单个字符，存在一个table中
function StringUtil.utf8tochars(input)
	local list = {}
	local len  = string.len(input)
	local index = 1
	--{0, 0xc0, 0xe0, 0xf0, 0xf8, 0xfc}
	while index <= len do
		local c = string.byte(input, index)
		local offset = 1
		if c < 0xc0 then
			offset = 1
		elseif c < 0xe0 then
			offset = 2
		elseif c < 0xf0 then
			offset = 3
		elseif c < 0xf8 then
			offset = 4
		elseif c < 0xfc then
			offset = 5
		end
		local str = string.sub(input, index, index + offset-1)
		index = index + offset
		table.insert(list, {byteNum = offset, char = str})
	end
	return list
end

---字符串的长度
function StringUtil.Utf8len(input)
	local len  = string.len(input)
	local left = len
	local cnt  = 0
	local arr  = {0, 0xc0, 0xe0, 0xf0, 0xf8, 0xfc}
	while left ~= 0 do
		local tmp = string.byte(input, -left)
		local i   = #arr
		while arr[i] do
			if tmp >= arr[i] then
				left = left - i
				break
			end
			i = i - 1
		end
		cnt = cnt + 1
	end
	return cnt
end

---转换为ASCII字节数组
---@return table<number, number>
function StringUtil.ToASCII(str)
	local t = {}
	for i = 1, #str do
		table.insert(t, string.byte(i, 1))
	end
	return t
end

---获取Utf8的长度
function StringUtil.Length(str)
	return utf8.len(str)
end

---截取Utf8字符串
---@param index number 开始位置
---@param len number 长度
function StringUtil.Sub(str, index, len)
	return utf8.sub(str, index, len)
end

---替换字符串
---@param oldValue string 旧串
---@param newValue string 新串
function StringUtil.Replace(str, oldValue,  newValue)
	return string.gsub(str, oldValue,  newValue)
end


return StringUtil