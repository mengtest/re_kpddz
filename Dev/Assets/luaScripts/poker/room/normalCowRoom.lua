-- -----------------------------------------------------------------


-- *
-- * Filename:    normalCowRoom.lua
-- * Summary:     普通牛牛房间
-- *
-- * Version:     1.0.0
-- * Author:      HAN-PC
-- * Date:        2017-3-2 14:42:01
-- -----------------------------------------------------------------

-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("normalCowRoom")
local UnityTools = IMPORT_MODULE("UnityTools")
local roomMgr = IMPORT_MODULE("roomMgr")
local platformMgr = IMPORT_MODULE("PlatformMgr")
local _itemMgr = IMPORT_MODULE("ItemMgr")
local protobuf = sluaAux.luaProtobuf.getInstance()

local function initListener()
    roomMgr.SetMaxPersonCnt(5)
    protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_room_state_update,"NCRRecvRoomStateReply")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_enter_room_reply,"RoomMgrRecvEnterRoomReply")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_enter_room_player_info_update,"RoomMgrRecvPlayerInfoReply")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_leave_room_player_pos_update,"RoomMgrRecvPlayerLeaveReply")
    protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_player_back_to_room_info_update,"RoomMgrRecvBackToRoomReply")
    
end
M.InitListener = initListener

local function removeListener()
    protobuf:removeMessageHandler(protoIdSet.sc_niu_room_state_update)
    protobuf:removeMessageHandler(protoIdSet.sc_niu_enter_room_reply)
    protobuf:removeMessageHandler(protoIdSet.sc_niu_enter_room_player_info_update)
    protobuf:removeMessageHandler(protoIdSet.sc_niu_leave_room_player_pos_update)
    protobuf:removeMessageHandler(protoIdSet.sc_niu_player_back_to_room_info_update)
end
M.RemoveListener = removeListener

local function cleanRoom()
    removeListener()
end
M.CleanRoom = cleanRoom

function RoomMgrChangeBoardSprite(board, side)
    if side == 1 then
        if board.spriteName == "buling_h_1" then
            board.spriteName = "buling_h_2"
        else
            board.spriteName = "buling_h_1"
        end
    else
        if board.spriteName == "buling_v_1" then
            board.spriteName = "buling_v_2"
        else
            board.spriteName = "buling_v_1"
        end
    end
    
    if roomMgr.State() == roomMgr.TState.BetRounds and roomMgr.LastTime() > 4 then
        gTimer.registerOnceTimer(250, "RoomMgrChangeBoardSprite", board, side)
    else 
        board.spriteName = "light_yellow"
    end
end

-- 设置角色头像区
local function setPlayerIcon(go,gameObject, playerInfo, dealer, ignoreFlag)
    if gameObject == nil then return nil end
    local tf = gameObject.transform
    local pImg = UnityTools.FindGo(tf, "player_img_bg")
    if pImg == nil then return nil end
    local hImg = UnityTools.FindCo(pImg.transform, "UITexture", "img")
    local mask = UnityTools.FindGo(pImg.transform, "mask")
    local board = UnityTools.FindGo(pImg.transform, "board")
    local defaultImg = UnityTools.FindCo(pImg.transform,"UISprite","img/head")
    if playerInfo ~= nil then
        ComponentData.Get(board).Text = playerInfo.player_uuid
        UnityTools.AddOnClick(board, function (btn) 
            local uuid = ComponentData.Get(btn).Text
            if platformMgr.PlayerUuid() ~= uuid then
                platformMgr.OpenPlayerInfoInGame(uuid)
            else 
                -- 审核版本屏蔽内容
                if version.VersionData.IsReviewingVersion() then
                    LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
                    LogError("审核版本：屏蔽兑换功能")
                    return;
                end
                UnityTools.CreateLuaWin("ExchangeWin")
            end
        end)
    end
    if playerInfo == nil and roomMgr.RoomType() == 10 and gameObject.name =="playerCell1" then
        playerInfo = {}
        playerInfo.player_uuid = platformMgr.PlayerUuid()
        playerInfo.player_name = platformMgr.UserName()
        playerInfo.gold_num = platformMgr.GetDiamond()
        playerInfo.icon_url = platformMgr.GetIcon()
        playerInfo.vip_level= platformMgr.GetVipLv()
        playerInfo.sex = platformMgr.getSex()
    end
    if playerInfo == nil then 
    -- 没有头像
        hImg.mainTexture = nil
        board:SetActive(false)
        mask:SetActive(true)
        if defaultImg ~= nil then
            defaultImg.spriteName = "player_empty"
            -- defaultImg.spriteName = platformMgr.PlayerDefaultHead(playerInfo.sex)
        end
    else
    -- 设置头像
        hImg.mainTexture = nil
        UnityTools.SetPlayerHead(playerInfo.icon_url, hImg, platformMgr.PlayerUuid() == playerInfo.player_uuid)
        board:SetActive(true)
        mask:SetActive(false)
        -- if defaultImg ~= nil and (playerInfo.icon_url == nil or playerInfo.icon_url == "") then
            defaultImg.spriteName = platformMgr.PlayerDefaultHead(playerInfo.sex)
        -- end
    end
    
    
    -- 设置名字与金币信息
    local name = UnityTools.FindCo(tf, "UILabel", "name") 
    if name ~= nil and playerInfo ~= nil then
        name.gameObject:SetActive(true)
        name.text = playerInfo.player_name
        local money = UnityTools.FindCo(name.transform, "UILabel", "money") 
        if money ~= nil then
            if playerInfo.player_uuid == platformMgr.PlayerUuid() and roomMgr.RoomType()==10 then
                money.text = UnityTools.GetShortNum(platformMgr.GetDiamond())
            else
                money.text = UnityTools.GetShortNum(playerInfo.gold_num)
            end
        end
        
        local quan = UnityTools.FindCo(name.transform, "UILabel", "quan") 
        if quan ~= nil then
            if roomMgr.RoomType() == 10 then
                local redBagNum = _itemMgr.GetItemNum(109);
                if redBagNum % 10 == 0 then
                    quan.text = string.format("%d",redBagNum/10);
                else
                    quan.text = string.format("%.1f",redBagNum/10);
                end
            else
                quan.text = UnityTools.GetShortNum(platformMgr.GetCash())
            end

            -- 审核版本屏蔽内容
            if version.VersionData.IsReviewingVersion() then
                quan.gameObject:SetActive(false)
                LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
                LogError("审核版本：屏蔽兑换券")
            end
        end
    else
        name.gameObject:SetActive(false)
        if money ~= nil then
            money.text = UnityTools.GetShortNum(platformMgr.GetDiamond())
        end
        local quan = UnityTools.FindCo(name.transform, "UILabel", "quan") 
        if quan ~= nil then
            quan.text = UnityTools.GetShortNum(platformMgr.GetCash())
        end
    end

    local vip = UnityTools.FindGo(pImg.transform, "vip/vipBox")
    if vip ~= nil then
        if playerInfo ~= nil then
            
            UnityTools.SetNewVipBox(vip, playerInfo.vip_level,"vip", go)
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

    local flag = UnityTools.FindTf(tf, "flag")
    if flag ~= nil and ignoreFlag == nil then 
        if dealer ~= nil then
            flag.gameObject:SetActive(true)
            local boardSp = UnityTools.FindCo(pImg.transform, "UISprite", "board")
            local bg = UnityTools.FindCo(pImg.transform, "UISprite", "bg")
            local side = 1
            if bg == nil then
                side = 1
            else
                if bg.spriteName == "playerSide_h" then
                    side = 1
                else 
                    side = 2
                end
            end
            local effect_icon = UnityTools.AddEffect(flag.transform,"effect_zhuang", {complete = function (obj) 
    
                local gObj = obj.EffectGameObj
                gObj.transform.localScale = UnityEngine.Vector3.one
            end})
            UnityTools.PlaySound("Sounds/dealerChose")
            gTimer.registerOnceTimer(250, "RoomMgrChangeBoardSprite", boardSp, side)
        else
            local dealerLayer = UnityTools.FindGo(tf, "dealer")
            UnityTools.SetActive(dealerLayer, false)
            -- dealerLayer:SetActive(false)
            flag.gameObject:SetActive(false)
        end
    end
