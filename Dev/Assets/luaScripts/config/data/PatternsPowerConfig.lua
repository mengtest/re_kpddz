--表名: 牌型倍率, 字段描述：_key:编号, _name:名称, _commision:倍率, 
local M = {}
M["1"] = {key = "1", name = "五小牛", commision = "8", }
M["2"] = {key = "2", name = "五花牛", commision = "5", }
M["3"] = {key = "3", name = "四炸", commision = "4", }
M["4"] = {key = "4", name = "牛牛", commision = "3", }
M["5"] = {key = "5", name = "牛9", commision = "2", }
M["6"] = {key = "6", name = "牛8", commision = "2", }
M["7"] = {key = "7", name = "牛7", commision = "2", }
M["8"] = {key = "8", name = "牛6", commision = "1", }
M["9"] = {key = "9", name = "牛5", commision = "1", }
M["10"] = {key = "10", name = "牛4", commision = "1", }
M["11"] = {key = "11", name = "牛3", commision = "1", }
M["12"] = {key = "12", name = "牛2", commision = "1", }
M["13"] = {key = "13", name = "牛1", commision = "1", }
M["14"] = {key = "14", name = "没牛", commision = "1", }
LuaConfigMgr.PatternsPowerConfigLen = 14
LuaConfigMgr.PatternsPowerConfig = M