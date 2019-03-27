-- -----------------------------------------------------------------


-- *
-- * Filename:    BigRewardWinController.lua
-- * Summary:     BigRewardWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        2/28/2017 3:29:12 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("BigRewardWinController")

local UnityTools = IMPORT_MODULE("UnityTools")

-- 界面名称
local wName = "BigRewardWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

M.LabelValue=0
M.bDelay=false
M.bMask=false
M.StillTime=3000
M.Type=1
M.Free=0
M.Sound="Sounds/Laba/goldresult"
M.Sound2=""
M.PoolPos = 0
local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)
    M.StillTime=3000
    M.bDelay =false
    M.bMask=false
    M.Type=1
    M.Free=0
    M.Sound="Sounds/Laba/goldresult"
    M.Sound2=""
    M.PoolPos = 0
    triggerScriptEvent(EVENT_LABA_SPIN_AUTO,{})
end
UI.Controller.UIManager.RegisterLuaWinFunc("BigRewardWin", OnCreateCallBack, OnDestoryCallBack)
local protobuf = sluaAux.luaProtobuf.getInstance()

-- 返回当前模块
return M
