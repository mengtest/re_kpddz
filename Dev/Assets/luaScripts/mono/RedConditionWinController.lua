-- -----------------------------------------------------------------


-- *
-- * Filename:    RedConditionWinController.lua
-- * Summary:     红包场入口补充界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/15/2017 5:31:59 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RedConditionWinController")



-- 界面名称
local wName = "RedConditionWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local data = { isInit = false ,isGame = false}
local protobuf = sluaAux.luaProtobuf.getInstance();
local UnityTools = IMPORT_MODULE("UnityTools")
local GameMgr = IMPORT_MODULE("GameMgr")
M.LeftTimes=0
--- 数据初始化
function data:Init()
    self.list = {}
    self.isInit = true
    for k, v in pairs(LuaConfigMgr.DiamondReviveTask) do
        local targetNum = tonumber(v.parameter3)
        local value = { cfg = v, target = targetNum, current = 0 }
        table.insert(self.list, value)
    end
    table.sort(self.list, function(a, b)
        local aId = tonumber(a.cfg.key)
        local bId = tonumber(b.cfg.key)
        return aId < bId
    end)
end

function OnRedConditionWinClear()
    local ctrl = IMPORT_MODULE("RedConditionWinController");
    if ctrl ~= nil and ctrl.Data ~= nil then
        ctrl.Data:Clear()
    end
end

registerScriptEvent(EXIT_CLEAR_ALL_DATA, "OnRedConditionWinClear")

function data:Clear()
    self.isInit = false
    self.isGame = false
end

--更新数据
function data:Update(mission)
    local taskId = mission.id .. "";

    for i = 1, #self.list do
        if self.list[i].cfg.key == taskId then
            self.list[i].current = mission.count
        end
    end
end

function data:GetItem(index)
    return self.list[index]
end


--- 所有的条件是否达成了
function data:IsAllCompleted()
    if self.isInit == false then
        self:Init()
    end

    local count = #self.list
    local completedCount = 0
    for i = 1, count do
        if self.list[i].current >= self.list[i].target then
            completedCount = completedCount + 1
        end
    end

    return count == completedCount
end


local function OnCreateCallBack(gameObject)
end
local function OnGoGame()
    GameMgr.EnterGame(1, 10, function()
        UnityTools.DestroyWin("MainCenterWin")
        UnityTools.DestroyWin("MainWin")
        UnityTools.DestroyWin("RedConditionWin")
        UnityTools.DestroyWin("GameCenterWin")
    end)
end


local function OnDestoryCallBack(gameObject)
    data.isGame = false
end
function OnRedPackReliveReply(msgId, tMsgData)
    if tMsgData ~= nil then
        if tMsgData.result == 0 and data.isGame == false then
            local mainWin = IMPORT_MODULE("NormalCowMainMono");
            if mainWin ~= nil then
                mainWin.ClickRestartCall(nil);
                UnityTools.DestroyWin("RedConditionWin")
            else
                OnGoGame()
            end
            
            M.LeftTimes=M.LeftTimes-1
        elseif tMsgData.result == 0 then
            UnityTools.DestroyWin("RedConditionWin")
            UnityTools.ShowMessage(LuaText.GetString("red_condition_win_tip12"))
            M.LeftTimes=M.LeftTimes-1
        else
            UnityTools.ShowMessage(tMsgData.err)
        end
    end
end
-- function OnRedPackReliveTimesReply(msgId, tMsgData)
--     if tMsgData ~= nil then
--         M.LeftTimes = tMsgData.times
--     end
-- end

protobuf:registerMessageScriptHandler(protoIdSet.sc_redpack_relive_reply, "OnRedPackReliveReply")
-- protobuf:registerMessageScriptHandler(protoIdSet.sc_redpack_relive_times, "OnRedPackReliveTimesReply")


UI.Controller.UIManager.RegisterLuaWinFunc("RedConditionWin", OnCreateCallBack, OnDestoryCallBack)

M.Data = data
-- 返回当前模块
return M
