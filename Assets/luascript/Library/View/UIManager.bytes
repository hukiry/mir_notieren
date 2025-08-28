

---UI性能的提升
---方法一UI的隐藏和显示，修改为每个视图UI上挂载Canvas，用enabled=true 或 false来显示和隐藏。
---方法二：视图UI上挂载Canvas，使用CanvasGroup做渐变隐藏，用enabled=true 或 false来启动Canvas

--窗口层
---@class ViewLayer
ViewLayer = {
	---UI 场景层
	SceneBubble = 1,
	---场景固定UI层
	SceneFixed = 2,
	---全屏窗口层
	Window = 3,
	---新手引导层
	Guide = 4,
	---动画层：货币飞起动画
	Animation = 5,
	---加载场景进度条层
	ProgressLoad = 6,
	---tips层，网络弹框
	Tips = 7,
	---网络菊花层
	Loading = 8,
}

--窗口背景模式
---@class ViewBackgoundMode
ViewBackgoundMode = {
	---该界面不包含任何背景
	None = 0,
	---透明背景，含有碰撞，含事件
	Collider_Event = 1,
	---透明背景，含有碰撞，不含事件
	Collider = 2,
	---模糊背景，含有碰撞，含事件
	Blur_Collider_Event = 3,
	---模糊背景，含有碰撞，不含事件
	Blur_Collider = 4,
}

---@class UIManager
UIManager = {}

---@type table<number, UnityEngine.Transform>
local ui_layer_list = {}
--所有活动的窗体列表
---@type table<number, UIWindowBase>
local show_window_list = {}
--所有不活动的窗体列表
---@type table<number, UIWindowBase>
local hidden_window_list = {}

---界面配置表
---@type UIConfigView
local uiconfigView = UIConfigView

--每多长时间检测一次 秒
local delay_detection_Time = 10

local pool
---UI Root对象
---@type UnityEngine.Transform
local mainCanvas = nil;
---Scene Root对象
---@type UnityEngine.GameObject
local mainGameGO = nil
---@type RootCanvas
--MainCanvas_Instance = RootCanvas.Instance

require("Library.View.EventTriggerType")
require("Library.MessageBox.TipMessageBox")

-------------------------------------华丽分割线----------------------------------------
--初始化
function UIManager:Init()
	mainGameGO = GameObject.Find("_Game")
	if mainGameGO == nil then
		print("获取界面根目录失败!","red") return
	end

	mainCanvas = mainGameGO.transform:Find("UIManager")
	---UI相机
	---@type UnityEngine.Camera
	UIManager.UICamera = mainGameGO.transform:Find("UICamera"):GetComponent("Camera")

	local ReporterDebug =  mainGameGO.transform:Find("Reporter")
	if ReporterDebug then
		ReporterDebug.gameObject:SetActive(UNITY_EDITOR or DEBUG or DEVELOP)
	end

	ui_layer_list[ViewLayer.SceneBubble] = self:InitCreateLayer("SceneBubbleLayer")		--场景气泡层
	ui_layer_list[ViewLayer.SceneFixed] = self:InitCreateLayer("SceneFixedLayer")		--场景固定UI层
	ui_layer_list[ViewLayer.Window] = self:InitCreateLayer("WindowLayer")	--全屏窗口层
	ui_layer_list[ViewLayer.Guide] = self:InitCreateLayer("GuideLayer")		--引导层
	ui_layer_list[ViewLayer.Animation] = self:InitCreateLayer("AnimationLayer")	--资源动画层
	ui_layer_list[ViewLayer.ProgressLoad] = self:InitCreateLayer("ProgressLoadLayer")	--进度加载层
	ui_layer_list[ViewLayer.Tips] = self:InitCreateLayer("TipsLayer")		--tips层
	ui_layer_list[ViewLayer.Loading] = self:InitCreateLayer("LoadingLayer")	--菊花层

	pool = self:InitCreateLayer("Pool")	--存储对象池
	---按时间检测，是否需要销毁
	Single.TimerManger():DoTime(self, self._onFrame, delay_detection_Time, -1)
end

function UIManager:_onFrame()
	for k, v in pairs(hidden_window_list) do
		---@type UIWindowBase
		local _window = v
		if not _window.permanenceExist and Time.realtimeSinceStartup - (_window.hideTime or 0) > 600 then
			self:DestroyWindow(v)
		end
	end
