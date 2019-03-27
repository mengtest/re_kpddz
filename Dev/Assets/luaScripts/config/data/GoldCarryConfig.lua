--表名: 上庄筹码, 字段描述：_key:ID, _gold_carry:上庄携带金额, 
local M = {}
M["1"] = {key = "1", gold_carry = "20000000", }
M["2"] = {key = "2", gold_carry = "30000000", }
M["3"] = {key = "3", gold_carry = "50000000", }
M["4"] = {key = "4", gold_carry = "100000000", }
M["5"] = {key = "5", gold_carry = "500000000", }
LuaConfigMgr.GoldCarryConfigLen = 5
LuaConfigMgr.GoldCarryConfig = M