-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    ShareSelectWinMono.lua
-- * Summary:     分享选择界面
-- *
-- * Version:     1.0.0
-- * Author:      MMCUXDSPE5IA8O3
-- * Date:        5/18/2017 6:08:16 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ShareSelectWinMono")



-- 界面名称
local wName = "ShareSelectWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local _winBg;


-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
local CTRL_MAIN = IMPORT_MODULE("ShareWinController")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")


local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

local function WeChatOnClick(gameObject)
    local strTitle = LuaText.GetString("shar_friend_big_gift")
    local strDesc = LuaText.GetString("shar_friend_desc")
    UtilTools.ShareWeChat(CTRL_MAIN.MyCode, strTitle, strDesc)
end
local function WeChatMomentsOnClick(gameObject)
    local strTitle = LuaText.GetString("shar_friend_big_gift")
    local strDesc = LuaText.GetString("shar_friend_desc")
    UtilTools.ShareWeChatMoments(CTRL_MAIN.MyCode, strTitle, strDesc)
end

local function AutoLuaBind(gameObject)
	_winBg = UnityTools.FindGo(gameObject.transform, "Container")
	
    local backGo = _winBg.transform:Find("closeBtn").gameObject;
    UIEventListener.Get(backGo).onClick = CloseWin;
	
    local btnWeChat = _winBg.transform:Find("bg/bg3/WeiChat").gameObject;
    UIEventListener.Get(btnWeChat).onClick = WeChatOnClick;
    local btnWeChatMoments = _winBg.transform:Find("bg/bg3/WeiChatMoments").gameObject;
    UIEventListener.Get(btnWeChatMoments).onClick = WeChatMomentsOnClick;
end

local function Awake(gameObject)
	AutoLuaBind(gameObject);
end


local function Start(gameObject)

end


local function OnDestroy(gameObject)

end


local function OnEnable(gameObject)

end


local function OnDisable(gameObject)

end




-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.OnEnable = OnEnable
M.OnDisable = OnDisable


-- 返回当前模块
return M
