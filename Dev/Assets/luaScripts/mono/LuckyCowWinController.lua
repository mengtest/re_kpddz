-- -----------------------------------------------------------------


-- *
-- * Filename:    LuckyCowWinController.lua
-- * Summary:     招财金牛
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/6/2017 11:26:24 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("LuckyCowWinController")



-- 界面名称
local wName = "LuckyCowWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local _platformMgr = IMPORT_MODULE("PlatformMgr");
local UnityTools = IMPORT_MODULE("UnityTools");
local protobuf = sluaAux.luaProtobuf.getInstance();

local data = { lv = 1, openNeed = 10 }
M.Process = 0

local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end

local function cfgLenCheck(lv)
    if LuaConfigMgr.GodGoldConfigLen == nil then
        return lv
    elseif lv>LuaConfigMgr.GodGoldConfigLen then
        return LuaConfigMgr.GodGoldConfigLen
    end
    if lv == 0 then
        return 1;
    end
    return lv
 end

local function ResetData()
    local rmb = _platformMgr.RMB()
    if data.rmb == nil or data.rmb ~= rmb then
        data.rmb = rmb;
        data.lv = math.floor(rmb / 10);
        data.lv = cfgLenCheck(data.lv)
        if rmb < 10 then
            data.isOpen = false;

        else
            data.isOpen = true;
        end
        if LuaConfigMgr.GodGoldConfig[data.lv .. ""] ~= nil then
            data.cfg = LuaConfigMgr.GodGoldConfig[data.lv .. ""];
        end
        if rmb < 10 then
            data.openNeed = 10 - rmb;
        else
            data.openNeed = 10 - rmb;
        end
        triggerScriptEvent(UPDATE_LUCKY_COW_WIN, "allData", data);
        return
    end
end

function OnResouceUpdateInLuckyCow(msgID, value)
--    LogWarn("[LuckyCowWinController.OnResouceUpdateInLuckyCow]");
    ResetData();
end

registerScriptEvent(EVENT_RESCOURCE_UDPATE, "OnResouceUpdateInLuckyCow")
function OnGoldenBullMissionUpdate(msgId,tMsgData)
    LogError("ssss")
    M.Process = tonumber(tMsgData.process)
    triggerScriptEvent(UPDATE_LUCKY_COW_WIN, "mission");
end

--- desc:金牛领奖信息 登入和领完奖时同步
--- YQ.Qu
function OnGolderBullInfoUpdate(msgId, tMsgData)

    if tMsgData == nil then
        return;
    end
    data.leftTime = tMsgData.left_times;
    local rmb = _platformMgr.RMB()
    if data.rmb == nil or data.rmb ~= rmb then
        data.rmb = rmb;
        data.lv = math.floor(rmb / 10);
        data.lv = cfgLenCheck(data.lv)
        if rmb < 10 then
            data.isOpen = false;

        else
            data.isOpen = true;
        end

        if LuaConfigMgr.GodGoldConfig[data.lv .. ""] ~= nil then
            data.cfg = LuaConfigMgr.GodGoldConfig[data.lv .. ""];
        end
        if rmb < 10 then
            data.openNeed = 10 - rmb;
        else
            data.openNeed = 10 - rmb;
        end
        triggerScriptEvent(UPDATE_LUCKY_COW_WIN, "allData", data);
        return
    end
    triggerScriptEvent(UPDATE_LUCKY_COW_WIN, "leftTime", data);
end

--- desc:
--- YQ.Qu
function OnGoldenBullDrawReply(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    if tMsgData.result == 0 then
        ShowAward_Monoey(data.cfg.get_gold);
    else
        UnityTools.ShowMessage(tMsgData.err);
    end
end

UI.Controller.UIManager.RegisterLuaWinFunc("LuckyCowWin", OnCreateCallBack, OnDestoryCallBack)

protobuf:registerMessageScriptHandler(protoIdSet.sc_golden_bull_info_update, "OnGolderBullInfoUpdate");
protobuf:registerMessageScriptHandler(protoIdSet.sc_golden_bull_draw_reply, "OnGoldenBullDrawReply");
protobuf:registerMessageScriptHandler(protoIdSet.sc_golden_bull_mission, "OnGoldenBullMissionUpdate");
M.Data = data;
-- 返回当前模块
return M
