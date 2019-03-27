-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    RichCarMasterWinMono.lua
-- * Summary:     RichCarMasterWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        4/24/2017 4:41:12 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RichCarMasterWinMono")



-- 界面名称
local wName = "RichCarMasterWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
local mainCtrl = IMPORT_MODULE("RichCarWinController")
local _platformMgr = IMPORT_MODULE("PlatformMgr")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local MoneyTable={200000,300000,500000,100000000,200000000}
local _left={}
local _right={}
local _lbBtnName
local _leftBtnClose
--- [ALD END]



local _money = 0
local _selectIndex=-1
local function GetNumText(num)
    local newNum=0
    num=tonumber(num)
    num=num*100
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
local function OnClickBeMaster(gameObject)
    if CTRL.WinType == 3 then
        if _money == 0 then return end
        if _money>_platformMgr.GetGod() then
            UtilTools.ShowMessage(LuaText.GetString("rich_car_tip45"),"[ffffff]")    
            return
        end
        local protobuf = sluaAux.luaProtobuf.getInstance()
        local req = {}
        req.money = _money
        protobuf:sendMessage(protoIdSet.cs_car_add_money_req, req)
    elseif CTRL.WinType==2 then
        UtilTools.ShowWaitFlag()
        local protobuf = sluaAux.luaProtobuf.getInstance()
        local req = {}
        req.flag = 2
        req.money = 0
        protobuf:sendMessage(protoIdSet.cs_car_master_req, req)
    elseif CTRL.WinType==1 then
        local data = LuaConfigMgr.GoldCarryConfig["1"]
        if data ==nil then
            return
        end
        if _money > _platformMgr.GetGod() and _money < tonumber(data.gold_carry)  then
            UtilTools.ShowMessage(LuaText.Format("rich_car_tip25",data.gold_carry),"[ffffff]")    
            return
        end
        UtilTools.ShowWaitFlag()
        local protobuf = sluaAux.luaProtobuf.getInstance()
        local req = {}
        if mainCtrl.MasterInfo.self == 3 then
            req.flag = 3
        else
            req.flag = 1
        end
        req.money = _money
        
        protobuf:sendMessage(protoIdSet.cs_car_master_req, req)
    end   
end
local function OnClickMoney(gameObject)
    local comData = gameObject:GetComponent("ComponentData")
    if comData == nil then return end
    if comData.Id == -1 then return end
    if _selectIndex == comData.Id then return end
    _selectIndex = comData.Id
    for i=1 ,#_left.btn do
        if i == _selectIndex then
            _left.select[i]:SetActive(true)
            _left.unSelect[i]:SetActive(false)
        else
            _left.select[i]:SetActive(false)
            _left.unSelect[i]:SetActive(true)
        end
    end
    if _selectIndex == 6 then
        _money = _platformMgr.GetGod()
    elseif _selectIndex<6 and _selectIndex > 0 then
        local data = LuaConfigMgr.GoldCarryConfig[tostring(_selectIndex)]
        if data ~= nil then
            _money = tonumber(data.gold_carry)
        end
    end

end
--- [ALF END]




local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

local function UpdateList()
    local delCount = _right.cellMgr.CellCount - #CTRL.List
    if delCount>=0 then
        for i=1,delCount do
            _right.cellMgr:DelteLastNode()
        end
        _right.cellMgr:UpdateCells()
    else
        _right.cellMgr:ClearCells()
        for i=1,#CTRL.List do
            _right.cellMgr:NewCellsBox(_right.cellMgr.Go)
        end
        _right.cellMgr.Grid:Reposition()
        _right.cellMgr:UpdateCells()
        _right.scrollview:ResetPosition()
    end
end
local function OnShowItem(cellbox, index, item)
    index=index+1
    local data= CTRL.List[index]
    UnityTools.FindGo(item.transform,"bg/vip"):GetComponent("UISprite").spriteName=data.vip
    UnityTools.FindGo(item.transform,"bg/name"):GetComponent("UILabel").text=data.name
    UnityTools.FindGo(item.transform,"bg/money"):GetComponent("UILabel").text=GetNumText(data.money)

    

end
local function UpdateMasterInfo()
--头像设置 undo
    if mainCtrl.MasterInfo.self== 1 then
        _right.lbName.text = _platformMgr.UserName()
        _right.lbMoney.text = GetNumText(mainCtrl.MasterInfo.money)
        UnityTools.SetHead(_right.head, _platformMgr.GetIcon(), _platformMgr.GetVipLv(),true,_platformMgr.getSex())   
    else
        _right.lbName.text = mainCtrl.MasterInfo.name
        _right.lbMoney.text = GetNumText(mainCtrl.MasterInfo.money)
        UnityTools.SetHead(_right.head, mainCtrl.MasterInfo.head, mainCtrl.MasterInfo.vip,false,mainCtrl.MasterInfo.sex)   
    end
