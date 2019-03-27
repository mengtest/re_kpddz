-- -----------------------------------------------------------------


-- *
-- * Filename:    RedConditionWinMono.lua
-- * Summary:     红包场入口补充界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/15/2017 5:31:58 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RedConditionWinMono")



-- 界面名称
local wName = "RedConditionWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local GameMgr = IMPORT_MODULE("GameMgr")
local _platformMgr = IMPORT_MODULE("PlatformMgr")
local _diamondCTRL = IMPORT_MODULE("DiamondBagWinController")
local _taskCTRL = IMPORT_MODULE("TaskWinController")
local _itemMgr = IMPORT_MODULE("ItemMgr");
local _winBg
local _btnClose
local _cellTb={}
local mainCityCtrl = IMPORT_MODULE("MainWinController")
local _diamondLb
local _grid
local _spGift
local _spShop
local _slider
local _lbProcess
--- [ALD END]
local canRelive= false
local _roomMgr = IMPORT_MODULE("roomMgr")


local function IsHaveTask()
    local taskList = _itemMgr.GetMissionListByType(6)
    if taskList == nil then
        return false
    end
    local completedCount = 0
    for i = 1, #taskList do
        if taskList[i].state == 2 then
            completedCount = completedCount + 1;
        end
    end
    return #taskList ~= completedCount
end



    -- UnityTools.AddOnClick(btnGo, function(go)
    --     GoTo(info.cfg.skip_id, wName)
    -- end)

local function OnRecoverHandler()
    local shopCtrl = IMPORT_MODULE("ShopWinController");
    if shopCtrl ~= nil then
        shopCtrl.OpenShop(2)
    end
end

local function OnGoGame(gameObject)
    local mainWin = IMPORT_MODULE("NormalCowMain");
    if mainWin ~= nil then
        mainWin.ClickRestartCall(nil);
        UnityTools.DestroyWin("RedConditionWin")
    else
        GameMgr.EnterGame(1, 10, function()
            UnityTools.DestroyWin("MainCenterWin")
            UnityTools.DestroyWin("MainWin")
            UnityTools.DestroyWin("RedConditionWin")
            UnityTools.DestroyWin("GameCenterWin")
        end)
    end
end

--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end



local function InitWin()
    
    local missionData = CTRL.Data:GetItem(1) 
    -- if missionData ~= nil then
    --     _slider.value = missionData.current/missionData.target
    --     _lbProcess.text = missionData.current.."/"..missionData.target
    --     if _platformMgr.Config.redBagRoomResetNum >0 and missionData.current >= missionData.target then 
    --         UtilTools.RevertGray(_cellTb[1].cellGo.gameObject,true,false)
    --         _cellTb[1].lbBtn.text = LuaText.GetString("red_condition_win_tip11")
    --         _cellTb[1].btn.enabled = true
    --     elseif _platformMgr.Config.redBagRoomResetNum <=0 and missionData.current < missionData.target then
    --         UtilTools.RevertGray(_cellTb[1].cellGo.gameObject,true,false)
    --         _cellTb[1].lbBtn.text = LuaText.GetString("goto_lb")
    --         _cellTb[1].btn.enabled = true
    --     else
    --         UtilTools.SetGray(_cellTb[1].btn.gameObject,true,false)
    --         _cellTb[1].lbBtn.text = LuaText.GetString("red_condition_win_tip11")
    --         _cellTb[1].btn.enabled = false
    --     end 
    -- end
    _cellTb[1].lbBtn.text = LuaText.GetString("red_condition_win_tip11")
    _cellTb[1].btn.enabled = true
    _cellTb[1].lbDesc.text = LuaText.GetString("red_condition_win_tip9")
    _cellTb[2].lbDesc.text = LuaText.GetString("red_condition_win_tip10")
    UnityTools.SetActive(_spGift.gameObject,false)
    _cellTb[2].lbBtn.text = LuaText.GetString("goto_lb")
    -- if _diamondCTRL.data.todayIsBuy == false then --显示福袋
    --     _cellTb[2].lbDesc.text = LuaText.GetString("red_condition_win_tip8")
    --     _cellTb[2].lbBtn.text = LuaText.GetString("goto_lb")
    --     UnityTools.SetActive(_spShop.gameObject,false)
    -- elseif _diamondCTRL.data.todayIsBuy and CTRL.LeftTimes <= 0 then --显示商店
    --     _cellTb[2].lbDesc.text = LuaText.GetString("red_condition_win_tip10")
    --     UnityTools.SetActive(_spGift.gameObject,false)
    --     _cellTb[2].lbBtn.text = LuaText.GetString("goto_lb")
    -- elseif _diamondCTRL.data.todayIsBuy and CTRL.LeftTimes>0 then --显示复活
    --     _cellTb[2].lbDesc.text = LuaText.GetString("red_condition_win_tip8")
    --     UnityTools.SetActive(_spShop.gameObject,false)
    --     _cellTb[2].lbBtn.text = LuaText.GetString("red_condition_win_tip11")
    -- end
    _cellTb[3].lbDesc.text = LuaText.GetString("red_condition_win_tip7")
    _cellTb[3].lbBtn.text = LuaText.GetString("goto_lb")
    if IsHaveTask() then
        _cellTb[3].btn.enabled = true
    else
        _cellTb[3].btn.enabled = false
        UtilTools.SetGray(_cellTb[3].cellGo.gameObject,true,false)

    end
    _diamondLb.text = UnityTools.GetShortNum(_platformMgr.GetDiamond())
