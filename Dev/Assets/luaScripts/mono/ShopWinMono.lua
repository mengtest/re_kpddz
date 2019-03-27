-- -----------------------------------------------------------------


-- *
-- * Filename:    ShopWinMono.lua
-- * Summary:     商城界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/8/2017 9:58:22 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ShopWinMono")



-- 界面名称
local wName = "ShopWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local _itemMgr = IMPORT_MODULE("ItemMgr");
local _platformMgr = IMPORT_MODULE("PlatformMgr");
local protobuf = sluaAux.luaProtobuf.getInstance();



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")


local _tabList = {
    [1] = { icon = "tab1", name = "gold" },
    [2] = { icon = "tab2", name = "diamond" },
    -- [3] = { icon = "tab3", name = "item" },
    [3] = { icon = "tab4", name = "vip" },
    [4] = { icon = "tab4", name = "vipgift" },
}
local _tabs = {}
local _tabIndex = 1
local _currList;
local _cdDownList = {}


local _winBg
local _btnClose
local _moneyLb
local _diamondLb
local _tabGrid
local _shopContainer
local _shopScrollView
local _shopScrollView_mgr
local _vipContainer
local _vipScrollView
local _vipScrollView_mgr
local _vipTipLb
local _tabCell
local _go
local _defaultLbColor = {}
local optimize = {}---优化方法
local _vipGiftObj={}


local _scrollBg
--- [ALD END]


local function OnCloseHandler(gameObject)
    if CTRL.closeOpenOtherWin ~= nil then
        CTRL.closeOpenOtherWin();
    end
    UnityTools.DestroyWin(wName)
end


local function GetNumText(num)
    local newNum=0
    num=tonumber(num)
    if num < 10000 then
        return tostring(num)
    elseif num < 100000000 then --1亿
        newNum=num/10000
        if math.floor(newNum) <10 then
            newNum = math.floor(num/1000)
            return LuaText.Format("num_wan",newNum/10)
        else
            return LuaText.Format("num_wan",math.floor(newNum))
        end
    else
        newNum=num/100000000
        if math.floor(newNum) <10 then
            newNum = math.floor(num/10000000)
            return LuaText.Format("num_yi",newNum/10)
        else
            return LuaText.Format("num_yi",math.floor(newNum))
        end
    end
end
--- desc:
-- YQ.Qu:2017/3/8 0008
local function OnCDShowHandler(cdDownList)
    local isStop = true;
    for k, v in pairs(cdDownList) do
        if v.time ~= 0 then
            isStop = false;
            v.time = v.time - 1;
            v.label.text = dwTimeToCurrentStrTime(v.time);
        end
    end
    if isStop then
        gTimer.removeTimer(OnCDShowHandler)
    end
end
local function UpdateVipGiftInfo()
    local vipLv= _platformMgr.GetVipLv()
    local vipConfig= LuaConfigMgr.VipConfig[tostring(vipLv+1)]
    local exp=_platformMgr.RMB()
    if vipConfig ~=nil then
        UnityTools.SetActive(_vipGiftObj.slider.gameObject,true)
        _vipGiftObj.spVip.spriteName = "v"..tostring(vipLv+1)
        _vipGiftObj.num.text = LuaText.Format("shop_desc7",exp,vipConfig.need_gold)
        _vipGiftObj.desc.text =  LuaText.Format("shop_desc6",tonumber(vipConfig.need_gold)-exp)
        _vipGiftObj.spVip.transform.localPosition = UnityEngine.Vector3(_vipGiftObj.desc.width + 3,1,0)
        if vipConfig.need_gold ~=0 then
            _vipGiftObj.slider.value = exp/vipConfig.need_gold
        end
    else
        UnityTools.SetActive(_vipGiftObj.slider.gameObject,false)
    end
end
--- desc:显示商店界面
-- YQ.Qu:2017/3/8 0008
local function ShowShopContainer(isRefresh)
    if isRefresh == nil then
        isRefresh = false;
    end

    if isRefresh == false then return end;
    UnityTools.SetActive(_vipGiftObj.obj,false)
    UnityTools.SetActive(_shopContainer,true)
    UnityTools.SetActive(_vipContainer,false)
    UnityTools.SetActive(_scrollBg,true)
