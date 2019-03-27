-- -----------------------------------------------------------------


-- *
-- * Filename:    PlayerInfoWinMono.lua
-- * Summary:     玩家信息界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/22/2017 10:14:17 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("PlayerInfoWinMono")



-- 界面名称
local wName = "PlayerInfoWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr")
local _itemMgr = IMPORT_MODULE("ItemMgr")
local _ctrl = IMPORT_MODULE("PlayerInfoWinController")

local _tabList = {}
local _tabIndex = 0
local _btnClose
local _flag_openBagFirst = true; --是否为刚刚打开背包
local _bagList = {}
local _timerFun = {}
local _bWxLogin = false;


local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

local _go
local _baseContainer
local _winBg
local _btnChangePassWord
local _sexShow
local _sexShowLb
local _sexShowSpr
local _playerExp
local _playerExpLb
local _btnChangePlayerInfo
local _btnCommitPlayerInfo
local _moneyLb
local _diamondLb
local _infoWinLb
local _infoMaxMoneyLb
local _totalProfitLb
local _weekProfitLb
local _niuTenNumLb
local _bombNumLb
local _fiftyNumLb
local _fiveSmallNumLb
local _otherNumLb
local _playerAccountLb
local _playerLvLb
local _playerShowNameLb
local _sexChange
local _sexCheck0
local _sexCheck1
local _playerNameChange
local _playerChangeInput
local _bagContainer
local _itemScrollView
local _itemScrollView_mgr
local _btnActiveVip
local _bindingPopTip
local _btnBindingPhone
local _headTexture
local _headImg
local _headDefaultImg
local _changeHeadTip
local _btnBindingID
--- [ALD END]









--- desc:显示Vip激活
-- YQ.Qu:2017/3/30 0030
-- @return nil
local function ShowActiveVipBtn()
    -- 审核版本屏蔽内容
    if version.VersionData.IsReviewingVersion() then
        LogError(" 审核版本：隐藏VIP激活")
        return
    end

    if _platformMgr.config_vip == false then
        _btnActiveVip:SetActive(false);
    else
        _btnActiveVip:SetActive(_platformMgr.GetVipLv() == 0);
    end
end



--desc:
--YQ.Qu:2017/2/22 0022
local function UpdatePlayerShowSex()
    if _sexShow.activeSelf == false then
        _sexShow:SetActive(true);
    end
    local function sexSpr(value)
        if value == 0 then
            return "boy"
        end
        return "girl"
    end

    local sex = _platformMgr.getSex()
    _sexShowLb.text = LuaText.GetString("Sex" .. sex)
    _sexShowSpr.spriteName = sexSpr(sex + 0)
end


--desc:重置玩家信息显示
--YQ.Qu:2017/2/22 0022
local function ResetPlayerInfoShow()
    _playerChangeInput.value = "";
    if not _bWxLogin then
        _btnChangePlayerInfo:SetActive(true);
    else
        _btnChangePlayerInfo:SetActive(false);
    end
    
    _btnCommitPlayerInfo:SetActive(false);
    _playerShowNameLb.gameObject:SetActive(true);
    _playerShowNameLb.text = _platformMgr.UserName();
    _playerNameChange:SetActive(false);

    UpdatePlayerShowSex();
    _sexChange:SetActive(false)
end

--desc:玩家胜负信息显示
--YQ.Qu:2017/2/22 0022
local function PlayerInfoWin()
    local win = _platformMgr.getWinNum()
    local lose = _platformMgr.getLoseNum()
    local winPercess = _platformMgr.getWinRate();

    _infoWinLb.text = LuaText.Format("playerWin", winPercess, win, lose);
    _infoMaxMoneyLb.text = LuaText.Format("playerMaxMoney", UnityTools.GetShortNum(_platformMgr.getMaxMoney()));
    _totalProfitLb.text = LuaText.Format("playerTotalProfit", UnityTools.GetShortNum(_platformMgr.getTotalProfit()));
    _weekProfitLb.text = LuaText.Format("playerWeekProfit", UnityTools.GetShortNum(_platformMgr.getWeekProfit()));

    _niuTenNumLb.text = LuaText.Format("playerNiuTenNum", _platformMgr.getNiuTenNum());
    _bombNumLb.text = LuaText.Format("playerBombNum", _platformMgr.getBombNum());
    _fiftyNumLb.text = LuaText.Format("playerFiftyNum", _platformMgr.getFiftyNum());
    _fiveSmallNumLb.text = LuaText.Format("playerFiveSmallNum", _platformMgr.getFiveSmallNum());
    _otherNumLb.text = LuaText.Format("playerOtherNum", _platformMgr.getOtherNum());

    _playerAccountLb.text = LuaText.Format("playerAccount", _platformMgr.Account());
