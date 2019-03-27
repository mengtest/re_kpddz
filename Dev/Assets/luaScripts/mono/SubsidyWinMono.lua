-- -----------------------------------------------------------------


-- *
-- * Filename:    SubsidyWinMono.lua
-- * Summary:     破产补助
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/28/2017 10:42:42 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("SubsidyWinMono")



-- 界面名称
local wName = "SubsidyWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance();



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _winBg
local _box
local _box1
local _tipLb




--desc:
--YQ.Qu:2017/3/2 0002
local function CdCompletedGetSubsidy()
    CTRL.NiuSubsidyReq();
end



--- [ALD END]
local function OnGetSubsidyHandler(gameObject)
    gTimer.removeTimer(CdCompletedGetSubsidy);
    CdCompletedGetSubsidy();
    UnityTools.PlaySound("Sounds/clickBox");
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")


    _box = UnityTools.FindGo(gameObject.transform, "Container/box")
    UnityTools.AddOnClick(_box.gameObject, OnGetSubsidyHandler)

    _box1 = UnityTools.FindGo(gameObject.transform, "Container/box1")


    _tipLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/tip/Label")

    --- [ALB END]
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
end


local function Start(gameObject)
    local _platformMgr = IMPORT_MODULE("PlatformMgr")
    _tipLb.text = LuaText.Format("subsidy_tip", _platformMgr.SubsidyLeftTime(), CTRL.SubsidyMaxTime())
    UnityTools.OpenAction(_winBg);
    gTimer.registerOnceTimer(1300, CdCompletedGetSubsidy);
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("SubsidyWinMono")
    gTimer.removeTimer(CdCompletedGetSubsidy);
end



local function effectClose()
    CTRL.GetSubsidySucc();
    CloseWin(nil);
end

function NiuSubisdyReply(msgId, tMsgData)
    if tMsgData == nil then
        return
    end
    if tMsgData.result == 0 then
        --TODO 添加领取成功的效果
        --        UnityTools.ShowMessage("领取成功");
        local _platformMgr = IMPORT_MODULE("PlatformMgr")
        _tipLb.text = LuaText.Format("subsidy_tip", _platformMgr.SubsidyLeftTime(), CTRL.SubsidyMaxTime())
        _box:SetActive(false);
        _box1:SetActive(true);
        local rewardInfo = {}
        local info = {}
        info.base_id = 101;
        info.count = _platformMgr.SubsidyGold();
        rewardInfo[1] = info;
        ShowAwardWin(rewardInfo);
        gTimer.registerOnceTimer(2000, effectClose)
        UnityTools.PlaySound("Sounds/boxOpen")
    else
        UnityTools.ShowMessage(tMsgData.err);
        CloseWin(nil)
    end
end


-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy

protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_subsidy_reply, "NiuSubisdyReply")
-- 返回当前模块
return M
