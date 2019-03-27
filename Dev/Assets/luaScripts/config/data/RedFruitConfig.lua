--奖励金币数量,描述
--表名: 水果狂欢红包奖励, 字段描述：_key:ID, _total_gold:ID, _red_packet:类型, _describe:拉霸累计获得金币数, 
local M = {}
M["1"] = {key = "1", total_gold = "200000", red_packet = "5000", describe = "水果狂欢累计赢金20万金币", }
M["2"] = {key = "2", total_gold = "1000000", red_packet = "10000", describe = "水果狂欢累计赢金100万金币", }
M["3"] = {key = "3", total_gold = "5000000", red_packet = "50000", describe = "水果狂欢累计赢金500万金币", }
M["4"] = {key = "4", total_gold = "10000000", red_packet = "80000", describe = "水果狂欢累计赢金1000万金币", }
M["5"] = {key = "5", total_gold = "30000000", red_packet = "200000", describe = "水果狂欢累计赢金3000万金币", }
M["6"] = {key = "6", total_gold = "50000000", red_packet = "200000", describe = "水果狂欢累计赢金5000万金币", }
M["7"] = {key = "7", total_gold = "100000000", red_packet = "400000", describe = "水果狂欢累计赢金1亿金币", }
M["8"] = {key = "8", total_gold = "200000000", red_packet = "600000", describe = "水果狂欢累计赢金2亿金币", }
M["9"] = {key = "9", total_gold = "400000000", red_packet = "800000", describe = "水果狂欢累计赢金4亿金币", }
M["10"] = {key = "10", total_gold = "600000000", red_packet = "800000", describe = "水果狂欢累计赢金6亿金币", }
M["11"] = {key = "11", total_gold = "800000000", red_packet = "800000", describe = "水果狂欢累计赢金8亿金币", }
M["12"] = {key = "12", total_gold = "1000000000", red_packet = "800000", describe = "水果狂欢累计赢金10亿金币", }
M["13"] = {key = "13", total_gold = "1300000000", red_packet = "1200000", describe = "水果狂欢累计赢金13亿金币", }
M["14"] = {key = "14", total_gold = "1600000000", red_packet = "1200000", describe = "水果狂欢累计赢金16亿金币", }
M["15"] = {key = "15", total_gold = "2000000000", red_packet = "1500000", describe = "水果狂欢累计赢金20亿金币", }
M["16"] = {key = "16", total_gold = "2500000000", red_packet = "1800000", describe = "水果狂欢累计赢金25亿金币", }
M["17"] = {key = "17", total_gold = "3000000000", red_packet = "1800000", describe = "水果狂欢累计赢金30亿金币", }
LuaConfigMgr.RedFruitConfigLen = 17
LuaConfigMgr.RedFruitConfig = M