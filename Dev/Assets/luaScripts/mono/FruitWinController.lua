-- -----------------------------------------------------------------


-- *
-- * Filename:    FruitWinController.lua
-- * Summary:     FruitWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        3/17/2017 10:18:04 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FruitWinController")



-- 界面名称
local wName = "FruitWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local UnityTools = IMPORT_MODULE("UnityTools")
local platformMgr = IMPORT_MODULE("PlatformMgr")
local roomMgr = IMPORT_MODULE("roomMgr")
local protobuf = sluaAux.luaProtobuf.getInstance()
M.BaseInfo={}
M.BaseInfo.lineNum = 1
M.BaseInfo.lineSetChips = 1
M.BaseInfo.FreeTimes = 0
M.BaseInfo.FruitList = {}
M.BaseInfo.PoolNum=0
M.AwardInfo={}
M.AwardInfo.RewardNum=0
M.AwardInfo.FruitList = {}
M.AwardInfo.RewardList = {}
M.AwardInfo.bPool=false
M.AwardInfo.bFree=0
M.PoolInfo=nil
M.TaskTable={}
M.BoxTable={}
M.BoxTable.boxStart = 0
M.RedTable={}
M.CanGet = false
M.TotalGold= 0
M.CowTable ={}
M.DuiJuBox = {}
M.ToRoomType = 0
M.IsOpenSuper = 0
M.isSuperShow = false
M.StartTime = 0
M.EndTime = 0
M.isJumpToSuper = false 
local function OnCreateCallBack(gameObject)
    UtilTools.SetBgm("Sounds/Laba/lababgm")
    roomMgr.bExiting = false
    
end

---desc:
--YQ.Qu:2017/4/13 0013
-- @param
-- @return
local function ResetWinRenderQ(go)
    triggerScriptEvent(EVENT_RENDER_CHANGE_WIN,wName)
end


local function OnDestoryCallBack(gameObject)
    UnityTools.SetBGM(platformMgr.MainMusic)
    protobuf:removeMessageHandler(protoIdSet.sc_niu_room_chest_info_update,"FruitRecvGameTimesReply")
    -- if GameDataMgr.LOGIN_DATA.IsConnectGamerServer then
    --     protobuf:sendMessage(protoIdSet.cs_task_pay_info_request, {})
    -- end
    M.isJumpToSuper = false
    M.ToRoomType = 0
end
function OnReceiveLabaBaseInfo(idMsg, tMsgData)
    if tMsgData.type == 2 then
        M.isSuperShow = true
    else
        M.isSuperShow = false
    end
    
    M.BaseInfo.lineNum = tMsgData.line_num
    M.BaseInfo.lineSetChips = tMsgData.line_set_chips
    M.BaseInfo.FreeTimes = tMsgData.last_free_times
    LogError("tMsgData.start_time="..tMsgData.start_time)
    M.StartTime = tMsgData.start_time
    M.EndTime = tMsgData.end_time
    if tMsgData.fruit_list ~=nil then 
        M.BaseInfo.FruitList = tMsgData.fruit_list
        table.sort(M.BaseInfo.FruitList,function(a,b) return a.pos_id<b.pos_id end)
    end
    
    if UtilTools.GetServerTime() >= M.StartTime and UtilTools.GetServerTime()<=M.EndTime then
        M.IsOpenSuper = M.EndTime
    else
        M.IsOpenSuper = 0
    end 
    triggerScriptEvent(EVENT_LABA_MODE_SWTICH,{})
    
    UnityTools.CreateLuaWin(wName)
end
function OnLabaLeaveRoom(idMsg, tMsgData)
    if M.ToRoomType ~= 0 then
        protobuf:sendMessage(protoIdSet.cs_laba_enter_room_req,{type = M.ToRoomType})
        M.ToRoomType=0
    end
end
-- M.test=true
function OnLabaSpinResponse(idMsg, tMsgData)
    M.AwardInfo.RewardNum = tonumber(tMsgData.total_reward_num)
    if tMsgData.fruit_list ~=nil then
        M.AwardInfo.FruitList = tMsgData.fruit_list
        table.sort(M.AwardInfo.FruitList,function(a,b) return a.pos_id<b.pos_id end)
        M.BaseInfo.FruitList=M.AwardInfo.FruitList
    end
    M.AwardInfo.RewardList={}
    if tMsgData.reward_list ~=nil then
        M.AwardInfo.RewardList = tMsgData.reward_list
    end
    M.BaseInfo.FreeTimes = tMsgData.new_last_free_times
    if tMsgData.pool ~= nil and tMsgData.pool == 1 then
        M.AwardInfo.bPool=true
    else
        M.AwardInfo.bPool=false
    end 
    if tMsgData.free ~= nil and tMsgData.free >= 1 and tMsgData.free <=3 then
        M.AwardInfo.bFree=tMsgData.free
    else
        M.AwardInfo.bFree=0
    end 
    UtilTools.HideWaitFlag()
    triggerScriptEvent(EVENT_LABA_SPIN_REPLY,{})
