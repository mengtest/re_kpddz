-- -----------------------------------------------------------------


-- *
-- * Filename:    PlayerInfoWinController.lua
-- * Summary:     玩家信息界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/22/2017 10:14:17 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("PlayerInfoWinController")
local protobuf = sluaAux.luaProtobuf.getInstance()
local _platformMgr = IMPORT_MODULE("PlatformMgr")
local _itemMgr = IMPORT_MODULE("ItemMgr")
local _unityTools = IMPORT_MODULE("UnityTools")

--变量
local _newName = ""
local _newSex = -1;


-- 界面名称
local wName = "PlayerInfoWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

M.IsRealName = false

local function OnCreateCallBack(gameObject)
    local req = {}
    --    req.obj_player_uuid = "";
    protobuf:sendMessage(protoIdSet.cs_query_player_winning_rec_req, req);
    --        LogError("[PlayerInfoWinController.OnCreateCallBack]");
end

--desc:修改玩家的名字及性别（Send）
--YQ.Qu:2017/2/23 0023
local function ChangeNameAndSex(newName, newSex)
    --    LogWarn("[PlayerInfoWinController.ChangeNameAndSex]................");
    _newName = newName;
    _newSex = newSex;
    if newName ~= _platformMgr.UserName() then
        local passNewName = GameText.Instance:StrFilter(newName, 42)
        if passNewName ~= newName and string.find(passNewName, "*") >= 1 then
            _unityTools.ShowMessage(LuaText.GetString("StrFilter_failed"));
            return;
        end
        if passNewName ~= newName then
            newName = passNewName;
            _newName = passNewName;
        end

        LogWarn("[PlayerInfoWinController.ChangeNameAndSex]" .. newName .. "          passsWardName ==== " .. passNewName);
        local req = {}
        req.name = newName
        LogWarn("[PlayerInfoWinController.ChangeNameAndSex]protoIdSet.cs_player_change_name_req");
        protobuf:sendMessage(protoIdSet.cs_player_change_name_req, req);
    elseif newSex ~= _platformMgr.getSex() then
        local req = {}
        req.sex = _newSex;
        req.icon = _platformMgr.Icon();
        LogWarn("[PlayerInfoWinController.ChangeNameAndSex]protoIdSet.cs_player_change_headicon_req");
        protobuf:sendMessage(protoIdSet.cs_player_change_headicon_req, req);
    else
        triggerScriptEvent(END_CHANGE_NAME_AND_SEX, {})
    end
end

local function ChangePlayerHead()
    local req = {}
    local icon = _platformMgr.GetIcon();
    _newSex = _platformMgr.getSex();
    req.sex = _newSex;
    req.icon = icon;
    LogWarn("[PlayerInfoWinController.ChangePlayerHead]" .. icon);
    protobuf:sendMessage(protoIdSet.cs_player_change_headicon_req, req);
end

--desc:改名成功后提交性别修改
function OnPlayerChangeNameReply(msgId, tMsgData)
    LogWarn("[PlayerInfoWinController.OnPlayerChangeNameReply]==tMsgData.result" .. tMsgData.result);
    if tMsgData.result == 0 then
        LogWarn("[PlayerInfoWinController.OnPlayerChangeNameReply]名字修改成功");
        _platformMgr.SetUserName(_newName)
        --更新玩家信息
        triggerScriptEvent(EVENT_RESCOURCE_UDPATE, 1)
        if _platformMgr.getSex() == _newSex then
            LogWarn("[PlayerInfoWinController.OnPlayerChangeNameReply]名字修改成功，性别不变");
            triggerScriptEvent(END_CHANGE_NAME_AND_SEX, true)
            _newSex = -1;
        else
            LogWarn("[PlayerInfoWinController.OnPlayerChangeNameReply]名字修改成功，性别也改变");
            local req = {}
            req.sex = _newSex;
            protobuf:sendMessage(protoIdSet.cs_player_change_headicon_req, req);
        end
    else
        _newSex = -1;
        LogWarn("[PlayerInfoWinController.OnPlayerChangeNameReply]名字修改失败");
        --        UtilTools.ShowMessage(LuaText.GetString("changeNameFailed", "[FFFFFF]"));
        _unityTools.ShowMessage(LuaText.GetString("changeNameFailed"..tMsgData.result));
        triggerScriptEvent(END_CHANGE_NAME_AND_SEX, false)
    end
end

--desc:
--YQ.Qu:2017/2/23 0023
function OnPlayerChangeHeadIconReply(msgId, tMgsData)
    if tMgsData.result == 0 then
        if _newSex ~= -1 then
            _platformMgr.SetSex(_newSex);
            triggerScriptEvent(END_CHANGE_NAME_AND_SEX, true)
        end
        triggerScriptEvent(END_CHANGE_PLAYER_HEAD, {})
        
    else

        if _newSex ~= -1 then
            triggerScriptEvent(END_CHANGE_NAME_AND_SEX, false)
        end
        UtilTools.ShowMessage(LuaText.GetString("changeSexFailed"), "[FFFFFF]");
    end

    _newSex = -1;
end

--desc:物品更新
--YQ.Qu:2017/2/24 0024
function OnItemInitUpdate(msgId, tMsgData)
    if tMsgData ~= nil then
        _itemMgr.ItemInit(tMsgData);
        triggerScriptEvent(UPDATE_ITEM, "init")
    end
end

--desc:物品删除
--YQ.Qu:2017/2/24 0024
function OnItemDel(msgId, tMsgData)
    if tMsgData ~= nil then
        _itemMgr.ItemDel(tMsgData);
        triggerScriptEvent(UPDATE_ITEM, "del")
    end
end

--desc:物品添加
--YQ.Qu:2017/2/24 0024
function ItemAdd(msgId, tMsgData)
    if tMsgData ~= nil then
        _itemMgr.ItemAdd(tMsgData);
        triggerScriptEvent(UPDATE_ITEM, "add")
    end
end

--desc:物品更新
--YQ.Qu:2017/2/24 0024
function ItemUpdate(msgId, tMsgData)
    if tMsgData ~= nil then
        _itemMgr.ItemUpdate(tMsgData);
        triggerScriptEvent(UPDATE_ITEM, "update")
    end
end

function OnRealNameStatusRes(msgId,tMgsData)
    M.IsRealName = tMgsData.type == 1
    triggerScriptEvent(EVENT_RESCOURCE_UDPATE, {})
    
end

local function OnDestoryCallBack(gameObject)
end


UI.Controller.UIManager.RegisterLuaWinFunc("PlayerInfoWin", OnCreateCallBack, OnDestoryCallBack)
protobuf:registerMessageScriptHandler(protoIdSet.sc_player_change_name_reply, "OnPlayerChangeNameReply")
protobuf:registerMessageScriptHandler(protoIdSet.sc_player_change_headicon_reply, "OnPlayerChangeHeadIconReply")
protobuf:registerMessageScriptHandler(protoIdSet.sc_items_init_update, "OnItemInitUpdate")
protobuf:registerMessageScriptHandler(protoIdSet.sc_items_delete, "OnItemDel")
protobuf:registerMessageScriptHandler(protoIdSet.sc_items_add, "ItemAdd")
protobuf:registerMessageScriptHandler(protoIdSet.sc_items_update, "ItemUpdate")
protobuf:registerMessageScriptHandler(protoIdSet.sc_real_name_req, "OnRealNameStatusRes")


M.ChangeNameAndSex = ChangeNameAndSex
M.ChangePlayerHead = ChangePlayerHead
-- 返回当前模块
return M
