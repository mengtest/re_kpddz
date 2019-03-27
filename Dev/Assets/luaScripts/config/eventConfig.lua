
-- EventManager事件定义

-- 定义一个计数器
local counter = function ()
    local index = 0
    return function () index = index + 1 return index end
end

-- 计数器
local INDEX = counter()

-- 添加全局变量并赋值一个自增长ID
local function add(evt)
    _G[evt] = INDEX()
end

---------------------------------------------------事件定义----------------------------------------------------------------

                      --C#与Lua交互事件--
add "event_resource_update_from_csharp"                        --更新数据
add "event_clear_all_data_from_csharp"                         --清理数据
add "event_player_head_from_csharp"                            --更新头像
add "event_open_guide"                                         --开启新手引导
                      --****-----------
add "EVENT_GAME_START_EFFECT"                                  -- 播放游戏开始特效
add "EVENT_NIU_UPDATE_PLAYER_INFO"                             -- 牛牛 更新用户数据
add "EVENT_NIU_UPDATE_ROOM_STATE"                              -- 牛牛 更新房间状态
add "EVENT_NIU_UPDATE_MY_POKER"                                -- 牛牛 更新我的扑克牌
add "EVENT_NIU_UPDATE_RESULT"                                  -- 牛牛 更新结果数据
add "EVENT_NIU_ENTER_ROOM"                                     -- 牛牛 进入房间

add "EVENT_RESCOURCE_UDPATE"                                   --金币或者钻更新   t = 1:更新所有玩家信息  t = 2:更新金币及钻
add "EVENT_CHAGNE_TOP"                                         --改变主界面顶部
add "EVENT_SHOW_MAIN_WIN"                                      --显示主功能上的按钮


add "EVENT_ROOM_RECV_UPDATE_POKER"                             -- 房间收到牌组信息
add "EVENT_ROOM_RECV_ADD_POKER"                                -- 房间收到添加牌组信息
add "EVENT_ROOM_RECV_DEL_POKER"                                -- 房间收到删除牌组信息
add "EVENT_ROOM_RECV_CHAT_INFO"                                -- 房间收到聊天信息

add "EVENT_UPDATE_PLAYER_WIN_INFO"                             --更新玩家胜负的记录信息
add "END_CHANGE_NAME_AND_SEX"                                  --玩家改名成功
add "END_CHANGE_PLAYER_HEAD"                                   --玩家头像成功
add "SEND_MAGIC_FACE_TO_OTHER"                                 --发送表示到其他玩家
add "UPDATE_ITEM"                                              --物品刷新
-- add "UPDATE_SUBSIDY"                                           --补助刷新
add "UPDATE_MAIN_WIN_RED"                                      --更新主界面上的红点
add "EXCHANGE_QUERY_BACK"                                      --兑换库存返回
add "SHOW_PLAYER_INFO"                                    --显示排行上玩家的基本信息

add "EXIT_CLEAR_ALL_DATA"                                      --退出清理所有的数据
add "OPEN_RECHANGE_CARD_PASSWORD_WIN"                          --打卡提取卡密界面

-- 登录奖励模块事件定义
add "LOGIN_AWARD_UI_EVENT_REFRESH_WIN"                         --刷新登录奖励界面
add "LOGIN_AWARD_UI_EVENT_UPDATE_DAY"                          --更新某一天的UI数据 param: 索引
add "LOGIN_AWARD_UI_EVENT_VIP_AWARD_STATUS_CHANGE"             --VIP领奖状态改变

add "RED_BAG_UPDATE"                                           --红包界面刷新
add "RED_BAG_GUESS_UPDATE"                                     --红包猜界面刷新


