-- -----------------------------------------------------------------


-- *
-- * Filename:    FreeWinMono.lua
-- * Summary:     免费界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/4/2017 3:47:33 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FreeWinMono")



-- 界面名称
local wName = "FreeWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr")

local _listData = {}


local _winBg
local _btnClose
local _scrollView
local _scrollView_mgr
local _go
--- [ALD END]
local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName);
end

local function TaskIsCompelete(key)
    if key == "600003" then --破产补助

        if _platformMgr.GetGod() < 2000 and _platformMgr.SubsidyLeftTime() > 0 then
            return 1;
        elseif _platformMgr.SubsidyLeftTime() == 0 then
            return 2;
        end
    elseif key == "600005" then --绑定手机
        if CTRL.Data().isDraw == false and _platformMgr.IsTouris() == false then
            return 1;
        elseif CTRL.Data().isDraw then
            return 2
        end
    end
    return 0;
end

local function OnShowItem(cellbox, index, item)
    local info = _listData[index + 1];
    local nameLb = UnityTools.FindCo(item.transform, "UILabel", "name");
    local descLb = UnityTools.FindCo(item.transform, "UILabel", "desc");
    local btnGet = UnityTools.FindGo(item.transform, "btnGet");
    local btnGetLb = UnityTools.FindCo(item.transform, "UILabel", "btnGet/Label");
    local btnGo = UnityTools.FindGo(item.transform, "btnGo");
    local btnGoLb = UnityTools.FindCo(item.transform, "UILabel", "btnGo/Label");
    local btnGotRed = UnityTools.FindGo(item.transform, "btnGet/red");
    local iconSpr = UnityTools.FindCo(item.transform, "UISprite", "icon");
    local btnLook = UnityTools.FindGo(item.transform, "btnLook");
    local btnLookLb = UnityTools.FindCo(item.transform, "UILabel", "btnLook/Label");
    if info ~= nil then
        iconSpr.spriteName = info.icon;

        --        LogWarn("[FreeWinMono.OnShowItem]" .. info.icon .. " title = " .. info.title .. " key = " .. info.key   );
        descLb.text = info.desc;
        local isComplete = TaskIsCompelete(info.key);
        if info.key == "600003" then
            local subsidyCtrl = IMPORT_MODULE("SubsidyWinController");
            nameLb.text = LuaText.Format("free_subsidy_name", subsidyCtrl.SubsidyMaxTime(), _platformMgr.SubsidyLeftTime());
        else
            nameLb.text = "[904c1d]" .. info.title .. "[-]";
        end

        if info.skip_id == info.skip_id2 then --查看任务
            btnLook:SetActive(true);
            btnGo:SetActive(false);
            btnGet:SetActive(false);
            btnLookLb.text = info.skip_name;
            UnityTools.AddOnClick(btnLook, function(go)
                GoTo(info.skip_id)
            end)
        else
            btnLook:SetActive(false);
            btnGet:SetActive(isComplete > 0);
            btnGo:SetActive(isComplete == 0 and info.skip_id ~= 0);

            if isComplete == 1 then --满足条件
                btnGotRed:SetActive(info.red == "1");
                btnGetLb.text = info.skip1_name;
                if info.skip_id1 == "0" then
                    UnityTools.AddOnClick(btnGet, function(go)
                        GoTo(info.skip_id);
                    end)
                else
                    UnityTools.AddOnClick(btnGet, function(go)
                        GoTo(info.skip_id1);
                    end)
                end
            elseif isComplete == 0 then --未满足
                btnGoLb.text = info.skip_name;
                UnityTools.AddOnClick(btnGo, function(go)
                    GoTo(info.skip_id);
                end)
            elseif isComplete == 2 then --已完成
                btnGotRed:SetActive(false);
                btnGetLb.text = info.skip2_name;
                UnityTools.AddOnClick(btnGet, function(go)
                    GoTo(info.skip_id2);
                end)
            end
        end
    end
end

--- desc:
-- YQ.Qu:2017/3/10 0010
local function UpdateList()
    local config = LuaConfigMgr.FreeConfig
    if config ~= nil then

        _scrollView_mgr:ClearCells();
        --[[for k, v in pairs(config) do
            local item = _scrollView_mgr:NewCellsBox(_scrollView_mgr.Go)
            --            LogWarn("[FreeWinMono.UpdateList]"..k..type(item.parent));
            if item ~= nil then
                --                local cData = UnityTools.FindCo(item,"ComponentData","cell");
                local cData = item:GetComponent("ComponentData");
                if cData ~= nil then
                    cData.Text = v.key;
                end
            end
        end]]

        for i = 1, #_listData do
            _scrollView_mgr:NewCellsBox(_scrollView_mgr.Go)
        end
        _scrollView_mgr.Grid:Reposition();
        _scrollView_mgr:UpdateCells();
        --        _scrollView_mgr:ClearCells();
    end
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, OnCloseHandler)

    _scrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/scrollBg/ScrollView")
    _scrollView_mgr = UnityTools.FindCoInChild(_scrollView, "UIGridCellMgr")
    _scrollView_mgr.onShowItem = OnShowItem
    -- _controller.SetScrollViewRenderQueue(_scrollView)

    --- [ALB END]
end

local function InitAllScroll()
    local scrollView = UnityTools.FindGo(_go.transform, "Container/scrollBg/ScrollView");
    _controller:SetScrollViewRenderQueue(scrollView);
    local scrollView2 = UnityTools.FindGo(_go.transform, "Container/Panel"):GetComponent("UIPanel")
    _controller:SetScrollViewRenderQueue(scrollView2.gameObject);
    scrollView2.startingRenderQueue =scrollView2.startingRenderQueue+ 10;
end

local function Awake(gameObject)
    _go = gameObject
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
end

--- 根据表的Id排序
local function sortConfig()
    local config = LuaConfigMgr.FreeConfig
    if config ~= nil then
        for k, v in pairs(config) do
            if CTRL.IsShowBindingPhone(k) then --不是游客不显示手机绑定
            else
                -- 审核版本屏蔽内容
                if version.VersionData.IsReviewingVersion() then
                    if "600002" ~= k then
                        _listData[#_listData + 1] = v;
                    end

                    LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
                    LogError("审核版本：屏蔽登录奖励任务")
                else
                    _listData[#_listData + 1] = v;
                end
            end
        end
    end

    table.sort(_listData, function(a, b)
        return (a.key + 0) < (b.key + 0);
    end)
end

local function UpdateBinding()
    _listData = {}
    sortConfig()
    UpdateList()
 end

local function Start(gameObject)
    --[[local scrollView = UnityTools.FindGo(gameObject.transform, "Container/scrollBg/ScrollView");
    _controller:SetScrollViewRenderQueue(scrollView);]]
    UnityTools.OpenAction(_winBg);
    sortConfig();

    --    UpdateList();

    gTimer.registerOnceTimer(100, InitAllScroll)
    gTimer.registerOnceTimer(200, UpdateList)
end




local function OnDestroy(gameObject)
    gTimer.removeTimer(InitAllScroll)
    gTimer.removeTimer(UpdateList)
    CLEAN_MODULE("FreeWinMono")
end



-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.UpdateBinding = UpdateBinding


-- 返回当前模块
return M
