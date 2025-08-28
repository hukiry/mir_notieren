---
---Update Time:2024-8-8
---Author:Hukiry
---

---@class ViewID
ViewID = {
	---@type ChestView
	Chest = 1,
	---@type DrawcardView
	Drawcard = 2,
	---@type PropsView
	Props = 3,
	---@type RechargeFirstView
	RechargeFirst = 4,
	---@type GiftView
	Gift = 5,
	---@type GmView
	Gm = 6,
	---@type GmWinView
	GmWin = 7,
	---@type GuideView
	Guide = 8,
	---@type LevelMainView
	LevelMain = 9,
	---@type LevelChallengeView
	LevelChallenge = 10,
	---@type LevelFailView
	LevelFail = 11,
	---@type LevelQuitView
	LevelQuit = 12,
	---@type LevelWinView
	LevelWin = 13,
	---@type LoadingView
	Loading = 14,
	---@type MailTipView
	MailTip = 15,
	---@type BrowseView
	Browse = 16,
	---@type GameView
	Game = 17,
	---@type LifeView
	Life = 18,
	---@type FriendView
	Friend = 19,
	---@type MetaEditorView
	MetaEditor = 20,
	---@type MetaHomeView
	MetaHome = 21,
	---@type MetaSelectView
	MetaSelect = 22,
	---@type MetaEnterView
	MetaEnter = 23,
	---@type MetaTipView
	MetaTip = 24,
	---@type MyMapCreateView
	MyMapCreate = 25,
	---@type MyMapExpandView
	MyMapExpand = 26,
	---@type MyMapFightView
	MyMapFight = 27,
	---@type MyMapInfoView
	MyMapInfo = 28,
	---@type MyMapSettingView
	MyMapSetting = 29,
	---@type MyMapTipView
	MyMapTip = 30,
	---@type MetaFailView
	MetaFail = 31,
	---@type MetaWinView
	MetaWin = 32,
	---@type PassPayView
	PassPay = 33,
	---@type PassView
	Pass = 34,
	---@type BuyTipView
	BuyTip = 35,
	---@type CommonTipView
	CommonTip = 36,
	---@type PromptBoxView
	PromptBox = 37,
	---@type PropsTipView
	PropsTip = 38,
	---@type ServerTipView
	ServerTip = 39,
	---@type FeedfaceView
	Feedface = 40,
	---@type LanageView
	Lanage = 41,
	---@type RoleView
	Role = 42,
	---@type SettingView
	Setting = 43,
	---@type UserView
	User = 44,
	---@type RechargeView
	Recharge = 45,
	---@type SignMonthView
	SignMonth = 46,
	---@type TaskView
	Task = 47,
}

