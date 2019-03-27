--表名: 一本万利激活描述, 字段描述：_key:ID, _des:激活金钱, _type_des:描述, _buying_des:描述2, _buying_des1:描述3, _buying_des2:描述4, 
local M = {}
M["401"] = {key = "401", des = "30万", type_des = "一阶", buying_des = "30元购买", buying_des1 = "11倍", buying_des2 = "返利", }
M["402"] = {key = "402", des = "98万", type_des = "二阶", buying_des = "98元购买", buying_des1 = "11倍", buying_des2 = "返利", }
M["403"] = {key = "403", des = "198万", type_des = "三阶", buying_des = "198元购买", buying_des1 = "11倍", buying_des2 = "返利", }
M["404"] = {key = "404", des = "328万", type_des = "四阶", buying_des = "328元购买", buying_des1 = "11倍", buying_des2 = "返利", }
M["405"] = {key = "405", des = "648万", type_des = "五阶", buying_des = "648元购买", buying_des1 = "11倍", buying_des2 = "返利", }
LuaConfigMgr.ActDesConfigLen = 5
LuaConfigMgr.ActDesConfig = M