-- -----------------------------------------------------------------


-- *
-- * Filename:    protoMsgDef.lua.txt
-- * Summary:     proto消息定义，用于脚本消息发送
-- *
-- * Version:     1.0.0
-- * Author:      WP.Chu
-- * Date:        10/20/2016 6:44:11 AM
-- -----------------------------------------------------------------


-- 生成模块，模块导出接口需包含在M表中
___lua_proto_msg_def_table___ = {}
local M = ___lua_proto_msg_def_table___

-- 全局函数
local tostring = tostring

-- 注册消息
local function addProtoMsgDef(idMsg, tMsgDef)
	if M[idMsg] ~= nil then
		error("Message has been added: " .. tostring(idMsg))
		return
	end

	M[idMsg] = tMsgDef
end

-- 字段修饰符定义
local required = 1
local optional = 2
local repeated = 3

-- 字段类型
local boolean	= 3
local bool 		= 3
local int16		= 7
local uint16	= 8
local int32		= 9
local uint32	= 10
local int64		= 11
local uint64	= 12
local float		= 13
local double	= 14
local string	= 18  	-- utf8
local byteArray	= 101 	-- byte[]
local bytes     = 101	-- byte[]
local nestType	= 104	-- message

---------***ProtoId**---------------------
protoIdSet = {}
   protoIdSet.sc_activity_config_info_update = 250;		--活动配置 只登入时下发
   protoIdSet.cs_activity_info_query_req = 251;		--活动数据查询
   protoIdSet.sc_activity_info_query_reply = 252;		--活动数据查询 返回
   protoIdSet.cs_activity_draw_req = 253;		--活动领奖
   protoIdSet.sc_activity_draw_reply = 254;		--活动领奖
   protoIdSet.cs_task_pay_award_request = 257;		--一本万利
   protoIdSet.sc_task_pay_award_response = 258;		--一本万利返回
   protoIdSet.sc_task_pay_info_response = 259;		--一本万利
   protoIdSet.cs_task_pay_info_request = 264;		--一本万利查询
   protoIdSet.cs_car_enter_req = 280;		--进入豪车
   protoIdSet.sc_car_enter_reply = 281;		--进入豪车返回
   protoIdSet.cs_car_exit_req = 282;		--离开豪车
   protoIdSet.sc_car_exit_reply = 283;		--离开豪车返回
   protoIdSet.cs_car_master_req = 284;		--排庄
   protoIdSet.sc_car_master_reply = 285;		--排庄返回
   protoIdSet.cs_car_bet_req = 286;		--下注
   protoIdSet.sc_car_bet_reply = 287;		--下注返回
   protoIdSet.cs_car_rebet_req = 288;		--续压
   protoIdSet.sc_car_rebet_reply = 289;		--续压返回
   protoIdSet.cs_car_master_list_req = 290;		--排庄列表
   protoIdSet.cs_car_user_list_req = 291;		--在线列表
   protoIdSet.sc_car_user_list_reply = 292;		--在线列表返回
   protoIdSet.sc_car_result_history_req = 294;		--开奖结果历史
   protoIdSet.sc_car_master_wait_list_reply = 295;		--排庄列表
   protoIdSet.sc_car_master_info_reply = 296;		--庄家信息
   protoIdSet.sc_car_status_reply = 297;		--盘面状态
   protoIdSet.sc_car_room_info_reply = 298;		--房间信息
   protoIdSet.sc_car_hint_reply = 299;		--提示信息
   protoIdSet.sc_car_result_reply = 300;		--开奖结果
   protoIdSet.sc_car_pool_reply = 301;		--奖池总额
   protoIdSet.cs_car_add_money_req = 303;		--加钱
   protoIdSet.sc_car_add_money_reply = 304;		--加钱返回
   protoIdSet.cs_car_syn_in_game_state_req = 305;		--玩家在玩豪车状态通知消息 无返回
   protoIdSet.cs_player_niu_room_chest_draw = 142;		--领取对局宝箱
   protoIdSet.sc_niu_room_chest_draw_reply = 143;		--领取宝箱返回
   protoIdSet.sc_niu_room_chest_info_update = 144;		--对局信息更新
   protoIdSet.sc_niu_room_chest_times_update = 148;		--对局领取免费钻石次数
   protoIdSet.cs_common_heartbeat = 8;		--心跳协议
   protoIdSet.sc_common_heartbeat_reply = 9;		--心跳协议返回
   protoIdSet.cs_common_proto_count = 10;		--更新客户端已接收的协议计数
   protoIdSet.sc_common_proto_count = 11;		--更新服务端已接收的协议计数
   protoIdSet.cs_common_proto_clean = 12;		--通知服务端清理协议缓存
   protoIdSet.sc_common_proto_clean = 13;		--通知客户端清理协议缓存
   protoIdSet.cs_common_bug_feedback = 28;		--玩家BUG反馈
   protoIdSet.sc_common_bug_feedback = 29;		--玩家BUG反馈返回
   protoIdSet.sc_hundred_niu_room_state_update = 90;		--房间状态同步 接收到时开始倒计时
   protoIdSet.cs_hundred_niu_enter_room_req = 91;		--玩家进入房间
   protoIdSet.sc_hundred_niu_enter_room_reply = 92;		--玩家进入房间返回
   protoIdSet.cs_hundred_niu_player_list_query_req = 93;		--玩家查询闲家人员列表
   protoIdSet.sc_hundred_niu_player_list_query_reply = 94;		--玩家查询闲家人员列表返回
   protoIdSet.cs_hundred_niu_free_set_chips_req = 95;		--闲家下注
   protoIdSet.sc_hundred_niu_free_set_chips_reply = 96;		--闲家下注返回
   protoIdSet.sc_hundred_niu_free_set_chips_update = 97;		--闲家下注更新
   protoIdSet.cs_hundred_niu_sit_down_req = 98;		--上下座
   protoIdSet.sc_hundred_niu_sit_down_reply = 99;		--上下座返回
   protoIdSet.sc_hundred_niu_seat_player_info_update = 100;		--座位上人员信息 ( 包括庄家(pos=0)金币变化 ) 通过player_uuid比较得知位置上是否自己
   protoIdSet.cs_hundred_be_master_req = 101;		--上庄
   protoIdSet.sc_hundred_be_master_reply = 102;		--上庄返回
   protoIdSet.cs_hundred_query_master_list_req = 103;		--获取上庄列表
   protoIdSet.sc_hundred_query_master_list_reply = 104;		--上庄列表更新
   protoIdSet.cs_hundred_niu_in_game_syn_req = 105;		--同步在游戏中 (结算消息收到后立即发送)
   protoIdSet.cs_hundred_leave_room_req = 106;		--离开房间
   protoIdSet.sc_hundred_leave_room_reply = 107;		--离开房间返回
   protoIdSet.cs_hundred_query_winning_rec_req = 108;		--查询押注走势
   protoIdSet.sc_hundred_query_winning_rec_reply = 109;		--查询押注走势返回
   protoIdSet.sc_hundred_player_gold_change_update = 120;		--百人 自己金币变动更新 (在结算 奖池开奖 下注时 都主动发送)
   protoIdSet.cs_hundred_query_pool_win_player_req = 121;		--查询上次分奖池钱最多的人
   protoIdSet.sc_hundred_query_pool_win_player_reply = 122;		--查询上次分奖池钱最多的人返回
   protoIdSet.sc_items_update = 30;		--物品更新
   protoIdSet.sc_items_add = 31;		--物品新增
   protoIdSet.sc_items_delete = 32;		--物品删除
   protoIdSet.sc_items_init_update = 33;		--背包初始化
   protoIdSet.cs_item_use_req = 34;		--物品使用
   protoIdSet.sc_item_use_reply = 35;		--物品使用返回
   protoIdSet.cs_laba_enter_room_req = 201;		--进入房间
   protoIdSet.sc_laba_enter_room_reply = 202;		--进入房间返回
   protoIdSet.cs_laba_leave_room_req = 203;		--离开房间
   protoIdSet.sc_laba_leave_room_reply = 204;		--离开房间返回 玩家下线时检测 从容器中去除
   protoIdSet.sc_laba_pool_num_update = 205;		--奖池数量更新 发送给在房间的人
   protoIdSet.cs_laba_spin_req = 206;		--投注
   protoIdSet.sc_laba_spin_reply = 207;		--投注返回
   protoIdSet.cs_win_player_list = 217;		--中奖人信息
   protoIdSet.sc_win_player_list = 218;		--中奖人信息返回
   protoIdSet.cs_login = 1;		--请求登陆
   protoIdSet.sc_login_reply = 2;		--登陆返回 
   protoIdSet.cs_login_out = 3;		--退出登陆 
   protoIdSet.cs_login_reconnection = 4;		--请求重连  
   protoIdSet.sc_login_reconnection_reply = 5;		--请求重连返回  
   protoIdSet.sc_login_repeat = 6;		--重复登陆
   protoIdSet.sc_login_proto_complete = 7;		--登陆协议全部发送成功  
   protoIdSet.sc_mails_init_update = 40;		--邮件初始化更新
   protoIdSet.sc_mail_add = 41;		--发系统邮件
   protoIdSet.cs_mail_delete_request = 42;		--删除邮件
   protoIdSet.sc_mail_delete_reply = 43;		--删除邮件返回
   protoIdSet.cs_read_mail = 44;		--读邮件
   protoIdSet.cs_mail_draw_request = 45;		--领取邮件的请求
   protoIdSet.sc_mail_draw_reply = 46;		--领取邮件返回
   protoIdSet.cs_draw_mission_request = 110;		--领取任务奖励
   protoIdSet.sc_draw_mission_result_reply = 111;		--领取任务奖励返回
   protoIdSet.sc_mission = 112;		--更新任务信息,登入时发送
   protoIdSet.sc_mission_update = 113;		--更新单条任务
   protoIdSet.sc_mission_add = 114;		--增加单条任务
   protoIdSet.sc_mission_del = 115;		--删除单条任务
   protoIdSet.sc_game_task_info_update = 208;		--游戏中任务信息 百人和水果
   protoIdSet.sc_game_task_box_info_update = 209;		--游戏中累计任务信息更新
   protoIdSet.cs_game_task_draw_req = 210;		--任务奖励领取
   protoIdSet.sc_game_task_draw_reply = 211;		--任务奖励领取 返回
   protoIdSet.cs_game_task_box_draw_req = 212;		--宝箱任务奖励领取
   protoIdSet.sc_game_task_box_draw_reply = 213;		--宝箱任务奖励领取 返回
   protoIdSet.sc_redpack_task_draw_list_update = 214;		--红包任务 领奖列表 进入房间 和 任务赢钱数变化时更新
   protoIdSet.cs_redpack_task_draw_req = 215;		--红包任务奖励领取
   protoIdSet.sc_redpack_task_draw_reply = 216;		--红包任务奖励领取 返回
   protoIdSet.sc_niu_room_state_update = 50;		--房间状态同步 接收到时开始倒计时
   protoIdSet.cs_niu_enter_room_req = 51;		--玩家进入房间
   protoIdSet.sc_niu_enter_room_reply = 52;		--玩家进入房间返回
   protoIdSet.sc_niu_enter_room_player_info_update = 53;		--新加入的玩家信息更新
   protoIdSet.cs_niu_choose_master_rate_req = 54;		--抢庄 选倍率
   protoIdSet.sc_niu_choose_master_rate_reply = 55;		--抢庄 选倍率返回
   protoIdSet.sc_niu_player_choose_master_rate_update = 56;		--抢庄 选倍率信息更新
   protoIdSet.cs_niu_choose_free_rate_req = 57;		--闲家下注
   protoIdSet.sc_niu_choose_free_rate_reply = 58;		--闲家下注返回
   protoIdSet.sc_niu_player_choose_free_rate_update = 59;		--闲家下注更新
   protoIdSet.cs_niu_leave_room_req = 60;		--玩家离开房间(  发完该消息后切勿在发送在玩同步消息  )
   protoIdSet.sc_niu_leave_room_reply = 61;		--玩家离开返回
   protoIdSet.sc_niu_leave_room_player_pos_update = 62;		--离开玩家位置更新
   protoIdSet.cs_niu_submit_card_req = 63;		--提交牌型
   protoIdSet.sc_niu_submit_card_reply = 64;		--提交牌型返回
   protoIdSet.sc_niu_player_submit_card_update = 65;		--提交牌型更新
   protoIdSet.cs_niu_syn_in_game_state_req = 66;		--玩家在玩牛牛状态通知消息 无返回(收到20状态消息后发该消息即可)
   protoIdSet.cs_niu_query_player_room_info_req = 72;		--查询玩家房间状态信息 返回下面的消息
   protoIdSet.sc_niu_player_room_info_update = 67;		--登入时通知客户端上局游戏是否还在继续
   protoIdSet.sc_niu_player_back_to_room_info_update = 68;		--返回房间下发的更新消息 只发给进入玩家
   protoIdSet.sc_redpack_room_reset_times_update = 160;		--房间次数消息 登入时 , 变化时 , 隔天时 同步
   protoIdSet.sc_redpack_room_player_times_update = 161;		--当前局数信息 进房间 变化时 登入 同步
   protoIdSet.sc_redpack_room_redpack_notice_update = 162;		--红包可领取状态同步 收到该消息后一段时间内显示可领取红包
   protoIdSet.cs_redpack_room_draw_req = 163;		--红包领取请求
   protoIdSet.sc_redpack_room_draw_reply = 164;		--红包领取请求 返回
   protoIdSet.sc_redpack_redpack_timer_sec_update = 165;		--进房间时同步的领奖时间
   protoIdSet.cs_redpack_relive_req = 166;		--红包场复活
   protoIdSet.sc_redpack_relive_reply = 167;		--红包场复活返回
   protoIdSet.sc_redpack_relive_times = 168;		--红包场复活剩余次数
   protoIdSet.sc_fudai_pool_update = 169;		--福袋池更新
   protoIdSet.sc_player_base_info = 14;		--玩家信息
   protoIdSet.cs_player_change_name_req = 15;		--修改名字
   protoIdSet.sc_player_change_name_reply = 16;		--修改名字返回
   protoIdSet.cs_player_change_headicon_req = 17;		--修改头像
   protoIdSet.sc_player_change_headicon_reply = 18;		--修改头像返回
   protoIdSet.cs_player_chat = 19;		--聊天
   protoIdSet.sc_player_chat = 20;		--聊天返回
   protoIdSet.sc_player_sys_notice = 21;		--系统公告
   protoIdSet.sc_tips = 22;		--服务端主动发给客户端的提示文字
   protoIdSet.cs_query_player_winning_rec_req = 23;		--查询玩家胜负记录
   protoIdSet.sc_query_player_winning_rec_reply = 24;		--查询玩家胜负记录返回
   protoIdSet.cs_niu_query_in_game_player_num_req = 25;		--请求获取在游戏中的人数
   protoIdSet.sc_niu_query_in_game_player_num_reply = 26;		--请求获取在游戏中的人数返回
   protoIdSet.cs_niu_subsidy_req = 69;		--破产补助
   protoIdSet.sc_niu_subsidy_reply = 70;		--破产补助请求返回
   protoIdSet.sc_niu_subsidy_info_update = 71;		--破产补助信息
   protoIdSet.cs_niu_special_subsidy_share = 73;		--破产特别补助分享
   protoIdSet.sc_niu_special_subsidy_share = 74;		--破产特别补助分享返回
   protoIdSet.cs_daily_checkin_req = 75;		--每日签到
   protoIdSet.sc_daily_checkin_reply = 76;		--每日签到  和 补签 返回
   protoIdSet.sc_daily_checkin_info_update = 77;		--签到配置信息
   protoIdSet.cs_make_up_for_checkin_req = 78;		--补签
   protoIdSet.sc_player_phone_num_info_update = 140;		--手机号信息更新
   protoIdSet.sc_player_bind_phone_num = 141;		--绑定手机号返回
   protoIdSet.cs_player_bind_phone_num_draw = 145;		--绑定手机号领取奖励
   protoIdSet.sc_player_bind_phone_num_draw_reply = 146;		--绑定手机号领奖返回
   protoIdSet.sc_niu_special_subsidy_info_update = 147;		--破产特别补助信息
   protoIdSet.cs_rank_query_req = 123;		--排行榜查询
   protoIdSet.sc_rank_qurey_reply = 124;		--排行榜查询返回
   protoIdSet.cs_vip_daily_reward = 240;		--领取VIP特别奖励
   protoIdSet.sc_vip_daily_reward = 241;		--领取VIP特别奖励返回
   protoIdSet.sc_guide_info_update = 150;		--新手引导 更新
   protoIdSet.cs_guide_next_step_req = 151;		--新手引导请求
   protoIdSet.sc_guide_next_step_reply = 152;		--新手引导请求返回 (主动发更新)
   protoIdSet.cs_hundred_last_week_rank_query_req = 153;		--百人上周中奖
   protoIdSet.sc_hundred_last_week_rank_query_reply = 154;		--百人上周中奖返回
   protoIdSet.cs_real_name_update = 155;		--实名制
   protoIdSet.sc_real_name_update = 156;		--实名制返回
   protoIdSet.cs_real_name_req = 157;		--实名制查询
   protoIdSet.sc_real_name_req = 158;		--实名制查询返回
   protoIdSet.cs_super_laba_last_week_rank_query_req = 320;		--超级拉霸上周中奖名单
   protoIdSet.sc_super_laba_last_week_rank_query_reply = 321;		--超级拉霸上周中奖名单返回
   protoIdSet.sc_prize_config_update = 230;		--奖品兑换更新 登入时发
   protoIdSet.cs_prize_query_one_req = 231;		--查询物品库存
   protoIdSet.sc_prize_query_one_reply = 232;		--查询物品库存 返回
   protoIdSet.cs_prize_exchange_req = 233;		--兑换
   protoIdSet.sc_prize_exchange_reply = 234;		--兑换返回 ( 同时发查询物品库存返回)
   protoIdSet.sc_prize_exchange_record_update = 235;		--兑换记录 登入和更新时发送
   protoIdSet.sc_prize_address_info_update = 236;		--地址更新 登入时和改变时发送
   protoIdSet.cs_prize_address_change_req = 237;		--地址修改
   protoIdSet.sc_prize_address_change_reply = 238;		--地址修改返回
   protoIdSet.sc_prize_storage_red_point_update = 239;		--同步实物库存 红点
   protoIdSet.cs_prize_query_phonecard_key_req = 255;		--查询卡密
   protoIdSet.sc_prize_query_phonecard_key_reply = 256;		--查询卡密返回
   protoIdSet.cs_red_pack_query_list_req = 220;		--查询红包列表
   protoIdSet.sc_red_pack_query_list_reply = 221;		--查询红包列表返回
   protoIdSet.cs_red_pack_open_req = 222;		--拆红包
   protoIdSet.sc_red_pack_open_reply = 223;		--拆红包返回
   protoIdSet.cs_red_pack_create_req = 224;		--发红包
   protoIdSet.sc_red_pack_create_reply = 225;		--发红包返回
   protoIdSet.sc_red_pack_notice_update = 226;		--红包通知
   protoIdSet.cs_red_pack_cancel_req = 227;		--取消红包
   protoIdSet.sc_red_pack_cancel_reply = 228;		--取消红包返回
   protoIdSet.sc_self_red_pack_info = 229;		--总红包数，玩家自己发的红包的红包信息
   protoIdSet.cs_red_pack_do_select_req = 260;		--红包确认请求
   protoIdSet.sc_red_pack_do_select_reply = 261;		--红包确认返回
   protoIdSet.cs_red_pack_search_req = 262;		--红包查询请求
   protoIdSet.sc_red_pack_search_reply = 263;		--红包查询返回
   protoIdSet.cs_share_new_bee_reward_req = 266;		--分享新手礼包领取
   protoIdSet.sc_share_new_bee_reward_reply = 267;		--分享新手礼包领取返回
   protoIdSet.cs_share_mission_reward_req = 268;		--分享任务奖励领取
   protoIdSet.sc_share_mission_reward_reply = 269;		--分享任务奖励领取返回
   protoIdSet.sc_share_info = 270;		--分享信息
   protoIdSet.sc_share_mission_update = 271;		--分享任务更新
   protoIdSet.cs_share_draw_request = 272;		--分享抽奖
   protoIdSet.cs_share_friend_request = 273;		--分享好友进度
   protoIdSet.cs_share_rank_request = 274;		--分享好友榜单
   protoIdSet.sc_share_draw_response = 275;		--分享抽奖返回
   protoIdSet.sc_share_history_response = 276;		--分享好友进度
   protoIdSet.sc_share_rank_response = 277;		--分享好友榜单
   protoIdSet.sc_draw_count_response = 278;		--分享抽奖剩余次数
   protoIdSet.sc_task_seven_info_response = 310;		--7日狂欢任务信息
   protoIdSet.cs_task_seven_award_request = 311;		--7日狂欢领奖
   protoIdSet.sc_task_seven_award_response = 312;		--7日狂欢领奖返回
   protoIdSet.cs_share_with_friends_req = 313;		--分享朋友圈
   protoIdSet.sc_shop_all_item_base_config = 80;		--商店所有物品信息
   protoIdSet.cs_shop_buy_query = 81;		--商店购买
   protoIdSet.sc_shop_buy_reply = 82;		--商店购买返回
   protoIdSet.sc_golden_bull_info_update = 83;		--金牛领奖信息 登入和领完奖时同步
   protoIdSet.cs_golden_bull_draw_req = 84;		--金牛领奖
   protoIdSet.sc_golden_bull_draw_reply = 85;		--金牛领奖返回
   protoIdSet.sc_month_card_info_update = 86;		--月卡信息更新 登入 隔天 和 领奖返回时下发
   protoIdSet.cs_month_card_draw_req = 87;		--月卡领奖
   protoIdSet.sc_month_card_draw_reply = 88;		--月卡领奖返回
   protoIdSet.cs_cash_transformation_req = 130;		--奖券兑换
   protoIdSet.sc_cash_transformation_reply = 131;		--奖券兑换返回
   protoIdSet.cs_golden_bull_mission = 132;		--金牛任务进度
   protoIdSet.sc_golden_bull_mission = 133;		--

