
---场景逻辑管理
---@class SceneApplication
SceneApplication = {}

---游戏退出时
function SceneApplication.OnDestroy()
	Single.ClearData()
end

---回到原来的状态
---@param loadFinish function 场景加载完成回调
---@param initFinish function 加载完成并初始化完成回调
function SceneApplication.BackLastState(loadFinish, initFinish)
	SceneApplication.ChangeState(SceneRule.LastScene, loadFinish, initFinish)
end

---改变场景
---@param newState ISceneBase 新的状态
---@param loadFinish function 场景加载完成回调
---@param initFinish function 加载完成并初始化完成回调
function SceneApplication.ChangeState(newState, loadFinish, initFinish)
	SceneRule.Loading = true
	local oldState = SceneApplication.LevelState
	--旧的状态退出
	if(oldState and oldState.OnStateExit) then
		oldState.OnStateExit(oldState)
	end
	if newState ~= oldState then
		SceneApplication.OldState = oldState
	end

	SceneApplication.LevelState = newState
	SceneApplication.loadFinish = loadFinish
	SceneApplication.initFinish = initFinish
	--新的状态进入
	if(newState and newState.OnStateEnter) then
		newState.OnStateEnter(newState)
	end

	newState.OnSceneLoadingStep(newState)
end

---场景加载开始：初始化数据
function SceneApplication.OnLevelLoadStart()
	local LevelState = SceneApplication.LevelState

	if LevelState and LevelState.OnLevelLoadStart then
		LevelState.OnLevelLoadStart(LevelState)
	end
end

---场景加载完成:载入场景视图
function SceneApplication.OnLevelWasLoaded()
	log("场景加载完成，进入状态  %s", SceneRule.CurSceneType)
	local oldState = SceneApplication.OldState;
	if oldState and oldState.OnOtherStateEnter then
		oldState.OnOtherStateEnter(oldState);
	end

	local LevelState = SceneApplication.LevelState;
	if LevelState and LevelState.OnLevelWasLoaded then
		LevelState.OnLevelWasLoaded(LevelState);
	end

	if SceneApplication.loadFinish ~= nil then
		SceneApplication.loadFinish()
	end
end

---场景加载完成后，并初始化完成
function SceneApplication.OnLevelInitFinish()
	log("场景加载完成后，并初始化完成  %s", SceneRule.CurSceneType)
	SceneRule.Loading = false
	if SceneApplication.initFinish ~= nil then
		SceneApplication.initFinish()
	end

	local LevelState = SceneApplication.LevelState
	if LevelState and LevelState.OnLevelInitFinish then
		LevelState.OnLevelInitFinish(LevelState)
	end
end