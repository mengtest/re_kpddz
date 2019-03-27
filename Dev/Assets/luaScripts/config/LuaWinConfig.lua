-- --------------------------------------------------


-- *
-- * Summary: 	动态LuaWin配置表，用于代码中动态创建lua界面
-- * Version: 	1.0.0
-- * Author: 	chr
-- --------------------------------------------------


-- *************** 配置说明 *****************
--
-- 格式: { [key] = script }
-- key： 脚本绑定的GameObject名字，注意要唯一
-- script： 绑定的脚本，同make
--
-- *****************************************


-- 配置表（全局唯一，C#中使用）
local UILevel = {
    BACKGROUND = 0, --depth：-1000~-1
    NORMAL = 1, --depth:0~99999
    HIGHT = 2, --depth:100000~199999
    TOP = 3, --depth:200000~
    TOP_HIGHT = 4, --depth:300000~不到最后时刻勿用，切记切记
    TOP_TOP = 5, --depth:400000~不到最后时刻勿用，切记切记
}
___lua_win_table___ = {
    ["MainCenterWin"] = { UILevel.BACKGROUND, "UI/MainCity/MainCenterWin.prefab" },
    ["MainWin"] = { UILevel.NORMAL, "UI/MainCity/MainWin.prefab" },
    ["MainLoginTourisTip"] = { UILevel.TOP, "UI/MainCity/MainLoginTourisTip.prefab" },
    ["MainBugFeedBack"] = { UILevel.NORMAL, "UI/MainCity/MainBugFeedBack.prefab" },
    ["RoomLvSelectWin"] = { UILevel.NORMAL, "UI/Room/select/RoomLvSelectWin.prefab" },
    ["LuaTestWin"] = { UILevel.HIGHT, "UI/LuaTest/LuaTestWin.prefab" },
    ["NormalCowMain"] = { UILevel.NORMAL, "UI/PokerGame/normalCow/NormalCowMain.prefab", true },
    ["NormalCowUI"] = { UILevel.NORMAL, "UI/PokerGame/normalCow/NormalCowUI.prefab", true },
    ["NormalCowTop"] = { UILevel.NORMAL, "UI/PokerGame/normalCow/NormalCowTop.prefab", true },
    ["NormalCowRedUI"] = { UILevel.NORMAL, "UI/PokerGame/normalCow/NormalCowRedUI.prefab", true },
    ["OpenRedpackWin"] = { UILevel.HIGHT, "UI/PokerGame/normalCow/OpenRedpackWin.prefab", true },
    ["HundredCowMain"] = { UILevel.NORMAL, "UI/PokerGame/hundredCow/HundredCowMain.prefab", true },
    ["PIChangePassWordWin"] = { UILevel.NORMAL, "UI/PlayerInfo/PIChangePassWordWin.prefab" },
    ["PlayerInfoWin"] = { UILevel.NORMAL, "UI/PlayerInfo/PlayerInfoWin.prefab" },
    ["PlayerRankInfoWin"] = { UILevel.NORMAL, "UI/PlayerInfo/PlayerRankInfoWin.prefab" },
    ["GamePlayerInfoWin"] = { UILevel.NORMAL, "UI/PlayerInfo/GamePlayerInfoWin.prefab" },
    ["PlayerPicSelectWin"] = { UILevel.NORMAL, "UI/PlayerInfo/PlayerPicSelectWin.prefab" },
    ["PlayerInfoTipsWin"] = { UILevel.TOP, "UI/PlayerInfo/PlayerInfoTipsWin.prefab" },
    ["SubsidyWin"] = { UILevel.NORMAL, "UI/Subsidy/SubsidyWin.prefab" },
    ["AwardWin"] = { UILevel.TOP, "UI/Subsidy/AwardWin.prefab" },
    ["MailWin"] = { UILevel.NORMAL, "UI/Mail/MailWin.prefab" },
    ["FreeWin"] = { UILevel.NORMAL, "UI/Free/FreeWin.prefab" },
    ["ShopWin"] = { UILevel.NORMAL, "UI/Shop/ShopWin.prefab" },
    ["SettingWin"] = { UILevel.HIGHT, "UI/Setting/SettingWin.prefab" },
    ["HelpWin"] = { UILevel.HIGHT, "UI/Setting/HelpWin.prefab" },
    ["ChatMainWin"] = { UILevel.HIGHT, "UI/Chat/ChatMainWin.prefab" },
    ["TaskWin"] = { UILevel.NORMAL, "UI/Task/TaskWin.prefab" },
    ["OpenBoxWin"] = { UILevel.HIGHT, "UI/PokerGame/normalCow/OpenBoxWin.prefab" },
    ["TaskWin"] = { UILevel.NORMAL, "UI/Task/TaskWin.prefab" },
    ["ExchangeWin"] = { UILevel.NORMAL, "UI/Exchange/ExchangeWin.prefab" },
    ["AddressWin"] = { UILevel.NORMAL, "UI/Exchange/AddressWin.prefab" },
    ["ExchangeSureWin"] = { UILevel.NORMAL, "UI/Exchange/ExchangeSureWin.prefab" },
    ["RechargePhoneWin"] = { UILevel.NORMAL, "UI/Exchange/RechargePhoneWin.prefab" },
    ["RechargeTypeWin"] = { UILevel.NORMAL, "UI/Exchange/RechargeTypeWin.prefab" },
    ["RechargeCardPasswordWin"] = { UILevel.NORMAL, "UI/Exchange/RechargeCardPasswordWin.prefab" },
    ["RedBagWin"] = { UILevel.NORMAL, "UI/RedBag/RedBagWin.prefab" },
    ["RedBagGuessWin"] = { UILevel.NORMAL, "UI/RedBag/RedBagGuessWin.prefab" },
    ["RedBagSendWin"] = { UILevel.NORMAL, "UI/RedBag/RedBagSendWin.prefab" },
    ["LoginAwardWin"] = { UILevel.NORMAL, "UI/LoginAward/LoginAwardWin.prefab" },
    ["FastAddGoldWin"] = { UILevel.HIGHT, "UI/FastAddGold/FastAddGoldWin.prefab", true },
    ["ActivityAndAnnouncement"] = { UILevel.NORMAL, "UI/ActivityAndAnnouncement/ActivityAndAnnouncement.prefab" },
    ["ActivityRules"] = { UILevel.HIGHT, "UI/ActivityAndAnnouncement/ActivityRules.prefab"},

    ["FruitWin"] = {UILevel.NORMAL, "UI/Fruit/FruitWin.prefab"},
    ["FruitBoxWin"] = {UILevel.HIGHT, "UI/Fruit/FruitBoxWin.prefab"},
    ["FruitAwardWin"] = {UILevel.HIGHT, "UI/Fruit/FruitAwardWin.prefab"},
    ["FruitPoolWin"] = {UILevel.HIGHT, "UI/Fruit/FruitPoolWin.prefab"},
    ["BigRewardWin"] = {UILevel.HIGHT, "UI/Fruit/BigRewardWin.prefab"},
    ["LuckyCowWin"] = {UILevel.HIGHT, "UI/LuckyCow/LuckyCowWin.prefab"},
    ["PublicSignWin"] = {UILevel.TOP_HIGHT, "UI/Announcement/PublicSignWin.prefab"},
    ["GuideWin"] = {UILevel.TOP, "UI/Guide/GuideWin.prefab"},
    ["MonthCardWin"] = {UILevel.NORMAL, "UI/MonthCard/MonthCardWin.prefab"},
    ["MonthCardHelpWin"] = {UILevel.NORMAL, "UI/MonthCard/MonthCardHelpWin.prefab"},
    ["FirstPayWin"] = {UILevel.HIGHT, "UI/FirstPay/FirstPayWin.prefab"},
    ["ResultLayer"] = {UILevel.NORMAL, "UI/PokerGame/resultRes/ResultLayer.prefab"},
    ["ResultLoseLayer"] = {UILevel.NORMAL, "UI/PokerGame/resultRes/ResultLoseLayer.prefab"},
    ["RedConditionWin"] = {UILevel.NORMAL, "UI/Task/RedConditionWin.prefab"},
    ["DiamondBagWin"] = {UILevel.NORMAL, "UI/DiamondBag/DiamondBagWin.prefab"},

    ["RichCarMasterWin"] = {UILevel.NORMAL, "UI/RichCar/RichCarMasterWin.prefab"},
    ["RichCarRuleWin"] = {UILevel.NORMAL, "UI/RichCar/RichCarRuleWin.prefab"},
    ["RichCarWin"] = {UILevel.NORMAL, "UI/RichCar/RichCarWin.prefab"},

    -- 分享
    ["ShareWin"] = {UILevel.NORMAL, "UI/Share/ShareWin.prefab"},
    ["ShareSelectWin"] = {UILevel.NORMAL, "UI/Share/ShareSelectWin.prefab"},
    ["ShareFriendWin"] = {UILevel.NORMAL, "UI/Share/ShareFriendWin.prefab"},
    ["ShareExchangeWin"] = {UILevel.NORMAL, "UI/Share/ShareExchangeWin.prefab"},
    ["ShareTipsWin"] = {UILevel.NORMAL, "UI/Share/ShareTipsWin.prefab"},
    ["RankWin"] = {UILevel.NORMAL, "UI/PokerGame/hundredCow/RankWin.prefab"},
    ["RankInfoWin"] = {UILevel.NORMAL, "UI/PokerGame/hundredCow/RankInfoWin.prefab"},
    ["GameCenterWin"] = {UILevel.NORMAL, "UI/MainCity/GameCenterWin.prefab"},
    ["GoldPoolWin"] = {UILevel.NORMAL, "UI/GoldPool/GoldPoolWin.prefab"},
    ["GoldPoolRuleWin"] = {UILevel.NORMAL, "UI/GoldPool/GoldPoolRuleWin.prefab"},
    ["ExchangeTipWin"] = {UILevel.NORMAL, "UI/Exchange/ExchangeTipWin.prefab"},
    ["RechargeQBWin"] = {UILevel.NORMAL, "UI/Exchange/RechargeQBWin.prefab"},
    ["NewShareWin"] = {UILevel.NORMAL, "UI/Share/NewShareWin.prefab"},
    ["SevenDailyTaskWin"] = {UILevel.NORMAL, "UI/DailyTask/SevenDailyTaskWin.prefab"},
    ["UserIDCardBindWin"] = {UILevel.NORMAL, "UI/Login/UserIDCardBindWin.prefab"},
    ["UserAgreeWin"] = {UILevel.HIGHT, "UI/Login/UserAgreeWin.prefab"},
    ["RechargeTaskWin"] = {UILevel.NORMAL, "UI/RechargeTask/RechargeTaskWin.prefab"},

    ["FruitSuperAwardWin"] = {UILevel.NORMAL, "UI/Fruit/FruitSuperAwardWin.prefab"},
    ["FruitRankWin"] = {UILevel.NORMAL, "UI/Fruit/FruitRankWin.prefab"}
    
}

 