end
-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _left.go = UnityTools.FindGo(gameObject.transform, "Container/left")
    _left.btn={}
    _left.select={}
    _left.unSelect={}
    for i=1,6 do 
        _left.btn[i] = UnityTools.FindGo(gameObject.transform, "Container/left/bg/grid/btn"..i)
        _left.select[i]= UnityTools.FindGo(_left.btn[i].transform,"sel")
        _left.unSelect[i]= UnityTools.FindGo(_left.btn[i].transform,"unsel")
        if i~=6 then
            local label=UnityTools.FindGo(_left.select[i].transform,"desc"):GetComponent("UILabel")
            if _platformMgr.GetGod() < tonumber(LuaConfigMgr.GoldCarryConfig[tostring(i)].gold_carry) then
                UtilTools.SetGray(_left.btn[i],true,false)
                local comData = _left.btn[i]:GetComponent("ComponentData")
                comData.Id=-1
            end
            label.text = GetNumText(LuaConfigMgr.GoldCarryConfig[tostring(i)].gold_carry)

            
            label=UnityTools.FindGo(_left.unSelect[i].transform,"desc"):GetComponent("UILabel")
            label.text = GetNumText(LuaConfigMgr.GoldCarryConfig[tostring(i)].gold_carry)
        end
        UnityTools.AddOnClick(_left.btn[i].gameObject, OnClickMoney)

    end
    
    _left.btnUp = UnityTools.FindGo(gameObject.transform, "Container/left/btnopen")
    UnityTools.AddOnClick(_left.btnUp.gameObject, OnClickBeMaster)
    
    _right.go = UnityTools.FindGo(gameObject.transform, "Container/right")
    _right.tween = _right.go:GetComponent("TweenPosition")
    _right.head = UnityTools.FindGo(gameObject.transform, "Container/right/bg/Sprite/head")

    
    _right.lbName = UnityTools.FindGo(gameObject.transform, "Container/right/bg/Sprite/namebg/name"):GetComponent("UILabel")

    _right.lbMoney = UnityTools.FindGo(gameObject.transform, "Container/right/bg/Sprite/num"):GetComponent("UILabel")

    _right.scrollview = UnityTools.FindGo(gameObject.transform, "Container/right/list/scrollview"):GetComponent("UIScrollView")

    _right.cellMgr = UnityTools.FindGo(gameObject.transform, "Container/right/list/scrollview/cellmgr"):GetComponent("UIGridCellMgr")

    _right.btnClose = UnityTools.FindGo(gameObject.transform, "Container/right/btnclose")
    UnityTools.AddOnClick(_right.btnClose.gameObject, CloseWin)

    _right.type1 = UnityTools.FindGo(gameObject.transform, "Container/right/type1")

    _right.type2 = UnityTools.FindGo(gameObject.transform, "Container/right/type2")
    
    _lbBtnName = UnityTools.FindGo(gameObject.transform, "Container/left/btnopen/Label"):GetComponent("UILabel")
    
    _leftBtnClose = UnityTools.FindGo(gameObject.transform, "Container/left/btnclose")
    UnityTools.AddOnClick(_leftBtnClose.gameObject, CloseWin)

    if mainCtrl.MasterInfo.self == 0 then
        CTRL.WinType=1
        _lbBtnName.text = LuaText.GetString("rich_car_tip13")
    elseif mainCtrl.MasterInfo.self == 1 then
        _lbBtnName.text = LuaText.GetString("rich_car_tip44")
        CTRL.WinType = 3
    elseif mainCtrl.MasterInfo.self == 2 then
        CTRL.WinType = 2
        _lbBtnName.text = LuaText.GetString("rich_car_tip27")
    elseif mainCtrl.MasterInfo.self == 3 then
        CTRL.WinType = 1
        _lbBtnName.text = LuaText.GetString("rich_car_tip42")
    end
    -- if mainCtrl.MasterInfo.self == 1 then
    --     CTRL.WinType = 1
    --     -- _right.type1:SetActive(true)
    --     -- _right.type2:SetActive(false)
    --     _lbBtnName.text = LuaText.GetString("rich_car_tip13")
    -- elseif CTRL.WinType == 2 then
    --     -- _left.go:SetActive(false)
    --     -- _right.type1:SetActive(false)
    --     -- _right.type2:SetActive(true)
    --     -- _right.go.transform.localPosition=UnityEngine.Vector3(0, 0, 0)
    --     _lbBtnName.text = LuaText.GetString("rich_car_tip27")
    -- elseif CTRL.WinType == 3 then
    --     -- _right.go.gameObject:SetActive(false)
    --     -- _left.go.transform.localPosition=UnityEngine.Vector3(0, 0, 0)
    --     -- _leftBtnClose.gameObject:SetActive(true)
    --     _lbBtnName.text = LuaText.GetString("rich_car_tip44")
    -- end
    _right.cellMgr.onShowItem=OnShowItem


