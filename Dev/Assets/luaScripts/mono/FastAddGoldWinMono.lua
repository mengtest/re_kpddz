-- -----------------------------------------------------------------


-- *
-- * Filename:    FastAddGoldWinMono.lua
-- * Summary:     补充金币
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/21/2017 4:49:38 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("FastAddGoldWinMono")



-- 界面名称
local wName = "FastAddGoldWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
local platformMgr = IMPORT_MODULE("PlatformMgr")
local roomMgr = IMPORT_MODULE("roomMgr")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _maskBtn
local _closeBtn
local _enterAction
local _titleDesc
local _mainMoney
local _extraDesc
local _btnNum
local _btnDisNum
local _buyBtn
--- [ALD END]


local _currentShopKey = 0







local function ClickbuyBtnCall(gameObject)
    platformMgr.OpenPay(tonumber(_currentShopKey))
end

--- [ALF END]




local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

local function closeByAction(gameObject)
    -- _enterAction:Play(false)
    -- gTimer.registerOnceTimer(240, CloseWin, gameObject)
    CloseWin(gameObject)
end

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _maskBtn = UnityTools.FindGo(gameObject.transform, "Container/mask")
    UnityTools.AddOnClick(_maskBtn.gameObject, closeByAction)

    _closeBtn = UnityTools.FindGo(gameObject.transform, "Container/Win/closeBtn")
    UnityTools.AddOnClick(_closeBtn.gameObject, closeByAction)

    _enterAction = UnityTools.FindCo(gameObject.transform, "TweenScale", "Container/Win")

    _titleDesc = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/Win/label_0")

    _mainMoney = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/Win/money")

    _extraDesc = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/Win/extra/desc")

    _btnNum = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/Win/buyBtn/price")

    _btnDisNum = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/Win/buyBtn/Label")

    _buyBtn = UnityTools.FindGo(gameObject.transform, "Container/Win/buyBtn")
    UnityTools.AddOnClick(_buyBtn.gameObject, ClickbuyBtnCall)

--- [ALB END]









end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    local rType = tonumber(roomMgr.RoomType())
    local gType = tonumber(roomMgr.GetGameType()) * 10
    -- LogError(tostring(4000 + gType + rType) .. "  " .. gType)
    -- UnityTools.HasObj(LuaConfigMgr.BaseShopItemConfig)
    _currentShopKey = tostring(4000 + gType + rType)
    local shopItem = LuaConfigMgr.BaseShopItemConfig[_currentShopKey]
    _titleDesc.text = shopItem.name
    _mainMoney.text = UnityTools.GetLongNumber(shopItem.item_num)
    _extraDesc.text = LuaText.GetStr(LuaText.fast_add_gold_tip, shopItem.item_extra_num)
    _btnNum.text = tostring(shopItem.cost_list[1][2]) .. ".00"
    _btnDisNum.text = LuaText.GetStr(LuaText.fast_add_gold_btn_tip, tonumber(shopItem.cost_list[1][2]) * 2)
    
end


local function Start(gameObject)
    registerScriptEvent(EVENT_GAME_START_EFFECT, CloseWin)
    _enterAction:Play(true)
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(EVENT_GAME_START_EFFECT, CloseWin)
    CLEAN_MODULE("FastAddGoldWinMono")
end




-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
