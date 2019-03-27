-- -----------------------------------------------------------------


-- *
-- * Filename:    ExchangeWinMono.lua
-- * Summary:     兑换奖励
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/16/2017 5:58:13 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ExchangeWinMono")



-- 界面名称
local wName = "ExchangeWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr")
local _itemMgr = IMPORT_MODULE("ItemMgr")
local _sureCtrl = IMPORT_MODULE("ExchangeSureWinController")


----------
local _tabs = {}
local _tabIndex = 0;
local _tabNames = {
    [1] = { name = "exchangeTab1", icon = "tab1" },
    [2] = { name = "exchangeTab2", icon = "tab2" },
    [3] = { name = "exchangeTab3", icon = "tab3" },
    [4] = { name = "exchangeTab4", icon = "tab4" },
    [5] = { name = "exchangeTab5", icon = "tab5" },
}

local _showType = 1 --- 1.显示商店，2.显示背包
local _currList;
local _go

---------

local _winBg
local _titleSpr
local _btnClose
local _addressUrl
local _cashLb
local _tabGrid
local _btnChange
local _shopContainer
local _shopScrollView
local _shopScrollView_mgr
local _bagContainer
local _bagScrollView
local _bagScrollView_mgr
local _tabCell
local _bagEmpty
local _help
local _redBagNumLb
local _timerFun = {}
--- [ALD END]



--- 更新炒票显示
function OnExchangeResourceUpdate(msgId, value)
    if value == 1 then
        if _cashLb == nil then
            return
        end
        _cashLb.text = _platformMgr.GetCash();
    elseif value == 109 then
        if _redBagNumLb == nil then
            return
        end
        local redBagNum = _itemMgr.GetItemNum(109);
        if redBagNum % 10 == 0 then
            _redBagNumLb.text = string.format("%d",redBagNum/10);
        else
            _redBagNumLb.text = string.format("%.1f",redBagNum/10);
        end

    end
end


--- desc:更新列表显示
-- YQ.Qu:2017/3/17 0017
-- @param isReset Scroll位置是否重置
-- @return
local function UpdateList(isReset)
    local list;
    if _showType == 1 then --- 商店
        _shopScrollView_mgr:ClearCells();
        list = CTRL.GetExchangeShop(_tabIndex)
        local len = #list;
        _currList = list;
        for i = 1, len do
            _shopScrollView_mgr:NewCellsBox(_shopScrollView_mgr.Go)
        end
        _shopScrollView_mgr.Grid:Reposition();
        _shopScrollView_mgr:UpdateCells();

        if isReset then
            _shopScrollView:ResetPosition();
        end
    else --- 背包
        _bagScrollView_mgr:ClearCells();
        list = CTRL.GetRecordList(_tabIndex)
        local len = #list;
        _currList = list;
        _bagEmpty:SetActive(len == 0);
        for i = 1, len do
            _bagScrollView_mgr:NewCellsBox(_bagScrollView_mgr.Go)
        end
        _bagScrollView_mgr.Grid:Reposition();
        _bagScrollView_mgr:UpdateCells();

        if isReset then
            _bagScrollView:ResetPosition();
        end
    end
end

--- desc:tab显示
-- YQ.Qu:2017/3/17 0017
-- @param tab
-- @param i
-- @param isFirst
-- @return
local function SetTabShow(tab, i, isFirst)
    local spr = tab:GetComponent("UISprite");
    local lb = UnityTools.FindCo(tab.transform, "UILabel", "Label");
    if isFirst then
        local icon = UnityTools.FindCo(tab.transform, "UISprite", "Sprite");
        icon.spriteName = _tabNames[i].icon;
    end

    if spr ~= nil and lb ~= nil then
        if i == _tabIndex then
            spr.spriteName = "tabBg2"
            lb.text = LuaText.GetString(_tabNames[i].name);
        else
            spr.spriteName = "tabBg1"
            lb.text = LuaText.GetString(_tabNames[i].name);
        end
    end
end

--- desc:更新Tab显示
-- YQ.Qu:2017/3/17 0017
-- @param index
-- @param isFirst
-- @return
local function UpdateTabShow(index, isFirst)
    index = index or 1;
    isFirst = isFirst or false;
    _tabIndex = index;
    for i = 1, #_tabs do
        SetTabShow(_tabs[i], i, isFirst)
        if isFirst then
            UnityTools.AddOnClick(_tabs[i], function(go)
                UpdateTabShow(i);
            end)
        end
    end

    UpdateList(true);
end

