---
--- 副本鼠标事件
--- Created by Administrator.
--- DateTime: 2021/4/29 17:53
---

local manualHeight = 1080
local manualWidth = 1920

local VISBLE_WIDTH = (manualWidth/2)/100;
local VISBLE_HEIGHT = (manualHeight/2)/100;

---@class DungeonMouseEvent:BaseMouseEvent
DungeonMouseEvent = Class(BaseMouseEvent)

function DungeonMouseEvent:ctor(go)
    ---@type UnityEngine.Camera
    self.camera = self.transform:GetComponent("Camera")
    self.cameraTf = self.camera.transform
    self.cameraPositionZ = self.cameraTf.position.z

    self.scaleMax = self:GetMaxScale()    --最大缩放

    Single.DungeonMgr().dungeonScale = 1
    local scale_rate = SingleConfig.GlobalValue():GetKey(26).value/100
    self.scaleMin = self.scaleMax / scale_rate  --最小缩放
    self.scaleCur = self.scaleMax    --初始缩放
    self.scaleSpeed = 2     --拉近拉远的速度
    self.cameraAspect = self.camera.aspect
    self:RegisterEvent()
end

function DungeonMouseEvent:RegisterEvent()
    EventDispatch:Register(self , UIEvent.DungeonRevertCameraScale , "RevertScaleCamera")
end

--鼠标按下
function DungeonMouseEvent:MouseBottonDown()

end

--鼠标抬起
function DungeonMouseEvent:MouseBottonUp()
end

--鼠标有效点击
function DungeonMouseEvent:OnMosueClick()
    ---@type UnityEngine
    local hit2d = Physics2D.Raycast( self.camera:ScreenToWorldPoint(self.mousePosition), Vector2.zero);
    if hit2d and hit2d.transform then
        local tag = hit2d.transform.tag
        --print(tag, hit2d.transform.name ,"red")
        if tag == Tag_Type.Type_Item then   --副本/建筑
            EventDispatch:Broadcast(UIEvent.Dungeon_Mouse_Event_Select, tonumber(hit2d.transform.name), hit2d.point)
        else
            EventDispatch:Broadcast(UIEvent.Dungeon_Mouse_Event_Select, -1, hit2d.point)
        end
    end
end

-------------------------------[[ 还原计时器相关 begin]]--------------------------------
local timeGap = 0.05
local sumTime = 0
local runTime = 0
local scaleCur = 0
local duration = 0
local subScale = 0
local callback = nil

---@deprecated 还原场景
function DungeonMouseEvent:RevertScaleCamera(_duration , _callback)
    duration = _duration
    callback = _callback

    runTime = 0
    sumTime = 0

    scaleCur = self.scaleCur
    subScale = (scaleCur - self.scaleMax)/(duration/timeGap)


    if Single.DungeonMgr().dungeonScale == 1 then
        if callback then
            callback()
            callback = nil
        end
    else
        Single.TimerManagerNew():DoTime(self , self.revertLogic , timeGap, -1)
    end
end

---@deprecated 还原逻辑
function DungeonMouseEvent:revertLogic()
    runTime = runTime + 1
    sumTime = runTime * timeGap

    local scale = scaleCur - (subScale * runTime)
    if scale <= self.scaleMax then
        self:OnScaleCamera(scale)
    end

    if sumTime == duration + 0.3 then
        if callback then
            callback()
            callback = nil
        end
        Single.TimerManagerNew():RemoveHandler(self , self.revertLogic)
    end
end
--------------------------------[[ 还原计时器相关 end ]]--------------------------------------------

---相机缩放时
function DungeonMouseEvent:OnScaleCamera(scale)

    self.camera.orthographicSize = scale
    --self.scaleDiff = (scale - 5.4) * self.cameraAspect
    self.sizeWidth = scale * self.cameraAspect  ---水平
    self.sizeHigth = scale ---垂直

    Single.DungeonMgr().dungeonScale =   self.scaleMax/scale

    --local offset = self:GetBorderOffset(Vector3.zero)
    --CSVectorHelper.AddLocalPosition(self.gameObject, offset.x, offset.y, offset.z)
end

--相机移动(触摸移动/点击移动)
function DungeonMouseEvent:MoveDistance(offset)
    if offset == Vector3.zero then
        return
    end

    --如果玩家操作了移动，去除相机移动到目标的操作
    self.targetPos = Vector3.zero
    self:MoveMap(offset)
end

--function DungeonMouseEvent:MoveMap(offset)
--    offset = Utils.Camera().ScreenProportionToUGUIProportion(offset)
--    offset = offset * (self.scaleCur / 540)
--    offset = self:GetBorderOffset(offset)
--    --CSVectorHelper.AddLocalPosition(self.gameObject, offset.x, offset.y, offset.z)
--end

---相机定位到某位置
function DungeonMouseEvent:MoveToTargetPos()
    if self.targetPos == Vector3.zero then
        return
    end

    local worldPos = Vector3.Lerp(self.cameraTf.position, self.targetPos, Time.deltaTime * 6)
    self.cameraTf.transform.localPosition = worldPos
    if Vector3.Distance(self.cameraTf.position, self.targetPos) < 0.01 then
        self.targetPos = Vector3.zero
    end
end

--限定相机位置，不可拖出视图外
function DungeonMouseEvent:GetBorderOffset(pDistance)
    local temp = self.localPosition
    temp.z = 0
    local pos = self.localPosition + pDistance
    ---总大小-相机的尺寸
    local x = Mathf.Clamp(pos.x, -(VISBLE_WIDTH - self.sizeWidth), (VISBLE_WIDTH - self.sizeWidth))
    local y = Mathf.Clamp(pos.y, -(VISBLE_HEIGHT - self.sizeHigth), (VISBLE_HEIGHT - self.sizeHigth))
    return Vector3.New(x, y, 0) - self.localPosition
end

---@return number 获取最大缩放
function DungeonMouseEvent:GetMaxScale()
    ---设备分辨率
    local screenScale = Screen.height / Screen.width
    ---图片分辨率 1080/1920 = 0.5625
    local manualScale = manualHeight / manualWidth
    ---设备>图片?图片:设备
    local inputRate = screenScale > manualScale and manualScale or screenScale
    ---适配大多数分辨率计算 2.2:1 ~~ 0.0225
    local max = (inputRate - 0.0225) * 10

    return max
end

function DungeonMouseEvent:OnDisable()
    EventDispatch:Register(self , UIEvent.DungeonRevertCameraScale)
    BaseMouseEvent.OnDisable(self)
end

function DungeonMouseEvent:OnDestroy()
    BaseMouseEvent.OnDestroy(self)
    self:UnRegisterEvent()
end

return DungeonMouseEvent