--    _shopContainer:SetActive(true);
--    _vipContainer:SetActive(false);
    --    LogWarn("[ShopWinMono.ShowShopContainer]" .. _tabIndex);
    local list = _itemMgr.GetShopListByType(_tabIndex);
    _currList = list;
    if list ~= nil then
        _cdDownList = {}
        _shopScrollView_mgr:ClearCells();
        local len = #list;

        -- 审核版本屏蔽内容
        if version.VersionData.IsReviewingVersion() then
            len = 2
            LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
            LogError(" 审核版本：控制充值档位的个数")
        end

        if _shopScrollView_mgr.Go == nil then
            LogWarn("[ShopWinMono.ShowShopContainer]空");
        end

        for i = 1, len do
            _shopScrollView_mgr:NewCellsBox(_shopScrollView_mgr.Go);
        end
        _shopScrollView_mgr.Grid:Reposition();
        _shopScrollView_mgr:UpdateCells();
        _shopScrollView:ResetPosition()
        gTimer.removeTimer(OnCDShowHandler)
        gTimer.registerDelayTimerEvent(1000, 1000, OnCDShowHandler, _cdDownList);
    end
end
local function ShowVipGiftainer(isRefresh)
    if isRefresh == nil then
        isRefresh = false;
    end

    if isRefresh == false then return end;
    UpdateVipGiftInfo()
    UnityTools.SetActive(_vipGiftObj.obj,true)
    UnityTools.SetActive(_scrollBg,false)
    local list = _itemMgr.GetShopListByTypeWithoutZero(8);
    _currList = list;
    if list ~= nil then
        _cdDownList = {}
        _vipGiftObj.cellGridMgr:ClearCells();
        local len = #list;
        if _vipGiftObj.cellGridMgr.Go == nil then
            LogWarn("[ShopWinMono.ShowShopContainer]空");
        end
        for i = 1, len do
            _vipGiftObj.cellGridMgr:NewCellsBox(_vipGiftObj.cellGridMgr.Go);
        end
        _vipGiftObj.cellGridMgr.Grid:Reposition();
        _vipGiftObj.cellGridMgr:UpdateCells();
        _vipGiftObj.scrollView:ResetPosition()
        gTimer.removeTimer(OnCDShowHandler)
        gTimer.registerDelayTimerEvent(1000, 1000, OnCDShowHandler, _cdDownList);
    end
    
end 
--- desc:显示VIP界面
-- YQ.Qu:2017/3/8 0008
local function ShowVipContainer()
--    if _vipContainer.activeSelf then return end;
    UnityTools.SetActive(_scrollBg,true)
    UnityTools.SetActive(_vipGiftObj.obj,false)
    UnityTools.SetActive(_vipContainer,true)
    UnityTools.SetActive(_shopContainer,false)
    local list = LuaConfigMgr.ShopVipConfig;
    --    PrintTable(list)
    _currList = list;
    if list ~= nil then
        if _vipScrollView_mgr.CellCount>0 then
            _vipScrollView_mgr:UpdateCells();
            _vipScrollView:ResetPosition();
            return;
        end
        _vipScrollView_mgr:ClearCells();
        for k, v in pairs(list) do
            _vipScrollView_mgr:NewCellsBox(_vipScrollView_mgr.Go);
        end
        _vipScrollView_mgr.Grid:Reposition();
        _vipScrollView_mgr:UpdateCells();
        _vipScrollView:ResetPosition();
    end

    --- TODO
end

--desc:设置Tab显示
--YQ.Qu:2017/3/8 0008
local function tabShowSet(tab, index, isInit)
    isInit = isInit or false;
    local spr = tab:GetComponent("UISprite");

    local iconSpr = UnityTools.FindCo(tab.transform, "UISprite", "Sprite");
    local lb = UnityTools.FindCo(tab.transform, "UILabel", "Label");
    local isSelect = index == _tabIndex;
    local lbText = LuaText.GetString("shop_tab_" .. _tabList[index].name);


    if spr ~= nil and lb ~= nil then
        if isSelect then
            spr.spriteName = "tabBg2"
            lb.text = lbText;
        else
            spr.spriteName = "tabBg1"
            lb.text = lbText ;
        end
    end
    if isInit then
        local cData = tab:GetComponent("ComponentData");
        if cData ~= nil then
            cData.Id = index;
        end
        if iconSpr ~= nil then
            iconSpr.spriteName = _tabList[index].icon;
        end
    end
