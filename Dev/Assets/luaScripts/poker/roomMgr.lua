-- -----------------------------------------------------------------


-- *
-- * Filename:    roomMgr.lua
-- * Summary:     房间管理器
-- *              位置信息列表，房间信息 
-- * Version:     1.0.0
-- * Author:      HAN-PC
-- * Date:        2017-2-7 10:09:40
-- -----------------------------------------------------------------

-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("roomMgr")

local UnityTools = IMPORT_MODULE("UnityTools")
local platformMgr = IMPORT_MODULE("PlatformMgr")
local protobuf = sluaAux.luaProtobuf.getInstance()

-- 牛牛房间状态定义
local TState = {
    WaitJoin = 10,          -- 等待加入房间
    WaitStart = 11,         -- 等待牌局开始
    WaitDealer = 20,        -- 等待选庄
    OverDealer = 21,        -- 显示庄家
    BetRounds = 30,         -- 下注
    Flop = 40,              -- 开牌
    Over = 50,              -- 结算
}

-- 房间类型
local TRoom = {
    "normalCowRoom",
    "hundredCowRoom",
}

-------------------外部设置信息
-- 房间类型(房间ID，配置表)
local _room_type = 0    
local _game_type = 0

-------------------房间配置信息
-- 最大人数
local _room_person_max = 5
-- 房间佣金
local _room_rake = 100
-- 底注
local _room_min_bet = 50

-------------------网络数据
-- 当前人数
local _room_person_cnt = 0
-- 房间位置信息列表
local _room_pos_list = {}
-- 位置与对应索引表
local _pos_to_index = {}
-- 自己所在的位置
local _my_pos = 1 
-- 当前房间状态(默认等待加入)
local _room_state = TState.WaitJoin
-- 房间状态倒计时
local _room_state_ltime = 0
local _state_end_time = 0
-- 庄家位置索引
local _dealer_index = 0

local _min_gold_limit = 0
local _max_gold_limit = 0

local _enter_room_call = nil

local _currentRoom = nil
local _currentRoomName = nil

local _lastChatWinType = 1
local _chatInfoList = {}
M.bContinued = 0
M.bExiting = false
local function resetRoomData()
    _room_pos_list = {}
    _pos_to_index = {}
    _my_pos = 1
    _room_state = TState.WaitJoin
    _room_state_ltime = 0
    _dealer_index = 0
    _state_end_time = 0
    _chatInfoList = {}
    _lastChatWinType = 1
end

-- 卸载房间
local function unloadRoom()
    if _currentRoom ~= nil then 
        _currentRoom.CleanRoom()
    end
    CLEAN_MODULE(_currentRoomName)
    _currentRoom = nil
    _game_type = 0
end

-- 清空房间数据
local function cleanRoom()
    -- protobuf:removeMessageHandler(protoIdSet.sc_niu_player_room_info_update)
    protobuf:removeMessageHandler(protoIdSet.sc_player_chat)
    protobuf:removeMessageHandler(protoIdSet.sc_tips)
    gTimer.recycling("RoomMgr")
    unloadRoom()
    resetRoomData()
end

local function exitMgr()
    cleanRoom()
end

-- 加载房间
local function loadRoom(roomIndex)
    local roomName = TRoom[roomIndex]
    LoadLuaFile("poker/room/" .. roomName)
    if _currentRoom == nil then
        _currentRoom = IMPORT_MODULE(roomName)
        _currentRoomName = roomName
        _currentRoom.InitListener()
    end
end


local function initRoomInfo(roomType)
    _room_min_bet = tonumber(LuaConfigMgr.BettingRoomConfig[tostring(_room_type)].score)
    _min_gold_limit = tonumber(stringToTable(LuaConfigMgr.BettingRoomConfig[tostring(_room_type)].doorsill, ",")[1])
    local maxVal = stringToTable(LuaConfigMgr.BettingRoomConfig[tostring(_room_type)].doorsill, ",")[2]
    if roomType == 10 then
        _room_person_max = 3
    end
    if maxVal == nil then 
        maxVal = 0
    end
    _max_gold_limit = tonumber(maxVal)
    protobuf:registerMessageScriptHandler(protoIdSet.sc_player_chat,"RoomMgrRecvChatInfoReply")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_tips,"RoomMgrRecvTipsReply")
    -- 设置外部信息
end

