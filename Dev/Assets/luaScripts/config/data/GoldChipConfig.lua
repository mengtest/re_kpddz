--表名: 豪车下注金额, 字段描述：_key:ID, _gold_chip:押注额, 
local M = {}
M["1"] = {key = "1", gold_chip = "1000", }
M["2"] = {key = "2", gold_chip = "10000", }
M["3"] = {key = "3", gold_chip = "50000", }
M["4"] = {key = "4", gold_chip = "100000", }
M["5"] = {key = "5", gold_chip = "500000", }
LuaConfigMgr.GoldChipConfigLen = 5
LuaConfigMgr.GoldChipConfig = M