end

--desc:
--YQ.Qu:2017/3/8 0008
local function UpdateTabShow()
    for i = 1, #_tabs do
        tabShowSet(_tabs[i], i);
    end
end

--desc:
--YQ.Qu:2017/3/8 0008
local function OnTabSelect(go)
    local cData = go:GetComponent("ComponentData");
    if cData ~= nil and cData.Id ~= _tabIndex then
        _tabIndex = cData.Id;
        UpdateTabShow();
        if cData.Id == 3 then
            ShowVipContainer();
        elseif cData.Id == 4 then
            ShowVipGiftainer(true)
        else
            ShowShopContainer(true);
        end
    end
end

--- desc:刷新界面
-- YQ.Qu:2017/3/8 0008
local function UpdateWin()
    if _tabIndex == 3 then
        ShowVipContainer();
    elseif _tabIndex == 4 then
        ShowVipGiftainer(true)
    else
        ShowShopContainer(true);
    end
    if gTimer.hasTimer(UpdateWin) then
        gTimer.removeTimer(UpdateWin)
    end

end

local function ChangeChina(str)
    local value = string.gsub(str, "（", "(");
    return string.gsub(value, "）", ")");
end

local function SetButtonGray(btn, btnSpr, isGray)
    local btnUI = btn:GetComponent("UIButton");
    local lbUI = UnityTools.FindCo(btn.transform, "UILabel", "Label")
    if _defaultLbColor.color == nil then
            _defaultLbColor.color = lbUI.color
            _defaultLbColor.effectColor = lbUI.effectColor
    end
    if isGray then
        
        --        boxCollider.enabled = false;
        UtilTools.SetGray(btn.transform, true);
        btnUI.isEnabled = false;
    else
        btnUI.isEnabled = true;
        UtilTools.RevertGray(btn.transform, true, true);
        if _defaultLbColor.color ~= nil then
            lbUI.color = _defaultLbColor.color
            lbUI.effectColor = _defaultLbColor.effectColor
        end
        
    end
    --    LogWarn("[ShopWinMono.SetButtonGray]"..btnSpr.spriteName .." isGray = "..tostring(isGray));
end

function optimize.SetActive(obj,value)
    local target = obj.gameObject;
    if target.activeSelf == value then
        return
    end
    target:SetActive(value)
 end


local function OnShopShowItem(cellbox, index, item)
    if _currList == nil or index > #_currList then
        return;
    end
    local info = _currList[index + 1];
    local iconSrp = UnityTools.FindCo(item.transform, "UISprite", "icon");
    local stateSpr = UnityTools.FindCo(item.transform, "UISprite", "state");
    local numLb = UnityTools.FindCo(item.transform, "UILabel", "num/Label");
    local numSpr = UnityTools.FindGo(item.transform, "num");
    local descLb = UnityTools.FindCo(item.transform, "UILabel", "desc");
    local btnBuy = UnityTools.FindGo(item.transform, "btnBuy");
    local btnBuyCollider = UnityTools.FindCo(item.transform, "BoxCollider", "btnBuy");
    local btnBuySpr = UnityTools.FindCo(item.transform, "UISprite", "btnBuy");
    local btnBuyCostLb = UnityTools.FindCo(item.transform, "UILabel", "btnBuy/Label");
    local btnBuyIcon = UnityTools.FindCo(item.transform, "UISprite", "btnBuy/icon");
    local oldLb = UnityTools.FindCo(item.transform, "UILabel", "Label");
    local itemIcon = UnityTools.FindCo(item.transform, "UISprite", "itemIcon");
    local limitLb = UnityTools.FindCo(item.transform, "UILabel", "limit");
--    numSpr:SetActive(false);
    local isIcon = string.find(info.icon, "gold") == 1 or string.find(info.icon, "diamond") == 1;
    --    LogWarn("[ShopWinMono.OnShopShowItem]"..tostring(info.icon).." isIcon =="..info.icon);