end
function OnLabaPoolUpdate(idMsg, tMsgData)
    if tMsgData.total_pool_num ~=nil and  M.BaseInfo.PoolNum~=tonumber(tMsgData.total_pool_num) then
        M.BaseInfo.PoolNum = tonumber(tMsgData.total_pool_num)
        triggerScriptEvent(EVENT_LABA_POOL_UPDATE,{})
    end
    if tMsgData.win_player ~=nil then
        M.PoolInfo = tMsgData.win_player
    end
end
function OnGetMissionAwardReply(idMsg,tMsgData)
    UtilTools.HideWaitFlag()
    if tMsgData.result == 0 then
        ShowAwardWin(tMsgData.reward)
        triggerScriptEvent(EVENT_LABA_GOLD_UPDATE,"labagold",{})
    else
        UtilTools.ShowMessage(tMsgData.err,"[FFFFFF]");
    end
end
function OnGetBoxAwardReply(idMsg,tMsgData)
    if tMsgData.result == 0 then
        ShowAwardWin(tMsgData.reward)
        triggerScriptEvent(EVENT_LABA_GOLD_UPDATE,"labagold",{})
    else
        UtilTools.ShowMessage(tMsgData.err,"[FFFFFF]");
    end
end
function OnLabaTaskUpdate(idMsg,tMsgData)
    if tMsgData.tast_info == nil then
        M.TaskTable = {}
        return
    end
    for i=1,#tMsgData.tast_info do
        if tMsgData.tast_info[i].tast_type == 2 then
            M.TaskTable.taskId = tMsgData.tast_info[i].taskId
            M.TaskTable.process = tMsgData.tast_info[i].process
            M.TaskTable.status = tMsgData.tast_info[i].status
            M.TaskTable.remaindTime = tMsgData.tast_info[i].remaindTime
            M.BoxTable.boxStatus = tMsgData.tast_info[i].boxStatus
            M.BoxTable.boxProcess = tMsgData.tast_info[i].boxProcess
            M.BoxTable.vip_level = tMsgData.tast_info[i].vip_level
            if  M.BoxTable.boxStart ==0 and tMsgData.tast_info[i].boxStart == 1 then
                M.BoxTable.boxStart = tMsgData.tast_info[i].boxStart
                --出现特效
                triggerScriptEvent(EVENT_LABA_GOLD_UPDATE,"labatask",{isShow=true})
            else
                M.BoxTable.boxStart = tMsgData.tast_info[i].boxStart
                triggerScriptEvent(EVENT_LABA_GOLD_UPDATE,"labatask",{isShow=false})
            end
        elseif tMsgData.tast_info[i].tast_type == 1 then
            local isFresh=false
            if M.CowTable.taskId == nil or M.CowTable.taskId ~=tMsgData.tast_info[i].taskId then
                isFresh =true
            end
            
            M.CowTable.taskId = tMsgData.tast_info[i].taskId
            M.CowTable.process = tMsgData.tast_info[i].process
            M.CowTable.status = tMsgData.tast_info[i].status
            M.CowTable.remaindTime = tMsgData.tast_info[i].remaindTime
            if isFresh or M.CowTable.status==2 then
                triggerScriptEvent(EVENT_TASK_INFO_UPDATE,{})
            end
        end
        
    end
end
function OnFruitBoxInfoUpdate(idMsg,tMsgData)
    -- LogError("tMsgData.game_type="..tMsgData.game_type)
    if tMsgData.game_type == 2 then
        M.BoxTable.boxStatus = tMsgData.box_flag_list
        triggerScriptEvent(EVENT_LABA_GOLD_UPDATE,"labatask",{})
    end
end
local function sortFunc(a,b)
    if a.status == 2 and b.status ~=2 then 
        return true
    elseif a.status ~=2 and b.status == 2 then
        return false
    else
        if a.status == b.status then
            return a.index < b.index
        end
        return a.status < b.status 
    end 
