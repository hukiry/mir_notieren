---
--- LevelPropsAnimation       
--- Author : hukiry     
--- DateTime 2023/7/30 10:15   
---

---@class LevelPropsAnimation:IUIItem
local LevelPropsAnimation = Class()

function LevelPropsAnimation:ctor()

end

---每次从对象池中拿出来时调用
---@param firstPropsGo UnityEngine.GameObject
---@param target UnityEngine.Transform
---@param slider Hukiry.UI.UIProgressbarMask
---@param startPos UnityEngine.Vector3
function LevelPropsAnimation:Start(firstPropsGo, slider, target, startPos)
    self.startPos = firstPropsGo.transform.position
    self.firstPropsGo =  firstPropsGo
    self.slider = slider
    self.target = target
    self.startPos = startPos
end

--更新数据
function LevelPropsAnimation:OnEnable()
    local propTab = Single.Match().bornPropTab
    self.firstPropsGo:SetActive(#propTab>0)
    if #propTab>0 then
        self.slider.fillAmount = 0
        self.slider:DOFillAmount(1, 1):OnComplete(function()
            self:PlaceToScene(propTab)
        end)
    end
end

---生成物品
function LevelPropsAnimation:PlaceToScene(propTab)
    StartCoroutine(function()
        for y = MatchRule.Size, -MatchRule.Size, -1 do
            local x = -MatchRule.Size
            while x < MatchRule.Size do
                local info = Single.Match():GetMatchInfo(x, y)
                if info and info:IsNormal() then
                    local prop = table.remove(propTab)
                    local horizontal = prop == EPropsType.Rocket and math.random(0,1) or 0
                    self:FlyToScene(prop, x, y, Handle(self, self._PlaceBorn, prop, x, y, horizontal == 1))
                    x = x + Mathf.Random(1,3)
                    WaitForSeconds(0.3)
                    if prop == nil or #propTab==0 then
                        goto FINISH_PLACE;
                    end
                else
                    x = x + 1
                end
            end
        end

        ::FINISH_PLACE::
        WaitForSeconds(1)
        self:OnDisable()
    end)
end

---播放到场景
function LevelPropsAnimation:FlyToScene(prop, x, y, finishCall)
    local iconSpriteName = Single.Match():GetMapConifg():GetPropsInfo(prop).icon
    local pos = Util.Map().IndexCoordToWorld(x, y)
    Single.Animation():PlayItemToScene(self.startPos, pos, GameObject.Find("Game_Home/Map"),
            iconSpriteName, EAnimationFly.ViewToScene, Vector2.New(140,140), finishCall)
end

---播放完成，生成物品
function LevelPropsAnimation:_PlaceBorn(prop, x, y, horizontal)
    EventDispatch:Broadcast(UIEvent.Match_Create_GM, prop, x, y, horizontal)
end

function LevelPropsAnimation:OnDisable()
    self.firstPropsGo.transform:DOMove(self.target.position, 1.2):OnComplete(function()
        self.firstPropsGo:SetActive(false)
        self.firstPropsGo.transform.position = self.startPos
    end)
end

return LevelPropsAnimation