--    iconSrp.gameObject:SetActive(isIcon);
--    itemIcon.gameObject:SetActive(isIcon == false);
    optimize.SetActive(iconSrp,isIcon)
    optimize.SetActive(itemIcon,isIcon==false)
    if isIcon then
        iconSrp.spriteName = info.icon;
    else
        itemIcon.spriteName = info.icon;
    end

    numLb.text = info.item_num .. "";
    descLb.text = ChangeChina(info.name);
    if info.special_flag < 1 or info.special_flag > 3 then
        optimize.SetActive(stateSpr,false)
    else
        optimize.SetActive(stateSpr,true)
        stateSpr.spriteName = "itemSellState" .. (info.special_flag - 1);
    end
    if info.cost_list ~= nil and #info.cost_list > 0 then
        local cost = info.cost_list[1];
        btnBuyCostLb.text = UnityTools.GetShortNum(cost.cost_num * (info.discount / 100))
        btnBuyIcon.spriteName = "icon" .. cost.cost_type;
        UnityTools.AddOnClick(btnBuy, function(gObject)
            if info.vip_limit>0 and _platformMgr.GetVipLv()<info.vip_limit then
                UtilTools.ShowMessage(LuaText.GetStr(LuaText.shop_desc1, info.vip_limit), "[FF0000]")
                return
            end
            if cost.cost_type == 999 then --调用充值
                --- TODO 调用充值接口
                _platformMgr.OpenPay(info.id);

                return;
            end
            _platformMgr.MoneyIsEnough(cost.cost_type, cost.cost_num * (info.discount / 100), function()
                local req = {}
                req.id = info.id;
                protobuf:sendMessage(protoIdSet.cs_shop_buy_query, req);
            end)
        end)
        optimize.SetActive(oldLb,info.discount~=100)
        if info.discount ~= 100 then
            oldLb.text = LuaText.Format("shop_old_price", cost.cost_num);
        end
        optimize.SetActive(limitLb,info.limit_times < 99)
        if info.limit_times < 99 then
            optimize.SetActive(oldLb,false)
            limitLb.text = LuaText.Format("shop_item_limit", info.left_times);
            SetButtonGray(btnBuy, btnBuySpr, info.left_times == 0)

        else
            SetButtonGray(btnBuy, btnBuySpr, false)
            btnBuyCollider.enabled = true;
--            oldLb.gameObject:SetActive(info.discount ~= 100);
--            limitLb.gameObject:SetActive(false);
        end
    end
    if info.special_flag == 3 then --限时的话显示时间
        local serverTime = UtilTools.GetServerTime();
        if serverTime < info.end_time then
            local obj = {}
            obj.label = numLb
            obj.time = info.end_time - UtilTools.GetServerTime();
            obj.end_time = info.end_time;
            _cdDownList[index] = obj;
--            numSpr:SetActive(true);
            optimize.SetActive(numSpr,true)
            numLb.text = dwTimeToCurrentStrTime(obj.time);
        else
--            numSpr:SetActive(false)
            optimize.SetActive(numSpr,false)
            _cdDownList[index] = nil;
        end
    elseif info.item_extra_num > 0 then
--        numSpr:SetActive(true);
        optimize.SetActive(numSpr,true)
        -- local numExtra = (info.item_extra_num / info.item_num) * 100;
        -- local numExtraStr = string.format("%d", numExtra);
        numLb.text = LuaText.Format("shop_item_extra1", info.item_extra_num);
    else
--        numSpr:SetActive(false)
        optimize.SetActive(numSpr,false)
    end
end
local function SetVipEffectRenderQ(container, effect)
    UtilTools.SetEffectRenderQueueByUIParent(container, effect, 5);
end

