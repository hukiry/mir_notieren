---
--- ScreenIPhoneSize       
--- Author : hukiry     
--- DateTime 2024/7/11 15:14   
---

---@class ScreenIPhoneSize
local ScreenIPhoneSize = {}

function ScreenIPhoneSize:InitData()
    self.screenTab = {
        { w=1334 , h= 750	},--1.778666667
        { w=2208 , h= 1242	},--1.777777778
        { w=2436 , h= 1125	},--2.165333333
        { w=2688 , h= 1242	},--2.164251208
        { w=1792 , h= 828	},--2.164251208
        { w=2340 , h= 1080	},--2.166666667
        { w=2532 , h= 1170	},--2.164102564
        { w=2778 , h= 1284	},--2.163551402
        { w=2556 , h= 1179	},--2.167938931
        { w=2796 , h= 1290	},--2.16744186

        { w=2752,	h=2064	},--1.333333333
        { w=2420,	h=1668	},--1.450839329
        { w=2732,	h=2048	},--1.333984375
        { w=2388,	h=1668	},--1.431654676
        { w=2224,	h=1688	},--1.317535545
        { w=2048,	h=1536	},--1.333333333
        { w=2360,	h=1640	},--1.43902439
        { w=2266,	h=1488	},--1.522849462
        { w=2160,	h=1620	}--1.333333333
    }

    ---比率
    self.rateTab = {
        {rate = 1.78, size = 11.4, y=0.7},
        {rate = 2.16, size = 14, y=0.7},
        {rate = 2.17, size = 14, y=0.7},
        {rate = 1.31, size = 10.8, y=0.7},
        {rate = 1.32, size = 10.8, y=0.7},
        {rate = 1.33, size = 10.8, y=0.7},
        {rate = 1.34, size = 10.8, y=0.7},
        {rate = 1.43, size = 10.9, y=0.7},
        {rate = 1.44, size = 10.9, y=0.7},
        {rate = 1.45, size = 10.9, y=0.7},
        {rate = 1.52, size = 10.9, y=0.7},

        ---Android
        --{rate = 1.42, size = 10.9, y=0.7},
        --{rate = 1.5, size = 10.9, y=0.7},
        --{rate = 1.6, size = 11.3, y=0.7},
        --{rate = 2.05, size = 12, y=0.7},
    }
end

---@return number
function ScreenIPhoneSize:GetRateSize()
    local rate = UnityEngine.Screen.height/UnityEngine.Screen.width
    local rateShort = tonumber(string.format("%0.2f", rate))
    for _, v in ipairs(self.rateTab) do
        if v.rate == rateShort then
            return v.size
        end
    end
    return self:GetDefaultScaleSize()
end

---@private
---@return number 获取最大缩放
function ScreenIPhoneSize:GetDefaultScaleSize()
    ---设备分辨率
    local screenScale =  Screen.height/Screen.width
    ---图片分辨率 1080/1920 = 0.5625
    local textureScale, default_Rate = 1.2, 1.778
    local result = screenScale>default_Rate and (screenScale-default_Rate)*3 or -(default_Rate-screenScale)-screenScale
    ---设备>图片?图片:设备
    local inputRate = screenScale > textureScale and textureScale or screenScale
    ---适配大多数分辨率计算 2.2:1 ~~ 0.0225
    local max = inputRate * 10 + screenScale + result

    return max
end

---@private
---@return number 获取最大缩放
function ScreenIPhoneSize:GetCurrentScaleSize(minRate, maxRate,  minSize, maxSize)
    ---设备分辨率
    local default_Rate = 0.5
    local curRate =  Screen.height/Screen.width
    local perRate = (maxRate-minRate)/(maxSize-minSize)
    local result = (curRate-minRate)*perRate+minSize+default_Rate
    return result
end

return ScreenIPhoneSize