-- -----------------------------------------------------------------


-- *
-- * Filename:    DiamondBagWinController.lua
-- * Summary:     钻石福袋
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/19/2017 4:38:59 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("DiamondBagWinController")

local _itemMgr = IMPORT_MODULE("ItemMgr")

-- 界面名称
local wName = "DiamondBagWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local _data = {
    isInit = false, --数据是否初始化
    isFirstBuy = false, --是否第一次购买
    todayIsBuy = false, --是否今天已经购买过
}

--- 初始化数据
function _data:Init()
    self.isInit = true
    local list = _itemMgr.GetShopListByType(7)
    if list ~= nil and #list > 0 then
--        LogWarn("[DiamondBagWinController._data:Init]"..list[1].id.."  leftTime = "..list[1].left_times);
        local info = list[1]
        if info.left_times > 0 then
            self.isFirstBuy = false
        else
            self.isFirstBuy = true
        end
        if #list > 1 then
--            LogWarn("[DiamondBagWinController._data:Init]"..list[2].id.."  leftTime = "..list[2].left_times);
            info = list[2]
            if info.left_times > 98 then
                self.todayIsBuy = false
            else
                self.todayIsBuy = true
--                self.isFirstBuy = true
            end
        else
            
            if list[1].left_times == 1 or list[1].left_times == 99 then
                self.todayIsBuy = false
            else
                self.todayIsBuy = true
--                self.isFirstBuy = true
            end
        end
--       LogWarn("[DiamondBagWinController._data:Init]"..tostring(self.isFirstBuy).."    "..tostring(self.todayIsBuy)); 
    end
    LogError("self.todayIsBuy="..tostring(self.todayIsBuy))
end

--- 购买成功
function _data:BuySucc()
    if self.isFirstBuy == false then
        self.isFirstBuy = true
    end

    if self.todayIsBuy == false then
        self.todayIsBuy = true
    end

    triggerScriptEvent(DIAMOND_BAG_UPDATE, self.todayIsBuy)
end

function _data:Update()
    self:Init()
    triggerScriptEvent(DIAMOND_BAG_UPDATE, self.todayIsBuy)
 end

function _data:Clear()
    self.isInit = false
    self.isFirstBuy = false
    self.todayIsBuy = false
end

function ClearDiamondBagController(id, value)
    _data:Clear()
end

registerScriptEvent(EXIT_CLEAR_ALL_DATA, "ClearDiamondBagController")

local function OnCreateCallBack(gameObject)
end


local function OnDestoryCallBack(gameObject)
end




UI.Controller.UIManager.RegisterLuaWinFunc("DiamondBagWin", OnCreateCallBack, OnDestoryCallBack)

M.data = _data
-- 返回当前模块
return M
