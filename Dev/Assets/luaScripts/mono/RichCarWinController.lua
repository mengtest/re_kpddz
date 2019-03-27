-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    RichCarWinController.lua
-- * Summary:     RichCarWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        4/15/2017 11:23:43 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RichCarWinController")



-- 界面名称
local wName = "RichCarWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local UnityTools = IMPORT_MODULE("UnityTools")
local protobuf = sluaAux.luaProtobuf.getInstance()
local _platformMgr = IMPORT_MODULE("PlatformMgr");
M.ChatList={}
M.MasterInfo={}
M.StatusInfo={}
M.RoomInfo={}
M.IsOpenWin=false
M.Result={}
M.PoolNum=0
M.History={}
M.UserList={}
M.UserListStatus=0
M.Limit = 0
local function OnCreateCallBack(gameObject)
    protobuf:registerMessageScriptHandler(protoIdSet.sc_player_chat , "OnRecieveRichCarChat")
    UtilTools.SetBgm("Sounds/RichCar/bg")
end


local function OnDestoryCallBack(gameObject)
    protobuf:removeMessageHandler(protoIdSet.sc_player_chat)
    M.UserListStatus=0
    UnityTools.SetBGM(_platformMgr.MainMusic)
    M.ChatList={}
    M.MasterInfo={}
    M.StatusInfo={}
    M.RoomInfo={}
    M.IsOpenWin=false
    protobuf:sendMessage(protoIdSet.cs_car_exit_req,{})
    UnityTools.DestroyWin("BarrageWin")
end
function OnExitRichCar(idMsg,tMsgData)
    UtilTools.HideWaitFlag()
    if tMsgData.result ~= 0 then
        UtilTools.ShowMessage(tMsgData.err,"[FFFFFF]")
    end
end
local function OnResetRender(gameObject)
    triggerScriptEvent(EVENT_RENDER_CHANGE_WIN,{})
end
--进入豪车界面
function OnEnterRichCar(idMsg, tMsgData)
    if tMsgData.result == 0 then  
        UnityTools.CreateLuaWin("RichCarWin")
    else
        UtilTools.HideWaitFlag()
        UtilTools.ShowMessage(tMsgData.err,"[FFFFFF]")
    end
