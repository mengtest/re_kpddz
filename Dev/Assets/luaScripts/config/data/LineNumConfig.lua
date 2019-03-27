--表名: 单线投注, 字段描述：_key:排行区间, _gold_bet:单线投注, _rate1:3个7彩池, _rate2:4个7彩池, _rate3:5个7彩池, 
local M = {}
M["1"] = {key = "1", gold_bet = "100", rate1 = "0", rate2 = "0", rate3 = "0", }
M["2"] = {key = "2", gold_bet = "500", rate1 = "0", rate2 = "0", rate3 = "0", }
M["3"] = {key = "3", gold_bet = "2500", rate1 = "0.2", rate2 = "0.4", rate3 = "0.8", }
M["4"] = {key = "4", gold_bet = "5000", rate1 = "0.4", rate2 = "0.8", rate3 = "1.6", }
M["5"] = {key = "5", gold_bet = "25000", rate1 = "3", rate2 = "5", rate3 = "8", }
M["6"] = {key = "6", gold_bet = "50000", rate1 = "6", rate2 = "10", rate3 = "16", }
M["7"] = {key = "7", gold_bet = "250000", rate1 = "20", rate2 = "25", rate3 = "30", }
M["8"] = {key = "8", gold_bet = "500000", rate1 = "20", rate2 = "40", rate3 = "60", }
LuaConfigMgr.LineNumConfigLen = 8
LuaConfigMgr.LineNumConfig = M