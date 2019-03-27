-- -----------------------------------------------------------------
-- * Copyright (c) 2018 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    RechargeTaskWinMono.lua
-- * Summary:     RechargeTaskWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        3/9/2018 2:48:03 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RechargeTaskWinMono")



-- 界面名称
local wName = "RechargeTaskWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local _platformMgr = IMPORT_MODULE("PlatformMgr");

-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _lbType
local _spGold
local _lbDiscount
local _lbGet1
local _lbBack1
local _lbBack2
local _scrollview
local _gridCell
local _lbHasGet
local _btnBuy
local _lbNeed
local _btnBuyed
local _btnRule
local _btnClose
local _totalGold = 0
local _lbDisCountType
--- [ALD END]






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









local function OnClickBuy(gameObject)
    _platformMgr.OpenPay(CTRL.PayId)
    
end

local function OnClickRule(gameObject)
    local retipctrl = IMPORT_MODULE("MonthCardHelpWinController")
    retipctrl.ShowTipWin(LuaText.GetString("recharge_task_desc8"))
    -- UnityTools.CreateLuaWin("RechargeTipWin")
end

--- [ALF END]


local _mainData = {}
local _configTb = {}

local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

local function UpdateWin()
    _spGold.spriteName = "gold"..tonumber(CTRL.MainData.key)%10
    _lbType.text = _mainData.type_des
    _lbGet1.text = _mainData.des
    _lbDiscount.text = _mainData.buying_des1
    _lbDisCountType.text = _mainData.buying_des2
    _lbBack1.text = GetNumText(_totalGold)
    _lbHasGet.text = LuaText.Format("recharge_task_desc4",CTRL.Process)
    _lbNeed.text = _mainData.buying_des
    if CTRL.IsOpen == 2 then
        CloseWin(nil)
    elseif CTRL.IsOpen == 0 then
        _btnBuy.gameObject:SetActive(true)
        _btnBuyed.gameObject:SetActive(false)
    else
        _btnBuy.gameObject:SetActive(false)
        _btnBuyed.gameObject:SetActive(true)
    end

end
local function UpdateList()

    local delCount = _gridCell.CellCount - #_configTb
    if delCount>=0 then
        for i=1,delCount do
            _gridCell:DelteLastNode()
        end
        _gridCell:UpdateCells()
    else
        _gridCell:ClearCells()
        for i=1,#_configTb do
            _gridCell:NewCellsBox(_gridCell.Go)    
                --mainScrollView:Reposition()
        end
        _gridCell.Grid:Reposition()
        _gridCell:UpdateCells()
    end
    
end
local function OnClickGoto(gameObject)
    GoTo(ComponentData.Get(gameObject).Id,wName)
end
local function OnClickGetAward(gameObject)
     
    local protobuf = sluaAux.luaProtobuf.getInstance()
    protobuf:sendMessage(protoIdSet.cs_task_pay_award_request, {index=ComponentData.Get(gameObject).Id}) 
end

