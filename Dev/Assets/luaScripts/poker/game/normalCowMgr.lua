-- -----------------------------------------------------------------


-- *
-- * Filename:    normalCowMgr.lua
-- * Summary:     普通牛牛模式
-- *              
-- * Version:     1.0.0
-- * Author:      HAN-PC
-- * Date:        2017-2-8 14:27:19
-- -----------------------------------------------------------------

-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("normalCowMgr")

local UnityTools = IMPORT_MODULE("UnityTools")

local pokerMgr = IMPORT_MODULE("PokerMgr")
local roomMgr = IMPORT_MODULE("roomMgr")

local _loadedLua = {
    "NormalCowMainController",
    "NormalCowUIController",
    "NormalCowTopController",
    "NormalCowRedUIController",
    -- "OpenBoxWinController",
    "OpenRedpackWinController",
}

local function initMgr() 
    -- 设置牛牛计算器
    pokerMgr.SetCalculator("cowCalculator")
    for i, v in pairs(_loadedLua) do
        LoadLuaFile("mono/" .. v)
    end
end

local function cleanMgr()
    pokerMgr.CleanCalculator()
    pokerMgr.CleanMgr()
    for i, v in pairs(_loadedLua) do
        CLEAN_MODULE(v)
    end
end

local function openMainWin()
    UnityTools.CreateLuaWin("NormalCowMain")
end

local function start()
    openMainWin()
end

M.Start = start
M.OpenMainWin = openMainWin
M.InitMgr = initMgr
M.CleanMgr = cleanMgr
return M
-- 加载lua脚本
--initMgr()