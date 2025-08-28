---
--- 对战地图
--- Create Time:2023-10-15
--- Author: Hukiry
---

---@class MyMapFightView:UIWindowBase
local MyMapFightView = Class(UIWindowBase)

function MyMapFightView:ctor()
	---@type MyMapFightControl
	self.ViewCtrl = nil
end

---初始属性字段
function MyMapFightView:Awake()
	self.prefabName = "MyMapFight"
	self.prefabDirName = "Metauniverse/Tip"
	---@type boolean
	self.isEnableTimer = false
	---@type number
	self.delaySecond = 1
	---@type number
	self.loopCount = 0
end

---初始界面:注册按钮事件等
function MyMapFightView:Start()
	self:AddClick(self.ViewCtrl.closeBtnGo, Handle(self, self.Close))

	self.isActiveComment = self.ViewCtrl.commentBoxGo.activeSelf
	---点赞
	self:AddClick(self.ViewCtrl.rateBtnGo, Handle(self, self._CallMeta, true))
	---发送评论
	self:AddClick(self.ViewCtrl.sendBtnGo, Handle(self, self._CallMeta, false))
	---写评论
	self:AddClick(self.ViewCtrl.commentBtnGo, function()
		self.isActiveComment = not self.isActiveComment
		if self.isActiveComment then
			--播放
			self.commentAnimation:DORestart()
		else
			--回播放
			self.commentAnimation:DOPlayBackwards()
		end
		self.ViewCtrl.writeComment.spriteName = self.isActiveComment and "quxiao2" or "y_write"
	end)

	---进入游戏：接受挑战
	self:AddClick(self.ViewCtrl.playBtnGo, function()
		Single.Meta().numberId = self.info.numberId
		SceneApplication.ChangeState(FightScene)
		TipMessageBox.ShowUI("接受挑战 。。。")
	end)


	self.ViewCtrl.inputField.onValueChanged:AddListener(Handle(self, self._onValueChanged))
	self.ViewCtrl.commentBoxGo:SetActive(true)
	---动画组件初始化时，不需要做任何操作
	---@type DG.Tweening.DOTweenAnimation
	self.commentAnimation = self.ViewCtrl.commentBoxGo:GetComponent("DG.Tweening.DOTweenAnimation")
	self.comment = ''
end

function MyMapFightView:_CallMeta(isLike)
	if not Single.Http():IsHaveNetwork() then
		TipMessageBox.ShowUI(GetLanguageText(10002))
		return
	end

	if isLike then
		Single.Request().SendMeta(EFriendHandleType.like, self.info.id, self.info.numberId)
	else
		Single.Request().SendMeta(EFriendHandleType.comment, self.info.id, self.info.numberId, self.comment)
	end
end

function  MyMapFightView:_onValueChanged(v)
	self.comment = string.Trim(v)
	self.ViewCtrl.sendBtnGo:SetActive(string.len(self.comment)>0)
end

---显示窗口:初次打开
---@param info MetaFightInfo
function MyMapFightView:OnEnable(info)
	self.info = info
	local dataArray = Single.Meta():GetCacheData():GetImageData(self.info.numberId)
	local atlas = Single.SpriteAtlas():LoadAtlas(EAtlasResPath.Item)
	self.ViewCtrl.iconBack:LoadMesh(atlas, dataArray)

	local mapinfo = Single.Meta():GetCacheData():GetMapInfo(self.info.numberId)
	self.ViewCtrl.nameTxt.text = mapinfo.mapName
	self.ViewCtrl.mapdesc.text = mapinfo.mapDesc
	self.ViewCtrl.nickTxt.text = mapinfo.author

	self.ViewCtrl.mapTxt.text = mapinfo:GetMapIdText(self.info.numberId)
	self.ViewCtrl.timeTxt.text = mapinfo:GetTimeText()
	local str = string.format(":<b>%s</b>", (self.info.numberId + 200))
	self.ViewCtrl.downTxt.text = GetLanguageText(16118) ..  str

	self.ViewCtrl.headIcon.spriteName = "role_" .. Single.Player().headId
	self.ViewCtrl.sendBtnGo:SetActive(string.len(self.comment)>0)
end

---隐藏窗口
function MyMapFightView:OnDisable()
	
end

---消耗释放资源
function MyMapFightView:OnDestroy()
	self.ViewCtrl:OnDestroy()
end

return MyMapFightView