end

---实始化创建层
---@private
function UIManager:InitCreateLayer(layerName)
	---@type UnityEngine.Transform
	local tf = mainCanvas:Find(layerName)
	if tf == nil then
		tf = GameObject.New(layerName):AddComponent(typeof(UnityEngine.RectTransform))
		tf:SetParent(mainCanvas, false)
	end

	tf.anchorMin = Vector2.zero;
	tf.anchorMax = Vector2.one;
	tf.sizeDelta = Vector2.zero;
	tf:SetAsLastSibling()
	return tf
end

--打开UI界面
---@param panelId number 窗口ID
---@return UIWindowBase
function UIManager:OpenWindow(panelId, ...)
	if uiconfigView[panelId] == nil then
		log("尝试打开窗口，但未找到配置相关的Panel", panelId,"red")
		return
	end

	local config = uiconfigView[panelId]
	---判断是否在打开状态，如果在打开，重新调用OnShowPanel
	---@type UIWindowBase
	local window = show_window_list[panelId] or hidden_window_list[panelId]
	if window == nil and config ~= nil then
		--绑定的window脚本
		window =  require(config.classPath).New(panelId)
		if window == nil then--创建失败
			log("script lua 创建失败:",panelId,config.classPath,"yellow")
			return
		end
	end

	hidden_window_list[window.panelId] = nil
	show_window_list[window.panelId] = window

	self:ShowPanel(window, ...)
	return window
end

---激活窗口
---@private
---@param window UIWindowBase
function UIManager:ShowPanel(window,...)
	window:OpenWindow(...)

	if EMoneyView_Config[self.panelId]  then
		UIManager:OpenWindow(ViewID.Money, self.panelId)
	end
end

--创建一个新的界面
---@param panelId number 窗口ID
---@return UIWindowBase
function UIManager:OnCreateNewPanel(panelId)
	local config = uiconfigView[panelId]
	if config == nil then
		print("窗口未配置,panelId", panelId)
		return nil
	end

end

--关闭窗口
---@param panelId ViewID
---@param isAnimation boolean 是否含有关闭动画 默认：true
function UIManager:CloseWindow(panelId, isAnimation)
	if panelId == 0 or not panelId then
		return
	end
	local window = show_window_list[panelId]
	show_window_list[panelId] = nil
	if window == nil then
		return
	end

	if EMoneyView_Config[panelId] and panelId ~= ViewID.Main and panelId ~= ViewID.LevelMain then
		if SceneRule.CurSceneType == SceneType.HomeCity then
			UIManager:OpenWindow(ViewID.Money, ViewID.Main)
		elseif SceneRule.CurSceneType == SceneType.LevelCity then
			UIManager:OpenWindow(ViewID.Money, ViewID.LevelMain)
		elseif SceneRule.CurSceneType == SceneType.MetaCity then
			UIManager:OpenWindow(ViewID.Money, ViewID.LevelSelect)
		end
	end

	if window.closeWhenDestroy then
		UIManager:DestroyWindow(window)
		window:HideWindow()
	else
		hidden_window_list[panelId] = window
		window:HideWindow(isAnimation == nil and true or false)
		--记录窗口使用时间，待一段时间后销毁
		if not window.permanenceExist then
			window.hideTime = Time.realtimeSinceStartup
		end
	end
end

--销毁窗口
---@param window UIWindowBase
function UIManager:DestroyWindow(window)
	if window == nil then
		return
	end
	show_window_list[window.panelId] = nil
	hidden_window_list[window.panelId] = nil
	window:Destroy()

	--对UI资源的释放
	local config = UIConfigView[window.panelId]
	ResManager:Unload(config.resPath, 0)
end

--销毁所有未激活的窗口
function UIManager:DestroyAll()
	self:CloseAllWindow(ViewID.Loading)
	for key, window in pairs(hidden_window_list) do
		UIManager:DestroyWindow(window)
		hidden_window_list[key] = nil
	end
end

--关闭全部的窗口层界面
function UIManager:CloseAllWindow(...)
	local panelTab = {...}--保留打开的视图
	for key, window in pairs(show_window_list) do
		local isContain = false
		for i, v in ipairs(panelTab) do
			if v == window.panelId then
				isContain = true
				break
			end
		end

		if not isContain then
			UIManager:CloseWindow(key, false)
		end
	end
