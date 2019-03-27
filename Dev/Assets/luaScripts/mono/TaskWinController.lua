-- -----------------------------------------------------------------


-- *
-- * Filename:    TaskWinController.lua
-- * Summary:     任务面板
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/13/2017 5:34:31 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("TaskWinController")
local _itemMgr = IMPORT_MODULE("ItemMgr")
local UnityTools = IMPORT_MODULE("UnityTools")
local protobuf = sluaAux.luaProtobuf.getInstance();

M.TabIndex = 1
-- 界面名称
local wName = "TaskWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local  isNoShowGo = false;

local function OnCreateCallBack(gameObject)
end

local function IsNoShowGo()
    return isNoShowGo;
 end


local function OnDestoryCallBack(gameObject)
    isNoShowGo = false;
    M.TabIndex = 1
end

local function IsWinShow()
    return UI.Controller.UIManager.IsWinShow("TaskWin");
end


---desc:牌局内打开任务界面
--YQ.Qu:2017/3/24 0024
-- @param
-- @return
local function OpenByGame()
    isNoShowGo = true;
    UnityTools.CreateLuaWin("TaskWin");
end

local function Open()
    isNoShowGo = false;
    UnityTools.CreateLuaWin("TaskWin");
 end

function OnMissionInit(msgId, tMsgData)
--    LogWarn("[TaskWinController.OnMissionInit]任务信息下来了。。。");
    if tMsgData == nil then
        return;
    end

    _itemMgr.InitMission(tMsgData);
    if IsWinShow() then
        local win = IMPORT_MODULE("TaskWinMono")
        win.UpdateWin();
    end
    triggerScriptEvent(UPDATE_MAIN_WIN_RED,"task")
end



function OnMissionUpdate(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    _itemMgr.UpdateOneMission(tMsgData);
    if IsWinShow() then
        local win = IMPORT_MODULE("TaskWinMono")
        win.UpdateWin();
    end
    triggerScriptEvent(UPDATE_MAIN_WIN_RED,"task")
    if tMsgData.mission_.id > 511000 and tMsgData.mission_.id <520000 then
        triggerScriptEvent(EVENT_TASK_INFO_UPDATE,{})
    end
end

function OnMissionAdd(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    _itemMgr.AddOneMission(tMsgData);

    if IsWinShow() then
        local win = IMPORT_MODULE("TaskWinMono")
        win.UpdateList();
    end
    triggerScriptEvent(UPDATE_MAIN_WIN_RED,"task")
    if tMsgData.mission_.id > 511000 and tMsgData.mission_.id <520000 then
        triggerScriptEvent(EVENT_TASK_INFO_UPDATE,{})
    end
end

function OnMissionDel(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    _itemMgr.DelOneMission(tMsgData);

    if IsWinShow() then
        local win = IMPORT_MODULE("TaskWinMono")
        win.UpdateList();
    end
    triggerScriptEvent(UPDATE_MAIN_WIN_RED,"task")
    if tMsgData.id > 511000 and tMsgData.id <520000 then
        triggerScriptEvent(EVENT_TASK_INFO_UPDATE,{})
    end
end
function OnDrawMissionResultReply(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    if tMsgData.result == 0 then
        ShowAwardWin(tMsgData.reward_info_s);
    else
        UnityTools.ShowMessage(tMsgData.err_msg);
    end
end


UI.Controller.UIManager.RegisterLuaWinFunc("TaskWin", OnCreateCallBack, OnDestoryCallBack)
protobuf:registerMessageScriptHandler(protoIdSet.sc_mission, "OnMissionInit")
protobuf:registerMessageScriptHandler(protoIdSet.sc_mission_update, "OnMissionUpdate")
protobuf:registerMessageScriptHandler(protoIdSet.sc_mission_add, "OnMissionAdd")
protobuf:registerMessageScriptHandler(protoIdSet.sc_mission_del, "OnMissionDel")
protobuf:registerMessageScriptHandler(protoIdSet.sc_draw_mission_result_reply, "OnDrawMissionResultReply")

M.IsNoShowGo = IsNoShowGo;
M.OpenByGame = OpenByGame;
M.Open = Open;
-- 返回当前模块
return M
