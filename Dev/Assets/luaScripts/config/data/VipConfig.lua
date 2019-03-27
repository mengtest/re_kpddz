--表名: VIP, 字段描述：_key:排行区间, _next_lv:下一等级, _need_gold:升级需求, _title1:称呼, _title2:称号, _desc:VIP描述, _reward:每日签到赠送, _open_function:功能开启, _desc2:商城VIP特权显示描述, 
local M = {}
M["1"] = {key = "1", next_lv = "2", need_gold = "29", title1 = "牛1", title2 = "VIP1", desc = "1", reward = {{"item", "101", "2000", }, }, open_function = "caihongbao", desc2 = "1", }
M["2"] = {key = "2", next_lv = "3", need_gold = "69", title1 = "牛2", title2 = "VIP2", desc = "2", reward = {{"item", "101", "2800", }, }, open_function = "", desc2 = "2", }
M["3"] = {key = "3", next_lv = "4", need_gold = "159", title1 = "牛3", title2 = "VIP3", desc = "3", reward = {{"item", "101", "3800", }, }, open_function = "huahongbao", desc2 = "3", }
M["4"] = {key = "4", next_lv = "5", need_gold = "389", title1 = "牛4", title2 = "VIP4", desc = "4", reward = {{"item", "101", "5800", }, }, open_function = "", desc2 = "4", }
M["5"] = {key = "5", next_lv = "6", need_gold = "799", title1 = "牛5", title2 = "VIP5", desc = "5", reward = {{"item", "101", "8800", }, }, open_function = "", desc2 = "5", }
M["6"] = {key = "6", next_lv = "7", need_gold = "1599", title1 = "牛6", title2 = "VIP6", desc = "6", reward = {{"item", "101", "12800", }, }, open_function = "", desc2 = "6", }
M["7"] = {key = "7", next_lv = "8", need_gold = "3699", title1 = "牛7", title2 = "VIP7", desc = "7", reward = {{"item", "101", "22800", }, }, open_function = "", desc2 = "7", }
M["8"] = {key = "8", next_lv = "9", need_gold = "8899", title1 = "牛8", title2 = "VIP8", desc = "8", reward = {{"item", "101", "38000", }, }, open_function = "", desc2 = "8", }
M["9"] = {key = "9", next_lv = "10", need_gold = "18899", title1 = "牛9", title2 = "VIP9", desc = "9", reward = {{"item", "101", "50000", }, }, open_function = "", desc2 = "9", }
M["10"] = {key = "10", next_lv = "0", need_gold = "58888", title1 = "牛10", title2 = "VIP10", desc = "10", reward = {{"item", "101", "88000", }, }, open_function = "", desc2 = "10", }
LuaConfigMgr.VipConfigLen = 10
LuaConfigMgr.VipConfig = M