---@class UIConfigView
UIConfigView = {
	[ViewID.Chest] = { classPath = "Game/UI/ActivityView/ChestView", resPath = "prefab/UI/ActivityView/ChestView"},
	[ViewID.Drawcard] = { classPath = "Game/UI/ActivityView/DrawcardView", resPath = "prefab/UI/ActivityView/DrawcardView"},
	[ViewID.Props] = { classPath = "Game/UI/ActivityView/PropsView", resPath = "prefab/UI/ActivityView/PropsView"},
	[ViewID.RechargeFirst] = { classPath = "Game/UI/ActivityView/RechargeFirstView", resPath = "prefab/UI/ActivityView/RechargeFirstView"},
	[ViewID.Gift] = { classPath = "Game/UI/Gift/GiftView", resPath = "prefab/UI/Gift/GiftView"},
	[ViewID.Gm] = { classPath = "Game/UI/Gm/GmView", resPath = "prefab/UI/Gm/GmView"},
	[ViewID.GmWin] = { classPath = "Game/UI/Gm/GmWinView", resPath = "prefab/UI/Gm/GmWinView"},
	[ViewID.Guide] = { classPath = "Game/UI/Guide/GuideView", resPath = "prefab/UI/Guide/GuideView"},
	[ViewID.LevelMain] = { classPath = "Game/UI/Level/LevelMainView", resPath = "prefab/UI/Level/LevelMainView"},
	[ViewID.LevelChallenge] = { classPath = "Game/UI/LevelTip/LevelChallengeView", resPath = "prefab/UI/LevelTip/LevelChallengeView"},
	[ViewID.LevelFail] = { classPath = "Game/UI/LevelTip/LevelFailView", resPath = "prefab/UI/LevelTip/LevelFailView"},
	[ViewID.LevelQuit] = { classPath = "Game/UI/LevelTip/LevelQuitView", resPath = "prefab/UI/LevelTip/LevelQuitView"},
	[ViewID.LevelWin] = { classPath = "Game/UI/LevelTip/LevelWinView", resPath = "prefab/UI/LevelTip/LevelWinView"},
	[ViewID.Loading] = { classPath = "Game/UI/Loading/LoadingView", resPath = "prefab/UI/Loading/LoadingView"},
	[ViewID.MailTip] = { classPath = "Game/UI/Mail/MailTipView", resPath = "prefab/UI/Mail/MailTipView"},
	[ViewID.Browse] = { classPath = "Game/UI/Main/BrowseView", resPath = "prefab/UI/Main/BrowseView"},
	[ViewID.Game] = { classPath = "Game/UI/Main/GameView", resPath = "prefab/UI/Main/GameView"},
	[ViewID.Life] = { classPath = "Game/UI/Main/LifeView", resPath = "prefab/UI/Main/LifeView"},
	[ViewID.Friend] = { classPath = "Game/UI/Main/Panel/FriendView", resPath = "prefab/UI/Friend/FriendView"},
	[ViewID.MetaEditor] = { classPath = "Game/UI/Metauniverse/MetaEditorView", resPath = "prefab/UI/Metauniverse/MetaEditorView"},
	[ViewID.MetaHome] = { classPath = "Game/UI/Metauniverse/MetaHomeView", resPath = "prefab/UI/Metauniverse/MetaHomeView"},
	[ViewID.MetaSelect] = { classPath = "Game/UI/Metauniverse/MetaSelectView", resPath = "prefab/UI/Metauniverse/MetaSelectView"},
	[ViewID.MetaEnter] = { classPath = "Game/UI/Metauniverse/Tip/MetaEnterView", resPath = "prefab/UI/Metauniverse/Tip/MetaEnterView"},
	[ViewID.MetaTip] = { classPath = "Game/UI/Metauniverse/Tip/MetaTipView", resPath = "prefab/UI/Metauniverse/Tip/MetaTipView"},
	[ViewID.MyMapCreate] = { classPath = "Game/UI/Metauniverse/Tip/MyMapCreateView", resPath = "prefab/UI/Metauniverse/Tip/MyMapCreateView"},
	[ViewID.MyMapExpand] = { classPath = "Game/UI/Metauniverse/Tip/MyMapExpandView", resPath = "prefab/UI/Metauniverse/Tip/MyMapExpandView"},
	[ViewID.MyMapFight] = { classPath = "Game/UI/Metauniverse/Tip/MyMapFightView", resPath = "prefab/UI/Metauniverse/Tip/MyMapFightView"},
	[ViewID.MyMapInfo] = { classPath = "Game/UI/Metauniverse/Tip/MyMapInfoView", resPath = "prefab/UI/Metauniverse/Tip/MyMapInfoView"},
	[ViewID.MyMapSetting] = { classPath = "Game/UI/Metauniverse/Tip/MyMapSettingView", resPath = "prefab/UI/Metauniverse/Tip/MyMapSettingView"},
	[ViewID.MyMapTip] = { classPath = "Game/UI/Metauniverse/Tip/MyMapTipView", resPath = "prefab/UI/Metauniverse/Tip/MyMapTipView"},
	[ViewID.MetaFail] = { classPath = "Game/UI/Metauniverse/WinFail/MetaFailView", resPath = "prefab/UI/Metauniverse/WinFail/MetaFailView"},
	[ViewID.MetaWin] = { classPath = "Game/UI/Metauniverse/WinFail/MetaWinView", resPath = "prefab/UI/Metauniverse/WinFail/MetaWinView"},
	[ViewID.PassPay] = { classPath = "Game/UI/Pass/PassPayView", resPath = "prefab/UI/Pass/PassPayView"},
	[ViewID.Pass] = { classPath = "Game/UI/Pass/PassView", resPath = "prefab/UI/Pass/PassView"},
	[ViewID.BuyTip] = { classPath = "Game/UI/PromptBox/BuyTipView", resPath = "prefab/UI/PromptBox/BuyTipView"},
	[ViewID.CommonTip] = { classPath = "Game/UI/PromptBox/CommonTipView", resPath = "prefab/UI/PromptBox/CommonTipView"},
	[ViewID.PromptBox] = { classPath = "Game/UI/PromptBox/PromptBoxView", resPath = "prefab/UI/PromptBox/PromptBoxView"},
	[ViewID.PropsTip] = { classPath = "Game/UI/PromptBox/PropsTipView", resPath = "prefab/UI/PromptBox/PropsTipView"},
	[ViewID.ServerTip] = { classPath = "Game/UI/PromptBox/ServerTipView", resPath = "prefab/UI/PromptBox/ServerTipView"},
	[ViewID.Feedface] = { classPath = "Game/UI/Setting/FeedfaceView", resPath = "prefab/UI/Setting/FeedfaceView"},
	[ViewID.Lanage] = { classPath = "Game/UI/Setting/LanageView", resPath = "prefab/UI/Setting/LanageView"},
	[ViewID.Role] = { classPath = "Game/UI/Setting/RoleView", resPath = "prefab/UI/Setting/RoleView"},
	[ViewID.Setting] = { classPath = "Game/UI/Setting/SettingView", resPath = "prefab/UI/Setting/SettingView"},
	[ViewID.User] = { classPath = "Game/UI/Setting/UserView", resPath = "prefab/UI/Setting/UserView"},
	[ViewID.Recharge] = { classPath = "Game/UI/Shop/RechargeView", resPath = "prefab/UI/Shop/RechargeView"},
	[ViewID.SignMonth] = { classPath = "Game/UI/Sign/SignMonthView", resPath = "prefab/UI/Sign/SignMonthView"},
	[ViewID.Task] = { classPath = "Game/UI/Task/TaskView", resPath = "prefab/UI/Task/TaskView"},
}

