-- -----------------------------------------------------------------


-- *
-- * Filename:    NormalCowRedUIMono.lua
-- * Summary:     看牌红包UI
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/9/2017 11:23:30 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("NormalCowRedUIMono")



-- 界面名称
local wName = "NormalCowRedUI"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local _platformMgr = IMPORT_MODULE("PlatformMgr");

-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _itemMgr = IMPORT_MODULE("ItemMgr")
local _btnDiaAdd
local _btnExRed
local _btnGoldAdd
local _lbGold
-- local _tipObj
local _poolNum
local _rate
local _btnDiamondBag
local _redObj
local _flag1
local _flag2
local _flag3
--- [ALD END]

local _go











local function OnClickAddDiamond(gameObject)
    local shopCtrl = IMPORT_MODULE("ShopWinController")
    if shopCtrl ~= nil then
        shopCtrl.CtrlData.startTab = 2
    end
    UnityTools.CreateLuaWin("ShopWin");
end

local function OnClickExchangeRed(gameObject)
    UnityTools.CreateLuaWin("ExchangeWin")
end

local function OnClickAddGold(gameObject)
    local shopCtrl = IMPORT_MODULE("ShopWinController")
    if shopCtrl ~= nil then
        shopCtrl.CtrlData.startTab = 1
    end
    UnityTools.CreateLuaWin("ShopWin");
end

local function OnClickOpenDiamondBag(gameObject)
    UnityTools.CreateLuaWin("DiamondBagWin");
end

--- [ALF END]







local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _btnDiaAdd = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/playerCell1/name/money/add")
    UnityTools.AddOnClick(_btnDiaAdd.gameObject, OnClickAddDiamond)

    _btnExRed = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/playerCell1/name/quan/add")
    UnityTools.AddOnClick(_btnExRed.gameObject, OnClickExchangeRed)

    _btnGoldAdd = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/playerCell1/gold/add")
    UnityTools.AddOnClick(_btnGoldAdd.gameObject, OnClickAddGold)

    _lbGold = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/playerCell1/gold"):GetComponent("UILabel")

    -- _tipObj = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn9/tip"):GetComponent("TweenAlpha")

    _poolNum = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn9/num5"):GetComponent("UILabel")

    _rate = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn9/num"):GetComponent("UILabel")
    
    _btnDiamondBag = UnityTools.FindGo(gameObject.transform, "Container/bg/topLayer/btn9")
    UnityTools.AddOnClick(_btnDiamondBag.gameObject, OnClickOpenDiamondBag)
    _redObj = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/playerCell1/name/quan/add/red")

    _flag1 = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/playerCell1/flag"):GetComponent("UIPanel")

    _flag2 = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/playerCell2/flag"):GetComponent("UIPanel")

    _flag3 = UnityTools.FindGo(gameObject.transform, "Container/playerLayer/playerCell3/flag"):GetComponent("UIPanel")

--- [ALB END]












end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    _go = gameObject
    AutoLuaBind(gameObject)
end
function OnCowRedResourceUpdate()
    _lbGold.text = UnityTools.GetShortNum(_platformMgr.GetGod())
    local num = CTRL.poolNum/10
    _poolNum.text = LuaText.Format("item_107",num)
    local rateNum =  math.floor(num/6)
    _rate.text =LuaText.Format("normal_red_tip4",rateNum)  
    -- if rateNum >=1 then
    --     _tipObj.gameObject:SetActive(true)
    --     _tipObj:PlayForward()
    -- else
    --     _tipObj.gameObject:SetActive(false)
    --     _tipObj:ResetToBeginning()
    -- end
    local redBagNum = _itemMgr.GetItemNum(109);
    if redBagNum >=20 then
        UnityTools.SetActive(_redObj.gameObject,true)
    else
        UnityTools.SetActive(_redObj.gameObject,false)
    end
end
local function ResetRedWinRenderQ(go)
    if UnityTools.IsWinShow(wName) == false then return nil end
    if _go == nil then return end
    local stRender=_go:GetComponent("UIPanel").startingRenderQueue
    _flag3.startingRenderQueue =  stRender+ 100
    _flag1.startingRenderQueue =  stRender+ 100
    _flag2.startingRenderQueue =  stRender+ 100
end
local function Start(gameObject)
    triggerScriptEvent(NORMAL_COW_RED_UI_WIN_START, gameObject)
    local redBagNum = _itemMgr.GetItemNum(109);
    if redBagNum >=20 then
        UnityTools.SetActive(_redObj.gameObject,true)
    else
        UnityTools.SetActive(_redObj.gameObject,false)
    end
    ResetRedWinRenderQ(nil)
    registerScriptEvent(EVENT_RESCOURCE_UDPATE, "OnCowRedResourceUpdate")
    registerScriptEvent(COW_RED_PACK_UPDATE, "OnCowRedResourceUpdate")
    OnCowRedResourceUpdate()
    UnityTools.ShowMessage("连续游戏5局，即可领取红包，100%中奖！")
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("NormalCowRedUIMono")
    unregisterScriptEvent(EVENT_RESCOURCE_UDPATE, "OnCowRedResourceUpdate")
    unregisterScriptEvent(COW_RED_PACK_UPDATE, "OnCowRedResourceUpdate")
end


local function OnEnable(gameObject)

end


local function OnDisable(gameObject)

end


UI.Controller.UIManager.RegisterLuaWinRenderFunc("NormalCowRedUI", ResetRedWinRenderQ)

-- if roomMgr.RoomType == 10 then return nil end

-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy
M.OnEnable = OnEnable
M.OnDisable = OnDisable


-- 返回当前模块
return M