end

--desc：更新玩家信息
--YQ.Qu:2017/2/22 0022
local function UpdatePlayerInfo()
    local lv = _platformMgr.Lv();
    local expCfg = LuaConfigMgr.PlayerLvlConfig[lv .. ""];
    _playerShowNameLb.text = _platformMgr.UserName();
    _playerLvLb.text = LuaText.Format("playerInfoLv", lv);
    if expCfg ~= nil then
        local exp = _platformMgr.Exp();
        _playerExp.value = exp / expCfg.exp;
        _playerExpLb.text = exp .. "/" .. expCfg.exp;
    else
        _playerExp.value = 0;
        _playerExpLb.text = "";
    end
    UpdatePlayerShowSex();
    _moneyLb.text = _platformMgr.GetGod()
    _diamondLb.text = _platformMgr.GetDiamond()
    PlayerInfoWin();
end




local function OnCloseWin(gameObject)
    CloseWin();
end

local function OnChangePassWord(gameObject)
    --    UnityTools.CreateLuaWin("PIChangePassWordWin", true);
    UnityTools.CreateWin("PIChangePassWordWin");
end

local function OnChangePlayerInfo(gameObject)
    gameObject:SetActive(false);
    _btnCommitPlayerInfo:SetActive(true);
    _sexShow:SetActive(false);
    _sexChange:SetActive(true);
    local sex = _platformMgr.getSex()
    _sexCheck0.value = sex == 0;
    _sexCheck1.value = sex == 1;

    local count = _itemMgr.GetItemNum(100001);
    if count > 0 then
        _playerNameChange:SetActive(true);
        _playerShowNameLb.gameObject:SetActive(false);
        _playerChangeInput.defaultText = _playerShowNameLb.text;
    end
    
end

--desc:确认花改名卡修改名字
local function SureChangeNameAndSex()
    local newSex = 0;
    if _sexCheck1.value then
        newSex = 1;
    end
    local newName = _platformMgr.UserName()
    if _playerChangeInput.value ~= "" then
        newName = _playerChangeInput.value;
    end
    _ctrl.ChangeNameAndSex(newName, newSex)
end

--提示玩家信息调整
local function OnCommitPlayerInfo(gameObject)

    local newSex = 0;
    if _sexCheck1.value then
        newSex = 1;
    end
    local newName = _platformMgr.UserName()
    if _playerChangeInput.value ~= "" then
        newName = _playerChangeInput.value;
    end

    if newName == _platformMgr.UserName() and newSex == _platformMgr.getSex() then
        _playerChangeInput.value = "";
        ResetPlayerInfoShow();
        return;
    elseif newName ~= _platformMgr.UserName() then
        --        UnityTools.MessageDialog("test",{isShowClose = true})
        --        UtilTools.ShowWaitWin(10,10000,nil);
        UnityTools.MessageDialog(LuaText.GetString("changeNameTip"), { okCall = SureChangeNameAndSex })
        return;
    elseif newSex ~= _platformMgr.getSex() then
        SureChangeNameAndSex();
        return;
    end


    _platformMgr.UpdateNameAndSex(newName, newSex);
    _playerChangeInput.value = "";
    ResetPlayerInfoShow();
end



--改名或性结束
function OnPlayerNameAndSexSucc(msgId, isShowChange)
    if isShowChange then
        UnityTools.ShowMessage(LuaText.GetString("changeNameAndSex"));
    end
    _playerChangeInput.value = "";
    _headDefaultImg.spriteName = _platformMgr.PlayerDefaultHead();
    ResetPlayerInfoShow();
