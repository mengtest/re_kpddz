-- -----------------------------------------------------------------


-- *
-- * Filename:    MonthCardHelpWinMono.lua
-- * Summary:     月卡帮助界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/13/2017 11:04:07 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("MonthCardHelpWinMono")



-- 界面名称
local wName = "MonthCardHelpWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _winBg
local _btnSure
local _lb
--- [ALD END]




--- [ALF END]



local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnSure = UnityTools.FindGo(gameObject.transform, "Container/btnSure")
    UnityTools.AddOnClick(_btnSure.gameObject, CloseWin)

    _lb = UnityTools.FindGo(gameObject.transform, "Container/Label"):GetComponent("UILabel")

--- [ALB END]



end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

end


local function Start(gameObject)
    if CTRL.TipWord == "" then
        CTRL.TipWord = LuaText.GetString("month_card_help")
    end
    _lb.text = CTRL.TipWord
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("MonthCardHelpWinMono")
end



-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
