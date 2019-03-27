-- -----------------------------------------------------------------


-- *
-- * Filename:    LuckyCowWinMono.lua
-- * Summary:     招财金牛
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/6/2017 11:26:24 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("LuckyCowWinMono")
local protobuf = sluaAux.luaProtobuf.getInstance();


-- 界面名称
local wName = "LuckyCowWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)




-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _data; --- 返回的数据


local _winBg
local _btnClose
local _moneyLb
local _cowLvLb
local _cowLvLb2
local _btnGet
local _leftTimeLb
local _btnGo
local _btnGray
local _topTipLb
local _hand1
local _index1 = 0
local _index2 = 61
local _isMinus = false
local _passTime = 0
local _hand2
local _bClick = false
local _lbTip2
local _lbTip3
local _bMissionComplete= false
--- [ALD END]




local function SendGetMessage()
    local req = {}
    req.key = _data.lv;
    protobuf:sendMessage(protoIdSet.cs_golden_bull_draw_req, req);

    UnityTools.SetActive(_hand2.gameObject,false)
    UnityTools.SetActive(_hand1.gameObject,true)
    _index1 = 0
    _passTime = 0
    _isMinus = false
    _bClick = false
end
local function OnGetHandler(gameObject)
    UnityTools.SetActive(_hand2.gameObject,true)
    UnityTools.SetActive(_hand1.gameObject,false)
    _index2 = 61
    _passTime = 0
    _isMinus = false
    _bClick = true 
end
local function SetOpenShop(isOpen)
    local shopCtrl = IMPORT_MODULE("ShopWinController");
    if shopCtrl ~= nil then
        if isOpen then
            shopCtrl.closeOpenOtherWin = function()
                UnityTools.CreateLuaWin("LuckyCowWin");
            end
        else
            shopCtrl.closeOpenOtherWin = nil;
        end
    end
end

local function OnGoRechangeHandler(gameObject)
    SetOpenShop(true);
    GoTo(105, wName);
end

local function OnGrayHandler(gameObject)
    if _bMissionComplete then
        UnityTools.ShowMessage(LuaText.luckyCowTomorrow);
    else
        UnityTools.ShowMessage(LuaText.luckyCowTomorrow2);
    end
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

    _moneyLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/money/Label")

    _cowLvLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/lv/Label")
    _cowLvLb2 = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/lv/Label2")
    _btnGet = UnityTools.FindGo(gameObject.transform, "Container/btnGet")
    UnityTools.AddOnClick(_btnGet.gameObject, OnGetHandler)

    _leftTimeLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/leftTime")

    _btnGo = UnityTools.FindGo(gameObject.transform, "Container/btnGo")
    UnityTools.AddOnClick(_btnGo.gameObject, OnGoRechangeHandler)

    _btnGray = UnityTools.FindGo(gameObject.transform, "Container/btnGray")
    UnityTools.AddOnClick(_btnGray.gameObject, OnGrayHandler)

    _topTipLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/Label")

        _hand1 = UnityTools.FindGo(gameObject.transform, "Container/cow/hand/s1"):GetComponent("UISprite")

    _hand2 = UnityTools.FindGo(gameObject.transform, "Container/cow/hand2/s1"):GetComponent("UISprite")
    UnityTools.SetActive(_hand2.gameObject,false)
    _lbTip2 = UnityTools.FindGo(gameObject.transform, "Container/tip2"):GetComponent("UILabel")

    _lbTip3 = UnityTools.FindGo(gameObject.transform, "Container/tip3"):GetComponent("UILabel")

--- [ALB END]




end

local function getTimeStr()
    local cDate = os.date("*t", UtilTools.GetServerTime())
    local time = "";
    if 24 - cDate.hour > 0 then
        time = time .. LuaText.GetStr(LuaText.hourTime, 24 - cDate.hour);
    end
    if 60 - cDate.min > 0 then
        time = time .. LuaText.GetStr(LuaText.minuteTime, 60 - cDate.min);
    end
    return LuaText.GetStr(LuaText.luckyCowLeftTime, time);
end

function UpadateLuckyCowLeftTimeLb()
    _leftTimeLb.text = getTimeStr();
end

local function UpdateWin()
    if _data.isOpen then
        _lbTip2.text =""
        -- _topTipLb.text = LuaText.luckyCowSmallTip;
        _btnGo:SetActive(false);
        
        _leftTimeLb.text = getTimeStr();
        if _data.leftTime == 0 then
            gTimer.registerRepeatTimer(1000 * 60, UpadateLuckyCowLeftTimeLb);
        else
            gTimer.removeTimer(UpadateLuckyCowLeftTimeLb);
        end
        if _bMissionComplete then
            _btnGray:SetActive(_data.leftTime == 0);
            _btnGet:SetActive(_data.leftTime > 0);
            _leftTimeLb.gameObject:SetActive(_data.leftTime == 0);
            -- _lbTip3.text = ""
        else
            _leftTimeLb.gameObject:SetActive(_data.leftTime == 0);
            _btnGray:SetActive(true);
            _btnGet:SetActive(false);
        end
    else
        _btnGo:SetActive(true);
        _btnGray:SetActive(false);
        _btnGet:SetActive(false);
        _lbTip2.text = LuaText.GetStr(LuaText.luckyCowSmallTip2, _data.openNeed)
    end
    if _data.lv == 1 then
        _moneyLb.text = LuaConfigMgr.GodGoldConfig["1"].get_gold;
    elseif _data.cfg ~= nil then
        _moneyLb.text = _data.cfg.get_gold;
    end
