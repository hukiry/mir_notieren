--- 视图场景或登录场景
--- ViewScene       
--- Author : hukiry     
--- DateTime 2023/3/1 17:51   
---

require 'Game.Scene.ESpriteAtlasResource'

---@class ViewScene:ISceneBase
ViewScene = {}

--进入 步骤2
function ViewScene:OnStateEnter()
    SceneRule.CurSceneType = SceneType.ViewCity
end

--离开 步骤1
function ViewScene:OnStateExit()
    SceneRule.LastScene = ViewScene
    Single.Sound():StopMusic()
    --SingleData.Guide():CloseGuide()
end

--场景加载进度列表 步骤3
function ViewScene:OnSceneLoadingStep()
    local steps = {}
    local step = SceneLoadingStep.New(0.9, SceneRule.CurSceneType, SceneApplication.OnLevelLoadStart)
    table.insert(steps, step)

    local paths = {
        EAtlasResPath.Item,
        EAtlasResPath.Scene,
    }
    --资源预加载
    self.preloaded = PrefabLoadingStep.New(0.05, paths, SceneApplication.OnLevelWasLoaded)
    table.insert(steps, self.preloaded)
    --
    ---@type InitializeLoadingStep
    ViewScene.initStep = InitializeLoadingStep.New(0.05)
    table.insert(steps, ViewScene.initStep)
    --
    UIManager:OpenWindow(ViewID.Loading, steps, true)
end

--场景加载开始 步骤4
function ViewScene:OnLevelLoadStart()
end

--进入其它状态 步骤5前
function ViewScene:OnOtherStateEnter()
    if self.preloaded then
        self.preloaded:UnloadAll()
        self.preloaded=nil
    end
end

--场景加载完成 步骤5
function ViewScene:OnLevelWasLoaded()
    ViewScene.initStep:SetDone()
end

--场景初始化完成 (当进度条走完了，显示场景的时候调用) 步骤6
function ViewScene:OnLevelInitFinish()
    UIManager:OpenWindow(ViewID.Game, function()
        ---关闭窗口
        UIManager:CloseAllWindow(ViewID.Game)
    end)

    --Single.Sound():InitData()
end