local function initMgr(room, roomType, enterCall)
    _game_type = tonumber(room)     
    _room_type = roomType
    _enter_room_call = enterCall
    M.SendReqRoomMsg()
end

-- 初始化座位信息
local function initPosition()
    for index = 1, _room_person_max, 1 do
        local p = _my_pos + index - 1
        if p > _room_person_max then
            p = p - _room_person_max
        end
        _pos_to_index[p] = index
    end
end

-- 进入房间设置我的座位信息
local function setMyPosition(pos)
    _my_pos = pos
    initPosition()
end

-- 座位号转索引编号
local function posToIndex(pos)
    return _pos_to_index[pos]
end

-- 角色进入
local function playerInPosition(playerInfo)
    local index = posToIndex(playerInfo.pos)
    if index == nil then LogError(playerInfo.pos .. " -> playerInPosition") end
    _room_pos_list[index] = playerInfo
    _room_person_cnt = _room_person_cnt + 1
    -- 发送更新用户信息事件
    triggerScriptEvent(EVENT_NIU_UPDATE_PLAYER_INFO, index, _room_pos_list[index])
end

-- 角色离开
local function playerExitPosition(index)
    _room_pos_list[index] = nil
    _room_person_cnt = _room_person_cnt - 1
    triggerScriptEvent(EVENT_NIU_UPDATE_PLAYER_INFO, index, nil)
end

-- 获得角色列表
local function playerInfoList()
    return _room_pos_list
end

-- 获得位置上的角色
local function getPlayerInfo(index)
    return _room_pos_list[index]
end

-- 根据uuid获得角色index
local function getIndexByUUID(uuid)
    for k, v in pairs(_room_pos_list) do
        if v ~= nil and v.player_uuid == uuid then
            return k
        end
    end
    return 0
end

local function removeAllPlayerPosition()
    for k, v in pairs(_room_pos_list) do
        if v ~= nil then
            playerExitPosition(k)
        end
    end
    _room_person_cnt = 0
end

-- 设置位置牌信息
local function setPositionPoker(pos, pokerInfo, type)
    local index = posToIndex(pos)
    pokerInfo.Count = #pokerInfo
    triggerScriptEvent(EVENT_ROOM_RECV_UPDATE_POKER, index, pokerInfo, type)
end

local function setMyPokerInfo(pokerInfos)
    setPositionPoker(_my_pos, pokerInfos)
end

-- 设置状态剩余时间
local function setLastTime(lastTime)
    _state_end_time = lastTime
    -- LogError(tostring(lastTime) .. "   " .. UtilTools.GetServerTime() .. "   " .. _room_state_ltime)
    if _state_end_time ~= nil then
        _room_state_ltime = _state_end_time - UtilTools.GetServerTime()
        if _room_state_ltime < 0 then 
            _room_state_ltime = 0
        end
    end
end

-- 获取消息数据
local function setMsgValue(msgData, value)
    if msgData ~= nil then
        value = msgData
    end
end

-- 设置角色头像区
local function setPlayerIcon(go,gameObject, playerInfo, dealer, ignoreDealer)
    _currentRoom.SetPlayerIcon(go,gameObject, playerInfo, dealer, ignoreDealer)
end

local function setState(state)
    _room_state = state
end

local function getState()
    return _room_state
end

local function getLastTime()
    return _room_state_ltime
end

local function getLastTime()
    return _room_state_ltime
end

local function getDealerIndex()
    return _dealer_index
end

local function getMinBet()
    return _room_min_bet
end

local function setDealerIndex(pos)
    _dealer_index = posToIndex(pos)
    -- LogError(_dealer_index)
end

local function setMaxPersonCnt(cnt)
    _room_person_max = cnt
end

local function getMyIndex()
    return posToIndex(_my_pos)
end

local function getChatInfoList()
    return _chatInfoList
end

local function getGameType()
    return _game_type
end

local function getLimitGold()
    return _min_gold_limit, _max_gold_limit
end

