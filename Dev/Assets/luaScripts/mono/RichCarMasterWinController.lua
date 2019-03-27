-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    RichCarMasterWinController.lua
-- * Summary:     RichCarMasterWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        4/24/2017 4:41:13 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RichCarMasterWinController")



-- 界面名称
local wName = "RichCarMasterWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance()
local UnityTools = IMPORT_MODULE("UnityTools")
M.WinType = 1
M.List = {}

local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)
    M.WinType = 1
end

function OnRichCarMasterListResponse(idMsg,tMsgData)
    UtilTools.HideWaitFlag()
    -- M.WinType=tMsgData.flag
    if tMsgData.list==nil then
        M.List = {}
    else
        M.List = tMsgData.list
    end
    if UI.Controller.UIManager.IsWinShow("RichCarMasterWin") then
        triggerScriptEvent(EVENT_RICHCAR_MASTER_LIST_UPDATE, {})
    else
        UnityTools.CreateLuaWin("RichCarMasterWin")
    end
end
function OnRichCarAddMoneyResponse(idMsg,tMsgData)
    if tMsgData.result == 0 then
        UtilTools.ShowMessage(LuaText.GetString("rich_car_tip46"),"[FFFFFF]")  
        UnityTools.DestroyWin(wName)
    else
        UtilTools.ShowMessage(tMsgData.err,"[FFFFFF]")
        -- triggerScriptEvent(EVENT_UPDATE_RICHCAR_ADDMONEY_RESPONSE, {})
    end
end

UI.Controller.UIManager.RegisterLuaWinFunc("RichCarMasterWin", OnCreateCallBack, OnDestoryCallBack)
protobuf:registerMessageScriptHandler(protoIdSet.sc_car_master_wait_list_reply , "OnRichCarMasterListResponse")   --进入
protobuf:registerMessageScriptHandler(protoIdSet.sc_car_add_money_reply , "OnRichCarAddMoneyResponse")   --进入

-- 返回当前模块
return M