--- [ALB END]



end
function RichCarMasterBtnUpdate()
    if _platformMgr.GetGod() == 0 then
        CloseWin(nil)
        return
    end
    for i=1,6 do 
        if i~=6 then
            if _platformMgr.GetGod() < tonumber(LuaConfigMgr.GoldCarryConfig[tostring(i)].gold_carry) then
                UtilTools.SetGray(_left.btn[i],true,false)
            else
                UtilTools.RevertGray(_left.btn[i],true,false)
            end
        end
    end
end
local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    
end
function RichCarMasterWinUpdate()
    UpdateList()
end
function RichCarMasterStatusUpdate(idMsg,data)

    local req ={}
    req.flag= CTRL.WinType
    local protobuf = sluaAux.luaProtobuf.getInstance()
    protobuf:sendMessage(protoIdSet.cs_car_master_list_req,req)
    if data == nil then
        UpdateMasterInfo()
        
        return
    end
    if data.flag == 0 then
        UtilTools.ShowMessage(LuaText.GetString("rich_car_tip54"),"[FFFFFF]")
        CTRL.WinType=1
        _lbBtnName.text = LuaText.GetString("rich_car_tip13")

    elseif data.flag == 1 then
        UtilTools.ShowMessage(LuaText.GetString("rich_car_tip26"),"[FFFFFF]")
        _lbBtnName.text = LuaText.GetString("rich_car_tip44")
        CTRL.WinType = 3
    elseif data.flag == 2 then
        UtilTools.ShowMessage(LuaText.GetString("rich_car_tip26"),"[FFFFFF]")
        CTRL.WinType = 2
        _lbBtnName.text = LuaText.GetString("rich_car_tip27")
    elseif data.flag == 3 then
        UtilTools.ShowMessage(LuaText.GetString("rich_car_tip53"),"[FFFFFF]")
        CTRL.WinType = 1
        _lbBtnName.text = LuaText.GetString("rich_car_tip42")
    end
    -- if data.flag == 1 or data.flag == 2 then
    --     UtilTools.ShowMessage(LuaText.GetString("rich_car_tip26"),"[FFFFFF]")
        
    --     -- _left.go:SetActive(false)
    --     -- _right.type1:SetActive(false)
    --     -- _right.type2:SetActive(true)
    --     -- _right.tween.enabled=true
    --     -- _right.tween:PlayForward()
    -- end
end
function RichCarMasterWinInfoUpdate()
    UpdateMasterInfo()
    local req ={}
    req.flag= CTRL.WinType
    local protobuf = sluaAux.luaProtobuf.getInstance()
    protobuf:sendMessage(protoIdSet.cs_car_master_list_req,req)
end
local function Start(gameObject)
    OnClickMoney(_left.btn[1])
    UpdateMasterInfo()
    UpdateList()
    _controller:SetScrollViewRenderQueue(_right.scrollview.gameObject)
    registerScriptEvent(EVENT_RICHCAR_MASTER_LIST_UPDATE, "RichCarMasterWinUpdate")
    registerScriptEvent(EVENT_UPDATE_RICH_CAR_STATUS, "RichCarMasterStatusUpdate")
    registerScriptEvent(EVENT_RICHCAR_MASTER_UPDATE, "RichCarMasterWinInfoUpdate")
    
end


local function OnDestroy(gameObject)
    unregisterScriptEvent(EVENT_RICHCAR_MASTER_LIST_UPDATE, "RichCarMasterWinUpdate")
    unregisterScriptEvent(EVENT_UPDATE_RICH_CAR_STATUS, "RichCarMasterStatusUpdate")
    unregisterScriptEvent(EVENT_RICHCAR_MASTER_UPDATE, "RichCarMasterWinInfoUpdate")
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
