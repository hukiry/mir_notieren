
---
--- ConfigurationBase       
--- Created by Hukiry 工具自动生成. 
---
---@class ConfigurationBase
ConfigurationBase = Class()

function ConfigurationBase:ctor()
	---子类源数据
    ---@type table<number, {}>
    self.sourceTable={}	
	---解析后的数据
    ---@type table<number, {}>
    self.classTable = {}	
    ---子类
    self.TableItem = nil
end

---@private
---初始化所有配置数据
function ConfigurationBase:_InitAllConfig()
    if table.length(self.sourceTable) > 0 then
        for k, v in pairs(self.sourceTable) do
            self.classTable[k] = self.TableItem.New(v)
            self.sourceTable[k] = nil
        end
    end
end

---获取整张配置表数据
---@return table<number, {}>
function ConfigurationBase:GetTable()
    self:_InitAllConfig()
    return self.classTable;
end

---@private
---@param key number 主键
---@param faultTolerance boolean 默认为true:容错，如果未找到则返回第一条数据，false:不容错
---@return table
function ConfigurationBase:_GetKey(key, faultTolerance)
    if key == nil then
        log(string.format('从TableCurrency获取数据错误,key:%s', key),'pink')
    end

    if self.classTable[key] then
        return self.classTable[key]
    elseif self.sourceTable[key] then
        self.classTable[key] = self.TableItem.New(self.sourceTable[key])
        self.sourceTable[key] = nil
    else
        if faultTolerance == nil or faultTolerance == true then
            log(string.format('从TableCurrency配置表获取Key错误 Key:%s  允许容错处理返回第一条数据', key),'red')
            if table.length(self.classTable) > 0 then
                return table.first(self.classTable)
            elseif table.length(self.sourceTable) > 0 then
                local v, k = table.first(self.sourceTable)
                self.classTable[k] = self.TableItem.New(v)
                self.sourceTable[k] = nil
                return self.classTable[k]
            end
        end
    end
    return self.classTable[key]
end
