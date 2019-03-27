--表名: 新手引导表, 字段描述：_key:ID, _character:文字, _image:形象, _position:位置, _reward:奖励金币, _choose:正确选项, 
local M = {}
M["1"] = {key = "1", character = "欢迎来到我们的游戏！游戏的规则很简单，跟着我两步就能学会。", image = "66001", position = "0", reward = "0", choose = "0", }
M["2"] = {key = "2", character = "5张牌中选取[ff6f21]3[-]张，数字相加是[ff6f21]10的倍数[-]（JQK算做10），就是[ff6f21]有牛[-]啦！", image = "66001", position = "0", reward = "0", choose = "0", }
M["3"] = {key = "3", character = "再看剩下的2张牌相加，总和个位数值就是你牌的大小.下面就是[ff6f21]牛六[-]啦！", image = "66001", position = "0", reward = "0", choose = "0", }
M["4"] = {key = "4", character = "亮牌后，庄家分别与其它玩家比较牌大小，牌大的就是[ff6f21]赢家！[-]", image = "66001", position = "0", reward = "0", choose = "0", }
M["5"] = {key = "5", character = "好啦，我已经讲解了游戏的基本规则，现在来考考你吧，下面哪副牌是[ff6f21]有牛[-]呢？请点击选择！", image = "66001", position = "0", reward = "0", choose = "1", }
M["6"] = {key = "6", character = "太棒了！恭喜你答对了！那再来看下面哪副牌是[ff6f21]牛牛[-]呢？", image = "66001", position = "0", reward = "0", choose = "1", }
LuaConfigMgr.GuideConfigLen = 6
LuaConfigMgr.GuideConfig = M