end

local function UpdateCowLv()
    --[[if _data.lv > 1 then
        _cowLvLb.text = LuaText.GetStr(LuaText.cowLv, _data.lv - 1);
    else
        _cowLvLb.text = LuaText.GetStr(LuaText.cowLv, _data.lv);
    end]]
    _cowLvLb.text = LuaText.GetStr(LuaText.cowLv, _data.lv);
    _cowLvLb2.text = LuaText.GetStr(LuaText.cowLv, _data.lv);
end

--- desc:
--- YQ.Qu
function OnLuckyCowWinUpdate(msgId, type, data)
    if type ~= "mission" then
        _data = data
    end
    if _data == nil then
        LogError("333")
        return
    end
    if _data.lv == 1 then
            _bMissionComplete = CTRL.Process >=tonumber(LuaConfigMgr.GodGoldConfig["1"].gold_total)
            if _bMissionComplete then
                _lbTip3.text = LuaText.Format("luckyCowSmallTip3",LuaConfigMgr.GodGoldConfig["1"].gold_total,LuaConfigMgr.GodGoldConfig["1"].gold_total)
            else
                _lbTip3.text = LuaText.Format("luckyCowSmallTip3",CTRL.Process,LuaConfigMgr.GodGoldConfig["1"].gold_total)
            end
    elseif _data.cfg ~= nil then
        
        _bMissionComplete = CTRL.Process >=tonumber(_data.cfg.gold_total)
        if _bMissionComplete then
            _lbTip3.text = LuaText.Format("luckyCowSmallTip3",_data.cfg.gold_total,_data.cfg.gold_total)
        else
            _lbTip3.text = LuaText.Format("luckyCowSmallTip3",CTRL.Process,_data.cfg.gold_total)
        end
    end
    UpdateCowLv();
    UpdateWin();
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

    SetOpenShop(false);

end
local function Update()
    _passTime = _passTime + gTimer.deltaTime();
    if _passTime >=0.15 then
        _passTime = 0
        if not _bClick then
            _hand1.spriteName = "shouman_00".._index1
            if _index1 >=3 then
                _isMinus = true
            elseif _index1 <= 0 then
                _isMinus = false
            end
            if _isMinus then
                _index1 = _index1 - 1
            else    
                _index1 = _index1 + 1
            end
        else
            _hand2.spriteName = "shouliandong_0".._index2
            if _index1 >=64 then
                _isMinus = true
            elseif _index1 <= 61 then
                SendGetMessage()
            end
            if _isMinus then
                _index1 = _index1 - 1
            else    
                _index1 = _index1 + 1
            end
        end
        
    end
    
end

local function Start(gameObject)
    UnityTools.OpenAction(_winBg);
    _data = CTRL.Data;
    registerScriptEvent(UPDATE_LUCKY_COW_WIN, "OnLuckyCowWinUpdate")
    if _data ~= nil then
        if _data.lv == 1 then
            
            _bMissionComplete = CTRL.Process >=tonumber(LuaConfigMgr.GodGoldConfig["1"].gold_total)
            if _bMissionComplete then
                _lbTip3.text = LuaText.Format("luckyCowSmallTip3",LuaConfigMgr.GodGoldConfig["1"].gold_total,LuaConfigMgr.GodGoldConfig["1"].gold_total)
            else
                _lbTip3.text = LuaText.Format("luckyCowSmallTip3",CTRL.Process,LuaConfigMgr.GodGoldConfig["1"].gold_total)
            end
            
        elseif _data.cfg ~= nil then
            
            _bMissionComplete = CTRL.Process >=tonumber(_data.cfg.gold_total)
            if _bMissionComplete then
                _lbTip3.text = LuaText.Format("luckyCowSmallTip3",_data.cfg.gold_total,_data.cfg.gold_total)
            else
                _lbTip3.text = LuaText.Format("luckyCowSmallTip3",CTRL.Process,_data.cfg.gold_total)
            end
        end
        
        UpdateCowLv();
        UpdateWin();
    end
end


local function OnDestroy(gameObject)
    gTimer.removeTimer(UpadateLuckyCowLeftTimeLb);
    unregisterScriptEvent(UPDATE_LUCKY_COW_WIN, "OnLuckyCowWinUpdate")
    CLEAN_MODULE("LuckyCowWinMono")
end



-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.Update = Update

-- 返回当前模块
return M