end

local function ChangePlayerHead()
    local playerIcon = _platformMgr.GetIcon();
    
    if playerIcon ~= nil and playerIcon ~= "" then
        UnityTools.SetPlayerHead(_platformMgr.GetIcon(), _headTexture, true);
    end
end

--头像设置
function OnChangePlayerHeadInfo(msgId, value)
    ChangePlayerHead();
end

local function ShowItemTip(go, state)
    if go ~= nil then
        local cData = go:GetComponent("ComponentData");
        local cfgData = LuaConfigMgr.ItemBaseConfig[cData.Id .. ""];
        UnityTools.ShowPlayerInfoItemTip(go, state, cData.Id, { posX = 0.15, posY = 0.37, isShowTitle = false, contentColor = "[874c38]" });
    end
end

local function OnBagShowItem(cellbox, index, item)
    local itemShow = UnityTools.FindGo(item.transform, "item");
    local numLb = UnityTools.FindCo(item.transform, "UILabel", "item/numBg/Label");
    local itemNameBg = UnityTools.FindCo(item.transform, "UILabel", "item/itemName");
    local itemIcon = UnityTools.FindCo(item.transform, "UISprite", "item/icon");
    local tipLb = UnityTools.FindCo(item.transform, "UILabel", "item/tip/Label");
