-- -----------------------------------------------------------------


-- *
-- * Filename:    TaskWinMono.lua
-- * Summary:     任务面板
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/13/2017 5:34:31 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("TaskWinMono")



-- 界面名称
local wName = "TaskWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance();



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _itemMgr = IMPORT_MODULE("ItemMgr");

local _winBg
local _btnClose
local _tabGrid
local _ScrollView
local _ScrollView_mgr
local _tabCell
local _taskList = {};
local _config
local _taskTab = { [1] = "newTask", [2] = "dailyTask", [3] = "weekTask", [4] = "timeTask" ,[5] = "diamondTask"}
local _tabs = {};
local _go


local _tabIndex = 0;
--- [ALD END]
local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

--- desc:更新Tab显示
-- YQ.Qu:2017/3/13 0013
local function UpdateTabShow()
    for i = 1, #_tabs do
        local tab = _tabs[i];
        local tabSpr = tab:GetComponent("UISprite");
        local tabLb = UnityTools.FindCo(tab.transform, "UILabel", "sel");
        local tabLb2 = UnityTools.FindCo(tab.transform, "UILabel", "unsel");
        local red = UnityTools.FindGo(tab.transform, "red")
        local redLb = UnityTools.FindCo(tab.transform, "UILabel", "red/Label");
        local key = tonumber(_tabs[i].transform.name)
        if key == 5 then key = 6 end
        local taskCompleteNum = _itemMgr.GetTaskCompleteByType(key)
        if key == 6 then key = 5 end
        
        tabLb.text = LuaText.GetString(_taskTab[key])
        tabLb2.text = LuaText.GetString(_taskTab[key])
        if key == _tabIndex then
            tabSpr.spriteName = "tabSelect"
            UnityTools.SetActive(tabLb.gameObject,true)
            UnityTools.SetActive(tabLb2.gameObject,false)
        else
            tabSpr.spriteName = ""
            UnityTools.SetActive(tabLb.gameObject,false)
            UnityTools.SetActive(tabLb2.gameObject,true)
        end
        if taskCompleteNum > 0 then
            red:SetActive(true);
            redLb.text = taskCompleteNum .. "";
        else
            red:SetActive(false);
        end
    end
end

--- desc:
-- YQ.Qu:2017/3/13 0013
local function GetCurrentConfig()
    if _tabIndex == 2 then
        return LuaConfigMgr.DailyTaskConfig;
    elseif _tabIndex == 3 then
        return LuaConfigMgr.WeekTaskConfig;
    elseif _tabIndex == 4 then
        return LuaConfigMgr.TimeTaskConfig;
    end
    return LuaConfigMgr.NewTaskConfig;
end
local function GetCurrentConfigById(id)
    if id < 510000 then
        return LuaConfigMgr.NewTaskConfig;
    elseif id < 520000 then
        return LuaConfigMgr.DailyTaskConfig;
    elseif id < 530000 then
        return LuaConfigMgr.WeekTaskConfig;
    elseif id < 540000 then
        return LuaConfigMgr.TimeTaskConfig;
    end
end


