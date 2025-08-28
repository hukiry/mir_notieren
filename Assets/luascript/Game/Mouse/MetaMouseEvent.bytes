---
--- 庄园地图鼠标事件
--- Created huiry
--- DateTime: 2021/4/15 11:45
---

---@class MetaMouseEvent:BaseMouseEvent
MetaMouseEvent = Class(BaseMouseEvent)
---@param go UnityEngine.GameObject 相机对象
function MetaMouseEvent:ctor(go)
    ---@type UnityEngine.Camera
    self.camera = self.transform:GetComponent("Camera")
    self.cameraTf = self.camera.transform
    self.cameraPositionZ = self.cameraTf.position.z
    self.scaleMin = 5.4   --最小缩放 5.4
    self.scaleMax = 13    --最大缩放
    self.scaleCur = 12     --初始缩放
    self.scaleSpeed = 3     --拉近拉远的速度
    ---@type number
    self.scaleAmortize = self.scaleMin  ---缩放缓冲值限制
    ---aspect = w/h
    self.cameraAspect = self.camera.aspect

    ---@type number 物品唯一id
    self.itemOnlyID, self.vx, self.vy = 0, 0 ,0

    ---305,225
    self.scaleHeight = SCALE_HEIGHT
    if UnityEngine.Screen.height/UnityEngine.Screen.width>0.6 then
        SCALE_HEIGHT = UnityEngine.Screen.height/2
    end
    self.scaleRate = 1

    self.visibleWidth = 0
    self.visibleHight = 0

    ---@type UnityEngine.SpriteRenderer
    self.background = self:FindSpriteRenderer("background")
end

function MetaMouseEvent:OnStart()
    self:CaculateScreen()
end

function MetaMouseEvent:CaculateScreen()
    self.camera.orthographicSize = self:GetMaxScale()
    local rate =  self.camera.orthographicSize/(Screen.height/200)
    UtilFunction.SetScreenAdaption(self.background, Vector3.New(Screen.width*rate, Screen.height*rate))
end

---鼠标按下：仅场景
function MetaMouseEvent:MouseBottonDown()
    self.vx, self.vy = self:GetSelectObj()
end

---鼠标移动抬起：UI和场景, 点击抬起无效
function MetaMouseEvent:MouseBottonUp(mousePosition)
end

---鼠标点击抬起：仅场景, 移动无效
function MetaMouseEvent:OnMosueClick(mousePosition)
    ---点击UI返回
    if self.isTouchOverUI then
        return
    end

    --点击的坐标
    if self:IsRange(self.vx, self.vy) then
        EventDispatch:Broadcast(UIEvent.Meta_Operate_Click, self.vx, self.vy)
    end
end

---获取选择的物品对象
function MetaMouseEvent:GetSelectObj()
    local worldPos = Util.Camera().ScreenToWorldPoint(self.mousePosition)
    local ix, iy = Util.Map().WorldToIndexCoord(worldPos)
    return ix, iy
end

---相机移动(触摸移动/点击移动)
---@param isPlayer boolean 是否由人为操作
---@param offset UnityEngine.Vector3
function MetaMouseEvent:MoveDistance(offset, isPlayer)
    if offset == Vector3.zero then
        return
    end

    if isPlayer then
        local ix, iy = Util.Map().ScreenPositionToIndexCoord(self.mousePosition)
        if self:IsRange(ix, iy) then
            if ix ~= self.vx or iy ~= self.vy then
                --移动到新位置的坐标
                EventDispatch:Broadcast(UIEvent.Meta_Operate_Move, ix, iy)
                self.vx, self.vy = ix, iy
            end
        end
    end
end

---坐标在地图内
---@return boolean
function MetaMouseEvent:IsRange(x, y)
    if x > MatchRule.Size or x < -MatchRule.Size then
        return false
    end

    if y > MatchRule.Size or y < -MatchRule.Size then
        return false
    end
    return true
end

---@return number 获取最大缩放
function MetaMouseEvent:GetMaxScale()
    ---设备分辨率
    local screenScale =  Screen.height/Screen.width
    ---图片分辨率 1080/1920 = 0.5625
    local textureScale, default_Rate = 1.2, 1.778
    local result = screenScale>default_Rate and (screenScale-default_Rate)*3 or -(default_Rate-screenScale)-screenScale
    ---设备>图片?图片:设备
    local inputRate = screenScale > textureScale and textureScale or screenScale
    ---适配大多数分辨率计算 2.2:1 ~~ 0.0225
    local max = inputRate * 10 + screenScale + result

    return max
end

function MetaMouseEvent:OnDisable()
    EventDispatch:UnRegister(self)
    BaseMouseEvent.OnDisable(self)
end

function MetaMouseEvent:OnDestroy()
    self:OnDisable()
end