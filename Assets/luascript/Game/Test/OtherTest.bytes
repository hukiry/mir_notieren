---
--- OtherTest       
--- Author : hukiry     
--- DateTime 2024/8/5 21:01   
---

local OtherTest = {}

function OtherTest:Start()
    local params = ''
    local len = 16
    for i = 1, len do
        if i==1 then
            params = params .. '\nif #tab == ' .. i .. ' then return '
        else
            params = params .. '\nelseif #tab == ' .. i .. ' then return '
        end

        for k = 1, i-1 do
            params = params .. 'tab['.. k ..'], '
        end
        params = params .. 'tab['.. i ..']'
        params = i==len and params .. ' end' or params
    end
    log(params)
end

function OtherTest:Time()
    local t = os.time()
    ---@type ETimeDateType
    local t1 = os.date("*t", t)
    local t2 = os.date("!*t", t)
    log("*t---", t1.hour)
    log("!*t---", t2.hour)
end

function OtherTest:String()
    local t = "asdfg你好！中文asdfg你好！中文"

    log(string.find(t,"好"), utf8.contains(t, 0,"g你"), "yellow")
end

---@param transform UnityEngine.Transform
function OtherTest:Loop(transform)
    transform:DOLocalMove(Vector3.zero, 1):SetLoops(-1, LoopType.Yoyo);
end

function OtherTest:File()
    local tab = {
       t = "asdfg你好！中文asdfg你好！中文",
       age=18,
       sex=true,
    }
    Single.Binary():SaveText("test",json.encode(tab))
    log("保存完成：","green")
    Single.Binary():ReadText("test", function(str)
        local result =  json.decode(str)
        log(result.t, result.age, result.sex,"yellow")
    end)
end

---@param test Hukiry.HukirySupperText
function OtherTest:Event(test)
    test.OnHrefHandler = test.OnHrefHandler+function(id, tag)
        log(id, tag,"yellow")
    end

end

function OtherTest:TimerTest()
    self.id = 5656
    Single.TimerManger():DoTime(self, Handle(self, self.UnloadImmediate, "assetName", true), 1, 1)
end

function OtherTest:UnloadImmediate(assetName, force, _self)
    log(assetName, force, _self.id,"yellow")
end


function OtherTest:TableTest()
    local temp = {}
    table.insert(temp,  "a")
    table.insert(temp,  "b")
    table.insert(temp,  "c")
    table.insert(temp,  "d")

    local index = #temp
    while(index>0) do
        log("remove ",index,table.remove(temp, index))
        index = index -1
    end
end

return OtherTest