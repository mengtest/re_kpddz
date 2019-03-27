--表名: 活动数据标题表, 字段描述：_key:ID, _name:活动名字, _tag:活动标签, _mainTitle:主标题, _subTitle:副标题, _dataTitle:数据统计标题, _imgAd:广告图, _rules:活动规则, _activityTable:对应的活动表名, 
local M = {}
M["1"] = {key = "1", name = "超级水果中大奖", tag = "2", mainTitle = "tt_superfruit", subTitle = "", dataTitle = "", imgAd = "ad_superfruit", rules = "", activityTable = "", }
M["2"] = {key = "3", name = "大家都在领红包", tag = "1", mainTitle = "tt_qhb", subTitle = "", dataTitle = "", imgAd = "ad_qhb", rules = "", activityTable = "", }
M["3"] = {key = "4", name = "钻石特惠买就送", tag = "0", mainTitle = "tt_buydiamond", subTitle = "", dataTitle = "", imgAd = "ad_buydiamond", rules = "", activityTable = "", }
M["4"] = {key = "5", name = "红包兑换流程", tag = "0", mainTitle = "tt_redpacket", subTitle = "", dataTitle = "", imgAd = "ad_redpacket", rules = "", activityTable = "", }
LuaConfigMgr.ActivityTitleConfigLen = 4
LuaConfigMgr.ActivityTitleConfig = M