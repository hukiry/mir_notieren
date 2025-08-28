---
--- 活动场景：元宇宙主视图场景
--- Created huiry
--- DateTime: 2019/3/27 0027 19:33

---@class HomeScene:ISceneBase
HomeScene = {}
HomeScene.loadingPanelId = ViewID.Loading

---@type PrefabLoadingStep
HomeScene.preloaded = nil

--当要进入时 步骤2
function HomeScene:OnStateEnter()
	SceneRule.CurSceneType = SceneType.HomeCity
end

--当要离开时 步骤1 ：切换场景执行
function HomeScene:OnStateExit()
	---关闭指定界面
	SceneRule.LastScene = HomeScene
	if self.mainView then
		self.mainView:OnDestroy()
		self.mainView = nil
	end

	--预加载资源类，在离开场景时，需要释放
	if self.preloaded then self.preloaded:UnloadAll() end
end

---场景加载进度列表 步骤3
---@return table<number, SceneProgressBase>
function HomeScene:OnSceneLoadingStep()
	local steps = {}
	---场景预加载
	local sceneLoadingStep = SceneLoadingStep.New(0.9, SceneRule.CurSceneType, SceneApplication.OnLevelLoadStart)
	table.insert(steps, sceneLoadingStep)

	local paths = {
		EAtlasResPath.Item
	}
	---资源预加载
	self.preloadedStep = PrefabLoadingStep.New(0.05, paths, SceneApplication.OnLevelWasLoaded)
	table.insert(steps, self.preloadedStep)

	---@type InitializeLoadingStep
	self.initStep = InitializeLoadingStep.New(0.05)
	table.insert(steps, self.initStep)

	UIManager:OpenWindow(ViewID.Loading, steps)
end

---场景开始加载： 步骤4
function HomeScene:OnLevelLoadStart()
	---初始化地图数据
end

---步骤5前：切换场景执行
function HomeScene:OnOtherStateEnter()

end

---步骤5：加载场景视图
function HomeScene:OnLevelWasLoaded()
	---UI视图时
	self.initStep:SetDone()
end

---场景初始化完成 (当进度条走完了，显示场景的时候调用) 步骤6
function HomeScene:OnLevelInitFinish()
	UIManager:CloseAllWindow()
	---打开主界面
	UIManager:OpenWindow(ViewID.MetaHome)

	--Single.TimerManger():DoTime(self,function()
	--	SingleData.Guide():CheckGuide()
	--end,0.5,1)
end