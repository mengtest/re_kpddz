-- -----------------------------------------------------------------


-- *
-- * Filename:    SettingWinMono.lua
-- * Summary:     设置界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/9/2017 2:28:57 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("SettingWinMono")



-- 界面名称
local wName = "SettingWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr")

local _winBg
local _btnClose
local _headTexture
local _playerNameLb
local _btnChangeAccount
local _btnHelp
local _btnFeedBack
local _musicSpr
local _soundSpr
local _musicCircle
local _soundCircle

local moveTime = 0.5;
local openX = 162;
local closeX = 37;
local _headGo
local _versionInfo
local _headSpr
local _vipBox		
local _go	 
--- [ALD END]

local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

local function OnChangeAccountHandler(gameObject)
    --    UnityTools.MessageDialog("功能开发中....")
    UnityTools.ChangeLogin();
end

local function OnHelpHandler(gameObject)
    UnityTools.CreateLuaWin("HelpWin");
end

local function OnFeedBack(gameObject)
    UnityTools.CreateLuaWin("MainBugFeedBack");
end


local function initCircle(spr, circle, b)
    if b then
        spr.spriteName = "openBg";
		circle.spriteName = "btnCircle1"								
        circle.transform.localPosition = UnityEngine.Vector3(openX, 0, 0);
    else
        spr.spriteName = "closeBg";
		circle.spriteName = "btnCircle2"								
        circle.transform.localPosition = UnityEngine.Vector3(closeX, 0, 0);
    end
end

local function changeCircle(spr, circel)
    local isClose = true;
    local disX = openX;
    if spr.spriteName == "openBg" then
        spr.spriteName = "closeBg"
		circel.spriteName = "btnCircle2"								
        disX = closeX;
    else
        isClose = false;
        spr.spriteName = "openBg"
		circel.spriteName = "btnCircle1"								
    end
    local hash = iTween.Hash("time", moveTime, "x", disX, "islocal", true)
    iTween.MoveTo(circel.gameObject, hash)
    return isClose;
end

local function OnChangeMusic(gameObject)
    --    _musicCircle
    local isClose = changeCircle(_musicSpr, _musicCircle);
    if isClose then
        _platformMgr.SetMusic(0);
        UtilTools.StopBGM();
    else
        _platformMgr.SetMusic(50);
        UnityTools.SetBGM()
    end
end

local function OnChangeSound(gameObject)
    local isClose = changeCircle(_soundSpr, _soundCircle);
    --- TODO 设置音效
    _platformMgr.SetSound(isClose == false);
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, OnCloseHandler)

    _headTexture = UnityTools.FindCo(gameObject.transform, "UITexture", "Container/head/Texture")

    _playerNameLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/playerName")

    _btnChangeAccount = UnityTools.FindGo(gameObject.transform, "Container/btnChangeAccount")
    UnityTools.AddOnClick(_btnChangeAccount.gameObject, OnChangeAccountHandler)

    _btnHelp = UnityTools.FindGo(gameObject.transform, "Container/btnHelp")
    UnityTools.AddOnClick(_btnHelp.gameObject, OnHelpHandler)

    _btnFeedBack = UnityTools.FindGo(gameObject.transform, "Container/btnFeedBack")
    UnityTools.AddOnClick(_btnFeedBack.gameObject, OnFeedBack)

    _musicSpr = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/music/Sprite")
    UnityTools.AddOnClick(_musicSpr.gameObject, OnChangeMusic)

    _soundSpr = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/sound/Sprite")
    UnityTools.AddOnClick(_soundSpr.gameObject, OnChangeSound)

    _musicCircle = UnityTools.FindGo(gameObject.transform, "Container/music/btn"):GetComponent("UISprite")

    _soundCircle = UnityTools.FindGo(gameObject.transform, "Container/sound/btn"):GetComponent("UISprite")

    _headGo = UnityTools.FindGo(gameObject.transform, "Container/head")
    _headSpr = UnityTools.FindCo(gameObject.transform,"UISprite", "Container/head")

    _versionInfo = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/version")

	_vipBox = UnityTools.FindGo(gameObject.transform, "Container/head/vip/vipBox")																			  
--- [ALB END]

end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    UnityTools.OpenAction(_winBg);
    _go = gameObject
    local playerIcon = _platformMgr.GetIcon();
    local playerName = _platformMgr.UserName();
    if playerIcon ~= nil and playerIcon ~= "" then
        LogError(playerIcon)
        UnityTools.SetPlayerHead(playerIcon, _headTexture, true);
    else
        _headSpr.spriteName = _platformMgr.PlayerDefaultHead(_platformMgr.getSex())
    end

	
    _playerNameLb.text = playerName;
    _versionInfo.text = UnityEngine.Application.version .. "." .. g_luaVersion
end


local function Start(gameObject)
    initCircle(_musicSpr, _musicCircle, _platformMgr.GetMusic() > 0)
    initCircle(_soundSpr, _soundCircle, _platformMgr.GetSound() > 0)
    UnityTools.SetNewVipBox(_vipBox, _platformMgr.GetVipLv(),"vip",_go);
    registerScriptEvent(EVENT_GAME_START_EFFECT, CloseWin)
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(EVENT_GAME_START_EFFECT, CloseWin)
    CLEAN_MODULE("SettingWinMono")
end



-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
