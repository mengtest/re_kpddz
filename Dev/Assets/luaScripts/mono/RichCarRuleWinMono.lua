-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    RichCarRuleWinMono.lua
-- * Summary:     RichCarRuleWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        5/3/2017 8:21:44 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RichCarRuleWinMono")



-- 界面名称
local wName = "RichCarRuleWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _btnConfirm
local _btnClose
local _headPos
local _roadPos
--- [ALD END]





--- [ALF END]



local function CloseWin(gameObject)
    local isFirst = UnityEngine.PlayerPrefs.GetInt("IsRichCarFirst",0)
    if isFirst == 0 then
        UnityEngine.PlayerPrefs.SetInt("IsRichCarFirst",1)
        triggerScriptEvent(EVENT_UPDATE_RICH_CAR_TIP,{})
    end
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _btnConfirm = UnityTools.FindGo(gameObject.transform, "Container/BtnConfirm")
    UnityTools.AddOnClick(_btnConfirm.gameObject, CloseWin)

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/Close")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

    _headPos = UnityTools.FindGo(gameObject.transform, "Container/head")

    _roadPos = UnityTools.FindGo(gameObject.transform, "Container/road")

--- [ALB END]




end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

end


local function Start(gameObject)

end


local function OnDestroy(gameObject)
    CLEAN_MODULE(wName .. "Mono")
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