--    LogWarn("[PlayerInfoWinMono.OnBagShowItem]bag show item index ===== " .. index .. "  length=" .. #_bagList);
    if _bagList ~= nil and _bagList[index] ~= nil then
        local info = _itemMgr.GetItemByKey(_bagList[index].key);
        local cData = item:GetComponent("ComponentData");
        if info ~= nil then
            if cData ~= nil then
                cData.Text = info.uuid;
            end
            numLb.text = info.count;
        else
            numLb.text = 0;
        end
        local cfgData = _bagList[index];
        if cfgData ~= nil then
            if cData ~= nil then
                cData.Id = tonumber(cfgData.key);
            end
            itemIcon.spriteName = cfgData.icon;
            itemNameBg.text = cfgData.name;
            tipLb.text = cfgData.desc;
        end
    end
    UIEventListener.Get(item).onPress = function(go, isPressed)
        local delay = 200;
        if isPressed then
            gTimer.registerOnceTimer(delay, ShowItemTip, go, isPressed)
        else
            gTimer.removeTimer(ShowItemTip)
            ShowItemTip(go, isPressed)
        end
    end
    --    UnityTools.ShowPlayerInfoItemTip(item);
end


--desc:重置背包的显示
--YQ.Qu:2017/2/23 0023
local function ResetBagContainer()
    if _flag_openBagFirst then
        _flag_openBagFirst = false;
        _itemScrollView_mgr:ClearCells();
        local list = _itemMgr.GetBagBaseList();
        _bagList = {}
        local index = 0;
        for k, v in pairs(list) do
            _bagList[index] = v;
            index = index + 1
            _itemScrollView_mgr:NewCellsBox(_itemScrollView_mgr.Go)
        end
        if _itemScrollView_mgr.Grid ~= nil then
            _itemScrollView_mgr.Grid:Reposition();
        end
        _itemScrollView_mgr:UpdateCells();
    elseif _itemScrollView_mgr.Grid ~= nil then
        _itemScrollView_mgr.Grid:Reposition()
    end
end


local function UpdateList()
    _itemScrollView_mgr:ClearCells();
    local list = _itemMgr.GetBagBaseList();
    _bagList = {}
    local index = 0;
    for k, v in pairs(list) do
        _bagList[index] = v;
        _itemScrollView_mgr:NewCellsBox(_itemScrollView_mgr.Go)
        index = index + 1;
    end
    if _itemScrollView_mgr.Grid ~= nil then
        _itemScrollView_mgr.Grid:Reposition();
    end
    _itemScrollView_mgr:UpdateCells();
end

local function OnToActiveVip(gameObject)
    --    UnityTools.ShowMessage("功能开发中...")
    --    UnityTools.CreateLuaWin("ShopWin");
    local shopCtrl = IMPORT_MODULE("ShopWinController");
    if shopCtrl then
        shopCtrl.OpenShop(4);
    end
end

--打开绑定界面
local function OnBindingPhoneHandler(gameObject)
    --    UnityTools.ShowMessage("功能开发中...")
    --    UnityTools.CreateWin("RegisterBindingWin");
    UtilTools.BindingPhone();
end


--- desc:
--- YQ.Qu
function OnPlayerInfoBagUpdate(msgId, type)
    if type ~= "init" then
        if type == "update" then
            UpdateList();
        elseif type == "add" or type == "del" then
            UpdateList();
        end
    end
end

local function OnChangePlayerHead(gameObject)
    UnityTools.CreateLuaWin("PlayerPicSelectWin");
end

local function OnClickBindingID(gameObject)
    UnityTools.CreateLuaWin("UserIDCardBindWin");
end
local function UpdatePlayerInfoBtns()
end
--- [ALF END]

function OnUpdatePlayerWinInfo(msgID, value)
    PlayerInfoWin();
    UpdatePlayerInfoBtns();
end







-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _bWxLogin = UnityEngine.PlayerPrefs.GetString("accountServerLoginContent") ~= ""

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, OnCloseWin)


    _baseContainer = UnityTools.FindGo(gameObject.transform, "Container/baseContainer")
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnChangePassWord = UnityTools.FindGo(gameObject.transform, "Container/baseContainer/head/btnChangePassWord")
    UnityTools.AddOnClick(_btnChangePassWord.gameObject, OnChangePassWord)

    _sexShow = UnityTools.FindGo(gameObject.transform, "Container/baseContainer/sex/show")

    _sexShowLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/sex/show/Label")

    _sexShowSpr = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/baseContainer/sex/show/Sprite")

    _playerExp = UnityTools.FindCo(gameObject.transform, "UISlider", "Container/baseContainer/exp")

    _playerExpLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/exp/Label")

    _btnChangePlayerInfo = UnityTools.FindGo(gameObject.transform, "Container/baseContainer/btnChangePlayerInfo")
    UnityTools.AddOnClick(_btnChangePlayerInfo.gameObject, OnChangePlayerInfo)

    _btnCommitPlayerInfo = UnityTools.FindGo(gameObject.transform, "Container/baseContainer/btnCommitPlayerInfo")
    UnityTools.AddOnClick(_btnCommitPlayerInfo.gameObject, OnCommitPlayerInfo)


    _moneyLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/money/Label")

    _diamondLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/diamond/Label")

    _infoWinLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/info/win")

    _infoMaxMoneyLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/info/maxMoney")

    _totalProfitLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/info/totalProfit")

    _weekProfitLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/info/weekProfit")

    _niuTenNumLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/info/niuTenNum")

    _bombNumLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/info/bombNum")

    _fiftyNumLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/info/fiftyNum")

    _fiveSmallNumLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/info/fiveSmallNum")

    _otherNumLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/info/otherNum")

    _playerAccountLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/info/account")

    _playerLvLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/lv")

    _playerShowNameLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/baseContainer/name/nameLb")

    _sexChange = UnityTools.FindGo(gameObject.transform, "Container/baseContainer/sex/change")

    _sexCheck0 = UnityTools.FindCo(gameObject.transform, "UIToggle", "Container/baseContainer/sex/change/check0")

    _sexCheck1 = UnityTools.FindCo(gameObject.transform, "UIToggle", "Container/baseContainer/sex/change/check1")

    _playerNameChange = UnityTools.FindGo(gameObject.transform, "Container/baseContainer/name/change")

    _playerChangeInput = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/baseContainer/name/change/Label")

    _bagContainer = UnityTools.FindGo(gameObject.transform, "Container/bagContainer")
    _btnActiveVip = UnityTools.FindGo(gameObject.transform, "Container/baseContainer/head/btnActiveVip")
    UnityTools.AddOnClick(_btnActiveVip.gameObject, OnToActiveVip)

    -- 审核版本屏蔽内容
    if version.VersionData.IsReviewingVersion() then
        _btnActiveVip:SetActive(false)
        LogError(">>>>>>>>>>>>>>>> WARNING!!! APPLE'S REVIEWER IS COMING!!!  <<<<<<<<<<<<<<<<")
        LogError(" 审核版本：隐藏VIP激活")
    end

    _bindingPopTip = UnityTools.FindGo(gameObject.transform, "Container/baseContainer/head/popTip")

    _btnBindingPhone = UnityTools.FindGo(gameObject.transform, "Container/baseContainer/head/btnBindingPhone")
    UnityTools.AddOnClick(_btnBindingPhone.gameObject, OnBindingPhoneHandler)

    _headTexture = UnityTools.FindCo(gameObject.transform, "UITexture", "Container/baseContainer/head/headImg/Texture")

    _headImg = UnityTools.FindGo(gameObject.transform, "Container/baseContainer/head/headImg")

    
    _headDefaultImg = UnityTools.FindCo(gameObject.transform, "UISprite", "Container/baseContainer/head/headImg")
    gTimer.registerOnceTimer(100, _timerFun.bagScrollBind)
    _changeHeadTip = UnityTools.FindGo(gameObject.transform, "Container/baseContainer/head/Sprite")
    if not _bWxLogin then
        UnityTools.AddOnClick(_headImg.gameObject, OnChangePlayerHead)
        _changeHeadTip.gameObject:SetActive(true)
        _btnChangePlayerInfo:SetActive(true);
    else
        _changeHeadTip.gameObject:SetActive(false)
        _btnChangePlayerInfo:SetActive(false);
    end
    _btnBindingID = UnityTools.FindGo(gameObject.transform, "Container/baseContainer/head/btnBindingID")
    UnityTools.AddOnClick(_btnBindingID.gameObject, OnClickBindingID)