add "EVENT_LABA_POOL_UPDATE"                                  -- 水果机奖池刷新
add "EVENT_LABA_SPIN_REPLY"                                  -- 水果机抽奖返回
add "EVENT_LABA_SPIN_AUTO"                                  -- 水果机自动抽奖
add "EVENT_LABA_MODE_SWTICH"     
add "UPDATE_LUCKY_COW_WIN"                                    ---更新招财金牛的界面
add "GUIDE_STEP_UPDATE"                                       ---新手引导步骤更新
add "EVENT_RENDER_CHANGE_WIN"                                  ---界面的RenderQ发生改变
add "EVENT_GAMECENTERR_ENDER_CHANGE_WIN"                                  ---界面的RenderQ发生改变
add "EVENT_MONTH_CARD_UPDATE"                                  ---月卡界面刷新
add "EVENT_SHOP_BUY_UPDATE"                                    ---商城购买成功返回

add "EVENT_LABA_GOLD_UPDATE"                                    ---拉霸金币刷新add "EVENT_SHOP_BUY_UPDATE"                                    ---商城购买成功返回
add "EVENT_RED_WIN_UPDATE"
add "EVENT_FIRST_PAY"                                          ---商城购买成功返回
add "EVNET_HIDE_FUNCTION_FLAG_UDPATE"                          ---功能按钮显示及关闭

add "ACTIVITY_AND_ANNOUNCEMENT_UPDATE_ACTIVITY_PAGE"           -- 更新活动页面
add "ACTIVITY_AND_ANNOUNCEMENT_UPDATE_ACTIVITY_AWARD_ITEM"           -- 更新活动页面奖励对象

add "NORMAL_COW_UI_WIN_START"                                   -- 看牌抢庄UI界面开始
add "NORMAL_COW_TOP_WIN_START"                                   -- 看牌抢庄TOP界面开始
add "NORMAL_COW_RED_UI_WIN_START"                                   -- 看牌抢庄红包界面开始

add "EVENT_IS_IN_GAME"                                          -- 已经在游戏中
add "DIAMOND_BAG_UPDATE"                                        -- 钻石福袋更新
add "EVENT_IS_IN_GAME_FOR_CAR"                                          -- 已经在游戏中

add "EVENT_UPDATE_CHAT"                    --聊天
add "EVENT_RICHCAR_MASTER_UPDATE"                    --庄家信息
add "EVENT_RICHCAR_STAKE_RESPONSE"                    --下注返回
add "EVENT_RICHCAR_ROOM_RESPONSE"   --房间信息
add "EVENT_RICHCAR_MASTER_LIST_UPDATE"  --排庄列表刷新
add "EVENT_RICHCAR_STATUS_UPDATE"  --排庄状态刷新
add "EVENT_RICHCAR_RESULT_RESPONSE"  --结果返回
add "EVENT_UPDATE_RICH_CAR_GOLD"  --结果返回
add "EVENT_UPDATE_RICH_CAR_STATUS"  --排庄返回
add "EVENT_UPDATE_RICH_CAR_USERLIST"  --排庄返回
add "EVENT_UPDATE_RICH_CAR_TIP"  --排庄提示
add "EVENT_UPDATE_RICHCAR_STAKEINFO"  --
add "EVENT_UPDATE_RICHCAR_ADDMONEY_RESPONSE"  --
add "EVENT_UPDATE_SHARE_MAIN_UPDATE"                            --分享主界面刷新

add "EVENT_UPDATE_RANK_INFO"                            --刷新百人排行
add "EVENT_TASK_INFO_UPDATE"

add "EVENT_ADD_NOTICE"
add "HIDE_OR_SHOW_GIRL"
add "COW_RED_PACK_UPDATE"
add "EVENT_SEVEN_TASK_STATUS_UPDATE" --七日狂欢刷新
add "EVENT_NEW_SHARE_LOTTO_RESPONSE" --分享抽奖返回
add "EVENT_SHARE_RANK_RESPONSE" --分享排名
add "EVENT_SHARE_HISTORY_RESPONSE" --我的分享
-- add "EVENT_NEWSHARE_RED_LOTTO_COUNT" --抽奖次数
add "EVENT_AUDIOLOAD_COMPLETE" --声音播放

add "EVENT_UPDATE_RECHARGE_TASK" --yiben

add "EVENT_UPDATE_FRUIT_SUPER_RANK_INFO"  --超级水果排行榜
add "EVENT_RECONNECT_SOCKET"