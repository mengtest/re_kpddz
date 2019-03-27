using System;
using System.Collections.Generic;
using System.IO;
using network;
using ProtoBuf;

public enum ProtoID
{
    // 活动配置 只登入时下发
	sc_activity_config_info_update = 250,
	// 活动数据查询
	cs_activity_info_query_req = 251,
	// 活动数据查询 返回
	sc_activity_info_query_reply = 252,
	// 活动领奖
	cs_activity_draw_req = 253,
	// 活动领奖
	sc_activity_draw_reply = 254,
	// 进入豪车
	cs_car_enter_req = 280,
	// 进入豪车返回
	sc_car_enter_reply = 281,
	// 离开豪车
	cs_car_exit_req = 282,
	// 离开豪车返回
	sc_car_exit_reply = 283,
	// 排庄
	cs_car_master_req = 284,
	// 排庄返回
	sc_car_master_reply = 285,
	// 下注
	cs_car_bet_req = 286,
	// 下注返回
	sc_car_bet_reply = 287,
	// 续压
	cs_car_rebet_req = 288,
	// 续压返回
	sc_car_rebet_reply = 289,
	// 排庄列表
	cs_car_master_list_req = 290,
	// 在线列表
	cs_car_user_list_req = 291,
	// 在线列表返回
	sc_car_user_list_reply = 292,
	// 开奖结果历史
	sc_car_result_history_req = 294,
	// 排庄列表
	sc_car_master_wait_list_reply = 295,
	// 庄家信息
	sc_car_master_info_reply = 296,
	// 盘面状态
	sc_car_status_reply = 297,
	// 房间信息
	sc_car_room_info_reply = 298,
	// 提示信息
	sc_car_hint_reply = 299,
	// 开奖结果
	sc_car_result_reply = 300,
	// 奖池总额
	sc_car_pool_reply = 301,
	// 加钱
	cs_car_add_money_req = 303,
	// 加钱返回
	sc_car_add_money_reply = 304,
	// 玩家在玩豪车状态通知消息 无返回
	cs_car_syn_in_game_state_req = 305,
	// 领取对局宝箱
	cs_player_niu_room_chest_draw = 142,
	// 领取宝箱返回
	sc_niu_room_chest_draw_reply = 143,
	// 对局信息更新
	sc_niu_room_chest_info_update = 144,
	// 对局领取免费钻石次数
	sc_niu_room_chest_times_update = 148,
	// 心跳协议
	CS_COMMON_HEARTBEAT = 8,
	// 心跳协议返回
	SC_COMMON_HEARTBEAT_REPLY = 9,
	// 更新客户端已接收的协议计数
	CS_COMMON_PROTO_COUNT = 10,
	// 更新服务端已接收的协议计数
	SC_COMMON_PROTO_COUNT = 11,
	// 通知服务端清理协议缓存
	CS_COMMON_PROTO_CLEAN = 12,
	// 通知客户端清理协议缓存
	SC_COMMON_PROTO_CLEAN = 13,
	// 玩家BUG反馈
	CS_COMMON_BUG_FEEDBACK = 28,
	// 玩家BUG反馈返回
	SC_COMMON_BUG_FEEDBACK = 29,
	// 房间状态同步 接收到时开始倒计时
	sc_hundred_niu_room_state_update = 90,
	// 玩家进入房间
	cs_hundred_niu_enter_room_req = 91,
	// 玩家进入房间返回
	sc_hundred_niu_enter_room_reply = 92,
	// 玩家查询闲家人员列表
	cs_hundred_niu_player_list_query_req = 93,
	// 玩家查询闲家人员列表返回
	sc_hundred_niu_player_list_query_reply = 94,
	// 闲家下注
	cs_hundred_niu_free_set_chips_req = 95,
	// 闲家下注返回
	sc_hundred_niu_free_set_chips_reply = 96,
	// 闲家下注更新
	sc_hundred_niu_free_set_chips_update = 97,
	// 上下座
	cs_hundred_niu_sit_down_req = 98,
	// 上下座返回
	sc_hundred_niu_sit_down_reply = 99,
	// 座位上人员信息 ( 包括庄家(pos=0)金币变化 ) 通过player_uuid比较得知位置上是否自己
	sc_hundred_niu_seat_player_info_update = 100,
	// 上庄
	cs_hundred_be_master_req = 101,
	// 上庄返回
	sc_hundred_be_master_reply = 102,
	// 获取上庄列表
	cs_hundred_query_master_list_req = 103,
	// 上庄列表更新
	sc_hundred_query_master_list_reply = 104,
	// 同步在游戏中 (结算消息收到后立即发送)
	cs_hundred_niu_in_game_syn_req = 105,
	// 离开房间
	cs_hundred_leave_room_req = 106,
	// 离开房间返回
	sc_hundred_leave_room_reply = 107,
	// 查询押注走势
	cs_hundred_query_winning_rec_req = 108,
	// 查询押注走势返回
	sc_hundred_query_winning_rec_reply = 109,
	// 百人 自己金币变动更新 (在结算 奖池开奖 下注时 都主动发送)
	sc_hundred_player_gold_change_update = 120,
	// 查询上次分奖池钱最多的人
	cs_hundred_query_pool_win_player_req = 121,
	// 查询上次分奖池钱最多的人返回
	sc_hundred_query_pool_win_player_reply = 122,
	// 物品更新
	sc_items_update = 30,
	// 物品新增
	sc_items_add = 31,
	// 物品删除
	sc_items_delete = 32,
	// 背包初始化
	sc_items_init_update = 33,
	// 物品使用
	cs_item_use_req = 34,
	// 物品使用返回
	sc_item_use_reply = 35,
	// 进入房间
	cs_laba_enter_room_req = 201,
	// 进入房间返回
	sc_laba_enter_room_reply = 202,
	// 离开房间
	cs_laba_leave_room_req = 203,
	// 离开房间返回 玩家下线时检测 从容器中去除
	sc_laba_leave_room_reply = 204,
	// 奖池数量更新 发送给在房间的人
	sc_laba_pool_num_update = 205,
	// 投注
	cs_laba_spin_req = 206,
	// 投注返回
	sc_laba_spin_reply = 207,
	// 请求登陆
	CS_LOGIN = 1,
	// 登陆返回 
	SC_LOGIN_REPLY = 2,
	// 退出登陆 
	CS_LOGIN_OUT = 3,
	// 请求重连  
	CS_LOGIN_RECONNECTION = 4,
	// 请求重连返回  
	SC_LOGIN_RECONNECTION_REPLY = 5,
	// 重复登陆
	SC_LOGIN_REPEAT = 6,
	// 登陆协议全部发送成功  
	SC_LOGIN_PROTO_COMPLETE = 7,
	// 邮件初始化更新
	sc_mails_init_update = 40,
	// 发系统邮件
	sc_mail_add = 41,
	// 删除邮件
	cs_mail_delete_request = 42,
	// 删除邮件返回
	sc_mail_delete_reply = 43,
	// 读邮件
	cs_read_mail = 44,
	// 领取邮件的请求
	cs_mail_draw_request = 45,
	// 领取邮件返回
	sc_mail_draw_reply = 46,
	// 领取任务奖励
	cs_draw_mission_request = 110,
	// 领取任务奖励返回
	sc_draw_mission_result_reply = 111,
	// 更新任务信息,登入时发送
	sc_mission = 112,
	// 更新单条任务
	sc_mission_update = 113,
	// 增加单条任务
	sc_mission_add = 114,
	// 删除单条任务
	sc_mission_del = 115,
	// 游戏中任务信息 百人和水果
	sc_game_task_info_update = 208,
	// 游戏中累计任务信息更新
	sc_game_task_box_info_update = 209,
	// 任务奖励领取
	cs_game_task_draw_req = 210,
	// 任务奖励领取 返回
	sc_game_task_draw_reply = 211,
	// 宝箱任务奖励领取
	cs_game_task_box_draw_req = 212,
	// 宝箱任务奖励领取 返回
	sc_game_task_box_draw_reply = 213,
	// 红包任务 领奖列表 进入房间 和 任务赢钱数变化时更新
	sc_redpack_task_draw_list_update = 214,
	// 红包任务奖励领取
	cs_redpack_task_draw_req = 215,
	// 红包任务奖励领取 返回
	sc_redpack_task_draw_reply = 216,
	// 房间状态同步 接收到时开始倒计时
	sc_niu_room_state_update = 50,
	// 玩家进入房间
	cs_niu_enter_room_req = 51,
	// 玩家进入房间返回
	sc_niu_enter_room_reply = 52,
	// 新加入的玩家信息更新
	sc_niu_enter_room_player_info_update = 53,
	// 抢庄 选倍率
	cs_niu_choose_master_rate_req = 54,
	// 抢庄 选倍率返回
	sc_niu_choose_master_rate_reply = 55,
	// 抢庄 选倍率信息更新
	sc_niu_player_choose_master_rate_update = 56,
	// 闲家下注
	cs_niu_choose_free_rate_req = 57,
	// 闲家下注返回
	sc_niu_choose_free_rate_reply = 58,
	// 闲家下注更新
	sc_niu_player_choose_free_rate_update = 59,
	// 玩家离开房间(  发完该消息后切勿在发送在玩同步消息  )
	cs_niu_leave_room_req = 60,
	// 玩家离开返回
	sc_niu_leave_room_reply = 61,
	// 离开玩家位置更新
	sc_niu_leave_room_player_pos_update = 62,
	// 提交牌型
	cs_niu_submit_card_req = 63,
	// 提交牌型返回
	sc_niu_submit_card_reply = 64,
	// 提交牌型更新
	sc_niu_player_submit_card_update = 65,
	// 玩家在玩牛牛状态通知消息 无返回(收到20状态消息后发该消息即可)
	cs_niu_syn_in_game_state_req = 66,
	// 查询玩家房间状态信息 返回下面的消息
	cs_niu_query_player_room_info_req = 72,
	// 登入时通知客户端上局游戏是否还在继续
	sc_niu_player_room_info_update = 67,
	// 返回房间下发的更新消息 只发给进入玩家
	sc_niu_player_back_to_room_info_update = 68,
	// 房间次数消息 登入时 , 变化时 , 隔天时 同步
	sc_redpack_room_reset_times_update = 160,
	// 当前局数信息 进房间 变化时 登入 同步
	sc_redpack_room_player_times_update = 161,
	// 红包可领取状态同步 收到该消息后一段时间内显示可领取红包
	sc_redpack_room_redpack_notice_update = 162,
	// 红包领取请求
	cs_redpack_room_draw_req = 163,
	// 红包领取请求 返回
	sc_redpack_room_draw_reply = 164,
	// 进房间时同步的领奖时间
	sc_redpack_redpack_timer_sec_update = 165,
	// 红包场复活
	cs_redpack_relive_req = 166,
	// 红包场复活返回
	sc_redpack_relive_reply = 167,
	// 红包场复活剩余次数
	sc_redpack_relive_times = 168,
	// 福袋池更新
	sc_fudai_pool_update = 169,
	// 玩家信息
	sc_player_base_info = 14,
	// 修改名字
	cs_player_change_name_req = 15,
	// 修改名字返回
	sc_player_change_name_reply = 16,
	// 修改头像
	cs_player_change_headicon_req = 17,
	// 修改头像返回
	sc_player_change_headicon_reply = 18,
	// 聊天
	cs_player_chat = 19,
	// 聊天返回
	sc_player_chat = 20,
	// 系统公告
	sc_player_sys_notice = 21,
	// 服务端主动发给客户端的提示文字
	sc_tips = 22,
	// 查询玩家胜负记录
	cs_query_player_winning_rec_req = 23,
	// 查询玩家胜负记录返回
	sc_query_player_winning_rec_reply = 24,
	// 请求获取在游戏中的人数
	cs_niu_query_in_game_player_num_req = 25,
	// 请求获取在游戏中的人数返回
	sc_niu_query_in_game_player_num_reply = 26,
	// 破产补助
	cs_niu_subsidy_req = 69,
	// 破产补助请求返回
	sc_niu_subsidy_reply = 70,
	// 破产补助信息
	sc_niu_subsidy_info_update = 71,
	// 破产特别补助分享
	cs_niu_special_subsidy_share = 73,
	// 破产特别补助分享返回
	sc_niu_special_subsidy_share = 74,
	// 每日签到
	cs_daily_checkin_req = 75,
	// 每日签到  和 补签 返回
	sc_daily_checkin_reply = 76,
	// 签到配置信息
	sc_daily_checkin_info_update = 77,
	// 补签
	cs_make_up_for_checkin_req = 78,
	// 手机号信息更新
	sc_player_phone_num_info_update = 140,
	// 绑定手机号返回
	sc_player_bind_phone_num = 141,
	// 绑定手机号领取奖励
	cs_player_bind_phone_num_draw = 145,
	// 绑定手机号领奖返回
	sc_player_bind_phone_num_draw_reply = 146,
	// 破产特别补助信息
	sc_niu_special_subsidy_info_update = 147,
	// 排行榜查询
	cs_rank_query_req = 123,
	// 排行榜查询返回
	sc_rank_qurey_reply = 124,
	// 领取VIP特别奖励
	cs_vip_daily_reward = 240,
	// 领取VIP特别奖励返回
	sc_vip_daily_reward = 241,
	// 新手引导 更新
	sc_guide_info_update = 150,
	// 新手引导请求
	cs_guide_next_step_req = 151,
	// 新手引导请求返回 (主动发更新)
	sc_guide_next_step_reply = 152,
	// 百人上周中奖
	cs_hundred_last_week_rank_query_req = 153,
	// 百人上周中奖返回
	sc_hundred_last_week_rank_query_reply = 154,
	// 实名制
	cs_real_name_update = 155,
	// 实名制返回
	sc_real_name_update = 156,
	// 实名制查询
	cs_real_name_req = 157,
	// 实名制查询返回
	sc_real_name_req = 158,
	// 奖品兑换更新 登入时发
	sc_prize_config_update = 230,
	// 查询物品库存
	cs_prize_query_one_req = 231,
	// 查询物品库存 返回
	sc_prize_query_one_reply = 232,
	// 兑换
	cs_prize_exchange_req = 233,
	// 兑换返回 ( 同时发查询物品库存返回)
	sc_prize_exchange_reply = 234,
	// 兑换记录 登入和更新时发送
	sc_prize_exchange_record_update = 235,
	// 地址更新 登入时和改变时发送
	sc_prize_address_info_update = 236,
	// 地址修改
	cs_prize_address_change_req = 237,
	// 地址修改返回
	sc_prize_address_change_reply = 238,
	// 同步实物库存 红点
	sc_prize_storage_red_point_update = 239,
	// 查询卡密
	cs_prize_query_phonecard_key_req = 255,
	// 查询卡密返回
	sc_prize_query_phonecard_key_reply = 256,
	// 查询红包列表
	cs_red_pack_query_list_req = 220,
	// 查询红包列表返回
	sc_red_pack_query_list_reply = 221,
	// 拆红包
	cs_red_pack_open_req = 222,
	// 拆红包返回
	sc_red_pack_open_reply = 223,
	// 发红包
	cs_red_pack_create_req = 224,
	// 发红包返回
	sc_red_pack_create_reply = 225,
	// 红包通知
	sc_red_pack_notice_update = 226,
	// 取消红包
	cs_red_pack_cancel_req = 227,
	// 取消红包返回
	sc_red_pack_cancel_reply = 228,
	// 总红包数，玩家自己发的红包的红包信息
	sc_self_red_pack_info = 229,
	// 红包确认请求
	cs_red_pack_do_select_req = 260,
	// 红包确认返回
	sc_red_pack_do_select_reply = 261,
	// 红包查询请求
	cs_red_pack_search_req = 262,
	// 红包查询返回
	sc_red_pack_search_reply = 263,
	// 分享新手礼包领取
	cs_share_new_bee_reward_req = 266,
	// 分享新手礼包领取返回
	sc_share_new_bee_reward_reply = 267,
	// 分享任务奖励领取
	cs_share_mission_reward_req = 268,
	// 分享任务奖励领取返回
	sc_share_mission_reward_reply = 269,
	// 分享信息
	sc_share_info = 270,
	// 分享任务更新
	sc_share_mission_update = 271,
	// 分享抽奖
	cs_share_draw_request = 272,
	// 分享好友进度
	cs_share_friend_request = 273,
	// 分享好友榜单
	cs_share_rank_request = 274,
	// 分享抽奖返回
	sc_share_draw_response = 275,
	// 分享好友进度
	sc_share_history_response = 276,
	// 分享好友榜单
	sc_share_rank_response = 277,
	// 分享抽奖剩余次数
	sc_draw_count_response = 278,
	// 7日狂欢任务信息
	sc_task_seven_info_response = 310,
	// 7日狂欢领奖
	cs_task_seven_award_request = 311,
	// 7日狂欢领奖返回
	sc_task_seven_award_response = 312,
	// 分享朋友圈
	cs_share_with_friends_req = 313,
	// 商店所有物品信息
	sc_shop_all_item_base_config = 80,
	//商店购买
	cs_shop_buy_query = 81,
	//商店购买返回
	sc_shop_buy_reply = 82,
	//金牛领奖信息 登入和领完奖时同步
	sc_golden_bull_info_update = 83,
	//金牛领奖
	cs_golden_bull_draw_req = 84,
	//金牛领奖返回
	sc_golden_bull_draw_reply = 85,
	//月卡信息更新 登入 隔天 和 领奖返回时下发
	sc_month_card_info_update = 86,
	//月卡领奖
	cs_month_card_draw_req = 87,
	//月卡领奖返回
	sc_month_card_draw_reply = 88,
	// 奖券兑换
	cs_cash_transformation_req = 130,
	// 奖券兑换返回
	sc_cash_transformation_reply = 131,
	// 金牛任务进度
	cs_golden_bull_mission = 132,
	// 
	sc_golden_bull_mission = 133,
	
}
public class ProtoEncryptList
{
    public static HashSet<ProtoID> protoEncryptList = new HashSet<ProtoID>();
    static ProtoEncryptList()
    {
    }
}
public class ProtoIdNames
{
    public static Dictionary<ProtoID, string> protoIdNames = new Dictionary<ProtoID, string>();
    static ProtoIdNames()
    {
        protoIdNames.Add(ProtoID.sc_activity_config_info_update, "network.sc_activity_config_info_update");
		protoIdNames.Add(ProtoID.cs_activity_info_query_req, "network.cs_activity_info_query_req");
		protoIdNames.Add(ProtoID.sc_activity_info_query_reply, "network.sc_activity_info_query_reply");
		protoIdNames.Add(ProtoID.cs_activity_draw_req, "network.cs_activity_draw_req");
		protoIdNames.Add(ProtoID.sc_activity_draw_reply, "network.sc_activity_draw_reply");
		protoIdNames.Add(ProtoID.cs_car_enter_req, "network.cs_car_enter_req");
		protoIdNames.Add(ProtoID.sc_car_enter_reply, "network.sc_car_enter_reply");
		protoIdNames.Add(ProtoID.cs_car_exit_req, "network.cs_car_exit_req");
		protoIdNames.Add(ProtoID.sc_car_exit_reply, "network.sc_car_exit_reply");
		protoIdNames.Add(ProtoID.cs_car_master_req, "network.cs_car_master_req");
		protoIdNames.Add(ProtoID.sc_car_master_reply, "network.sc_car_master_reply");
		protoIdNames.Add(ProtoID.cs_car_bet_req, "network.cs_car_bet_req");
		protoIdNames.Add(ProtoID.sc_car_bet_reply, "network.sc_car_bet_reply");
		protoIdNames.Add(ProtoID.cs_car_rebet_req, "network.cs_car_rebet_req");
		protoIdNames.Add(ProtoID.sc_car_rebet_reply, "network.sc_car_rebet_reply");
		protoIdNames.Add(ProtoID.cs_car_master_list_req, "network.cs_car_master_list_req");
		protoIdNames.Add(ProtoID.cs_car_user_list_req, "network.cs_car_user_list_req");
		protoIdNames.Add(ProtoID.sc_car_user_list_reply, "network.sc_car_user_list_reply");
		protoIdNames.Add(ProtoID.sc_car_result_history_req, "network.sc_car_result_history_req");
		protoIdNames.Add(ProtoID.sc_car_master_wait_list_reply, "network.sc_car_master_wait_list_reply");
		protoIdNames.Add(ProtoID.sc_car_master_info_reply, "network.sc_car_master_info_reply");
		protoIdNames.Add(ProtoID.sc_car_status_reply, "network.sc_car_status_reply");
		protoIdNames.Add(ProtoID.sc_car_room_info_reply, "network.sc_car_room_info_reply");
		protoIdNames.Add(ProtoID.sc_car_hint_reply, "network.sc_car_hint_reply");
		protoIdNames.Add(ProtoID.sc_car_result_reply, "network.sc_car_result_reply");
		protoIdNames.Add(ProtoID.sc_car_pool_reply, "network.sc_car_pool_reply");
		protoIdNames.Add(ProtoID.cs_car_add_money_req, "network.cs_car_add_money_req");
		protoIdNames.Add(ProtoID.sc_car_add_money_reply, "network.sc_car_add_money_reply");
		protoIdNames.Add(ProtoID.cs_car_syn_in_game_state_req, "network.cs_car_syn_in_game_state_req");
		protoIdNames.Add(ProtoID.cs_player_niu_room_chest_draw, "network.cs_player_niu_room_chest_draw");
		protoIdNames.Add(ProtoID.sc_niu_room_chest_draw_reply, "network.sc_niu_room_chest_draw_reply");
		protoIdNames.Add(ProtoID.sc_niu_room_chest_info_update, "network.sc_niu_room_chest_info_update");
		protoIdNames.Add(ProtoID.sc_niu_room_chest_times_update, "network.sc_niu_room_chest_times_update");
		protoIdNames.Add(ProtoID.CS_COMMON_HEARTBEAT, "network.cs_common_heartbeat");
		protoIdNames.Add(ProtoID.SC_COMMON_HEARTBEAT_REPLY, "network.sc_common_heartbeat_reply");
		protoIdNames.Add(ProtoID.CS_COMMON_PROTO_COUNT, "network.cs_common_proto_count");
		protoIdNames.Add(ProtoID.SC_COMMON_PROTO_COUNT, "network.sc_common_proto_count");
		protoIdNames.Add(ProtoID.CS_COMMON_PROTO_CLEAN, "network.cs_common_proto_clean");
		protoIdNames.Add(ProtoID.SC_COMMON_PROTO_CLEAN, "network.sc_common_proto_clean");
		protoIdNames.Add(ProtoID.CS_COMMON_BUG_FEEDBACK, "network.cs_common_bug_feedback");
		protoIdNames.Add(ProtoID.SC_COMMON_BUG_FEEDBACK, "network.sc_common_bug_feedback");
		protoIdNames.Add(ProtoID.sc_hundred_niu_room_state_update, "network.sc_hundred_niu_room_state_update");
		protoIdNames.Add(ProtoID.cs_hundred_niu_enter_room_req, "network.cs_hundred_niu_enter_room_req");
		protoIdNames.Add(ProtoID.sc_hundred_niu_enter_room_reply, "network.sc_hundred_niu_enter_room_reply");
		protoIdNames.Add(ProtoID.cs_hundred_niu_player_list_query_req, "network.cs_hundred_niu_player_list_query_req");
		protoIdNames.Add(ProtoID.sc_hundred_niu_player_list_query_reply, "network.sc_hundred_niu_player_list_query_reply");
		protoIdNames.Add(ProtoID.cs_hundred_niu_free_set_chips_req, "network.cs_hundred_niu_free_set_chips_req");
		protoIdNames.Add(ProtoID.sc_hundred_niu_free_set_chips_reply, "network.sc_hundred_niu_free_set_chips_reply");
		protoIdNames.Add(ProtoID.sc_hundred_niu_free_set_chips_update, "network.sc_hundred_niu_free_set_chips_update");
		protoIdNames.Add(ProtoID.cs_hundred_niu_sit_down_req, "network.cs_hundred_niu_sit_down_req");
		protoIdNames.Add(ProtoID.sc_hundred_niu_sit_down_reply, "network.sc_hundred_niu_sit_down_reply");
		protoIdNames.Add(ProtoID.sc_hundred_niu_seat_player_info_update, "network.sc_hundred_niu_seat_player_info_update");
		protoIdNames.Add(ProtoID.cs_hundred_be_master_req, "network.cs_hundred_be_master_req");
		protoIdNames.Add(ProtoID.sc_hundred_be_master_reply, "network.sc_hundred_be_master_reply");
		protoIdNames.Add(ProtoID.cs_hundred_query_master_list_req, "network.cs_hundred_query_master_list_req");
		protoIdNames.Add(ProtoID.sc_hundred_query_master_list_reply, "network.sc_hundred_query_master_list_reply");
		protoIdNames.Add(ProtoID.cs_hundred_niu_in_game_syn_req, "network.cs_hundred_niu_in_game_syn_req");
		protoIdNames.Add(ProtoID.cs_hundred_leave_room_req, "network.cs_hundred_leave_room_req");
		protoIdNames.Add(ProtoID.sc_hundred_leave_room_reply, "network.sc_hundred_leave_room_reply");
		protoIdNames.Add(ProtoID.cs_hundred_query_winning_rec_req, "network.cs_hundred_query_winning_rec_req");
		protoIdNames.Add(ProtoID.sc_hundred_query_winning_rec_reply, "network.sc_hundred_query_winning_rec_reply");
		protoIdNames.Add(ProtoID.sc_hundred_player_gold_change_update, "network.sc_hundred_player_gold_change_update");
		protoIdNames.Add(ProtoID.cs_hundred_query_pool_win_player_req, "network.cs_hundred_query_pool_win_player_req");
		protoIdNames.Add(ProtoID.sc_hundred_query_pool_win_player_reply, "network.sc_hundred_query_pool_win_player_reply");
		protoIdNames.Add(ProtoID.sc_items_update, "network.sc_items_update");
		protoIdNames.Add(ProtoID.sc_items_add, "network.sc_items_add");
		protoIdNames.Add(ProtoID.sc_items_delete, "network.sc_items_delete");
		protoIdNames.Add(ProtoID.sc_items_init_update, "network.sc_items_init_update");
		protoIdNames.Add(ProtoID.cs_item_use_req, "network.cs_item_use_req");
		protoIdNames.Add(ProtoID.sc_item_use_reply, "network.sc_item_use_reply");
		protoIdNames.Add(ProtoID.cs_laba_enter_room_req, "network.cs_laba_enter_room_req");
		protoIdNames.Add(ProtoID.sc_laba_enter_room_reply, "network.sc_laba_enter_room_reply");
		protoIdNames.Add(ProtoID.cs_laba_leave_room_req, "network.cs_laba_leave_room_req");
		protoIdNames.Add(ProtoID.sc_laba_leave_room_reply, "network.sc_laba_leave_room_reply");
		protoIdNames.Add(ProtoID.sc_laba_pool_num_update, "network.sc_laba_pool_num_update");
		protoIdNames.Add(ProtoID.cs_laba_spin_req, "network.cs_laba_spin_req");
		protoIdNames.Add(ProtoID.sc_laba_spin_reply, "network.sc_laba_spin_reply");
		protoIdNames.Add(ProtoID.CS_LOGIN, "network.cs_login");
		protoIdNames.Add(ProtoID.SC_LOGIN_REPLY, "network.sc_login_reply");
		protoIdNames.Add(ProtoID.CS_LOGIN_OUT, "network.cs_login_out");
		protoIdNames.Add(ProtoID.CS_LOGIN_RECONNECTION, "network.cs_login_reconnection");
		protoIdNames.Add(ProtoID.SC_LOGIN_RECONNECTION_REPLY, "network.sc_login_reconnection_reply");
		protoIdNames.Add(ProtoID.SC_LOGIN_REPEAT, "network.sc_login_repeat");
		protoIdNames.Add(ProtoID.SC_LOGIN_PROTO_COMPLETE, "network.sc_login_proto_complete");
		protoIdNames.Add(ProtoID.sc_mails_init_update, "network.sc_mails_init_update");
		protoIdNames.Add(ProtoID.sc_mail_add, "network.sc_mail_add");
		protoIdNames.Add(ProtoID.cs_mail_delete_request, "network.cs_mail_delete_request");
		protoIdNames.Add(ProtoID.sc_mail_delete_reply, "network.sc_mail_delete_reply");
		protoIdNames.Add(ProtoID.cs_read_mail, "network.cs_read_mail");
		protoIdNames.Add(ProtoID.cs_mail_draw_request, "network.cs_mail_draw_request");
		protoIdNames.Add(ProtoID.sc_mail_draw_reply, "network.sc_mail_draw_reply");
		protoIdNames.Add(ProtoID.cs_draw_mission_request, "network.cs_draw_mission_request");
		protoIdNames.Add(ProtoID.sc_draw_mission_result_reply, "network.sc_draw_mission_result_reply");
		protoIdNames.Add(ProtoID.sc_mission, "network.sc_mission");
		protoIdNames.Add(ProtoID.sc_mission_update, "network.sc_mission_update");
		protoIdNames.Add(ProtoID.sc_mission_add, "network.sc_mission_add");
		protoIdNames.Add(ProtoID.sc_mission_del, "network.sc_mission_del");
		protoIdNames.Add(ProtoID.sc_game_task_info_update, "network.sc_game_task_info_update");
		protoIdNames.Add(ProtoID.sc_game_task_box_info_update, "network.sc_game_task_box_info_update");
		protoIdNames.Add(ProtoID.cs_game_task_draw_req, "network.cs_game_task_draw_req");
		protoIdNames.Add(ProtoID.sc_game_task_draw_reply, "network.sc_game_task_draw_reply");
		protoIdNames.Add(ProtoID.cs_game_task_box_draw_req, "network.cs_game_task_box_draw_req");
		protoIdNames.Add(ProtoID.sc_game_task_box_draw_reply, "network.sc_game_task_box_draw_reply");
		protoIdNames.Add(ProtoID.sc_redpack_task_draw_list_update, "network.sc_redpack_task_draw_list_update");
		protoIdNames.Add(ProtoID.cs_redpack_task_draw_req, "network.cs_redpack_task_draw_req");
		protoIdNames.Add(ProtoID.sc_redpack_task_draw_reply, "network.sc_redpack_task_draw_reply");
		protoIdNames.Add(ProtoID.sc_niu_room_state_update, "network.sc_niu_room_state_update");
		protoIdNames.Add(ProtoID.cs_niu_enter_room_req, "network.cs_niu_enter_room_req");
		protoIdNames.Add(ProtoID.sc_niu_enter_room_reply, "network.sc_niu_enter_room_reply");
		protoIdNames.Add(ProtoID.sc_niu_enter_room_player_info_update, "network.sc_niu_enter_room_player_info_update");
		protoIdNames.Add(ProtoID.cs_niu_choose_master_rate_req, "network.cs_niu_choose_master_rate_req");
		protoIdNames.Add(ProtoID.sc_niu_choose_master_rate_reply, "network.sc_niu_choose_master_rate_reply");
		protoIdNames.Add(ProtoID.sc_niu_player_choose_master_rate_update, "network.sc_niu_player_choose_master_rate_update");
		protoIdNames.Add(ProtoID.cs_niu_choose_free_rate_req, "network.cs_niu_choose_free_rate_req");
		protoIdNames.Add(ProtoID.sc_niu_choose_free_rate_reply, "network.sc_niu_choose_free_rate_reply");
		protoIdNames.Add(ProtoID.sc_niu_player_choose_free_rate_update, "network.sc_niu_player_choose_free_rate_update");
		protoIdNames.Add(ProtoID.cs_niu_leave_room_req, "network.cs_niu_leave_room_req");
		protoIdNames.Add(ProtoID.sc_niu_leave_room_reply, "network.sc_niu_leave_room_reply");
		protoIdNames.Add(ProtoID.sc_niu_leave_room_player_pos_update, "network.sc_niu_leave_room_player_pos_update");
		protoIdNames.Add(ProtoID.cs_niu_submit_card_req, "network.cs_niu_submit_card_req");
		protoIdNames.Add(ProtoID.sc_niu_submit_card_reply, "network.sc_niu_submit_card_reply");
		protoIdNames.Add(ProtoID.sc_niu_player_submit_card_update, "network.sc_niu_player_submit_card_update");
		protoIdNames.Add(ProtoID.cs_niu_syn_in_game_state_req, "network.cs_niu_syn_in_game_state_req");
		protoIdNames.Add(ProtoID.cs_niu_query_player_room_info_req, "network.cs_niu_query_player_room_info_req");
		protoIdNames.Add(ProtoID.sc_niu_player_room_info_update, "network.sc_niu_player_room_info_update");
		protoIdNames.Add(ProtoID.sc_niu_player_back_to_room_info_update, "network.sc_niu_player_back_to_room_info_update");
		protoIdNames.Add(ProtoID.sc_redpack_room_reset_times_update, "network.sc_redpack_room_reset_times_update");
		protoIdNames.Add(ProtoID.sc_redpack_room_player_times_update, "network.sc_redpack_room_player_times_update");
		protoIdNames.Add(ProtoID.sc_redpack_room_redpack_notice_update, "network.sc_redpack_room_redpack_notice_update");
		protoIdNames.Add(ProtoID.cs_redpack_room_draw_req, "network.cs_redpack_room_draw_req");
		protoIdNames.Add(ProtoID.sc_redpack_room_draw_reply, "network.sc_redpack_room_draw_reply");
		protoIdNames.Add(ProtoID.sc_redpack_redpack_timer_sec_update, "network.sc_redpack_redpack_timer_sec_update");
		protoIdNames.Add(ProtoID.cs_redpack_relive_req, "network.cs_redpack_relive_req");
		protoIdNames.Add(ProtoID.sc_redpack_relive_reply, "network.sc_redpack_relive_reply");
		protoIdNames.Add(ProtoID.sc_redpack_relive_times, "network.sc_redpack_relive_times");
		protoIdNames.Add(ProtoID.sc_fudai_pool_update, "network.sc_fudai_pool_update");
		protoIdNames.Add(ProtoID.sc_player_base_info, "network.sc_player_base_info");
		protoIdNames.Add(ProtoID.cs_player_change_name_req, "network.cs_player_change_name_req");
		protoIdNames.Add(ProtoID.sc_player_change_name_reply, "network.sc_player_change_name_reply");
		protoIdNames.Add(ProtoID.cs_player_change_headicon_req, "network.cs_player_change_headicon_req");
		protoIdNames.Add(ProtoID.sc_player_change_headicon_reply, "network.sc_player_change_headicon_reply");
		protoIdNames.Add(ProtoID.cs_player_chat, "network.cs_player_chat");
		protoIdNames.Add(ProtoID.sc_player_chat, "network.sc_player_chat");
		protoIdNames.Add(ProtoID.sc_player_sys_notice, "network.sc_player_sys_notice");
		protoIdNames.Add(ProtoID.sc_tips, "network.sc_tips");
		protoIdNames.Add(ProtoID.cs_query_player_winning_rec_req, "network.cs_query_player_winning_rec_req");
		protoIdNames.Add(ProtoID.sc_query_player_winning_rec_reply, "network.sc_query_player_winning_rec_reply");
		protoIdNames.Add(ProtoID.cs_niu_query_in_game_player_num_req, "network.cs_niu_query_in_game_player_num_req");
		protoIdNames.Add(ProtoID.sc_niu_query_in_game_player_num_reply, "network.sc_niu_query_in_game_player_num_reply");
		protoIdNames.Add(ProtoID.cs_niu_subsidy_req, "network.cs_niu_subsidy_req");
		protoIdNames.Add(ProtoID.sc_niu_subsidy_reply, "network.sc_niu_subsidy_reply");
		protoIdNames.Add(ProtoID.sc_niu_subsidy_info_update, "network.sc_niu_subsidy_info_update");
		protoIdNames.Add(ProtoID.cs_niu_special_subsidy_share, "network.cs_niu_special_subsidy_share");
		protoIdNames.Add(ProtoID.sc_niu_special_subsidy_share, "network.sc_niu_special_subsidy_share");
		protoIdNames.Add(ProtoID.cs_daily_checkin_req, "network.cs_daily_checkin_req");
		protoIdNames.Add(ProtoID.sc_daily_checkin_reply, "network.sc_daily_checkin_reply");
		protoIdNames.Add(ProtoID.sc_daily_checkin_info_update, "network.sc_daily_checkin_info_update");
		protoIdNames.Add(ProtoID.cs_make_up_for_checkin_req, "network.cs_make_up_for_checkin_req");
		protoIdNames.Add(ProtoID.sc_player_phone_num_info_update, "network.sc_player_phone_num_info_update");
		protoIdNames.Add(ProtoID.sc_player_bind_phone_num, "network.sc_player_bind_phone_num");
		protoIdNames.Add(ProtoID.cs_player_bind_phone_num_draw, "network.cs_player_bind_phone_num_draw");
		protoIdNames.Add(ProtoID.sc_player_bind_phone_num_draw_reply, "network.sc_player_bind_phone_num_draw_reply");
		protoIdNames.Add(ProtoID.sc_niu_special_subsidy_info_update, "network.sc_niu_special_subsidy_info_update");
		protoIdNames.Add(ProtoID.cs_rank_query_req, "network.cs_rank_query_req");
		protoIdNames.Add(ProtoID.sc_rank_qurey_reply, "network.sc_rank_qurey_reply");
		protoIdNames.Add(ProtoID.cs_vip_daily_reward, "network.cs_vip_daily_reward");
		protoIdNames.Add(ProtoID.sc_vip_daily_reward, "network.sc_vip_daily_reward");
		protoIdNames.Add(ProtoID.sc_guide_info_update, "network.sc_guide_info_update");
		protoIdNames.Add(ProtoID.cs_guide_next_step_req, "network.cs_guide_next_step_req");
		protoIdNames.Add(ProtoID.sc_guide_next_step_reply, "network.sc_guide_next_step_reply");
		protoIdNames.Add(ProtoID.cs_hundred_last_week_rank_query_req, "network.cs_hundred_last_week_rank_query_req");
		protoIdNames.Add(ProtoID.sc_hundred_last_week_rank_query_reply, "network.sc_hundred_last_week_rank_query_reply");
		protoIdNames.Add(ProtoID.cs_real_name_update, "network.cs_real_name_update");
		protoIdNames.Add(ProtoID.sc_real_name_update, "network.sc_real_name_update");
		protoIdNames.Add(ProtoID.cs_real_name_req, "network.cs_real_name_req");
		protoIdNames.Add(ProtoID.sc_real_name_req, "network.sc_real_name_req");
		protoIdNames.Add(ProtoID.sc_prize_config_update, "network.sc_prize_config_update");
		protoIdNames.Add(ProtoID.cs_prize_query_one_req, "network.cs_prize_query_one_req");
		protoIdNames.Add(ProtoID.sc_prize_query_one_reply, "network.sc_prize_query_one_reply");
		protoIdNames.Add(ProtoID.cs_prize_exchange_req, "network.cs_prize_exchange_req");
		protoIdNames.Add(ProtoID.sc_prize_exchange_reply, "network.sc_prize_exchange_reply");
		protoIdNames.Add(ProtoID.sc_prize_exchange_record_update, "network.sc_prize_exchange_record_update");
		protoIdNames.Add(ProtoID.sc_prize_address_info_update, "network.sc_prize_address_info_update");
		protoIdNames.Add(ProtoID.cs_prize_address_change_req, "network.cs_prize_address_change_req");
		protoIdNames.Add(ProtoID.sc_prize_address_change_reply, "network.sc_prize_address_change_reply");
		protoIdNames.Add(ProtoID.sc_prize_storage_red_point_update, "network.sc_prize_storage_red_point_update");
		protoIdNames.Add(ProtoID.cs_prize_query_phonecard_key_req, "network.cs_prize_query_phonecard_key_req");
		protoIdNames.Add(ProtoID.sc_prize_query_phonecard_key_reply, "network.sc_prize_query_phonecard_key_reply");
		protoIdNames.Add(ProtoID.cs_red_pack_query_list_req, "network.cs_red_pack_query_list_req");
		protoIdNames.Add(ProtoID.sc_red_pack_query_list_reply, "network.sc_red_pack_query_list_reply");
		protoIdNames.Add(ProtoID.cs_red_pack_open_req, "network.cs_red_pack_open_req");
		protoIdNames.Add(ProtoID.sc_red_pack_open_reply, "network.sc_red_pack_open_reply");
		protoIdNames.Add(ProtoID.cs_red_pack_create_req, "network.cs_red_pack_create_req");
		protoIdNames.Add(ProtoID.sc_red_pack_create_reply, "network.sc_red_pack_create_reply");
		protoIdNames.Add(ProtoID.sc_red_pack_notice_update, "network.sc_red_pack_notice_update");
		protoIdNames.Add(ProtoID.cs_red_pack_cancel_req, "network.cs_red_pack_cancel_req");
		protoIdNames.Add(ProtoID.sc_red_pack_cancel_reply, "network.sc_red_pack_cancel_reply");
		protoIdNames.Add(ProtoID.sc_self_red_pack_info, "network.sc_self_red_pack_info");
		protoIdNames.Add(ProtoID.cs_red_pack_do_select_req, "network.cs_red_pack_do_select_req");
		protoIdNames.Add(ProtoID.sc_red_pack_do_select_reply, "network.sc_red_pack_do_select_reply");
		protoIdNames.Add(ProtoID.cs_red_pack_search_req, "network.cs_red_pack_search_req");
		protoIdNames.Add(ProtoID.sc_red_pack_search_reply, "network.sc_red_pack_search_reply");
		protoIdNames.Add(ProtoID.cs_share_new_bee_reward_req, "network.cs_share_new_bee_reward_req");
		protoIdNames.Add(ProtoID.sc_share_new_bee_reward_reply, "network.sc_share_new_bee_reward_reply");
		protoIdNames.Add(ProtoID.cs_share_mission_reward_req, "network.cs_share_mission_reward_req");
		protoIdNames.Add(ProtoID.sc_share_mission_reward_reply, "network.sc_share_mission_reward_reply");
		protoIdNames.Add(ProtoID.sc_share_info, "network.sc_share_info");
		protoIdNames.Add(ProtoID.sc_share_mission_update, "network.sc_share_mission_update");
		protoIdNames.Add(ProtoID.cs_share_draw_request, "network.cs_share_draw_request");
		protoIdNames.Add(ProtoID.cs_share_friend_request, "network.cs_share_friend_request");
		protoIdNames.Add(ProtoID.cs_share_rank_request, "network.cs_share_rank_request");
		protoIdNames.Add(ProtoID.sc_share_draw_response, "network.sc_share_draw_response");
		protoIdNames.Add(ProtoID.sc_share_history_response, "network.sc_share_history_response");
		protoIdNames.Add(ProtoID.sc_share_rank_response, "network.sc_share_rank_response");
		protoIdNames.Add(ProtoID.sc_draw_count_response, "network.sc_draw_count_response");
		protoIdNames.Add(ProtoID.sc_task_seven_info_response, "network.sc_task_seven_info_response");
		protoIdNames.Add(ProtoID.cs_task_seven_award_request, "network.cs_task_seven_award_request");
		protoIdNames.Add(ProtoID.sc_task_seven_award_response, "network.sc_task_seven_award_response");
		protoIdNames.Add(ProtoID.cs_share_with_friends_req, "network.cs_share_with_friends_req");
		protoIdNames.Add(ProtoID.sc_shop_all_item_base_config, "network.sc_shop_all_item_base_config");
		protoIdNames.Add(ProtoID.cs_shop_buy_query, "network.cs_shop_buy_query");
		protoIdNames.Add(ProtoID.sc_shop_buy_reply, "network.sc_shop_buy_reply");
		protoIdNames.Add(ProtoID.sc_golden_bull_info_update, "network.sc_golden_bull_info_update");
		protoIdNames.Add(ProtoID.cs_golden_bull_draw_req, "network.cs_golden_bull_draw_req");
		protoIdNames.Add(ProtoID.sc_golden_bull_draw_reply, "network.sc_golden_bull_draw_reply");
		protoIdNames.Add(ProtoID.sc_month_card_info_update, "network.sc_month_card_info_update");
		protoIdNames.Add(ProtoID.cs_month_card_draw_req, "network.cs_month_card_draw_req");
		protoIdNames.Add(ProtoID.sc_month_card_draw_reply, "network.sc_month_card_draw_reply");
		protoIdNames.Add(ProtoID.cs_cash_transformation_req, "network.cs_cash_transformation_req");
		protoIdNames.Add(ProtoID.sc_cash_transformation_reply, "network.sc_cash_transformation_reply");
		protoIdNames.Add(ProtoID.cs_golden_bull_mission, "network.cs_golden_bull_mission");
		protoIdNames.Add(ProtoID.sc_golden_bull_mission, "network.sc_golden_bull_mission");
		

    }
}
public class ProtoSerializer
{
    public static object ParseFrom(ProtoID protoType, Stream stream)
    {
        switch (protoType) {

			case ProtoID.sc_activity_config_info_update: return Serializer.Deserialize<network.sc_activity_config_info_update>(stream);
			case ProtoID.cs_activity_info_query_req: return Serializer.Deserialize<network.cs_activity_info_query_req>(stream);
			case ProtoID.sc_activity_info_query_reply: return Serializer.Deserialize<network.sc_activity_info_query_reply>(stream);
			case ProtoID.cs_activity_draw_req: return Serializer.Deserialize<network.cs_activity_draw_req>(stream);
			case ProtoID.sc_activity_draw_reply: return Serializer.Deserialize<network.sc_activity_draw_reply>(stream);
			case ProtoID.cs_car_enter_req: return Serializer.Deserialize<network.cs_car_enter_req>(stream);
			case ProtoID.sc_car_enter_reply: return Serializer.Deserialize<network.sc_car_enter_reply>(stream);
			case ProtoID.cs_car_exit_req: return Serializer.Deserialize<network.cs_car_exit_req>(stream);
			case ProtoID.sc_car_exit_reply: return Serializer.Deserialize<network.sc_car_exit_reply>(stream);
			case ProtoID.cs_car_master_req: return Serializer.Deserialize<network.cs_car_master_req>(stream);
			case ProtoID.sc_car_master_reply: return Serializer.Deserialize<network.sc_car_master_reply>(stream);
			case ProtoID.cs_car_bet_req: return Serializer.Deserialize<network.cs_car_bet_req>(stream);
			case ProtoID.sc_car_bet_reply: return Serializer.Deserialize<network.sc_car_bet_reply>(stream);
			case ProtoID.cs_car_rebet_req: return Serializer.Deserialize<network.cs_car_rebet_req>(stream);
			case ProtoID.sc_car_rebet_reply: return Serializer.Deserialize<network.sc_car_rebet_reply>(stream);
			case ProtoID.cs_car_master_list_req: return Serializer.Deserialize<network.cs_car_master_list_req>(stream);
			case ProtoID.cs_car_user_list_req: return Serializer.Deserialize<network.cs_car_user_list_req>(stream);
			case ProtoID.sc_car_user_list_reply: return Serializer.Deserialize<network.sc_car_user_list_reply>(stream);
			case ProtoID.sc_car_result_history_req: return Serializer.Deserialize<network.sc_car_result_history_req>(stream);
			case ProtoID.sc_car_master_wait_list_reply: return Serializer.Deserialize<network.sc_car_master_wait_list_reply>(stream);
			case ProtoID.sc_car_master_info_reply: return Serializer.Deserialize<network.sc_car_master_info_reply>(stream);
			case ProtoID.sc_car_status_reply: return Serializer.Deserialize<network.sc_car_status_reply>(stream);
			case ProtoID.sc_car_room_info_reply: return Serializer.Deserialize<network.sc_car_room_info_reply>(stream);
			case ProtoID.sc_car_hint_reply: return Serializer.Deserialize<network.sc_car_hint_reply>(stream);
			case ProtoID.sc_car_result_reply: return Serializer.Deserialize<network.sc_car_result_reply>(stream);
			case ProtoID.sc_car_pool_reply: return Serializer.Deserialize<network.sc_car_pool_reply>(stream);
			case ProtoID.cs_car_add_money_req: return Serializer.Deserialize<network.cs_car_add_money_req>(stream);
			case ProtoID.sc_car_add_money_reply: return Serializer.Deserialize<network.sc_car_add_money_reply>(stream);
			case ProtoID.cs_car_syn_in_game_state_req: return Serializer.Deserialize<network.cs_car_syn_in_game_state_req>(stream);
			case ProtoID.cs_player_niu_room_chest_draw: return Serializer.Deserialize<network.cs_player_niu_room_chest_draw>(stream);
			case ProtoID.sc_niu_room_chest_draw_reply: return Serializer.Deserialize<network.sc_niu_room_chest_draw_reply>(stream);
			case ProtoID.sc_niu_room_chest_info_update: return Serializer.Deserialize<network.sc_niu_room_chest_info_update>(stream);
			case ProtoID.sc_niu_room_chest_times_update: return Serializer.Deserialize<network.sc_niu_room_chest_times_update>(stream);
			case ProtoID.CS_COMMON_HEARTBEAT: return Serializer.Deserialize<network.cs_common_heartbeat>(stream);
			case ProtoID.SC_COMMON_HEARTBEAT_REPLY: return Serializer.Deserialize<network.sc_common_heartbeat_reply>(stream);
			case ProtoID.CS_COMMON_PROTO_COUNT: return Serializer.Deserialize<network.cs_common_proto_count>(stream);
			case ProtoID.SC_COMMON_PROTO_COUNT: return Serializer.Deserialize<network.sc_common_proto_count>(stream);
			case ProtoID.CS_COMMON_PROTO_CLEAN: return Serializer.Deserialize<network.cs_common_proto_clean>(stream);
			case ProtoID.SC_COMMON_PROTO_CLEAN: return Serializer.Deserialize<network.sc_common_proto_clean>(stream);
			case ProtoID.CS_COMMON_BUG_FEEDBACK: return Serializer.Deserialize<network.cs_common_bug_feedback>(stream);
			case ProtoID.SC_COMMON_BUG_FEEDBACK: return Serializer.Deserialize<network.sc_common_bug_feedback>(stream);
			case ProtoID.sc_hundred_niu_room_state_update: return Serializer.Deserialize<network.sc_hundred_niu_room_state_update>(stream);
			case ProtoID.cs_hundred_niu_enter_room_req: return Serializer.Deserialize<network.cs_hundred_niu_enter_room_req>(stream);
			case ProtoID.sc_hundred_niu_enter_room_reply: return Serializer.Deserialize<network.sc_hundred_niu_enter_room_reply>(stream);
			case ProtoID.cs_hundred_niu_player_list_query_req: return Serializer.Deserialize<network.cs_hundred_niu_player_list_query_req>(stream);
			case ProtoID.sc_hundred_niu_player_list_query_reply: return Serializer.Deserialize<network.sc_hundred_niu_player_list_query_reply>(stream);
			case ProtoID.cs_hundred_niu_free_set_chips_req: return Serializer.Deserialize<network.cs_hundred_niu_free_set_chips_req>(stream);
			case ProtoID.sc_hundred_niu_free_set_chips_reply: return Serializer.Deserialize<network.sc_hundred_niu_free_set_chips_reply>(stream);
			case ProtoID.sc_hundred_niu_free_set_chips_update: return Serializer.Deserialize<network.sc_hundred_niu_free_set_chips_update>(stream);
			case ProtoID.cs_hundred_niu_sit_down_req: return Serializer.Deserialize<network.cs_hundred_niu_sit_down_req>(stream);
			case ProtoID.sc_hundred_niu_sit_down_reply: return Serializer.Deserialize<network.sc_hundred_niu_sit_down_reply>(stream);
			case ProtoID.sc_hundred_niu_seat_player_info_update: return Serializer.Deserialize<network.sc_hundred_niu_seat_player_info_update>(stream);
			case ProtoID.cs_hundred_be_master_req: return Serializer.Deserialize<network.cs_hundred_be_master_req>(stream);
			case ProtoID.sc_hundred_be_master_reply: return Serializer.Deserialize<network.sc_hundred_be_master_reply>(stream);
			case ProtoID.cs_hundred_query_master_list_req: return Serializer.Deserialize<network.cs_hundred_query_master_list_req>(stream);
			case ProtoID.sc_hundred_query_master_list_reply: return Serializer.Deserialize<network.sc_hundred_query_master_list_reply>(stream);
			case ProtoID.cs_hundred_niu_in_game_syn_req: return Serializer.Deserialize<network.cs_hundred_niu_in_game_syn_req>(stream);
			case ProtoID.cs_hundred_leave_room_req: return Serializer.Deserialize<network.cs_hundred_leave_room_req>(stream);
			case ProtoID.sc_hundred_leave_room_reply: return Serializer.Deserialize<network.sc_hundred_leave_room_reply>(stream);
			case ProtoID.cs_hundred_query_winning_rec_req: return Serializer.Deserialize<network.cs_hundred_query_winning_rec_req>(stream);
			case ProtoID.sc_hundred_query_winning_rec_reply: return Serializer.Deserialize<network.sc_hundred_query_winning_rec_reply>(stream);
			case ProtoID.sc_hundred_player_gold_change_update: return Serializer.Deserialize<network.sc_hundred_player_gold_change_update>(stream);
			case ProtoID.cs_hundred_query_pool_win_player_req: return Serializer.Deserialize<network.cs_hundred_query_pool_win_player_req>(stream);
			case ProtoID.sc_hundred_query_pool_win_player_reply: return Serializer.Deserialize<network.sc_hundred_query_pool_win_player_reply>(stream);
			case ProtoID.sc_items_update: return Serializer.Deserialize<network.sc_items_update>(stream);
			case ProtoID.sc_items_add: return Serializer.Deserialize<network.sc_items_add>(stream);
			case ProtoID.sc_items_delete: return Serializer.Deserialize<network.sc_items_delete>(stream);
			case ProtoID.sc_items_init_update: return Serializer.Deserialize<network.sc_items_init_update>(stream);
			case ProtoID.cs_item_use_req: return Serializer.Deserialize<network.cs_item_use_req>(stream);
			case ProtoID.sc_item_use_reply: return Serializer.Deserialize<network.sc_item_use_reply>(stream);
			case ProtoID.cs_laba_enter_room_req: return Serializer.Deserialize<network.cs_laba_enter_room_req>(stream);
			case ProtoID.sc_laba_enter_room_reply: return Serializer.Deserialize<network.sc_laba_enter_room_reply>(stream);
			case ProtoID.cs_laba_leave_room_req: return Serializer.Deserialize<network.cs_laba_leave_room_req>(stream);
			case ProtoID.sc_laba_leave_room_reply: return Serializer.Deserialize<network.sc_laba_leave_room_reply>(stream);
			case ProtoID.sc_laba_pool_num_update: return Serializer.Deserialize<network.sc_laba_pool_num_update>(stream);
			case ProtoID.cs_laba_spin_req: return Serializer.Deserialize<network.cs_laba_spin_req>(stream);
			case ProtoID.sc_laba_spin_reply: return Serializer.Deserialize<network.sc_laba_spin_reply>(stream);
			case ProtoID.CS_LOGIN: return Serializer.Deserialize<network.cs_login>(stream);
			case ProtoID.SC_LOGIN_REPLY: return Serializer.Deserialize<network.sc_login_reply>(stream);
			case ProtoID.CS_LOGIN_OUT: return Serializer.Deserialize<network.cs_login_out>(stream);
			case ProtoID.CS_LOGIN_RECONNECTION: return Serializer.Deserialize<network.cs_login_reconnection>(stream);
			case ProtoID.SC_LOGIN_RECONNECTION_REPLY: return Serializer.Deserialize<network.sc_login_reconnection_reply>(stream);
			case ProtoID.SC_LOGIN_REPEAT: return Serializer.Deserialize<network.sc_login_repeat>(stream);
			case ProtoID.SC_LOGIN_PROTO_COMPLETE: return Serializer.Deserialize<network.sc_login_proto_complete>(stream);
			case ProtoID.sc_mails_init_update: return Serializer.Deserialize<network.sc_mails_init_update>(stream);
			case ProtoID.sc_mail_add: return Serializer.Deserialize<network.sc_mail_add>(stream);
			case ProtoID.cs_mail_delete_request: return Serializer.Deserialize<network.cs_mail_delete_request>(stream);
			case ProtoID.sc_mail_delete_reply: return Serializer.Deserialize<network.sc_mail_delete_reply>(stream);
			case ProtoID.cs_read_mail: return Serializer.Deserialize<network.cs_read_mail>(stream);
			case ProtoID.cs_mail_draw_request: return Serializer.Deserialize<network.cs_mail_draw_request>(stream);
			case ProtoID.sc_mail_draw_reply: return Serializer.Deserialize<network.sc_mail_draw_reply>(stream);
			case ProtoID.cs_draw_mission_request: return Serializer.Deserialize<network.cs_draw_mission_request>(stream);
			case ProtoID.sc_draw_mission_result_reply: return Serializer.Deserialize<network.sc_draw_mission_result_reply>(stream);
			case ProtoID.sc_mission: return Serializer.Deserialize<network.sc_mission>(stream);
			case ProtoID.sc_mission_update: return Serializer.Deserialize<network.sc_mission_update>(stream);
			case ProtoID.sc_mission_add: return Serializer.Deserialize<network.sc_mission_add>(stream);
			case ProtoID.sc_mission_del: return Serializer.Deserialize<network.sc_mission_del>(stream);
			case ProtoID.sc_game_task_info_update: return Serializer.Deserialize<network.sc_game_task_info_update>(stream);
			case ProtoID.sc_game_task_box_info_update: return Serializer.Deserialize<network.sc_game_task_box_info_update>(stream);
			case ProtoID.cs_game_task_draw_req: return Serializer.Deserialize<network.cs_game_task_draw_req>(stream);
			case ProtoID.sc_game_task_draw_reply: return Serializer.Deserialize<network.sc_game_task_draw_reply>(stream);
			case ProtoID.cs_game_task_box_draw_req: return Serializer.Deserialize<network.cs_game_task_box_draw_req>(stream);
			case ProtoID.sc_game_task_box_draw_reply: return Serializer.Deserialize<network.sc_game_task_box_draw_reply>(stream);
			case ProtoID.sc_redpack_task_draw_list_update: return Serializer.Deserialize<network.sc_redpack_task_draw_list_update>(stream);
			case ProtoID.cs_redpack_task_draw_req: return Serializer.Deserialize<network.cs_redpack_task_draw_req>(stream);
			case ProtoID.sc_redpack_task_draw_reply: return Serializer.Deserialize<network.sc_redpack_task_draw_reply>(stream);
			case ProtoID.sc_niu_room_state_update: return Serializer.Deserialize<network.sc_niu_room_state_update>(stream);
			case ProtoID.cs_niu_enter_room_req: return Serializer.Deserialize<network.cs_niu_enter_room_req>(stream);
			case ProtoID.sc_niu_enter_room_reply: return Serializer.Deserialize<network.sc_niu_enter_room_reply>(stream);
			case ProtoID.sc_niu_enter_room_player_info_update: return Serializer.Deserialize<network.sc_niu_enter_room_player_info_update>(stream);
			case ProtoID.cs_niu_choose_master_rate_req: return Serializer.Deserialize<network.cs_niu_choose_master_rate_req>(stream);
			case ProtoID.sc_niu_choose_master_rate_reply: return Serializer.Deserialize<network.sc_niu_choose_master_rate_reply>(stream);
			case ProtoID.sc_niu_player_choose_master_rate_update: return Serializer.Deserialize<network.sc_niu_player_choose_master_rate_update>(stream);
			case ProtoID.cs_niu_choose_free_rate_req: return Serializer.Deserialize<network.cs_niu_choose_free_rate_req>(stream);
			case ProtoID.sc_niu_choose_free_rate_reply: return Serializer.Deserialize<network.sc_niu_choose_free_rate_reply>(stream);
			case ProtoID.sc_niu_player_choose_free_rate_update: return Serializer.Deserialize<network.sc_niu_player_choose_free_rate_update>(stream);
			case ProtoID.cs_niu_leave_room_req: return Serializer.Deserialize<network.cs_niu_leave_room_req>(stream);
			case ProtoID.sc_niu_leave_room_reply: return Serializer.Deserialize<network.sc_niu_leave_room_reply>(stream);
			case ProtoID.sc_niu_leave_room_player_pos_update: return Serializer.Deserialize<network.sc_niu_leave_room_player_pos_update>(stream);
			case ProtoID.cs_niu_submit_card_req: return Serializer.Deserialize<network.cs_niu_submit_card_req>(stream);
			case ProtoID.sc_niu_submit_card_reply: return Serializer.Deserialize<network.sc_niu_submit_card_reply>(stream);
			case ProtoID.sc_niu_player_submit_card_update: return Serializer.Deserialize<network.sc_niu_player_submit_card_update>(stream);
			case ProtoID.cs_niu_syn_in_game_state_req: return Serializer.Deserialize<network.cs_niu_syn_in_game_state_req>(stream);
			case ProtoID.cs_niu_query_player_room_info_req: return Serializer.Deserialize<network.cs_niu_query_player_room_info_req>(stream);
			case ProtoID.sc_niu_player_room_info_update: return Serializer.Deserialize<network.sc_niu_player_room_info_update>(stream);
			case ProtoID.sc_niu_player_back_to_room_info_update: return Serializer.Deserialize<network.sc_niu_player_back_to_room_info_update>(stream);
			case ProtoID.sc_redpack_room_reset_times_update: return Serializer.Deserialize<network.sc_redpack_room_reset_times_update>(stream);
			case ProtoID.sc_redpack_room_player_times_update: return Serializer.Deserialize<network.sc_redpack_room_player_times_update>(stream);
			case ProtoID.sc_redpack_room_redpack_notice_update: return Serializer.Deserialize<network.sc_redpack_room_redpack_notice_update>(stream);
			case ProtoID.cs_redpack_room_draw_req: return Serializer.Deserialize<network.cs_redpack_room_draw_req>(stream);
			case ProtoID.sc_redpack_room_draw_reply: return Serializer.Deserialize<network.sc_redpack_room_draw_reply>(stream);
			case ProtoID.sc_redpack_redpack_timer_sec_update: return Serializer.Deserialize<network.sc_redpack_redpack_timer_sec_update>(stream);
			case ProtoID.cs_redpack_relive_req: return Serializer.Deserialize<network.cs_redpack_relive_req>(stream);
			case ProtoID.sc_redpack_relive_reply: return Serializer.Deserialize<network.sc_redpack_relive_reply>(stream);
			case ProtoID.sc_redpack_relive_times: return Serializer.Deserialize<network.sc_redpack_relive_times>(stream);
			case ProtoID.sc_fudai_pool_update: return Serializer.Deserialize<network.sc_fudai_pool_update>(stream);
			case ProtoID.sc_player_base_info: return Serializer.Deserialize<network.sc_player_base_info>(stream);
			case ProtoID.cs_player_change_name_req: return Serializer.Deserialize<network.cs_player_change_name_req>(stream);
			case ProtoID.sc_player_change_name_reply: return Serializer.Deserialize<network.sc_player_change_name_reply>(stream);
			case ProtoID.cs_player_change_headicon_req: return Serializer.Deserialize<network.cs_player_change_headicon_req>(stream);
			case ProtoID.sc_player_change_headicon_reply: return Serializer.Deserialize<network.sc_player_change_headicon_reply>(stream);
			case ProtoID.cs_player_chat: return Serializer.Deserialize<network.cs_player_chat>(stream);
			case ProtoID.sc_player_chat: return Serializer.Deserialize<network.sc_player_chat>(stream);
			case ProtoID.sc_player_sys_notice: return Serializer.Deserialize<network.sc_player_sys_notice>(stream);
			case ProtoID.sc_tips: return Serializer.Deserialize<network.sc_tips>(stream);
			case ProtoID.cs_query_player_winning_rec_req: return Serializer.Deserialize<network.cs_query_player_winning_rec_req>(stream);
			case ProtoID.sc_query_player_winning_rec_reply: return Serializer.Deserialize<network.sc_query_player_winning_rec_reply>(stream);
			case ProtoID.cs_niu_query_in_game_player_num_req: return Serializer.Deserialize<network.cs_niu_query_in_game_player_num_req>(stream);
			case ProtoID.sc_niu_query_in_game_player_num_reply: return Serializer.Deserialize<network.sc_niu_query_in_game_player_num_reply>(stream);
			case ProtoID.cs_niu_subsidy_req: return Serializer.Deserialize<network.cs_niu_subsidy_req>(stream);
			case ProtoID.sc_niu_subsidy_reply: return Serializer.Deserialize<network.sc_niu_subsidy_reply>(stream);
			case ProtoID.sc_niu_subsidy_info_update: return Serializer.Deserialize<network.sc_niu_subsidy_info_update>(stream);
			case ProtoID.cs_niu_special_subsidy_share: return Serializer.Deserialize<network.cs_niu_special_subsidy_share>(stream);
			case ProtoID.sc_niu_special_subsidy_share: return Serializer.Deserialize<network.sc_niu_special_subsidy_share>(stream);
			case ProtoID.cs_daily_checkin_req: return Serializer.Deserialize<network.cs_daily_checkin_req>(stream);
			case ProtoID.sc_daily_checkin_reply: return Serializer.Deserialize<network.sc_daily_checkin_reply>(stream);
			case ProtoID.sc_daily_checkin_info_update: return Serializer.Deserialize<network.sc_daily_checkin_info_update>(stream);
			case ProtoID.cs_make_up_for_checkin_req: return Serializer.Deserialize<network.cs_make_up_for_checkin_req>(stream);
			case ProtoID.sc_player_phone_num_info_update: return Serializer.Deserialize<network.sc_player_phone_num_info_update>(stream);
			case ProtoID.sc_player_bind_phone_num: return Serializer.Deserialize<network.sc_player_bind_phone_num>(stream);
			case ProtoID.cs_player_bind_phone_num_draw: return Serializer.Deserialize<network.cs_player_bind_phone_num_draw>(stream);
			case ProtoID.sc_player_bind_phone_num_draw_reply: return Serializer.Deserialize<network.sc_player_bind_phone_num_draw_reply>(stream);
			case ProtoID.sc_niu_special_subsidy_info_update: return Serializer.Deserialize<network.sc_niu_special_subsidy_info_update>(stream);
			case ProtoID.cs_rank_query_req: return Serializer.Deserialize<network.cs_rank_query_req>(stream);
			case ProtoID.sc_rank_qurey_reply: return Serializer.Deserialize<network.sc_rank_qurey_reply>(stream);
			case ProtoID.cs_vip_daily_reward: return Serializer.Deserialize<network.cs_vip_daily_reward>(stream);
			case ProtoID.sc_vip_daily_reward: return Serializer.Deserialize<network.sc_vip_daily_reward>(stream);
			case ProtoID.sc_guide_info_update: return Serializer.Deserialize<network.sc_guide_info_update>(stream);
			case ProtoID.cs_guide_next_step_req: return Serializer.Deserialize<network.cs_guide_next_step_req>(stream);
			case ProtoID.sc_guide_next_step_reply: return Serializer.Deserialize<network.sc_guide_next_step_reply>(stream);
			case ProtoID.cs_hundred_last_week_rank_query_req: return Serializer.Deserialize<network.cs_hundred_last_week_rank_query_req>(stream);
			case ProtoID.sc_hundred_last_week_rank_query_reply: return Serializer.Deserialize<network.sc_hundred_last_week_rank_query_reply>(stream);
			case ProtoID.cs_real_name_update: return Serializer.Deserialize<network.cs_real_name_update>(stream);
			case ProtoID.sc_real_name_update: return Serializer.Deserialize<network.sc_real_name_update>(stream);
			case ProtoID.cs_real_name_req: return Serializer.Deserialize<network.cs_real_name_req>(stream);
			case ProtoID.sc_real_name_req: return Serializer.Deserialize<network.sc_real_name_req>(stream);
			case ProtoID.sc_prize_config_update: return Serializer.Deserialize<network.sc_prize_config_update>(stream);
			case ProtoID.cs_prize_query_one_req: return Serializer.Deserialize<network.cs_prize_query_one_req>(stream);
			case ProtoID.sc_prize_query_one_reply: return Serializer.Deserialize<network.sc_prize_query_one_reply>(stream);
			case ProtoID.cs_prize_exchange_req: return Serializer.Deserialize<network.cs_prize_exchange_req>(stream);
			case ProtoID.sc_prize_exchange_reply: return Serializer.Deserialize<network.sc_prize_exchange_reply>(stream);
			case ProtoID.sc_prize_exchange_record_update: return Serializer.Deserialize<network.sc_prize_exchange_record_update>(stream);
			case ProtoID.sc_prize_address_info_update: return Serializer.Deserialize<network.sc_prize_address_info_update>(stream);
			case ProtoID.cs_prize_address_change_req: return Serializer.Deserialize<network.cs_prize_address_change_req>(stream);
			case ProtoID.sc_prize_address_change_reply: return Serializer.Deserialize<network.sc_prize_address_change_reply>(stream);
			case ProtoID.sc_prize_storage_red_point_update: return Serializer.Deserialize<network.sc_prize_storage_red_point_update>(stream);
			case ProtoID.cs_prize_query_phonecard_key_req: return Serializer.Deserialize<network.cs_prize_query_phonecard_key_req>(stream);
			case ProtoID.sc_prize_query_phonecard_key_reply: return Serializer.Deserialize<network.sc_prize_query_phonecard_key_reply>(stream);
			case ProtoID.cs_red_pack_query_list_req: return Serializer.Deserialize<network.cs_red_pack_query_list_req>(stream);
			case ProtoID.sc_red_pack_query_list_reply: return Serializer.Deserialize<network.sc_red_pack_query_list_reply>(stream);
			case ProtoID.cs_red_pack_open_req: return Serializer.Deserialize<network.cs_red_pack_open_req>(stream);
			case ProtoID.sc_red_pack_open_reply: return Serializer.Deserialize<network.sc_red_pack_open_reply>(stream);
			case ProtoID.cs_red_pack_create_req: return Serializer.Deserialize<network.cs_red_pack_create_req>(stream);
			case ProtoID.sc_red_pack_create_reply: return Serializer.Deserialize<network.sc_red_pack_create_reply>(stream);
			case ProtoID.sc_red_pack_notice_update: return Serializer.Deserialize<network.sc_red_pack_notice_update>(stream);
			case ProtoID.cs_red_pack_cancel_req: return Serializer.Deserialize<network.cs_red_pack_cancel_req>(stream);
			case ProtoID.sc_red_pack_cancel_reply: return Serializer.Deserialize<network.sc_red_pack_cancel_reply>(stream);
			case ProtoID.sc_self_red_pack_info: return Serializer.Deserialize<network.sc_self_red_pack_info>(stream);
			case ProtoID.cs_red_pack_do_select_req: return Serializer.Deserialize<network.cs_red_pack_do_select_req>(stream);
			case ProtoID.sc_red_pack_do_select_reply: return Serializer.Deserialize<network.sc_red_pack_do_select_reply>(stream);
			case ProtoID.cs_red_pack_search_req: return Serializer.Deserialize<network.cs_red_pack_search_req>(stream);
			case ProtoID.sc_red_pack_search_reply: return Serializer.Deserialize<network.sc_red_pack_search_reply>(stream);
			case ProtoID.cs_share_new_bee_reward_req: return Serializer.Deserialize<network.cs_share_new_bee_reward_req>(stream);
			case ProtoID.sc_share_new_bee_reward_reply: return Serializer.Deserialize<network.sc_share_new_bee_reward_reply>(stream);
			case ProtoID.cs_share_mission_reward_req: return Serializer.Deserialize<network.cs_share_mission_reward_req>(stream);
			case ProtoID.sc_share_mission_reward_reply: return Serializer.Deserialize<network.sc_share_mission_reward_reply>(stream);
			case ProtoID.sc_share_info: return Serializer.Deserialize<network.sc_share_info>(stream);
			case ProtoID.sc_share_mission_update: return Serializer.Deserialize<network.sc_share_mission_update>(stream);
			case ProtoID.cs_share_draw_request: return Serializer.Deserialize<network.cs_share_draw_request>(stream);
			case ProtoID.cs_share_friend_request: return Serializer.Deserialize<network.cs_share_friend_request>(stream);
			case ProtoID.cs_share_rank_request: return Serializer.Deserialize<network.cs_share_rank_request>(stream);
			case ProtoID.sc_share_draw_response: return Serializer.Deserialize<network.sc_share_draw_response>(stream);
			case ProtoID.sc_share_history_response: return Serializer.Deserialize<network.sc_share_history_response>(stream);
			case ProtoID.sc_share_rank_response: return Serializer.Deserialize<network.sc_share_rank_response>(stream);
			case ProtoID.sc_draw_count_response: return Serializer.Deserialize<network.sc_draw_count_response>(stream);
			case ProtoID.sc_task_seven_info_response: return Serializer.Deserialize<network.sc_task_seven_info_response>(stream);
			case ProtoID.cs_task_seven_award_request: return Serializer.Deserialize<network.cs_task_seven_award_request>(stream);
			case ProtoID.sc_task_seven_award_response: return Serializer.Deserialize<network.sc_task_seven_award_response>(stream);
			case ProtoID.cs_share_with_friends_req: return Serializer.Deserialize<network.cs_share_with_friends_req>(stream);
			case ProtoID.sc_shop_all_item_base_config: return Serializer.Deserialize<network.sc_shop_all_item_base_config>(stream);
			case ProtoID.cs_shop_buy_query: return Serializer.Deserialize<network.cs_shop_buy_query>(stream);
			case ProtoID.sc_shop_buy_reply: return Serializer.Deserialize<network.sc_shop_buy_reply>(stream);
			case ProtoID.sc_golden_bull_info_update: return Serializer.Deserialize<network.sc_golden_bull_info_update>(stream);
			case ProtoID.cs_golden_bull_draw_req: return Serializer.Deserialize<network.cs_golden_bull_draw_req>(stream);
			case ProtoID.sc_golden_bull_draw_reply: return Serializer.Deserialize<network.sc_golden_bull_draw_reply>(stream);
			case ProtoID.sc_month_card_info_update: return Serializer.Deserialize<network.sc_month_card_info_update>(stream);
			case ProtoID.cs_month_card_draw_req: return Serializer.Deserialize<network.cs_month_card_draw_req>(stream);
			case ProtoID.sc_month_card_draw_reply: return Serializer.Deserialize<network.sc_month_card_draw_reply>(stream);
			case ProtoID.cs_cash_transformation_req: return Serializer.Deserialize<network.cs_cash_transformation_req>(stream);
			case ProtoID.sc_cash_transformation_reply: return Serializer.Deserialize<network.sc_cash_transformation_reply>(stream);
			case ProtoID.cs_golden_bull_mission: return Serializer.Deserialize<network.cs_golden_bull_mission>(stream);
			case ProtoID.sc_golden_bull_mission: return Serializer.Deserialize<network.sc_golden_bull_mission>(stream);
			

			default: break;
        }
        return null;
    }