end
function OnGetRedTaskUpdate(idMsg,tMsgData)
    UtilTools.HideWaitFlag()
    if tMsgData.result == 0 and tMsgData.game_type == 2 then
        ShowAwardWin(tMsgData.reward)
        for i=1,#M.RedTable do
            if M.RedTable[i].index == tMsgData.task_id then
                M.RedTable[i].status = 1
            end
        end
        triggerScriptEvent(EVENT_RED_WIN_UPDATE,{})
    else    
       UtilTools.ShowMessage(tMsgData.err,"[FFFFFF]");     
    end
end
function OnGetRedTask(idMsg,tMsgData)
    for i=1,#tMsgData.list do
        if tMsgData.list[i].game_type == 2 then
            if tMsgData.upd_type == 1 then
                M.RedTable={}
                local index=1
                M.CanGet = false
                M.TotalGold=tMsgData.list[i].gold_num 
                for k,v in pairs(LuaConfigMgr.RedFruitConfig) do
                    M.RedTable[index]= v
                    M.RedTable[index].index=tonumber(v.key)
                    
                    if tMsgData.list[i].gold_num >= tonumber(v.total_gold) then
                        M.RedTable[index].status = 2
                        if tMsgData.list[i].draw_list~=nil then
                            for j=1,#tMsgData.list[i].draw_list do
                                if tMsgData.list[i].draw_list[j] == tonumber(v.key) then
                                    M.RedTable[index].status = 1
                                    break
                                end
                            end
                        end
                        if M.RedTable[index].status == 2 then
                            M.CanGet=true
                        end
                    else
                        M.RedTable[index].status = 0
                    end
                    --0未领取 1已领取 2可领取
                    index=index+1
                end
                table.sort(M.RedTable,sortFunc)
                triggerScriptEvent(EVENT_RED_WIN_UPDATE,{})
            elseif tMsgData.upd_type == 2 then
                M.TotalGold=tMsgData.list[i].gold_num 
                for k=1,#M.RedTable do
                    if M.RedTable[k].status ~=1 then
                        if M.TotalGold >= tonumber(M.RedTable[k].total_gold) then
                            M.CanGet=false
                            M.RedTable[k].status = 2
                            if tMsgData.list[i].draw_list~=nil then
                                for j=1,#tMsgData.list[i].draw_list do
                                    if tMsgData.list[i].draw_list[j] == tonumber(v.key) then
                                        M.RedTable[k].status = 1
                                        break
                                    end
                                end
                            end
                            if M.RedTable[k].status == 2 then
                                M.CanGet=true
                            end
                        end
                    end
                end
                triggerScriptEvent(EVENT_RED_WIN_UPDATE,{})
            end
            break
        end
    end
end
function FruitRecvGameTimesReply(msgID, result)
    M.DuiJuBox= result
    triggerScriptEvent(EVENT_LABA_GOLD_UPDATE,"labamission",{})
end
local function InitLis()
    protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_room_chest_info_update,"FruitRecvGameTimesReply")
end
protobuf:registerMessageScriptHandler(protoIdSet.sc_laba_enter_room_reply , "OnReceiveLabaBaseInfo")   --23451 SC_CLabaEnterResponse
protobuf:registerMessageScriptHandler(protoIdSet.sc_laba_leave_room_reply , "OnLabaLeaveRoom")
protobuf:registerMessageScriptHandler(protoIdSet.sc_laba_spin_reply , "OnLabaSpinResponse")
protobuf:registerMessageScriptHandler(protoIdSet.sc_laba_pool_num_update , "OnLabaPoolUpdate")
protobuf:registerMessageScriptHandler(protoIdSet.sc_game_task_draw_reply , "OnGetMissionAwardReply")
protobuf:registerMessageScriptHandler(protoIdSet.sc_game_task_info_update , "OnLabaTaskUpdate")
protobuf:registerMessageScriptHandler(protoIdSet.sc_game_task_box_draw_reply , "OnGetBoxAwardReply")
protobuf:registerMessageScriptHandler(protoIdSet.sc_redpack_task_draw_list_update , "OnGetRedTask")
protobuf:registerMessageScriptHandler(protoIdSet.sc_redpack_task_draw_reply , "OnGetRedTaskUpdate")
protobuf:registerMessageScriptHandler(protoIdSet.sc_game_task_box_info_update , "OnFruitBoxInfoUpdate")



UI.Controller.UIManager.RegisterLuaWinFunc("FruitWin", OnCreateCallBack, OnDestoryCallBack)
UI.Controller.UIManager.RegisterLuaWinRenderFunc("FruitWin", ResetWinRenderQ)
M.InitLis = InitLis

-- 返回当前模块
return M
 