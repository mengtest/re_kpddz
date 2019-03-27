-- -----------------------------------------------------------------


-- *
-- * Filename:    MainWinController.lua
-- * Summary:     主界面功能Win
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/18/2017 9:46:28 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("MainWinController")
local protobuf = sluaAux.luaProtobuf.getInstance();
local _platformMgr = IMPORT_MODULE("PlatformMgr")
local UnityTools = IMPORT_MODULE("UnityTools")
local _listData = {};
local _myRank = {}
local _go
local activeFlag = 0;--1，隐藏 2.显示
M.NoticeList={}
-- 界面名称
local wName = "MainWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
M.SkipId = 0
local function OnCreateCallBack(gameObject)
    _go = gameObject
    if activeFlag == 1 then
        UnityTools.SetActive(_go,false)
    end

    -- 发送活动查询信息
    local aaaCtrl = IMPORT_MODULE("ActivityAndAnnouncementController")
    aaaCtrl.ActivitiesManager:queryAllActivityInfo()
end

local function DestroyWin(winName)
    if UI.Controller.UIManager.IsWinShow(winName) then
        UnityTools.DestroyWin(winName);
    end
end
local function OnDestoryCallBack(gameObject)
    _go = nil
    activeFlag = 0
    DestroyWin("RoomLvSelectWin")
    DestroyWin("GameCenterWin");
end

local function IsWinShow(winName)
    return UI.Controller.UIManager.IsWinShow(winName);
end

local function Hide()
    if _go ~= nil then
        UnityTools.SetActive(_go, false)
        activeFlag = 0
    else
        activeFlag = 1
    end
end

local function Show()
    if _go ~= nil then
        UnityTools.SetActive(_go, true)
        activeFlag = 0
    else
        activeFlag = 2
    end
end

function OnRankQureyReply(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    local rankType = tMsgData.rank_type;
    _myRank[rankType] = tMsgData.my_rank;
    _listData[rankType] = {}
    if rankType == 6 then
        local _goldPoolCtrl = IMPORT_MODULE("FruitRankWinController")
        _goldPoolCtrl.my_recharge_money = tMsgData.my_recharge_money
    end
    -- if rankType == 6 then
    
    --     local _goldPoolCtrl = IMPORT_MODULE("GoldPoolWinController")
    --     _goldPoolCtrl.ActInfo = {}
    --     _goldPoolCtrl.ActInfo.start_time = tMsgData.start_time
    --     _goldPoolCtrl.ActInfo.end_time = tMsgData.end_time 
    --     _goldPoolCtrl.ActInfo.pool = tMsgData.pool
    --     _goldPoolCtrl.ActInfo.my_recharge_money = tMsgData.my_recharge_money
        
    --     local t = os.date("*t", _goldPoolCtrl.ActInfo.start_time)
    --     _goldPoolCtrl.ActInfo.startStr=t.month.."."..t.day
    --     t = os.date("*t", _goldPoolCtrl.ActInfo.end_time)
    --     _goldPoolCtrl.ActInfo.endStr=t.month.."."..t.day
        
    --     _goldPoolCtrl.ActInfo.start_time = math.mod(_goldPoolCtrl.ActInfo.start_time,86400) * 86400
    --     _goldPoolCtrl.ActInfo.end_time = math.mod(_goldPoolCtrl.ActInfo.start_time,86400) * 86400 
    -- end
    if tMsgData.rank_info_list ~= nil then
        for i = 1, #tMsgData.rank_info_list do
            _listData[rankType][tMsgData.rank_info_list[i].rank] = tMsgData.rank_info_list[i];
        end
    end
    triggerScriptEvent(EVENT_UPDATE_RANK_INFO,{})
    triggerScriptEvent(EVENT_UPDATE_FRUIT_SUPER_RANK_INFO,{})
    if IsWinShow("MainWin") then
        local mono = IMPORT_MODULE("MainWinMono");
        if mono ~= nil then
            mono.UpdateRankList(rankType);
        end
    end
end
local function ResetWinRenderQ(go)
    triggerScriptEvent(EVENT_RENDER_CHANGE_WIN,wName)
end
function OnGetServerNotice(msgId, tMsgData)
    if tMsgData == nil then return end
    if UnityTools.IsWinShow(wName) then
        if tMsgData.flag == 1 then
            M.NoticeList[#M.NoticeList+1] = tMsgData.content
        end
        triggerScriptEvent(EVENT_ADD_NOTICE,{})
    end
end
local function closeFunc()
    triggerScriptEvent(EVENT_RECONNECT_SOCKET,{})
end
UI.Controller.UIManager.RegisterLuaWinFunc("MainWin", OnCreateCallBack, OnDestoryCallBack)
protobuf:registerMessageScriptHandler(protoIdSet.sc_rank_qurey_reply, "OnRankQureyReply")
protobuf:registerMessageScriptHandler(protoIdSet.sc_player_sys_notice, "OnGetServerNotice")
UI.Controller.UIManager.RegisterLuaFuncCall("OnApplicationFocus", closeFunc)

UI.Controller.UIManager.RegisterLuaWinRenderFunc("MainWin", ResetWinRenderQ)
M.RankList = _listData
M.MyRank = _myRank
M.Hide = Hide
M.Show = Show
-- 返回当前模块
return M
