-- -----------------------------------------------------------------


-- *
-- * Filename:    hundredCowRoom.lua
-- * Summary:     百人牛牛房间
-- *
-- * Version:     1.0.0
-- * Author:      HAN-PC
-- * Date:        2017-3-2 16:32:45
-- -----------------------------------------------------------------

-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("hundredCowRoom")
local UnityTools = IMPORT_MODULE("UnityTools")
local roomMgr = IMPORT_MODULE("roomMgr")
local platformMgr = IMPORT_MODULE("PlatformMgr")
local protobuf = sluaAux.luaProtobuf.getInstance()

local function initListener()
    roomMgr.SetMaxPersonCnt(8)
    protobuf:registerMessageScriptHandler(protoIdSet.sc_hundred_niu_room_state_update,"HCRRecvRoomStateReply")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_hundred_niu_enter_room_reply,"HCRRecvEnterRoomReply")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_hundred_niu_seat_player_info_update,"HUCRecvPlayerInfoReply")
    -- protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_leave_room_player_pos_update,"RoomMgrRecvPlayerLeaveReply")
    -- protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_player_back_to_room_info_update,"RoomMgrRecvBackToRoomReply")
end
M.InitListener = initListener

local function removeListener()
    protobuf:removeMessageHandler(protoIdSet.sc_hundred_niu_room_state_update)
    protobuf:removeMessageHandler(protoIdSet.sc_hundred_niu_enter_room_reply)
    protobuf:removeMessageHandler(protoIdSet.sc_hundred_niu_seat_player_info_update)
    -- protobuf:removeMessageHandler(protoIdSet.sc_niu_leave_room_player_pos_update)
    -- protobuf:removeMessageHandler(protoIdSet.sc_niu_player_back_to_room_info_update)
end
M.RemoveListener = removeListener

local function cleanRoom()
    removeListener()
end
M.CleanRoom = cleanRoom

local function setPlayerIcon(go,gameObject, playerInfo, dealer)
    local head = UnityTools.FindGo(gameObject.transform, "icon")
    local mask = UnityTools.FindGo(gameObject.transform, "mask")
    if head == nil then return nil end
    local headSpr = UnityTools.FindCo(gameObject.transform,"UISprite","icon/img/head")
    if playerInfo == nil then
        -- head:SetActive(false)
        -- mask:SetActive(true)
        UnityTools.SetActive(head, false)
        UnityTools.SetActive(mask, true)
        if headSpr ~= nil then
            headSpr.spriteName = "player_empty"
        end
    else 
        UnityTools.SetActive(head, true)
        UnityTools.SetActive(mask, false)
        -- head:SetActive(true)
        -- mask:SetActive(false)  
        local img = UnityTools.FindCo(head.transform, "UITexture", "img")
        img.mainTexture = nil
        if playerInfo.player_uuid == platformMgr.PlayerUuid() then
            UnityTools.SetPlayerHead(playerInfo.icon_url, img, true)
        else
            UnityTools.SetPlayerHead(playerInfo.icon_url, img, false)
        end
        local money = UnityTools.FindCo(head.transform, "UILabel", "money")
        
        if playerInfo.gold ~= nil then
            if playerInfo.player_uuid == platformMgr.PlayerUuid() then
                money.text = UnityTools.GetShortNum(platformMgr.GetGod())
            else
                money.text = UnityTools.GetShortNum(playerInfo.gold)
            end
        else
            money.text = UnityTools.GetShortNum(0)
        end
        -- if playerInfo.icon_url == "" and headSpr ~= nil then
        headSpr.spriteName = platformMgr.PlayerDefaultHead(playerInfo.sex)
        -- end
    end

    local getNum = UnityTools.FindCo(gameObject.transform, "UILabel", "get")
    local costNum = UnityTools.FindCo(gameObject.transform, "UILabel", "cost")
    UnityTools.SetActive(getNum, false)
    UnityTools.SetActive(costNum, false)
    getNum.text = ""
    costNum.text = ""

    local vip = UnityTools.FindGo(gameObject.transform, "vip/vipBox")
    if vip ~= nil then
        if playerInfo ~= nil then
            UnityTools.SetNewVipBox(vip, playerInfo.vip_level,"vip",go)
        else
            UnityTools.SetNewVipBox(vip, -1,"vip")
        end

        -- 审核版本屏蔽内容
        if version.VersionData.IsReviewingVersion() then
            vip:SetActive(false)
            LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
            LogError("审核版本：屏蔽对局内头像VIP显示")
        end
    end

end
M.SetPlayerIcon = setPlayerIcon

function HCRRecvEnterRoomReply(msgID, msgData)
    
    if msgData ~= nil and (msgData.result == 0 or msgData.result == 2) then
        if msgData.game_data ~= nil then
            local gameData = msgData.game_data
            roomMgr.SetMyPosition(0)
            
            roomMgr.SetLastTime(gameData.end_sec_time)
            if msgData.result == 2 and gameData.state_id == 20 then
                roomMgr.bContinued = 2
            elseif gameData.state_id == 20 then
                gameData.state_id = 30
                roomMgr.bContinued = 1 --跳过下注阶段
            else
                roomMgr.bContinued = 0
            end
            roomMgr.SetState(gameData.state_id)
            triggerScriptEvent(EVENT_NIU_ENTER_ROOM, gameData)
            
        end
        
    else
        LogError("Recv nil HCRRecvEnterRoomReply")
    end
end

function HCRRecvRoomStateReply(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        roomMgr.SetState(msgData.state_id)
        roomMgr.SetLastTime(msgData.end_sec_time)
        if roomMgr.State() == 10 then


        elseif roomMgr.State() == 20 then
            
        elseif roomMgr.State() == 30 then

        end
        -- print("triggerScriptEvent->" .. roomMgr.State())
        triggerScriptEvent(EVENT_NIU_UPDATE_ROOM_STATE, msgData)
    else
        LogError("Recv nil NCRRecvRoomStateReply")
    end
end


local function sendJoinRoomMsg(type)
    roomMgr.sendMsg(protoIdSet.cs_hundred_niu_enter_room_req, {room_type = type})
end
-- 加入房间
M.SendJoinRoomMsg = sendJoinRoomMsg

local function sendInGameMsg()
    LogWarn("Continue Game")
    roomMgr.sendMsg(protoIdSet.cs_hundred_niu_in_game_syn_req, {})
end
-- 继续游戏
M.SendInGameMsg = sendInGameMsg

local function sendRateMsg(rate, index)
    roomMgr.sendMsg(protoIdSet.cs_hundred_niu_free_set_chips_req, {pos = index, chips_num = rate})
end
-- 下注
M.SendRateMsg = sendRateMsg


local function sendLeaveRoomMsg()
    roomMgr.sendMsg(protoIdSet.cs_hundred_leave_room_req, {})
end
-- 离开房间
M.SendLeaveRoomMsg = sendLeaveRoomMsg