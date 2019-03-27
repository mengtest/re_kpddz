--商品纹理
--表名: 活动数据, 字段描述：_key:Id, _name:活动描述, _activity_type:活动类型, _data1:数据条件, _reward2:奖励物品id, _tex:每日可重复领取, 
local M = {}
M["101"] = {key = "101", name = "签到1天", activity_type = "1", data1 = "1", reward2 = {{"", }, }, tex = {{"gold2", }, }, }
M["102"] = {key = "102", name = "签到2天", activity_type = "1", data1 = "2", reward2 = {{"", }, }, tex = {{"gold2", }, }, }
M["103"] = {key = "103", name = "签到3天", activity_type = "1", data1 = "3", reward2 = {{"", }, }, tex = {{"gold3", }, }, }
M["104"] = {key = "104", name = "签到4天", activity_type = "1", data1 = "4", reward2 = {{"", }, }, tex = {{"gold3", }, }, }
M["105"] = {key = "105", name = "签到5天", activity_type = "1", data1 = "5", reward2 = {{"", }, }, tex = {{"gold4", }, }, }
M["106"] = {key = "106", name = "签到6天", activity_type = "1", data1 = "6", reward2 = {{"", }, }, tex = {{"gold4", }, }, }
M["107"] = {key = "107", name = "签到7天", activity_type = "1", data1 = "7", reward2 = {{"", }, }, tex = {{"gold5", }, }, }
LuaConfigMgr.ActivityConfigLen = 9
LuaConfigMgr.ActivityConfig = M