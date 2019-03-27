-- -----------------------------------------------------------------


-- *
-- * Filename:    OpenBoxWinMono.lua
-- * Summary:     打开宝箱界面
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/17/2017 4:28:20 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("OpenBoxWinMono")
local roomMgr = IMPORT_MODULE("roomMgr")
local protobuf = sluaAux.luaProtobuf.getInstance()
local DiamondCtrl = IMPORT_MODULE("DiamondBagWinController")
-- 界面名称
local wName = "OpenBoxWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _platformMgr = IMPORT_MODULE("PlatformMgr")
local _winTip
local _bgMask
local _cell1
local _cell2
local _cell3
local _lbDesc
--- [ALD END]





local _boxConfig = nil
local _currHasMoney = 0
local _currGameTimes = 0

local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end

local function ClickMaskCall(gameObject)
    CloseWin()
end

--- [ALF END]







-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _winTip = UnityTools.FindCo(gameObject.transform, "UILabel", "Container/Win/tip")

    _bgMask = UnityTools.FindGo(gameObject.transform, "Container/mask")
    UnityTools.AddOnClick(_bgMask.gameObject, ClickMaskCall)

    _cell1 = UnityTools.FindGo(gameObject.transform, "Container/Win/layer/cell1")

    _cell2 = UnityTools.FindGo(gameObject.transform, "Container/Win/layer/cell2")

    _cell3 = UnityTools.FindGo(gameObject.transform, "Container/Win/layer/cell3")

    _lbDesc = UnityTools.FindCo(gameObject.transform, "UILabel","Container/Win/layer/cell2/desc")

--- [ALB END]






end
local function openDiamondBag()
    CloseWin()
    UnityTools.CreateLuaWin("DiamondBagWin")
end
local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    
    protobuf:registerMessageScriptHandler(protoIdSet.sc_niu_room_chest_draw_reply,"OpenBoxWinRecvItemReply")
    
end

local function clickBtnCall(gameObject)
    local tag = ComponentData.Get(gameObject).Tag
    if tag == 1 then
        tag = 2
        if CTRL.WinType ~= 1 then
            local leftTimes = 5
            if DiamondCtrl.data.todayIsBuy then
                leftTimes=leftTimes+5
            end
            if leftTimes-CTRL.LeftTimes[CTRL.WinType] <=0 then
                UnityTools.MessageDialog(LuaText.GetString("open_box_win_tip3"),{okCall=openDiamondBag})
                return
            end
        end
    elseif tag == 2 then
        tag = 3 
    else 
        tag = 1
    end
    roomMgr.sendMsg(protoIdSet.cs_player_niu_room_chest_draw, {type = tag,game_type = CTRL.WinType})
end

