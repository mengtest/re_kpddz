--表名: 免费金币, 字段描述：_key:ID, _icon:图标, _title:标题, _desc:描述, _skip_name:未完成按钮文字, _skip1_name:达成条件按钮文字, _skip2_name:完成按钮文字, _skip_id:未完成跳转界面ID, _skip_id1:满足条件需要处理, _skip_id2:完成跳转界面ID, _red:满足条件时是否显示红点, 
local M = {}
M["600002"] = {key = "600002", icon = "icon1", title = "每日登录礼包", desc = "[b1906e]每天登录游戏即可领取金币[-]", skip_name = "签到", skip1_name = "签到", skip2_name = "签到", skip_id = "1003", skip_id1 = "0", skip_id2 = "103", red = "1", }
M["600003"] = {key = "600003", icon = "icon2", title = "破产补助", desc = "[b1906e]金币不足[-][e36500]2000[-][b1906e]时可领取破产补助[-]", skip_name = "去游戏", skip1_name = "领取", skip2_name = "充值", skip_id = "113", skip_id1 = "1001", skip_id2 = "105", red = "0", }
M["600004"] = {key = "600004", icon = "icon3", title = "游戏任务", desc = "[b1906e]完成任务可获得大量金币[-]", skip_name = "查看任务", skip1_name = "查看任务", skip2_name = "查看任务", skip_id = "106", skip_id1 = "0", skip_id2 = "106", red = "0", }
M["600005"] = {key = "600005", icon = "icon4", title = "绑定手机号", desc = "[b1906e]绑定手机号，立即赠送[-][e36500]10000[-][b1906e]金币[-]", skip_name = "去绑定", skip1_name = "领取", skip2_name = "去绑定", skip_id = "107", skip_id1 = "1004", skip_id2 = "0", red = "0", }
M["600006"] = {key = "600006", icon = "icon5", title = "对局宝箱", desc = "[b1906e]完成游戏戏局即可获得金币奖励哦[-]", skip_name = "去游戏", skip1_name = "去游戏", skip2_name = "去游戏", skip_id = "104", skip_id1 = "0", skip_id2 = "104", red = "0", }
LuaConfigMgr.FreeConfigLen = 24
LuaConfigMgr.FreeConfig = M