local function changePlayerGold(index, chgNum)
    local playerInfo = getPlayerInfo(index)
    if playerInfo ~= nil then
        local pGold = 0
        
        if playerInfo.gold_num ~= nil then
            
            playerInfo.gold_num = playerInfo.gold_num + chgNum
            pGold = playerInfo.gold_num
        elseif playerInfo.gold ~= nil then
            
            playerInfo.gold = playerInfo.gold + chgNum
            pGold = playerInfo.gold
        end
        
        if getMyIndex() == index and pGold < _min_gold_limit then
            if _room_type == 1 then
                platformMgr.SubsidyOpen(_room_type, function() 
                    
                end, 
                function ()
                    UnityTools.ShowMessage("您携带的金币不足，将不得不离开位置")
                end)        
            elseif _room_type ~= 10 then
                UnityTools.ShowMessage("您携带的金币不足，将不得不离开位置")
            end
        end
    end
end

---------------------网络事件响应函数



function RoomMgrRecvRoomInfoReply(msgID, msgData)
    UtilTools.HideWaitFlag()
    triggerScriptEvent(EVENT_IS_IN_GAME, msgData)
    triggerScriptEvent(EVENT_IS_IN_GAME_FOR_CAR, msgData)
    if _enter_room_call ~= nil then
        local toIndex =_game_type 
        LogError("_game_type=".._game_type)
        if msgData.room_id > 0 then
            _game_type = 1
            _room_type = msgData.room_id
        end
        -- LogError("initMgr .." .. _game_type .. "  " .. _room_type)
        loadRoom(_game_type)
        initRoomInfo(_room_type)
        resetRoomData()
        LogError("coolTime2="..msgData.enter_end_time)
        _enter_room_call(msgData.room_id, _game_type,msgData.enter_end_time-UtilTools.GetServerTime(),toIndex,msgData.enter_end_time)
        _enter_room_call = nil
    end
end

function RoomMgrRecvBackToRoomReply(msgID, msgData)
    _room_state = msgData.state_id
    setLastTime(msgData.end_sec_time)
    if msgData.my_pos ~= nil then
        setMyPosition(msgData.my_pos)
        triggerScriptEvent(EVENT_NIU_ENTER_ROOM, msgData.result)
    end
    if msgData.master_pos ~= nil then
        local dealerPos = msgData.master_pos
        _dealer_index = posToIndex(dealerPos)
    else
        _dealer_index = 0
    end
    RoomMgrRecvPlayerInfoReply(0, msgData)
    if _room_state > TState.WaitDealer then
        triggerScriptEvent(EVENT_NIU_UPDATE_ROOM_STATE, TState.WaitDealer)
    end
    triggerScriptEvent(EVENT_NIU_UPDATE_ROOM_STATE, _room_state)
    
end

