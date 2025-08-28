---
---
--- Create Time:2023-03-10
--- Author: Hukiry
---

---@class GuideView:UIWindowBase
local GuideView = Class(UIWindowBase)

function GuideView:ctor()
	---@type GuideControl
	self.ViewCtrl = nil
end

---初始属性字段
function GuideView:Awake()
	self.prefabName = "Guide"
	self.prefabDirName = "Guide"
	---@type boolean
	self.isEnableTimer = true
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = -1

	---@type ViewLayer
	self.panelLayer = ViewLayer.Guide
end

---初始界面:注册按钮事件等
function GuideView:Start()
	self:AddClick(self.ViewCtrl.maskGo, Handle(self, self.OnTouchNext, false))
	self:AddClick(self.ViewCtrl.shaderMashGo, Handle(self, self.OnTouchNext, true))
	self.stepIndex = 0


	--if self.aniSetGo == nil then
	--	---@type UnityEngine.GameObject
	--	local original = self.btnList[EPropsView.None].gameObject
	--	self.aniSetGo = GameObject.Instantiate(original, original.transform.position, Quaternion.identity)
	--	self.aniSetGo.transform:SetParent(self.ViewCtrl.settingPanelGo.transform, false)
	--	self.aniSetGo.transform:SetAsFirstSibling()
	--	self:AddClick(self.aniSetGo,function()
	--		UIEventListener.ExecuteEvent(self.btnList[EPropsView.None].gameObject, EventTriggerType.PointerClick)
	--	end)
	--end

	if self.materialItem == nil then
		---@type UnityEngine.UI.Image
		local image = self.ViewCtrl.shaderMashGo:GetComponent("Image")
		---@type UnityEngine.Material
		self.materialItem = Material.New(Shader.Find("Hukiry/Mask"))
		image.material = self.materialItem
	end
end

function GuideView:OnTouchNext(isGoto)
	if not self.isCanClick then
		return
	end

	if isGoto then
		local goHandle = SingleData.Guide():GetClickHandler(self.stepIndex)
		if goHandle then
			self:ExecuteClickHandle(goHandle)
		end
	end

	---点击执行下一步
	if self.finishTouchCall then
		self:Close()
		self.finishTouchCall()
	end
end

---启动计时器后，调用此方法
function GuideView:OnTimer()
	if self.delayTime  > 0 then
		self.delayTime = self.delayTime - 1
		if self.delayTime <= 0 then
			---显示按钮
			self.ViewCtrl.skipBtnGo:SetActive(self.info:IsShowSkip())
		end
	end
end

---显示窗口:初次打开
---@param info GuideInfo
---@param finishTouchCall function
function GuideView:OnEnable(stepIndex, info, finishTouchCall)
	self.isCanClick = false
	self.info = info
	self.stepIndex = stepIndex
	self.finishTouchCall = finishTouchCall
	self.delayTime = self.info.delayTime
	self.ViewCtrl.title.text = info:GetTitle()
	self.ViewCtrl.content.text = info:GetContent()
	self.ViewCtrl.contentMask.text = info:GetContent()
	self.ViewCtrl.shaderMashGo:SetActive(info:IsClickType())
	self.ViewCtrl.maskGo:SetActive(not info:IsClickType())
	self.ViewCtrl.skipBtnGo:SetActive(false)
	self.ViewCtrl.bgGo:SetActive(not info:IsClickType())
	self.ViewCtrl.arrowTF.gameObject:SetActive(info:IsClickType())

	local pos, z=info:GetArrowPos()
	self.ViewCtrl.arrowTF.transform.anchoredPosition = pos
	self.ViewCtrl.arrowTF.transform:SetRotation(Vector3.New(0,0, z))
	self:SetMask(info:GetAreaMask())

	local targetId,_ = info:GetItemiconIds()
	self.ViewCtrl.itemIcon.gameObject:SetActive(targetId>0)
	if targetId> 0 then
		SetUIIcon(self.ViewCtrl.itemIcon, targetId)
	end

	if info:IsMapOperate() then
		---指引到地图
	end

	if info:IsClickType() then
		self.ViewCtrl.arrowTF.transform:DOKill()
		self.ViewCtrl.arrowTF.transform:SetScale(Vector3.one)
		self.ViewCtrl.arrowTF.transform:DOScale(Vector3.one*1.1,0.5):SetLoops(-1, LoopType.Yoyo)
	end
end

---显示中重复打开
---@param tab table<number>
function GuideView:SetMask(tab)
	if #tab > 0 then
		self.materialItem:SetFloat("_offsetX", tab[1])
		self.materialItem:SetFloat("_offsetY", tab[2])
		self.materialItem:SetFloat("_Opacity", tab[3])
		self.materialItem:SetFloat("_Width", 10)
		self.materialItem:SetFloat("_Heigh", 10)
		self.materialItem:DOFloat(tab[4],"_Width",0.8)
		self.materialItem:DOFloat(tab[5],"_Heigh",0.8):OnComplete(function()
			self.isCanClick = true
		end)
		self.materialItem:SetFloat("_Fillet", tab[6])
		self.materialItem:SetFloat("_Wfade", tab[7])
	else
		self.isCanClick = true
	end
end

---隐藏窗口
function GuideView:OnDisable()
	self.ViewCtrl.arrowTF.transform:DOKill()
end

---消耗释放资源
function GuideView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return GuideView