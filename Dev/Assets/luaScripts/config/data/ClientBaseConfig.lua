--表名: 客户端常量表, 字段描述：_key:字段名, _name:名字, _value:数值, _value1:数值, _desc:描述, 
local M = {}
M["1"] = {key = "1", name = "relife_gold", value = "3000", value1 = "0", desc = "救济每次领取金币数量", }
M["2"] = {key = "2", name = "first_gold", value = "20000", value1 = "0", desc = "初始默认金币数量", }
M["3"] = {key = "3", name = "first_diamonds", value = "0", value1 = "0", desc = "初始默认钻石数量", }
M["4"] = {key = "4", name = "new_reward_lvl", value = "3", value1 = "0", desc = "2级后无法领取新人奖励", }
M["5"] = {key = "5", name = "share_new_reward", value = "10000", value1 = "0", desc = "分享填写邀请码新人奖励5钻石（改为1W金币）", }
M["6"] = {key = "6", name = "share_friend_reward", value = "5000", value1 = "0", desc = "每日分享朋友圈得5000金币", }
M["7"] = {key = "7", name = "revive", value = "10000", value1 = "0", desc = "花费1W金币红包场复活", }
LuaConfigMgr.ClientBaseConfigLen = 7
LuaConfigMgr.ClientBaseConfig = M