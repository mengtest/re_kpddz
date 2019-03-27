-- -----------------------------------------------------------------


-- *
-- * Filename:    RedBagWinController.lua
-- * Summary:     红包广场
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/20/2017 4:26:41 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RedBagWinController")
local _platformMgr = IMPORT_MODULE("PlatformMgr");
local UnityTools = IMPORT_MODULE("UnityTools");
local protobuf = sluaAux.luaProtobuf.getInstance();



-- 界面名称
local wName = "RedBagWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local _redBagSearchList = {}
local _redBagList = {} --红包显示列表
local _noticeList = {} --通知列表
local _mineRedList = {} --自己的红包
local _wait = false;
local _maxNum = 9;
local _noticeRed = { redNum = 0 }

function RedBagWinControllerClear(msgId, value)
    _redBagList = {} --红包显示列表
    _noticeList = {} --通知列表
    _mineRedList = {} --自己的红包
    _wait = false;
    _noticeRed.redNum = 0 --通知红点数
    _maxNum = 9;
end

registerScriptEvent(EXIT_CLEAR_ALL_DATA, "RedBagWinControllerClear")


--- desc:红包红点数
-- YQ.Qu:2017/3/22 0022
-- @param
-- @return
local function RedBagHint()
    local num = _maxNum - #_mineRedList;
    if num < 0 then
        return 0;
    end
    return num;
end

---用于主界面的红显示
local function RedBagMainBtnHint()
    return RedBagHint()+_noticeRed.redNum
 end

--- desc:
-- YQ.Qu:2017/3/20 0020
-- @param
-- @return
local function GetRedBagList()
    return _redBagList;
end

--- desc:获取通知列表
-- YQ.Qu:2017/3/22 0022
-- @param
-- @return
local function GetNoticeList()
    return _noticeList
end


--- desc:请求红包列表数据
-- YQ.Qu:2017/3/20 0020
-- @param startId
-- @return
local function GetRedBagListFromServer(startId)
    --没有更多数据
    if _maxNum < startId and _maxNum >0  then return; end
    --已经在请求新的数据
    if _wait then return; end

    local req = {}
    _wait = true;
    req.begin_id = startId;
    req.end_id = startId + 8;
    --    LogWarn("[RedBagWinController.GetRedBagListFromServer]startId = "..startId.." endId = "..req.end_id);
    -- LogError("sendmsg")
    protobuf:sendMessage(protoIdSet.cs_red_pack_query_list_req, req);
end

local function OnCreateCallBack(gameObject)
    _wait = false;
    _redBagList = {}
    for i = 1, #_mineRedList do
        _redBagList[i] = _mineRedList[i];
    end
    GetRedBagListFromServer(1);
end



local function OnDestoryCallBack(gameObject)
end


--- desc:检测红包是否已经加到列表里了
-- YQ.Qu:2017/3/20 0020
-- @param value
-- @return
local function CheckRedList(value)
    if #_redBagList == 0 then
        return true;
    end
    for i = 1, #_redBagList do
        if _redBagList[i].uid == value.uid or value.uid == _platformMgr.PlayerUuid() then
            --            LogWarn("[RedBagWinController.CheckRedList]"..value.player_name.."   value over tiem = "..value.over_time);
            return false;
        end
    end

    return true;
end

--- desc:检测自己的红包是否已经在列表里
-- YQ.Qu:2017/3/21 0021
-- @param list
-- @param value
-- @return
local function checkSelfRedList(list, value)
    local index = 0;
    for i = 1, #list do
        if list[i].uid == value.uid then
            return false, i;
        end

        if list[i].player_id == _platformMgr.PlayerUuid() then
            index = i;
        end
    end

    return true, index + 1;
end

