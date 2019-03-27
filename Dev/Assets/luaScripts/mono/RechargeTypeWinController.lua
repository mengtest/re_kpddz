-- -----------------------------------------------------------------


-- *
-- * Filename:    RechargeTypeWinController.lua
-- * Summary:     充值方式
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/17/2017 5:46:13 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RechargeTypeWinController")
local UnityTools = IMPORT_MODULE("UnityTools");


local _saveData;
local _queryData;

-- 界面名称
local wName = "RechargeTypeWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end

local function GetSaveData()
    return _saveData;
end

local function GetQueryData()
    return _queryData;
 end


local function Open(obj, queryData)
    _saveData = obj;
    _queryData = queryData;
    UnityTools.CreateLuaWin(wName);
end




UI.Controller.UIManager.RegisterLuaWinFunc("RechargeTypeWin", OnCreateCallBack, OnDestoryCallBack)

M.Open = Open;
M.GetSaveData = GetSaveData;
M.GetQueryData = GetQueryData;
-- 返回当前模块
return M
