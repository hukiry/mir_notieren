---@class TimeUtil
local TimeUtil = {}

---服务器时间（秒）
TimeUtil.serverTime = 0
---服务器时间（毫秒）
TimeUtil.meseTime = 0

TimeUtil.daySecond = 86400	---一天多少秒
TimeUtil.dayMeseTime = TimeUtil.daySecond*1000	---一天多少毫秒
---服务器是否出海
TimeUtil.isOutseasServer = true

---获取服务器时间（秒）
function TimeUtil.GetServerTime() return math.floor(TimeUtil.serverTime) end

---获取服务器时间（豪秒）
function TimeUtil.GetMescTime() return TimeUtil.meseTime end

---获取服务器日期-时间
---@return ETimeDateType
function TimeUtil.GetServerDate() return TimeUtil.GetZoneDate(TimeUtil.GetServerTime()) end

---获取本地时间戳（秒）
---@return number
function TimeUtil.GetLocalTime() return os.time() end

---获取本地日期-时间
---@return ETimeDateType
function TimeUtil.GetLocalDate() return TimeUtil.GetZoneDate( os.time()) end

---获取字符串格式-年月日 时分秒: 邮件，所有活动
---@param timeFormat ETimeFormat
---@param timestamp number 时间戳
---@return string
function TimeUtil.GetStringFormatDate(timestamp, timeFormat)
	---@type ETimeDateType
	local data = TimeUtil.GetZoneDate( timestamp)
	if timeFormat==ETimeFormat.FullTime then
		return string.format('%04d-%02d-%02d %02d:%02d:%02d', data.year, data.month, data.day, data.hour, data.min, data.sec)
	elseif timeFormat==ETimeFormat.ShortTime then
		return string.format('%02d:%02d:%02d', data.hour, data.min, data.sec)
	else
		return string.format('%04d-%02d-%02d', data.year, data.month, data.day)
	end
end

---获取时间字符串:带后缀单位
---@param sumSecond number 总秒数
function TimeUtil.GetTimeStringBySecond(sumSecond)
	local summ, sumh , s = math.floor(sumSecond /60), math.floor(sumSecond /3600), sumSecond %60
	local sumd, m ,h = math.floor(sumh/24), summ%60, sumh%24
	if sumd>0 then return string.format("%02dd %02dh",sumd,h)
	elseif h>0 then return string.format("%02dh %02dm",h,m)
	elseif m>0 then return string.format("%02dm %02ds",m,s)
	else return string.format("0m %02ds",s)
	end
end

---获取时间字符串:无后缀单位
---@param sumSecond number 总秒数
function TimeUtil.GetTimeStringNotUnit(sumSecond)
	local summ, sumh , s = math.floor(sumSecond /60), math.floor(sumSecond /3600), sumSecond %60
	local sumd, m ,h = math.floor(sumh/24), summ%60, sumh%24
	if sumd>0 then return string.format("%02d-%02d:%02dm:%02ds", sumd, h, m, s)
	else string.format("%02dh:%02dm:%02ds", h, m, s)
	end
end

---获取时间字符串
function TimeUtil.GetTimeBySecond(sumSecond)
	local summ, sumh , s = math.floor(sumSecond /60), math.floor(sumSecond /3600), sumSecond %60
	local sumd, m ,h = math.floor(sumh/24), summ%60, sumh%24
	if sumd > 0 then
		return string.format("%02dd:%02dh:%02dm", sumd, h, m)
	end
	return string.format("%02d:%02d:%02d", h, m, s)
end

---是同一天
---@param time1 number 时间戳
---@param time2 number 时间戳
---@return boolean
function TimeUtil.IsSameDay(time1, time2)
	---@type ETimeDateType
	local date1 = TimeUtil.GetZoneDate(time1)
	local date2 = TimeUtil.GetZoneDate(time2)
	if date1.year==date2.year and date1.month==date2.month and date1.day==date2.day then
		return true
	end
	return false
end

---是超过指定的天数
---@param oldTime number 时间戳
---@param newTime number 时间戳
---@param dayNum number 超过的天数
---@return boolean
function TimeUtil.IsDifferentDay(oldTime, newTime, dayNum)
	if oldTime > 0 and newTime > 0 then
		---@type ETimeDateType
		local second = newTime - oldTime
		local costday = second/(3600*24)
		if costday>=dayNum then
			return true
		end
	end
	return false
end

---是超过指定的小时
---@param oldTime number 时间戳
---@param newTime number 时间戳
function TimeUtil.IsOverHour(oldTime, newTime, hour)
	if oldTime > 0 and newTime > 0 then
		---@type ETimeDateType
		local second = newTime - oldTime
		local curSencond = hour*3600
		return second >= curSencond
	end
	return false
end

---@private
---@return number
function TimeUtil.GetDayNumByMonth(year, month)
	if month==2 then
		if year%400 == 0 or (year%4==0 and year%100~=0) then
			return 29
		else
			return 28
		end
	elseif month==4 or month==6 or month==9 or month==11 then
		return 30
	else
		return 31
	end
end

---获取本地未来时间时间戳
---@param days 	number	未来哪天
---@param hours number		未来哪时
---@param minutes number	未来哪分
---@param seconds number	未来哪秒
function TimeUtil.GetFutureTime(days, hours, minutes, seconds)
	local temp_time = os.time() + days * 24 * 60 * 60
	local tempD = TimeUtil.GetZoneDate(temp_time)
	local news_date = { year=tempD.year, month=tempD.month, day=tempD.day, hour=hours, min=minutes, sec=seconds}
	return os.time(news_date)
end

---获取指定的时间错
function TimeUtil.GetWhatTime(year, month, day, hours, minutes, seconds)
	local news_date = { year=year, month=month, day=day, hour=hours, min=minutes, sec=seconds}
	return os.time(news_date)
end

function TimeUtil.GetDateFormat(timestemp)
	local t = TimeUtil.GetZoneDate(timestemp)
	return string.format("%s-%s-%s %s:%s:%s", t.year, t.month,t.day,t.hour,t.min,t.sec )
end

---获取时区日期
---@return ETimeDateType
function TimeUtil.GetZoneDate(timestemp)
	if TimeUtil.isOutseasServer then
		---海外服务器
		return os.date("*t", timestemp)
	else
		---香港服务器
		return os.date("!*t", timestemp)
	end
end

---时间类型 {"sec":28,"min":13,"day":16,"isdst":false,"wday":3,"yday":320,"year":2021,"month":11,"hour":10}
---@class ETimeDateType
ETimeDateType = {
	---几年
	year = 1,
	---几月
	month = 2,
	---几日
	day = 3,
	---星期几
	wday = 4,
	---几点
	hour = 5,
	---分
	min = 6,
	---秒
	sec = 7,
}

---@class ETimeFormat
ETimeFormat = {
	---时分秒
	ShortTime = 0,
	---年月日
	ShortData = 1,
	---年月日-时分秒
	FullTime = 2,
}

return TimeUtil