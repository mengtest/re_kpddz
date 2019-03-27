-- -----------------------------------------------------------------


-- *
-- * Filename:    HelpWinMono.lua
-- * Summary:     帮助文档
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/29/2017 6:21:39 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("HelpWinMono")



-- 界面名称
local wName = "HelpWin"

-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _saveData;
local _helps = {}
local _help_dic = 15
local _winBg
local _btnClose
local _grid
local _cell
--- [ALD END]
local function tabKey(type)
    if type == "Game" then --游戏帮助
        return "help"
    elseif type == "exchange" then --兑换流程
        return "exchange_help"
    elseif type == "exchange" then --兑换流程
        return "exchange_help"
    end
end

--- desc:设置帮助显示
-- YQ.Qu:2017/3/29 0029
-- @param item
-- @param index
-- @return h
local function ShowItemSet(item, index)
    local key = tabKey(_saveData.from);
    local tabLb = UnityTools.FindCo(item.transform, "UILabel", "tab/Label");
    local descLb = UnityTools.FindCo(item.transform, "UILabel", "desc");
    tabLb.text = LuaText.GetString(key .. "_tab" .. index);
    descLb.text = LuaText.GetString(key .. "_desc" .. index);
    return 38 + descLb.height;
end



local function ResetItemPos(item, startY)
    local descLb = UnityTools.FindCo(item.transform, "UILabel", "desc");
    item.transform.localPosition = UnityEngine.Vector3(0, startY, 0)
    return startY - 38 - descLb.height;
end

local function ResetItems()
    local startY = 0;
    for i = 1, _saveData.tabNum do
        startY = ResetItemPos(_helps[i], startY)
        startY = startY - _help_dic;
    end
end


--- [ALF END]
local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winBg = UnityTools.FindGo(gameObject.transform, "Container")

    _btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(_btnClose.gameObject, CloseWin)

    _grid = UnityTools.FindGo(gameObject.transform, "Container/dragBg/ScrollView/Grid")

    _cell = UnityTools.FindGo(gameObject.transform, "cell")

    --- [ALB END]
end




local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    UnityTools.OpenAction(_winBg);
    local scroll = UnityTools.FindGo(gameObject.transform, "Container/dragBg/ScrollView");
    _controller:SetScrollViewRenderQueue(scroll);

    _saveData = CTRL.OpenFrom;
end


local function Start(gameObject)
    local startY = 0;
    _helps = {}
    LogWarn("[HelpWinMono.Start]form = " .. _saveData.from .. "  tabNum = " .. _saveData.tabNum);
    for i = 1, _saveData.tabNum, 1 do
        local help = UtilTools.AddChild(_grid, _cell, UnityEngine.Vector3(0, 0, 0))
        help.transform.name = "help" .. i;
        local h = ShowItemSet(help, i);
        _helps[i] = help;
        help.transform.localPosition = UnityEngine.Vector3(0, startY, 0);
        --        startY = startY - h - 10;
    end

    gTimer.registerOnceTimer(100, ResetItems)
end




local function OnDestroy(gameObject)
    if _saveData ~= nil then
        _saveData:ResetData();
    end
    CLEAN_MODULE("HelpWinMono")
end



-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
