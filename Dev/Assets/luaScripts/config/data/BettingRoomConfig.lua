--表名: 赌注房间表, 字段描述：_key:编号, _name:名称, _commision:佣金, _score:底分, _doorsill:门槛, _door_des:门槛描述, _taxed:对应场抽税比例, 
local M = {}
M["1"] = {key = "1", name = "新手场", commision = "10", score = "10", doorsill = "2000", door_des = "2000以上", taxed = "0", }
M["10"] = {key = "10", name = "红包场", commision = "3", score = "3", doorsill = "30", door_des = "30钻", taxed = "0.3333333333333333", }
LuaConfigMgr.BettingRoomConfigLen = 2
LuaConfigMgr.BettingRoomConfig = M