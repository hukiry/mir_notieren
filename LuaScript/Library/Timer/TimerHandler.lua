---
--- TimerHandler       
--- Author : hukiry     
--- DateTime 2024/8/5 15:49   
---

---@class TimerHandler
local TimerHandler = Class()

function TimerHandler:ctor()
    --监听者
    self.listener = nil

    --执行间隔
    self.delay = 1

    --是否用帧率
    self.useFrame = false

    --执行时间
    self.exeTime = 1

    --执行次数	-1无限循环
    self.loopCount = -1

    --回调函数
    self.method = nil

    --处理方法
    self.callbackFunc = nil
end

function TimerHandler:Clear()
    self.listener = nil
    self.callbackFunc = nil
end

return TimerHandler