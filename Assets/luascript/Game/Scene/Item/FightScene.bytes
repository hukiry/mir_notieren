---
--- FightScene       
--- Author : hukiry     
--- DateTime 2023/10/20 10:26   
---

require 'Game.Core.Match.MatchRule'
---@class FightScene:ISceneBase
FightScene = {}
FightScene.loadingPanelId = ViewID.Loading

---@type SceneLoadingStep
FightScene.preloaded = nil

--步骤2：当要进入时
function FightScene:OnStateEnter()
    SceneRule.CurSceneType = SceneType.LevelCity
end

--步骤3：-场景加载进度列表
---@return table<number, SceneProgressBase>
function FightScene:OnSceneLoadingStep()
    local steps = {}
    ---场景预加载
    local sceneLoadingStep = SceneLoadingStep.New(0.9, SceneRule.CurSceneType, SceneApplication.OnLevelLoadStart)
    table.insert(steps, sceneLoadingStep)

    local paths = {
        EAtlasResPath.Item,
        EAtlasResPath.Scene
    }
    --资源预加载
    self.preloaded = PrefabLoadingStep.New(0.05, paths, SceneApplication.OnLevelWasLoaded)
    table.insert(steps, self.preloaded )

    ---@type InitializeLoadingStep
    self.initStep = InitializeLoadingStep.New(0.05)
    table.insert(steps, self.initStep)

    UIManager:OpenWindow(ViewID.Loading, steps)
end

--步骤4：场景开始加载,初始化数据
function FightScene:OnLevelLoadStart()
    ---初始化地图数据
    SingleData.Metauniverse():StartFight()
end

--步骤5前：切换场景执行，已经完成进入其它场景
function FightScene:OnOtherStateEnter()
    ----预加载资源类，在离开场景时，需要释放
    if self.preloaded then
        self.preloaded:UnloadAll()
        self.preloaded = nil
    end
end

--步骤5：-场景资源加载完成
function FightScene:OnLevelWasLoaded()
    Single.Sound():PlayMusic(ESoundResType.MusicMeta)
    ---1,场景声音，场景类视图开始
    local homeView = require("Game.Core.Match.MainView")
    ---@type MainView
    self.mainView = homeView.New(GameObject.Find("Game_Home"), FightScene.initStep)
end

--步骤6：场景初始化完成 (当进度条走完了，显示场景的时候调用)
function FightScene:OnLevelInitFinish()
    UIManager:CloseAllWindow()
    ---打开主界面
    if UNITY_EDITOR then
        UIManager:OpenWindow(ViewID.Gm)--编辑模式
    end
    UIManager:OpenWindow(ViewID.LevelMain, true)
end

--步骤1 ：当要离开时、切换场景执行
function FightScene:OnStateExit()
    ---关闭指定界面
    SceneRule.LastScene = FightScene

    if self.mainView then
        self.mainView:OnDestroy()
        self.mainView = nil
    end

    Single.Sound():StopMusic()
    GameObjectPool.ClearAll()
end