local function OnShowItem(cellbox, index, item)
    index = index+1
    
    local lbTitle = UnityTools.FindGo(item.transform, "tittle"):GetComponent("UILabel")
    local lbDesc = UnityTools.FindGo(item.transform, "desc"):GetComponent("UILabel")
    local geted = UnityTools.FindGo(item.transform, "geted")
    local btnGoto = UnityTools.FindGo(item.transform, "goto")
    local btnGetAward = UnityTools.FindGo(item.transform, "get")
    ComponentData.Get(btnGetAward).Id = _configTb[index].awardIndex
    ComponentData.Get(btnGoto).Id = tonumber(_configTb[index].go_to)
    UnityTools.AddOnClick(btnGoto.gameObject, OnClickGoto)
    UnityTools.AddOnClick(btnGetAward.gameObject, OnClickGetAward)
    
    lbDesc.text = _configTb[index].describe
    if _configTb[index].status == 0 then
        btnGoto.gameObject:SetActive(true)
        geted.gameObject:SetActive(false)
        btnGetAward.gameObject:SetActive(false)
    elseif _configTb[index].status == 1 then
        btnGoto.gameObject:SetActive(false)
        geted.gameObject:SetActive(false)
        btnGetAward.gameObject:SetActive(true)
    else
        btnGoto.gameObject:SetActive(false)
        geted.gameObject:SetActive(true)
        btnGetAward.gameObject:SetActive(false)
    end
    local icon = UnityTools.FindGo(item.transform, "item/img"):GetComponent("UISprite")
    local num = UnityTools.FindGo(item.transform, "item/num"):GetComponent("UILabel")
    icon.spriteName = "C".._configTb[index].reward[1][2]
    if tonumber(_configTb[index].reward[1][2]) == 109 then
        num.text = LuaText.Format("item_107",tonumber(_configTb[index].reward[1][3])/10)
        lbTitle.text = num.text.."红包奖励"
    else
        num.text = _configTb[index].reward[1][3]
        lbTitle.text = GetNumText(num.text).."金币奖励"
    end
    
end
-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _lbType = UnityTools.FindGo(gameObject.transform, "Container/texbg/type"):GetComponent("UILabel")

    _spGold = UnityTools.FindGo(gameObject.transform, "Container/texbg/gold"):GetComponent("UISprite")

    _lbDiscount = UnityTools.FindGo(gameObject.transform, "Container/texbg/discount/lbCount"):GetComponent("UILabel")

    _lbGet1 = UnityTools.FindGo(gameObject.transform, "Container/texbg/get1"):GetComponent("UILabel")

    

    _lbBack1 = UnityTools.FindGo(gameObject.transform, "Container/texbg/back1"):GetComponent("UILabel")

    

    _scrollview = UnityTools.FindGo(gameObject.transform, "Container/texbg/list/scrollview"):GetComponent("UIScrollView")

    _gridCell = UnityTools.FindGo(gameObject.transform, "Container/texbg/list/scrollview/grid"):GetComponent("UIGridCellMgr")
    _gridCell.onShowItem = OnShowItem
    _lbHasGet = UnityTools.FindGo(gameObject.transform, "Container/texbg/hasget"):GetComponent("UILabel")

    _btnBuy = UnityTools.FindGo(gameObject.transform, "Container/texbg/btnBuy")
    UnityTools.AddOnClick(_btnBuy.gameObject, OnClickBuy)

    _lbNeed = UnityTools.FindGo(gameObject.transform, "Container/texbg/btnBuy/Label"):GetComponent("UILabel")

    _btnBuyed = UnityTools.FindGo(gameObject.transform, "Container/texbg/btnBuyed")

    _btnRule = UnityTools.FindGo(gameObject.transform, "Container/texbg/btnrule")
    UnityTools.AddOnClick(_btnRule.gameObject, OnClickRule)

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/texbg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

    _lbDisCountType = UnityTools.FindGo(gameObject.transform, "Container/texbg/discount/lbType"):GetComponent("UILabel")

--- [ALB END]



end
local function GetConfig()
    _mainData = CTRL.MainData
    _totalGold = CTRL.TotalGold
    _configTb = CTRL.ConfigTb
end
local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    LogError(CTRL.PayId)
    GetConfig()
    
end
function UpdateRechargeTask()
    
    GetConfig()
    if CTRL.NeedInitData then
        _scrollview:ResetPosition()
        CTRL.NeedInitData= false
    end
    UpdateWin()
    UpdateList()
end

local function Start(gameObject)
    _controller:SetScrollViewRenderQueue(_scrollview.gameObject)
    registerScriptEvent(EVENT_UPDATE_RECHARGE_TASK , "UpdateRechargeTask")
    UpdateWin()
    UpdateList()
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(EVENT_UPDATE_RECHARGE_TASK , "UpdateRechargeTask")
    CLEAN_MODULE(wName .. "Mono")
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
