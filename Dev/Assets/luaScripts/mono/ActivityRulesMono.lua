-- -----------------------------------------------------------------


-- *
-- * Filename:    ActivityRulesMono.lua
-- * Summary:     活动信息说明界面
-- *
-- * Version:     1.0.0
-- * Author:      WP.Chu
-- * Date:        3/25/2017 1:55:58 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ActivityRulesMono")



-- 界面名称
local wName = "ActivityRules"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local btnClose
--- [ALD END]


-- //////////////////////////////////////////////////////////////////////////////////////
-- UI事件响应


local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

-- 关闭按钮点击
local function onCloseBtnClick(gameObject)
    CloseWin()
end

--- [ALF END]



-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    btnClose = UnityTools.FindGo(gameObject.transform, "Container/bg/btnClose")
    UnityTools.AddOnClick(btnClose.gameObject, onCloseBtnClick)

--- [ALB END]

end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

end


local function Start(gameObject)
    local CTRL = IMPORT_MODULE("ActivityAndAnnouncementController")
    local actsMgr = CTRL.ActivitiesManager
    if actsMgr ~= nil then
        local activeAct = actsMgr:activeActivity()
        if activeAct == nil then
            return
        end

        local strRules = activeAct:rules()
        local lblDesc = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/contentBg/text")
        if lblDesc ~= nil then
            lblDesc.text = strRules
        end
    end
end


local function OnDestroy(gameObject)
    CLEAN_MODULE("ActivityRulesMono")
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
