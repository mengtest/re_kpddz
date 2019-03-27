-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    ShareExchangeWinMono.lua
-- * Summary:     分享邀请码界面
-- *
-- * Version:     1.0.0
-- * Author:      MMCUXDSPE5IA8O3
-- * Date:        5/19/2017 8:49:40 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ShareExchangeWinMono")



-- 界面名称
local wName = "ShareExchangeWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
local CTRL_MAIN = IMPORT_MODULE("ShareWinController")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _inputBox
--- [ALD END]


--- [ALF END]

local _winBg = nil
local _input = nil


local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

local function ConfirmOnClick(gameObject)
	local code = _input.value
	if code == nil or code == "" or code == "请输入分享玩家的邀请码" then
		LogWarn("code 为 空")
		return
	end
	if tonumber(code) == nil then
		UtilTools.ShowMessage("输入错误，邀请码由数字组成","[FFFFFF]")
		return
	end
	LogWarn(code)
	
    local protobuf = sluaAux.luaProtobuf.getInstance()
    local req ={}
    req.code = code
    protobuf:sendMessage(protoIdSet.cs_share_new_bee_reward_req,req)
end

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
	_winBg = UnityTools.FindGo(gameObject.transform, "Container")
	
    local backGo = _winBg.transform:Find("closeBtn").gameObject;
    UIEventListener.Get(backGo).onClick = CloseWin;
	
	local inputGo = _winBg.transform:Find("input").gameObject;
	_input = inputGo:GetComponent("UIInput");
	
    local btnConfirm = _winBg.transform:Find("confirmBtn").gameObject;
    UIEventListener.Get(btnConfirm).onClick = ConfirmOnClick;

	_inputBox = UnityTools.FindGo(gameObject.transform, "Container/input"):GetComponent("UILabel")
	_inputBox.overflowMethod = 1
end


--- [ALB END]


function ShareExchangeUpdate()
	if CTRL_MAIN.Get == 1 then
		UnityTools.DestroyWin("ShareExchangeWin")
	end
end



local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
	registerScriptEvent(EVENT_UPDATE_SHARE_MAIN_UPDATE, "ShareExchangeUpdate")
end


local function Start(gameObject)
	_inputBox.width = 350
	_inputBox.height = 70
	
end


local function OnDestroy(gameObject)
	unregisterScriptEvent(EVENT_UPDATE_SHARE_MAIN_UPDATE, "ShareExchangeUpdate")
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
