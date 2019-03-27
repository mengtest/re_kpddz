--权重,图标
--表名: 豪车倍率表, 字段描述：_key:ID, _name:名称, _icon:倍率, 
local M = {}
M["1"] = {key = "1", name = "宝马", icon = "H101", }
M["2"] = {key = "2", name = "奔驰", icon = "H102", }
M["3"] = {key = "3", name = "奥迪", icon = "H103", }
M["4"] = {key = "4", name = "路虎", icon = "H104", }
M["5"] = {key = "5", name = "法拉利", icon = "H105", }
M["6"] = {key = "6", name = "劳斯莱斯", icon = "H106", }
M["7"] = {key = "7", name = "宾利", icon = "H107", }
M["8"] = {key = "8", name = "兰博基尼", icon = "H108", }
LuaConfigMgr.CardPowerConfigLen = 8
LuaConfigMgr.CardPowerConfig = M