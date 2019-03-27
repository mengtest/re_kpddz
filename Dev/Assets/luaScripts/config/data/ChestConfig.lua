--表名: 对局宝箱, 字段描述：_key:编号, _name:名称, _min_door:最低门槛, _condition:领取宝箱需要对局数, _free_get:免费领取金币, _get2:领取镖票（改钻石）, _get3:领取镖票（改钻石）, _get_limit:当天未购买福袋用户每日免费领取次数限制, _get_limit2:当天购买福袋用户每日免费领取次数限制, 
local M = {}
M["1"] = {key = "1", name = "新手场", min_door = "10000", condition = "5", free_get = {{"1000", "0", }, }, get2 = {{"1", "10000", }, }, get3 = {{"5", "50000", }, }, get_limit = "5", get_limit2 = "10", }
M["2"] = {key = "2", name = "百人大战", min_door = "10000", condition = "50000", free_get = {{"1000", "0", }, }, get2 = {{"1", "0", }, }, get3 = {{"5", "50000", }, }, get_limit = "5", get_limit2 = "10", }
M["3"] = {key = "3", name = "水果狂欢", min_door = "10000", condition = "50000", free_get = {{"1000", "0", }, }, get2 = {{"1", "0", }, }, get3 = {{"5", "50000", }, }, get_limit = "5", get_limit2 = "10", }
M["4"] = {key = "4", name = "超级水果", min_door = "10000", condition = "50000", free_get = {{"1000", "0", }, }, get2 = {{"1", "0", }, }, get3 = {{"5", "50000", }, }, get_limit = "5", get_limit2 = "10", }
LuaConfigMgr.ChestConfigLen = 4
LuaConfigMgr.ChestConfig = M