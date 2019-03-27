-- -----------------------------------------------------------------


-- *
-- * Filename:    FruitRankWinController.lua
-- * Summary:     超级水果排行榜
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/26/2018 2:08:55 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FruitRankWinController")



-- 界面名称
local wName = "FruitRankWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance()
M.RankTable={}
M.LastWeekTable ={}
M.my_rank = 0
M.my_recharge_money = 0
local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end

local function sortFunc(a,b)
    return a.rank<b.rank
end
function OnHundredFruitRankReqBack(msgId, tMsgData)
    LogError("M.RankTable="..#M.RankTable)
    if tMsgData == nil then
        return;
    end
    LogError("tMsgData.rank_type="..tMsgData.rank_type)
    if tMsgData.rank_type ~= 6 then
        return
    end
    if tMsgData.rank_info_list ~= nil then
        M.RankTable = tMsgData.rank_info_list
    end
    table.sort(M.RankTable,sortFunc)
    M.my_rank = tMsgData.my_rank
    M.my_recharge_money = tMsgData.my_recharge_money
    triggerScriptEvent(EVENT_UPDATE_FRUIT_SUPER_RANK_INFO,{})
end

function OnHunderedLastWeekReqBack(msgId,tMsgData)
    if tMsgData.list~=nil then
        M.LastWeekTable = tMsgData.list
    end
    triggerScriptEvent(EVENT_UPDATE_FRUIT_SUPER_RANK_INFO,{})
end


UI.Controller.UIManager.RegisterLuaWinFunc("FruitRankWin", OnCreateCallBack, OnDestoryCallBack)

-- protobuf:registerMessageScriptHandler(protoIdSet.sc_rank_qurey_reply , "OnHundredFruitRankReqBack") 

protobuf:registerMessageScriptHandler(protoIdSet.sc_super_laba_last_week_rank_query_reply,"OnHunderedLastWeekReqBack")
-- 返回当前模块
return M
