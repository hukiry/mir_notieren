---
--- 协议功能枚举说明
--- Created by hukiry.
--- DateTime: 2023/5/24 20:40
---

---传输公共功能 1=周排行，2=月排行，3=年排行
---@class EHttpRankType
EHttpRankType = {
    ---月排行
    Month = 1,
    ---年排行
    Year = 2,
    ---总排行
    Total = 3
}

---游戏问题类型:1=游戏问题，2=崩溃问题，3=充值问题，4=其他问题
---@class EHttpFeedfaceType
EHttpFeedfaceType = {
    ---游戏问题
    game = 1,
    ---崩溃问题
    collapse = 2,
    ---充值问题
    recharge = 3,
    ---其他问题
    other = 4
}


---传输活动功能
---@class EHttpActivityState
EHttpActivityState = {
    ---1=请求数据
    Request = 1,
    ---2=活动完成
    Finished = 2,
    ---3=活动进度值
    FinishProgressbar = 3,
}


---邮件传输状态
---@class EMailState
EHttpMailState={
    Request = 1,
    Reded = 2
}


---登录传输状态
---@class EHttpLoginState
EHttpLoginState={
    Logout = 1,
    Bind = 2,
    ChangeDevice = 3,
    FixNick = 4
}

---商店
---@class EHttpShopState
EHttpShopState={
    Request = 1,
    ---播放特效
    Pay = 2,
    ---不播放特效
    Other = 3
}

---操作状态
---@class EFriendState
EFriendState = {
    none = 0,
    ---1,我的好友
    Friend = 1,
    ---2,好友消息
    Message=2,
    ---3, 陌生人
    Stranger=3,
    ---4, 查找好友
    Search=4,
    ---5, 刷新陌生
    Refresh = 5,
    ---6, 生命列表
    Life = 6,
}

---1=好友，2=挑战，3=生命
---@class EFriendHandleType
EFriendHandleType= {
    ---限制 30人
    Friend = 1,
    ---每天限制1人
    Challenge = 2,
    ---限制5分钟
    Life = 3
}

---0=帮助生命，1=申请好友，2=接受好友，3=邀请好友挑战，4=接受挑战
---@class EFriendHandleState
EFriendHandleState = {
    ---陌生人状态
    None = 0,
    ---发起好友申请|请求帮助|发起挑战
    SendAccept = 1,
    ---取消好友申请|取消发送挑战
    CancelAccept = 2,
    ---接受好友申请|获得帮助|接收好友挑战
    Accept = 3,
    ---拒绝好友申请|拒绝挑战
    RefuseAccept = 4,
    ---删除好友|完成帮助|删除挑战
    Delete = 5,
    ---被邀请|收到帮助|收到挑战
    ReceiveAccepted = 6,
    ---好友
    Friend  = 7,
    ---完成挑战
    Finished = 8,
    ---超出20个限制
    OverLimit=9,
}

---聊天内容状态：一条消息限制30个字符，一共25条上线
---@class EChatState
EChatState= {
    ---获取聊天
    Chat = 0,
    ---发送聊天
    SendChat = 1,
}

---生命状态
---@class ELifeState
ELifeState = {
    ---生命列表
    Life = 0,
    --- 询问帮助
    AskHelp = 1,
    --- 取消
    GetHelp = 2,
    --- 帮助
    Help = 3,
    ---社团列表
    Gass =4,
    ---加入社团
    AddGass = 5,
    ---退出社团
    QuitGass = 6,
    ---删除社团
    DeleteGass = 7,
    ---创建社团
    CreateGass = 8,
    ---超出30个限制
    OverLimit = 9,
}