end
M.SetPlayerIcon = setPlayerIcon

function NCRRecvRoomStateReply(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        roomMgr.SetState(msgData.state_id)
        roomMgr.SetLastTime(msgData.end_sec_time)

        if roomMgr.State() == roomMgr.TState.WaitDealer then
            -- 更新牌事件
            if msgData.open_card_list ~= nil then
                roomMgr.SetMyPokerInfo(msgData.open_card_list)
                triggerScriptEvent(EVENT_NIU_UPDATE_MY_POKER, msgData.open_card_list)
            end
        elseif roomMgr.State() == roomMgr.TState.OverDealer then
            -- 设置庄家位
            local dealerPos = msgData.master_pos
            roomMgr.SetDealerIndex(dealerPos)
            
        elseif roomMgr.State() == roomMgr.TState.Flop then
            -- 更新牌事件
            if msgData.open_card_list ~= nil then
                local index = roomMgr.GetMyIndex()
                triggerScriptEvent(EVENT_ROOM_RECV_ADD_POKER, index, msgData.open_card_list)
                triggerScriptEvent(EVENT_NIU_UPDATE_MY_POKER, msgData.open_card_list)
            end
        elseif roomMgr.State() == roomMgr.TState.Over then
            print("roomMgr.State() == TState.Over")
            if msgData.settle_list ~= nil then
                local resultList = msgData.settle_list.all_player_settle_info
                if resultList ~= nil then
                    gTimer.registerOnceTimer(1000, function() 
                        triggerScriptEvent(EVENT_NIU_UPDATE_RESULT, resultList)      
                    end)
                end
            end
        end
        -- print("triggerScriptEvent->" .. roomMgr.State())
        triggerScriptEvent(EVENT_NIU_UPDATE_ROOM_STATE, roomMgr.State())
    else
        LogError("Recv nil NCRRecvRoomStateReply")
    end
end

local function sendJoinRoomMsg(type)
    roomMgr.sendMsg(protoIdSet.cs_niu_enter_room_req, {room_type = type})
end

local function sendDealerRateMsg(rate)
    if roomMgr.State() ~= roomMgr.TState.WaitDealer then return nil end
    roomMgr.sendMsg(protoIdSet.cs_niu_choose_master_rate_req, {rate_num = rate})
end

local function sendRateMsg(rate)
    if roomMgr.State() ~= roomMgr.TState.BetRounds then return nil end
    roomMgr.sendMsg(protoIdSet.cs_niu_choose_free_rate_req, {rate_num = rate})
end

local function sendLeaveRoomMsg()
    roomMgr.sendMsg(protoIdSet.cs_niu_leave_room_req, {})
end

local function sendSubmitMsg()
    if roomMgr.State() ~= roomMgr.TState.Flop then return nil end
    roomMgr.sendMsg(protoIdSet.cs_niu_submit_card_req, {})
end

local function sendInGameMsg()
    if roomMgr.State() ~= roomMgr.TState.Over then return nil end
    roomMgr.sendMsg(protoIdSet.cs_niu_syn_in_game_state_req, {})
end

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

return M