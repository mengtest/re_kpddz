-- -----------------------------------------------------------------


-- *
-- * Filename:    MonthCardWinController.lua
-- * Summary:     至尊月卡
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/12/2017 6:23:47 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("MonthCardWinController")
local protobuf = sluaAux.luaProtobuf.getInstance();
local UnityTools = IMPORT_MODULE("UnityTools");


-- 界面名称
local wName = "MonthCardWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)


local data = { flag = 2, leftDays = 0, nowBuyGet = 3000000, dayGet = 40000, days = 30 }

local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end

--- 是否显示界面上的按钮
--- @param return flag bool
local function IsShowMonthCardBtn()
    return data.flag ~= 0;
end

--- desc:
--- YQ.Qu
function OnMonthCardInfoUpdate(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    data.flag = tMsgData.today_draw_flag;
    data.leftDays = tMsgData.left_times;
    triggerScriptEvent(EVENT_MONTH_CARD_UPDATE, data.flag,"monthCard")
end

--- desc:
--- YQ.Qu
function OnMonthCardDrawReply(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    if tMsgData.result == 0 then
        data.flag = 0;
        triggerScriptEvent(EVENT_MONTH_CARD_UPDATE, data.flag,"monthCard")
        ShowAward_Monoey(data.dayGet, 101);
    else
        UnityTools.ShowMessage(tMsgData.err);
    end
end



UI.Controller.UIManager.RegisterLuaWinFunc("MonthCardWin", OnCreateCallBack, OnDestoryCallBack)
protobuf:registerMessageScriptHandler(protoIdSet.sc_month_card_info_update, "OnMonthCardInfoUpdate")
protobuf:registerMessageScriptHandler(protoIdSet.sc_month_card_draw_reply, "OnMonthCardDrawReply")

M.IsShowMonthCardBtn = IsShowMonthCardBtn;
M.Data = data;
-- 返回当前模块
return M
