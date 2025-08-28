
---所有Panel的基类

local Back_PREFAB_PATH = ""
---@class UIWindowBase:DisplayObjectBase
UIWindowBase = Class(DisplayObjectBase)

function UIWindowBase:ctor(panelId)
	self.panelId = panelId
	---预制体名称
	---@type string
	self.prefabName = ""
	---预制体所在目录名称
	---@type string
	self.prefabDirName = nil
	---是否为异步加载
	self.asynLoad = true
	---关闭的时候销毁
	self.closeWhenDestroy = false
	---永不销毁
	self.permanenceExist = false
	---是否正在异步加载中
	self.isLoading = false
	---是否处于激活状态，从开始加载，到HidePanel
	self.isActive = true
	---是否在显示中，从OpenWindow到hidePanel
	self.isShow = false
	---这个窗口是否已经被销毁
	self.isDestroy = false

	---@type ViewLayer
	self.panelLayer = ViewLayer.Window
	---@type ViewBackgoundMode
	self.panelBgMode = ViewBackgoundMode.None
	---启动计时器
	self.isEnableTimer = false
	---每隔多少秒执行一次
	self.delaySecond = 1
	---循环次数：-1=无限循环 , 0或1 默认执行一次，
	self.loopCount = 0
	---子视图
	self.chindrenViews = {}
	---页面
	---@type UnityEngine.GameObject
	self.panelGo = nil


	self.delayOpen = true		--沿用通用的延时打开窗口
	self.canBgClose = true	--是否可以背景事件关闭
	self.isBattleRecord = true;--进入战斗时是否记录
	self.isBattleClose = true;--进入战斗时是否关闭

	self:Awake()
end

--界面加载回调
function UIWindowBase:OnAssetLoadedCallback(name, prefab)
	--实例化界面
	---@type UnityEngine.GameObject
	local go = GameObject.Instantiate(prefab)
	---对象池中创建空对象，作为视图的父对象，用于做动画效果
	go.transform:SetParent(UIManager:GetLayerParent(self.panelLayer), false)
	---设置属性
	self:SetGameObject(go)

	self.panelGo = go

	local dirName = self.prefabDirName or self.prefabName
	try{
		function()
			local viewCrl = require ("game.ui.".. dirName ..'.' .. self.prefabName .. "Control");
			self.ViewCtrl = viewCrl.New(go);
		end,
		catch = function(error)
			logError("请配置目录：prefabName", self.prefabName, "error:", error)
		end
	}
	self:Start()
end

function UIWindowBase:LoadPanel(callFinish)
	if self.isLoading then return end

	self.isLoading = true
	local uiResPath = UIManager:GetPath(self.panelId)
	---@param go UnityEngine.GameObject
	local func = function(name, go)
		local errorTips = "";
		if go==nil or go:Equals(nil) then errorTips = "窗口资源加载失败"; end
		if self.isActive == false then errorTips = "窗口已被关闭"; end
		if self.isDestroy then errorTips = "窗口已经被销毁"; end

		if errorTips ~= "" then
			ResManager:Unload(name, 0)
			if callFinish then callFinish(false, self.panelId, self, string.format("%s=%s", errorTips, name)) end
			return
		end
		self.isLoading = false;
		self:OnAssetLoadedCallback(name, go)
		if callFinish then callFinish(true, self.panelId, self) end
	end

	if self.asynLoad then	--为异步加载
		ResManager:LoadAsync(uiResPath, func)
	else
		func(uiResPath, ResManager:Load(uiResPath))
	end
end

---打开窗口，每次打开都会执行
function UIWindowBase:OpenWindow( ... )
	local args = {...}

	self.isActive = true
	if self.panelGo == nil then	---未加载过
		self:LoadPanel(function(isSuccess, panelId, _win, errorTip)
			if isSuccess then
				self:ShowPanel(params_arg(args))
			else
				logError("窗口打开失败：", errorTip)
				UIManager:CloseWindow(panelId)
			end
		end);
	else
		---如果窗口再次打开时，执行OnRefresh()
		if self.isShow then
			self:OnRefresh(params_arg(args))
		else
			self:ShowPanel(params_arg(args))
		end
	end
