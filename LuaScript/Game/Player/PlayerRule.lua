---
--- 记录玩家属性
--- PlayerRule
--- DateTime: 2022/5/3 18:09
---

---@class ELogin
ELogin = {
    LangCode = 0, -- 语言切换代码
    Platform= 1,  -- 登录平台
    Nick= 2, -- 昵称
    IpAddress= 3, -- ip地址
    Type= 4, -- 登录类型：游客或者账号
    HeadID= 5, -- 头像ID
    GassID= 6, -- 社团或者组队 id
}

---@class EMoneyType
EMoneyType = {
    ---不是货币
    None = 0,
    ---大炮数量：消除列
    gun = 1,
    ---垂子数量：粉碎一个
    hammer = 2,
    ---弓箭数量：消除行
    bows = 3,
    ---帽子数量，随机值
    cap = 4,
    ---炸弹数量：以炸弹当前位置向外扩大2各消除
    bomb = 5,
    ---蜻蜓数量：优先随机查找障碍物消除
    dragonfly = 6,
    ---火箭数量：行或列消除
    rocket = 7,
    ---彩虹球数量：与交换的颜色属性，或随机的颜色属性消除
    rainbowBall = 8,
    ---金币数量：购买5次移动数
    gold = 9,
    ---生命值
    life = 10,

    ---大炮购买次数：消除列
    gunCount = 11,
    ---垂子购买次数：粉碎一个
    hammerCount = 12,
    ---弓箭购买次数：消除行
    bowsCount = 13,
    ---帽子购买次数，随机值
    capCount = 14,

    ---关卡等级
    level = 15,
    ---每一个生命持续时间：小时
    lifehour = 16,
    ---最大生命是=5，领取时不可以超过，完成任务时直接加时间
    lifeMax = 17,

    ---昵称购买次数
    nickCount = 18,
    ---生命购买次数
    lifeCount = 19,
    ---生命购免费次数
    lifeFree = 20,
    ---移动购买次数
    moveCount = 21,
    ---移动次数累计
    passMove = 22,
    ---最多可创建的数量
    metaExpendNum = 23,
    ---用户积分
    integral = 24
}


---货币按钮状态
---@class EMBtnState
EMBtnState = {
    ---不显示加号按钮
    None = 0,
    ---显示加号按钮
    Add = 1,
}

---货币栏配置
---@class MoneyView_Config
EMoneyView_Config = {
    --[ViewID.Main] = { [EMoneyType.Explore] = EMBtnState.None, [EMoneyType.Diamond] = EMBtnState.Add, [EMoneyType.Gold] = EMBtnState.None ,[EMoneyType.Enegry] = EMBtnState.None },
    --[ViewID.Shop] = {[EMoneyType.Gold] = EMBtnState.None, [EMoneyType.Diamond] = EMBtnState.None, [EMoneyType.Enegry] = EMBtnState.Add },
    --[ViewID.ShopEnergy] = { [EMoneyType.Gold] = EMBtnState.None, [EMoneyType.Enegry] = EMBtnState.None ,[EMoneyType.Diamond] = EMBtnState.Add},
    --[ViewID.Role] = {[EMoneyType.Explore] = EMBtnState.None },
    --[ViewID.MonthCard] = {[EMoneyType.Diamond] = EMBtnState.None },
    --[ViewID.Gift] = {[EMoneyType.Diamond] = EMBtnState.None },
}

---游戏声音
---@class ESoundResType
ESoundResType = {
    ---点击物品泡泡 √
    ClickBubble = 'Sound/bubble',
    ---点击 √
    ClickButton = 'Sound/button_click',
    ---购买金币成功
    BuyGoldClick = 'Sound/buy_glod',
    ---玻璃瓶破碎 √
    Bottle_Poshui = 'Sound/glass_poshui',
    ---物品炸弹 √
    item_bomb = 'Sound/item_bomb',
    ---物品火箭 √
    item_rocket = 'Sound/item_rocket',
    ---物品竹蜻蜓 √
    item_zhuqingting = 'Sound/item_zhuqingting',
    ---关卡失败 √
    LevelFail = 'Sound/level_fail',
    ---关卡胜利 √
    LevelWin = 'Sound/level_win',
    ---卡片声音 √
    Card = 'Sound/mail_card',
    ---合成新物品 √
    MergeNewItem = 'Sound/merge_newItem',
    ---物品破碎 √
    item_poshui = 'Sound/item_poshui',
    ---障碍物破碎 √
    Obstacle_poshui = 'Sound/Obstacle_poshui',
    ---主界面页签点击 √
    ClickPage = 'Sound/page_Click',
    ---支付和获取奖励的声音 √
    PayReward = 'Sound/pay_reward',
    ---加金币 √
    AddGold = 'Sound/sound_gold',
    ---使用道具 √
    UseProps = 'Sound/Use_Props',
    ---物品发射 √
    ItemMove = "Sound/item_move",

    ---关卡背景声音 √
    MusicLevel = "Music/Music_Level_",
    ---元宇宙副本
    MusicMeta ="Music/Music_Meta"
}

---游戏缓存的key
---@class EGameStoreKey
EGameSetting = {
    OfflineTime = "OfflineTime",
    ---声音部分
    MusicMute = "MusicMute",
    SoundMute = "SoundMute",
    MusicVolume = "MusicVolume",
    SoundVolume = "SoundVolume",

    BindFacebook = "BindFacebook",
}