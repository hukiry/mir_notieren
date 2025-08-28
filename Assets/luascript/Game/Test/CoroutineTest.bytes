---
--- CoroutineTest       
--- Author : hukiry     
--- DateTime 2024/8/5 20:54   
---

local CoroutineTest = {}

function CoroutineTest:Start()
    self.name = "testCoroutine"
    ---开始协程
    self.coroutine = StartCoroutine(Handle(self, self.TestCoroutine, 72))

end

function CoroutineTest:TestCoroutine(r)

    log("name:",self.name,r )
    local req = UnityWebRequest.Get('http://www.baidu.com')
    Yield(req:SendWebRequest()) --测试 UnityWebRequest

    Yield(1) ---等待0秒
    local s = tolua.tolstring(req.downloadHandler.data)--将字节转换为字符串
    WaitForSeconds(1) ---等待1秒
    WaitForEndOfFrame()---等待结束帧
    WaitForFixedUpdate()---等待一帧
    log(s)
    self:Stop()
    WaitForSeconds(2)
    log("----------------------name:",self.name)

end

function CoroutineTest:Stop()
    ---停止协程
    StopCoroutine(self.coroutine)
end

function CoroutineTest:Start2()
    local luaCo =coroutine.create(function(param)
        log("-----1", param,"yellow")
        coroutine.yield(1)
        log("-----2")
        return 10
    end)

    local ok, result = coroutine.resume(luaCo, "param")
    coroutine.running(luaCo)
    log("result=",ok, result)
end



return CoroutineTest