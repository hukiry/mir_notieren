---
--- UIEvent       
--- Author : hukiry     
--- DateTime 2024/8/5 15:46   
---

---@class UIEvent
UIEvent = {
    Scene_Load_Finish = 0,
    ----UI 注册面板 1-300
    -----300~310------------------
    ---振动视图：x， y
    Match_Shake_View = 301,
    ---背景遮罩：bool
    Match_BackgoundMask_View = 302,
    ---AI提示:背景流光
    Match_AI_Tip_View = 303,
    ---AI提示:物品振动
    Match_AITip_Shake_View = 304,

    Match_RemoveFloat_View = 305,
    Match_RemoveBottom_View = 306,
    -----350~400 视图操作------------------
    ---物品取消选中  x, y, x1, y1
    Match_Operate_Move = 351,
    ---物品取消选中  x, y
    Match_Operate_Click = 352,
    ---使用道具 props,x, y
    Match_Operate_Props = 353,
    ---Gm命令：props, x, y, isHorizontal
    Match_Create_GM = 360,

    ---------400-500 ui部分 --------------------
    ---主界面红点刷新
    UI_MainView_Redpoint = 401,
    ---聊天消息：EChatState
    UI_ChatView = 402,

    ---------500-550 元宇宙 --------------------
    Meta_Operate_Move = 500,
    Meta_Operate_Click = 501,
    Meta_Operate_ResetDo = 502,
    ---改变编辑场景层：层状态
    Meta_Operate_LayerView = 503,

    ---场景视图刷新
    ChangeView_Language = 1000,
}