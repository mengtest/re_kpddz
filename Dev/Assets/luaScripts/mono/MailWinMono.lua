-- -----------------------------------------------------------------


-- *
-- * Filename:    MailWinMono.lua 
-- * Summary:     邮件界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/28/2017 4:37:32 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("MailWinMono")
local _itemMgr = IMPORT_MODULE("ItemMgr");



-- 界面名称
local wName = "MailWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance();


-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _mails;

local _mailId;
local _isOpenFirst = true;
local _rewardList;

local _winBg
local _empty
local _mail
local _mailScrollView
local _mailScrollView_mgr
local _titleLb
local _contentLb
local _btnGet
local _rewardScrollView
local _rewardScrollView_mgr
local _btnClose
local _mailist = {list = {}}
local _scrollViewContent
local _spContent

--- [ALD END]
local function UpdateMailList()
    _mailScrollView_mgr:ClearCells();
    for i = 1, #_mails do
        _mailScrollView_mgr:NewCellsBox(_mailScrollView_mgr.Go)
    end
    _mailScrollView_mgr.Grid:Reposition();
    _mailScrollView_mgr:UpdateCells();
    _mailScrollView:ResetPosition()
end

local function SetRewardList(list)
    _rewardScrollView_mgr:ClearCells();
    if list == nil or #list == 0 then
        return;
    end
    _rewardList = list;
    local len = #list;
    for i = 1, len do
        _rewardScrollView_mgr:NewCellsBox(_rewardScrollView_mgr.Go)
    end
    _rewardScrollView_mgr.Grid:Reposition();
    _rewardScrollView_mgr:UpdateCells();
end

--- desc:刷新左边
-- YQ.Qu:2017/3/14 0014
local function UpdateLeft(isResetMailId)
    isResetMailId = isResetMailId or false;
