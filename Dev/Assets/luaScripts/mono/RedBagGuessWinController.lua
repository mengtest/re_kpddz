-- -----------------------------------------------------------------


-- *
-- * Filename:    RedBagGuessWinController.lua
-- * Summary:     红包猜测界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/20/2017 6:09:53 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("RedBagGuessWinController")

local UnityTools = IMPORT_MODULE("UnityTools");
local protobuf = sluaAux.luaProtobuf.getInstance();
local _saveData;

-- 界面名称
local wName = "RedBagGuessWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end

local function GetSaveData()
    return _saveData;
end

--- desc:打开界面
-- YQ.Qu:2017/3/20 0020
-- @param info
-- @return
local function Open(info)
    _saveData = info;
    UnityTools.CreateLuaWin("RedBagGuessWin");
end



UI.Controller.UIManager.RegisterLuaWinFunc("RedBagGuessWin", OnCreateCallBack, OnDestoryCallBack)


M.Open = Open
M.GetSaveData = GetSaveData
-- 返回当前模块
return M
