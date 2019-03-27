-- -----------------------------------------------------------------


-- *
-- * Filename:    RedBagWinMono.lua
-- * Summary:     红包广场
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/20/2017 4:26:41 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RedBagWinMono")



-- 界面名称
local wName = "RedBagWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance();



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr")

local _tabNames = {
    [1] = "tab1",
    [2] = "tab2"
}
local _tabs = {}
local _tabIndex = 0;
local _currList;


local _winBg
local _btnClose
local _tabGrid
local _btnSendRed
local _empty
local _bagContainer
local _bagScrollView
local _bagScrollView_mgr
local _noticeContainer
local _noticeScrollView
local _noticeScrollView_mgr
local _tabCell
local _go
local _timerFun = {}
--- [ALD END]
local _btnSearch
local _searchInput
local _isInSearch=false
--- desc:显示Tab上的红点
-- YQ.Qu:2017/3/22 0022
-- @param
-- @return
local function SetTabRedNum(tab,index)
    local red = UnityTools.FindGo(tab.transform, "red");
    local redLb = UnityTools.FindCo(tab.transform, "UILabel", "red/Label");
    local redNum = 0;
    if index == 1 then
        redNum = CTRL.RedBagHint();
    else
        redNum = CTRL.NoticeRed.redNum;
    end
    
    if redNum > 0 then
        red:SetActive(true);
        redLb.text = redNum .. "";
    else
        red:SetActive(false);
    end
end

--desc:设置Tab显示
--YQ.Qu:2017/3/8 0008
local function tabShowSet(tab, index, isInit)
    isInit = isInit or false;
    local spr = tab:GetComponent("UISprite");


    local red = UnityTools.FindGo(tab.transform, "red");
    --    local redLb = UnityTools.FindCo(tab.transform, "UILabel", "red/Label");
    local lb = UnityTools.FindCo(tab.transform, "UILabel", "Label");
    local isSelect = index == _tabIndex;
    local lbText = LuaText.GetString("redBagTab" .. index);


    if spr ~= nil and lb ~= nil then
        if isSelect then
            spr.spriteName = "tabBg2"
            lb.text = lbText;
        else
            spr.spriteName = "tabBg1"
            lb.text = lbText;
        end
    end

    --- TODO 显示可领取的红包的数量
    SetTabRedNum(tab,index)
    --[[if index == 1 then
        SetTabRedNum(tab)
    elseif red.activeSelf == true then
        red:SetActive(false);
    end]]

    if isInit then
        local cData = tab:GetComponent("ComponentData");
        if cData ~= nil then
            cData.Id = index;
        end
        local iconSpr = UnityTools.FindCo(tab.transform, "UISprite", "Sprite");
        if iconSpr ~= nil then
            iconSpr.spriteName = _tabNames[index];
        end
    end
end

--- desc:更新所有Tab的显示
-- YQ.Qu:2017/3/20 0008
local function UpdateTabShow()
    for i = 1, #_tabs do
        tabShowSet(_tabs[i], i);
    end
end

--- desc:显示红包广场数据
--- YQ.Qu:2017/3/20 0008
--- @param isNoRefreshPos 不重置ScrollView的位置
local function ShowBagContainer(isNoRefreshPos)
    isNoRefreshPos = isNoRefreshPos or false;
    local list = CTRL.GetRedBagList();

    _currList = list;
    _bagScrollView_mgr:ClearCells();
    local len = #list;
    for i = 1, len do
        _bagScrollView_mgr:NewCellsBox(_bagScrollView_mgr.Go);
    end
    _bagScrollView_mgr.Grid:Reposition();
    _bagScrollView_mgr:UpdateCells();
    if isNoRefreshPos == false then
        _bagScrollView:ResetPosition();
    end
    if _empty.activeSelf ~= (len == 0) then
        _empty:SetActive(len == 0)
    end
end

--- desc:显示搜索后红包广场数据
--- YQ.Qu:2017/3/20 0008
--- @param isNoRefreshPos 不重置ScrollView的位置
local function ShowSearchBagContainer()
    isNoRefreshPos = isNoRefreshPos or false;
    local list = CTRL.GetRedBagSearchList();

    
