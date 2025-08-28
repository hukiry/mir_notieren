--- 场景基类
--- ISceneBase
--- Author : hukiry     
--- DateTime 2023/9/15 12:12   
---

---@class ISceneBase
ISceneBase = Class()

---步骤1：准备进入场景
function ISceneBase:OnStateEnter()
    SceneRule.CurSceneType = SceneType.MetaCity
    ---@type PrefabLoadingStep
    self.preloaded = nil
end

---步骤2：场景加载进度步骤
function ISceneBase:OnSceneLoadingStep()
    local steps = {}
    ---预加载场景
    local step = SceneLoadingStep.New(0.9, SceneRule.CurSceneType, SceneApplication.OnLevelLoadStart, self.OnLevelLoadFinish)
    table.insert(steps, step)
    --todo 预加载资源路径
    local pathsCustom = {
        --EAtlasResPath.Item,
        --EAtlasResPath.Scene
    }
    ---预预加载资源
    self.preloaded = PrefabLoadingStep.New(0.05, pathsCustom, SceneApplication.OnLevelWasLoaded)
    table.insert(steps, self.preloaded)
    ---初始化完成
    self.initStep = InitializeLoadingStep.New(0.05)
    table.insert(steps, self.initStep)
    ---打开进度视图
    UIManager:OpenWindow(ViewID.Loading, steps)
end

---步骤3：初始化场景数据（场景开始加载）
function ISceneBase:OnLevelLoadStart()
    ----todo 初始化数据
    --Single.Match():LoadMap()
end

---步骤3：场景加载完成
function ISceneBase:OnLevelLoadFinish() end
---步骤4：切换新场景完成时，执行此方法
function ISceneBase:OnOtherStateEnter()
    ---预加载资源类，在离开场景时，需要释放
    if self.preloaded then
        self.preloaded:UnloadAll()
        self.preloaded = nil
    end
end

---步骤5：加载场景视图（场景完成加载）
function ISceneBase:OnLevelWasLoaded()
    ----todo 播放场景声音
    --Single.Sound():PlayMusic(ESoundResType.MusicMeta)
    ----todo 场景视图开始
    --local homeView = require("Game.Core.Match.MainView")
    -----@type MainView
    --self.mainView = homeView.New(GameObject.Find("Game_Home"), LevelScene.initStep)
end

---步骤6：加载UI视图（场景视图完成）
function ISceneBase:OnLevelInitFinish()
    ----todo 关闭所有UI
    --UIManager:CloseAllWindow()
    ----todo 打开场景主UI
    --UIManager:OpenWindow(ViewID.Main)
    ----todo 打开引导检查
    --Single.TimerManger():DoTime(self,function()
    --	SingleData.Guide():CheckGuide()
    --end,0.5,1)
end

---步骤7：退出场景(切换其他场景时)
function ISceneBase:OnStateExit()
    --SceneRule.LastScene = SceneBase
    --if self.mainView then
    --    self.mainView:OnDestroy()
    --    self.mainView = nil
    --end

    ----todo 停止声音
    --Single.Sound():StopMusic()
    ----todo 释放对象池
    --GameObjectPool.ClearAll()
    ----todo 关闭引导
    --SingleData.Guide():CloseGuide()
end