---@class UIItemType
UIItemType = {
	---@type ChestItem
	ChestItem = 1,
	---@type DrawcardItem
	DrawcardItem = 2,
	---@type ViewFlyItem
	ViewFlyItem = 3,
	---@type ChatItem
	ChatItem = 4,
	---@type GiftItem
	GiftItem = 5,
	---@type RewardGitItem
	RewardGitItem = 6,
	---@type TargetItem
	TargetItem = 7,
	---@type PropsItem
	PropsItem = 8,
	---@type MailItem
	MailItem = 9,
	---@type FriendItem
	FriendItem = 10,
	---@type HeadItem
	HeadItem = 11,
	---@type RankItem
	RankItem = 12,
	---@type LifeHelpItem
	LifeHelpItem = 13,
	---@type GraphicCardItem
	GraphicCardItem = 14,
	---@type GraphicFightItem
	GraphicFightItem = 15,
	---@type SelectCardItem
	SelectCardItem = 16,
	---@type TargetEditorItem
	TargetEditorItem = 17,
	---@type PassItem
	PassItem = 18,
	---@type LanButtonItem
	LanButtonItem = 19,
	---@type RewardItem
	RewardItem = 20,
	---@type ShopCoinItem
	ShopCoinItem = 21,
	---@type ShopItem
	ShopItem = 22,
	---@type SignItem
	SignItem = 23,
	---@type TaskItem
	TaskItem = 24,
	---@type TipItem
	TipItem = 25,
}