function RoomMgrRecvEnterRoomReply(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        removeAllPlayerPosition()
        setMyPosition(msgData.my_pos)
        triggerScriptEvent(EVENT_NIU_ENTER_ROOM, msgData.result)
    else
        UnityTools.PrintTable(msgData)
        LogError("Recv nil RoomMgrRecvEnterRoomReply")
    end
end

function RoomMgrRecvPlayerInfoReply(msgID, msgData)
    -- LogError("Recv RoomMgrRecvPlayerInfoReply " .. #msgData.player_list)
    if UnityTools.CheckMsg(msgID, msgData) then
        if msgData.player_list ~= nil then
            for i = 1, #msgData.player_list, 1 do
                -- if _room_type == 10 and _room_state == 50 then
                --     local timer = gTimer.registerOnceTimer(2000, function(data) 
                --         playerInPosition(data)
                --     end, msgData.player_list[i])
                --     gTimer.setRecycler("RoomMgr", timer)
                -- else
                playerInPosition(msgData.player_list[i])
                -- end
            end
        end
    else
        LogError("Recv nil RoomMgrRecvPlayerInfoReply")
    end
end

function HUCRecvPlayerInfoReply(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        -- LogError("HUCRecvPlayerInfoReply------------")
        if msgData.delete_seat_pos ~= nil then
            local index = posToIndex(msgData.delete_seat_pos)
            playerExitPosition(index)
        end
        if msgData.seat_list ~= nil then
            -- LogError("Count " .. #msgData.seat_list)
            for i = 1, #msgData.seat_list, 1 do
                playerInPosition(msgData.seat_list[i])
            end
        end
    else
        LogError("Recv nil HUCRecvPlayerInfoReply")
    end
end

function RoomMgrRecvPlayerLeaveReply(msgID, msgData)
    
    if UnityTools.CheckMsg(msgID, msgData) then
        local index = posToIndex(msgData.leave_pos)
        -- LogError("RoomMgrRecvPlayerLeaveReply " .. index)
        if index ~= nil then
            playerExitPosition(index)
            triggerScriptEvent(EVENT_ROOM_RECV_DEL_POKER, index)
        end
    else
        LogError("Recv nil RoomMgrRecvPlayerLeaveReply")
    end
end

function RoomMgrRecvChatInfoReply(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        if msgData.room_type == _game_type then
            if msgData.content_type == 0 or msgData.content_type == 3 then
                _chatInfoList[#_chatInfoList + 1] = msgData
            end
            triggerScriptEvent(EVENT_ROOM_RECV_CHAT_INFO, msgData)
        else
            LogError("Recv Error room type .. " .. msgData.room_type)
        end
    else
        LogError("Recv nil RoomMgrRecvPlayerLeaveReply")
    end
end

function RoomMgrRecvTipsReply(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        -- UnityTools.ShowMessage(msgData.text, "[fff601]")
    else
        LogError("Recv nil RoomMgrRecvPlayerLeaveReply")
    end
end

---------------------------发送网络消息
local function sendMsg(msgID, req)
    protobuf:sendMessage(msgID, req);
end

local function sendJoinRoomMsg()
    _currentRoom.SendJoinRoomMsg(_room_type)
end

local function sendDealerRateMsg(rate)
    _currentRoom.SendDealerRateMsg(rate)
end

local function sendRateMsg(...)
    _currentRoom.SendRateMsg(...)
end

local function sendLeaveRoomMsg()
    if _currentRoom == nil then return end 
    LogError("leave")
    _currentRoom.SendLeaveRoomMsg()
end

local function sendSubmitMsg()
    _currentRoom.SendSubmitMsg()
end

local function sendInGameMsg()
    _currentRoom.SendInGameMsg()
end

local function sendReqRoomMsg()
    sendMsg(protoIdSet.cs_niu_query_player_room_info_req, {})
end

local function sendChatMsg(type, text, toUUID)
    toUUID = toUUID or ""
    if toUUID ~= platformMgr.PlayerUuid() then
        sendMsg(protoIdSet.cs_player_chat, {room_type = _game_type, content_type = type, content = text, obj_player_uuid = toUUID})
    else
        UnityTools.ShowMessage("不能给自己发表情哦")
    end
end

M.GetLimitGold = getLimitGold
M.GetGameType = getGameType

M.SendChatMsg = sendChatMsg

M.sendMsg = sendMsg
-- 请求房间状态
M.SendReqRoomMsg = sendReqRoomMsg
-- 加入房间
M.SendJoinRoomMsg = sendJoinRoomMsg
-- 离开房间
M.SendLeaveRoomMsg = sendLeaveRoomMsg
-- 抢庄
M.SendDealerRateMsg = sendDealerRateMsg
-- 下注
M.SendRateMsg = sendRateMsg
-- 提交牌
M.SendSubmitMsg = sendSubmitMsg
-- 继续游戏
M.SendInGameMsg = sendInGameMsg

M.SetMaxPersonCnt = setMaxPersonCnt
M.InitPosition = initPosition
M.ChangePlayerGold = changePlayerGold
M.CleanRoom = cleanRoom
M.InitRoomInfo = initRoomInfo
M.SetMyPosition = setMyPosition
M.PosToIndex = posToIndex
M.PlayerInfoList = playerInfoList
M.GetPlayerInfo = getPlayerInfo
M.SetPlayerIcon = setPlayerIcon
M.ExitMgr = exitMgr
M.InitMgr = initMgr
M.State = getState
M.SetState = setState
M.LastTime = getLastTime
M.SetLastTime = setLastTime
M.DealerIndex = getDealerIndex
M.TState = TState
M.MinBet = getMinBet
M.SetMyPokerInfo = setMyPokerInfo
M.SetDealerIndex = setDealerIndex
M.GetMyIndex = getMyIndex
M.PlayerExitPosition = playerExitPosition
M.PlayerInPosition = playerInPosition
M.GetChatInfoList = getChatInfoList
M.ChatWinLastType = _lastChatWinType
M.GetIndexByUUID = getIndexByUUID
M.RemoveAllPlayerPosition = removeAllPlayerPosition
local function getRoomType()
    return _room_type
end
M.RoomType = getRoomType


protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_player_room_info_update,"RoomMgrRecvRoomInfoReply")

return M