--    _mails = _itemMgr.SysMails();
    _mailist:Init()
    if isResetMailId and #_mailist.list > 0 then
        _mailId = _mailist.list[1].mail_id;
    end
    UpdateMailList();
    if isResetMailId then
        _mailScrollView:ResetPosition();
    end

    --刷新左边显示
    if #_mails == 0 then
        _empty:SetActive(true);
        _mail:SetActive(false);
    else
        _titleLb.text = _mails[1].title;
        _contentLb.text = _mails[1].content;
        if _mails[1].reward_list ~= nil then
            _btnGet:SetActive(#_mails[1].reward_list > 0);
            UnityTools.AddOnClick(_btnGet, function(go)
                CTRL.sc_mailDrawRequst(_mails[1].mail_id)
            end)
        else
            _btnGet:SetActive(false);
        end

        SetRewardList(_mails[1].reward_list);
    end
end

local function OnCloseHandler(gameObject)
    UnityTools.DestroyWin(wName)
end

local function GetTimeStr(startTime, endTime)

    local time = endTime - startTime;
    local day, hour = sec2DayHourMinSec(time);
    local timeStr = LuaText.GetString("mailTime");
    if day > 0 then
        timeStr = timeStr .. LuaText.Format("dayTime", day)
    end
    if hour > 0 then
        timeStr = timeStr .. LuaText.Format("hourTime", hour)
    end



    return timeStr;
end

--[[local function SetRewardList(list)
    _rewardScrollView_mgr:ClearCells();
    if list == nil or #list == 0 then
        return;
    end
    _rewardList = list;
    local len = #list;
    for i = 1, len do
        _rewardScrollView_mgr:NewCellsBox(_rewardScrollView_mgr.Go)
    end
    _rewardScrollView_mgr.Grid:Reposition();
    _rewardScrollView_mgr:UpdateCells();
end]]

--- 显示左边的邮件
local function UpdateRight(info)
    if info == nil then
        LogError("[MailWinMono.UpdateRight]info是空的。。。。。");
        return;
    end
    _titleLb.text = info.title;
    _contentLb.text = info.content;
    if info.reward_list ~= nil then
        _contentLb.height = 172
        _spContent.height = 172
        _scrollViewContent.gameObject:SetActive(true)
        _btnGet:SetActive(#info.reward_list > 0);
        UnityTools.AddOnClick(_btnGet, function(go)
            CTRL.sc_mailDrawRequst(info.mail_id)
        end)
    else
        _scrollViewContent.gameObject:SetActive(false)
        _contentLb.height = 357
        _spContent.height = 357
        _btnGet:SetActive(false);
    end


    SetRewardList(info.reward_list);
end

local function OnClickMail(go)
    local cData = go:GetComponent("ComponentData");
    if cData ~= nil and cData.Text ~= _mailId then
        _mailId = cData.Text;
        _mailScrollView_mgr:UpdateCells();

        UpdateRight(_mails[cData.Id + 1]);
    end
end

local function OnMailScrollView(cellbox, index, item)
    local cData = item:GetComponent("ComponentData");
    local info = _mailist.list[index + 1];
    local bgSrp = UnityTools.FindCo(item.transform, "UISprite", "bg");
    local icon = UnityTools.FindCo(item.transform, "UISprite", "icon");
    local titleLb = UnityTools.FindCo(item.transform, "UILabel", "title");
    local dateLb = UnityTools.FindCo(item.transform, "UILabel", "date");
    local timeLb = UnityTools.FindCo(item.transform, "UILabel", "time");
    local hint = UnityTools.FindGo(item.transform, "hint");
    
    if info ~= nil then
        if cData ~= nil then
            cData.Text = info.mail_id;
            cData.Id = index;
        end
        local iconName = "mailIcon1";
        if info.reward_list ~=nil and  #info.reward_list > 0 then
            iconName = "mailIcon2";
        end
        icon.spriteName = iconName;
        local timeStr = GetTimeStr(info.receive_date, info.expire_date);
        local date = os.date("%Y/%m/%d", info.receive_date);
        hint:SetActive(info.read == false);
        if info.mail_id == _mailId then
            if info.read == false then --告诉后端这个邮件已经读取了
                local req = {}
                req.mail_id = _mailId;
                protobuf:sendMessage(protoIdSet.cs_read_mail, req);
                hint:SetActive(false);
                _itemMgr.MailRead(_mailId);
            end
            dateLb.text = "[ffd178]" .. date .. "[-]";
            bgSrp.spriteName = "tabBg2";
            titleLb.text = info.title;
            timeLb.text = "[ffcabc]" .. timeStr .. "[-]";
            titleLb.effectStyle = 3--UILabel.Effect.Outline8
        else
            dateLb.text = "[e36500]" .. date .. "[-]";
            bgSrp.spriteName = "tabBg1";
            titleLb.effectStyle = 0--UILabel.Effect.None
            titleLb.text = "[904c1d]" .. info.title .. "[-]";
            timeLb.text = "[b1906e]" .. timeStr .. "[-]";
        end

        UnityTools.AddOnClick(item, OnClickMail);


        if _isOpenFirst and index == 0 then
            _isOpenFirst = false;
            UpdateRight(info);
        end
    end
end

local function OnGetHandler(gameObject)
end

local function OnRewardShowItem(cellbox, index, item)
    local info = _rewardList[index + 1];

    if info == nil then return end;

    local iconSpr = UnityTools.FindCo(item.transform, "UISprite", "icon")
    local numLb = UnityTools.FindCo(item.transform, "UILabel", "numBg/Label")
    local numBg = UnityTools.FindGo(item.transform, "numBg");
    iconSpr.spriteName = "C" .. info.base_id;
    if info.count > 1 then
        numBg.gameObject:SetActive(true);
        numLb.text = info.count;
    else
        numBg.gameObject:SetActive(false);
    end
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")



    _empty = UnityTools.FindGo(gameObject.transform, "Container/empty")

    _mail = UnityTools.FindGo(gameObject.transform, "Container/mail")

    _mailScrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/mail/scrollBg/ScrollView")
    _mailScrollView_mgr = UnityTools.FindCoInChild(_mailScrollView, "UIGridCellMgr")
    _mailScrollView_mgr.onShowItem = OnMailScrollView
    -- _controller.SetScrollViewRenderQueue(_mailScrollView)

    _titleLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/mail/content/title/Label")

    _contentLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/mail/content/Label")
    _spContent = UnityTools.FindGo(gameObject.transform, "Container/mail/content/Label/Sprite"):GetComponent("UISprite")
    _btnGet = UnityTools.FindGo(gameObject.transform, "Container/mail/content/btnGet")
    UnityTools.AddOnClick(_btnGet.gameObject, OnGetHandler)

    _rewardScrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/mail/content/scrollContainer/ScrollView")
    _rewardScrollView_mgr = UnityTools.FindCoInChild(_rewardScrollView, "UIGridCellMgr")
    _rewardScrollView_mgr.onShowItem = OnRewardShowItem
    -- _controller.SetScrollViewRenderQueue(_rewardScrollView)

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose, OnCloseHandler)
    --- [ALB END]
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    local mailScrollView = UnityTools.FindGo(gameObject.transform, "Container/mail/scrollBg/ScrollView");
    _controller:SetScrollViewRenderQueue(mailScrollView);
    local scrollView = UnityTools.FindGo(gameObject.transform, "Container/mail/content/scrollContainer/ScrollView");
    _scrollViewContent = UnityTools.FindGo(gameObject.transform, "Container/mail/content/scrollContainer")
    _controller:SetScrollViewRenderQueue(scrollView);
end

---初始化邮件
function _mailist:Init()
    local list = _itemMgr.SysMails()
    if list ~= nil and #list > 0 then
        self.list = {}
        for i = 1, #list do
            self.list[i] = list[i]    
        end
    end
 end

local function Start(gameObject)
    UnityTools.OpenAction(_winBg);
    _mails = _itemMgr.SysMails();
    if _mails == nil or #_mails > 0 then
        _mailist:Init()
        _mailId = _mailist.list[1].mail_id
        _empty:SetActive(false);
        _mail:SetActive(true);
        gTimer.registerOnceTimer(100,UpdateMailList)
--        UpdateMailList();
    else
        _empty:SetActive(true);
        _mail:SetActive(false);
    end
end

local function OnDestroy(gameObject)
    gTimer.removeTimer(UpdateMailList)
    _mailist.list = {}
    CLEAN_MODULE("MailWinMono")
end





-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy

M.UpdateLeft = UpdateLeft


-- 返回当前模块
return M
