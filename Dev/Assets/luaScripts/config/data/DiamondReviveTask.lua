--跳转界面ID
--表名: 红包场复活任务, 字段描述：_key:ID, _icon:图标, _title:开启等级, _achieve_conditicon:任务标题, _achieve_type:数组, _parameter1:活动类型, _parameter2:参数, _parameter3:参数, _item1_id:条件参数, _item1_num:奖励物品1ID, _item2_id:奖励物品1数量, _item2_num:奖励物品2ID, _desc:奖励物品2数量, _skip_id:描述, 
local M = {}
M["710005"] = {key = "710005", icon = "T1001", title = "百人大战累计押注50000\n可免费复活一次", achieve_conditicon = "16,1,0,50000", achieve_type = {{"16", }, }, parameter1 = "1", parameter2 = "0", parameter3 = "50000", item1_id = {{"0", }, }, item1_num = "0", item2_id = "0", item2_num = "0", desc = "百人大战累计押注50000\n可免费复活一次", skip_id = "101", }
LuaConfigMgr.DiamondReviveTaskLen = 39
LuaConfigMgr.DiamondReviveTask = M