local function OnVipShowItem(cellbox, index, item)
    if _currList == nil then
        return;
    end
    local key = (10 - index) .. "";
    local info = _currList[key];

    local headSpr = UnityTools.FindCo(item.transform, "UISprite", "head");
    local titleLb = UnityTools.FindCo(item.transform, "UILabel", "title");
    local descLb = UnityTools.FindCo(item.transform, "UILabel", "desc");
    local btnGet = UnityTools.FindGo(item.transform, "btnGet");
    local btnGray = UnityTools.FindGo(item.transform, "btnGray");
    local headTexture = UnityTools.FindCo(item.transform, "UITexture", "head/Texture");

    if info ~= nil then
        titleLb.text = info.title
        descLb.text = info.des;
        local vipLv = info.key + 0;
        local isShowGray = vipLv > _platformMgr.GetVipLv();
        btnGray:SetActive(isShowGray == false);
        btnGet:SetActive(isShowGray);

        if _platformMgr.GetVipLv() > 0 then
            _vipTipLb.text = LuaText.Format("vip_shop_tip2", _platformMgr.UserName(), _platformMgr.RMB(), _platformMgr.GetVipLv());
        else
            local needRMB = LuaConfigMgr.VipConfig["1"].need_gold + 0;
            _vipTipLb.text = LuaText.Format("vip_shop_tip", _platformMgr.UserName(), needRMB - _platformMgr.RMB());
        end
        UnityTools.AddOnClick(btnGet, function(go)
            --- TODO Vip选项的
--            UnityTools.ShowMessage("功能开发中...");
            OnTabSelect(_tabs[1])
        end)
        local headIcon = _platformMgr.GetIcon();
        UnityTools.SetPlayerHead(headIcon, headTexture, true);
        UnityTools.SetNewVipBox(headSpr.transform:Find("vip/vipBox"), vipLv,"vip", nil,0.7*0.64);
        if headIcon == nil or headIcon == "" then
            headSpr.spriteName = _platformMgr.PlayerDefaultHead()
        end
    else
        LogWarn("[ShopWinMono.OnVipShowItem]" .. (index + 1));
    end
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end
local function ResourceUpdate()
    _moneyLb.text = UnityTools.GetShortNum(_platformMgr.GetGod())
    _diamondLb.text = UnityTools.GetShortNum(_platformMgr.GetDiamond())
end


--- desc:刷新金币及钻
-- YQ.Qu:2017/3/29 0029
-- @param msgId
-- @param value
-- @return
function OnUpdateShopWinResource(msgId, value)
    ResourceUpdate();
    UpdateVipGiftInfo();
end


local function OnClickRecharge(gameObject)
    OnTabSelect(_tabs[1]);
end
local function OnShowVipGiftItem(cellbox, index, item)
    if _currList == nil or index > #_currList then
        return;
    end
    local info = _currList[index + 1];
    local tittleLb = UnityTools.FindCo(item.transform, "UILabel", "tittle");
    local stateSpr = UnityTools.FindCo(item.transform, "UISprite", "state");
    local numLb = UnityTools.FindCo(item.transform, "UILabel", "num");

    local descLb = UnityTools.FindCo(item.transform, "UILabel", "desc");
    local btnBuy = UnityTools.FindGo(item.transform, "btnBuy");
    local btnBuyCollider = UnityTools.FindCo(item.transform, "BoxCollider", "btnBuy");
    local btnBuySpr = UnityTools.FindCo(item.transform, "UISprite", "btnBuy");
    local freeobj = UnityTools.FindGo(item.transform, "btnBuy/free");
    local costObj = UnityTools.FindGo(item.transform, "btnBuy/normal");
    local btnBuyCostLb = UnityTools.FindCo(item.transform, "UILabel", "btnBuy/normal/Label");
    local btnBuyIcon = UnityTools.FindCo(item.transform, "UISprite", "btnBuy/normal/icon");
    local itemIcon = UnityTools.FindCo(item.transform, "UISprite", "itemIcon");
    local limitLb = UnityTools.FindCo(item.transform, "UILabel", "limit");
    itemIcon.spriteName = info.icon;
    numLb.text = LuaText.Format("shop_gold_desc",GetNumText(info.item_num))
    tittleLb.text = ChangeChina(info.name);
    
    if info.cost_list ~= nil and #info.cost_list > 0 then
        local cost = info.cost_list[1];
        if cost.cost_num == 0 then --free
            costObj.transform.localPosition = UnityEngine.Vector3(30000,0,0)
            freeobj.transform.localPosition = UnityEngine.Vector3(0,0,0)
            stateSpr.spriteName = "limitword"
            descLb.text = LuaText.Format("shop_desc2",info.limit_times)
            limitLb.text = LuaText.Format("shop_desc3",info.limit_times)
            UnityTools.AddOnClick(btnBuy, function(gObject)
                if info.vip_limit>0 and _platformMgr.GetVipLv()<info.vip_limit then
                    UtilTools.ShowMessage(LuaText.GetStr(LuaText.shop_desc1, info.vip_limit), "[FF0000]")
                    return
                end
                if info.left_times > 0 then
                    local req = {}
                    req.id = info.id;
                    protobuf:sendMessage(protoIdSet.cs_shop_buy_query, req);
                end
            end)
        else
            stateSpr.spriteName = "itemSellState" .. (info.special_flag - 1);
            freeobj.transform.localPosition = UnityEngine.Vector3(30000,0,0)
            costObj.transform.localPosition = UnityEngine.Vector3(0,0,0)
            descLb.text = LuaText.Format("shop_desc4",GetNumText(info.item_extra_num))
            limitLb.text = LuaText.Format("shop_desc5",info.limit_times)
            btnBuyCostLb.text = UnityTools.GetShortNum(cost.cost_num * (info.discount / 100))
            btnBuyIcon.spriteName = "icon" .. cost.cost_type;
            UnityTools.AddOnClick(btnBuy, function(gObject)
                if info.vip_limit>0 and _platformMgr.GetVipLv()<info.vip_limit then
                    UtilTools.ShowMessage(LuaText.GetStr(LuaText.shop_desc1, info.vip_limit), "[FF0000]")
                    return
                end
                if cost.cost_type == 999 then --调用充值
                    --- TODO 调用充值接口
                    _platformMgr.OpenPay(info.id);

                    return;
                end
                _platformMgr.MoneyIsEnough(cost.cost_type, cost.cost_num * (info.discount / 100), function()
                    local req = {}
                    req.id = info.id;
                    protobuf:sendMessage(protoIdSet.cs_shop_buy_query, req);
                end)
            end)
        end
        if info.left_times == 0 then
            UtilTools.SetGray(btnBuySpr.gameObject,true, true)
            btnBuyCollider.enabled = false;

        else
            UtilTools.RevertGray(btnBuySpr.gameObject,true, true)
            btnBuyCollider.enabled = true;
        end
    end