--- desc:初始Tab
-- YQ.Qu:
function _timerFun.InitTab()
    for i = 1, 5 do
        local tab = UtilTools.AddChild(_tabGrid.gameObject, _tabCell, UnityEngine.Vector3(0, 0, 0));
        _tabs[i] = tab;
    end
    _tabs[4].gameObject:SetActive(false)
    UpdateTabShow(1, true);
    _tabGrid:Reposition()
end

local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName);
end

--- 打开地址管理界面
local function OnOpenSetAddressWin(gameObject)
    UnityTools.CreateLuaWin("AddressWin");
end

local function OnChangeScrollContainer(gameObject)
    if _showType == 1 then --切到背包
        _showType = 2
    else
        _showType = 1;
    end
    if _showType == 1 then
        _bagEmpty:SetActive(false);
        _titleSpr.spriteName = "articleExchangeTitle"
    else
        _titleSpr.spriteName = "myItemTitle"
    end

    _btnChange.spriteName = "flag" .. _showType;
    _shopContainer:SetActive(_showType == 1)
    _bagContainer:SetActive(_showType == 2)
    _help:SetActive(_showType == 1);
    _addressUrl:SetActive(_showType == 2);

    _tabIndex = 1;
    UpdateTabShow(_tabIndex);
    --    UpdateList(true);
end

local function OnShopItemShow(cellbox, index, item)
    if _currList == nil then return end
    local info = _currList[index + 1];
    if info == nil then return end

    local icon = UnityTools.FindCo(item.transform, "UISprite", "icon");
    local stateSpr = UnityTools.FindCo(item.transform, "UISprite", "state");
    local descLb = UnityTools.FindCo(item.transform, "UILabel", "desc");
    local downLb = UnityTools.FindCo(item.transform, "UILabel", "Label");
    local buyIcon = UnityTools.FindCo(item.transform, "UISprite", "btnBuy/icon");
    local btnBuy = UnityTools.FindGo(item.transform, "btnBuy");
    local btnBuyLabel = UnityTools.FindCo(item.transform, "UILabel", "btnBuy/Label");
    local slider =  UnityTools.FindCo(item.transform, "UISlider", "slider");
    local lbslider =  UnityTools.FindCo(item.transform, "UILabel", "slider/Label");
    local limit =  UnityTools.FindCo(item.transform, "UILabel", "count"); 
    UnityTools.SetCostIcon(buyIcon, info.need_item_id .. "");
    LogError("sort_id="..info.sort_id)
    if CTRL.GetStorageLeftTimeList()[info.obj_id] < 100 then
        limit.gameObject:SetActive(true)
        LogError("ss 99050="..CTRL.GetStorageLeftTimeList()[info.obj_id])
        limit.text = CTRL.GetStorageLeftTimeList()[info.obj_id]
    else
        limit.gameObject:SetActive(false)
    end
    local isShow=true
    if info.tag >= 1 and info.tag <= 3   then
        stateSpr.gameObject:SetActive(true)
        stateSpr.spriteName = "itemSellState" .. (info.tag - 1);
    else
        stateSpr.gameObject:SetActive(false);
    end
    if CTRL.GetStorageLeftTimeList()[info.obj_id] <=0 then
        isShow=false
    end
    --    icon.spriteName = "icon" .. info.obj_id;
    icon.spriteName = info.icon;
    descLb.text = info.name;
    if _tabIndex < 3 then
        descLb.text = "";
    end
    if info.need_item_id == 109  then
        if info.need_item_num%10==0 then
            btnBuyLabel.text = info.need_item_num/10;
        else
            btnBuyLabel.text = string.format("%.1f",info.need_item_num/10);
        end
        local sVal = tonumber(_redBagNumLb.text)/tonumber(btnBuyLabel.text)
        if sVal >=1 then
            slider.value = 1
            stateSpr.spriteName = "itemSellState4"
            stateSpr.gameObject:SetActive(isShow);
            
        else
            slider.value = sVal
            
        end
        lbslider.text = _redBagNumLb.text.."/"..btnBuyLabel.text
    elseif info.need_item_id == 103  then
        btnBuyLabel.text = info.need_item_num .. "";
        local hasNum =  _platformMgr.GetCash()
        if hasNum >= info.need_item_num then
            slider.value = 1
            stateSpr.spriteName = "itemSellState4"
            stateSpr.gameObject:SetActive(isShow);
        else
            slider.value = hasNum/info.need_item_num
        end
        lbslider.text = hasNum.."/"..info.need_item_num
    else
        btnBuyLabel.text = info.need_item_num .. "";
        local hasNum =  _itemMgr.GetItemNum(info.need_item_id)
        
        
        if hasNum >= info.need_item_num then
            slider.value = 1
            stateSpr.spriteName = "itemSellState4"
            stateSpr.gameObject:SetActive(isShow);
        else
            slider.value = hasNum/info.need_item_num
        end
        lbslider.text = hasNum.."/"..info.need_item_num
    end

    local vipLv = _platformMgr.GetVipLv();
    if _platformMgr.config_vip then
        downLb.text = LuaText.Format("exchangeVip", info.need_vip_level);
    else
        downLb.text = "";
    end
    UnityTools.AddOnClick(btnBuy, function(go)
        if _platformMgr.IsTouris() then
            UtilTools.BindingPhone();
            UnityTools.ShowMessage(LuaText.exchangeTourise)
            return;
        end
        if vipLv < info.need_vip_level then
            UnityTools.ShowMessage(LuaText.GetString("vipNoMatch"));
            return;
        end
        
        
        --如果没有设置地址，兑换前先设置地址
        if info.cls == 3 then
            if CTRL.IsSetDefaultList() then
                _sureCtrl.Open(info)
            else
                UnityTools.CreateLuaWin("AddressWin")
                return
            end
        end
        _sureCtrl.Open(info)
        --[[if CTRL.IsSetDefaultList() then
            _sureCtrl.Open(info);
        else
            UnityTools.CreateLuaWin("AddressWin");
        end]]
    end);
