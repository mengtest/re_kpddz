-- -----------------------------------------------------------------


-- *
-- * Filename:    RankInfoWinMono.lua
-- * Summary:     RankInfoWin
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        7/7/2017 9:53:04 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RankInfoWinMono")



-- 界面名称
local wName = "RankInfoWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _btnClose
local _cellMgr
local _scrollview
local _listData
--- [ALD END]
local function GetNumText2(num)
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
local function OnShowItem(cellbox, index, item)
    local data = _listData[tostring(index+1)]
    if data ==nil then 
        return 
    end
    local desc=UnityTools.FindGo(item.transform, "des"):GetComponent("UILabel")
    local num = UnityTools.FindGo(item.transform, "ic/num"):GetComponent("UILabel")
    desc.text = data.des
    num.text = GetNumText2(data.reward)
end


--- [ALF END]
local function ShowList()
    if CTRL.WinType == 1 then
        _listData = LuaConfigMgr.RewardRankingConfig
        if _listData ~= nil then
        _cellMgr:ClearCells()
        for i=1,LuaConfigMgr.RewardRankingConfigLen do
            _cellMgr:NewCellsBox(_cellMgr.Go)
        end
        _cellMgr.Grid:Reposition()
        _cellMgr:UpdateCells()
        _scrollview:ResetPosition()
    end
    elseif CTRL.WinType == 2 then
        _listData = LuaConfigMgr.TotalRankingConfig
        if _listData ~= nil then
        _cellMgr:ClearCells()
        for i=1,LuaConfigMgr.TotalRankingConfigLen do
            _cellMgr:NewCellsBox(_cellMgr.Go)
        end
        _cellMgr.Grid:Reposition()
        _cellMgr:UpdateCells()
        _scrollview:ResetPosition()
    end
    end
    
end


local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/close")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

    _cellMgr = UnityTools.FindGo(gameObject.transform, "Container/right/bg/list/scrollview/grid"):GetComponent("UIGridCellMgr")

    _scrollview = UnityTools.FindGo(gameObject.transform, "Container/right/bg/list/scrollview"):GetComponent("UIScrollView")
    _cellMgr.onShowItem=OnShowItem
--- [ALB END]



end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

end


local function Start(gameObject)
    _controller:SetScrollViewRenderQueue(_scrollview.gameObject)
    ShowList()
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("RankInfoWinMono")
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