end
-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, OnCloseHandler)

    _moneyLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/money/Label")

    _diamondLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/diamond/Label")

    _tabGrid = UnityTools.FindCo(gameObject.transform, "UIGrid", "Container/tabBg/Grid")

    _shopContainer = UnityTools.FindGo(gameObject.transform, "Container/scrollBg/shopContainer")

    _shopScrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/scrollBg/shopContainer/ScrollView")
    _shopScrollView_mgr = UnityTools.FindCoInChild(_shopScrollView, "UIGridCellMgr")
    _shopScrollView_mgr.onShowItem = OnShopShowItem
    -- _controller.SetScrollViewRenderQueue(_shopScrollView)

    _vipContainer = UnityTools.FindGo(gameObject.transform, "Container/scrollBg/vipContainer")

    _vipScrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/scrollBg/vipContainer/ScrollView")
    _vipScrollView_mgr = UnityTools.FindCoInChild(_vipScrollView, "UIGridCellMgr")
    _vipScrollView_mgr.onShowItem = OnVipShowItem
    -- _controller.SetScrollViewRenderQueue(_vipScrollView)

    _vipTipLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/scrollBg/vipContainer/vipTip/Label")

    _tabCell = UnityTools.FindGo(gameObject.transform, "tabCell")

    _scrollBg = UnityTools.FindGo(gameObject.transform, "Container/scrollBg")

    _vipGiftObj.obj = UnityTools.FindGo(gameObject.transform, "Container/scrollBg2")
    _vipGiftObj.scrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/scrollBg2/shopContainer2/ScrollView")
    _vipGiftObj.cellGridMgr = UnityTools.FindCo(gameObject.transform, "UIGridCellMgr", "Container/scrollBg2/shopContainer2/ScrollView/grid")
    _vipGiftObj.cellGridMgr.onShowItem = OnShowVipGiftItem
    _vipGiftObj.slider = UnityTools.FindCo(gameObject.transform, "UISlider", "Container/scrollBg2/slider")
    _vipGiftObj.num = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/scrollBg2/slider/num") 
    _vipGiftObj.desc = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/scrollBg2/slider/desc")
    _vipGiftObj.spVip = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/scrollBg2/slider/desc/vip")
    _vipGiftObj.btnBuy = UnityTools.FindGo(gameObject.transform, "Container/scrollBg2/btnBuy")
    UnityTools.AddOnClick(_vipGiftObj.btnBuy.gameObject, OnClickRecharge)