end

local function OnCLickBtn(gameObject)
    local id = ComponentData.Get(gameObject).Id
    if id == 1 then
        local mainWin = IMPORT_MODULE("NormalCowMainMono");
        if mainWin ~= nil then
            mainCityCtrl.SkipId = 104 
            mainWin.CloseWin(0);
            UnityTools.DestroyWin("RedConditionWin")
        else
            GoTo(104,wName)
        end
    elseif id == 2 then
        CloseWin(nil)
        OnRecoverHandler()
        -- if _diamondCTRL.data.todayIsBuy == false then --显示福袋
        --     UnityTools.CreateLuaWin("DiamondBagWin")
        -- elseif _diamondCTRL.data.todayIsBuy and CTRL.LeftTimes <= 0 then --显示商店
        --     OnRecoverHandler()
        -- elseif _diamondCTRL.data.todayIsBuy and CTRL.LeftTimes>0 then --显示复活
        --     local protobuf = sluaAux.luaProtobuf.getInstance()
        --     protobuf:sendMessage(protoIdSet.cs_redpack_relive_req, {})
        -- end
    elseif id == 3 then
        CloseWin(nil)
        _taskCTRL.TabIndex = 5
        UnityTools.CreateLuaWin("TaskWin")
    end
end
-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

    

    _diamondLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/bg/diamond/Label")
    for i=1 ,3 do
        _cellTb[i]={}
        _cellTb[i].cellGo = UnityTools.FindGo(gameObject.transform, "Container/scrollBg/Grid/cell"..i)
        
        _cellTb[i].btn = UnityTools.FindGo(_cellTb[i].cellGo.transform,  "btnRecover"):GetComponent("BoxCollider")
        ComponentData.Get(_cellTb[i].btn.gameObject).Id = i
        UnityTools.AddOnClick(_cellTb[i].btn.gameObject, OnCLickBtn)
        _cellTb[i].lbBtn = UnityTools.FindCo(_cellTb[i].btn.transform, "UILabel", "Label")
        _cellTb[i].lbDesc = UnityTools.FindCo(_cellTb[i].cellGo.transform, "UILabel", "desc")
    end

    _grid = UnityTools.FindGo(gameObject.transform, "Container/scrollBg/Grid")

    _spGift = UnityTools.FindGo(gameObject.transform, "Container/scrollBg/Grid/cell2/gift")

    _spShop = UnityTools.FindGo(gameObject.transform, "Container/scrollBg/Grid/cell2/shop")


    _slider = UnityTools.FindGo(gameObject.transform, "Container/scrollBg/Grid/cell1/slider"):GetComponent("UISlider")

    _lbProcess = UnityTools.FindGo(gameObject.transform, "Container/scrollBg/Grid/cell1/slider/Label"):GetComponent("UILabel")

    _cellTb[1].cellGo.gameObject:SetActive(false);
    _grid:GetComponent("UIGrid").repositionNow = true;
--- [ALB END]






end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
end


local function Start(gameObject)
    --    local scrollView = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/scrollBg/ScrollView")
    --    _controller:SetScrollViewRenderQueue(scrollView)
    InitWin()
    -- gTimer.registerOnceTimer(100, InitWin)
    --    ResetScroll()
end


local function OnDestroy(gameObject)
--    UnityTools.RemoveDeactive(_btnGoGame)
--    UnityTools.RemoveDeactive(_btnRecover)

    -- gTimer.removeTimer(InitWin)
    CLEAN_MODULE("RedConditionWinMono")
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
