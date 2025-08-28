
---拷贝副本
function Clone(tab)
    local new_table = {}
    for key, value in pairs(tab) do
        new_table[key] = value
    end
    --类型一致
    setmetatable(new_table, getmetatable(tab))
    return new_table
end

---转换字符串
function ToString(v) return v == nil and "" or tostring(v) end

---unity 对象判断为空时可以用下面这个函数。
function IsNil(uobj) return uobj == nil or uobj:Equals(nil) end

---判断字符串对象
function IsEmptyString(v) return v == nil or v == "" end

---判断表是为空
function IsEmptyTable(_table) return _table == nil or  _G.next(_table) == nil end

function reload(script)
    if package.loaded[script] then return package.loaded[script] end
    return require(script)
end

--region 异常捕获
---异常捕获
---@param block table catch,finally
function try(block)
    local main = block[1]
    local catch = block.catch
    local finally = block.finally
    assert(main)
    local ok, errors = xpcall(main, debug.traceback)-- try to call it
    if not ok then if catch then catch(errors) end end -- run the catch function
    if finally then finally(ok, errors) end  -- run the finally function
    if ok then return errors end-- ok?
end

---异常截取
function tryError(funcBody) xpcall(funcBody, function(msg) logError(msg) end) end
--endregion

---@class EClass
EClass = { base = 0 }
local _class ={}
---@return EClass
function Class(base)
    local class_type = {};
    class_type.ctor = false;  -- 是否有构造函数
    class_type.base = base; -- 是否有子类
    class_type.superclass = _class[base]; -- 存储了基类的class，用来在子类function调用父类function时使用
    --模拟构造函数的function
    class_type.New = function(...)
        local obj= {};
        setmetatable(obj, _class[class_type])
        _class[class_type].__index = _class[class_type]
        local create; -- 注意，不能把这里的声明和下一句进行合并！！否则会出现引用的create是一个未定义的全局变量的bug
        create = function(c,...)
            if c.base then create(c.base,...) end
            if c.ctor then obj.base = c.base; c.ctor(obj,...) end
        end
        create(class_type,...)
        return obj
    end

    local temp ={}-- 用一个table来构造类的函数表
    _class[class_type] = temp
    --参数1就是表class_type本身，当添加一个新方法的时候就会执行此__newindex的实现
    setmetatable(class_type,{ __index= temp, __newindex = function(t, k, v) temp[k] = v end })
    --如果本类有子类的话，设置本类的虚表的__index操作，也就是说，找到子类的重载的方法
    if base then
        --这里，就是查找子类的虚表的操作
        local indexFunction = function(t, k)
            local ret=_class[base][k]
            temp[k] = ret
            return ret
        end
        setmetatable(temp,{ __index = indexFunction})
    end
    return class_type
end

function params_arg(tab)
    if tab==nil or #tab == 0 then return nil end
    if #tab == 1 then return tab[1]
    elseif #tab == 2 then return tab[1], tab[2]
    elseif #tab == 3 then return tab[1], tab[2], tab[3]
    elseif #tab == 4 then return tab[1], tab[2], tab[3], tab[4]
    elseif #tab == 5 then return tab[1], tab[2], tab[3], tab[4], tab[5]
    elseif #tab == 6 then return tab[1], tab[2], tab[3], tab[4], tab[5], tab[6]
    elseif #tab == 7 then return tab[1], tab[2], tab[3], tab[4], tab[5], tab[6], tab[7]
    elseif #tab == 8 then return tab[1], tab[2], tab[3], tab[4], tab[5], tab[6], tab[7], tab[8]
    elseif #tab == 9 then return tab[1], tab[2], tab[3], tab[4], tab[5], tab[6], tab[7], tab[8], tab[9]
    elseif #tab == 10 then return tab[1], tab[2], tab[3], tab[4], tab[5], tab[6], tab[7], tab[8], tab[9], tab[10]
    elseif #tab == 11 then return tab[1], tab[2], tab[3], tab[4], tab[5], tab[6], tab[7], tab[8], tab[9], tab[10], tab[11]
    elseif #tab == 12 then return tab[1], tab[2], tab[3], tab[4], tab[5], tab[6], tab[7], tab[8], tab[9], tab[10], tab[11], tab[12]
    elseif #tab == 13 then return tab[1], tab[2], tab[3], tab[4], tab[5], tab[6], tab[7], tab[8], tab[9], tab[10], tab[11], tab[12], tab[13]
    elseif #tab == 14 then return tab[1], tab[2], tab[3], tab[4], tab[5], tab[6], tab[7], tab[8], tab[9], tab[10], tab[11], tab[12], tab[13], tab[14]
    elseif #tab == 15 then return tab[1], tab[2], tab[3], tab[4], tab[5], tab[6], tab[7], tab[8], tab[9], tab[10], tab[11], tab[12], tab[13], tab[14], tab[15] end
end

---兼容最多5个参数传入
---@public
---@param obj table self自己
---@param method function
---@return function
function Handle(obj, method, ...)
    local param1, param2, param3, param4, param5,param6, param7 = params_arg({...})
    return HandleParamsSelf(obj, method, param1, param2, param3, param4, param5, param6, param7)
end

---可变参数
---@private
---@param _self table self自己
---@param method function
---@return function
function HandleParamsSelf(_self, method, param1, param2, param3, param4, param5, param6,param7)
    if param7 ~= nil then
        return function(...) return method(_self, param1, param2, param3, param4, param5, param6, param7, ...) end
    elseif param6 ~= nil then
        return function(...) return method(_self, param1, param2, param3, param4, param5, param6, ...) end
    elseif param5 ~= nil then
        return function(...) return method(_self, param1, param2, param3, param4, param5, ...) end
    elseif param4 ~= nil then
        return function(...) return method(_self, param1, param2, param3, param4, ...) end
    elseif param3 ~= nil then
        return function(...) return method(_self, param1, param2, param3, ...) end
    elseif param2 ~= nil then
        return function(...) return method(_self, param1, param2, ...) end
    elseif param1 ~= nil then
        return function(...) return method(_self, param1, ...) end
    else
        return function(...) return method(_self, ...) end
    end
end

---@public
---无 self 的闭包函数带参
function HandleParams(method, param1, param2, param3, param4, param5)
    if param5 ~= nil then
        return function(...) return method(param1, param2, param3, param4, param5, ...) end
    elseif param4 ~= nil then
        return function(...) return method(param1, param2, param3, param4, ...) end
    elseif param3 ~= nil then
        return function(...) return method(param1, param2, param3, ...) end
    elseif param2 ~= nil then
        return function(...) return method(param1, param2, ...) end
    elseif param1 ~= nil then
        return function(...) return method(param1, ...) end
    else
        return function(...) return method(...) end
    end
end
