-- -----------------------------------------------------------------


-- *
-- * Filename:    DiamondBagWinMono.lua
-- * Summary:     钻石福袋
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/19/2017 4:38:59 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("DiamondBagWinMono")



-- 界面名称
local wName = "DiamondBagWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr")

local _winBg
local _btnBuy
local _btnBuyButton
local _btnClose
local _firstBuyFlag
local _firstBuyFlag2

local _buyTip
local _firstObj
local _normalObj
--- [ALD END]


local function OnBuyHandler(gameObject)
    -- if version.VersionData.isAppStoreVersion() then
    --     UnityTools.MessageDialog("由于苹果官方限制，购买福袋请移步至官方公众号《就是牛OL》内购买！", {});
    -- end
    if CTRL.data.isFirstBuy == false then
        _platformMgr.OpenPay(70002)
    else
        _platformMgr.OpenPay(70002)
    end
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnBuy = UnityTools.FindGo(gameObject.transform, "Container/Texture/btnBuy")
    _btnBuyButton = UnityTools.FindCo(gameObject.transform, "UIButton", "Container/Texture/btnBuy")
    UnityTools.AddOnClick(_btnBuy.gameObject, OnBuyHandler)


    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

    -- _firstBuyFlag = UnityTools.FindGo(gameObject.transform, "Container/Texture/flag")
    -- _firstBuyFlag2 = UnityTools.FindGo(gameObject.transform, "Container/Texture/flag2")
    _buyTip = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/Texture/btnBuy/buyTip")

    _firstObj = UnityTools.FindGo(gameObject.transform, "Container/first")

    _normalObj = UnityTools.FindGo(gameObject.transform, "Container/normal")

--- [ALB END]


end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
end

local function UpdateShow()
    -- UnityTools.SetActive(_firstBuyFlag, CTRL.data.isFirstBuy == false)
    -- UnityTools.SetActive(_firstBuyFlag2, CTRL.data.isFirstBuy == true)
    -- if CTRL.data.todayIsBuy then
    --     _btnBuyButton.normalSprite = "btn_gray_big"
    --     _buyTip.text = LuaText.diamond_bag_buy_lb3
    -- end
    if CTRL.data.todayIsBuy then
        UnityTools.SetActive(_normalObj.gameObject,true)
        UnityTools.SetActive(_firstObj.gameObject,false)
    else
        UnityTools.SetActive(_normalObj.gameObject,false)
        UnityTools.SetActive(_firstObj.gameObject,true)
    end
end

function OnWinUpdate(id, value)
    UpdateShow()
end

local function Start(gameObject)
    registerScriptEvent(DIAMOND_BAG_UPDATE, "OnWinUpdate")
    UpdateShow()
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(DIAMOND_BAG_UPDATE, "OnWinUpdate")
    CLEAN_MODULE("DiamondBagWinMono")
end


local function OnEnable(gameObject)
end


local function OnDisable(gameObject)
end




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
