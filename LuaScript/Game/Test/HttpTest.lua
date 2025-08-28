---
---
--- Created by Administrator.
--- DateTime: 2024/8/5 20:59
---

local HttpTest = {}

function HttpTest:Start()
    ---@type HttpGet
    local postBinary = require("Network.Http.HttpGet").New()
    local args={
        age = 1,
        name = 'lily',
        height =172,
    }
    postBinary:Get("http://192.168.1.9/game",nil,function(cmd, text)
        log(cmd, text)
    end)
end

return HttpTest