-----------------------** end protoID**-------------

-- /////////////////////////////////////////////////////////////////////////////////////////////


-- 检查消息定义
local function checkProtoMsgDef()
	local keyAux = {}

	for k ,v in pairs(M) do
		for p, q in pairs(v) do
			if type(q[2]) == "string" then
				if M[q[2]] == nil then
					error("Non exist message define <" .. q[2] .. ">")
				end
			end
		end
	end 
end






-- *********************************


----pb类型数据
local protoMsg = {
	pb_activity_config = {
		[1] = {required,'pb_activity_data','activity_data'},
		[2] = {repeated,'pb_sub_activity_config','sub_list'},
	},
	pb_sub_activity_config = {
		[1] = {required,uint32,'id'},
		[2] = {required,uint64,'data'},
		[3] = {repeated,'pb_reward_info','reward_list'},
	},
	pb_activity_data = {
		[1] = {required,uint32,'id'},
		[2] = {required,uint64,'current_data'},
		[3] = {repeated,uint32,'draw_info_list'},
	},
	pb_annoucement_config = {
		[1] = {required,uint32,'id'},
		[2] = {required,byteArray,'title'},
		[3] = {required,byteArray,'content'},
	},
	pb_car_master_item = {
		[1] = {required,byteArray,'name'},
		[2] = {required,int32,'money'},
		[3] = {required,int32,'vip'},
	},
	pb_car_user_item = {
		[1] = {required,byteArray,'name'},
		[2] = {required,int32,'money'},
		[3] = {required,int32,'vip'},
		[4] = {required,string,'head'},
		[5] = {required,int32,'sex'},
	},
	pb_car_result_history_item = {
		[1] = {required,string,'open_index'},
		[2] = {required,int32,'result'},
		[3] = {required,int32,'pool'},
	},
	pb_car_bet_item = {
		[1] = {required,int32,'index'},
		[2] = {required,int32,'money'},
	},
	pb_pool_reward_info = {
		[1] = {required,uint32,'set_pos'},
		[2] = {required,uint64,'total_reward_num'},
		[3] = {repeated,'hundred_game_over_settlement','seat_reward_num'},
	},
	hundred_game_over_settlement = {
		[1] = {required,uint32,'player_pos'},
		[2] = {required,int64,'reward_num'},
		[3] = {repeated,uint32,'set_pos_list'},
	},
	pb_one_player_poker_card = {
		[1] = {required,uint32,'seat_pos'},
		[2] = {repeated,'pb_poker_card','card_list'},
		[3] = {required,uint32,'card_type'},
		[4] = {optional,bool,'is_win'},
	},
	pb_hundred_niu_room_data = {
		[1] = {required,uint32,'state_id'},
		[2] = {required,uint32,'end_sec_time'},
		[3] = {optional,uint64,'reward_pool_num'},
		[4] = {repeated,'pb_set_chips_info','my_set_chips_info'},
	},
	pb_hundred_niu_player_info = {
		[1] = {required,string,'player_uuid'},
		[2] = {required,uint32,'icon_type'},
		[3] = {optional,string,'icon_url'},
		[4] = {optional,byteArray,'player_name'},
		[5] = {required,uint32,'vip_level'},
		[6] = {optional,uint64,'gold'},
		[7] = {optional,uint32,'pos'},
		[8] = {optional,uint64,'pool_win_gold'},
		[9] = {required,uint32,'sex'},
	},
	pb_set_chips_info = {
		[1] = {required,uint32,'pos'},
		[2] = {required,uint64,'total_chips'},
		[3] = {repeated,'pb_seat_set_info','seat_pos_list'},
	},
	pb_seat_set_info = {
		[1] = {required,uint32,'seat_pos'},
		[2] = {required,uint64,'set_chips_num'},
	},
	pb_hundred_win_rec = {
		[1] = {required,bool,'win_1'},
		[2] = {required,bool,'win_2'},
		[3] = {required,bool,'win_3'},
		[4] = {required,bool,'win_4'},
		[5] = {repeated,uint32,'pool_win'},
	},
	pb_item_info = {
		[1] = {required,string,'uuid'},
		[2] = {required,uint32,'base_id'},
		[3] = {required,uint32,'count'},
	},
	pb_reward_info = {
		[1] = {required,uint32,'base_id'},
		[2] = {required,uint64,'count'},
	},
	pb_pool_win_player_info = {
		[1] = {required,string,'player_uuid'},
		[2] = {optional,string,'icon_url'},
		[3] = {optional,byteArray,'player_name'},
		[4] = {required,uint32,'vip_level'},
		[5] = {optional,string,'win_gold'},
		[6] = {optional,int32,'c_time'},
	},
	pb_laba_fruit_info = {
		[1] = {required,uint32,'pos_id'},
		[2] = {required,uint32,'fruit_type'},
	},
	pb_laba_line_reward = {
		[1] = {required,uint32,'line_id'},
		[2] = {required,uint32,'same_num'},
	},
	pb_mail = {
		[1] = {required,string,'mail_id'},
		[2] = {required,uint32,'cls'},
		[3] = {required,byteArray,'title'},
		[4] = {required,byteArray,'content'},
		[5] = {required,bool,'read'},
		[6] = {required,uint32,'receive_date'},
		[7] = {required,uint32,'expire_date'},
		[8] = {repeated,'pb_reward_info','reward_list'},
	},
	pb_mission = {
		[1] = {required,uint32,'id'},
		[2] = {required,uint32,'state'},
		[3] = {required,uint64,'count'},
	},
	pb_game_task_info = {
		[1] = {required,int32,'taskId'},
		[2] = {optional,int32,'process'},
		[3] = {optional,int32,'status'},
		[4] = {optional,int32,'boxStart'},
		[5] = {optional,int32,'boxProcess'},
		[6] = {repeated,int32,'boxStatus'},
		[7] = {optional,int32,'remaindTime'},
		[8] = {required,int32,'tast_type'},
		[9] = {required,int32,'vip_level'},
	},
	pb_redpack_task_draw_info = {
		[1] = {required,uint32,'game_type'},
		[2] = {required,uint64,'gold_num'},
		[3] = {repeated,uint32,'draw_list'},
	},
	pb_poker_card = {
		[1] = {required,uint32,'number'},
		[2] = {required,uint32,'color'},
	},
	game_over_settlement = {
		[1] = {repeated,'pb_settle_info','all_player_settle_info'},
	},
	pb_settle_info = {
		[1] = {required,uint32,'player_pos'},
		[2] = {required,int64,'reward_num'},
		[3] = {required,uint32,'card_type'},
		[4] = {repeated,'pb_poker_card','card_list'},
	},
	pb_niu_player_info = {
		[1] = {required,uint32,'pos'},
		[2] = {required,string,'player_uuid'},
		[3] = {required,uint64,'gold_num'},
		[4] = {required,uint32,'icon_type'},
		[5] = {optional,string,'icon_url'},
		[6] = {optional,byteArray,'player_name'},
		[7] = {optional,uint32,'master_rate'},
		[8] = {optional,uint32,'free_rate'},
		[9] = {repeated,'pb_poker_card','open_card_list'},
		[10] = {optional,uint32,'card_type'},
		[11] = {required,uint32,'vip_level'},
		[12] = {required,uint32,'sex'},
	},
	pb_room_player_num = {
		[1] = {required,uint32,'room_level'},
		[2] = {required,uint32,'player_num'},
	},
	pb_checkin_info = {
		[1] = {required,uint32,'day'},
		[2] = {repeated,'pb_reward_info','rewards'},
		[3] = {required,bool,'is_draw'},
	},
	pb_rank_info = {
		[1] = {required,uint32,'rank'},
		[2] = {required,string,'player_uuid'},
		[3] = {required,byteArray,'player_name'},
		[4] = {required,string,'player_icon'},
		[5] = {required,uint32,'player_vip'},
		[6] = {optional,uint64,'gold_num'},
		[7] = {optional,uint64,'win_gold_num'},
		[8] = {optional,uint64,'cash_num'},
		[9] = {optional,uint32,'sex'},
		[10] = {optional,uint64,'hundred_win'},
		[11] = {required,string,'account'},
	},
	pb_hundred_last_week_data = {
		[1] = {required,uint32,'rank'},
		[2] = {required,uint64,'reward_gold'},
		[3] = {required,byteArray,'name1_round_win'},
		[4] = {required,byteArray,'name2_total_win'},
	},
	pb_prize_info = {
		[1] = {required,uint32,'obj_id'},
		[2] = {required,byteArray,'name'},
		[3] = {required,uint32,'need_item_id'},
		[4] = {required,uint64,'need_item_num'},
		[5] = {required,uint32,'need_vip_level'},
		[6] = {required,string,'icon'},
		[7] = {required,uint32,'tag'},
		[8] = {required,uint32,'cls'},
		[9] = {required,byteArray,'dsc'},
		[10] = {required,uint32,'today_buy_times'},
		[11] = {required,int32,'sort_id'},
	},
	pb_prize_exchange_record = {
		[1] = {required,string,'id'},
		[2] = {required,uint32,'record_type'},
		[3] = {required,uint32,'obj_id'},
		[4] = {required,uint32,'need_item_id'},
		[5] = {required,uint64,'need_item_num'},
		[6] = {required,uint32,'second_time'},
		[7] = {optional,uint32,'recharge_type'},
		[8] = {optional,uint32,'recharge_state'},
		[9] = {optional,string,'card_number'},
		[10] = {optional,string,'card_psd'},
	},
	pb_prize_address_info = {
		[1] = {required,uint32,'id'},
		[2] = {required,byteArray,'name'},
		[3] = {required,uint32,'sex'},
		[4] = {required,byteArray,'province_name'},
		[5] = {required,byteArray,'city_name'},
		[6] = {required,byteArray,'address'},
		[7] = {required,byteArray,'phone_number'},
	},
	pb_prize_query_one = {
		[1] = {required,uint32,'obj_id'},
		[2] = {required,uint32,'store_num'},
		[3] = {required,uint32,'crad_num'},
	},
	pb_red_pack_info = {
		[1] = {required,string,'uid'},
		[2] = {required,byteArray,'player_name'},
		[3] = {required,byteArray,'player_icon'},
		[4] = {required,string,'player_id'},
		[5] = {required,uint64,'min_num'},
		[6] = {required,uint64,'max_num'},
		[7] = {required,uint32,'over_time'},
		[8] = {required,byteArray,'des'},
		[9] = {required,string,'account'},
		[10] = {required,uint32,'sex'},
	},
	pb_red_pack_notice = {
		[1] = {required,string,'notice_id'},
		[2] = {required,uint32,'notice_type'},
		[3] = {required,uint32,'get_sec_time'},
		[4] = {required,byteArray,'content'},
		[5] = {required,uint64,'gold_num'},
		[6] = {required,uint32,'type'},
		[7] = {required,byteArray,'open_player_name'},
		[8] = {required,string,'open_player_account'},
		[9] = {required,string,'uid'},
	},
	pb_share_mission = {
		[1] = {required,string,'friend_id'},
		[2] = {required,byteArray,'name'},
		[3] = {required,string,'head'},
		[4] = {required,int32,'vip_lv'},
		[5] = {required,string,'title'},
		[6] = {required,int32,'type'},
		[7] = {required,int32,'status'},
	},
	pb_share_history_item_response = {
		[1] = {required,string,'name'},
		[2] = {required,int32,'process'},
	},
	pb_share_history_friend_item_response = {
		[1] = {required,string,'userId'},
		[2] = {required,string,'name'},
		[3] = {required,int32,'create_time'},
		[4] = {required,int32,'first_day'},
		[5] = {required,int32,'three_day'},
		[6] = {required,int32,'seven_day'},
		[7] = {required,int32,'is_red'},
	},
	pb_share_rank_item_response = {
		[1] = {required,int32,'rank'},
		[2] = {required,string,'name'},
		[3] = {required,int32,'count'},
	},
	pb_shop_item = {
		[1] = {required,uint32,'id'},
		[2] = {required,uint32,'shop_type'},
		[3] = {required,uint32,'item_id'},
		[4] = {required,uint32,'item_num'},
		[5] = {required,uint32,'item_extra_num'},
		[6] = {repeated,'pb_cost_type_and_num','cost_list'},
		[7] = {required,uint32,'discount'},
		[8] = {required,uint32,'special_flag'},
		[9] = {required,uint32,'start_time'},
		[10] = {required,uint32,'end_time'},
		[11] = {required,uint32,'limit_times'},
		[12] = {required,uint32,'vip_limit'},
		[13] = {required,uint32,'left_times'},
		[14] = {required,string,'icon'},
		[15] = {required,byteArray,'name'},
		[16] = {required,uint32,'sort'},
	},
	pb_cost_type_and_num = {
		[1] = {required,uint32,'cost_type'},
		[2] = {required,uint32,'cost_num'},
	},



--*******
	sc_activity_config_info_update = {
		[1] = {repeated,'pb_activity_config','activity_list'},
		[2] = {required,uint32,'hide_function_flag'},
	},
	cs_activity_info_query_req = {
		[1] = {required,uint32,'id'},
	},
	sc_activity_info_query_reply = {
		[1] = {required,'pb_activity_data','activity_data'},
	},
	cs_activity_draw_req = {
		[1] = {required,uint32,'activity_id'},
		[2] = {required,uint32,'sub_id'},
	},
	sc_activity_draw_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','reward_list'},
		[4] = {required,uint32,'activity_id'},
		[5] = {required,uint32,'sub_id'},
	},
	cs_task_pay_award_request = {
		[1] = {required,int32,'index'},
	},
	sc_task_pay_award_response = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','reward_list'},
	},
	sc_task_pay_info_response = {
		[1] = {required,int32,'task_id'},
		[2] = {required,string,'process'},
		[3] = {repeated,int32,'status'},
		[4] = {required,int32,'open'},
	},
	cs_task_pay_info_request = {
	},
	cs_car_enter_req = {
	},
	sc_car_enter_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
	},
	cs_car_exit_req = {
	},
	sc_car_exit_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
	},
	cs_car_master_req = {
		[1] = {required,int32,'flag'},
		[2] = {required,int32,'money'},
	},
	sc_car_master_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {required,int32,'flag'},
	},
	cs_car_bet_req = {
		[1] = {required,int32,'index'},
		[2] = {required,int32,'money'},
	},
	sc_car_bet_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {required,int32,'index'},
		[4] = {required,int32,'money'},
		[5] = {required,int32,'self'},
	},
	cs_car_rebet_req = {
		[1] = {repeated,'cs_car_bet_req','list'},
	},
	sc_car_rebet_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {repeated,'sc_car_bet_reply','list'},
	},
	cs_car_master_list_req = {
		[1] = {required,int32,'flag'},
	},
	cs_car_user_list_req = {
	},
	sc_car_user_list_reply = {
		[1] = {required,int32,'flag'},
		[2] = {repeated,'pb_car_user_item','list'},
	},
	sc_car_result_history_req = {
		[1] = {repeated,'pb_car_result_history_item','list'},
	},
	sc_car_master_wait_list_reply = {
		[1] = {required,int32,'flag'},
		[2] = {repeated,'pb_car_master_item','list'},
	},
	sc_car_master_info_reply = {
		[1] = {required,int32,'self'},
		[2] = {required,byteArray,'name'},
		[3] = {required,int32,'money'},
		[4] = {required,int32,'score'},
		[5] = {required,int32,'count'},
		[6] = {required,string,'head'},
		[7] = {required,int32,'vip'},
		[8] = {required,int32,'sex'},
	},
	sc_car_status_reply = {
		[1] = {required,int32,'status'},
		[2] = {required,int32,'time'},
	},
	sc_car_room_info_reply = {
		[1] = {required,'sc_car_master_info_reply','masterInfo'},
		[2] = {repeated,'pb_car_bet_item','list'},
		[3] = {repeated,'pb_car_bet_item','listSelf'},
		[4] = {required,int32,'result'},
		[5] = {required,int32,'self_num'},
		[6] = {required,int32,'master_num'},
		[7] = {required,int32,'pool_sub'},
		[8] = {required,int32,'pool'},
		[9] = {optional,int32,'bet_limit'},
	},
	sc_car_hint_reply = {
		[1] = {required,byteArray,'msg'},
	},
	sc_car_result_reply = {
		[1] = {required,int32,'result'},
		[2] = {required,int32,'resultIndex'},
		[3] = {required,int32,'selfNum'},
		[4] = {required,int32,'masterNum'},
		[5] = {required,int32,'poolSub'},
		[6] = {required,int32,'pool'},
	},
	sc_car_pool_reply = {
		[1] = {required,int32,'result'},
	},
	cs_car_add_money_req = {
		[1] = {required,int32,'money'},
	},
	sc_car_add_money_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
	},
	cs_car_syn_in_game_state_req = {
	},
	cs_player_niu_room_chest_draw = {
		[1] = {required,uint32,'type'},
		[2] = {required,int32,'game_type'},
	},
	sc_niu_room_chest_draw_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','rewards'},
		[4] = {required,int32,'game_type'},
	},
	sc_niu_room_chest_info_update = {
		[1] = {required,uint32,'times'},
		[2] = {required,int32,'game_type'},
	},
	sc_niu_room_chest_times_update = {
		[1] = {required,uint32,'times_niu'},
		[2] = {required,uint32,'times_hundred'},
		[3] = {required,uint32,'times_laba'},
		[4] = {required,int32,'update_type'},
	},
	cs_common_heartbeat = {
	},
	sc_common_heartbeat_reply = {
		[1] = {required,uint32,'now_time'},
	},
	cs_common_proto_count = {
		[1] = {required,uint32,'count'},
	},
	sc_common_proto_count = {
		[1] = {required,uint32,'count'},
	},
	cs_common_proto_clean = {
		[1] = {required,uint32,'count'},
	},
	sc_common_proto_clean = {
		[1] = {required,uint32,'count'},
	},
	cs_common_bug_feedback = {
		[1] = {required,byteArray,'content'},
	},
	sc_common_bug_feedback = {
		[1] = {required,uint32,'result'},
	},
	sc_hundred_niu_room_state_update = {
		[1] = {required,uint32,'state_id'},
		[2] = {required,uint32,'end_sec_time'},
		[3] = {repeated,'pb_one_player_poker_card','last_card_info'},
		[4] = {repeated,'hundred_game_over_settlement','settle_list'},
		[5] = {optional,uint64,'reward_pool_num'},
		[6] = {optional,'pb_hundred_win_rec','last_win_rec'},
		[7] = {repeated,'pb_pool_reward_info','pool_reward'},
		[8] = {optional,uint32,'master_continuous'},
	},
	cs_hundred_niu_enter_room_req = {
	},
	sc_hundred_niu_enter_room_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {optional,'pb_hundred_niu_room_data','game_data'},
	},
	cs_hundred_niu_player_list_query_req = {
		[1] = {required,uint32,'type'},
	},
	sc_hundred_niu_player_list_query_reply = {
		[1] = {required,uint32,'type'},
		[2] = {repeated,'pb_hundred_niu_player_info','list'},
	},
	cs_hundred_niu_free_set_chips_req = {
		[1] = {required,uint32,'pos'},
		[2] = {required,uint64,'chips_num'},
	},
	sc_hundred_niu_free_set_chips_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {required,uint32,'pos'},
		[4] = {required,uint64,'total_chips_num'},
	},
	sc_hundred_niu_free_set_chips_update = {
		[1] = {repeated,'pb_set_chips_info','upd_list'},
		[2] = {optional,uint32,'upd_flag'},
	},
	cs_hundred_niu_sit_down_req = {
		[1] = {required,uint32,'pos'},
	},
	sc_hundred_niu_sit_down_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
	},
	sc_hundred_niu_seat_player_info_update = {
		[1] = {repeated,'pb_hundred_niu_player_info','seat_list'},
		[2] = {optional,uint32,'delete_seat_pos'},
	},
	cs_hundred_be_master_req = {
		[1] = {required,uint32,'flag'},
		[2] = {required,uint64,'set_max'},
	},
	sc_hundred_be_master_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
	},
	cs_hundred_query_master_list_req = {
	},
	sc_hundred_query_master_list_reply = {
		[1] = {repeated,'pb_hundred_niu_player_info','list'},
	},
	cs_hundred_niu_in_game_syn_req = {
	},
	cs_hundred_leave_room_req = {
	},
	sc_hundred_leave_room_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
	},
	cs_hundred_query_winning_rec_req = {
	},
	sc_hundred_query_winning_rec_reply = {
		[1] = {repeated,'pb_hundred_win_rec','list'},
	},
	sc_hundred_player_gold_change_update = {
		[1] = {required,int64,'alter_num'},
	},
	cs_hundred_query_pool_win_player_req = {
	},
	sc_hundred_query_pool_win_player_reply = {
		[1] = {repeated,'pb_hundred_niu_player_info','list'},
	},
	sc_items_update = {
		[1] = {repeated,'pb_item_info','upd_list'},
	},
	sc_items_add = {
		[1] = {repeated,'pb_item_info','add_list'},
	},
	sc_items_delete = {
		[1] = {repeated,string,'del_list'},
	},
	sc_items_init_update = {
		[1] = {repeated,'pb_item_info','all_list'},
	},
	cs_item_use_req = {
		[1] = {required,string,'item_uuid'},
		[2] = {required,uint32,'num'},
	},
	sc_item_use_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'err_msg'},
	},
	cs_laba_enter_room_req = {
		[1] = {required,int32,'type'},
	},
	sc_laba_enter_room_reply = {
		[1] = {required,uint32,'line_num'},
		[2] = {required,uint32,'line_set_chips'},
		[3] = {required,uint32,'last_free_times'},
		[4] = {required,int32,'type'},
		[5] = {optional,int32,'start_time'},
		[6] = {optional,int32,'end_time'},
	},
	cs_laba_leave_room_req = {
		[1] = {required,int32,'type'},
	},
	sc_laba_leave_room_reply = {
		[1] = {required,uint32,'result'},
	},
	sc_laba_pool_num_update = {
		[1] = {required,string,'total_pool_num'},
	},
	cs_laba_spin_req = {
		[1] = {required,uint32,'line_num'},
		[2] = {required,uint32,'line_set_chips'},
		[3] = {required,int32,'type'},
	},
	sc_laba_spin_reply = {
		[1] = {required,string,'total_reward_num'},
		[2] = {repeated,'pb_laba_fruit_info','fruit_list'},
		[3] = {repeated,'pb_laba_line_reward','reward_list'},
		[4] = {required,uint32,'new_last_free_times'},
		[5] = {required,uint32,'pool'},
		[6] = {required,uint32,'free'},
	},
	cs_win_player_list = {
		[1] = {required,int32,'type'},
	},
	sc_win_player_list = {
		[1] = {repeated,'pb_pool_win_player_info','win_players'},
	},
	cs_login = {
		[1] = {required,uint32,'platform_flag'},
		[2] = {required,string,'uid'},
		[3] = {required,byteArray,'password'},
		[4] = {required,string,'sz_param'},
		[5] = {required,string,'version'},
		[6] = {required,string,'network_type'},
		[7] = {required,uint32,'sys_type'},
		[8] = {required,uint32,'chnid'},
		[9] = {required,uint32,'sub_chnid'},
		[10] = {required,string,'ios_idfa'},
		[11] = {required,string,'ios_idfv'},
		[12] = {required,string,'mac_address'},
		[13] = {required,string,'device_type'},
	},
	sc_login_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'reason'},
		[3] = {required,string,'reconnect_key'},
		[4] = {required,byteArray,'proto_key'},
	},
	cs_login_out = {
	},
	cs_login_reconnection = {
		[1] = {required,uint32,'platform_flag'},
		[2] = {required,string,'user'},
		[3] = {required,string,'reconnect_key'},
	},
	sc_login_reconnection_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'reason'},
		[3] = {required,byteArray,'proto_key'},
	},
	sc_login_repeat = {
	},
	sc_login_proto_complete = {
		[1] = {required,bool,'is_new_player'},
	},
	sc_mails_init_update = {
		[1] = {repeated,'pb_mail','sys_mails'},
	},
	sc_mail_add = {
		[1] = {required,'pb_mail','add_sys_mail'},
	},
	cs_mail_delete_request = {
		[1] = {required,string,'mail_id'},
		[2] = {optional,string,'request_mark'},
	},
	sc_mail_delete_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err_msg'},
		[3] = {optional,string,'request_mark'},
	},
	cs_read_mail = {
		[1] = {required,string,'mail_id'},
	},
	cs_mail_draw_request = {
		[1] = {repeated,string,'mail_ids'},
		[2] = {optional,string,'request_mark'},
	},
	sc_mail_draw_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err_msg'},
		[3] = {repeated,'pb_reward_info','reward_info_s'},
		[4] = {optional,string,'request_mark'},
	},
	cs_draw_mission_request = {
		[1] = {required,uint32,'id'},
	},
	sc_draw_mission_result_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err_msg'},
		[3] = {repeated,'pb_reward_info','reward_info_s'},
	},
	sc_mission = {
		[1] = {repeated,'pb_mission','missions'},
	},
	sc_mission_update = {
		[1] = {required,'pb_mission','mission_'},
	},
	sc_mission_add = {
		[1] = {required,'pb_mission','mission_'},
	},
	sc_mission_del = {
		[1] = {required,uint32,'id'},
	},
	sc_game_task_info_update = {
		[1] = {repeated,'pb_game_task_info','tast_info'},
	},
	sc_game_task_box_info_update = {
		[1] = {required,uint32,'game_type'},
		[2] = {repeated,uint32,'box_flag_list'},
	},
	cs_game_task_draw_req = {
		[1] = {required,uint32,'game_type'},
		[2] = {required,uint32,'task_id'},
	},
	sc_game_task_draw_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','reward'},
	},
	cs_game_task_box_draw_req = {
		[1] = {required,uint32,'box'},
		[2] = {required,uint32,'game_type'},
	},
	sc_game_task_box_draw_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','reward'},
	},
	sc_redpack_task_draw_list_update = {
		[1] = {required,uint32,'upd_type'},
		[2] = {repeated,'pb_redpack_task_draw_info','list'},
	},
	cs_redpack_task_draw_req = {
		[1] = {required,uint32,'game_type'},
		[2] = {required,uint32,'task_id'},
	},
	sc_redpack_task_draw_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','reward'},
		[4] = {required,uint32,'game_type'},
		[5] = {required,uint32,'task_id'},
	},
	sc_niu_room_state_update = {
		[1] = {required,uint32,'state_id'},
		[2] = {required,uint32,'end_sec_time'},
		[3] = {repeated,'pb_poker_card','open_card_list'},
		[4] = {optional,uint32,'master_pos'},
		[5] = {optional,'pb_poker_card','last_card_info'},
		[6] = {optional,'game_over_settlement','settle_list'},
	},
	cs_niu_enter_room_req = {
		[1] = {required,uint32,'room_type'},
	},
	sc_niu_enter_room_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,uint32,'my_pos'},
	},
	sc_niu_enter_room_player_info_update = {
		[1] = {repeated,'pb_niu_player_info','player_list'},
	},
	cs_niu_choose_master_rate_req = {
		[1] = {required,uint32,'rate_num'},
	},
	sc_niu_choose_master_rate_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
	},
	sc_niu_player_choose_master_rate_update = {
		[1] = {required,uint32,'pos'},
		[2] = {required,uint32,'rate_num'},
	},
	cs_niu_choose_free_rate_req = {
		[1] = {required,uint32,'rate_num'},
	},
	sc_niu_choose_free_rate_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
	},
	sc_niu_player_choose_free_rate_update = {
		[1] = {required,uint32,'pos'},
		[2] = {required,uint32,'rate_num'},
	},
	cs_niu_leave_room_req = {
	},
	sc_niu_leave_room_reply = {
		[1] = {required,uint32,'result'},
	},
	sc_niu_leave_room_player_pos_update = {
		[1] = {required,uint32,'leave_pos'},
	},
	cs_niu_submit_card_req = {
	},
	sc_niu_submit_card_reply = {
		[1] = {required,uint32,'result'},
	},
	sc_niu_player_submit_card_update = {
		[1] = {required,uint32,'player_pos'},
		[2] = {required,uint32,'card_type'},
		[3] = {repeated,'pb_poker_card','card_list'},
	},
	cs_niu_syn_in_game_state_req = {
	},
	cs_niu_query_player_room_info_req = {
	},
	sc_niu_player_room_info_update = {
		[1] = {required,uint32,'room_id'},
		[2] = {required,uint32,'enter_end_time'},
	},
	sc_niu_player_back_to_room_info_update = {
		[1] = {required,uint32,'state_id'},
		[2] = {required,uint32,'end_sec_time'},
		[3] = {repeated,'pb_niu_player_info','player_list'},
		[4] = {optional,uint32,'master_pos'},
		[5] = {required,uint32,'my_pos'},
	},
	sc_redpack_room_reset_times_update = {
		[1] = {required,uint32,'left_reset_times'},
		[2] = {required,uint32,'reset_seconds'},
		[3] = {required,uint32,'reset_mission_is_draw'},
	},
	sc_redpack_room_player_times_update = {
		[1] = {required,uint32,'now_play_times'},
	},
	sc_redpack_room_redpack_notice_update = {
		[1] = {required,uint32,'close_draw_second'},
		[2] = {required,uint32,'next_open_redpack_second'},
	},
	cs_redpack_room_draw_req = {
	},
	sc_redpack_room_draw_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','reward'},
		[4] = {required,uint32,'next_can_draw_second'},
	},
	sc_redpack_redpack_timer_sec_update = {
		[1] = {required,uint32,'next_can_draw_second'},
		[2] = {required,uint32,'next_open_redpack_second'},
	},
	cs_redpack_relive_req = {
	},
	sc_redpack_relive_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'err'},
	},
	sc_redpack_relive_times = {
		[1] = {required,uint32,'times'},
	},
	sc_fudai_pool_update = {
		[1] = {required,int32,'num'},
	},
	sc_player_base_info = {
		[1] = {required,string,'player_uuid'},
		[2] = {required,string,'account'},
		[3] = {required,byteArray,'name'},
		[4] = {required,uint64,'gold'},
		[5] = {required,uint32,'diamond'},
		[6] = {required,uint32,'cash'},
		[7] = {required,uint32,'exp'},
		[8] = {required,uint32,'level'},
		[9] = {required,string,'icon'},
		[10] = {required,uint32,'sex'},
		[11] = {required,uint32,'vip_level'},
		[12] = {required,uint32,'rmb'},
		[13] = {required,int32,'block'},
	},
	cs_player_change_name_req = {
		[1] = {required,byteArray,'name'},
	},
	sc_player_change_name_reply = {
		[1] = {required,uint32,'result'},
	},
	cs_player_change_headicon_req = {
		[1] = {required,string,'icon'},
		[2] = {required,uint32,'sex'},
	},
	sc_player_change_headicon_reply = {
		[1] = {required,uint32,'result'},
	},
	cs_player_chat = {
		[1] = {required,uint32,'room_type'},
		[2] = {required,uint32,'content_type'},
		[3] = {required,byteArray,'content'},
		[4] = {required,string,'obj_player_uuid'},
	},
	sc_player_chat = {
		[1] = {required,uint32,'room_type'},
		[2] = {required,uint32,'content_type'},
		[3] = {required,byteArray,'content'},
		[4] = {required,string,'player_uuid'},
		[5] = {required,byteArray,'player_name'},
		[6] = {required,byteArray,'player_icon'},
		[7] = {required,uint32,'player_vip'},
		[8] = {required,uint32,'player_seat_pos'},
		[9] = {required,uint32,'send_time'},
		[10] = {optional,string,'des_player_uuid'},
		[11] = {optional,byteArray,'des_player_name'},
	},
	sc_player_sys_notice = {
		[1] = {required,uint32,'flag'},
		[2] = {required,byteArray,'content'},
	},
	sc_tips = {
		[1] = {required,uint32,'type'},
		[2] = {required,byteArray,'text'},
	},
	cs_query_player_winning_rec_req = {
		[1] = {optional,string,'obj_player_uuid'},
	},
	sc_query_player_winning_rec_reply = {
		[1] = {required,uint32,'win_rate'},
		[2] = {required,uint32,'win_count'},
		[3] = {required,uint32,'defeated_count'},
		[4] = {required,int64,'max_property'},
		[5] = {required,int64,'total_profit'},
		[6] = {required,int64,'week_profit'},
		[7] = {required,uint32,'niu_10'},
		[8] = {required,uint32,'niu_11'},
		[9] = {required,uint32,'niu_12'},
		[10] = {required,uint32,'niu_13'},
		[11] = {required,uint32,'niu_0_win'},
		[12] = {optional,string,'obj_player_uuid'},
		[13] = {optional,byteArray,'obj_name'},
		[14] = {optional,uint32,'sex'},
		[15] = {optional,uint64,'gold'},
		[16] = {optional,string,'icon'},
		[17] = {optional,uint32,'level'},
		[18] = {optional,uint32,'vip_level'},
		[19] = {optional,string,'account'},
	},
	cs_niu_query_in_game_player_num_req = {
		[1] = {required,uint32,'game_type'},
	},
	sc_niu_query_in_game_player_num_reply = {
		[1] = {repeated,'pb_room_player_num','list'},
	},
	cs_niu_subsidy_req = {
		[1] = {required,uint32,'type'},
	},
	sc_niu_subsidy_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
	},
	sc_niu_subsidy_info_update = {
		[1] = {required,uint32,'left_times'},
		[2] = {required,uint32,'subsidy_gold'},
	},
	cs_niu_special_subsidy_share = {
		[1] = {required,uint32,'result'},
	},
	sc_niu_special_subsidy_share = {
	},
	cs_daily_checkin_req = {
		[1] = {required,uint32,'flag'},
	},
	sc_daily_checkin_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','rewards'},
		[4] = {required,uint32,'flag'},
	},
	sc_daily_checkin_info_update = {
		[1] = {repeated,'pb_checkin_info','list'},
		[2] = {required,uint32,'all_checkin_day'},
		[3] = {required,bool,'is_checkin_today'},
		[4] = {required,bool,'vip_is_draw'},
	},
	cs_make_up_for_checkin_req = {
		[1] = {required,uint32,'flag'},
	},
	sc_player_phone_num_info_update = {
		[1] = {required,string,'phone_num'},
		[2] = {required,bool,'is_draw'},
	},
	sc_player_bind_phone_num = {
		[1] = {required,uint32,'result'},
	},
	cs_player_bind_phone_num_draw = {
	},
	sc_player_bind_phone_num_draw_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','rewards'},
	},
	sc_niu_special_subsidy_info_update = {
		[1] = {required,uint32,'left_times'},
		[2] = {required,uint32,'subsidy_gold'},
		[3] = {required,bool,'is_share'},
	},
	cs_rank_query_req = {
		[1] = {required,uint32,'rank_type'},
	},
	sc_rank_qurey_reply = {
		[1] = {required,uint32,'rank_type'},
		[2] = {required,uint32,'my_rank'},
		[3] = {repeated,'pb_rank_info','rank_info_list'},
		[4] = {optional,uint32,'pool'},
		[5] = {optional,uint32,'my_recharge_money'},
		[6] = {optional,uint32,'start_time'},
		[7] = {optional,uint32,'end_time'},
	},
	cs_vip_daily_reward = {
	},
	sc_vip_daily_reward = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','rewards'},
	},
	sc_guide_info_update = {
		[1] = {required,uint32,'step_id'},
	},
	cs_guide_next_step_req = {
		[1] = {required,uint32,'next_step_id'},
	},
	sc_guide_next_step_reply = {
		[1] = {required,uint32,'result'},
		[2] = {repeated,'pb_reward_info','reward'},
	},
	cs_hundred_last_week_rank_query_req = {
	},
	sc_hundred_last_week_rank_query_reply = {
		[1] = {repeated,'pb_hundred_last_week_data','list'},
	},
	cs_real_name_update = {
		[1] = {required,byteArray,'name'},
		[2] = {required,string,'id_card_num'},
	},
	sc_real_name_update = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','rewards'},
	},
	cs_real_name_req = {
	},
	sc_real_name_req = {
		[1] = {required,int32,'type'},
	},
	cs_super_laba_last_week_rank_query_req = {
	},
	sc_super_laba_last_week_rank_query_reply = {
		[1] = {repeated,'pb_hundred_last_week_data','list'},
	},
	sc_prize_config_update = {
		[1] = {repeated,'pb_prize_info','list'},
	},
	cs_prize_query_one_req = {
		[1] = {required,uint32,'obj_id'},
	},
	sc_prize_query_one_reply = {
		[1] = {required,uint32,'obj_id'},
		[2] = {required,uint32,'day_times_config'},
		[3] = {required,uint32,'today_exchange_times'},
		[4] = {required,uint32,'store_num'},
		[5] = {required,uint32,'crad_num'},
	},
	cs_prize_exchange_req = {
		[1] = {required,uint32,'obj_id'},
		[2] = {optional,uint32,'phone_card_charge_type'},
		[3] = {optional,string,'phone_number'},
		[4] = {optional,uint32,'address_id'},
	},
	sc_prize_exchange_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','reward'},
		[4] = {required,uint32,'obj_id'},
	},
	sc_prize_exchange_record_update = {
		[1] = {repeated,'pb_prize_exchange_record','list'},
	},
	sc_prize_address_info_update = {
		[1] = {repeated,'pb_prize_address_info','list'},
	},
	cs_prize_address_change_req = {
		[1] = {required,'pb_prize_address_info','new_info'},
	},
	sc_prize_address_change_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'err'},
	},
	sc_prize_storage_red_point_update = {
		[1] = {repeated,'pb_prize_query_one','list'},
	},
	cs_prize_query_phonecard_key_req = {
		[1] = {required,string,'rec_id'},
	},
	sc_prize_query_phonecard_key_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'err'},
		[3] = {required,uint32,'state'},
		[4] = {required,string,'key'},
	},
	cs_red_pack_query_list_req = {
		[1] = {required,uint32,'begin_id'},
		[2] = {required,uint32,'end_id'},
	},
	sc_red_pack_query_list_reply = {
		[1] = {required,uint32,'begin_id'},
		[2] = {required,uint32,'end_id'},
		[3] = {required,uint32,'max_num'},
		[4] = {repeated,'pb_red_pack_info','list'},
	},
	cs_red_pack_open_req = {
		[1] = {required,string,'uid'},
		[2] = {required,uint64,'check_num'},
	},
	sc_red_pack_open_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'err'},
		[3] = {required,uint64,'reward_num'},
		[4] = {required,string,'uid'},
	},
	cs_red_pack_create_req = {
		[1] = {required,uint64,'set_num'},
		[2] = {required,byteArray,'des'},
	},
	sc_red_pack_create_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'err'},
	},
	sc_red_pack_notice_update = {
		[1] = {repeated,'pb_red_pack_notice','list'},
		[2] = {repeated,string,'delete_notice_list'},
	},
	cs_red_pack_cancel_req = {
		[1] = {required,string,'uid'},
	},
	sc_red_pack_cancel_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'err'},
		[3] = {required,string,'uid'},
	},
	sc_self_red_pack_info = {
		[1] = {required,uint32,'all_red_pack_num'},
		[2] = {repeated,'pb_red_pack_info','red_pack_list'},
	},
	cs_red_pack_do_select_req = {
		[1] = {required,string,'notice_id'},
		[2] = {required,uint32,'opt'},
	},
	sc_red_pack_do_select_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err_msg'},
		[3] = {required,string,'redpack_id'},
		[4] = {required,uint32,'opt'},
		[5] = {required,string,'notice_id'},
	},
	cs_red_pack_search_req = {
		[1] = {required,string,'uid'},
	},
	sc_red_pack_search_reply = {
		[1] = {repeated,'pb_red_pack_info','list'},
	},
	cs_share_new_bee_reward_req = {
		[1] = {required,string,'code'},
	},
	sc_share_new_bee_reward_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','rewards'},
	},
	cs_share_mission_reward_req = {
		[1] = {required,string,'friend_id'},
		[2] = {required,int32,'type'},
	},
	sc_share_mission_reward_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','rewards'},
		[4] = {required,string,'friend_id'},
		[5] = {required,int32,'type'},
	},
	sc_share_info = {
		[1] = {required,string,'my_code'},
		[2] = {required,string,'code'},
		[3] = {required,bool,'free'},
		[4] = {required,int32,'count'},
		[5] = {repeated,'pb_share_mission','list'},
	},
	sc_share_mission_update = {
		[1] = {repeated,'pb_share_mission','list'},
		[2] = {required,int32,'count'},
	},
	cs_share_draw_request = {
		[1] = {required,int32,'flag'},
	},
	cs_share_friend_request = {
		[1] = {required,int32,'page'},
		[2] = {optional,string,'user_id'},
	},
	cs_share_rank_request = {
		[1] = {required,int32,'page'},
	},
	sc_share_draw_response = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {required,int32,'index'},
	},
	sc_share_history_response = {
		[1] = {required,int32,'count'},
		[2] = {required,int32,'one_draw'},
		[3] = {required,int32,'three_draw'},
		[4] = {required,int32,'seven_draw'},
		[5] = {required,int32,'pages'},
		[6] = {repeated,'pb_share_history_friend_item_response','list'},
	},
	sc_share_rank_response = {
		[1] = {required,int32,'count'},
		[2] = {required,int32,'rank'},
		[3] = {required,int32,'pages'},
		[4] = {repeated,'pb_share_rank_item_response','list'},
		[5] = {optional,int32,'beginTime'},
		[6] = {optional,int32,'endTime'},
	},
	sc_draw_count_response = {
		[1] = {required,int32,'draw_count'},
		[2] = {required,int32,'draw_count_seven'},
		[3] = {required,int32,'draw_count_one'},
	},
	sc_task_seven_info_response = {
		[1] = {required,int32,'task_id'},
		[2] = {required,int32,'process'},
		[3] = {optional,int32,'status'},
		[4] = {optional,int32,'award'},
	},
	cs_task_seven_award_request = {
	},
	sc_task_seven_award_response = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err'},
		[3] = {repeated,'pb_reward_info','rewards'},
	},
	cs_share_with_friends_req = {
	},
	sc_shop_all_item_base_config = {
		[1] = {repeated,'pb_shop_item','item_list'},
	},
	cs_shop_buy_query = {
		[1] = {required,uint32,'id'},
	},
	sc_shop_buy_reply = {
		[1] = {required,uint32,'result'},
		[2] = {optional,byteArray,'err_msg'},
		[3] = {repeated,'pb_reward_info','rewards'},
		[4] = {required,uint32,'left_times'},
		[5] = {required,uint32,'id'},
	},
	sc_golden_bull_info_update = {
		[1] = {required,uint32,'left_times'},
	},
	cs_golden_bull_draw_req = {
		[1] = {required,uint32,'key'},
	},
	sc_golden_bull_draw_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'err'},
	},
	sc_month_card_info_update = {
		[1] = {required,uint32,'today_draw_flag'},
		[2] = {required,uint32,'left_times'},
	},
	cs_month_card_draw_req = {
	},
	sc_month_card_draw_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'err'},
	},
	cs_cash_transformation_req = {
		[1] = {required,int32,'num'},
	},
	sc_cash_transformation_reply = {
		[1] = {required,uint32,'result'},
		[2] = {required,byteArray,'err'},
	},
	cs_golden_bull_mission = {
	},
	sc_golden_bull_mission = {
		[1] = {required,int32,'process'},
	},

}