local function initWin()
    if _boxConfig == nil then return nil end
    if CTRL.WinType == 1 then
        if roomMgr.GetPlayerInfo(1) ~= nil then
            _currHasMoney = roomMgr.GetPlayerInfo(1).gold_num

        else
            _currHasMoney = 0 
        end 
    else
        _currHasMoney = _platformMgr.GetGod()
    end
    _winTip.text = LuaText.GetStr(LuaText.open_box_win_tip, _boxConfig.min_door)
    local cells = {_cell2, _cell1, _cell3}
    for i = 1, 3, 1 do
        local btnGo = UnityTools.FindGo(cells[i].transform, "btn")
        local btnGo2 = UnityTools.FindGo(cells[i].transform, "btn1")
        if btnGo2 ~= nil then
            ComponentData.Get(btnGo2).Tag = i
            UnityTools.AddOnClick(btnGo2, clickBtnCall)
            btnGo2.gameObject:SetActive(false)
        end
        ComponentData.Get(btnGo).Tag = i
        UnityTools.AddOnClick(btnGo, clickBtnCall)
        local price = UnityTools.FindCo(cells[i].transform, "UILabel", "price")
        local money = UnityTools.FindCo(cells[i].transform, "UILabel", "btn/money")
        local btnActive = false
        local hasTimes =true
        if _currGameTimes < tonumber(_boxConfig.condition) then
            btnActive = false
        else
            btnActive = true
        end
        if i == 3 then
            
            local reward = _boxConfig.free_get[1][1]
            money.text = "免费领取"
            price.text = reward .. "金币"
        else
            
            local reward = _boxConfig["get"..(i+1)][1]
            if CTRL.WinType == 1 and i == 1  then
                if _currHasMoney < tonumber(reward[2]) + _boxConfig.min_door then
                    btnActive = false
                end
                money= UnityTools.FindCo(cells[i].transform, "UILabel", "btn1/money")
                money.text = UnityTools.GetShortNum(tonumber(reward[2])+1)
                btnGo2.gameObject:SetActive(true)
                btnGo.gameObject:SetActive(false)
                btnGo = btnGo2
                _lbDesc.text = ""
            elseif i == 1 then
                
                money.text = "免费领取"
                _lbDesc.text = LuaText.Format("open_box_win_tip2",CTRL.LeftTimes[CTRL.WinType],10)
                local leftTimes = 5
                if DiamondCtrl.data.todayIsBuy then
                    leftTimes=leftTimes+5
                end

                if leftTimes-CTRL.LeftTimes[CTRL.WinType] <=0 then
                    if leftTimes >= 10 then
                        hasTimes = true
                    else
                        hasTimes = false
                    end
                    btnActive = false
                end
            else
                LogError(_currHasMoney)
                if _currHasMoney < tonumber(reward[2]) + _boxConfig.min_door then
                    btnActive = false
                end
                money.text = UnityTools.GetShortNum(tonumber(reward[2])+1)
            end
            price.text = reward[1] .. "钻石"
        end
        local btn = btnGo
        if btnActive == false then
            if i== 1 and hasTimes == false then
                btn:GetComponent("BoxCollider").enabled = true
            else
                btn:GetComponent("BoxCollider").enabled = false
            end
            btn:GetComponent("UISprite").spriteName = "btn_gray_small"
            -- money.gradientTop = UnityEngine.Color.white
            -- money.gradientBottom = UnityTools.RGB(227, 227, 227)
            -- money.effectColor = UnityTools.RGB(109, 109, 109)
        else
            btn:GetComponent("BoxCollider").enabled = true
            if i == 3 or i == 1 then
                btn:GetComponent("UISprite").spriteName = "btn_purple_normal"
                -- money.gradientTop = UnityEngine.Color.white
                -- money.gradientBottom = UnityTools.RGB(203, 252, 255)
                -- money.effectColor = UnityTools.RGB(34, 146, 187)
            else
                btn:GetComponent("UISprite").spriteName = "btn_yellow_big"
                -- money.gradientTop = UnityEngine.Color.white
                -- money.gradientBottom = UnityTools.RGB(255, 252, 203)
                -- money.effectColor = UnityTools.RGB(1893, 38, 19)
            end
        end
    end 
end

local function Start(gameObject)
    registerScriptEvent(EVENT_GAME_START_EFFECT, CloseWin)
    _currGameTimes = CTRL.Process
    _boxConfig = LuaConfigMgr.ChestConfig[tostring(CTRL.WinType)]
    initWin()
end


local function OnDestroy(gameObject)
    _boxConfig = nil
    unregisterScriptEvent(EVENT_GAME_START_EFFECT, CloseWin)
    protobuf:removeMessageHandler(protoIdSet.sc_niu_room_chest_draw_reply)
    CLEAN_MODULE("OpenBoxWinMono")
end



function OpenBoxWinRecvItemReply(msgID, msgData)
    if UnityTools.CheckMsg(msgID, msgData) then
        ShowAwardWin(msgData.rewards,true)
        ClickMaskCall()
    else
        LogError("Recv nil OpenBoxWinRecvItemReply")
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
