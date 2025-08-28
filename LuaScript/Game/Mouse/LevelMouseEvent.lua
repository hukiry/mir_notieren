---
--- 庄园地图鼠标事件
--- Created huiry
--- DateTime: 2021/4/15 11:45
---

---@class LevelMouseEvent:BaseMouseEvent
LevelMouseEvent = Class(BaseMouseEvent)
---@param go UnityEngine.GameObject 相机对象
function LevelMouseEvent:ctor(go)
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

    self.visibleWidth = 0;
    self.visibleHight = 0;

    ---@type UnityEngine.SpriteRenderer
    self.background = self:FindSpriteRenderer("background")
end

function LevelMouseEvent:OnStart()
    self:CaculateScreen()
end

---初始化地图视角范围
function LevelMouseEvent:CaculateScreen()
    self.camera.orthographicSize = Single.Screen():GetRateSize()
    local rate =  self.camera.orthographicSize/(Screen.height/200)
    UtilFunction.SetScreenAdaption(self.background, Vector3.New(Screen.width*rate, Screen.height*rate))
end

---鼠标按下：仅场景
function LevelMouseEvent:MouseBottonDown()
    self.vx, self.vy = self:GetSelectObj()
    self.isClickDown = true
    --local info = Single.Match():GetMatchInfo(self.vx, self.vy)
    --if info then
    --    self.itemOnlyID = info.onlyID
    --else
    --    self.itemOnlyID = 0
    --end
end

---鼠标移动抬起：UI和场景, 点击抬起无效
function LevelMouseEvent:MouseBottonUp(mousePosition)
end

---鼠标点击抬起：仅场景, 移动无效
function LevelMouseEvent:OnMosueClick(mousePosition)
    EventDispatch:Broadcast(UIEvent.Match_Operate_Click, self.vx, self.vy)
end

---获取选择的物品对象
function LevelMouseEvent:GetSelectObj()
    local worldPos = Util.Camera().ScreenToWorldPoint(self.mousePosition)
    local ix, iy = Util.Map().WorldToIndexCoord(worldPos)
    return ix, iy
end

---相机移动(触摸移动/点击移动)
---@param isPlayer boolean 是否由人为操作
---@param offset UnityEngine.Vector3
function LevelMouseEvent:MoveDistance(offset, isPlayer)
    if offset == Vector3.zero then
        return
    end

    if isPlayer and self.isClickDown  then--and self.itemOnlyID > 0
        local ix, iy = Util.Map().ScreenPositionToIndexCoord(self.mousePosition)
        if ix~=self.vx or iy~=self.vy then
            EventDispatch:Broadcast(UIEvent.Match_Operate_Move, self.vx, self.vy, ix, iy)
            --self.itemOnlyID = 0
            self.isClickDown = false
        end
    end
end

function LevelMouseEvent:OnDisable()
    EventDispatch:UnRegister(self)
    BaseMouseEvent.OnDisable(self)
end

function LevelMouseEvent:OnDestroy()
    self:OnDisable()
end