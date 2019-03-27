-- -----------------------------------------------------------------


-- *
-- * Filename:    PlayerRankInfoWinController.lua
-- * Summary:     牌局外查其他玩家的信息
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/23/2017 6:11:23 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("PlayerRankInfoWinController")

local otherPlayerData;

-- 界面名称
local wName = "PlayerRankInfoWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local _playerUuid
local UnityTools = IMPORT_MODULE("UnityTools")
local protobuf = sluaAux.luaProtobuf.getInstance();
local _mono




local function OnCreateCallBack(gameObject)
    _mono = IMPORT_MODULE(wName .. "mono")

    local req = {}
    req.obj_player_uuid = _playerUuid;
    protobuf:sendMessage(protoIdSet.cs_query_player_winning_rec_req, req);
end


local function OnDestoryCallBack(gameObject)
    otherPlayerData = nil;
    _mono = nil
end

--desc:打开
--YQ.Qu:2017/2/23 0023
local function Open(playerUuid)
    _playerUuid= playerUuid;
    UnityTools.CreateLuaWin("PlayerRankInfoWin");
end


UI.Controller.UIManager.RegisterLuaWinFunc("PlayerRankInfoWin", OnCreateCallBack, OnDestoryCallBack)
M.Open = Open
M.otherPlayerData = otherPlayerData
-- 返回当前模块
return M
