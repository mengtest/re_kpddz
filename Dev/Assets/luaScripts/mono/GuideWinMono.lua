-- -----------------------------------------------------------------


-- *
-- * Filename:    GuideWinMono.lua
-- * Summary:     新手引导
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        4/10/2017 5:06:43 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("GuideWinMono")



-- 界面名称
local wName = "GuideWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local protobuf = sluaAux.luaProtobuf.getInstance()
local _guideLayers = nil
local _currStep = 0
--- [ALD END]


--- [ALF END]

local pMgr = IMPORT_MODULE("PlatformMgr")


local function CloseWin(gameObject)
    triggerScriptEvent(GUIDE_STEP_UPDATE,"showMain",2)
--    pMgr.SetGuideStep(2)
    UnityTools.DestroyWin(wName)
end

local function doGuideStep()
    _guideLayers[_currStep]:SetActive(true)
end

local function ClickConfirmBtn(gameObject)
    local tag = ComponentData.Get(gameObject).Tag
    _guideLayers[_currStep]:SetActive(false)
    if _currStep == 5 then
        _currStep = 0
        protobuf:sendMessage(protoIdSet.cs_guide_next_step_req, {next_step_id = 1})
        protobuf:sendMessage(protoIdSet.cs_guide_next_step_req, {next_step_id = 2})
        return nil
    -- elseif _currStep == 6 then 
    --     _currStep = 0
    --     protobuf:sendMessage(protoIdSet.cs_guide_next_step_req, {next_step_id = 2})
    --     return nil
    end
    _currStep = _currStep + 1
    doGuideStep()
end

local function ClickFailBtn(gameObject)
    local tag = ComponentData.Get(gameObject).Tag
    -- 用户取消新手教学
    if tag == 1 then
        protobuf:sendMessage(protoIdSet.cs_guide_next_step_req, {next_step_id = 3})
        CloseWin(gameObject)
        return nil
    else
        UnityTools.ShowMessage(LuaText.guide_answer_wrong)
    end
end

-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _guideLayers = {}
    for i = 1, 6, 1 do
        _guideLayers[i] = UnityTools.FindGo(gameObject.transform, "Container/guide_" .. i)
        _guideLayers[i]:SetActive(false)
        local content = UnityTools.FindCo(_guideLayers[i].transform, "UILabel", "chatBg/content")
        content.text = "[636674]" .. LuaConfigMgr.GuideConfig[tostring(i)].character .. "[-]"
        local btn1 = UnityTools.FindGo(_guideLayers[i].transform, "btn1")
        local btn2 = UnityTools.FindGo(_guideLayers[i].transform, "btn2")
        ComponentData.Get(btn1).Tag = i
        UnityTools.AddOnClick(btn1, ClickConfirmBtn)
        if btn2 ~= nil then
            ComponentData.Get(btn2).Tag = i
            UnityTools.AddOnClick(btn2, ClickFailBtn)
        end
    end

--- [ALB END]

end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)

end

local function Start(gameObject)
    protobuf:registerMessageScriptHandler(protoIdSet.sc_guide_next_step_reply,"GWReceiveGuideNextReply")
    if pMgr.GetGuideStep() == 1 then
        _currStep = 0
        protobuf:sendMessage(protoIdSet.cs_guide_next_step_req, {next_step_id = 2})
    else
        _currStep = 1
    end
    doGuideStep()
end


local function OnDestroy(gameObject)
    protobuf:removeMessageHandler(protoIdSet.sc_guide_next_step_reply)
    _guideLayers = nil
    gTimer.recycling(wName)
    CLEAN_MODULE("GuideWinMono")
end


function GWReceiveGuideNextReply(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        if msgData.reward ~= nil and #msgData.reward > 0 then
            ShowAwardWin(msgData.reward)
            if _currStep == 0 then
                CloseWin()
            end
        end
    else
        LogError("Recv nil GWReceiveGuideNextReply")
    end 
end

-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy


-- 返回当前模块
return M
