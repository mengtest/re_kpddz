--表名: 分享奖励, 字段描述：_key:好友进阶任务, _descirbe:描述 , _condition:条件, _reward:奖励, 
local M = {}
M["3"] = {key = "3", descirbe = "邀请的好友首次成功兑换1元红包", condition = "10", reward = {{"item", "102", "40", }, }, }
M["4"] = {key = "4", descirbe = "邀请的好友首次成功兑换1元红包", condition = "10", reward = {{"item", "102", "40", }, }, }
LuaConfigMgr.ShareRewardConfigLen = 2
LuaConfigMgr.ShareRewardConfig = M