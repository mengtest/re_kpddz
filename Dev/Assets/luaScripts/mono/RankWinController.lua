-- -----------------------------------------------------------------


-- *
-- * Filename:    RankWinController.lua
-- * Summary:     RankWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        7/6/2017 1:28:50 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RankWinController")



-- 界面名称
local wName = "RankWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance()
M.RankTable={}


local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end
local function sortFunc(a,b)
    return a.rank<b.rank
end
function OnHundredRankReqBack(msgId, tMsgData)
    if tMsgData == nil then
        return;
    end
    if tMsgData.list == nil then
        M.RankTable = {} 
        return
    end
    M.RankTable = tMsgData.list
    table.sort(M.RankTable,sortFunc)
    triggerScriptEvent(EVENT_UPDATE_RANK_INFO,{})
end


UI.Controller.UIManager.RegisterLuaWinFunc("RankWin", OnCreateCallBack, OnDestoryCallBack)

protobuf:registerMessageScriptHandler(protoIdSet.sc_hundred_last_week_rank_query_reply , "OnHundredRankReqBack") 
-- 返回当前模块
return M