    public static void Serialize(ProtoID protoType, Stream stream, object proto)
    {
        switch (protoType) {

			case ProtoID.sc_activity_config_info_update: Serializer.Serialize(stream, (network.sc_activity_config_info_update)proto); break;
			case ProtoID.cs_activity_info_query_req: Serializer.Serialize(stream, (network.cs_activity_info_query_req)proto); break;
			case ProtoID.sc_activity_info_query_reply: Serializer.Serialize(stream, (network.sc_activity_info_query_reply)proto); break;
			case ProtoID.cs_activity_draw_req: Serializer.Serialize(stream, (network.cs_activity_draw_req)proto); break;
			case ProtoID.sc_activity_draw_reply: Serializer.Serialize(stream, (network.sc_activity_draw_reply)proto); break;
			case ProtoID.cs_car_enter_req: Serializer.Serialize(stream, (network.cs_car_enter_req)proto); break;
			case ProtoID.sc_car_enter_reply: Serializer.Serialize(stream, (network.sc_car_enter_reply)proto); break;
			case ProtoID.cs_car_exit_req: Serializer.Serialize(stream, (network.cs_car_exit_req)proto); break;
			case ProtoID.sc_car_exit_reply: Serializer.Serialize(stream, (network.sc_car_exit_reply)proto); break;
			case ProtoID.cs_car_master_req: Serializer.Serialize(stream, (network.cs_car_master_req)proto); break;
			case ProtoID.sc_car_master_reply: Serializer.Serialize(stream, (network.sc_car_master_reply)proto); break;
			case ProtoID.cs_car_bet_req: Serializer.Serialize(stream, (network.cs_car_bet_req)proto); break;
			case ProtoID.sc_car_bet_reply: Serializer.Serialize(stream, (network.sc_car_bet_reply)proto); break;
			case ProtoID.cs_car_rebet_req: Serializer.Serialize(stream, (network.cs_car_rebet_req)proto); break;
			case ProtoID.sc_car_rebet_reply: Serializer.Serialize(stream, (network.sc_car_rebet_reply)proto); break;
			case ProtoID.cs_car_master_list_req: Serializer.Serialize(stream, (network.cs_car_master_list_req)proto); break;
			case ProtoID.cs_car_user_list_req: Serializer.Serialize(stream, (network.cs_car_user_list_req)proto); break;
			case ProtoID.sc_car_user_list_reply: Serializer.Serialize(stream, (network.sc_car_user_list_reply)proto); break;
			case ProtoID.sc_car_result_history_req: Serializer.Serialize(stream, (network.sc_car_result_history_req)proto); break;
			case ProtoID.sc_car_master_wait_list_reply: Serializer.Serialize(stream, (network.sc_car_master_wait_list_reply)proto); break;
			case ProtoID.sc_car_master_info_reply: Serializer.Serialize(stream, (network.sc_car_master_info_reply)proto); break;
			case ProtoID.sc_car_status_reply: Serializer.Serialize(stream, (network.sc_car_status_reply)proto); break;
			case ProtoID.sc_car_room_info_reply: Serializer.Serialize(stream, (network.sc_car_room_info_reply)proto); break;
			case ProtoID.sc_car_hint_reply: Serializer.Serialize(stream, (network.sc_car_hint_reply)proto); break;
			case ProtoID.sc_car_result_reply: Serializer.Serialize(stream, (network.sc_car_result_reply)proto); break;
			case ProtoID.sc_car_pool_reply: Serializer.Serialize(stream, (network.sc_car_pool_reply)proto); break;
			case ProtoID.cs_car_add_money_req: Serializer.Serialize(stream, (network.cs_car_add_money_req)proto); break;
			case ProtoID.sc_car_add_money_reply: Serializer.Serialize(stream, (network.sc_car_add_money_reply)proto); break;
			case ProtoID.cs_car_syn_in_game_state_req: Serializer.Serialize(stream, (network.cs_car_syn_in_game_state_req)proto); break;
			case ProtoID.cs_player_niu_room_chest_draw: Serializer.Serialize(stream, (network.cs_player_niu_room_chest_draw)proto); break;
			case ProtoID.sc_niu_room_chest_draw_reply: Serializer.Serialize(stream, (network.sc_niu_room_chest_draw_reply)proto); break;
			case ProtoID.sc_niu_room_chest_info_update: Serializer.Serialize(stream, (network.sc_niu_room_chest_info_update)proto); break;
			case ProtoID.sc_niu_room_chest_times_update: Serializer.Serialize(stream, (network.sc_niu_room_chest_times_update)proto); break;
			case ProtoID.CS_COMMON_HEARTBEAT: Serializer.Serialize(stream, (network.cs_common_heartbeat)proto); break;
			case ProtoID.SC_COMMON_HEARTBEAT_REPLY: Serializer.Serialize(stream, (network.sc_common_heartbeat_reply)proto); break;
			case ProtoID.CS_COMMON_PROTO_COUNT: Serializer.Serialize(stream, (network.cs_common_proto_count)proto); break;
			case ProtoID.SC_COMMON_PROTO_COUNT: Serializer.Serialize(stream, (network.sc_common_proto_count)proto); break;
			case ProtoID.CS_COMMON_PROTO_CLEAN: Serializer.Serialize(stream, (network.cs_common_proto_clean)proto); break;
			case ProtoID.SC_COMMON_PROTO_CLEAN: Serializer.Serialize(stream, (network.sc_common_proto_clean)proto); break;
			case ProtoID.CS_COMMON_BUG_FEEDBACK: Serializer.Serialize(stream, (network.cs_common_bug_feedback)proto); break;
			case ProtoID.SC_COMMON_BUG_FEEDBACK: Serializer.Serialize(stream, (network.sc_common_bug_feedback)proto); break;
			case ProtoID.sc_hundred_niu_room_state_update: Serializer.Serialize(stream, (network.sc_hundred_niu_room_state_update)proto); break;
			case ProtoID.cs_hundred_niu_enter_room_req: Serializer.Serialize(stream, (network.cs_hundred_niu_enter_room_req)proto); break;
			case ProtoID.sc_hundred_niu_enter_room_reply: Serializer.Serialize(stream, (network.sc_hundred_niu_enter_room_reply)proto); break;
			case ProtoID.cs_hundred_niu_player_list_query_req: Serializer.Serialize(stream, (network.cs_hundred_niu_player_list_query_req)proto); break;
			case ProtoID.sc_hundred_niu_player_list_query_reply: Serializer.Serialize(stream, (network.sc_hundred_niu_player_list_query_reply)proto); break;
			case ProtoID.cs_hundred_niu_free_set_chips_req: Serializer.Serialize(stream, (network.cs_hundred_niu_free_set_chips_req)proto); break;
			case ProtoID.sc_hundred_niu_free_set_chips_reply: Serializer.Serialize(stream, (network.sc_hundred_niu_free_set_chips_reply)proto); break;
			case ProtoID.sc_hundred_niu_free_set_chips_update: Serializer.Serialize(stream, (network.sc_hundred_niu_free_set_chips_update)proto); break;
			case ProtoID.cs_hundred_niu_sit_down_req: Serializer.Serialize(stream, (network.cs_hundred_niu_sit_down_req)proto); break;
			case ProtoID.sc_hundred_niu_sit_down_reply: Serializer.Serialize(stream, (network.sc_hundred_niu_sit_down_reply)proto); break;
			case ProtoID.sc_hundred_niu_seat_player_info_update: Serializer.Serialize(stream, (network.sc_hundred_niu_seat_player_info_update)proto); break;
			case ProtoID.cs_hundred_be_master_req: Serializer.Serialize(stream, (network.cs_hundred_be_master_req)proto); break;
			case ProtoID.sc_hundred_be_master_reply: Serializer.Serialize(stream, (network.sc_hundred_be_master_reply)proto); break;
			case ProtoID.cs_hundred_query_master_list_req: Serializer.Serialize(stream, (network.cs_hundred_query_master_list_req)proto); break;
			case ProtoID.sc_hundred_query_master_list_reply: Serializer.Serialize(stream, (network.sc_hundred_query_master_list_reply)proto); break;
			case ProtoID.cs_hundred_niu_in_game_syn_req: Serializer.Serialize(stream, (network.cs_hundred_niu_in_game_syn_req)proto); break;
			case ProtoID.cs_hundred_leave_room_req: Serializer.Serialize(stream, (network.cs_hundred_leave_room_req)proto); break;
			case ProtoID.sc_hundred_leave_room_reply: Serializer.Serialize(stream, (network.sc_hundred_leave_room_reply)proto); break;
			case ProtoID.cs_hundred_query_winning_rec_req: Serializer.Serialize(stream, (network.cs_hundred_query_winning_rec_req)proto); break;
			case ProtoID.sc_hundred_query_winning_rec_reply: Serializer.Serialize(stream, (network.sc_hundred_query_winning_rec_reply)proto); break;
			case ProtoID.sc_hundred_player_gold_change_update: Serializer.Serialize(stream, (network.sc_hundred_player_gold_change_update)proto); break;
			case ProtoID.cs_hundred_query_pool_win_player_req: Serializer.Serialize(stream, (network.cs_hundred_query_pool_win_player_req)proto); break;
			case ProtoID.sc_hundred_query_pool_win_player_reply: Serializer.Serialize(stream, (network.sc_hundred_query_pool_win_player_reply)proto); break;
			case ProtoID.sc_items_update: Serializer.Serialize(stream, (network.sc_items_update)proto); break;
			case ProtoID.sc_items_add: Serializer.Serialize(stream, (network.sc_items_add)proto); break;
			case ProtoID.sc_items_delete: Serializer.Serialize(stream, (network.sc_items_delete)proto); break;
			case ProtoID.sc_items_init_update: Serializer.Serialize(stream, (network.sc_items_init_update)proto); break;
			case ProtoID.cs_item_use_req: Serializer.Serialize(stream, (network.cs_item_use_req)proto); break;
			case ProtoID.sc_item_use_reply: Serializer.Serialize(stream, (network.sc_item_use_reply)proto); break;
			case ProtoID.cs_laba_enter_room_req: Serializer.Serialize(stream, (network.cs_laba_enter_room_req)proto); break;
			case ProtoID.sc_laba_enter_room_reply: Serializer.Serialize(stream, (network.sc_laba_enter_room_reply)proto); break;
			case ProtoID.cs_laba_leave_room_req: Serializer.Serialize(stream, (network.cs_laba_leave_room_req)proto); break;
			case ProtoID.sc_laba_leave_room_reply: Serializer.Serialize(stream, (network.sc_laba_leave_room_reply)proto); break;
			case ProtoID.sc_laba_pool_num_update: Serializer.Serialize(stream, (network.sc_laba_pool_num_update)proto); break;
			case ProtoID.cs_laba_spin_req: Serializer.Serialize(stream, (network.cs_laba_spin_req)proto); break;
			case ProtoID.sc_laba_spin_reply: Serializer.Serialize(stream, (network.sc_laba_spin_reply)proto); break;
			case ProtoID.CS_LOGIN: Serializer.Serialize(stream, (network.cs_login)proto); break;
			case ProtoID.SC_LOGIN_REPLY: Serializer.Serialize(stream, (network.sc_login_reply)proto); break;
			case ProtoID.CS_LOGIN_OUT: Serializer.Serialize(stream, (network.cs_login_out)proto); break;
			case ProtoID.CS_LOGIN_RECONNECTION: Serializer.Serialize(stream, (network.cs_login_reconnection)proto); break;
			case ProtoID.SC_LOGIN_RECONNECTION_REPLY: Serializer.Serialize(stream, (network.sc_login_reconnection_reply)proto); break;
			case ProtoID.SC_LOGIN_REPEAT: Serializer.Serialize(stream, (network.sc_login_repeat)proto); break;
			case ProtoID.SC_LOGIN_PROTO_COMPLETE: Serializer.Serialize(stream, (network.sc_login_proto_complete)proto); break;
			case ProtoID.sc_mails_init_update: Serializer.Serialize(stream, (network.sc_mails_init_update)proto); break;
			case ProtoID.sc_mail_add: Serializer.Serialize(stream, (network.sc_mail_add)proto); break;
			case ProtoID.cs_mail_delete_request: Serializer.Serialize(stream, (network.cs_mail_delete_request)proto); break;
			case ProtoID.sc_mail_delete_reply: Serializer.Serialize(stream, (network.sc_mail_delete_reply)proto); break;
			case ProtoID.cs_read_mail: Serializer.Serialize(stream, (network.cs_read_mail)proto); break;
			case ProtoID.cs_mail_draw_request: Serializer.Serialize(stream, (network.cs_mail_draw_request)proto); break;
			case ProtoID.sc_mail_draw_reply: Serializer.Serialize(stream, (network.sc_mail_draw_reply)proto); break;
			case ProtoID.cs_draw_mission_request: Serializer.Serialize(stream, (network.cs_draw_mission_request)proto); break;
			case ProtoID.sc_draw_mission_result_reply: Serializer.Serialize(stream, (network.sc_draw_mission_result_reply)proto); break;
			case ProtoID.sc_mission: Serializer.Serialize(stream, (network.sc_mission)proto); break;
			case ProtoID.sc_mission_update: Serializer.Serialize(stream, (network.sc_mission_update)proto); break;
			case ProtoID.sc_mission_add: Serializer.Serialize(stream, (network.sc_mission_add)proto); break;
			case ProtoID.sc_mission_del: Serializer.Serialize(stream, (network.sc_mission_del)proto); break;
			case ProtoID.sc_game_task_info_update: Serializer.Serialize(stream, (network.sc_game_task_info_update)proto); break;
			case ProtoID.sc_game_task_box_info_update: Serializer.Serialize(stream, (network.sc_game_task_box_info_update)proto); break;
			case ProtoID.cs_game_task_draw_req: Serializer.Serialize(stream, (network.cs_game_task_draw_req)proto); break;
			case ProtoID.sc_game_task_draw_reply: Serializer.Serialize(stream, (network.sc_game_task_draw_reply)proto); break;
			case ProtoID.cs_game_task_box_draw_req: Serializer.Serialize(stream, (network.cs_game_task_box_draw_req)proto); break;
			case ProtoID.sc_game_task_box_draw_reply: Serializer.Serialize(stream, (network.sc_game_task_box_draw_reply)proto); break;
			case ProtoID.sc_redpack_task_draw_list_update: Serializer.Serialize(stream, (network.sc_redpack_task_draw_list_update)proto); break;
			case ProtoID.cs_redpack_task_draw_req: Serializer.Serialize(stream, (network.cs_redpack_task_draw_req)proto); break;
			case ProtoID.sc_redpack_task_draw_reply: Serializer.Serialize(stream, (network.sc_redpack_task_draw_reply)proto); break;
			case ProtoID.sc_niu_room_state_update: Serializer.Serialize(stream, (network.sc_niu_room_state_update)proto); break;
			case ProtoID.cs_niu_enter_room_req: Serializer.Serialize(stream, (network.cs_niu_enter_room_req)proto); break;
			case ProtoID.sc_niu_enter_room_reply: Serializer.Serialize(stream, (network.sc_niu_enter_room_reply)proto); break;
			case ProtoID.sc_niu_enter_room_player_info_update: Serializer.Serialize(stream, (network.sc_niu_enter_room_player_info_update)proto); break;
			case ProtoID.cs_niu_choose_master_rate_req: Serializer.Serialize(stream, (network.cs_niu_choose_master_rate_req)proto); break;
			case ProtoID.sc_niu_choose_master_rate_reply: Serializer.Serialize(stream, (network.sc_niu_choose_master_rate_reply)proto); break;
			case ProtoID.sc_niu_player_choose_master_rate_update: Serializer.Serialize(stream, (network.sc_niu_player_choose_master_rate_update)proto); break;
			case ProtoID.cs_niu_choose_free_rate_req: Serializer.Serialize(stream, (network.cs_niu_choose_free_rate_req)proto); break;
			case ProtoID.sc_niu_choose_free_rate_reply: Serializer.Serialize(stream, (network.sc_niu_choose_free_rate_reply)proto); break;
			case ProtoID.sc_niu_player_choose_free_rate_update: Serializer.Serialize(stream, (network.sc_niu_player_choose_free_rate_update)proto); break;
			case ProtoID.cs_niu_leave_room_req: Serializer.Serialize(stream, (network.cs_niu_leave_room_req)proto); break;
			case ProtoID.sc_niu_leave_room_reply: Serializer.Serialize(stream, (network.sc_niu_leave_room_reply)proto); break;
			case ProtoID.sc_niu_leave_room_player_pos_update: Serializer.Serialize(stream, (network.sc_niu_leave_room_player_pos_update)proto); break;
			case ProtoID.cs_niu_submit_card_req: Serializer.Serialize(stream, (network.cs_niu_submit_card_req)proto); break;
			case ProtoID.sc_niu_submit_card_reply: Serializer.Serialize(stream, (network.sc_niu_submit_card_reply)proto); break;
			case ProtoID.sc_niu_player_submit_card_update: Serializer.Serialize(stream, (network.sc_niu_player_submit_card_update)proto); break;
			case ProtoID.cs_niu_syn_in_game_state_req: Serializer.Serialize(stream, (network.cs_niu_syn_in_game_state_req)proto); break;
			case ProtoID.cs_niu_query_player_room_info_req: Serializer.Serialize(stream, (network.cs_niu_query_player_room_info_req)proto); break;
			case ProtoID.sc_niu_player_room_info_update: Serializer.Serialize(stream, (network.sc_niu_player_room_info_update)proto); break;
			case ProtoID.sc_niu_player_back_to_room_info_update: Serializer.Serialize(stream, (network.sc_niu_player_back_to_room_info_update)proto); break;
			case ProtoID.sc_redpack_room_reset_times_update: Serializer.Serialize(stream, (network.sc_redpack_room_reset_times_update)proto); break;
			case ProtoID.sc_redpack_room_player_times_update: Serializer.Serialize(stream, (network.sc_redpack_room_player_times_update)proto); break;
			case ProtoID.sc_redpack_room_redpack_notice_update: Serializer.Serialize(stream, (network.sc_redpack_room_redpack_notice_update)proto); break;
			case ProtoID.cs_redpack_room_draw_req: Serializer.Serialize(stream, (network.cs_redpack_room_draw_req)proto); break;
			case ProtoID.sc_redpack_room_draw_reply: Serializer.Serialize(stream, (network.sc_redpack_room_draw_reply)proto); break;
			case ProtoID.sc_redpack_redpack_timer_sec_update: Serializer.Serialize(stream, (network.sc_redpack_redpack_timer_sec_update)proto); break;
			case ProtoID.cs_redpack_relive_req: Serializer.Serialize(stream, (network.cs_redpack_relive_req)proto); break;
			case ProtoID.sc_redpack_relive_reply: Serializer.Serialize(stream, (network.sc_redpack_relive_reply)proto); break;
			case ProtoID.sc_redpack_relive_times: Serializer.Serialize(stream, (network.sc_redpack_relive_times)proto); break;
			case ProtoID.sc_fudai_pool_update: Serializer.Serialize(stream, (network.sc_fudai_pool_update)proto); break;
			case ProtoID.sc_player_base_info: Serializer.Serialize(stream, (network.sc_player_base_info)proto); break;
			case ProtoID.cs_player_change_name_req: Serializer.Serialize(stream, (network.cs_player_change_name_req)proto); break;
			case ProtoID.sc_player_change_name_reply: Serializer.Serialize(stream, (network.sc_player_change_name_reply)proto); break;
			case ProtoID.cs_player_change_headicon_req: Serializer.Serialize(stream, (network.cs_player_change_headicon_req)proto); break;
			case ProtoID.sc_player_change_headicon_reply: Serializer.Serialize(stream, (network.sc_player_change_headicon_reply)proto); break;
			case ProtoID.cs_player_chat: Serializer.Serialize(stream, (network.cs_player_chat)proto); break;
			case ProtoID.sc_player_chat: Serializer.Serialize(stream, (network.sc_player_chat)proto); break;
			case ProtoID.sc_player_sys_notice: Serializer.Serialize(stream, (network.sc_player_sys_notice)proto); break;
			case ProtoID.sc_tips: Serializer.Serialize(stream, (network.sc_tips)proto); break;
			case ProtoID.cs_query_player_winning_rec_req: Serializer.Serialize(stream, (network.cs_query_player_winning_rec_req)proto); break;
			case ProtoID.sc_query_player_winning_rec_reply: Serializer.Serialize(stream, (network.sc_query_player_winning_rec_reply)proto); break;
			case ProtoID.cs_niu_query_in_game_player_num_req: Serializer.Serialize(stream, (network.cs_niu_query_in_game_player_num_req)proto); break;
			case ProtoID.sc_niu_query_in_game_player_num_reply: Serializer.Serialize(stream, (network.sc_niu_query_in_game_player_num_reply)proto); break;
			case ProtoID.cs_niu_subsidy_req: Serializer.Serialize(stream, (network.cs_niu_subsidy_req)proto); break;
			case ProtoID.sc_niu_subsidy_reply: Serializer.Serialize(stream, (network.sc_niu_subsidy_reply)proto); break;
			case ProtoID.sc_niu_subsidy_info_update: Serializer.Serialize(stream, (network.sc_niu_subsidy_info_update)proto); break;
			case ProtoID.cs_niu_special_subsidy_share: Serializer.Serialize(stream, (network.cs_niu_special_subsidy_share)proto); break;
			case ProtoID.sc_niu_special_subsidy_share: Serializer.Serialize(stream, (network.sc_niu_special_subsidy_share)proto); break;
			case ProtoID.cs_daily_checkin_req: Serializer.Serialize(stream, (network.cs_daily_checkin_req)proto); break;
			case ProtoID.sc_daily_checkin_reply: Serializer.Serialize(stream, (network.sc_daily_checkin_reply)proto); break;
			case ProtoID.sc_daily_checkin_info_update: Serializer.Serialize(stream, (network.sc_daily_checkin_info_update)proto); break;
			case ProtoID.cs_make_up_for_checkin_req: Serializer.Serialize(stream, (network.cs_make_up_for_checkin_req)proto); break;
			case ProtoID.sc_player_phone_num_info_update: Serializer.Serialize(stream, (network.sc_player_phone_num_info_update)proto); break;
			case ProtoID.sc_player_bind_phone_num: Serializer.Serialize(stream, (network.sc_player_bind_phone_num)proto); break;
			case ProtoID.cs_player_bind_phone_num_draw: Serializer.Serialize(stream, (network.cs_player_bind_phone_num_draw)proto); break;
			case ProtoID.sc_player_bind_phone_num_draw_reply: Serializer.Serialize(stream, (network.sc_player_bind_phone_num_draw_reply)proto); break;
			case ProtoID.sc_niu_special_subsidy_info_update: Serializer.Serialize(stream, (network.sc_niu_special_subsidy_info_update)proto); break;
			case ProtoID.cs_rank_query_req: Serializer.Serialize(stream, (network.cs_rank_query_req)proto); break;
			case ProtoID.sc_rank_qurey_reply: Serializer.Serialize(stream, (network.sc_rank_qurey_reply)proto); break;
			case ProtoID.cs_vip_daily_reward: Serializer.Serialize(stream, (network.cs_vip_daily_reward)proto); break;
			case ProtoID.sc_vip_daily_reward: Serializer.Serialize(stream, (network.sc_vip_daily_reward)proto); break;
			case ProtoID.sc_guide_info_update: Serializer.Serialize(stream, (network.sc_guide_info_update)proto); break;
			case ProtoID.cs_guide_next_step_req: Serializer.Serialize(stream, (network.cs_guide_next_step_req)proto); break;
			case ProtoID.sc_guide_next_step_reply: Serializer.Serialize(stream, (network.sc_guide_next_step_reply)proto); break;
			case ProtoID.cs_hundred_last_week_rank_query_req: Serializer.Serialize(stream, (network.cs_hundred_last_week_rank_query_req)proto); break;
			case ProtoID.sc_hundred_last_week_rank_query_reply: Serializer.Serialize(stream, (network.sc_hundred_last_week_rank_query_reply)proto); break;
			case ProtoID.cs_real_name_update: Serializer.Serialize(stream, (network.cs_real_name_update)proto); break;
			case ProtoID.sc_real_name_update: Serializer.Serialize(stream, (network.sc_real_name_update)proto); break;
			case ProtoID.cs_real_name_req: Serializer.Serialize(stream, (network.cs_real_name_req)proto); break;
			case ProtoID.sc_real_name_req: Serializer.Serialize(stream, (network.sc_real_name_req)proto); break;
			case ProtoID.sc_prize_config_update: Serializer.Serialize(stream, (network.sc_prize_config_update)proto); break;
			case ProtoID.cs_prize_query_one_req: Serializer.Serialize(stream, (network.cs_prize_query_one_req)proto); break;
			case ProtoID.sc_prize_query_one_reply: Serializer.Serialize(stream, (network.sc_prize_query_one_reply)proto); break;
			case ProtoID.cs_prize_exchange_req: Serializer.Serialize(stream, (network.cs_prize_exchange_req)proto); break;
			case ProtoID.sc_prize_exchange_reply: Serializer.Serialize(stream, (network.sc_prize_exchange_reply)proto); break;
			case ProtoID.sc_prize_exchange_record_update: Serializer.Serialize(stream, (network.sc_prize_exchange_record_update)proto); break;
			case ProtoID.sc_prize_address_info_update: Serializer.Serialize(stream, (network.sc_prize_address_info_update)proto); break;
			case ProtoID.cs_prize_address_change_req: Serializer.Serialize(stream, (network.cs_prize_address_change_req)proto); break;
			case ProtoID.sc_prize_address_change_reply: Serializer.Serialize(stream, (network.sc_prize_address_change_reply)proto); break;
			case ProtoID.sc_prize_storage_red_point_update: Serializer.Serialize(stream, (network.sc_prize_storage_red_point_update)proto); break;
			case ProtoID.cs_prize_query_phonecard_key_req: Serializer.Serialize(stream, (network.cs_prize_query_phonecard_key_req)proto); break;
			case ProtoID.sc_prize_query_phonecard_key_reply: Serializer.Serialize(stream, (network.sc_prize_query_phonecard_key_reply)proto); break;
			case ProtoID.cs_red_pack_query_list_req: Serializer.Serialize(stream, (network.cs_red_pack_query_list_req)proto); break;
			case ProtoID.sc_red_pack_query_list_reply: Serializer.Serialize(stream, (network.sc_red_pack_query_list_reply)proto); break;
			case ProtoID.cs_red_pack_open_req: Serializer.Serialize(stream, (network.cs_red_pack_open_req)proto); break;
			case ProtoID.sc_red_pack_open_reply: Serializer.Serialize(stream, (network.sc_red_pack_open_reply)proto); break;
			case ProtoID.cs_red_pack_create_req: Serializer.Serialize(stream, (network.cs_red_pack_create_req)proto); break;
			case ProtoID.sc_red_pack_create_reply: Serializer.Serialize(stream, (network.sc_red_pack_create_reply)proto); break;
			case ProtoID.sc_red_pack_notice_update: Serializer.Serialize(stream, (network.sc_red_pack_notice_update)proto); break;
			case ProtoID.cs_red_pack_cancel_req: Serializer.Serialize(stream, (network.cs_red_pack_cancel_req)proto); break;
			case ProtoID.sc_red_pack_cancel_reply: Serializer.Serialize(stream, (network.sc_red_pack_cancel_reply)proto); break;
			case ProtoID.sc_self_red_pack_info: Serializer.Serialize(stream, (network.sc_self_red_pack_info)proto); break;
			case ProtoID.cs_red_pack_do_select_req: Serializer.Serialize(stream, (network.cs_red_pack_do_select_req)proto); break;
			case ProtoID.sc_red_pack_do_select_reply: Serializer.Serialize(stream, (network.sc_red_pack_do_select_reply)proto); break;
			case ProtoID.cs_red_pack_search_req: Serializer.Serialize(stream, (network.cs_red_pack_search_req)proto); break;
			case ProtoID.sc_red_pack_search_reply: Serializer.Serialize(stream, (network.sc_red_pack_search_reply)proto); break;
			case ProtoID.cs_share_new_bee_reward_req: Serializer.Serialize(stream, (network.cs_share_new_bee_reward_req)proto); break;
			case ProtoID.sc_share_new_bee_reward_reply: Serializer.Serialize(stream, (network.sc_share_new_bee_reward_reply)proto); break;
			case ProtoID.cs_share_mission_reward_req: Serializer.Serialize(stream, (network.cs_share_mission_reward_req)proto); break;
			case ProtoID.sc_share_mission_reward_reply: Serializer.Serialize(stream, (network.sc_share_mission_reward_reply)proto); break;
			case ProtoID.sc_share_info: Serializer.Serialize(stream, (network.sc_share_info)proto); break;
			case ProtoID.sc_share_mission_update: Serializer.Serialize(stream, (network.sc_share_mission_update)proto); break;
			case ProtoID.cs_share_draw_request: Serializer.Serialize(stream, (network.cs_share_draw_request)proto); break;
			case ProtoID.cs_share_friend_request: Serializer.Serialize(stream, (network.cs_share_friend_request)proto); break;
			case ProtoID.cs_share_rank_request: Serializer.Serialize(stream, (network.cs_share_rank_request)proto); break;
			case ProtoID.sc_share_draw_response: Serializer.Serialize(stream, (network.sc_share_draw_response)proto); break;
			case ProtoID.sc_share_history_response: Serializer.Serialize(stream, (network.sc_share_history_response)proto); break;
			case ProtoID.sc_share_rank_response: Serializer.Serialize(stream, (network.sc_share_rank_response)proto); break;
			case ProtoID.sc_draw_count_response: Serializer.Serialize(stream, (network.sc_draw_count_response)proto); break;
			case ProtoID.sc_task_seven_info_response: Serializer.Serialize(stream, (network.sc_task_seven_info_response)proto); break;
			case ProtoID.cs_task_seven_award_request: Serializer.Serialize(stream, (network.cs_task_seven_award_request)proto); break;
			case ProtoID.sc_task_seven_award_response: Serializer.Serialize(stream, (network.sc_task_seven_award_response)proto); break;
			case ProtoID.cs_share_with_friends_req: Serializer.Serialize(stream, (network.cs_share_with_friends_req)proto); break;
			case ProtoID.sc_shop_all_item_base_config: Serializer.Serialize(stream, (network.sc_shop_all_item_base_config)proto); break;
			case ProtoID.cs_shop_buy_query: Serializer.Serialize(stream, (network.cs_shop_buy_query)proto); break;
			case ProtoID.sc_shop_buy_reply: Serializer.Serialize(stream, (network.sc_shop_buy_reply)proto); break;
			case ProtoID.sc_golden_bull_info_update: Serializer.Serialize(stream, (network.sc_golden_bull_info_update)proto); break;
			case ProtoID.cs_golden_bull_draw_req: Serializer.Serialize(stream, (network.cs_golden_bull_draw_req)proto); break;
			case ProtoID.sc_golden_bull_draw_reply: Serializer.Serialize(stream, (network.sc_golden_bull_draw_reply)proto); break;
			case ProtoID.sc_month_card_info_update: Serializer.Serialize(stream, (network.sc_month_card_info_update)proto); break;
			case ProtoID.cs_month_card_draw_req: Serializer.Serialize(stream, (network.cs_month_card_draw_req)proto); break;
			case ProtoID.sc_month_card_draw_reply: Serializer.Serialize(stream, (network.sc_month_card_draw_reply)proto); break;
			case ProtoID.cs_cash_transformation_req: Serializer.Serialize(stream, (network.cs_cash_transformation_req)proto); break;
			case ProtoID.sc_cash_transformation_reply: Serializer.Serialize(stream, (network.sc_cash_transformation_reply)proto); break;
			case ProtoID.cs_golden_bull_mission: Serializer.Serialize(stream, (network.cs_golden_bull_mission)proto); break;
			case ProtoID.sc_golden_bull_mission: Serializer.Serialize(stream, (network.sc_golden_bull_mission)proto); break;
			
		default: break;
        }
    }
}