end
function OnRecieveRichCarChat(idMsg,tMsgData)
    if tMsgData.room_type == 3 then
        local data={}
        data.flag = tMsgData.content_type
        data.content = tMsgData.content
        data.name = tMsgData.player_name
        data.vip = tMsgData.player_vip
        if data.flag ~= 4 then
            if #M.ChatList < 20 then
                table.insert(M.ChatList,data)
                --M.ChatList[1+#M.ChatList]=tMsgData
            else
                table.remove(M.ChatList,1)
                table.insert(M.ChatList,data)
            end
        end
        triggerScriptEvent(EVENT_UPDATE_CHAT,"chatAdd",data)
    end
    
end
function OnRichCarMasterUpdate(idMsg,tMsgData)
    M.MasterInfo=tMsgData
    triggerScriptEvent(EVENT_RICHCAR_MASTER_UPDATE,{})
end
--盘面状态
function OnRecieveRichCarStatus(idMsg,tMsgData)
    if M.StatusInfo.status == nil and tMsgData.status == 4 then
        M.StatusInfo.status = 5
        M.StatusInfo.time = tMsgData.time
        return
    end
    if M.StatusInfo.status == nil and tMsgData.status == 3 then
        M.StatusInfo.status = 6
        M.StatusInfo.time = tMsgData.time
        return
    end
    M.StatusInfo.status = tMsgData.status
    M.StatusInfo.time = tMsgData.time
    triggerScriptEvent(EVENT_RICHCAR_STATUS_UPDATE,{})
    
end
--下注
function OnRichCarStakeResponse(idMsg,tMsgData)
    if tMsgData.result == 0 then
        triggerScriptEvent(EVENT_RICHCAR_STAKE_RESPONSE,"call",{tMsgData})
    else
        UtilTools.ShowMessage(tMsgData.err,"[FFFFFF]")
        triggerScriptEvent(EVENT_RICHCAR_STAKE_RESPONSE,"normalerror",tMsgData.list)
    end
end
function OnRichCarContinueStakeResponse(idMsg,tMsgData)
    UtilTools.HideWaitFlag()
    if tMsgData.result== 0 then
        triggerScriptEvent(EVENT_RICHCAR_STAKE_RESPONSE,"call",tMsgData.list)
    else
        UtilTools.ShowMessage(tMsgData.err,"[FFFFFF]")    
        triggerScriptEvent(EVENT_RICHCAR_STAKE_RESPONSE,"error",tMsgData.list)
    end
end
--房间信息
function OnRichCarRoomInfoResponse(idMsg,tMsgData)
    
    M.MasterInfo = tMsgData.masterInfo
    if tMsgData.listSelf==nil then
        M.RoomInfo.selfList={}
    else
        M.RoomInfo.selfList=tMsgData.listSelf
    end
    if tMsgData.list==nil then
        M.RoomInfo.allList={}
    else
        M.RoomInfo.allList= tMsgData.list
    end
    
    M.Result.selfNum = tMsgData.self_num
    M.Result.masterNum = tMsgData.master_num
    M.Result.poolSub = tMsgData.pool_sub
    M.Result.pool = tMsgData.pool
    M.Result.result = tMsgData.result
    M.Limit = tMsgData.bet_limit
    triggerScriptEvent(EVENT_RICHCAR_ROOM_RESPONSE,tMsgData)
end

function OnRichCarResultResponse(idMsg,tMsgData)
    M.Result = tMsgData
    M.Result.poolSub = -M.Result.poolSub
    local tb={}
    tb.open_index=""
    tb.result = tMsgData.result
    if tMsgData.poolSub ~=0 then
        tb.pool = 1
    else
        tb.pool = 0
    end
    if #M.History >=20 then
        table.remove(M.History,1)
    end
    table.insert(M.History,tb)
    triggerScriptEvent(EVENT_RICHCAR_RESULT_RESPONSE,{})
end
local function sortFunc(a,b)
    return tonumber(a.open_index)<tonumber(b.open_index)
end
function OnRichCarPoolResponse(idMsg,tMsgData)
    
    if tMsgData.result~=nil then
        M.PoolNum=tMsgData.result
    end
end
function OnRichCarHistoryResponse(idMsg,tMsgData)
    if tMsgData.list~=nil then
        M.History = tMsgData.list
        table.sort(M.History,sortFunc)
    else
        M.History = {}
    end 
end
function OnRichCarUpResponse(idMsg,tMsgData)
    UtilTools.HideWaitFlag()
    if tMsgData.result == 0 then
        M.MasterInfo.self = tMsgData.flag
        triggerScriptEvent(EVENT_UPDATE_RICH_CAR_STATUS,tMsgData)
    else
        UtilTools.ShowMessage(tMsgData.err,"[FFFFFF]")    
    end
    
end
local function sortUser(a,b)
    if a.vip == b.vip then
        return a.money > b.money
    else
        return a.vip>b.vip
    end
end
function OnRecieveRichCarUserList(idMsg,tMsgData)
    if tMsgData.flag == 1 then
        M.UserList={}
    elseif tMsgData.flag == 3 then
        M.UserListStatus = 0
    elseif tMsgData.flag == 4 then
        M.UserList={}
        M.UserListStatus = 0
    end
    if tMsgData.list~=nil then
        for i,v in pairs(tMsgData.list) do
            table.insert(M.UserList,v)
        end
    end
   
    table.sort(M.UserList,sortUser)
    triggerScriptEvent(EVENT_UPDATE_RICH_CAR_USERLIST,{})
end
local function UpdatePlayerGold()
    triggerScriptEvent(EVENT_RICHCAR_MASTER_UPDATE,{})
end
function OnRichCarHintReply(idMsg,tMsgData)
    UtilTools.ShowMessage(tMsgData.msg,"[FFFFFF]")    
end

-- local function UpdateGold()
--     triggerScriptEvent(EVENT_UPDATE_RICHCAR_GOLD,"gold",{})
-- end
protobuf:registerMessageScriptHandler(protoIdSet.sc_car_enter_reply , "OnEnterRichCar")   --进入
protobuf:registerMessageScriptHandler(protoIdSet.sc_car_exit_reply , "OnExitRichCar")   --退出

protobuf:registerMessageScriptHandler(protoIdSet.sc_car_bet_reply , "OnRichCarStakeResponse")   --下注
protobuf:registerMessageScriptHandler(protoIdSet.sc_car_rebet_reply , "OnRichCarContinueStakeResponse")   --续压返回
   --聊天
protobuf:registerMessageScriptHandler(protoIdSet.sc_car_user_list_reply , "OnRecieveRichCarUserList")   --在线列表返回

protobuf:registerMessageScriptHandler(protoIdSet.sc_car_master_info_reply , "OnRichCarMasterUpdate")   --庄家
protobuf:registerMessageScriptHandler(protoIdSet.sc_car_status_reply , "OnRecieveRichCarStatus")   --盘面状态
protobuf:registerMessageScriptHandler(protoIdSet.sc_car_room_info_reply , "OnRichCarRoomInfoResponse")   --房间信息

protobuf:registerMessageScriptHandler(protoIdSet.sc_car_result_reply , "OnRichCarResultResponse")--开奖
protobuf:registerMessageScriptHandler(protoIdSet.sc_car_pool_reply , "OnRichCarPoolResponse")--彩池
protobuf:registerMessageScriptHandler(protoIdSet.sc_car_result_history_req , "OnRichCarHistoryResponse")--历史
protobuf:registerMessageScriptHandler(protoIdSet.sc_car_master_reply , "OnRichCarUpResponse")--排庄返回
protobuf:registerMessageScriptHandler(protoIdSet.sc_car_hint_reply , "OnRichCarHintReply")--排庄返回

   --房间信息
UI.Controller.UIManager.RegisterLuaWinFunc("RichCarWin", OnCreateCallBack, OnDestoryCallBack)
UI.Controller.UIManager.RegisterLuaFuncCall("RichCarWin:UpdateGold", UpdatePlayerGold)
UI.Controller.UIManager.RegisterLuaWinRenderFunc("RichCarWin", OnResetRender)
-- UI.Controller.UIManager.RegisterLuaFuncCall("ExchangeItemWinController:UpdateTopPanel", UpdateGold)
-- 返回当前模块
return M

