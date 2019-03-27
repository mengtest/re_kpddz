--表名: 物品基础, 字段描述：_key:编号, _name:名称, _desc:物品描述, _icon:物品图标, _phase:品质, _cls:物品类型, _level_require:等级限制, _buy_gold:购买价格, _price:出售价格, _max_count:最大叠加, _tips:TIP信息, 
local M = {}
M["101"] = {key = "101", name = "金币", desc = "游戏币，用于游戏内消耗", icon = "C101", phase = "2", cls = "0", level_require = "1", buy_gold = "100", price = "10", max_count = "999999999", tips = "游戏币，用于游戏内消耗", }
M["102"] = {key = "102", name = "钻石", desc = "游戏币，用于购买赠送礼物等", icon = "C102", phase = "4", cls = "0", level_require = "1", buy_gold = "100", price = "10", max_count = "999999999", tips = "游戏币，用于购买赠送礼物等", }
M["103"] = {key = "103", name = "奖券", desc = "奖券，可在领奖中心领取各种丰厚奖品", icon = "C103", phase = "4", cls = "0", level_require = "1", buy_gold = "100", price = "10", max_count = "999999999", tips = "奖券，可在领奖中心领取各种丰厚奖品", }
M["201"] = {key = "201", name = "经验", desc = "经验", icon = "C201", phase = "4", cls = "0", level_require = "1", buy_gold = "100", price = "10", max_count = "999999999", tips = "经验", }
M["202"] = {key = "202", name = "充值rmb", desc = "充值rmb", icon = "C202", phase = "4", cls = "0", level_require = "1", buy_gold = "100", price = "10", max_count = "999999999", tips = "充值rmb", }
M["107"] = {key = "107", name = "话费券", desc = "可在领奖中心领取话费", icon = "C107", phase = "4", cls = "0", level_require = "1", buy_gold = "100", price = "10", max_count = "999999999", tips = "可在领奖中心领取话费", }
M["109"] = {key = "109", name = "元红包", desc = "可在领奖中心领取红包", icon = "C109", phase = "4", cls = "0", level_require = "1", buy_gold = "100", price = "10", max_count = "999999999", tips = "可在领奖中心领取红包", }
M["100001"] = {key = "100001", name = "改名卡", desc = "可用于修改游戏名字", icon = "C100001", phase = "2", cls = "1", level_require = "1", buy_gold = "100", price = "10", max_count = "99", tips = "可用于修改游戏名字", }
M["100002"] = {key = "100002", name = "补签卡", desc = "用于签到的补签", icon = "C100002", phase = "2", cls = "1", level_require = "1", buy_gold = "100", price = "10", max_count = "99", tips = "用于签到的补签", }
LuaConfigMgr.ItemBaseConfigLen = 9
LuaConfigMgr.ItemBaseConfig = M