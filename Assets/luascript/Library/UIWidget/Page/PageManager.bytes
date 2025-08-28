---
--- GamePageView       
--- Author : hukiry     
--- DateTime 2023/6/29 13:41   
---

---@class PageManager
local PageManager = Class()
function PageManager:ctor()
    ---@type table<number, IPageView>
    self.panelList = {}
end

---初始界面:注册按钮事件等
---@param index number 索引页签
---@param classPath string 脚本路径
---@param gameObject UnityEngine.GameObject
function PageManager:Start(index, classPath, gameObject)
    self.panelList[index] = require(classPath).New(gameObject)
    self.panelList[index]:Start()
end

---重置尺寸
---@param homeRawTF UnityEngine.Transform
function PageManager:ResetSize(visibleHeight, homeRawTF, centerIndex)
    for i, v in pairs(self.panelList) do
        v.transform:SetSizeDelta(v.transform.sizeDelta.x, visibleHeight)
        if i == centerIndex then
            local y = 1920
            local vx = v.transform.sizeDelta.x
            if vx>1080  then
                y = vx*1920/1080
                homeRawTF:SetAnchoredPosition(0,200)
            end
            homeRawTF:SetSizeDelta(vx, y)
        end
    end
end
---@param index number 索引页签
function PageManager:OnEnable(index)
    if self.panelList[index] and self.panelList[index].OnEnable then
        self.panelList[index]:OnEnable()
    end
end

---@param index number 索引页签
function PageManager:GetControl(index, moneyType)
    if self.panelList[index] and self.panelList[index].GetControl then
        return self.panelList[index]:GetControl(moneyType)
    end
    return nil
end

function PageManager:OnUpdate()
    for i, v in pairs(self.panelList) do
        if v.OnUpdate then
            v:OnUpdate()
        end
    end
end

function PageManager:ChangeLanguage()
    for i, v in pairs(self.panelList) do
        if v.ChangeLanguage then
            v:ChangeLanguage()
        end
    end
end

---@param index EGamePage
function PageManager:OnDispatch(index, value)
    if self.panelList[index] and self.panelList[index].OnDispatch then
        self.panelList[index]:OnDispatch(value)
    end
end

---隐藏窗口
function PageManager:OnDisable()
    for i, v in pairs(self.panelList) do
        v:OnDisable()
    end
end

return PageManager