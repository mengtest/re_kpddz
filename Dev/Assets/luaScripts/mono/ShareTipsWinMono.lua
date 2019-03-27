-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    ShareTipsWinMono.lua
-- * Summary:     分享规则介绍
-- *
-- * Version:     1.0.0
-- * Author:      WP.Chu
-- * Date:        5/25/2017 5:15:21 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ShareTipsWinMono")



-- 界面名称
local wName = "ShareTipsWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)


-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

-- 规则描述
local _rulesData = {
    {"share_rules_title_1", "share_rules_desc_1"},
    {"share_rules_title_2", "share_rules_desc_2"},
    {"share_rules_title_3", "share_rules_desc_3"},
}


local _helps = {}
local _help_dic = 15
local _winBg
local _btnClose
local _grid
local _cell
--- [ALD END]


--- desc:设置帮助显示
-- YQ.Qu:2017/3/29 0029
-- @param item
-- @param index
-- @return h
local function ShowItemSet(item, index)
    local tabLb = UnityTools.FindCo(item.transform, "UILabel", "tab/Label");
    local descLb = UnityTools.FindCo(item.transform, "UILabel", "desc");
    local descSpr = UnityTools.FindCo(item.transform, "UISprite", "Sprite");
    tabLb.text = LuaText.GetString(_rulesData[index][1]);
    descLb.text = LuaText.GetString(_rulesData[index][2]);
    LogWarn("[HelpWinMono.ShowItemSet]" .. descSpr.height .. " tabLb.height = " .. tabLb.height);
    return 38 + descSpr.height;
end


local function ResetItemPos(item, startY)
    local descSpr = UnityTools.FindCo(item.transform, "UISprite", "Sprite");
    item.transform.localPosition = UnityEngine.Vector3(0, startY, 0)
    return startY - 38 - descSpr.height;
end

local function ResetItems()
    local startY = 0;
    local nCount = table.getn(_rulesData)
    for i = 1, nCount do
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
    UnityTools.OpenAction(_winBg)
    local scroll = UnityTools.FindGo(gameObject.transform, "Container/dragBg/ScrollView")
    _controller:SetScrollViewRenderQueue(scroll)
end


local function Start(gameObject)
    local startY = 0;
    _helps = {}
    local nCount = table.getn(_rulesData)
    for i = 1, nCount do
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