end

local function OnBagItemShow(cellbox, index, item)
    if _currList == nil then return end
    local info = _currList[index + 1];
    if info == nil then return end

    local iconSpr = UnityTools.FindCo(item.transform, "UISprite", "icon");
    local titleLb = UnityTools.FindCo(item.transform, "UILabel", "title");
    local timeLb = UnityTools.FindCo(item.transform, "UILabel", "time");
    local neeContainer = UnityTools.FindGo(item.transform, "need");
    local needLb = UnityTools.FindCo(item.transform, "UILabel", "need/num");
    local num = UnityTools.FindGo(item.transform, "numBg");
    local numLb = UnityTools.FindCo(item.transform, "UILabel", "numBg/Label");
    local spicon = UnityTools.FindCo(item.transform, "UISprite", "need/Sprite");
    local phoneContainer = UnityTools.FindGo(item.transform, "phone"); --- 话费类型的
    local btnLook = UnityTools.FindGo(item.transform, "phone/btnLook");
    local phoneNeedLb = UnityTools.FindCo(item.transform, "UILabel", "phone/Label");
    -- local btnRechange = UnityTools.FindGo(item.transform, "phone/btnRechange"); --直充
    titleLb.text = info.name;
    iconSpr.spriteName = info.icon;
    --        LogWarn("[ExchangeWinMono.OnBagItemShow]"..info.contain());
    if _tabIndex == 2 then
        -- phoneContainer:SetActive(true)
        btnLook:SetActive(info.recharge_type == 2);
        -- btnRechange:SetActive(info.recharge_type == 1)
        num:SetActive(false)

        if info.recharge_type == 2 then
            UnityTools.AddOnClick(btnLook, function(go)
                --[[if info.recharge_state == 0 then
                    UnityTools.ShowMessage(LuaText.GetString("exchangeCardLooKWait"));
                else
                    --                    UnityTools.MessageDialog(LuaText.Format("exchangeCardLooK", info.card_number, info.card_psd));
                    triggerScriptEvent(OPEN_RECHANGE_CARD_PASSWORD_WIN, info.card_number, info.card_psd);
                end]]
                local protobuf = sluaAux.luaProtobuf.getInstance();
                local req ={}
                req.rec_id = tostring(info.id)
                protobuf:sendMessage(protoIdSet.cs_prize_query_phonecard_key_req,req);
            end)
        end
    else
        num:SetActive(false)
        -- phoneContainer:SetActive(false)
        neeContainer:SetActive(true)
    end
    --- TODO 背包显示及逻辑
    timeLb.text = os.date("%Y-%m-%d %H:%M", info.second_time);
    if info.need_item_id == 103 then
        needLb.text = info.need_item_num;
        spicon.spriteName = "cash"
        spicon.width = 35;
        spicon.height = 30;
        spicon.transform.localPosition=UnityEngine.Vector3(103,0,0)
    elseif info.need_item_id == 109 then
        needLb.text = info.need_item_num/10;
        spicon.spriteName = "redBag1"
        spicon.width = 38;
        spicon.height = 32;
        spicon.transform.localPosition=UnityEngine.Vector3(103,0,0)
    end
    phoneNeedLb.text = LuaText.Format("exchangeNeed", info.need_item_num);
end