--- [ALB END]


end

function _timerFun.bagScrollBind()
    _itemScrollView = UnityTools.FindCo(_go.transform, "UIScrollView", "Container/bagContainer/ScrollView")
    _itemScrollView_mgr = UnityTools.FindCoInChild(_itemScrollView, "UIGridCellMgr")
    _itemScrollView_mgr.onShowItem = OnBagShowItem
end

--- 绑定手机成功后调用
function OnUpdatePlayerInfo(msgId, value)
    if _platformMgr.config_vip == false then
        return;
    end
    if UnityTools.IsWinShow(wName) == false then return nil end
    if CTRL.IsRealName then
        local isTourise = _platformMgr.IsTouris();
        _btnBindingPhone:SetActive(isTourise);
        _btnChangePassWord:SetActive(isTourise == false);
        _bindingPopTip:SetActive(isTourise);
        _btnBindingID.gameObject:SetActive(false)
    else
        _btnBindingPhone:SetActive(false);
        _btnChangePassWord:SetActive(false);
        _bindingPopTip:SetActive(false);
        _btnBindingID.gameObject:SetActive(true)
    end
    ShowActiveVipBtn();
end

local function HideFun()
    if CTRL.IsRealName then
        local isTourise = _platformMgr.IsTouris();
        _btnBindingPhone:SetActive(isTourise);
        _btnChangePassWord:SetActive(isTourise == false);
        _bindingPopTip:SetActive(isTourise);
        _btnBindingID.gameObject:SetActive(false)
    else
        _btnBindingPhone:SetActive(false);
        _btnChangePassWord:SetActive(false);
        _bindingPopTip:SetActive(false);
        _btnBindingID.gameObject:SetActive(true)
    end
    --TODO 绿色包不显示未开发的功能
    local config_vie = _platformMgr.config_vip;
    if config_vie == false then
        _btnBindingPhone:SetActive(config_vie);
        _bindingPopTip:SetActive(config_vie);
        _btnChangePassWord:SetActive(config_vie);
    end
end

local function Awake(gameObject)
    _go = gameObject
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

    --    _baseContainer:SetActive(true)
    registerScriptEvent(END_CHANGE_PLAYER_HEAD, "OnChangePlayerHeadInfo");
    registerScriptEvent(END_CHANGE_NAME_AND_SEX, "OnPlayerNameAndSexSucc");
    registerScriptEvent(EVENT_UPDATE_PLAYER_WIN_INFO, "OnUpdatePlayerWinInfo");
    registerScriptEvent(EVENT_RESCOURCE_UDPATE, "OnUpdatePlayerInfo");
    registerScriptEvent(UPDATE_ITEM, "OnPlayerInfoBagUpdate");
