---
--- 工具管理
--- Created by hukiry.
--- DateTime: 2021/8/5 22:50
---

---@class Util
Util = {}
---@type table<string,table>
local m_list = {}
---@private
---@param className string
---@param classPath string
function Util.ImportClass(className, classPath)
    if m_list[className] == nil then
        m_list[className] = require(classPath)
    end
    return m_list[className]
end

---@return MathUtil
function Util.Math()
    return Util.ImportClass("MathUtil",'Library.Utility.MathUtil')
end

---@return NumberUtil
function Util.Number()
    return Util.ImportClass("NumberUtil",'Library.Utility.NumberUtil')
end

---@return TimeUtil
function Util.Time()
    return Util.ImportClass("TimeUtil",'Library.Utility.TimeUtil')
end

---@return UIWindowUtil
function Util.UIWindow()
    return Util.ImportClass("UIWindowUtil",'Library.Utility.UIWindowUtil')
end

---@return MaterialUtil
function Util.Material()
    return  Util.ImportClass("MaterialUtil",'Library.Utility.MaterialUtil')
end

---@return StringUtil
function Util.String()
    return  Util.ImportClass("StringUtil",'Library.Utility.StringUtil')
end

---@return CameraUtil
function Util.Camera()
    return  Util.ImportClass("CameraUtil",'Library.Utility.CameraUtil')
end

---@return MapUtil
function Util.Map()
    return  Util.ImportClass("MapUtil",'Library.Utility.MapUtil')
end