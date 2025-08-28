---
--- 鼠标事件基类
--- Created huiry
--- DateTime: 2021/4/15 11:45
---

---@class BaseMouseEvent : DisplayObjectBase
BaseMouseEvent = Class(DisplayObjectBase)

function BaseMouseEvent:ctor(pGo)
    self.clickPosition = Vector2.zero       --点击移动(点击屏幕的点)
    self.touchPositions = nil               --触摸移动(触摸屏幕的点)

    self.scaleTouchPositions = { nil, nil } --触摸scale远近(两个手指的屏幕点)
    self.oldTouchVector = nil
    self.oldTouchDistance = nil

    self.scaleSpeed = 2             --拉近拉远的速度
    self.deviation = 5              --鼠标按下松手之间的距离误差，小于这个值属于点击

    self.scaleMin = 0.8    --最小缩放
    self.scaleMax = 1.2    --最大缩放
    self.scaleCur = 1     --初始缩放

    ---@type number
    self.scaleAmortize = self.scaleMin - 0.2 ---缩放缓冲最小值

    self.targetPos = Vector3.zero

    self.mousePosition = Input.mousePosition
    self.localPosition = self.transform.localPosition

    self.clickDownTime = 0  --鼠标按下的时间
    self.isTouchOverUI = false

    self.clickDoubleTime = 0 ---双击时间间隔
    --self.clickDoubleCount = 0 --双击次数
    --self.isStartClick = false
    self.isSceneUI = false
end

-- 每次显示窗口时
function BaseMouseEvent:OnEnable()
    Single.TimerManger():DoFrame(self, self.Update, 1, -1)
    self:OnScaleCamera(self.scaleCur)
end

function BaseMouseEvent:Update()
    self.mousePosition = Input.mousePosition
    self.localPosition = self.transform.localPosition

    self:MoveToTargetPos()  --相机移动到目标

    self.frameIsOverUI = RootCanvas.Instance:IsTouchAllUI()--IsTouchMainUI()


    if self.targetPos == Vector3.zero and self.clickDownPos == nil then --如果地图还在缓动中
        self:_MoveDistance(Vector3.zero, false)
    end

    if Input.GetMouseButtonDown(0) then self:OnMouseButtonDown() end
    if Input.GetMouseButton(0) then self:OnMouseButton() end

    if GameSymbols:IsMobile() then
        if not self.isTouchOverSceneUI then
            self:TouchMove()
        end
    else
        if not self.isTouchOverSceneUI then
            self:OnDragMove()
        end
        self:ScrollScaleCamera()
    end

    if Input.GetMouseButtonUp(0) then
        self:OnMouseButtonUp()
    end
end

--鼠标按住
function BaseMouseEvent:OnMouseButton()
    if self.isTouchOverSceneUI then

    end
end

--鼠标按下
function BaseMouseEvent:OnMouseButtonDown()
    self.clickDownTime = Time.realtimeSinceStartup
    self.clickDownPos = self.mousePosition
    self.isTouchOverSceneUI = self.frameIsOverUI
    self.isTouchOverUI = RootCanvas.Instance:IsTouchAllUI()
    self.isSceneUI = self.isTouchOverUI

    --if not self.isStartClick then
    --    self.clickDoubleCount = 0
    --    self.clickDoubleTime  = Time.realtimeSinceStartup
    --end

    if not self.isTouchOverSceneUI then
        self:MouseBottonDown(self.mousePosition)
    end
end

--鼠标抬起
function BaseMouseEvent:OnMouseButtonUp()
    if self.clickDownPos == nil then return end
    ---距离在指定范围内，属于点击；否则，是拖拽
    if Vector2.Distance(self.clickDownPos, self.mousePosition) < self.deviation then
        if not self.isTouchOverSceneUI then ---场景点击
            self.isSceneUI = self.isTouchOverUI
            self:OnMosueClick(self.mousePosition)
        end
    else
        self:MouseBottonUp(self.mousePosition)
    end

    self.isTouchOverUI = false;
    self.isTouchOverSceneUI = false
    self.clickDownPos = nil
end

--鼠标按下
function BaseMouseEvent:MouseBottonDown(mp)

end

--鼠标抬起
function BaseMouseEvent:MouseBottonUp(mp)

