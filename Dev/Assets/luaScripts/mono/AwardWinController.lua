-- -----------------------------------------------------------------


-- *
-- * Filename:    AwardWinController.lua
-- * Summary:     奖励通用界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/4/2017 10:42:16 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("AwardWinController")
local UnityTools = IMPORT_MODULE("UnityTools")

local _rewardInfos;
local _isGameChange;

local showData = {}
local IsClose = true;

-- 界面名称
local wName = "AwardWin"
M.AwardCount = 0
local closeInt = 0;
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OpenWait()
    local b = UnityTools.CreateLuaWin(wName)

    LogWarn("[AwardWinController.OpenWait]是否创建奖励界面成功。。。。。 === "..tostring(b));
    if b==false then
        UnityTools.DestroyWin(wName)
        showData = {}
        IsClose = true
        gTimer.removeTimer(OpenWait)
    end
end

function AwardWinController(msgId, value)
    showData = {}
    IsClose = true
    gTimer.removeTimer(OpenWait)
end

registerScriptEvent(EXIT_CLEAR_ALL_DATA, "AwardWinController")

local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
    if gTimer.hasTimer(OpenWait) then
        return
    end
    IsClose = true;
    closeInt = closeInt + 1;
    LogWarn("[AwardWinController.OnDestoryCallBack]closeInt === "..closeInt);
    if M.GetShowDataLen() > 0 then
        --        LogWarn("[AwardWinController.OnDestoryCallBack]再次打开奖励界面" .. M.AwardCount);
        --[[if UnityTools.IsWinShow(wName) then
            LogWarn("[AwardWinController.OnDestoryCallBack]奖励界面还存在");
        end]]
        LogWarn("[AwardWinController.OnDestoryCallBack]sssssssssssss-------------->"..closeInt.." showDataLen = "..#showData);
        IsClose = false
        gTimer.removeTimer(OpenWait)
        gTimer.registerOnceTimer(300,OpenWait)
    end
end

local function GetAwardInfo()
    return _rewardInfos;
end

local function IsGameChange()
    return _isGameChange
end

local function GetShowData()
    if #showData > 0 then
        local data = showData[1]
        table.remove(showData, 1)
        return data
    end
    return nil
end

local function GetShowDataLen()
    return #showData
end

local function DelayCreateWin()
    UnityTools.CreateLuaWin(wName)
end


function ShowAwardWin(rewardInfo, isGameChange)
    local ctrl = IMPORT_MODULE("AwardWinController");
    ctrl.AwardCount = ctrl.AwardCount + 1;
    gTimer.removeTimer(DelayCreateWin)

    isGameChange = isGameChange or false;
    if rewardInfo ~= nil and #rewardInfo > 0 then
        UnityTools.PlaySound("Sounds/lingquwupin");
        table.insert(showData, { data = rewardInfo, isGameChange = isGameChange })
        if UnityTools.IsWinShow(wName) then
        elseif IsClose then
            IsClose = false;
            local b = UnityTools.CreateLuaWin(wName);
            LogWarn("[AwardWinController.ShowAwardWin] no create win "..tostring(b));
            if b == false then
                gTimer.registerOnceTimer(300,OpenWait)
            end
        end
    end
end

--- 获得了金币
--- @param num 金币数量
--- @param type 资源类型（默认是金币101）
function ShowAward_Monoey(num, type)
    type = type or 101;
    num = tonumber(num);
    local rewardInfo = {}
    local info = {}
    info.base_id = type;
    info.count = num;
    rewardInfo[1] = info;
    ShowAwardWin(rewardInfo);
end




UI.Controller.UIManager.RegisterLuaWinFunc("AwardWin", OnCreateCallBack, OnDestoryCallBack)

M.AwardInfo = GetAwardInfo;
M.IsGameChange = IsGameChange;

M.GetShowData = GetShowData
M.GetShowDataLen = GetShowDataLen
-- 返回当前模块
return M
