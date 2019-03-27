-- -----------------------------------------------------------------


-- *
-- * Filename:    GamePlayerInfoWinController.lua
-- * Summary:     牌局内查看玩家信息
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/24/2017 11:21:57 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("GamePlayerInfoWinController")
local _platformMgr = IMPORT_MODULE("PlatformMgr")
local protobuf = sluaAux.luaProtobuf.getInstance();

local _mono;
local otherPlayerData;
local _otherPlayerUuid;

-- 界面名称
local wName = "GamePlayerInfoWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)
        _mono = IMPORT_MODULE(wName .. "mono");
    local req = {}
    req.obj_player_uuid = _otherPlayerUuid;
    protobuf:sendMessage(protoIdSet.cs_query_player_winning_rec_req, req);
end

local function SetOpenPlayerUuid(otherPlayerUuid)
    LogWarn("[GamePlayerInfoWinController.SetOpenPlayerUuid]请求玩家的信息==" .. otherPlayerUuid);
    _otherPlayerUuid = otherPlayerUuid;
end


local function OnDestoryCallBack(gameObject)
--    _mono = nil;
--    otherPlayerData = nil;
--    _otherPlayerUuid = nil;
end
local function GetOtherPlayerData()
    return otherPlayerData
 end

function OnGameQueryPlayerWinRecReply(msgId, tMsgData)
    if tMsgData.obj_player_uuid == nil or tMsgData.obj_player_uuid == "" then
        _platformMgr.UpdatePlayerWinningRec(tMsgData)
        triggerScriptEvent(EVENT_UPDATE_PLAYER_WIN_INFO, {})
        return;
    end
    otherPlayerData = tMsgData;
    triggerScriptEvent(SHOW_PLAYER_INFO,tMsgData);
end

local function OtherPlayerUuid()
    return _otherPlayerUuid;
end




UI.Controller.UIManager.RegisterLuaWinFunc("GamePlayerInfoWin", OnCreateCallBack, OnDestoryCallBack)
protobuf:registerMessageScriptHandler(protoIdSet.sc_query_player_winning_rec_reply, "OnGameQueryPlayerWinRecReply");

M.SetOpenPlayerUuid = SetOpenPlayerUuid;
M.OtherPlayerUuid = OtherPlayerUuid;
M.GetOtherPlayerData = GetOtherPlayerData;

-- 返回当前模块
return M
