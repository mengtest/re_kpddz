-- -----------------------------------------------------------------


-- *
-- * Filename:    SubsidyWinController.lua
-- * Summary:     破产补助
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/28/2017 10:42:42 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("SubsidyWinController")
local protobuf = sluaAux.luaProtobuf.getInstance();
local _platformMgr = IMPORT_MODULE("PlatformMgr")
local _okFunc;
local _cancelFunc;
local _limitMoney = 2000;
local _subsidyMaxTime = 2; 


-- 界面名称
local wName = "SubsidyWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local UnityTools = IMPORT_MODULE("UnityTools")


local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end

--desc:补助次数上限
--YQ.Qu:2017/2/28 0028
local function SubsidyMaxTime()
    return _subsidyMaxTime;
end

--desc:破产补助
--YQ.Qu:2017/2/28 0028
local function NiuSubsidyReq()
    local req = {}
    req.type = 1;
    protobuf:sendMessage(protoIdSet.cs_niu_subsidy_req, req);
end

--desc:roomLv房间等级（1时为新手场），okFunc成功回调，cancelFunc失败回调
--YQ.Qu:2017/2/28 0028
local function Open(roomLv, okFunc, cancelFunc)
    if _platformMgr.GetGod() < _limitMoney then
        if roomLv == 1 then --新手场
            if _platformMgr.SubsidyLeftTime() > 0 then --还有补助次数
                UnityTools.CreateLuaWin(wName);
                _okFunc = okFunc;
                _cancelFunc = cancelFunc;
            else
                LogWarn("[SubsidyWinController.Open]SubsidyLeftTime = ".._platformMgr.SubsidyLeftTime());
                cancelFunc();
                --TODO 打开金币购买推荐广告界面
            end
        else
            cancelFunc();
            --TODO 打开金币购买推荐广告界面
        end
    else
        okFunc();
    end
end

local function GetSubsidySucc()
    _okFunc();
end





UI.Controller.UIManager.RegisterLuaWinFunc("SubsidyWin", OnCreateCallBack, OnDestoryCallBack)
--protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_subsidy_reply,"NiuSubisdyReply")

M.NiuSubsidyReq = NiuSubsidyReq;
M.SubsidyMaxTime = SubsidyMaxTime;
M.GetSubsidySucc = GetSubsidySucc;
M.Open = Open;

-- 返回当前模块
return M