end

function UIWindowBase:ShowPanel(...)
	if self.isShow == true then
		return
	end
	self.isShow = true
	if self.panelGo == nil then
		logError("UIWindowBase:ShowPanel时，窗口可能已经被销毁，请检查！panelId:", self.panelId)
		self:Close()
		return
	end

	self.panelGo:SetActive(true)

	EventDispatch:Register(self, self.panelId, self.OnDispatch)
	self:OnEnableTimer()
	self:OnEnable(...)

	self.panelGo.transform:SetAsLastSibling()
end

--隐藏窗口时
---@param isAnimation boolean 是否含有关闭动画
function UIWindowBase:HideWindow(isAnimation)
	self.isActive = false
	if not self.isShow then
		return
	end
	self.isShow = false
	EventDispatch:UnRegister(self)
	Single.TimerManger():RemoveHandler(self)
	self:OnDisable()
	self.gameObject:SetActive(false)
end



---启动定时器
function UIWindowBase:OnEnableTimer()
	if self.isEnableTimer then
		Single.TimerManger():DoTime(self, self.OnTimer, self.delaySecond, self.loopCount)
	end
end

-----------------------------------begin 生命周期---------------------------------------------
-----用于派生类对items的统一管理
function UIWindowBase:AddItems(value)
	table.insert(self.chindrenViews, value)
	return value
end

---初始化窗口属性
function UIWindowBase:Awake()
	for i = 1, #self.chindrenViews do
		if self.chindrenViews[i].Awake then self.chindrenViews[i]:Awake() end
	end
end

---初始化界面
function UIWindowBase:Start()
	for i = 1, #self.chindrenViews do
		if self.chindrenViews[i].Start then self.chindrenViews[i]:Start() end
	end
end

---事件派发
function UIWindowBase:OnDispatch(...)
	for i = 1, #self.chindrenViews do
		if self.chindrenViews[i].OnDispatch then self.chindrenViews[i]:OnDispatch(...) end
	end
end

---定时器
function UIWindowBase:OnTimer()
	for i = 1, #self.chindrenViews do
		if self.chindrenViews[i].OnTimer then self.chindrenViews[i]:OnTimer() end
	end
end

---显示窗口
function UIWindowBase:OnEnable(...)
	for i = 1, #self.chindrenViews do
		if self.chindrenViews[i].OnEnable then self.chindrenViews[i]:OnEnable() end
	end
end

---窗口显示中重复打开调用
function UIWindowBase:OnRefresh()
	for i = 1, #self.chindrenViews do
		if self.chindrenViews[i].OnRefresh then self.chindrenViews[i]:OnRefresh() end
	end
end

--隐藏窗口时
function UIWindowBase:OnDisable()
	for i = 1, #self.chindrenViews do
		if self.chindrenViews[i].OnDisable then self.chindrenViews[i]:OnDisable() end
	end
end

--背景单击回调事件
function UIWindowBase:OnBgClick()
	self:Close()
end

function UIWindowBase:OnDestroy()
	for i = 1, #self.chindrenViews do
		if self.chindrenViews[i].OnDestroy then self.chindrenViews[i]:OnDestroy() end
	end
end

-----------------------------------end 生命周期--------------------------------------------------

---@return UnityEngine.Material
function UIWindowBase:GetMaterialCircle()
	if self.MatWaitLoading == nil then
		self.MatWaitLoading = Material.New(Shader.Find("Custom/UIWaitLoading"))
	end
	return self.MatWaitLoading
end

--界面销毁前调用
function UIWindowBase:Destroy()
	if not self.isDestroy then
		self:OnDestroy()
		self.isDestroy = true
		GameObject.Destroy(self.panelGo)
		if not self.viewBgGo then
			GameObject.Destroy(self.viewBgGo)
			--ResManager:Unload(BLUE_PREFAB_PATH)
		end
	end
end

function UIWindowBase:IsDestroy()
	return self.panelGo == nil
end

--关闭界面 派生类自己调用，关闭处身
function UIWindowBase:Close(isAnimation)
	UIManager:CloseWindow(self.panelId, isAnimation)
end