--  LogError("INDEX="..#list)
    _currList = list;
    _bagScrollView_mgr:ClearCells();
    local len = #list;
    for i = 1, len do
        _bagScrollView_mgr:NewCellsBox(_bagScrollView_mgr.Go);
    end
    _bagScrollView_mgr.Grid:Reposition();
    _bagScrollView_mgr:UpdateCells();
    _bagScrollView:ResetPosition();
    if _empty.activeSelf ~= (len == 0) then
        _empty:SetActive(len == 0)
    end
end
--- desc:显示通知数据
-- YQ.Qu:2017/3/20 0020
-- @param isNoRefreshPos 不重置ScrollView的位置
-- @return
local function ShowNoticeContainer(isNoRefreshPos)
    isNoRefreshPos = isNoRefreshPos or false;

    local list = CTRL.GetNoticeList();
    _currList = list;
    _noticeScrollView_mgr:ClearCells();
    local len = #list;
    for i = 1, len do
        _noticeScrollView_mgr:NewCellsBox(_noticeScrollView_mgr.Go);
    end
    _noticeScrollView_mgr.Grid:Reposition();
    _noticeScrollView_mgr:UpdateCells();
    if isNoRefreshPos == false then
        _noticeScrollView:ResetPosition();
    end
    _empty:SetActive(len == 0)
end

--- desc:选择Tab
-- YQ.Qu:2017/3/20 0020
-- @param go
-- @return
local function OnTabSelect(go)

    local cData = go:GetComponent("ComponentData");
    if cData ~= nil and (cData.Id ~= _tabIndex or _isInSearch)  then
        _isInSearch=false
        _tabIndex = cData.Id;
        UpdateTabShow();
        --        _bagContainer:SetActive(_tabIndex == 1)
        --        _noticeContainer:SetActive(_tabIndex == 2)
        UnityTools.SetActive(_bagContainer, _tabIndex == 1)
        UnityTools.SetActive(_noticeContainer, _tabIndex == 2)
        if _tabIndex == 1 then
            ShowBagContainer();
            UnityTools.SetActive(_searchInput.gameObject,true)
            UnityTools.SetActive(_btnSearch.gameObject,true)
        else
            UnityTools.SetActive(_searchInput.gameObject,false)
            UnityTools.SetActive(_btnSearch.gameObject,false)
            ShowNoticeContainer();
        end
    end
end

--- desc:刷新显示
-- YQ.Qu:2017/3/20 0020
-- @param
-- @return
local function UpdateBagList()
    if _tabIndex == 1 then
        ShowBagContainer(true);
    end
end

local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

local function OnSendRedHandler(gameObject)
    UnityTools.CreateLuaWin("RedBagSendWin");
end

local function OnBagShowItem(cellbox, index, item)
    if _currList == nil or #_currList == 0 then
        return;
    end
    local info = _currList[index + 1];
    if info == nil then
        return;
    end

    local descLb = UnityTools.FindCo(item.transform, "UILabel", "Sprite/Label");
    local other = UnityTools.FindGo(item.transform, "other");
    local mine = UnityTools.FindGo(item.transform, "mine");
    local nameLb = UnityTools.FindCo(item.transform, "UILabel", "name");
    local isMine = _platformMgr.PlayerUuid() == info.player_id;
    local lbId= UnityTools.FindCo(item.transform, "UILabel", "id");
    nameLb.text = info.player_name;
    descLb.text = info.des;
    lbId.text = LuaText.Format("red_bag_win_desc1", info.uid)
    other:SetActive(isMine == false)
    mine:SetActive(isMine);

    UnityTools.AddOnClick(item, function(go)
        if isMine == false then
            local redCtrl = IMPORT_MODULE("RedBagGuessWinController");
            if redCtrl ~= nil then
                redCtrl.Open(info);
            end
        end
    end)
     
    --当前是最后一条红包数据，主动向后端请求剩下9条红包数据
    if #_currList == index + 1 and _isInSearch == false then
        CTRL.GetRedBagListFromServer(#_currList + 1);
    end
end

local function OnNoticeShowItem(cellbox, index, item)
    local value = _currList[index + 1];
    if value == nil then
        return;
    end
    local normal = UnityTools.FindGo(item.transform, "normal");
    local guess = UnityTools.FindGo(item.transform, "guess")
    if value.notice_type == 1 then
        if guess.activeSelf == true then
            guess:SetActive(false)
        end
        if normal.activeSelf == false then
            normal:SetActive(true)
        end


        local iconSpr = UnityTools.FindCo(item.transform, "UISprite", "icon");
        local titleLb = UnityTools.FindCo(item.transform, "UILabel", "normal/name");
        local timeLb = UnityTools.FindCo(item.transform, "UILabel", "normal/time");
        local numLb = UnityTools.FindCo(item.transform, "UILabel", "normal/num");
        local btnCancel = UnityTools.FindGo(item.transform, "normal/btnCancel");
        local guessName = UnityTools.FindGo(item.transform, "normal/guessName");
        local guessNameLb = UnityTools.FindCo(item.transform, "UILabel", "normal/guessName/guessName");
        local canceledLb = UnityTools.FindCo(item.transform, "UILabel", "normal/canceled");

        titleLb.text = value.content;
        timeLb.text = os.date("%Y/%m/%d", value.get_sec_time)
        numLb.text = comma_value(value.gold_num);
        if value.type == 2 then
            guessName:SetActive(true);
            btnCancel:SetActive(false);
            canceledLb.gameObject:SetActive(false);
            guessNameLb.text = value.open_player_name;
        else
            guessName:SetActive(false);
            btnCancel:SetActive(value.type == 1);
            canceledLb.gameObject:SetActive(value.type == 3);
        end
        --取消红包
        UnityTools.AddOnClick(btnCancel, function(go)
            local req = {}
            req.uid = value.notice_id;
            protobuf:sendMessage(protoIdSet.cs_red_pack_cancel_req, req);
        end)
    else
        if guess.activeSelf == false then
            guess:SetActive(true)
        end
        if normal.activeSelf == true then
            normal:SetActive(false)
        end
        
        local contentLb = UnityTools.FindCo(guess.transform, "UILabel", "Label");
        local nameLb = UnityTools.FindCo(guess.transform,"UILabel","name")
        local numLb = UnityTools.FindCo(guess.transform,"UILabel","num")
        contentLb.text = LuaText.GetStr(LuaText.redBagNoticeGuess,value.open_player_name)
        numLb.text = comma_value(value.gold_num);
        nameLb.text = value.content
        
--        LogError("[RedBagWinMono.OnNoticeShowItem]"..value.content);
        local btnSure = UnityTools.FindGo(guess.transform,"btnSure")
        local btnCancel = UnityTools.FindGo(guess.transform,"btnCancel") 
        
        UnityTools.AddOnClick(btnSure,function(go)
            ---同意给红包
            CTRL.RedPackDoSelectReq(value.notice_id,0)
         end)
         
         UnityTools.AddOnClick(btnCancel,function (go)
             ---不同意给红包
             CTRL.RedPackDoSelectReq(value.notice_id,1)
         end)
    end
end
local function OnClickSearch(gameObject)
    if _searchInput.value ~= "" then
        protobuf:sendMessage(protoIdSet.cs_red_pack_search_req, {uid=_searchInput.value})
    end
end
local function OnInputSubmit()
    if _searchInput.value ~= "" then
        protobuf:sendMessage(protoIdSet.cs_red_pack_search_req, {uid=_searchInput.value})
    end
end
--- [ALF END]
function _timerFun.bagScrollBind()
    _noticeScrollView = UnityTools.FindCo(_go.transform, "UIScrollView", "Container/noticeContainer/ScrollView")
    _noticeScrollView_mgr = UnityTools.FindCoInChild(_noticeScrollView, "UIGridCellMgr")
    _noticeScrollView_mgr.onShowItem = OnNoticeShowItem;
end

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, OnCloseHandler)



    _btnSendRed = UnityTools.FindGo(gameObject.transform, "Container/tabBg/btnSendRed")
    UnityTools.AddOnClick(_btnSendRed.gameObject, OnSendRedHandler)

    _searchInput = UnityTools.FindGo(gameObject.transform, "Container/search/back"):GetComponent("UIInput")
    _searchInput.defaultText = LuaText.GetString("red_bag_win_desc2")
    EventDelegate.Add(_searchInput.onSubmit,OnInputSubmit)
    _btnSearch = UnityTools.FindGo(gameObject.transform, "Container/search/btnsearch")
    UnityTools.AddOnClick(_btnSearch.gameObject, OnClickSearch)

    _empty = UnityTools.FindGo(gameObject.transform, "Container/scrollBg/empty")

    _bagContainer = UnityTools.FindGo(gameObject.transform, "Container/bagContainer")

    _bagScrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/bagContainer/ScrollView")
    _bagScrollView_mgr = UnityTools.FindCoInChild(_bagScrollView, "UIGridCellMgr")
    _bagScrollView_mgr.onShowItem = OnBagShowItem
    -- _controller.SetScrollViewRenderQueue(_bagScrollView)

    _noticeContainer = UnityTools.FindGo(gameObject.transform, "Container/noticeContainer")

    --[[_noticeScrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/noticeContainer/ScrollView")
    _noticeScrollView_mgr = UnityTools.FindCoInChild(_noticeScrollView, "UIGridCellMgr")
    _noticeScrollView_mgr.onShowItem = OnNoticeShowItem;]]


    gTimer.registerOnceTimer(100, _timerFun.bagScrollBind);
    --- [ALB END]
end



function OnRedBagWinUpdate(msgId, type, isRefresh)

        -- LogError("ssss2")
    if type == "redBag" and _tabIndex == 1 then
        -- LogError("ssss")
        
        ShowBagContainer(isRefresh ~= true);
    elseif type == "search" and _tabIndex == 1 then
        _isInSearch=true
        ShowSearchBagContainer()
    elseif type == "notice" and _tabIndex == 2 then
        ShowNoticeContainer(isRefresh ~= true);
        SetTabRedNum(_tabs[2],2);
    end

    SetTabRedNum(_tabs[1],1);
    
end

local function InitAllScroll()
    local bagScroll = UnityTools.FindGo(_go.transform, "Container/bagContainer/ScrollView");
    _controller:SetScrollViewRenderQueue(bagScroll);
    local noticeScroll = UnityTools.FindGo(_go.transform, "Container/noticeContainer/ScrollView");
    _controller:SetScrollViewRenderQueue(noticeScroll);
end

local function Awake(gameObject)
    _go = gameObject;
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

    

    --    gTimer.registerOnceTimer(100,InitAllScroll);
    --    UnityTools.OpenAction(_winBg);
end


local function Start(gameObject)
    UnityTools.OpenAction(_winBg);
    gTimer.registerOnceTimer(500, InitAllScroll);
    _tabGrid = UnityTools.FindGo(gameObject.transform, "Container/tabBg/Grid")
    _tabCell = UnityTools.FindGo(gameObject.transform, "tabCell")
    --    _tabIndex = 1;
    for i = 1, #_tabNames do
        local tab = NGUITools.AddChild(_tabGrid.gameObject, _tabCell);
        _tabs[i] = tab;
        tabShowSet(tab, i, true);
        UnityTools.AddOnClick(tab, OnTabSelect)
    end
    OnTabSelect(_tabs[1]);
    registerScriptEvent(RED_BAG_UPDATE, "OnRedBagWinUpdate");
end


local function OnDestroy(gameObject)
    UnityTools.RemoveDeactive(_bagContainer)
    UnityTools.RemoveDeactive(_noticeContainer)
    unregisterScriptEvent(RED_BAG_UPDATE, "OnRedBagWinUpdate");
    gTimer.removeTimer(InitAllScroll);
    gTimer.removeTimer(_timerFun.bagScrollBind)
    CLEAN_MODULE("RedBagWinMono")
end



-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


M.UpdateBagList = UpdateBagList


-- 返回当前模块
return M
