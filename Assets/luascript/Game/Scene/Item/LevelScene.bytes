---
--- 关卡副本场景
--- Created by Administrator.
--- DateTime: 2022/12/19 13:58
---

---@class LevelScene:ISceneBase
LevelScene = {}
LevelScene.loadingPanelId = ViewID.Loading

---@type SceneLoadingStep
LevelScene.preloaded = nil

--步骤2：当要进入时
function LevelScene:OnStateEnter()
    SceneRule.CurSceneType = SceneType.LevelCity
end

--步骤3：-场景加载进度列表
---@return table<number, SceneProgressBase>
function LevelScene:OnSceneLoadingStep()
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
function LevelScene:OnLevelLoadStart()
    ---初始化地图数据
    Single.Match():LoadMap()
end

--步骤5前：切换场景执行，已经完成进入其它场景
function LevelScene:OnOtherStateEnter()
    ----预加载资源类，在离开场景时，需要释放
    if self.preloaded then
        self.preloaded:UnloadAll()
        self.preloaded = nil
    end
end

--步骤5：-场景资源加载完成
function LevelScene:OnLevelWasLoaded()
    if not self.soundAudio then
        self.soundAudio = ESoundResType.MusicLevel .. Mathf.Random(1,3)
    end
    Single.Sound():PlayMusic(self.soundAudio)

    ---1,场景声音，场景类视图开始
    local homeView = require("Game.Core.Match.MainView")
    ---@type MainView
    self.mainView = homeView.New(GameObject.Find("Game_Home"), LevelScene.initStep)
end

--步骤6：场景初始化完成 (当进度条走完了，显示场景的时候调用)
function LevelScene:OnLevelInitFinish()
    UIManager:CloseAllWindow()
    ---打开主界面
    if UNITY_EDITOR then
        UIManager:OpenWindow(ViewID.Gm)--编辑模式
    end
    UIManager:OpenWindow(ViewID.LevelMain)
end

--步骤1 ：当要离开时、切换场景执行
function LevelScene:OnStateExit()
    ---关闭指定界面
    SceneRule.LastScene = LevelScene

    if self.mainView then
        self.mainView:OnDestroy()
        self.mainView = nil
    end

    Single.Sound():StopMusic()
    GameObjectPool.ClearAll()
end