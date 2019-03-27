-- -----------------------------------------------------------------


-- *
-- * Filename:    NormalCowTopMono.lua
-- * Summary:     看牌顶层界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/9/2017 10:28:00 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("NormalCowTopMono")



-- 界面名称
local wName = "NormalCowTop"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")


local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


local function Awake(gameObject)

end


local function Start(gameObject)
    triggerScriptEvent(NORMAL_COW_TOP_WIN_START, gameObject)
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("NormalCowTopMono")
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