end

--鼠标有效点击
function BaseMouseEvent:OnMosueClick(mp, isDouble)

end

---非移动平台移动
function BaseMouseEvent:OnDragMove()
    if Input.GetMouseButton(0) then
        if self.clickPosition == Vector2.zero then
            self.clickPosition = self.mousePosition
        else
            local newClickPosition = self.mousePosition
            self:_MoveDistance(self.clickPosition - newClickPosition, true)
            self.clickPosition = newClickPosition
        end
    else
        self.clickPosition = Vector2.zero
    end
end

--触摸移动和缩放摄像机 (移动平台)
function BaseMouseEvent:TouchMove()
    if Input.touchCount == 0 then ---无手指
        self.scaleTouchPositions[1] = nil
        self.scaleTouchPositions[2] = nil
        self.touchPositions = nil
    elseif Input.touchCount == 1 then ---一个手指移动
        if self.touchPositions == nil then
            self.touchPositions = Input.GetTouch(0).position
        else
            local newTouchPosition = Input.GetTouch(0).position
            self:_MoveDistance(Vector3.New(self.touchPositions.x - newTouchPosition.x, self.touchPositions.y - newTouchPosition.y, 0), true)
            self.touchPositions = newTouchPosition
        end
    elseif Input.touchCount == 2 then ---两个手指拉伸
        self.touchPositions = nil
        if self.scaleTouchPositions[2] == nil then
            self.scaleTouchPositions[1] = Input.GetTouch(0).position
            self.scaleTouchPositions[2] = Input.GetTouch(1).position
            self.oldTouchVector = self.scaleTouchPositions[1] - self.scaleTouchPositions[2]
            self.oldTouchDistance = self.oldTouchVector.magnitude
        else
            local newTouchPositions = { Input.GetTouch(0).position, Input.GetTouch(1).position }
            local newTouchVector = newTouchPositions[1] - newTouchPositions[2]
            local newTouchDistance = newTouchVector.magnitude
            self:ScaleCamera((newTouchDistance - self.oldTouchDistance) * 0.003)
            self.scaleTouchPositions[1] = newTouchPositions[1]
            self.scaleTouchPositions[2] = newTouchPositions[2]
            self.oldTouchVector = newTouchVector
            self.oldTouchDistance = newTouchDistance
        end
    end
end


--非移动平台拉远拉近摄像机 滚轮滚动缩放
function BaseMouseEvent:ScrollScaleCamera()
    local axisScroll = Input.GetAxis("Mouse ScrollWheel")
    if axisScroll ~= nil and axisScroll ~= 0 then
        self:ScaleCamera(axisScroll)
    end
end

---相机拉远拉近，缩放
function BaseMouseEvent:ScaleCamera(scale)
    local scaleDelta = scale * self.scaleSpeed
    self.scaleCur = self.scaleCur - scaleDelta
    ---场景缩放缓冲 scaleAmortize
    self.scaleCur = Mathf.Clamp(self.scaleCur, self.scaleAmortize, self.scaleMax)
    self:OnScaleCamera(self.scaleCur)
end

function BaseMouseEvent:OnScaleCamera(newScale)

end

function BaseMouseEvent:SetLocalPosition(pos)
end

--移动到目标
function BaseMouseEvent:MoveToTargetPos()
end

---相机移动(触摸移动/点击移动)
---@param pDistance Vector3 屏幕这一帧和上一帧的距离
---@param isPlayer boolean 是否由人为操作,其他移动属于定位移动
function BaseMouseEvent:_MoveDistance(pDistance, isPlayer)
    self:MoveDistance(pDistance/self.camera.aspect, isPlayer)
end

--相机移动(触摸移动/点击移动)
---@param pDistance Vector2 屏幕这一帧和上一帧的距离
function BaseMouseEvent:MoveDistance(pDistance, isPlayer)
end

function BaseMouseEvent:IsOverUI()
    if self.frameIsOverUI then
        return true
    end
    return false
end

-- 隐藏窗口时
function BaseMouseEvent:OnDisable()
    Single.TimerManger():RemoveHandler(self)
end

-- 界面销毁前调用
function BaseMouseEvent:OnDestroy()
end