-- **************************注册测试**************************
addProtoMsgDef("sc_activity_config_info_update",protoMsg.sc_activity_config_info_update)
addProtoMsgDef("pb_activity_config",protoMsg.pb_activity_config)
addProtoMsgDef("pb_sub_activity_config",protoMsg.pb_sub_activity_config)
addProtoMsgDef("pb_activity_data",protoMsg.pb_activity_data)
addProtoMsgDef("cs_activity_info_query_req",protoMsg.cs_activity_info_query_req)
addProtoMsgDef("sc_activity_info_query_reply",protoMsg.sc_activity_info_query_reply)
addProtoMsgDef("cs_activity_draw_req",protoMsg.cs_activity_draw_req)
addProtoMsgDef("sc_activity_draw_reply",protoMsg.sc_activity_draw_reply)
addProtoMsgDef("pb_annoucement_config",protoMsg.pb_annoucement_config)
addProtoMsgDef("cs_task_pay_award_request",protoMsg.cs_task_pay_award_request)
addProtoMsgDef("sc_task_pay_award_response",protoMsg.sc_task_pay_award_response)
addProtoMsgDef("sc_task_pay_info_response",protoMsg.sc_task_pay_info_response)
addProtoMsgDef("cs_task_pay_info_request",protoMsg.cs_task_pay_info_request)
addProtoMsgDef("cs_car_enter_req",protoMsg.cs_car_enter_req)
addProtoMsgDef("sc_car_enter_reply",protoMsg.sc_car_enter_reply)
addProtoMsgDef("cs_car_exit_req",protoMsg.cs_car_exit_req)
addProtoMsgDef("sc_car_exit_reply",protoMsg.sc_car_exit_reply)
addProtoMsgDef("cs_car_master_req",protoMsg.cs_car_master_req)
addProtoMsgDef("sc_car_master_reply",protoMsg.sc_car_master_reply)
addProtoMsgDef("cs_car_bet_req",protoMsg.cs_car_bet_req)
addProtoMsgDef("sc_car_bet_reply",protoMsg.sc_car_bet_reply)
addProtoMsgDef("cs_car_rebet_req",protoMsg.cs_car_rebet_req)
addProtoMsgDef("sc_car_rebet_reply",protoMsg.sc_car_rebet_reply)
addProtoMsgDef("cs_car_master_list_req",protoMsg.cs_car_master_list_req)
addProtoMsgDef("pb_car_master_item",protoMsg.pb_car_master_item)
addProtoMsgDef("cs_car_user_list_req",protoMsg.cs_car_user_list_req)
addProtoMsgDef("sc_car_user_list_reply",protoMsg.sc_car_user_list_reply)
addProtoMsgDef("pb_car_user_item",protoMsg.pb_car_user_item)
addProtoMsgDef("sc_car_result_history_req",protoMsg.sc_car_result_history_req)
addProtoMsgDef("pb_car_result_history_item",protoMsg.pb_car_result_history_item)
addProtoMsgDef("sc_car_master_wait_list_reply",protoMsg.sc_car_master_wait_list_reply)
addProtoMsgDef("sc_car_master_info_reply",protoMsg.sc_car_master_info_reply)
addProtoMsgDef("sc_car_status_reply",protoMsg.sc_car_status_reply)
addProtoMsgDef("pb_car_bet_item",protoMsg.pb_car_bet_item)
addProtoMsgDef("sc_car_room_info_reply",protoMsg.sc_car_room_info_reply)
addProtoMsgDef("sc_car_hint_reply",protoMsg.sc_car_hint_reply)
addProtoMsgDef("sc_car_result_reply",protoMsg.sc_car_result_reply)
addProtoMsgDef("sc_car_pool_reply",protoMsg.sc_car_pool_reply)
addProtoMsgDef("cs_car_add_money_req",protoMsg.cs_car_add_money_req)
addProtoMsgDef("sc_car_add_money_reply",protoMsg.sc_car_add_money_reply)
addProtoMsgDef("cs_car_syn_in_game_state_req",protoMsg.cs_car_syn_in_game_state_req)
addProtoMsgDef("cs_player_niu_room_chest_draw",protoMsg.cs_player_niu_room_chest_draw)
addProtoMsgDef("sc_niu_room_chest_draw_reply",protoMsg.sc_niu_room_chest_draw_reply)
addProtoMsgDef("sc_niu_room_chest_info_update",protoMsg.sc_niu_room_chest_info_update)
addProtoMsgDef("sc_niu_room_chest_times_update",protoMsg.sc_niu_room_chest_times_update)
addProtoMsgDef("cs_common_heartbeat",protoMsg.cs_common_heartbeat)
addProtoMsgDef("sc_common_heartbeat_reply",protoMsg.sc_common_heartbeat_reply)
addProtoMsgDef("cs_common_proto_count",protoMsg.cs_common_proto_count)
addProtoMsgDef("sc_common_proto_count",protoMsg.sc_common_proto_count)
addProtoMsgDef("cs_common_proto_clean",protoMsg.cs_common_proto_clean)
addProtoMsgDef("sc_common_proto_clean",protoMsg.sc_common_proto_clean)
addProtoMsgDef("cs_common_bug_feedback",protoMsg.cs_common_bug_feedback)
addProtoMsgDef("sc_common_bug_feedback",protoMsg.sc_common_bug_feedback)
addProtoMsgDef("sc_hundred_niu_room_state_update",protoMsg.sc_hundred_niu_room_state_update)
addProtoMsgDef("pb_pool_reward_info",protoMsg.pb_pool_reward_info)
addProtoMsgDef("hundred_game_over_settlement",protoMsg.hundred_game_over_settlement)
addProtoMsgDef("pb_one_player_poker_card",protoMsg.pb_one_player_poker_card)
addProtoMsgDef("cs_hundred_niu_enter_room_req",protoMsg.cs_hundred_niu_enter_room_req)
addProtoMsgDef("sc_hundred_niu_enter_room_reply",protoMsg.sc_hundred_niu_enter_room_reply)
addProtoMsgDef("pb_hundred_niu_room_data",protoMsg.pb_hundred_niu_room_data)
addProtoMsgDef("cs_hundred_niu_player_list_query_req",protoMsg.cs_hundred_niu_player_list_query_req)
addProtoMsgDef("sc_hundred_niu_player_list_query_reply",protoMsg.sc_hundred_niu_player_list_query_reply)
addProtoMsgDef("pb_hundred_niu_player_info",protoMsg.pb_hundred_niu_player_info)
addProtoMsgDef("cs_hundred_niu_free_set_chips_req",protoMsg.cs_hundred_niu_free_set_chips_req)
addProtoMsgDef("sc_hundred_niu_free_set_chips_reply",protoMsg.sc_hundred_niu_free_set_chips_reply)
addProtoMsgDef("sc_hundred_niu_free_set_chips_update",protoMsg.sc_hundred_niu_free_set_chips_update)
addProtoMsgDef("pb_set_chips_info",protoMsg.pb_set_chips_info)
addProtoMsgDef("pb_seat_set_info",protoMsg.pb_seat_set_info)
addProtoMsgDef("cs_hundred_niu_sit_down_req",protoMsg.cs_hundred_niu_sit_down_req)
addProtoMsgDef("sc_hundred_niu_sit_down_reply",protoMsg.sc_hundred_niu_sit_down_reply)
addProtoMsgDef("sc_hundred_niu_seat_player_info_update",protoMsg.sc_hundred_niu_seat_player_info_update)
addProtoMsgDef("cs_hundred_be_master_req",protoMsg.cs_hundred_be_master_req)
addProtoMsgDef("sc_hundred_be_master_reply",protoMsg.sc_hundred_be_master_reply)
addProtoMsgDef("cs_hundred_query_master_list_req",protoMsg.cs_hundred_query_master_list_req)
addProtoMsgDef("sc_hundred_query_master_list_reply",protoMsg.sc_hundred_query_master_list_reply)
addProtoMsgDef("cs_hundred_niu_in_game_syn_req",protoMsg.cs_hundred_niu_in_game_syn_req)
addProtoMsgDef("cs_hundred_leave_room_req",protoMsg.cs_hundred_leave_room_req)
addProtoMsgDef("sc_hundred_leave_room_reply",protoMsg.sc_hundred_leave_room_reply)
addProtoMsgDef("cs_hundred_query_winning_rec_req",protoMsg.cs_hundred_query_winning_rec_req)
addProtoMsgDef("sc_hundred_query_winning_rec_reply",protoMsg.sc_hundred_query_winning_rec_reply)
addProtoMsgDef("pb_hundred_win_rec",protoMsg.pb_hundred_win_rec)
addProtoMsgDef("sc_hundred_player_gold_change_update",protoMsg.sc_hundred_player_gold_change_update)
addProtoMsgDef("cs_hundred_query_pool_win_player_req",protoMsg.cs_hundred_query_pool_win_player_req)
addProtoMsgDef("sc_hundred_query_pool_win_player_reply",protoMsg.sc_hundred_query_pool_win_player_reply)
addProtoMsgDef("sc_items_update",protoMsg.sc_items_update)
addProtoMsgDef("sc_items_add",protoMsg.sc_items_add)
addProtoMsgDef("sc_items_delete",protoMsg.sc_items_delete)
addProtoMsgDef("sc_items_init_update",protoMsg.sc_items_init_update)
addProtoMsgDef("pb_item_info",protoMsg.pb_item_info)
addProtoMsgDef("cs_item_use_req",protoMsg.cs_item_use_req)
addProtoMsgDef("sc_item_use_reply",protoMsg.sc_item_use_reply)
addProtoMsgDef("pb_reward_info",protoMsg.pb_reward_info)
addProtoMsgDef("cs_laba_enter_room_req",protoMsg.cs_laba_enter_room_req)
addProtoMsgDef("sc_laba_enter_room_reply",protoMsg.sc_laba_enter_room_reply)
addProtoMsgDef("cs_laba_leave_room_req",protoMsg.cs_laba_leave_room_req)
addProtoMsgDef("sc_laba_leave_room_reply",protoMsg.sc_laba_leave_room_reply)
addProtoMsgDef("sc_laba_pool_num_update",protoMsg.sc_laba_pool_num_update)
addProtoMsgDef("pb_pool_win_player_info",protoMsg.pb_pool_win_player_info)
addProtoMsgDef("cs_laba_spin_req",protoMsg.cs_laba_spin_req)
addProtoMsgDef("sc_laba_spin_reply",protoMsg.sc_laba_spin_reply)
addProtoMsgDef("pb_laba_fruit_info",protoMsg.pb_laba_fruit_info)
addProtoMsgDef("pb_laba_line_reward",protoMsg.pb_laba_line_reward)
addProtoMsgDef("cs_win_player_list",protoMsg.cs_win_player_list)
addProtoMsgDef("sc_win_player_list",protoMsg.sc_win_player_list)
addProtoMsgDef("cs_login",protoMsg.cs_login)
addProtoMsgDef("sc_login_reply",protoMsg.sc_login_reply)
addProtoMsgDef("cs_login_out",protoMsg.cs_login_out)
addProtoMsgDef("cs_login_reconnection",protoMsg.cs_login_reconnection)
addProtoMsgDef("sc_login_reconnection_reply",protoMsg.sc_login_reconnection_reply)
addProtoMsgDef("sc_login_repeat",protoMsg.sc_login_repeat)
addProtoMsgDef("sc_login_proto_complete",protoMsg.sc_login_proto_complete)
addProtoMsgDef("sc_mails_init_update",protoMsg.sc_mails_init_update)
addProtoMsgDef("pb_mail",protoMsg.pb_mail)
addProtoMsgDef("sc_mail_add",protoMsg.sc_mail_add)
addProtoMsgDef("cs_mail_delete_request",protoMsg.cs_mail_delete_request)
addProtoMsgDef("sc_mail_delete_reply",protoMsg.sc_mail_delete_reply)
addProtoMsgDef("cs_read_mail",protoMsg.cs_read_mail)
addProtoMsgDef("cs_mail_draw_request",protoMsg.cs_mail_draw_request)
addProtoMsgDef("sc_mail_draw_reply",protoMsg.sc_mail_draw_reply)
addProtoMsgDef("cs_draw_mission_request",protoMsg.cs_draw_mission_request)
addProtoMsgDef("sc_draw_mission_result_reply",protoMsg.sc_draw_mission_result_reply)
addProtoMsgDef("sc_mission",protoMsg.sc_mission)
addProtoMsgDef("sc_mission_update",protoMsg.sc_mission_update)
addProtoMsgDef("sc_mission_add",protoMsg.sc_mission_add)
addProtoMsgDef("sc_mission_del",protoMsg.sc_mission_del)
addProtoMsgDef("pb_mission",protoMsg.pb_mission)
addProtoMsgDef("sc_game_task_info_update",protoMsg.sc_game_task_info_update)
addProtoMsgDef("pb_game_task_info",protoMsg.pb_game_task_info)
addProtoMsgDef("sc_game_task_box_info_update",protoMsg.sc_game_task_box_info_update)
addProtoMsgDef("cs_game_task_draw_req",protoMsg.cs_game_task_draw_req)
addProtoMsgDef("sc_game_task_draw_reply",protoMsg.sc_game_task_draw_reply)
addProtoMsgDef("cs_game_task_box_draw_req",protoMsg.cs_game_task_box_draw_req)
addProtoMsgDef("sc_game_task_box_draw_reply",protoMsg.sc_game_task_box_draw_reply)
addProtoMsgDef("sc_redpack_task_draw_list_update",protoMsg.sc_redpack_task_draw_list_update)
addProtoMsgDef("pb_redpack_task_draw_info",protoMsg.pb_redpack_task_draw_info)
addProtoMsgDef("cs_redpack_task_draw_req",protoMsg.cs_redpack_task_draw_req)
addProtoMsgDef("sc_redpack_task_draw_reply",protoMsg.sc_redpack_task_draw_reply)
addProtoMsgDef("sc_niu_room_state_update",protoMsg.sc_niu_room_state_update)
addProtoMsgDef("cs_niu_enter_room_req",protoMsg.cs_niu_enter_room_req)
addProtoMsgDef("sc_niu_enter_room_reply",protoMsg.sc_niu_enter_room_reply)
addProtoMsgDef("sc_niu_enter_room_player_info_update",protoMsg.sc_niu_enter_room_player_info_update)
addProtoMsgDef("cs_niu_choose_master_rate_req",protoMsg.cs_niu_choose_master_rate_req)
addProtoMsgDef("sc_niu_choose_master_rate_reply",protoMsg.sc_niu_choose_master_rate_reply)
addProtoMsgDef("sc_niu_player_choose_master_rate_update",protoMsg.sc_niu_player_choose_master_rate_update)
addProtoMsgDef("cs_niu_choose_free_rate_req",protoMsg.cs_niu_choose_free_rate_req)
addProtoMsgDef("sc_niu_choose_free_rate_reply",protoMsg.sc_niu_choose_free_rate_reply)
addProtoMsgDef("sc_niu_player_choose_free_rate_update",protoMsg.sc_niu_player_choose_free_rate_update)
addProtoMsgDef("cs_niu_leave_room_req",protoMsg.cs_niu_leave_room_req)
addProtoMsgDef("sc_niu_leave_room_reply",protoMsg.sc_niu_leave_room_reply)
addProtoMsgDef("sc_niu_leave_room_player_pos_update",protoMsg.sc_niu_leave_room_player_pos_update)
addProtoMsgDef("cs_niu_submit_card_req",protoMsg.cs_niu_submit_card_req)
addProtoMsgDef("sc_niu_submit_card_reply",protoMsg.sc_niu_submit_card_reply)
addProtoMsgDef("sc_niu_player_submit_card_update",protoMsg.sc_niu_player_submit_card_update)
addProtoMsgDef("cs_niu_syn_in_game_state_req",protoMsg.cs_niu_syn_in_game_state_req)
addProtoMsgDef("cs_niu_query_player_room_info_req",protoMsg.cs_niu_query_player_room_info_req)
addProtoMsgDef("sc_niu_player_room_info_update",protoMsg.sc_niu_player_room_info_update)
addProtoMsgDef("sc_niu_player_back_to_room_info_update",protoMsg.sc_niu_player_back_to_room_info_update)
addProtoMsgDef("pb_poker_card",protoMsg.pb_poker_card)
addProtoMsgDef("game_over_settlement",protoMsg.game_over_settlement)
addProtoMsgDef("pb_settle_info",protoMsg.pb_settle_info)
addProtoMsgDef("pb_niu_player_info",protoMsg.pb_niu_player_info)
addProtoMsgDef("sc_redpack_room_reset_times_update",protoMsg.sc_redpack_room_reset_times_update)
addProtoMsgDef("sc_redpack_room_player_times_update",protoMsg.sc_redpack_room_player_times_update)
addProtoMsgDef("sc_redpack_room_redpack_notice_update",protoMsg.sc_redpack_room_redpack_notice_update)
addProtoMsgDef("cs_redpack_room_draw_req",protoMsg.cs_redpack_room_draw_req)
addProtoMsgDef("sc_redpack_room_draw_reply",protoMsg.sc_redpack_room_draw_reply)
addProtoMsgDef("sc_redpack_redpack_timer_sec_update",protoMsg.sc_redpack_redpack_timer_sec_update)
addProtoMsgDef("cs_redpack_relive_req",protoMsg.cs_redpack_relive_req)
addProtoMsgDef("sc_redpack_relive_reply",protoMsg.sc_redpack_relive_reply)
addProtoMsgDef("sc_redpack_relive_times",protoMsg.sc_redpack_relive_times)
addProtoMsgDef("sc_fudai_pool_update",protoMsg.sc_fudai_pool_update)
addProtoMsgDef("sc_player_base_info",protoMsg.sc_player_base_info)
addProtoMsgDef("cs_player_change_name_req",protoMsg.cs_player_change_name_req)
addProtoMsgDef("sc_player_change_name_reply",protoMsg.sc_player_change_name_reply)
addProtoMsgDef("cs_player_change_headicon_req",protoMsg.cs_player_change_headicon_req)
addProtoMsgDef("sc_player_change_headicon_reply",protoMsg.sc_player_change_headicon_reply)
addProtoMsgDef("cs_player_chat",protoMsg.cs_player_chat)
addProtoMsgDef("sc_player_chat",protoMsg.sc_player_chat)
addProtoMsgDef("sc_player_sys_notice",protoMsg.sc_player_sys_notice)
addProtoMsgDef("sc_tips",protoMsg.sc_tips)
addProtoMsgDef("cs_query_player_winning_rec_req",protoMsg.cs_query_player_winning_rec_req)
addProtoMsgDef("sc_query_player_winning_rec_reply",protoMsg.sc_query_player_winning_rec_reply)
addProtoMsgDef("cs_niu_query_in_game_player_num_req",protoMsg.cs_niu_query_in_game_player_num_req)
addProtoMsgDef("pb_room_player_num",protoMsg.pb_room_player_num)
addProtoMsgDef("sc_niu_query_in_game_player_num_reply",protoMsg.sc_niu_query_in_game_player_num_reply)
addProtoMsgDef("cs_niu_subsidy_req",protoMsg.cs_niu_subsidy_req)
addProtoMsgDef("sc_niu_subsidy_reply",protoMsg.sc_niu_subsidy_reply)
addProtoMsgDef("sc_niu_subsidy_info_update",protoMsg.sc_niu_subsidy_info_update)
addProtoMsgDef("cs_niu_special_subsidy_share",protoMsg.cs_niu_special_subsidy_share)
addProtoMsgDef("sc_niu_special_subsidy_share",protoMsg.sc_niu_special_subsidy_share)
addProtoMsgDef("cs_daily_checkin_req",protoMsg.cs_daily_checkin_req)
addProtoMsgDef("sc_daily_checkin_reply",protoMsg.sc_daily_checkin_reply)
addProtoMsgDef("sc_daily_checkin_info_update",protoMsg.sc_daily_checkin_info_update)
addProtoMsgDef("pb_checkin_info",protoMsg.pb_checkin_info)
addProtoMsgDef("cs_make_up_for_checkin_req",protoMsg.cs_make_up_for_checkin_req)
addProtoMsgDef("sc_player_phone_num_info_update",protoMsg.sc_player_phone_num_info_update)
addProtoMsgDef("sc_player_bind_phone_num",protoMsg.sc_player_bind_phone_num)
addProtoMsgDef("cs_player_bind_phone_num_draw",protoMsg.cs_player_bind_phone_num_draw)
addProtoMsgDef("sc_player_bind_phone_num_draw_reply",protoMsg.sc_player_bind_phone_num_draw_reply)
addProtoMsgDef("sc_niu_special_subsidy_info_update",protoMsg.sc_niu_special_subsidy_info_update)
addProtoMsgDef("cs_rank_query_req",protoMsg.cs_rank_query_req)
addProtoMsgDef("sc_rank_qurey_reply",protoMsg.sc_rank_qurey_reply)
addProtoMsgDef("pb_rank_info",protoMsg.pb_rank_info)
addProtoMsgDef("cs_vip_daily_reward",protoMsg.cs_vip_daily_reward)
addProtoMsgDef("sc_vip_daily_reward",protoMsg.sc_vip_daily_reward)
addProtoMsgDef("sc_guide_info_update",protoMsg.sc_guide_info_update)
addProtoMsgDef("cs_guide_next_step_req",protoMsg.cs_guide_next_step_req)
addProtoMsgDef("sc_guide_next_step_reply",protoMsg.sc_guide_next_step_reply)
addProtoMsgDef("cs_hundred_last_week_rank_query_req",protoMsg.cs_hundred_last_week_rank_query_req)
addProtoMsgDef("sc_hundred_last_week_rank_query_reply",protoMsg.sc_hundred_last_week_rank_query_reply)
addProtoMsgDef("pb_hundred_last_week_data",protoMsg.pb_hundred_last_week_data)
addProtoMsgDef("cs_real_name_update",protoMsg.cs_real_name_update)
addProtoMsgDef("sc_real_name_update",protoMsg.sc_real_name_update)
addProtoMsgDef("cs_real_name_req",protoMsg.cs_real_name_req)
addProtoMsgDef("sc_real_name_req",protoMsg.sc_real_name_req)
addProtoMsgDef("cs_super_laba_last_week_rank_query_req",protoMsg.cs_super_laba_last_week_rank_query_req)
addProtoMsgDef("sc_super_laba_last_week_rank_query_reply",protoMsg.sc_super_laba_last_week_rank_query_reply)
addProtoMsgDef("sc_prize_config_update",protoMsg.sc_prize_config_update)
addProtoMsgDef("pb_prize_info",protoMsg.pb_prize_info)
addProtoMsgDef("cs_prize_query_one_req",protoMsg.cs_prize_query_one_req)
addProtoMsgDef("sc_prize_query_one_reply",protoMsg.sc_prize_query_one_reply)
addProtoMsgDef("cs_prize_exchange_req",protoMsg.cs_prize_exchange_req)
addProtoMsgDef("sc_prize_exchange_reply",protoMsg.sc_prize_exchange_reply)
addProtoMsgDef("sc_prize_exchange_record_update",protoMsg.sc_prize_exchange_record_update)
addProtoMsgDef("pb_prize_exchange_record",protoMsg.pb_prize_exchange_record)
addProtoMsgDef("sc_prize_address_info_update",protoMsg.sc_prize_address_info_update)
addProtoMsgDef("pb_prize_address_info",protoMsg.pb_prize_address_info)
addProtoMsgDef("cs_prize_address_change_req",protoMsg.cs_prize_address_change_req)
addProtoMsgDef("sc_prize_address_change_reply",protoMsg.sc_prize_address_change_reply)
addProtoMsgDef("sc_prize_storage_red_point_update",protoMsg.sc_prize_storage_red_point_update)
addProtoMsgDef("pb_prize_query_one",protoMsg.pb_prize_query_one)
addProtoMsgDef("cs_prize_query_phonecard_key_req",protoMsg.cs_prize_query_phonecard_key_req)
addProtoMsgDef("sc_prize_query_phonecard_key_reply",protoMsg.sc_prize_query_phonecard_key_reply)
addProtoMsgDef("cs_red_pack_query_list_req",protoMsg.cs_red_pack_query_list_req)
addProtoMsgDef("sc_red_pack_query_list_reply",protoMsg.sc_red_pack_query_list_reply)
addProtoMsgDef("pb_red_pack_info",protoMsg.pb_red_pack_info)
addProtoMsgDef("cs_red_pack_open_req",protoMsg.cs_red_pack_open_req)
addProtoMsgDef("sc_red_pack_open_reply",protoMsg.sc_red_pack_open_reply)
addProtoMsgDef("cs_red_pack_create_req",protoMsg.cs_red_pack_create_req)
addProtoMsgDef("sc_red_pack_create_reply",protoMsg.sc_red_pack_create_reply)
addProtoMsgDef("sc_red_pack_notice_update",protoMsg.sc_red_pack_notice_update)
addProtoMsgDef("pb_red_pack_notice",protoMsg.pb_red_pack_notice)
addProtoMsgDef("cs_red_pack_cancel_req",protoMsg.cs_red_pack_cancel_req)
addProtoMsgDef("sc_red_pack_cancel_reply",protoMsg.sc_red_pack_cancel_reply)
addProtoMsgDef("sc_self_red_pack_info",protoMsg.sc_self_red_pack_info)
addProtoMsgDef("cs_red_pack_do_select_req",protoMsg.cs_red_pack_do_select_req)
addProtoMsgDef("sc_red_pack_do_select_reply",protoMsg.sc_red_pack_do_select_reply)
addProtoMsgDef("cs_red_pack_search_req",protoMsg.cs_red_pack_search_req)
addProtoMsgDef("sc_red_pack_search_reply",protoMsg.sc_red_pack_search_reply)
addProtoMsgDef("cs_share_new_bee_reward_req",protoMsg.cs_share_new_bee_reward_req)
addProtoMsgDef("sc_share_new_bee_reward_reply",protoMsg.sc_share_new_bee_reward_reply)
addProtoMsgDef("cs_share_mission_reward_req",protoMsg.cs_share_mission_reward_req)
addProtoMsgDef("sc_share_mission_reward_reply",protoMsg.sc_share_mission_reward_reply)
addProtoMsgDef("pb_share_mission",protoMsg.pb_share_mission)
addProtoMsgDef("sc_share_info",protoMsg.sc_share_info)
addProtoMsgDef("sc_share_mission_update",protoMsg.sc_share_mission_update)
addProtoMsgDef("cs_share_draw_request",protoMsg.cs_share_draw_request)
addProtoMsgDef("cs_share_friend_request",protoMsg.cs_share_friend_request)
addProtoMsgDef("cs_share_rank_request",protoMsg.cs_share_rank_request)
addProtoMsgDef("sc_share_draw_response",protoMsg.sc_share_draw_response)
addProtoMsgDef("pb_share_history_item_response",protoMsg.pb_share_history_item_response)
addProtoMsgDef("pb_share_history_friend_item_response",protoMsg.pb_share_history_friend_item_response)
addProtoMsgDef("sc_share_history_response",protoMsg.sc_share_history_response)
addProtoMsgDef("pb_share_rank_item_response",protoMsg.pb_share_rank_item_response)
addProtoMsgDef("sc_share_rank_response",protoMsg.sc_share_rank_response)
addProtoMsgDef("sc_draw_count_response",protoMsg.sc_draw_count_response)
addProtoMsgDef("sc_task_seven_info_response",protoMsg.sc_task_seven_info_response)
addProtoMsgDef("cs_task_seven_award_request",protoMsg.cs_task_seven_award_request)
addProtoMsgDef("sc_task_seven_award_response",protoMsg.sc_task_seven_award_response)
addProtoMsgDef("cs_share_with_friends_req",protoMsg.cs_share_with_friends_req)
addProtoMsgDef("sc_shop_all_item_base_config",protoMsg.sc_shop_all_item_base_config)
addProtoMsgDef("pb_shop_item",protoMsg.pb_shop_item)
addProtoMsgDef("pb_cost_type_and_num",protoMsg.pb_cost_type_and_num)
addProtoMsgDef("cs_shop_buy_query",protoMsg.cs_shop_buy_query)
addProtoMsgDef("sc_shop_buy_reply",protoMsg.sc_shop_buy_reply)
addProtoMsgDef("sc_golden_bull_info_update",protoMsg.sc_golden_bull_info_update)
addProtoMsgDef("cs_golden_bull_draw_req",protoMsg.cs_golden_bull_draw_req)
addProtoMsgDef("sc_golden_bull_draw_reply",protoMsg.sc_golden_bull_draw_reply)
addProtoMsgDef("sc_month_card_info_update",protoMsg.sc_month_card_info_update)
addProtoMsgDef("cs_month_card_draw_req",protoMsg.cs_month_card_draw_req)
addProtoMsgDef("sc_month_card_draw_reply",protoMsg.sc_month_card_draw_reply)
addProtoMsgDef("cs_cash_transformation_req",protoMsg.cs_cash_transformation_req)
addProtoMsgDef("sc_cash_transformation_reply",protoMsg.sc_cash_transformation_reply)
addProtoMsgDef("cs_golden_bull_mission",protoMsg.cs_golden_bull_mission)
addProtoMsgDef("sc_golden_bull_mission",protoMsg.sc_golden_bull_mission)