--- desc:查询红包数据返回
--- YQ.Qu
function OnRedPackQureyListReply(msgId, tMsgData)

    _wait = false;
    if tMsgData == nil then
        return;
    end

    _maxNum = tMsgData.max_num;
    if tMsgData.list == nil then
        return;
    end
    --    LogError("[RedBagWinController.OnRedPackQureyListReply]\n ------>"..#tMsgData.list .."\n"..PrintTableStr(tMsgData.list));
    for i = 1, #tMsgData.list do
        --        LogError("[RedBagWinController.OnRedPackQureyListReply]");
        if CheckRedList(tMsgData.list[i]) then
            _redBagList[#_redBagList + 1] = tMsgData.list[i];
        end
    end
    table.sort(_redBagList, function(a, b)
        return a.over_time < b.over_time;
    end)
    --    LogError("[RedBagWinController.OnRedPackQureyListReply]------------->" .. _maxNum);
    triggerScriptEvent(UPDATE_MAIN_WIN_RED, "redBag");
    triggerScriptEvent(RED_BAG_UPDATE, "redBag", false);
end

--- desc:信息是否已经在通知列表里了
-- YQ.Qu:2017/3/21 0021
-- @param info
-- @return bool,int   是否存在，位置
local function checkNoticeList(info)
    for i = 1, #_noticeList do
        if info.uid == _noticeList[i].uid and (info.notice_id ~= nil and info.notice_id ~= "" and _noticeList[i].notice_id == info.notice_id) then
            return false, i;
        end
    end
    return true, #_noticeList + 1;
end

--- 通知列表排序
local function OnNoticeListSort(a, b)
    if a.type < b.type then
        return true;
    elseif a.type > b.type then
        return false;
    else
        return a.get_sec_time > b.get_sec_time;
    end
end

local function DelOneNotice(id)
    --    LogWarn("[RedBagWinController.DelOneNotice]----->>>>>> \n" .. tostring(id));
    for i = 1, #_noticeList do
        --        LogWarn("[RedBagWinController.DelOneNotice]i  ===== " .. _noticeList[i].notice_id);
        if _noticeList[i].notice_id == id then
            --            LogWarn("[RedBagWinController.DelOneNotice]要删除I ==== " .. i);
            table.remove(_noticeList, i)
            return
        end
    end
end

local function DelNoticeList(str)
    --    LogWarn("[RedBagWinController.DelList]要删除的通知列表" .. str)
    local delList = str --stringToTable(str, ",")
    LogWarn("[RedBagWinController.DelList]要删除的通知列表" .. "   delList.len = " .. #delList);
    if #delList > 0 then
        for i = 1, #delList do
            DelOneNotice(delList[i])
        end
    end
end

function _noticeRed:RedNumCount()
    self.redNum = 0
    for i = 1, #_noticeList do
        if _noticeList[i].notice_type == 2 then
            self.redNum = self.redNum + 1
        end
    end
end

--- desc:返回通知列表
--- YQ.Qu
function OnRedPackNoticUpdate(msgId, tMsgData)

    if tMsgData == nil then
        return;
    end
    if tMsgData.list == nil or #tMsgData.list == 0 then

    else
        if #_noticeList == 0 then
            _noticeList = tMsgData.list;
        else
            for i = 1, #tMsgData.list do
                local isIn, index = checkNoticeList(tMsgData.list[i]);
                --                LogWarn("[RedBagWinController.OnRedPackNoticUpdate]isInt = " .. tostring(isIn) .. "   index == " .. tostring(index) .. " len = " .. #_noticeList);
                if isIn then
                    table.insert(_noticeList, tMsgData.list[i])
                else
                    _noticeList[index] = tMsgData.list[i];
                end
            end
        end

        
    end
    if tMsgData.delete_notice_list ~= nil and tMsgData.delete_notice_list ~= "" then
        DelNoticeList(tMsgData.delete_notice_list)
    end
    _noticeRed:RedNumCount()
    --排序
    if #_noticeList > 1 then
        table.sort(_noticeList, OnNoticeListSort);
    end
    triggerScriptEvent(UPDATE_MAIN_WIN_RED, "redBag");
    triggerScriptEvent(RED_BAG_UPDATE, "notice", false);
end

--- desc:返回自己的红包列表
--- YQ.Qu
function OnSelfRedPackInfo(msgId, tMsgData)

    if tMsgData == nil then
        return;
    end
    _maxNum = tMsgData.all_red_pack_num;
    if #_mineRedList == 0 then
        _mineRedList = tMsgData.red_pack_list or {};
    end


    if tMsgData.red_pack_list ~= nil then
        if #_redBagList == 0 then
            --        _redBagList = _mineRedList;
            for i = 1, #_mineRedList do
                _redBagList[i] = _mineRedList[i];
            end
        else
            for i = 1, #tMsgData.red_pack_list do
                local isNoIn, index = checkSelfRedList(_redBagList, tMsgData.red_pack_list[i]);
                if isNoIn then
                    table.insert(_redBagList, index, tMsgData.red_pack_list[i]);
                else
                    _redBagList[index] = tMsgData.red_pack_list[i];
                end

                local isInSelfList, k = checkSelfRedList(_mineRedList, tMsgData.red_pack_list[i]);
                _mineRedList[k] = tMsgData.red_pack_list[i];
            end
        end
    end

    --    LogWarn("[RedBagWinController.OnSelfRedPackInfo]自己红包  长度="..#_mineRedList.."  红包长度 = "..#_redBagList);

    triggerScriptEvent(RED_BAG_UPDATE, "redBag", false);
    triggerScriptEvent(UPDATE_MAIN_WIN_RED, "redBag");
end

local function DelOneRedBagList(uid)

    --- 删除红包列表里的
    for i = 1, #_redBagList do
        if _redBagList[i].uid == uid then
            table.remove(_redBagList, i);
            _maxNum = _maxNum - 1;
            break;
        end
    end
end

local function DelOneRag(uid)
    if uid == nil or uid == "" then
        return false
    end
    DelOneRedBagList(uid);
    --- 删除自己的红包列表数据
    for i = 1, #_mineRedList do

        if _mineRedList[i].uid == uid then
            table.remove(_mineRedList, i);
            return true
        end
    end
end

local function SetCancelNotice(noticeId, uid)
    DelOneRag(uid)

    for i = 1, #_noticeList do
        if _noticeList[i].uid == uid and _noticeList.notice_type == 1 then
            _noticeList[i].type = 3
        end
    end
end

--- desc:玩家自己取消红包
--- YQ.Qu
function OnRedPackCancelReply(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    if tMsgData.result == 0 then
        SetCancelNotice(tMsgData.notice_id, tMsgData.uid)
        table.sort(_noticeList, OnNoticeListSort)
        triggerScriptEvent(RED_BAG_UPDATE, "notice", false);
    else
        UnityTools.ShowMessage(tMsgData.err);
    end
end

--- desc:猜红包返回
--- YQ.Qu
function OnRedPackOpenReply(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    if tMsgData.result == 0 then
        --[[local rewards = {}
        rewards[1] = {}
        rewards[1].base_id = 101;
        rewards[1].count = tMsgData.reward_num;
        ShowAwardWin(rewards);]]
        UnityTools.ShowMessage(LuaText.redBagGuessSuccWait)
        UnityTools.DestroyWin("RedBagGuessWin");
--        DelOneRedBagList(tMsgData.uid);
        triggerScriptEvent(RED_BAG_UPDATE, "redBag", true);
        triggerScriptEvent(UPDATE_MAIN_WIN_RED, "redBag");

    else
        triggerScriptEvent(RED_BAG_GUESS_UPDATE, false)
        UnityTools.ShowMessage(tMsgData.err);
    end
end

local function RedPackDoSelectReq(noticeId, opt)
    local req = {}
    req.notice_id = noticeId
    req.opt = opt
    protobuf:sendMessage(protoIdSet.cs_red_pack_do_select_req, req)
end


--- desc:红包确认请求返回
--- YQ.Qu
function RedPackDoSelectReply(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    if tMsgData.result == 0 then
        if tMsgData.opt == 0 then
            if DelOneRag(tMsgData.redpack_id) then
                triggerScriptEvent(RED_BAG_UPDATE, "redBag", false);
            end
        end
    else
        UnityTools.ShowMessage(tMsgData.err_msg)
    end
end

function OnRedPackSearchReply(msgId,tMsgData)
    if tMsgData.list == nil then
        _redBagSearchList={}
    else
        _redBagSearchList=tMsgData.list
    end
    triggerScriptEvent(RED_BAG_UPDATE, "search", true);
end
local function GetRedBagSearchList()
    return _redBagSearchList
end

UI.Controller.UIManager.RegisterLuaWinFunc("RedBagWin", OnCreateCallBack, OnDestoryCallBack)
protobuf:registerMessageScriptHandler(protoIdSet.sc_red_pack_query_list_reply, "OnRedPackQureyListReply")
protobuf:registerMessageScriptHandler(protoIdSet.sc_red_pack_notice_update, "OnRedPackNoticUpdate")
protobuf:registerMessageScriptHandler(protoIdSet.sc_self_red_pack_info, "OnSelfRedPackInfo")
protobuf:registerMessageScriptHandler(protoIdSet.sc_red_pack_cancel_reply, "OnRedPackCancelReply")
protobuf:registerMessageScriptHandler(protoIdSet.sc_red_pack_open_reply, "OnRedPackOpenReply")
protobuf:registerMessageScriptHandler(protoIdSet.sc_red_pack_do_select_reply, "RedPackDoSelectReply")
protobuf:registerMessageScriptHandler(protoIdSet.sc_red_pack_search_reply, "OnRedPackSearchReply")

M.GetRedBagSearchList = GetRedBagSearchList;
M.GetRedBagList = GetRedBagList;
M.GetRedBagListFromServer = GetRedBagListFromServer;
M.GetNoticeList = GetNoticeList;
M.RedBagHint = RedBagHint;
M.RedPackDoSelectReq = RedPackDoSelectReq
M.NoticeRed = _noticeRed
M.RedBagMainBtnHint = RedBagMainBtnHint
-- 返回当前模块
return M