local function OnOpenHelpWin(gameObject)
    OpenHelp("exchange", 3);
end

--- [ALF END]
function _timerFun.bagScrollBind()
    _bagScrollView = UnityTools.FindCo(_go.transform, "UIScrollView", "Container/bagContainer/ScrollView")
    _bagScrollView_mgr = UnityTools.FindCoInChild(_bagScrollView, "UIGridCellMgr")
    _bagScrollView_mgr.onShowItem = OnBagItemShow
 end

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _titleSpr = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/bg/title/Sprite")

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, OnCloseHandler)

    _addressUrl = UnityTools.FindGo(gameObject.transform, "Container/bg/addressUrl")
    UnityTools.AddOnClick(_addressUrl.gameObject, OnOpenSetAddressWin)



    _tabGrid = UnityTools.FindCo(gameObject.transform, "UIGrid", "Container/tabBg/Grid")

    _btnChange = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/tabBg/btnChange")
    UnityTools.AddOnClick(_btnChange.gameObject, OnChangeScrollContainer)

    _shopContainer = UnityTools.FindGo(gameObject.transform, "Container/shopContainer")

    _shopScrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/shopContainer/ScrollView")
    _shopScrollView_mgr = UnityTools.FindCoInChild(_shopScrollView, "UIGridCellMgr")
    _shopScrollView_mgr.onShowItem = OnShopItemShow
    -- _controller.SetScrollViewRenderQueue(_shopScrollView)

    _bagContainer = UnityTools.FindGo(gameObject.transform, "Container/bagContainer")

    --[[_bagScrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/bagContainer/ScrollView")
    _bagScrollView_mgr = UnityTools.FindCoInChild(_bagScrollView, "UIGridCellMgr")
    _bagScrollView_mgr.onShowItem = OnBagItemShow]]
    -- _controller.SetScrollViewRenderQueue(_bagScrollView)

    _tabCell = UnityTools.FindGo(gameObject.transform, "tabCell")

    _bagEmpty = UnityTools.FindGo(gameObject.transform, "Container/scrollBg/empty")

    _help = UnityTools.FindGo(gameObject.transform, "Container/bg/help")
    UnityTools.AddOnClick(_help.gameObject, OnOpenHelpWin)


    gTimer.registerOnceTimer(100,_timerFun.bagScrollBind)
    --- [ALB END]
end

 function _timerFun.InitAllScroll()

    local shopScroll = UnityTools.FindGo(_go.transform, "Container/shopContainer/ScrollView");
    _controller:SetScrollViewRenderQueue(shopScroll);
    local bagScroll = UnityTools.FindGo(_go.transform, "Container/bagContainer/ScrollView");
    _controller:SetScrollViewRenderQueue(bagScroll);
 end
 ---显示钞票及红包（货币）
 function _timerFun.InitResource()
     if _cashLb == nil then
         _cashLb = UnityTools.FindCo(_go.transform, "UILabel", "Container/cash/Label")
     end
     if _redBagNumLb == nil then
         _redBagNumLb = UnityTools.FindCo(_go.transform, "UILabel", "Container/redBag/Label")
     end

     _cashLb.text = _platformMgr.GetCash();
     local redBagNum = _itemMgr.GetItemNum(109)
     if redBagNum % 10 == 0 then
         _redBagNumLb.text = string.format("%d",redBagNum/10);
     else
         _redBagNumLb.text = string.format("%.1f",redBagNum/10);
     end
  end
function OnUpdateExchangeList()
    UpdateList(false)
end
local function Awake(gameObject)
    _go = gameObject;
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

    registerScriptEvent(UPDATE_MAIN_WIN_RED, "OnUpdateExchangeList")
    registerScriptEvent(EVENT_RESCOURCE_UDPATE, "OnExchangeResourceUpdate")
end


local function Start(gameObject)
    UnityTools.OpenAction(_winBg);
    gTimer.registerOnceTimer(50,_timerFun.InitResource)
    gTimer.registerOnceTimer(100, _timerFun.InitAllScroll)
    gTimer.registerOnceTimer(150,_timerFun.InitTab)
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(EVENT_RESCOURCE_UDPATE, "OnExchangeResourceUpdate")
    unregisterScriptEvent(UPDATE_MAIN_WIN_RED, "OnUpdateExchangeList")
    gTimer.removeTimer(_timerFun.InitAllScroll)
    gTimer.removeTimer(_timerFun.InitResource)
    gTimer.removeTimer(_timerFun.InitTab)
    _timerFun = nil
    CLEAN_MODULE("ExchangeWinMono")
end




-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


M.UpdateList = UpdateList


-- 返回当前模块
return M