---@class ItemPoolRule
ItemPoolRule = {
	[UIItemType.ChestItem] = { itemClass = "Game/UI/ActivityView/Item/ChestItem", resPath = "prefab/UI/ActivityView/ChestItem"},
	[UIItemType.DrawcardItem] = { itemClass = "Game/UI/ActivityView/Item/DrawcardItem", resPath = "prefab/UI/ActivityView/DrawcardItem"},
	[UIItemType.ViewFlyItem] = { itemClass = "Game/UI/Animation/Item/ViewFlyItem", resPath = "prefab/UI/Animation/ViewFlyItem"},
	[UIItemType.ChatItem] = { itemClass = "Game/UI/Friend/Item/ChatItem", resPath = "prefab/UI/Friend/ChatItem"},
	[UIItemType.GiftItem] = { itemClass = "Game/UI/Gift/Item/GiftItem", resPath = "prefab/UI/Gift/GiftItem"},
	[UIItemType.RewardGitItem] = { itemClass = "Game/UI/Gift/Item/RewardGitItem", resPath = "prefab/UI/Gift/RewardGitItem"},
	[UIItemType.TargetItem] = { itemClass = "Game/UI/Level/Item/TargetItem", resPath = "prefab/UI/Level/TargetItem"},
	[UIItemType.PropsItem] = { itemClass = "Game/UI/LevelTip/Item/PropsItem", resPath = "prefab/UI/LevelTip/PropsItem"},
	[UIItemType.MailItem] = { itemClass = "Game/UI/Mail/Item/MailItem", resPath = "prefab/UI/Mail/MailItem"},
	[UIItemType.FriendItem] = { itemClass = "Game/UI/Main/Item/FriendItem", resPath = "prefab/UI/Main/FriendItem"},
	[UIItemType.HeadItem] = { itemClass = "Game/UI/Main/Item/HeadItem", resPath = "prefab/UI/Main/HeadItem"},
	[UIItemType.RankItem] = { itemClass = "Game/UI/Main/Item/RankItem", resPath = "prefab/UI/Main/RankItem"},
	[UIItemType.LifeHelpItem] = { itemClass = "Game/UI/Mass/Item/LifeHelpItem", resPath = "prefab/UI/Mass/LifeHelpItem"},
	[UIItemType.GraphicCardItem] = { itemClass = "Game/UI/Metauniverse/Item/GraphicCardItem", resPath = "prefab/UI/Metauniverse/Item/GraphicCardItem"},
	[UIItemType.GraphicFightItem] = { itemClass = "Game/UI/Metauniverse/Item/GraphicFightItem", resPath = "prefab/UI/Metauniverse/Item/GraphicFightItem"},
	[UIItemType.SelectCardItem] = { itemClass = "Game/UI/Metauniverse/Item/SelectCardItem", resPath = "prefab/UI/Metauniverse/Item/SelectCardItem"},
	[UIItemType.TargetEditorItem] = { itemClass = "Game/UI/Metauniverse/Item/TargetEditorItem", resPath = "prefab/UI/Metauniverse/Item/TargetEditorItem"},
	[UIItemType.PassItem] = { itemClass = "Game/UI/Pass/Item/PassItem", resPath = "prefab/UI/Pass/PassItem"},
	[UIItemType.LanButtonItem] = { itemClass = "Game/UI/Setting/Item/LanButtonItem", resPath = "prefab/UI/Setting/LanButtonItem"},
	[UIItemType.RewardItem] = { itemClass = "Game/UI/Shop/Item/RewardItem", resPath = "prefab/UI/Shop/RewardItem"},
	[UIItemType.ShopCoinItem] = { itemClass = "Game/UI/Shop/Item/ShopCoinItem", resPath = "prefab/UI/Shop/ShopCoinItem"},
	[UIItemType.ShopItem] = { itemClass = "Game/UI/Shop/Item/ShopItem", resPath = "prefab/UI/Shop/ShopItem"},
	[UIItemType.SignItem] = { itemClass = "Game/UI/Sign/Item/SignItem", resPath = "prefab/UI/Sign/SignItem"},
	[UIItemType.TaskItem] = { itemClass = "Game/UI/Task/Item/TaskItem", resPath = "prefab/UI/Task/TaskItem"},
	[UIItemType.TipItem] = { itemClass = "Game/UI/Tip/Item/TipItem", resPath = "prefab/UI/Tip/TipItem"},
}

---UI面板类型
---@class UIPanelType
UIPanelType = {
	---@type FightMapPanel
	FightMapPanel = 1,
	---@type MyMapPanel
	MyMapPanel = 2,
	---@type PlayerMapPanel
	PlayerMapPanel = 3,
	---@type QuitMapPanel
	QuitMapPanel = 4,
	---@type SettingMapPanel
	SettingMapPanel = 5,
}

---切换页签动态加载的面板
---@class UIPanelRule
UIPanelRule = {
	[UIPanelType.FightMapPanel] = { itemClass = "Game/UI/Metauniverse/Panel/FightMapPanel", resPath = "prefab/UI/Metauniverse/Panel/FightMapPanel"},
	[UIPanelType.MyMapPanel] = { itemClass = "Game/UI/Metauniverse/Panel/MyMapPanel", resPath = "prefab/UI/Metauniverse/Panel/MyMapPanel"},
	[UIPanelType.PlayerMapPanel] = { itemClass = "Game/UI/Metauniverse/Panel/PlayerMapPanel", resPath = "prefab/UI/Metauniverse/Panel/PlayerMapPanel"},
	[UIPanelType.QuitMapPanel] = { itemClass = "Game/UI/Metauniverse/Panel/QuitMapPanel", resPath = "prefab/UI/Metauniverse/Panel/QuitMapPanel"},
	[UIPanelType.SettingMapPanel] = { itemClass = "Game/UI/Metauniverse/Panel/SettingMapPanel", resPath = "prefab/UI/Metauniverse/Panel/SettingMapPanel"},
}

