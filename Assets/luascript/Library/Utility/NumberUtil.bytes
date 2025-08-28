---数字工具类，与数字相关的

---@class NumberUtil
local NumberUtil = {}

local WeekStr = {"日","一","二","三","四","五","六",}---数字转文本星期天数
local ChineseNumber = {"一","二","三","四","五","六","七","八","九","十","十一","十二","十三","十四","十五",}---数字转中文数字

local hzUnit = {"", "十", "百", "千", "万", "十", "百", "千", "亿","十", "百", "千", "万", "十", "百", "千"}
local hzNum = {"零", "一", "二", "三", "四", "五", "六", "七", "八", "九"}

--数字直转中文
---@param pNum number
---@return string 1234 -> 一二三四
function NumberUtil.NumConverToStr(pNum)
	local result = ""
	local str = tostring(pNum)
	local len = str:len()
	for i = 1, len do
		result = result .. hzNum[tonumber(str:sub(i, i)) + 1]
	end
	return result
end

--日子转中文 100内的数字有效
---@param pNum number
---@return string 22-> 二十二
function NumberUtil.DayToChinese(pNum)
	if pNum > 100 then return nil end
	local unitPos = 0
	local result = ""
	while pNum > 0 do
		local n = pNum % 10
		if n ~= 0 then
			local strIns = hzNum[n + 1]
			if unitPos == 1 and strIns == "一" then
				strIns = ""
			end
			strIns = strIns .. hzUnit[unitPos + 1]
			result = strIns .. result
		end
		unitPos = unitPos + 1
		pNum = math.floor(pNum / 10)
	end
	return result
end

-- 取整到万以上
---@param pMinValue number 大于该值才取整,不填则默认为9999
function NumberUtil.ConverNumber(pNum,pMinValue)
	pNum = tonumber(pNum)
	if pMinValue == nil then
		pMinValue = 9999
	end
	if pNum > pMinValue then
		return string.format("%s万", math.floor(pNum/1000)/10)
	end
	return pNum
end

--{"一","二","三","四","五","六","日",}---数字转星期天数
function NumberUtil.Number2WeedTime(pNumber)
	local _Index = math.min(pNumber,table.length(WeekStr))
	_Index = math.max(1,_Index)
	return WeekStr[_Index]
end

-- 数字转中文字(1~10)
function NumberUtil.Number2ChineseNum(pNum)
	local _Index = math.min(pNum,table.length(ChineseNumber))
	_Index = math.max(1,_Index)
	return ChineseNumber[_Index]
end

-- 数字转中文字(0~10及以上)
function  NumberUtil.Number2ChineseNum2(szNum)
	---阿拉伯数字转中文大写
	local szChMoney = ""
	local iLen = 0
	local iNum = 0
	local iAddZero = 0
	if nil == tonumber(szNum) then
		return tostring(szNum)
	end
	iLen =string.len(szNum)
	if iLen > 10 or iLen == 0 or tonumber(szNum) < 0 then
		return tostring(szNum)
	end
	for i = 1, iLen  do
		iNum = string.sub(szNum,i,i)
		if iNum == 0 and i ~= iLen then
			iAddZero = iAddZero + 1
		else
			if iAddZero > 0 then
				szChMoney = szChMoney..hzNum[1]
			end
			szChMoney = szChMoney..hzNum[iNum + 1] --//转换为相应的数字
			iAddZero = 0
		end
		if (iAddZero < 4) and (0 == (iLen - i) % 4 or 0 ~= tonumber(iNum)) then
			szChMoney = szChMoney..hzUnit[iLen-i+1]
		end
	end
	local function removeZero(num)
		--去掉末尾多余的 零
		num = tostring(num)
		local szLen = string.len(num)
		local zero_num = 0
		for i = szLen, 1, -3 do
			szNum = string.sub(num,i-2,i)
			if szNum == hzNum[1] then
				zero_num = zero_num + 1
			else
				break
			end
		end
		num = string.sub(num, 1,szLen - zero_num * 3)
		szNum = string.sub(num, 1,6)
		--- 开头的 "一十" 转成 "十" , 贴近人的读法
		if szNum == hzNum[2]..hzUnit[2] then
			num = string.sub(num, 4, string.len(num))
		end
		return num
	end
	local _string = removeZero(szChMoney)
	return _string
end

return NumberUtil;