-- **************************监听测试**************************
addProtoMsgDef(250,protoMsg.sc_activity_config_info_update)		--活动配置 只登入时下发
addProtoMsgDef(251,protoMsg.cs_activity_info_query_req)		--活动数据查询
addProtoMsgDef(252,protoMsg.sc_activity_info_query_reply)		--活动数据查询 返回
addProtoMsgDef(253,protoMsg.cs_activity_draw_req)		--活动领奖
addProtoMsgDef(254,protoMsg.sc_activity_draw_reply)		--活动领奖
addProtoMsgDef(257,protoMsg.cs_task_pay_award_request)		--一本万利
addProtoMsgDef(258,protoMsg.sc_task_pay_award_response)		--一本万利返回
addProtoMsgDef(259,protoMsg.sc_task_pay_info_response)		--一本万利
addProtoMsgDef(264,protoMsg.cs_task_pay_info_request)		--一本万利查询
addProtoMsgDef(280,protoMsg.cs_car_enter_req)		--进入豪车
addProtoMsgDef(281,protoMsg.sc_car_enter_reply)		--进入豪车返回
addProtoMsgDef(282,protoMsg.cs_car_exit_req)		--离开豪车
addProtoMsgDef(283,protoMsg.sc_car_exit_reply)		--离开豪车返回
addProtoMsgDef(284,protoMsg.cs_car_master_req)		--排庄
addProtoMsgDef(285,protoMsg.sc_car_master_reply)		--排庄返回
addProtoMsgDef(286,protoMsg.cs_car_bet_req)		--下注
addProtoMsgDef(287,protoMsg.sc_car_bet_reply)		--下注返回
addProtoMsgDef(288,protoMsg.cs_car_rebet_req)		--续压
addProtoMsgDef(289,protoMsg.sc_car_rebet_reply)		--续压返回
addProtoMsgDef(290,protoMsg.cs_car_master_list_req)		--排庄列表
addProtoMsgDef(291,protoMsg.cs_car_user_list_req)		--在线列表
addProtoMsgDef(292,protoMsg.sc_car_user_list_reply)		--在线列表返回
addProtoMsgDef(294,protoMsg.sc_car_result_history_req)		--开奖结果历史
addProtoMsgDef(295,protoMsg.sc_car_master_wait_list_reply)		--排庄列表
addProtoMsgDef(296,protoMsg.sc_car_master_info_reply)		--庄家信息
addProtoMsgDef(297,protoMsg.sc_car_status_reply)		--盘面状态
addProtoMsgDef(298,protoMsg.sc_car_room_info_reply)		--房间信息
addProtoMsgDef(299,protoMsg.sc_car_hint_reply)		--提示信息
addProtoMsgDef(300,protoMsg.sc_car_result_reply)		--开奖结果
addProtoMsgDef(301,protoMsg.sc_car_pool_reply)		--奖池总额
addProtoMsgDef(303,protoMsg.cs_car_add_money_req)		--加钱
addProtoMsgDef(304,protoMsg.sc_car_add_money_reply)		--加钱返回
addProtoMsgDef(305,protoMsg.cs_car_syn_in_game_state_req)		--玩家在玩豪车状态通知消息 无返回
addProtoMsgDef(142,protoMsg.cs_player_niu_room_chest_draw)		--领取对局宝箱
addProtoMsgDef(143,protoMsg.sc_niu_room_chest_draw_reply)		--领取宝箱返回
addProtoMsgDef(144,protoMsg.sc_niu_room_chest_info_update)		--对局信息更新
addProtoMsgDef(148,protoMsg.sc_niu_room_chest_times_update)		--对局领取免费钻石次数
addProtoMsgDef(8,protoMsg.cs_common_heartbeat)		--心跳协议
addProtoMsgDef(9,protoMsg.sc_common_heartbeat_reply)		--心跳协议返回
addProtoMsgDef(10,protoMsg.cs_common_proto_count)		--更新客户端已接收的协议计数
addProtoMsgDef(11,protoMsg.sc_common_proto_count)		--更新服务端已接收的协议计数
addProtoMsgDef(12,protoMsg.cs_common_proto_clean)		--通知服务端清理协议缓存
addProtoMsgDef(13,protoMsg.sc_common_proto_clean)		--通知客户端清理协议缓存
addProtoMsgDef(28,protoMsg.cs_common_bug_feedback)		--玩家BUG反馈
addProtoMsgDef(29,protoMsg.sc_common_bug_feedback)		--玩家BUG反馈返回
addProtoMsgDef(90,protoMsg.sc_hundred_niu_room_state_update)		--房间状态同步 接收到时开始倒计时
addProtoMsgDef(91,protoMsg.cs_hundred_niu_enter_room_req)		--玩家进入房间
addProtoMsgDef(92,protoMsg.sc_hundred_niu_enter_room_reply)		--玩家进入房间返回
addProtoMsgDef(93,protoMsg.cs_hundred_niu_player_list_query_req)		--玩家查询闲家人员列表
addProtoMsgDef(94,protoMsg.sc_hundred_niu_player_list_query_reply)		--玩家查询闲家人员列表返回
addProtoMsgDef(95,protoMsg.cs_hundred_niu_free_set_chips_req)		--闲家下注
addProtoMsgDef(96,protoMsg.sc_hundred_niu_free_set_chips_reply)		--闲家下注返回
addProtoMsgDef(97,protoMsg.sc_hundred_niu_free_set_chips_update)		--闲家下注更新
addProtoMsgDef(98,protoMsg.cs_hundred_niu_sit_down_req)		--上下座
addProtoMsgDef(99,protoMsg.sc_hundred_niu_sit_down_reply)		--上下座返回
addProtoMsgDef(100,protoMsg.sc_hundred_niu_seat_player_info_update)		--座位上人员信息 ( 包括庄家(pos=0)金币变化 ) 通过player_uuid比较得知位置上是否自己
addProtoMsgDef(101,protoMsg.cs_hundred_be_master_req)		--上庄
addProtoMsgDef(102,protoMsg.sc_hundred_be_master_reply)		--上庄返回
addProtoMsgDef(103,protoMsg.cs_hundred_query_master_list_req)		--获取上庄列表
addProtoMsgDef(104,protoMsg.sc_hundred_query_master_list_reply)		--上庄列表更新
addProtoMsgDef(105,protoMsg.cs_hundred_niu_in_game_syn_req)		--同步在游戏中 (结算消息收到后立即发送)
addProtoMsgDef(106,protoMsg.cs_hundred_leave_room_req)		--离开房间
addProtoMsgDef(107,protoMsg.sc_hundred_leave_room_reply)		--离开房间返回
addProtoMsgDef(108,protoMsg.cs_hundred_query_winning_rec_req)		--查询押注走势
addProtoMsgDef(109,protoMsg.sc_hundred_query_winning_rec_reply)		--查询押注走势返回
addProtoMsgDef(120,protoMsg.sc_hundred_player_gold_change_update)		--百人 自己金币变动更新 (在结算 奖池开奖 下注时 都主动发送)
addProtoMsgDef(121,protoMsg.cs_hundred_query_pool_win_player_req)		--查询上次分奖池钱最多的人
addProtoMsgDef(122,protoMsg.sc_hundred_query_pool_win_player_reply)		--查询上次分奖池钱最多的人返回
addProtoMsgDef(30,protoMsg.sc_items_update)		--物品更新
addProtoMsgDef(31,protoMsg.sc_items_add)		--物品新增
addProtoMsgDef(32,protoMsg.sc_items_delete)		--物品删除
addProtoMsgDef(33,protoMsg.sc_items_init_update)		--背包初始化
addProtoMsgDef(34,protoMsg.cs_item_use_req)		--物品使用
addProtoMsgDef(35,protoMsg.sc_item_use_reply)		--物品使用返回
addProtoMsgDef(201,protoMsg.cs_laba_enter_room_req)		--进入房间
addProtoMsgDef(202,protoMsg.sc_laba_enter_room_reply)		--进入房间返回
addProtoMsgDef(203,protoMsg.cs_laba_leave_room_req)		--离开房间
addProtoMsgDef(204,protoMsg.sc_laba_leave_room_reply)		--离开房间返回 玩家下线时检测 从容器中去除
addProtoMsgDef(205,protoMsg.sc_laba_pool_num_update)		--奖池数量更新 发送给在房间的人
addProtoMsgDef(206,protoMsg.cs_laba_spin_req)		--投注
addProtoMsgDef(207,protoMsg.sc_laba_spin_reply)		--投注返回
addProtoMsgDef(217,protoMsg.cs_win_player_list)		--中奖人信息
addProtoMsgDef(218,protoMsg.sc_win_player_list)		--中奖人信息返回
addProtoMsgDef(1,protoMsg.cs_login)		--请求登陆
addProtoMsgDef(2,protoMsg.sc_login_reply)		--登陆返回 
addProtoMsgDef(3,protoMsg.cs_login_out)		--退出登陆 
addProtoMsgDef(4,protoMsg.cs_login_reconnection)		--请求重连  
addProtoMsgDef(5,protoMsg.sc_login_reconnection_reply)		--请求重连返回  
addProtoMsgDef(6,protoMsg.sc_login_repeat)		--重复登陆
addProtoMsgDef(7,protoMsg.sc_login_proto_complete)		--登陆协议全部发送成功  
addProtoMsgDef(40,protoMsg.sc_mails_init_update)		--邮件初始化更新
addProtoMsgDef(41,protoMsg.sc_mail_add)		--发系统邮件
addProtoMsgDef(42,protoMsg.cs_mail_delete_request)		--删除邮件
addProtoMsgDef(43,protoMsg.sc_mail_delete_reply)		--删除邮件返回
addProtoMsgDef(44,protoMsg.cs_read_mail)		--读邮件
addProtoMsgDef(45,protoMsg.cs_mail_draw_request)		--领取邮件的请求
addProtoMsgDef(46,protoMsg.sc_mail_draw_reply)		--领取邮件返回
addProtoMsgDef(110,protoMsg.cs_draw_mission_request)		--领取任务奖励
addProtoMsgDef(111,protoMsg.sc_draw_mission_result_reply)		--领取任务奖励返回
addProtoMsgDef(112,protoMsg.sc_mission)		--更新任务信息,登入时发送
addProtoMsgDef(113,protoMsg.sc_mission_update)		--更新单条任务
addProtoMsgDef(114,protoMsg.sc_mission_add)		--增加单条任务
addProtoMsgDef(115,protoMsg.sc_mission_del)		--删除单条任务
addProtoMsgDef(208,protoMsg.sc_game_task_info_update)		--游戏中任务信息 百人和水果
addProtoMsgDef(209,protoMsg.sc_game_task_box_info_update)		--游戏中累计任务信息更新
addProtoMsgDef(210,protoMsg.cs_game_task_draw_req)		--任务奖励领取
addProtoMsgDef(211,protoMsg.sc_game_task_draw_reply)		--任务奖励领取 返回
addProtoMsgDef(212,protoMsg.cs_game_task_box_draw_req)		--宝箱任务奖励领取
addProtoMsgDef(213,protoMsg.sc_game_task_box_draw_reply)		--宝箱任务奖励领取 返回
addProtoMsgDef(214,protoMsg.sc_redpack_task_draw_list_update)		--红包任务 领奖列表 进入房间 和 任务赢钱数变化时更新
addProtoMsgDef(215,protoMsg.cs_redpack_task_draw_req)		--红包任务奖励领取
addProtoMsgDef(216,protoMsg.sc_redpack_task_draw_reply)		--红包任务奖励领取 返回
addProtoMsgDef(50,protoMsg.sc_niu_room_state_update)		--房间状态同步 接收到时开始倒计时
addProtoMsgDef(51,protoMsg.cs_niu_enter_room_req)		--玩家进入房间
addProtoMsgDef(52,protoMsg.sc_niu_enter_room_reply)		--玩家进入房间返回
addProtoMsgDef(53,protoMsg.sc_niu_enter_room_player_info_update)		--新加入的玩家信息更新
addProtoMsgDef(54,protoMsg.cs_niu_choose_master_rate_req)		--抢庄 选倍率
addProtoMsgDef(55,protoMsg.sc_niu_choose_master_rate_reply)		--抢庄 选倍率返回
addProtoMsgDef(56,protoMsg.sc_niu_player_choose_master_rate_update)		--抢庄 选倍率信息更新
addProtoMsgDef(57,protoMsg.cs_niu_choose_free_rate_req)		--闲家下注
addProtoMsgDef(58,protoMsg.sc_niu_choose_free_rate_reply)		--闲家下注返回
addProtoMsgDef(59,protoMsg.sc_niu_player_choose_free_rate_update)		--闲家下注更新
addProtoMsgDef(60,protoMsg.cs_niu_leave_room_req)		--玩家离开房间(  发完该消息后切勿在发送在玩同步消息  )
addProtoMsgDef(61,protoMsg.sc_niu_leave_room_reply)		--玩家离开返回
addProtoMsgDef(62,protoMsg.sc_niu_leave_room_player_pos_update)		--离开玩家位置更新
addProtoMsgDef(63,protoMsg.cs_niu_submit_card_req)		--提交牌型
addProtoMsgDef(64,protoMsg.sc_niu_submit_card_reply)		--提交牌型返回
addProtoMsgDef(65,protoMsg.sc_niu_player_submit_card_update)		--提交牌型更新
addProtoMsgDef(66,protoMsg.cs_niu_syn_in_game_state_req)		--玩家在玩牛牛状态通知消息 无返回(收到20状态消息后发该消息即可)
addProtoMsgDef(72,protoMsg.cs_niu_query_player_room_info_req)		--查询玩家房间状态信息 返回下面的消息
addProtoMsgDef(67,protoMsg.sc_niu_player_room_info_update)		--登入时通知客户端上局游戏是否还在继续
addProtoMsgDef(68,protoMsg.sc_niu_player_back_to_room_info_update)		--返回房间下发的更新消息 只发给进入玩家
addProtoMsgDef(160,protoMsg.sc_redpack_room_reset_times_update)		--房间次数消息 登入时 , 变化时 , 隔天时 同步
addProtoMsgDef(161,protoMsg.sc_redpack_room_player_times_update)		--当前局数信息 进房间 变化时 登入 同步
addProtoMsgDef(162,protoMsg.sc_redpack_room_redpack_notice_update)		--红包可领取状态同步 收到该消息后一段时间内显示可领取红包
addProtoMsgDef(163,protoMsg.cs_redpack_room_draw_req)		--红包领取请求
addProtoMsgDef(164,protoMsg.sc_redpack_room_draw_reply)		--红包领取请求 返回
addProtoMsgDef(165,protoMsg.sc_redpack_redpack_timer_sec_update)		--进房间时同步的领奖时间
addProtoMsgDef(166,protoMsg.cs_redpack_relive_req)		--红包场复活
addProtoMsgDef(167,protoMsg.sc_redpack_relive_reply)		--红包场复活返回
addProtoMsgDef(168,protoMsg.sc_redpack_relive_times)		--红包场复活剩余次数
addProtoMsgDef(169,protoMsg.sc_fudai_pool_update)		--福袋池更新
addProtoMsgDef(14,protoMsg.sc_player_base_info)		--玩家信息
addProtoMsgDef(15,protoMsg.cs_player_change_name_req)		--修改名字
addProtoMsgDef(16,protoMsg.sc_player_change_name_reply)		--修改名字返回
addProtoMsgDef(17,protoMsg.cs_player_change_headicon_req)		--修改头像
addProtoMsgDef(18,protoMsg.sc_player_change_headicon_reply)		--修改头像返回
addProtoMsgDef(19,protoMsg.cs_player_chat)		--聊天
addProtoMsgDef(20,protoMsg.sc_player_chat)		--聊天返回
addProtoMsgDef(21,protoMsg.sc_player_sys_notice)		--系统公告
addProtoMsgDef(22,protoMsg.sc_tips)		--服务端主动发给客户端的提示文字
addProtoMsgDef(23,protoMsg.cs_query_player_winning_rec_req)		--查询玩家胜负记录
addProtoMsgDef(24,protoMsg.sc_query_player_winning_rec_reply)		--查询玩家胜负记录返回
addProtoMsgDef(25,protoMsg.cs_niu_query_in_game_player_num_req)		--请求获取在游戏中的人数
addProtoMsgDef(26,protoMsg.sc_niu_query_in_game_player_num_reply)		--请求获取在游戏中的人数返回
addProtoMsgDef(69,protoMsg.cs_niu_subsidy_req)		--破产补助
addProtoMsgDef(70,protoMsg.sc_niu_subsidy_reply)		--破产补助请求返回
addProtoMsgDef(71,protoMsg.sc_niu_subsidy_info_update)		--破产补助信息
addProtoMsgDef(73,protoMsg.cs_niu_special_subsidy_share)		--破产特别补助分享
addProtoMsgDef(74,protoMsg.sc_niu_special_subsidy_share)		--破产特别补助分享返回
addProtoMsgDef(75,protoMsg.cs_daily_checkin_req)		--每日签到
addProtoMsgDef(76,protoMsg.sc_daily_checkin_reply)		--每日签到  和 补签 返回
addProtoMsgDef(77,protoMsg.sc_daily_checkin_info_update)		--签到配置信息
addProtoMsgDef(78,protoMsg.cs_make_up_for_checkin_req)		--补签
addProtoMsgDef(140,protoMsg.sc_player_phone_num_info_update)		--手机号信息更新
addProtoMsgDef(141,protoMsg.sc_player_bind_phone_num)		--绑定手机号返回
addProtoMsgDef(145,protoMsg.cs_player_bind_phone_num_draw)		--绑定手机号领取奖励
addProtoMsgDef(146,protoMsg.sc_player_bind_phone_num_draw_reply)		--绑定手机号领奖返回
addProtoMsgDef(147,protoMsg.sc_niu_special_subsidy_info_update)		--破产特别补助信息
addProtoMsgDef(123,protoMsg.cs_rank_query_req)		--排行榜查询
addProtoMsgDef(124,protoMsg.sc_rank_qurey_reply)		--排行榜查询返回
addProtoMsgDef(240,protoMsg.cs_vip_daily_reward)		--领取VIP特别奖励
addProtoMsgDef(241,protoMsg.sc_vip_daily_reward)		--领取VIP特别奖励返回
addProtoMsgDef(150,protoMsg.sc_guide_info_update)		--新手引导 更新
addProtoMsgDef(151,protoMsg.cs_guide_next_step_req)		--新手引导请求
addProtoMsgDef(152,protoMsg.sc_guide_next_step_reply)		--新手引导请求返回 (主动发更新)
addProtoMsgDef(153,protoMsg.cs_hundred_last_week_rank_query_req)		--百人上周中奖
addProtoMsgDef(154,protoMsg.sc_hundred_last_week_rank_query_reply)		--百人上周中奖返回
addProtoMsgDef(155,protoMsg.cs_real_name_update)		--实名制
addProtoMsgDef(156,protoMsg.sc_real_name_update)		--实名制返回
addProtoMsgDef(157,protoMsg.cs_real_name_req)		--实名制查询
addProtoMsgDef(158,protoMsg.sc_real_name_req)		--实名制查询返回
addProtoMsgDef(320,protoMsg.cs_super_laba_last_week_rank_query_req)		--超级拉霸上周中奖名单
addProtoMsgDef(321,protoMsg.sc_super_laba_last_week_rank_query_reply)		--超级拉霸上周中奖名单返回
addProtoMsgDef(230,protoMsg.sc_prize_config_update)		--奖品兑换更新 登入时发
addProtoMsgDef(231,protoMsg.cs_prize_query_one_req)		--查询物品库存
addProtoMsgDef(232,protoMsg.sc_prize_query_one_reply)		--查询物品库存 返回
addProtoMsgDef(233,protoMsg.cs_prize_exchange_req)		--兑换
addProtoMsgDef(234,protoMsg.sc_prize_exchange_reply)		--兑换返回 ( 同时发查询物品库存返回)
addProtoMsgDef(235,protoMsg.sc_prize_exchange_record_update)		--兑换记录 登入和更新时发送
addProtoMsgDef(236,protoMsg.sc_prize_address_info_update)		--地址更新 登入时和改变时发送
addProtoMsgDef(237,protoMsg.cs_prize_address_change_req)		--地址修改
addProtoMsgDef(238,protoMsg.sc_prize_address_change_reply)		--地址修改返回
addProtoMsgDef(239,protoMsg.sc_prize_storage_red_point_update)		--同步实物库存 红点
addProtoMsgDef(255,protoMsg.cs_prize_query_phonecard_key_req)		--查询卡密
addProtoMsgDef(256,protoMsg.sc_prize_query_phonecard_key_reply)		--查询卡密返回
addProtoMsgDef(220,protoMsg.cs_red_pack_query_list_req)		--查询红包列表
addProtoMsgDef(221,protoMsg.sc_red_pack_query_list_reply)		--查询红包列表返回
addProtoMsgDef(222,protoMsg.cs_red_pack_open_req)		--拆红包
addProtoMsgDef(223,protoMsg.sc_red_pack_open_reply)		--拆红包返回
addProtoMsgDef(224,protoMsg.cs_red_pack_create_req)		--发红包
addProtoMsgDef(225,protoMsg.sc_red_pack_create_reply)		--发红包返回
addProtoMsgDef(226,protoMsg.sc_red_pack_notice_update)		--红包通知
addProtoMsgDef(227,protoMsg.cs_red_pack_cancel_req)		--取消红包
addProtoMsgDef(228,protoMsg.sc_red_pack_cancel_reply)		--取消红包返回
addProtoMsgDef(229,protoMsg.sc_self_red_pack_info)		--总红包数，玩家自己发的红包的红包信息
addProtoMsgDef(260,protoMsg.cs_red_pack_do_select_req)		--红包确认请求
addProtoMsgDef(261,protoMsg.sc_red_pack_do_select_reply)		--红包确认返回
addProtoMsgDef(262,protoMsg.cs_red_pack_search_req)		--红包查询请求
addProtoMsgDef(263,protoMsg.sc_red_pack_search_reply)		--红包查询返回
addProtoMsgDef(266,protoMsg.cs_share_new_bee_reward_req)		--分享新手礼包领取
addProtoMsgDef(267,protoMsg.sc_share_new_bee_reward_reply)		--分享新手礼包领取返回
addProtoMsgDef(268,protoMsg.cs_share_mission_reward_req)		--分享任务奖励领取
addProtoMsgDef(269,protoMsg.sc_share_mission_reward_reply)		--分享任务奖励领取返回
addProtoMsgDef(270,protoMsg.sc_share_info)		--分享信息
addProtoMsgDef(271,protoMsg.sc_share_mission_update)		--分享任务更新
addProtoMsgDef(272,protoMsg.cs_share_draw_request)		--分享抽奖
addProtoMsgDef(273,protoMsg.cs_share_friend_request)		--分享好友进度
addProtoMsgDef(274,protoMsg.cs_share_rank_request)		--分享好友榜单
addProtoMsgDef(275,protoMsg.sc_share_draw_response)		--分享抽奖返回
addProtoMsgDef(276,protoMsg.sc_share_history_response)		--分享好友进度
addProtoMsgDef(277,protoMsg.sc_share_rank_response)		--分享好友榜单
addProtoMsgDef(278,protoMsg.sc_draw_count_response)		--分享抽奖剩余次数
addProtoMsgDef(310,protoMsg.sc_task_seven_info_response)		--7日狂欢任务信息
addProtoMsgDef(311,protoMsg.cs_task_seven_award_request)		--7日狂欢领奖
addProtoMsgDef(312,protoMsg.sc_task_seven_award_response)		--7日狂欢领奖返回
addProtoMsgDef(313,protoMsg.cs_share_with_friends_req)		--分享朋友圈
addProtoMsgDef(80,protoMsg.sc_shop_all_item_base_config)		--商店所有物品信息
addProtoMsgDef(81,protoMsg.cs_shop_buy_query)		--商店购买
addProtoMsgDef(82,protoMsg.sc_shop_buy_reply)		--商店购买返回
addProtoMsgDef(83,protoMsg.sc_golden_bull_info_update)		--金牛领奖信息 登入和领完奖时同步
addProtoMsgDef(84,protoMsg.cs_golden_bull_draw_req)		--金牛领奖
addProtoMsgDef(85,protoMsg.sc_golden_bull_draw_reply)		--金牛领奖返回
addProtoMsgDef(86,protoMsg.sc_month_card_info_update)		--月卡信息更新 登入 隔天 和 领奖返回时下发
addProtoMsgDef(87,protoMsg.cs_month_card_draw_req)		--月卡领奖
addProtoMsgDef(88,protoMsg.sc_month_card_draw_reply)		--月卡领奖返回
addProtoMsgDef(130,protoMsg.cs_cash_transformation_req)		--奖券兑换
addProtoMsgDef(131,protoMsg.sc_cash_transformation_reply)		--奖券兑换返回
addProtoMsgDef(132,protoMsg.cs_golden_bull_mission)		--金牛任务进度
addProtoMsgDef(133,protoMsg.sc_golden_bull_mission)		--


-- 执行检查
checkProtoMsgDef()


