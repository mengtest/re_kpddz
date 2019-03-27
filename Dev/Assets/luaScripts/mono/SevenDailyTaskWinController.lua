-- -----------------------------------------------------------------
-- * Copyright (c) 2018 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    SevenDailyTaskWinController.lua
-- * Summary:     七日狂欢
-- *
-- * Version:     1.0.0
-- * Author:      E9GGF9T1HRZTCS8
-- * Date:        1/10/2018 11:02:38 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("SevenDailyTaskWinController")



-- 界面名称
local wName = "SevenDailyTaskWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local protobuf=sluaAux.luaProtobuf.getInstance()
local UnityTools = IMPORT_MODULE("UnityTools")

M.isReceive = false
M.taskId = 0 --任务编号
M.process = 0 --完成度
M.status = 0--0,进行中,1完成,2结束(奖励已领)
M.award = 0 

local function OnCreateCallBack(gameObject)

end

local function OnDestoryCallBack(gameObject)
    
end

-- function SevenDailyTaskReturnToLoginScene()
--     M.isReceive = false
--     M.taskId = 0 --任务编号
--     M.process = 0 --完成度
--     M.status = 0--0,进行中,1完成,2结束(奖励已领)
--     M.award = 0 
--     local activityCtrl=IMPORT_MODULE("ActivityMainWinController")
--     activityCtrl.isOpenOnce = 1
-- end

function OnReceiveAwardResponse(idMsg, tMsgData)
    -- body
    LogWarn("OnReceiveAwardResponse-----")
    if tMsgData == nil then
        return
    end

    if tMsgData.result == 0 then
        ShowAwardWin(tMsgData.rewards)
    else
        UnityTools.ShowMessage(tMsgData.err)
    end
end

function OnReceiveTaskSevenInfo(idMsg,tMsgData)
    LogWarn("OnReceiveTaskSevenInfo-----")
    if tMsgData == nil then
        return
    end

    M.taskId = tMsgData.task_id
    
    M.process = tMsgData.process
    M.status = tMsgData.status
    M.award = tMsgData.award
    M.isReceive = true

    triggerScriptEvent(EVENT_SEVEN_TASK_STATUS_UPDATE,{})
end
function SevenDailyTaskWinControllerClear()
    M.isReceive = false
    M.taskId = 0 --任务编号
    M.process = 0 --完成度
    M.status = 0--0,进行中,1完成,2结束(奖励已领)
    M.award = 0 
end
registerScriptEvent(EXIT_CLEAR_ALL_DATA, "SevenDailyTaskWinControllerClear")
protobuf:registerMessageScriptHandler(protoIdSet.sc_task_seven_award_response, "OnReceiveAwardResponse")
protobuf:registerMessageScriptHandler(protoIdSet.sc_task_seven_info_response, "OnReceiveTaskSevenInfo")
-- UI.Controller.UIManager.RegisterLuaFuncCall("ReturnToLoginScene", SevenDailyTaskReturnToLoginScene)
UI.Controller.UIManager.RegisterLuaWinFunc("SevenDailyTaskWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