end

--关闭当前层的所有窗口
function UIManager:CloseWindowLayer(panelLayer)
	for key, window in pairs(show_window_list) do
		if window.panelLayer == panelLayer then
			UIManager:CloseWindow(key, false)
		end
	end
end

---判断一个窗口是否在打开中
---@param pPanelId
---@return boolean
function UIManager:IsShowing(pPanelId)
	if pPanelId == nil then return false end
	return show_window_list[pPanelId] ~= nil
end

function UIManager:IsMulShowing(...)
	local tabID, isShow = {...}, false
	for _, vPanelId in ipairs(tabID) do
		if show_window_list[vPanelId] ~= nil then
			isShow = true--其中一个显示
			break
		end
	end
	return isShow
end

---获取一个正在激活中的界面
---@param pPanelId
---@return UIWindowBase
function UIManager:GetActiveWindow(pPanelId)
	if pPanelId == nil then return false end
	return show_window_list[pPanelId]
end

---------------------------------华丽的分割线------------------------------
--下面这些是支持方法

---把window移动到顶层
---@param window UIWindowBase
function UIManager:MoveToTopWindow(window)
	local childCount = window.transform.parent.childCount
	window.transform:SetSiblingIndex(childCount - 1)
end

---获取当前层最顶层的窗口
---@param panelLayer ViewLayer
---@return UIWindowBase
function UIManager:TopWindow(panelLayer)
	local maxWindowIndex = 0
	local result = nil
	for i, window in pairs(show_window_list) do
		if(panelLayer == window.panelLayer) then
			local curWindowIndex = window.transform:GetSiblingIndex()
			if curWindowIndex > maxWindowIndex then
				maxWindowIndex = curWindowIndex
				result = window
			end
		end
	end
	return result
end


--获取对应的资源路径
function UIManager:GetPath(panelId)
	local config = uiconfigView[panelId]
	if (config == nil) then
		print("未找到对应的UI配置", panelId) return nil
	end
	return config.resPath
end

---获取UI对象池
---@return UnityEngine.Transform
function UIManager:GetPool()
	return pool.transform
end

---获取root 游戏对象
---@return UnityEngine.GameObject
function UIManager:GetRootGameObject()
	return mainGameGO
end

---返回所在层的父对象
---@param layer ViewLayer
---@return UnityEngine.Transform
function UIManager:GetLayerParent(layer)
	return ui_layer_list[layer]
end

---@param layer ViewLayer
---@param viewGo UnityEngine.GameObject
---@return UnityEngine.Canvas
function UIManager:SetCanvasLayer(viewGo,layer)
	--local childCanvas = viewGo:GetComponentsInChildren(typeof(UnityEngine.Canvas), true)
	--if childCanvas~=nil then
	--	for i = 1, childCanvas.Length do
	--		childCanvas[i-1].sortingLayerName = layer
	--		childCanvas[i-1].sortingOrder = mainCanvas.childCount + 1
	--	end
	--end
	--
	-----@type UnityEngine.Canvas 画布
	--local  uiCanvas = viewGo:GetComponent("Canvas")
	--if uiCanvas == nil then
	--	uiCanvas = viewGo:AddComponent(typeof(UnityEngine.Canvas))
	--end
	----
	-------@type UnityEngine.UI.GraphicRaycaster 射线检查
	--local gRaycaster = viewGo:GetComponent("GraphicRaycaster")
	--if gRaycaster == nil then
	--	gRaycaster = viewGo:AddComponent(typeof(UnityEngine.UI.GraphicRaycaster))
	--end
	--
	--uiCanvas.overrideSorting = true
	--uiCanvas.sortingLayerName = layer
	--
	--viewGo.transform.anchorMin = Vector2.zero;
	--viewGo.transform.anchorMax = Vector2.one;
	--viewGo.transform.sizeDelta = Vector2.zero;
	--return uiCanvas
end

---获取UIRoot脚本
---@return UnityEngine.Transform
function UIManager:GetCanvasParent()
	return mainCanvas
end

---从隐藏列表获取某UI
function UIManager:GetUnActiveWindowById(panelId)
	return hidden_window_list[panelId]
end

---获取所有激活的窗口
---@return table<number,UIWindowBase>
function UIManager:GetActiveWindowList()
	return show_window_list
end
