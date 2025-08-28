---
--- 战斗特效
--- Created by hukiry.
--- DateTime: 2020/2/17 17:12
---

---@class EffectItem3D:EffectBase
EffectItem3D = Class(EffectBase)

---@param parentGo UnityEngine.GameObject 父对象
---@param effectName string 特效名字
function EffectItem3D:ctor(parentGo, effectName)

end

--设置档板位置
---@param attackPos UnityEngine.Vector2
---@param targetPos UnityEngine.Vector2
function EffectItem3D:SetEffectBoard(attackPos, targetPos)
    if self.psMount == nil or self.psMount.board.Length == 0 then
        return
    end
    local a = attackPos.x - targetPos.x
    local b = attackPos.y - targetPos.y
    local z = math.sqrt(a * a + b * b) - 0.5

    local count = self.psMount.board.Length;
    for i = 0, count - 1 do
        self.psMount.board[i].transform.localPosition = Vector3.New( 0, 0, z)
    end
end

---设置持续播放时间
---@param duration number 播放时间
function EffectItem3D:WaitPlay(duration, finishPlayCall)
    self.duration = duration
    self.loadPlayCall = finishPlayCall
    self:PlayAsync(nil, function()
        if  self.loadPlayCall then
            self.loadPlayCall()
        end

        if self.gameObject and self.duration>0 then
            self.gameObject.transform:DOScale(Vector3.one, self.duration):OnComplete(function()
                self:OnDisable()
            end)
        end
    end)
end