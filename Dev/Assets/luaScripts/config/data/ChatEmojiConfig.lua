--表名: 聊天表情, 字段描述：_key:ID, _type:表情, _show_text:文本, _sound:声音, _effect:特效, 
local M = {}
M["100"] = {key = "100", type = "0", show_text = "快点嘛！我等到花儿也谢了。", sound = "boy/boy1", effect = "", }
M["101"] = {key = "101", type = "0", show_text = "不好意思，我要离开一会。", sound = "boy/boy2", effect = "", }
M["102"] = {key = "102", type = "0", show_text = "不要吵啦，专心玩游戏吧。", sound = "boy/boy3", effect = "", }
M["103"] = {key = "103", type = "0", show_text = "不要走！决战到天亮。", sound = "boy/boy4", effect = "", }
M["104"] = {key = "104", type = "0", show_text = "大家好！很高兴见到各位！", sound = "boy/boy5", effect = "", }
M["105"] = {key = "105", type = "0", show_text = "猜猜我的手牌吧。", sound = "boy/boy6", effect = "", }
M["106"] = {key = "106", type = "0", show_text = "风水轮流转，我快没筹码了。", sound = "boy/boy7", effect = "", }
M["107"] = {key = "107", type = "0", show_text = "你玩的太好了，交个朋友吧", sound = "boy/boy8", effect = "", }
M["108"] = {key = "108", type = "0", show_text = "看我通杀全场！这些钱全是我的。", sound = "boy/boy9", effect = "", }
M["109"] = {key = "109", type = "0", show_text = "你是妹妹？还是哥哥？", sound = "boy/boy10", effect = "", }
M["110"] = {key = "110", type = "0", show_text = "我是庄家！谁敢挑战我？", sound = "boy/boy11", effect = "", }
M["111"] = {key = "111", type = "0", show_text = "太晚啦，我要走了，来日再战。", sound = "boy/boy12", effect = "", }
M["112"] = {key = "112", type = "0", show_text = "又断线！网络实在太差了。", sound = "boy/boy13", effect = "", }
M["113"] = {key = "113", type = "1", show_text = "快点呀！我等到花儿也谢了。", sound = "girl/girl1", effect = "", }
M["114"] = {key = "114", type = "1", show_text = "快递来了，等我一会哦。", sound = "girl/girl2", effect = "", }
M["115"] = {key = "115", type = "1", show_text = "大家不要吵啦，我唱歌给你们听吧。", sound = "girl/girl3", effect = "", }
M["116"] = {key = "116", type = "1", show_text = "再陪我玩一会吧，就一盘！", sound = "girl/girl4", effect = "", }
M["117"] = {key = "117", type = "1", show_text = "帅哥，你玩得真好！能留个电话吗？", sound = "girl/girl5", effect = "", }
M["118"] = {key = "118", type = "1", show_text = "大家好！我玩得不好，哥哥们手下留情哦。", sound = "girl/girl6", effect = "", }
M["119"] = {key = "119", type = "1", show_text = "(*^__^*) 嘻嘻……拿到一副好牌。", sound = "girl/girl7", effect = "", }
M["120"] = {key = "120", type = "1", show_text = "你是妹妹？还是哥哥？", sound = "girl/girl8", effect = "", }
M["200"] = {key = "200", type = "1", show_text = "没钱了，好伤心！哥哥们太狠啦。", sound = "girl/girl9", effect = "", }
M["201"] = {key = "201", type = "1", show_text = "终于轮到我坐庄了，决战吧！", sound = "girl/girl10", effect = "", }
M["202"] = {key = "202", type = "1", show_text = "再见啦！我会想念大家的。", sound = "girl/girl11", effect = "", }
M["203"] = {key = "203", type = "1", show_text = "又赢啦！哥哥们不要怪我呀。", sound = "girl/girl12", effect = "", }
M["204"] = {key = "204", type = "1", show_text = "又断线了！网络怎么这么差呀。", sound = "girl/girl13", effect = "", }
M["205"] = {key = "205", type = "2", show_text = "emoji1", sound = "", effect = "", }
M["206"] = {key = "206", type = "2", show_text = "emoji2", sound = "", effect = "", }
M["207"] = {key = "207", type = "2", show_text = "emoji3", sound = "", effect = "", }
M["208"] = {key = "208", type = "2", show_text = "emoji4", sound = "", effect = "", }
M["209"] = {key = "209", type = "2", show_text = "emoji5", sound = "", effect = "", }
M["210"] = {key = "210", type = "2", show_text = "emoji6", sound = "", effect = "", }
M["211"] = {key = "211", type = "2", show_text = "emoji7", sound = "", effect = "", }
M["212"] = {key = "212", type = "2", show_text = "emoji8", sound = "", effect = "", }
M["213"] = {key = "213", type = "2", show_text = "emoji9", sound = "", effect = "", }
M["214"] = {key = "214", type = "2", show_text = "emoji10", sound = "", effect = "", }
M["215"] = {key = "215", type = "2", show_text = "emoji11", sound = "", effect = "", }
M["216"] = {key = "216", type = "2", show_text = "emoji12", sound = "", effect = "", }
M["300"] = {key = "300", type = "3", show_text = "magic1", sound = "magic/egg", effect = "zhadan", }
M["301"] = {key = "301", type = "3", show_text = "magic2", sound = "magic/folwer", effect = "tuoxiezhakai", }
M["302"] = {key = "302", type = "3", show_text = "magic3", sound = "magic/cup", effect = "hua", }
M["303"] = {key = "303", type = "3", show_text = "magic4", sound = "magic/slipper", effect = "dianzhang", }
M["304"] = {key = "304", type = "3", show_text = "magic5", sound = "magic/brick", effect = "effect_brick", }
LuaConfigMgr.ChatEmojiConfigLen = 43
LuaConfigMgr.ChatEmojiConfig = M