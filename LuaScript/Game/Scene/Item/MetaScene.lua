--- 元宇宙场景：编辑
--- SelectScene       
--- Author : hukiry     
--- DateTime 2023/3/1 17:51   
---

require 'Game.Core.Meta.MetaRule'
require 'Game.Core.Match.MatchRule'
---@class MetaScene:ISceneBase
MetaScene = {}

--进入 步骤2
function MetaScene:OnStateEnter()
    SceneRule.CurSceneType = SceneType.MetaCity
end

--离开 步骤1
function MetaScene:OnStateExit()
    SceneRule.LastScene = MetaScene
    Single.Sound():StopMusic()
    Single.Meta():InitData()
    if self.mainView then
        self.mainView:OnDestroy()
        self.mainView = nil
    end

    SingleData.Guide():CloseGuide()
end

--场景加载进度列表 步骤3
function MetaScene:OnSceneLoadingStep()
    local steps = {}
    local step = SceneLoadingStep.New(0.9, SceneRule.CurSceneType, SceneApplication.OnLevelLoadStart)
    table.insert(steps, step)

    local paths = {
        EAtlasResPath.Item,
        EAtlasResPath.Scene
    }
    --资源预加载
    self.preloaded = PrefabLoadingStep.New(0.05, paths, SceneApplication.OnLevelWasLoaded)
    table.insert(steps, self.preloaded)
    --
    ---@type InitializeLoadingStep
    self.initStep = InitializeLoadingStep.New(0.05)
    table.insert(steps, self.initStep)
    --
    UIManager:OpenWindow(ViewID.Loading, steps)
end

--步骤4：场景开始加载,初始化数据
function MetaScene:OnLevelLoadStart()
    Single.Meta():LoadEditorMap()
end

--步骤5前：切换场景执行，已经完成进入其它场景
function MetaScene:OnOtherStateEnter()
    if self.preloaded then
        self.preloaded:UnloadAll()
        self.preloaded=nil
    end
end

--步骤5：-场景资源加载完成
function MetaScene:OnLevelWasLoaded()
    Single.Sound():PlayMusic(ESoundResType.MusicMeta)
    ---1,场景声音，场景类视图开始
    local homeView = require("Game.Core.Match.MainView")
    ---@type MainView
    self.mainView = homeView.New(GameObject.Find("Game_Home"), self.initStep)
end

--场景初始化完成 (当进度条走完了，显示场景的时候调用) 步骤6
function MetaScene:OnLevelInitFinish()
    UIManager:CloseAllWindow()
    ---打开元宇宙视图
    UIManager:OpenWindow(ViewID.MetaEditor)
    ---新地图引导
    Single.TimerManger():DoTime(self,function()
    	--SingleData.Guide():CheckGuide()
    end,0.5,1)
end