end

--desc:tab的颜色
--YQ.Qu:2017/2/22 0022
local function GetTabColor(isSelect)
    if isSelect then
        return "[ec7e2d]"
    end
    return "[6e6961]"
end

--desc:tab的状态
--YQ.Qu:2017/2/22 0022
local function GetTabSpriteName(isSelect)
    if isSelect then
        return "tabSelect"
    end
    return "tabNoSelect"
end

--desc:tab更新
--YQ.Qu:2017/2/22 0022
local function SetTabShow(parent, k, isSelect)
    local i = k - 1
    local lb = UnityTools.FindCo(parent, "UILabel", "Label")
    if lb ~= nil then lb.text = GetTabColor(isSelect) .. LuaText.GetString("playerInfoTab" .. i) .. "[-]" end
    local spr = parent:GetComponent("UISprite")
    if spr ~= nil then
        spr.spriteName = GetTabSpriteName(isSelect)
        if isSelect then
            spr.depth = 3;
            lb.transform.localPosition = UnityEngine.Vector3(-7, 0, 0)
        else
            lb.transform.localPosition = UnityEngine.Vector3(-7, -6, 0)
            spr.depth = 3;
        end
    end
end





--desc:点击Tab
--YQ.Qu:2017/2/22 0022
local function OnTabChange(gameObject)
    local cData = gameObject:GetComponent("ComponentData")

    if cData == nil or cData.Id == _tabIndex then
        return
    end
    _tabIndex = cData.Id
    for k, v in pairs(_tabList) do
        SetTabShow(_tabList[k], k, _tabIndex == k)
    end

    _baseContainer:SetActive(_tabIndex == 1);
    _bagContainer:SetActive(_tabIndex == 2);
    if _tabIndex == 1 then
        ResetPlayerInfoShow();
    elseif _tabIndex == 2 then
        gTimer.registerOnceTimer(100, ResetBagContainer)
        --        ResetBagContainer();
    end
end





local function InitAllScroll()
    local _scrollView = UnityTools.FindGo(_go.transform, "Container/bagContainer/ScrollView")
    _controller:SetScrollViewRenderQueue(_scrollView);
end

--- 分帧处理头像显示
local function PlayerHeadNextWait()
    if _headDefaultImg.spriteName ~= _platformMgr.PlayerDefaultHead() then
        _headDefaultImg.spriteName = _platformMgr.PlayerDefaultHead();
    end
    UnityTools.SetNewVipBox(_headImg.transform:Find("vip/vipBox"), _platformMgr.GetVipLv(), "vip",_go)
    ChangePlayerHead();
end


local function Start(gameObject)
    ShowActiveVipBtn();
    HideFun();
    registerScriptEvent(EVENT_GAME_START_EFFECT, CloseWin)
    UpdatePlayerInfo();
    UnityTools.OpenAction(_winBg);

    gTimer.registerOnceTimer(300, PlayerHeadNextWait)
    gTimer.registerOnceTimer(500, InitAllScroll)
end




local function OnDestroy(gameObject)
    unregisterScriptEvent(UPDATE_ITEM, "OnPlayerInfoBagUpdate");
    unregisterScriptEvent(EVENT_GAME_START_EFFECT, CloseWin)
    unregisterScriptEvent(END_CHANGE_PLAYER_HEAD, "OnChangePlayerHeadInfo");
    unregisterScriptEvent(END_CHANGE_NAME_AND_SEX, "OnPlayerNameAndSexSucc");
    unregisterScriptEvent(EVENT_UPDATE_PLAYER_WIN_INFO, "OnUpdatePlayerWinInfo");
    unregisterScriptEvent(EVENT_RESCOURCE_UDPATE, "OnUpdatePlayerInfo");
    gTimer.removeTimer(InitAllScroll)
    gTimer.removeTimer(_timerFun.bagScrollBind)
    CLEAN_MODULE(wName .. "mono");
end





-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.ChangePlayerHead = ChangePlayerHead


-- 返回当前模块
return M
