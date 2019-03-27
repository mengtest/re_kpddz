-- -----------------------------------------------------------------


-- *
-- * Filename:    UserIDCardBindWinController.lua
-- * Summary:     UserIDCardBindWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/14/2018 3:54:27 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("UserIDCardBindWinController")



-- 界面名称
local wName = "UserIDCardBindWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local protobuf = sluaAux.luaProtobuf.getInstance()

local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end

function OnRealNameResponse(idMsg,tMsgData)
    UtilTools.HideWaitFlag()
    if tMsgData.result == 0 then
        triggerScriptEvent(EVENT_UPDATE_PLAYER_WIN_INFO, {})
        ShowAwardWin(tMsgData.rewards)
    else
        UtilTools.ShowMessage(tMsgData.err,"[FFFFFF]");
    end
end


UI.Controller.UIManager.RegisterLuaWinFunc("UserIDCardBindWin", OnCreateCallBack, OnDestoryCallBack)

protobuf:registerMessageScriptHandler(protoIdSet.sc_real_name_update, "OnRealNameResponse");
-- 返回当前模块
return M
