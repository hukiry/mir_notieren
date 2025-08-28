---
---
--- Create Time:2021-8-8 15:33
--- Author: Hukiry
---

---@class LoadingView:UIWindowBase
local LoadingView = Class(UIWindowBase)

function LoadingView:ctor()
	---@type LoadingControl
	self.ViewCtrl = nil
end

---初始属性字段
function LoadingView:Awake()
	self.prefabName = "Loading"
	self.panelLayer = ViewLayer.ProgressLoad
	self.panelBgMode = ViewBackgoundMode.None
	self.asynLoad = false
end

---初始界面:注册按钮事件等
function LoadingView:Start()
	self:ReBackSize()
end

---事件派发
function LoadingView:OnDispatch(...)

end

---显示窗口:初次打开
---@param steps table<number, SceneProgressBase>
---@param backCall function
function LoadingView:OnEnable(steps, isHand, backCall)
	self:ReBackSize()
	if backCall then
		isHand = Mathf.Clamp(isHand, 0,1)
		self.ViewCtrl.sliderFore.fillAmount = isHand
		self.ViewCtrl.percentTxt.text = string.format("%.2f", isHand * 100) .. "%"
		backCall()
		return
	end
	self.isHand = isHand
	self.totalStep = #steps
	self.curStep = 0
	self.steps = steps

	self.lastProgress = 0
	self:NextStep()
	self.ViewCtrl.sliderFore.fillAmount = 0
	Single.TimerManger():DoFrame(self, self.OnProgress, 1, -1)
end

function LoadingView:ReBackSize()
	local y = 1920
	local vx = self.transform.rect.width
	if vx>1080  then
		y = vx*1920/1080
	end
	self.ViewCtrl.backTF:SetSizeDelta(vx, y)
end

---显示中重复打开
function LoadingView:OnRefresh(steps, endAnimation)
	self:OnEnable(steps, endAnimation)
end

---开始下一个阶段的加载
function LoadingView:NextStep()
	if self.curStep >= self.totalStep then	--表示加载完成，等待外部调用关闭加载界面
		return
	end
	self.curStep = self.curStep + 1
	self.progressBase = self.steps[self.curStep]
	self.progressBase:Start()
end

function LoadingView:OnProgress()
	local maxProgress, bufferMultiple = self:GetTotalProgress()
	local _progress = Mathf.Lerp(self.lastProgress, maxProgress, Time.deltaTime * bufferMultiple)--缓冲效果
	local _progress2 = math.max(_progress, self.lastProgress + 0.01)--单次最少推进0.01
	local curProgress = math.min(_progress2, maxProgress)

	curProgress = Mathf.Clamp(curProgress, 0,1)
	self.ViewCtrl.sliderFore.fillAmount = curProgress
	self.ViewCtrl.percentTxt.text = string.format("%.2f", curProgress*100) .. "%"
	self.lastProgress = curProgress

	if curProgress >= 1 and self.curStep >= self.totalStep and self.progressBase:IsDone() then
		self:OnSceneLoadFinish()
	end

	if self.progressBase:IsDone() then --走到下一阶段
		self:NextStep()
	end
end

function LoadingView:GetTotalProgress()
	local stepProgress = self.progressBase:GetProgress()
	local _progress = 0
	for k, v in ipairs(self.steps) do
		if k < self.curStep then
			_progress = _progress + v.proportion
		end
	end
	if self.curStep <= self.totalStep then
		_progress = _progress + stepProgress * self.progressBase.proportion
	end

	--精度问题容错处理，_progress永远不会等于1，一直处在0.999999999999999
	if _progress > 0.99 then
		_progress = 1
	end

	--缓冲倍数
	local bufferMultiple = 0.2
	if _progress >= 1 then
		bufferMultiple = 10
	elseif _progress > 0.9 then
		bufferMultiple = 5
	else
		bufferMultiple = 2
	end
	return _progress, bufferMultiple
end

--场景加载完成:关闭进度条
function LoadingView:OnSceneLoadFinish()
	Single.TimerManger():RemoveHandler(self,self.OnProgress)
	SceneApplication.OnLevelInitFinish()
	if not self.isHand then
		self:Close()
	end
end

---隐藏窗口
function LoadingView:OnDisable()
	
end

---消耗释放资源
function LoadingView:OnDestroy()
	
end

return LoadingView