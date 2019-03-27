-- -----------------------------------------------------------------


-- *
-- * Filename:    RoomLvSelectWinController.lua
-- * Summary:     房间等级选择界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/18/2017 10:55:48 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RoomLvSelectWinController")

local GameMgr = IMPORT_MODULE("GameMgr")
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr");

local _roomType = 0;



-- 界面名称
local wName = "RoomLvSelectWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)


local function RoomType()
    return _roomType;
 end

local function OnCreateCallBack(gameObject)
    triggerScriptEvent(EVENT_CHAGNE_TOP,{name = wName});
end


local function OnDestoryCallBack(gameObject)
    triggerScriptEvent(EVENT_CHAGNE_TOP,{name = ""});
    _roomType = 0;
end

--desc:进入游戏中
--YQ.Qu:2017/2/18 0018
local function EnterGame(roomId,roomType)
    GameMgr.EnterGame(roomId,roomType, function () 
        UnityTools.DestroyWin("MainCenterWin")
        UnityTools.DestroyWin("MainWin")
        UnityTools.DestroyWin("GameCenterWin")
        
        UnityTools.DestroyWin(wName);
        _platformMgr.SetOpenWinName("")
    end);
    
end

local function OpenWin(roomType)
--    LogWarn("[RoomLvSelectWinController.OpenWin]"..roomType);
    _roomType = roomType
    UnityTools.CreateLuaWin(wName);
 end


UI.Controller.UIManager.RegisterLuaWinFunc("RoomLvSelectWin", OnCreateCallBack, OnDestoryCallBack)

M.EnterGame = EnterGame;
M.RoomType = RoomType;
M.OpenWin = OpenWin;

-- 返回当前模块
return M