--- [ALB END]


end

function OnBuySudccHandler(msgId,value)
    if _shopScrollView_mgr ~= nil then
        _shopScrollView_mgr:UpdateCells();
    end

 end

local function Awake(gameObject)
    _go = gameObject
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    registerScriptEvent(EVENT_RESCOURCE_UDPATE, "OnUpdateShopWinResource")
    registerScriptEvent(EVENT_SHOP_BUY_UPDATE,"OnBuySudccHandler");

    -- 审核版本屏蔽内容
    if version.VersionData.IsReviewingVersion() then
        _tabList = {
            [1] = { icon = "tab1", name = "gold" },
            [2] = { icon = "tab2", name = "diamond" }
        }

        LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
        LogError("审核版本：屏蔽商店部分功能")
    -- elseif version.VersionData.isAppStoreVersion() then  -- 苹果商店版本
    --     _tabList = {
    --         [1] = { icon = "tab1", name = "gold" },
    --         [2] = { icon = "tab2", name = "diamond" },
    --         -- [3] = { icon = "tab3", name = "item" },
	-- 		[3] = { icon = "tab4", name = "vip" },
    --     }
    end
end

local function NextWait(flag)
    if flag == 0 then
        local showScrollView = UnityTools.FindCo(_go.transform, "UIScrollView", "Container/scrollBg/shopContainer/ScrollView")
        if showScrollView ~= nil then
            _controller:SetScrollViewRenderQueue(showScrollView.gameObject);
        end
        if _vipGiftObj.scrollView ~=nil then
            _controller:SetScrollViewRenderQueue(_vipGiftObj.scrollView.gameObject);
        end
        local vipScrollView = UnityTools.FindCo(_go.transform, "UIScrollView", "Container/scrollBg/vipContainer/ScrollView")
        if vipScrollView ~= nil then
            _controller:SetScrollViewRenderQueue(vipScrollView.gameObject);
        end
    end
    gTimer.removeTimer(NextWait)
end

local function tabInitNextWait()
    _tabIndex = CTRL.CtrlData.startTab or 1;
    for i = 1, #_tabList do
        local tab = NGUITools.AddChild(_tabGrid.gameObject, _tabCell);
        _tabs[i] = tab;
        if _platformMgr.config_vip == false and i>=4 then
            tabShowSet(tab, i+1, true);
        else
            tabShowSet(tab, i, true);
        end
        UnityTools.AddOnClick(tab, OnTabSelect)
    end
    _tabGrid:Reposition();
 end



local function Start(gameObject)
    UnityTools.OpenAction(_winBg)
--    NextWait()
--    gTimer.registerOnceTimer(50,NextWait)
--    gTimer.registerOnceTimer(200,tabInitNextWait)
    if _platformMgr.config_vip == false then
        table.remove(_tabList, 4);
    end
    tabInitNextWait();
--    ResourceUpdate()
    gTimer.registerOnceTimer(100,NextWait,0)
    gTimer.registerOnceTimer(200,UpdateWin)
    gTimer.registerOnceTimer(300,ResourceUpdate)
--    gTimer.registerOnceTimer(500,ResourceUpdate)

--    _moneyLb.text = UnityTools.GetShortNum(_platformMgr.GetGod())
--    _diamondLb.text = UnityTools.GetShortNum(_platformMgr.GetDiamond())
end


local function OnDestroy(gameObject)
    CTRL.CtrlData.startTab = 1;
    _defaultLbColor = nil
    unregisterScriptEvent(EVENT_RESCOURCE_UDPATE, "OnUpdateShopWinResource")
    unregisterScriptEvent(EVENT_SHOP_BUY_UPDATE,"OnBuySudccHandler");
    gTimer.removeTimer(OnCDShowHandler)
    gTimer.removeTimer(NextWait)
    gTimer.removeTimer(UpdateWin)
    gTimer.removeTimer(ResourceUpdate)
    if _vipContainer ~= nil then
        UnityTools.RemoveDeactive(_vipContainer)
    end
    if _shopContainer ~= nil then
        UnityTools.RemoveDeactive(_shopContainer)
    end
    CLEAN_MODULE("ShopWinMono")
end



------------------------- [接口回调]-------------

---------------


-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


M.UpdateWin = UpdateWin



-- 返回当前模块
return M
