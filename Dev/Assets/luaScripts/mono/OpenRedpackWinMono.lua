-- -----------------------------------------------------------------


-- *
-- * Filename:    OpenRedpackWinMono.lua
-- * Summary:     打开红包界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        5/15/2017 9:49:57 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("OpenRedpackWinMono")



-- 界面名称
local wName = "OpenRedpackWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)

local roomMgr = IMPORT_MODULE("roomMgr")

-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _flag
local _redClosed
local _bgEff
local _redOpened_0
local _redOpened_1
local _paper
local _numLb
local _mask
local _win
--- [ALD END]





local _isOpened = 0


local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

function ReceiveRedOpenReply(msgID, msgData)

    if UnityTools.CheckMsg(msgID, msgData) then
        
        for k, v in pairs(msgData.reward) do
            local rewardInfo = v
            -- LogError(rewardInfo.base_id .. "   " .. rewardInfo.count)
            if rewardInfo.base_id == 109 then
                _numLb.text = string.format("%.1f", (rewardInfo.count / 10))
                local action = _flag:GetComponent("TweenRotation")
                action.enabled = false
                _flag.transform.localRotation = UnityEngine.Quaternion.Euler(0, 0, 0)
                _redClosed:SetActive(false)
                _redOpened_0:SetActive(true)
                _redOpened_1:SetActive(true)
                _bgEff:SetActive(true)
                _paper:SetActive(true)
                -- if CTRL.OpenFun ~= nil then
                --     CTRL.OpenFun()
                --     CTRL.OpenFun = nil
                -- end
                _flag.gameObject:SetActive(false)
                _isOpened = 2
                local t = gTimer.registerOnceTimer(2000, function() 
                    CloseWin()
                end)
                gTimer.setRecycler(wName, t)
                break
            end
        end
    else
        LogError("Recv nil NormalCowMainRecvRedRewardUpdate")
    end
end

local function clickOpenFlag(gameObject)
    if _isOpened ~= 0 then return nil end
    _isOpened = 1
    local action = _flag:GetComponent("TweenRotation")
    action.enabled = true
    local t2 = gTimer.registerOnceTimer(500, function() 
        roomMgr.sendMsg(protoIdSet.cs_redpack_room_draw_req, {})
    end)
    gTimer.setRecycler(wName, t2)
    
    
end

local function clickOtherArea(gameObject)
    if _isOpened ~= 2 then
        clickOpenFlag(gameObject)
    else
        CloseWin(gameObject)
    end
end

--- [ALF END]




-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _flag = UnityTools.FindGo(gameObject.transform, "Container/Win/flag")
    UnityTools.AddOnClick(_flag.gameObject, clickOpenFlag)

    _redClosed = UnityTools.FindGo(gameObject.transform, "Container/Win/red")

    _bgEff = UnityTools.FindGo(gameObject.transform, "Container/Win/bgEff")

    _redOpened_0 = UnityTools.FindGo(gameObject.transform, "Container/Win/red_0")

    _redOpened_1 = UnityTools.FindGo(gameObject.transform, "Container/Win/red_1")

    _paper = UnityTools.FindGo(gameObject.transform, "Container/Win/paper")

    _numLb = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/Win/paper/num")

    _mask = UnityTools.FindGo(gameObject.transform, "Container/mask")
    UnityTools.AddOnClick(_mask.gameObject, clickOtherArea)

    _win = UnityTools.FindGo(gameObject.transform, "Container/Win")
    UnityTools.AddOnClick(_win.gameObject, clickOtherArea)

--- [ALB END]









end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    -- _numLb.text = CTRL.RedNum
end


local function Start(gameObject)

end


local function OnDestroy(gameObject)
    gTimer.recycling(wName)
    CLEAN_MODULE("OpenRedpackWinMono")
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
