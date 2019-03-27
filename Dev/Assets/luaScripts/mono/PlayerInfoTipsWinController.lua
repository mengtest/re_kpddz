-- -----------------------------------------------------------------


-- *
-- * Filename:    PlayerInfoTipsWinController.lua
-- * Summary:     人物界面上的物品Tip
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        2/27/2017 10:52:36 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("PlayerInfoTipsWinController")



-- 界面名称
local wName = "PlayerInfoTipsWin"
local _mono;
local _saveArgs;
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

--desc:
--YQ.Qu:2017/2/27 0027
local function ShowTips(args)
--    _controller:CallUIEvent(1,args);
    if _mono ~= nil then
        _mono.OnEventSetContent(args)
        _saveArgs = nil;
    else
        _saveArgs = args;
    end
--    _saveArgs = args;
end

local function OnCreateCallBack(gameObject)
    _mono = IMPORT_MODULE(wName.."Mono");
    PrintTable(_saveArgs)
    if _saveArgs ~= nil then
        _mono.OnEventSetContent(_saveArgs)
    end
end


local function OnDestoryCallBack(gameObject)
    _mono = nil;
    _saveArgs = nil;
end

local function ClearAll()
    _mono = nil;
    _saveArgs = nil;
end




UI.Controller.UIManager.RegisterLuaWinFunc("PlayerInfoTipsWin", OnCreateCallBack, OnDestoryCallBack)

M.ShowTips = ShowTips;
M.ClearAll = ClearAll;
-- 返回当前模块
return M