--- desc:更新列表显示
-- YQ.Qu:2017/3/13 0013
local function UpdateList()
    if _tabIndex==5 then
        _taskList = _itemMgr.GetMissionListByType(6);
        table.sort(_taskList, function(a, b)
            if a.state == b.state then
                if a.order_mark+0==b.order_mark + 0 then
                    return a.id + 0 < b.id + 0;
                else
                    return a.order_mark+0<b.order_mark + 0 
                end
                
            elseif a.state == 1 then
                return true
            elseif a.state == 2 then
                return false
            else
                if b.state == 1 then
                    return false
                else
                    return true
                end
            end
        end)        
    else
        _taskList = _itemMgr.GetMissionListByType(_tabIndex);

        if _tabIndex == 1 then
            local i = 0;
            while(i < #_taskList) do
                i = i + 1;
                if _taskList[i].id == 500004 then
                    table.remove(_taskList, i);
                    i= #_taskList;
                end
            end
        end

        table.sort(_taskList, function(a, b)
            if a.state == b.state then
                return a.id + 0 < b.id + 0;
            elseif a.state == 1 then
                return true
            elseif a.state == 2 then
                return false
            else
                if b.state == 1 then
                    return false
                else
                    return true
                end
            end
        end)
    end

    
    _ScrollView_mgr:ClearCells();

    if _taskList == nil then
        return;
    end

    local len = #_taskList;
    for i = 1, len do
        _ScrollView_mgr:NewCellsBox(_ScrollView_mgr.Go)
    end
    _ScrollView_mgr.Grid:Reposition();
    _ScrollView_mgr:UpdateCells();
end

local function UpdateWin()
    UpdateList();
    UpdateTabShow();
end
local function GetSplitNumStr(num)
    if num >= 1000000 then
        return math.floor(num/1000000)..","..string.format("%03d",math.floor((num%1000000)/1000))..","..string.format("%03d",num%1000000%1000)
    elseif num >=1000 then
        return math.floor(num/1000)..","..string.format("%03d",num%1000)
    else 
        return tostring(num)
    end
end
local function OnShowItem(cellbox, index, item)
    --    LogWarn("[TaskWinMono.OnShowItem]index = "..index);
    local info = _taskList[index + 1];
    if info == nil then
        return
    end
    if _tabIndex == 5 then
        _config = GetCurrentConfigById(info.id)
    end
    local cfgData = _config[info.id .. ""];
    if cfgData == nil then
        return
    end

    local titleLb = UnityTools.FindCo(item.transform, "UILabel", "title");
    local descLb = UnityTools.FindCo(item.transform, "UILabel", "desc");
    local iconSpr = UnityTools.FindCo(item.transform, "UISprite", "icon");
    local itemNumLb = UnityTools.FindCo(item.transform, "UILabel", "itemNumBg/Label");
    local slider = UnityTools.FindCo(item.transform, "UISlider", "slider");
    local sliderLb = UnityTools.FindCo(item.transform, "UILabel", "slider/Label");
    local btnGo = UnityTools.FindGo(item.transform, "btnGo");
    local btnGet = UnityTools.FindGo(item.transform, "btnGet");
    local btnGray = UnityTools.FindGo(item.transform, "btnGray");
    local btnGrayLb = UnityTools.FindCo(item.transform, "UILabel", "btnGray/Label")
    local getedSpr = UnityTools.FindGo(item.transform, "geted");
    getedSpr:SetActive(info.state == 2);
    btnGray:SetActive(false);
    titleLb.text = cfgData.title;
    descLb.text = cfgData.desc;
    if cfgData.diamond_mark == "1" then
        iconSpr.spriteName = "dia";
    else
        iconSpr.spriteName = "icon";
    end
    --    iconSpr.spriteName = cfgData.icon;
    itemNumLb.text = GetSplitNumStr(tonumber(cfgData.item1_num));
    btnGrayLb.text = LuaText.GetString("get");
    if _tabIndex == 4 then
        slider.gameObject:SetActive(false);
        btnGo:SetActive(false);
        local currDate = os.date("*t", UtilTools.GetServerTime());
        local min = cfgData.parameter1 + 0;
        local max = cfgData.parameter2 + 0;
        if (currDate.hour >= min and currDate.hour < max) and info.state < 2 then
            btnGray:SetActive(false);
            btnGet:SetActive(true);
            UnityTools.AddOnClick(btnGet, function(go)
                --领取奖励
                local req = {}
                req.id = info.id
                protobuf:sendMessage(protoIdSet.cs_draw_mission_request, req);
            end)
        else
            if info.state == 2 then
                btnGrayLb.text = LuaText.GetString("geted")
            end
            btnGet:SetActive(false);
            btnGray:SetActive(info.state == 0);
        end
        return;
    else
        local target = cfgData.parameter3 + 0;
        if target > 1 then
            local sliderValue = 0;
            if info.count > 0 then
                sliderValue = info.count / target;
            end
            slider.value = sliderValue;
            local count = info.count;
            if info.count >= target then
                count = target;
            end
            sliderLb.text = count .. "/" .. target;

            if info.state == 2 then
                slider.value = 1;
                sliderLb.text = target .. "/" .. target;
            end
            slider.gameObject:SetActive(true);
        else
            slider.gameObject:SetActive(false);
        end
    end


    btnGet:SetActive(info.state == 1);
    btnGo:SetActive(info.state == 0);
    if CTRL.IsNoShowGo() then
        btnGo:SetActive(false);
    end
    if info.state == 1 then
        --        SetBtnGray(btnGet, false);
        UnityTools.AddOnClick(btnGet, function(go)
            --领取奖励
            local req = {}
            req.id = info.id
            protobuf:sendMessage(protoIdSet.cs_draw_mission_request, req);
        end)
    end
    if info.state == 0 then
        UnityTools.AddOnClick(btnGo, function(go)
            --前往
            GoTo(cfgData.skip_id, "TaskWin");
        end)
    end
end

--- desc:
-- YQ.Qu:2017/3/13 0013
local function OnChangeTab(go)
    local index = go.transform.name + 0;
    if index == _tabIndex then
        return;
    end
    _tabIndex = index;
    _config = GetCurrentConfig();
    UpdateList();
    _ScrollView:ResetPosition();
    UpdateTabShow();
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

    _tabGrid = UnityTools.FindCo(gameObject.transform, "UIGrid", "Container/tabGrid")

    _ScrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/scrollBg/ScrollView")
    _ScrollView_mgr = UnityTools.FindCoInChild(_ScrollView, "UIGridCellMgr")
    _ScrollView_mgr.onShowItem = OnShowItem
    -- _controller.SetScrollViewRenderQueue(_ScrollView)

    _tabCell = UnityTools.FindGo(gameObject.transform, "tabCell")

    --- [ALB END]
end

local function InitAllScroll()
    local scrollView = UnityTools.FindCo(_go.transform, "UIScrollView", "Container/scrollBg/ScrollView");
    _controller:SetScrollViewRenderQueue(scrollView.gameObject);
      local scrollView2 = UnityTools.FindCo(_go.transform, "UIPanel", "Container/Panel");
    _controller:SetScrollViewRenderQueue(scrollView2.gameObject);
    scrollView2.startingRenderQueue =scrollView2.startingRenderQueue+ 10;
end

local function Awake(gameObject)
    _go = gameObject
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

    --[[local scrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/scrollBg/ScrollView");
    _controller:SetScrollViewRenderQueue(scrollView.gameObject);]]
end

local function InitTab()
    if CTRL.TabIndex <= #_tabs and CTRL.TabIndex > 0 then
        OnChangeTab(_tabs[CTRL.TabIndex].gameObject)
    else
        OnChangeTab(_tabs[1].gameObject)
    end
end

--- 是否这个标签下的所有任务都完成了
local function IsAddTaskTab(key)
    if key==5 then key = 6 end
    local taskList = _itemMgr.GetMissionListByType(key)
    if taskList == nil then
        return false
    end
    local completedCount = 0
    for i = 1, #taskList do
        if taskList[i].state == 2 then
            completedCount = completedCount + 1;
        end
    end
    return #taskList ~= completedCount
end

local function Start(gameObject)
    UnityTools.OpenAction(_winBg);
    registerScriptEvent(EVENT_GAME_START_EFFECT, CloseWin)
    local delCount = 0
    for i = 1, #_taskTab do
        if IsAddTaskTab(i) then
            local tab = UtilTools.AddChild(_tabGrid.gameObject, _tabCell, UnityEngine.Vector3(0, 0, 0));
            if tab ~= nil then
                tab.transform.name = i;
                UnityTools.AddOnClick(tab.gameObject, OnChangeTab)
                _tabs[#_tabs + 1] = tab;
                --            _tabs[i] = tab;
            end
        else
            delCount=delCount+1
        end
    end
    CTRL.TabIndex=CTRL.TabIndex-delCount
    gTimer.registerOnceTimer(50, InitAllScroll)
    gTimer.registerOnceTimer(100, InitTab)
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(EVENT_GAME_START_EFFECT, CloseWin)
    gTimer.removeTimer(InitTab)
    gTimer.removeTimer(InitAllScroll)
    CLEAN_MODULE("TaskWinMono")
end





-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.UpdateWin = UpdateWin
M.UpdateList = UpdateList



-- 返回当前模块
return M
