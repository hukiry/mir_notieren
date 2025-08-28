---
--- FlyItemBase       
--- Author : hukiry     
--- DateTime 2023/7/30 12:55   
---

---UI和场景动画基类
---@class FlyItemBase:DisplayObjectBase
FlyItemBase = Class(DisplayObjectBase)

function FlyItemBase:ctor(gameObject)
end

---更新数据
---@param startPos UnityEngine.Vector3
---@param targetPos UnityEngine.Vector3
function FlyItemBase:OnEnable(startPos, targetPos)
    self.transform:DOKill()
    self.transform:DOComplete()
    self.transform.position = Vector3.New(startPos.x, startPos.y,0)
    local posList = {}
    local randomX, randomY = Mathf.Random(0.1,1.0), Mathf.Random(0.1,0.5)
    ---@type UnityEngine.Vector3 中心位置点
    local center = (startPos + targetPos) * 0.5
    local distanceHeight, distancewidth = math.abs(targetPos.y - startPos.y), math.abs(targetPos.x - startPos.x)
    local pos1 = Vector3.New(center.x + randomX,center.y-distanceHeight/4 + randomY,0)
    if distancewidth < 1 then
        pos1 = Vector3.New(center.x - randomX,center.y - randomY,0)
    end
    table.insert(posList, pos1)
    table.insert(posList, Vector3.New(targetPos.x,targetPos.y,0))
    self.transform:DOPath(posList, 1, PathType.CatmullRom):SetEase(Ease.OutCubic):OnComplete(Handle(self, self.PlayFinish))
end

---子类重写
function FlyItemBase:Start()
end

---子类重写
function FlyItemBase:PlayFinish()
end

---隐藏窗口:子类重写
function FlyItemBase:OnDisable()
end

function FlyItemBase:OnDestroy()
end