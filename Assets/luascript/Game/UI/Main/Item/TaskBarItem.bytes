---
--- TaskBarItem       
--- Author : hukiry     
--- DateTime 2023/7/31 18:51   
---

---@class TaskBarItem:DisplayObjectBase
local TaskBarItem = Class(DisplayObjectBase)

function TaskBarItem:ctor(gameObject)

end

---每次从对象池中拿出来时调用
function TaskBarItem:Start(isTaskView)
    if not isTaskView then
        self:AddClick(self.gameObject, function()
            self.gameObject:SetActive(false)
            UIManager:OpenWindow(ViewID.Task, self.info, self.gameObject)
        end)
    end

    ---任务图标
    self.targetIcon = self:FindAtlasImage("targetIcon")
    self.rewardIcon = self:FindAtlasImage("rewardIcon")
    self.rewardTxt = self:FindHukirySupperText("rewardIcon/txt")
    ---任务进度条
    self.slider = self:FindProgressbarMask("slider")
    self.sliderTxt = self:FindHukirySupperText("sliderTxt")
    ---任务剩余时间
    self.remainTimeTxt =  self:FindHukirySupperText("timeBg/remain")
end

--更新数据
---@param info ETaskData
function TaskBarItem:OnEnable(info)
    self.info = info
    self.rewardIcon.spriteName = SingleConfig.Currency():GetKey(self.info.rewardType).icon

    local itemInfo = SingleConfig.Item():GetKey(self.info.itemId)
    self.targetIcon.spriteName =  string.len(itemInfo.targetIcon)>0 and itemInfo.targetIcon or itemInfo.icon
    self.rewardTxt.text = "x"..self.info.rewardNum
    self.sliderTxt.text = self.info.finishNum .."/".. self.info.itemNum
    self.slider.fillAmount = self.info.finishNum / self.info.itemNum
    self.remainTimeTxt.text = Util.Time().GetTimeStringBySecond(self.info.totalTime)
end

function TaskBarItem:OnUpdate()
    self.remainTimeTxt.text = Util.Time().GetTimeStringBySecond(self.info.totalTime)
end

function TaskBarItem:OnDisable()
    self.targetIcon = nil
    self.rewardIcon = nil
    self.rewardTxt = nil
    ---任务进度条
    self.slider = nil
    self.sliderTxt = nil
    ---任务剩余时间
    self.remainTimeTxt = nil
end

function TaskBarItem:OnDestroy()

end

return TaskBarItem