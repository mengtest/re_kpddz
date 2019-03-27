-- -----------------------------------------------------------------


-- *
-- * Filename:    RechargePhoneWinController.lua
-- * Summary:     充值手机号
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/18/2017 11:33:59 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RechargePhoneWinController")
local UnityTools = IMPORT_MODULE("UnityTools")
local protobuf = sluaAux.luaProtobuf.getInstance();

local _saveData;

-- 界面名称
local wName = "RechargePhoneWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end

local function GetSaveData()
    return _saveData;
 end

local function Open(obj)

    _saveData = obj;

    UnityTools.CreateLuaWin("RechargePhoneWin");
end

UI.Controller.UIManager.RegisterLuaWinFunc("RechargePhoneWin", OnCreateCallBack, OnDestoryCallBack)

M.GetSaveData = GetSaveData;
M.Open = Open;
-- 返回当前模块
return M
