-- -----------------------------------------------------------------


-- *
-- * Filename:    NormalCowUIController.lua
-- * Summary:     看牌UI
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/9/2017 10:20:15 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("NormalCowUIController")



-- 界面名称
local wName = "NormalCowUI"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



local function OnCreateCallBack(gameObject)
    -- gameObject:SetActive(true)
end


local function OnDestoryCallBack(gameObject)

end




UI.Controller.UIManager.RegisterLuaWinFunc("NormalCowUI", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
