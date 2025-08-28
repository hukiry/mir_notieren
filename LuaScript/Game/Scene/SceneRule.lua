---@class SceneType
SceneType = {
	Game = "Game",
	---元宇宙主界面
	HomeCity = "HomeCity",
	---关卡和元宇宙挑战+测试
	LevelCity = "LevelCity",
	---元宇宙编辑场景
	MetaCity = "MetaCity",
	---游戏主界面
	ViewCity = "ViewCity",
}

---@class SceneRule
SceneRule = {}
---上次打开的场景
SceneRule.LastScene = nil
---当前场景类型
SceneRule.CurSceneType = SceneType.Game
---是否正在切换场景中 false:未在加载状态，true:正在加载状态
SceneRule.Loading = false

---是否在登陆场景：登录界面的游戏
function SceneRule.IsInLoginScene()
	return SceneRule.CurSceneType == SceneType.Game
end

---当前是在关卡场景
---@return boolean
function SceneRule.IsInLevelScene()
	return not SceneRule.Loading and SceneRule.CurSceneType == SceneType.LevelCity
end

---是在家园场景中
function SceneRule.IsInHomeScene()
	return not SceneRule.Loading and SceneRule.CurSceneType == SceneType.HomeCity
end

------------------------------------场景对象配置--------------------------------------------
