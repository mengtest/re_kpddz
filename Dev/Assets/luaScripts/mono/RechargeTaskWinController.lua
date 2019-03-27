-- -----------------------------------------------------------------
-- * Copyright (c) 2018 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    RechargeTaskWinController.lua
-- * Summary:     RechargeTaskWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        3/9/2018 2:48:05 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RechargeTaskWinController")



-- 界面名称
local wName = "RechargeTaskWin"
-- 获取界面控制器
local protobuf = sluaAux.luaProtobuf.getInstance()
local _controller = UI.Controller.UIManager.GetControler(wName)
M.PayId = 0
M.IsOpen = 2
M.Process = 0
M.Status = {}
M.NeedInitData = true
M.IsShowRed = false
M.IsGetMsg = false
M.ConfigTb={}
M.MainData = {}
M.TotalGold = 0
local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)
    M.NeedInitData = true
end
local function GetConfig()
    local index = 1
    M.ConfigTb={}
    M.TotalGold = 0
    
    M.MainData = LuaConfigMgr.ActDesConfig[tostring(M.PayId)]
    if M.MainData == nil then
        return
    end
    for k,v in pairs(LuaConfigMgr.ActGoldingConfig) do
        if tonumber(v.recharge) == M.PayId then
            table.insert(M.ConfigTb,v)
            M.TotalGold = M.TotalGold + tonumber(v.reward[1][3])
            index=index+1
        end
    end
    table.sort(M.ConfigTb,function(a,b) return tonumber(a.key) <tonumber(b.key) end)
    M.IsShowRed=false
    for i=1,#M.ConfigTb do
        M.ConfigTb[i].status = 0
        for j=1,#M.Status do
            if tonumber(M.ConfigTb[i].key) == M.Status[j] then
                M.ConfigTb[i].status = 2
                break
            end
        end
        if M.ConfigTb[i].status ~= 2 then
            if M.Process>=tonumber(M.ConfigTb[i].total_gold) then
                M.ConfigTb[i].status =1
                M.IsShowRed=true
            else
                M.ConfigTb[i].status =0
            end
        end
       M.ConfigTb[i].awardIndex = tonumber(M.ConfigTb[i].key)
    end
    table.sort(M.ConfigTb,function(a,b)
            if a.status~= b.status then
                if a.status == 1 then
                    return true
                elseif a.status == 0 then
                    return b.status == 2
                elseif a.status == 2 then
                    return false
                else
                    return false
                end
            else
                return tonumber(a.key) <tonumber(b.key)
            end
        end)
end
function OnRechargeTaskInfo(idMsg,tMsgData)
    if tMsgData ~= nil then
        M.NeedInitData = M.PayId ~= tMsgData.task_id
        M.PayId = tMsgData.task_id
        M.IsOpen = tMsgData.open
        M.Process = tonumber(tMsgData.process)
        if tMsgData.status ~= nil then
            M.Status = tMsgData.status
        else
            M.Status={}
        end
        GetConfig()
        triggerScriptEvent(EVENT_UPDATE_RECHARGE_TASK,{})
        
    end
end
function OnRechargeDrawReply(idMsg,tMsgData)
    if tMsgData.result == 0 then
        ShowAwardWin(tMsgData.reward_list)
    else
        UtilTools.ShowMessage(tMsgData.err,"[FFFFFF]");
    end
end

UI.Controller.UIManager.RegisterLuaWinFunc("RechargeTaskWin", OnCreateCallBack, OnDestoryCallBack)
protobuf:registerMessageScriptHandler(protoIdSet.sc_task_pay_info_response,"OnRechargeTaskInfo")
protobuf:registerMessageScriptHandler(protoIdSet.sc_task_pay_award_response,"OnRechargeDrawReply")

-- 返回当前模块
return M
