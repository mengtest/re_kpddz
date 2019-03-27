-- -----------------------------------------------------------------


-- *
-- * Filename:    GoldPoolWinMono.lua
-- * Summary:     GoldPoolWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        10/16/2017 2:38:48 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("GoldPoolWinMono")



-- 界面名称
local wName = "GoldPoolWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local mainCityCtrl = IMPORT_MODULE("MainWinController")
local _platformMgr = IMPORT_MODULE("PlatformMgr");
-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _lbMineNum
local _lbMineRank
local _lbMinePool
local _spNeed
local _lbNeed
local _btnRecharge
local _mineHead
local _lbPool
local _lbTime
local _panel
local _scrollview
local _gridMgr
local _btnClose
local _ruleBtn
--- [ALD END]


local _myRank = 0
local _rankList={}







local function UpdateBottomInfo()
     if CTRL.ActInfo ~= nil then
        _lbMineNum.text = tostring(CTRL.ActInfo.my_recharge_money)
        _lbPool.text = tostring(CTRL.ActInfo.pool)
        _lbTime.text = LuaText.Format("gold_pool_desc5",CTRL.ActInfo.startStr,CTRL.ActInfo.endStr)
        _lbMineRank.text = tostring(_myRank)
        if _myRank == 0 then
            _lbMineRank.text = LuaText.GetString("gold_pool_desc2") 
            _spNeed.spriteName = "word1"
            if #_rankList > 0 then
                _lbNeed.text = LuaText.Format("gold_pool_desc3",_rankList[#_rankList].cash_num-CTRL.ActInfo.my_recharge_money+10) 
            else
                _lbNeed.text = LuaText.Format("gold_pool_desc3",10)
            end
            _lbMinePool.text = "0%"
            UnityTools.SetActive(_spNeed.gameObject,true)
        elseif _myRank == 1 then
            UnityTools.SetActive(_spNeed.gameObject,false)
            _lbNeed.text = ""
            if _rankList[1] ~=nil then
                _lbMinePool.text = LuaText.Format("gold_pool_desc4",_rankList[1].win_gold_num/100)
            end
        else
            
            _spNeed.spriteName = "word2"
            if #_rankList >= _myRank then
                _lbNeed.text = LuaText.Format("gold_pool_desc3",_rankList[_myRank-1].cash_num-CTRL.ActInfo.my_recharge_money+10)
                _lbMinePool.text = LuaText.Format("gold_pool_desc4",_rankList[_myRank].win_gold_num/100) 
            else
                _lbNeed.text = ""
                _lbMinePool.text = ""
            end
            UnityTools.SetActive(_spNeed.gameObject,true)   
        end
    end
    UnityTools.SetHead(_mineHead, _platformMgr.GetIcon(), _platformMgr.GetVipLv(),true,_platformMgr.getSex()) 
end
local function UpdateList()
    local delCount = _gridMgr.CellCount - #_rankList
    if delCount>=0 then
        for i=1,delCount do
            _gridMgr:DelteLastNode()
        end
        _gridMgr:UpdateCells()
    else
        _gridMgr:ClearCells()
        for i=1,#_rankList do
            _gridMgr:NewCellsBox(_gridMgr.Go)
        end
        _gridMgr.Grid:Reposition()
        _gridMgr:UpdateCells()
        _scrollview:ResetPosition()
    end
end


local function OnClickRechargeBtn(gameObject)
    UnityTools.CreateLuaWin("ShopWin");
end

local function OnClickRule(gameObject)
    UnityTools.CreateLuaWin("GoldPoolRuleWin");
    
end

--- [ALF END]





local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end
local function OnShowItem(cellbox, index, item)
   local index = index +1 
   local data = _rankList[index]
   
   if data == nil then return end
   local bg = nil
   if index == _myRank then 
        bg = UnityTools.FindGo(item.transform, "mine")
        
        UnityTools.SetActive(UnityTools.FindGo(item.transform, "other").gameObject,false)
        UnityTools.SetHead(UnityTools.FindGo(item.transform, "head"),data.player_icon,data.player_vip,true,data.sex)
   else
        bg = UnityTools.FindGo(item.transform, "other")
        UnityTools.SetActive(UnityTools.FindGo(item.transform, "mine").gameObject,false)
        LogError("data.player_icon=="..data.sex)
        UnityTools.SetHead(UnityTools.FindGo(item.transform, "head"),data.player_icon,data.player_vip,false,data.sex)
   end
   UnityTools.SetActive(bg.gameObject,true)
   local spRank = UnityTools.FindGo(item.transform, "rank"):GetComponent("UISprite")
   local lbRank = UnityTools.FindGo(bg.transform, "rank"):GetComponent("UILabel")
   local lbName = UnityTools.FindGo(bg.transform, "name"):GetComponent("UILabel")
   local lbPool = UnityTools.FindGo(bg.transform, "pool"):GetComponent("UILabel")
   if index <=3 then
       UnityTools.SetActive(spRank.gameObject,true)
       lbRank.text = ""
       
       spRank.spriteName = "rank"..index
   else
       UnityTools.SetActive(spRank.gameObject,false)
       UnityTools.SetActive(lbRank.gameObject,true)
       lbRank.text = tostring(index)
   end
   lbName.text = data.player_name
   lbPool.text = LuaText.Format("gold_pool_desc4",data.win_gold_num/100) 
   
   

end

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _lbMineNum = UnityTools.FindGo(gameObject.transform, "Container/bg/Panel/bg/num"):GetComponent("UILabel")

    _lbMineRank = UnityTools.FindGo(gameObject.transform, "Container/bg/Panel/bg/rank"):GetComponent("UILabel")

    _lbMinePool = UnityTools.FindGo(gameObject.transform, "Container/bg/Panel/bg/pool"):GetComponent("UILabel")

    _spNeed = UnityTools.FindGo(gameObject.transform, "Container/bg/Panel/bg/word"):GetComponent("UISprite")

    _lbNeed = UnityTools.FindGo(gameObject.transform, "Container/bg/Panel/bg/need"):GetComponent("UILabel")

    _btnRecharge = UnityTools.FindGo(gameObject.transform, "Container/bg/Panel/bg/btnRecharge")
    UnityTools.AddOnClick(_btnRecharge.gameObject, OnClickRechargeBtn)

    _mineHead = UnityTools.FindGo(gameObject.transform, "Container/bg/Panel/bg/head")

    _lbPool = UnityTools.FindGo(gameObject.transform, "Container/pool"):GetComponent("UILabel")

    _lbTime = UnityTools.FindGo(gameObject.transform, "Container/time"):GetComponent("UILabel")

    _panel = UnityTools.FindGo(gameObject.transform, "Container/bg/Panel"):GetComponent("UIPanel")

    _scrollview = UnityTools.FindGo(gameObject.transform, "Container/bg/list/scrollview"):GetComponent("UIScrollView")

    _gridMgr = UnityTools.FindGo(gameObject.transform, "Container/bg/list/scrollview/grid"):GetComponent("UIGridCellMgr")
    _gridMgr.onShowItem = OnShowItem
    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

    _ruleBtn = UnityTools.FindGo(gameObject.transform, "Container/rulebtn")
    UnityTools.AddOnClick(_ruleBtn.gameObject, OnClickRule)

--- [ALB END]


end

function OnRechargeActInfoUpdate()
    if mainCityCtrl.RankList[6] ~= nil then
        _rankList = mainCityCtrl.RankList[6]
    end
    if mainCityCtrl.MyRank[6] ~= nil then
        _myRank = mainCityCtrl.MyRank[6]
    end
    UpdateList()
    UpdateBottomInfo()
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    local protobuf = sluaAux.luaProtobuf.getInstance();
    protobuf:sendMessage(protoIdSet.cs_rank_query_req,{rank_type=6})
    AutoLuaBind(gameObject)
    registerScriptEvent(EVENT_UPDATE_RANK_INFO, "OnRechargeActInfoUpdate")
end
local function SetRender()
    
    _controller:SetScrollViewRenderQueue(_scrollview.gameObject)
    _panel.startingRenderQueue = _scrollview.gameObject:GetComponent("UIPanel").startingRenderQueue+50
    _panel.depth = 10
    
end

local function Start(gameObject)
    gTimer.registerOnceTimer(100, SetRender)
    
    -- _controller:SetScrollViewRenderQueue(_panel.gameObject)
    --UtilTools.SetEffectRenderQueueByUIParent(gameObject.transform,_panel.transform,50)
end
local function OnDestroy(gameObject)
    unregisterScriptEvent(EVENT_UPDATE_RANK_INFO, "OnRechargeActInfoUpdate")
    
    gTimer.removeTimer(SetRender)
    CLEAN_MODULE("GoldPoolWinMono")
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
