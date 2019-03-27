-- -----------------------------------------------------------------


-- *
-- * Filename:    FruitPoolWinController.lua
-- * Summary:     FruitPoolWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        3/18/2017 11:06:54 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FruitPoolWinController")


local protobuf = sluaAux.luaProtobuf.getInstance()
M.PoolInfo = {}
-- 界面名称
local wName = "FruitPoolWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end
function OnLabaGetPoolListUpdate(idMsg,tMsgData)
    if tMsgData.win_players ~=nil then
        M.PoolInfo = tMsgData.win_players
    else
        M.PoolInfo={}
    end
    table.sort(M.PoolInfo,function(a,b) return a.c_time>b.c_time end)
    triggerScriptEvent(EVENT_LABA_POOL_UPDATE,{})
end

protobuf:registerMessageScriptHandler(protoIdSet.sc_win_player_list , "OnLabaGetPoolListUpdate")
UI.Controller.UIManager.RegisterLuaWinFunc("FruitPoolWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
