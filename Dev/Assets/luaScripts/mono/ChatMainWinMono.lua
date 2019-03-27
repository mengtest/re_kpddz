-- -----------------------------------------------------------------


-- *
-- * Filename:    ChatMainWinMono.lua
-- * Summary:     聊天系统
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/14/2017 11:27:03 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ChatMainWinMono")



-- 界面名称
local wName = "ChatMainWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local roomMgr = IMPORT_MODULE("roomMgr")
local platformMgr = IMPORT_MODULE("PlatformMgr")

local _bgMask
local _enterAction
local _tab1
local _tab2
local _tab3
local _sv_tab3
local _sv_tab3_mgr
local _inputText
local _sendMsgBtn
local _dragLayer
local _sv_tab2
local _sv_tab2_mgr
local _sv_tab1
local _sv_tab1_mgr
local _cell1
--- [ALD END]




local _needResetPos = true
local _chatDefaultList = {}
local _emojiList = {}
local _thisObj
local _bPlaySound = false
-- local _recordCount = 0
local _nEffectVolum =0
local function CloseWin(gameObject)
    -- 清空GridCellMgr中的数据
    _sv_tab3_mgr:ClearCells()
    _sv_tab2_mgr:ClearCells()
    _sv_tab1_mgr:ClearCells()
    UnityTools.DestroyWin(wName)
end

local function ClickMaskAreaCall(gameObject)
    CloseWin(gameObject)
    -- _enterAction:Play(false)
    -- gTimer.registerOnceTimer(240, CloseWin, gameObject)
end
local function ClickPlaySound(gameObject)
    local index = ComponentData.Get(gameObject).Id
    local chatMsg = roomMgr.GetChatInfoList()[#roomMgr.GetChatInfoList() - index]
    if chatMsg == nil then
        return
    end
    if chatMsg.content_type == 3 then
        _bPlaySound = true
        UtilTools.LoadAudioFile(chatMsg.des_player_uuid,gameObject);
        
    end
end
local function OnShowTab3(cellbox, index, item)
    -- item:SetActive(true)
    -- index = index + 1
    if index == 0 then
        _needResetPos = true
    end

    local chatMsg = roomMgr.GetChatInfoList()[#roomMgr.GetChatInfoList() - index]
    local hImg = UnityTools.FindCo(item.transform, "UITexture", "img")
    local defaultImg = UnityTools.FindCo(item.transform,"UISprite","img/head")
    -- LogError(chatMsg.player_icon .. "  " .. chatMsg.player_vip)
    if chatMsg == nil then 
    -- 没有头像
        hImg.mainTexture = nil
        defaultImg.spriteName = platformMgr.PlayerDefaultHead(chatMsg.sex)
    else
    -- 设置头像
        hImg.mainTexture = nil
        UnityTools.SetPlayerHead(chatMsg.player_icon, hImg, platformMgr.PlayerUuid() == chatMsg.player_uuid)
        defaultImg.spriteName = platformMgr.PlayerDefaultHead(chatMsg.sex)
    end
    ComponentData.Get(item.gameObject).Id = index
    UnityTools.AddOnClick(item.gameObject, ClickPlaySound)

    local vip = UnityTools.FindGo(item.transform, "img/vip/vipBox")
    if vip ~= nil then
        if chatMsg ~= nil then
            UnityTools.SetNewVipBox(vip, chatMsg.player_vip,"vip",nil)--_thisObj)
        else
            UnityTools.SetNewVipBox(vip, 0,"vip")
        end
    end
    local chatItem = nil
    local bSelf = platformMgr.PlayerUuid() == chatMsg.player_uuid
    if bSelf then
        hImg.transform.localPosition = UnityEngine.Vector3(145,0,0)
        chatItem = UnityTools.FindGo(item.transform, "self")
        UnityTools.FindGo(item.transform, "other"):SetActive(false)
    else
        hImg.transform.localPosition = UnityEngine.Vector3(-145,0,0)
        chatItem = UnityTools.FindGo(item.transform, "other")
        UnityTools.FindGo(item.transform, "self"):SetActive(false)
        local name = UnityTools.FindCo(chatItem.transform, "UILabel", "name") 
        name.text = chatMsg.player_name
        
    end
    chatItem.gameObject:SetActive(true)

    local content = UnityTools.FindCo(chatItem.transform, "UILabel", "content") 
    local chatSign = UnityTools.FindGo(chatItem.transform, "chatsign")
    if chatMsg.content_type == 3 then
        chatSign.gameObject:SetActive(true)
        if bSelf then
            content.text = GetShowChatContent(chatMsg.content)
        else
            content.text = "    "..GetShowChatContent(chatMsg.content)
        end
    else        
        chatSign.gameObject:SetActive(false)
        content.text = GetShowChatContent(chatMsg.content)
    end   
    -- content.text = GetShowChatContent(chatMsg.content)
end

local function sendMsgCall(gameObject)
    local text = _inputText.value
    _inputText.value = LuaText.chat_win_default_text
    _needResetPos = true
    if string.len(text) ~= 0 and text ~= LuaText.chat_win_default_text then
        text = GameText.Instance:StrFilter(text, 42)
        roomMgr.SendChatMsg(0, text)
        ClickMaskAreaCall(gameObject)
    end
end

local function clickDragLayer(gameObject, isPress)
    _needResetPos = false
end

local function OnShowTab2(cellbox, index, item)
    -- item:SetActive(true)
    local chatMsg = _chatDefaultList[index + 1]
    local content = UnityTools.FindCo(item.transform, "UILabel", "content") 
    content.text = chatMsg.show_text
    -- local bgBtn = UnityTools.FindGo(item.transform, "bg")
    UnityTools.AddOnClick(item.gameObject, function() 
        roomMgr.SendChatMsg(0, "{#emoji}" .. chatMsg.key)
        ClickMaskAreaCall()
    end)
end

local function OnShowTab1(cellbox, index, item)
    -- item:SetActive(true)
    local emojiData = _emojiList[index + 1]
    local icon = UnityTools.FindCo(item.transform, "UISprite", "icon") 
    icon.spriteName = emojiData.show_text
    UnityTools.AddOnClick(item, function() 
        roomMgr.SendChatMsg(1, "{#emoji}" .. emojiData.key)
        ClickMaskAreaCall()
    end)
end

--- [ALF END]





local function updateChatTextListLayer()
    if roomMgr.GetChatInfoList() == nil then
        return nil 
    end
    local cnt = #roomMgr.GetChatInfoList() - _sv_tab3_mgr.CellCount
    for i = 1, cnt, 1 do
        _sv_tab3_mgr:NewCellsBox(_sv_tab3_mgr.Go)
    end
    -- 对scrollview、gridCellMgr重新排序
    _sv_tab3_mgr.Grid:Reposition()
    _sv_tab3_mgr:UpdateCells()
    if _needResetPos == true then
        _sv_tab3:ResetPosition() 
    end 
end

local function updateDefaultChatLayer()
    if _chatDefaultList == nil or #_chatDefaultList == 0 then
        return nil 
    end

    -- 遍历增加新的元素
    local cnt = #_chatDefaultList - _sv_tab2_mgr.CellCount
    for i = 1, cnt, 1 do
        _sv_tab2_mgr:NewCellsBox(_sv_tab2_mgr.Go)
    end

    -- 对scrollview、gridCellMgr重新排序
    
    _sv_tab2_mgr.Grid:Reposition()
    _sv_tab2_mgr:UpdateCells()
    _sv_tab2:ResetPosition()
    -- _sv_playerList_mgr:ScrollCellToIndex(0, 4, 400)
end

local function updateEmojiLayer()
    if _emojiList == nil or #_emojiList == 0 then
        return nil 
    end

    -- 遍历增加新的元素
    local cnt = #_emojiList - _sv_tab1_mgr.CellCount
    for i = 1, cnt, 1 do
        _sv_tab1_mgr:NewCellsBox(_sv_tab1_mgr.Go)
    end

    -- 对scrollview、gridCellMgr重新排序
    
    _sv_tab1_mgr.Grid:Reposition()
    _sv_tab1_mgr:UpdateCells()
    _sv_tab1:ResetPosition()
    -- _sv_playerList_mgr:ScrollCellToIndex(0, 4, 400)
end

local function clickTabCall(gameObject)
    local tabs = {_tab1, _tab2, _tab3}
    local funcs = {updateEmojiLayer, updateDefaultChatLayer, updateChatTextListLayer}
    for i = 1, 3, 1 do
        local dragLayer = UnityTools.FindGo(tabs[i].transform, "dragLayer")
        local scrollView = UnityTools.FindGo(tabs[i].transform, "scrollView")
        
        if gameObject.name == tabs[i].name then
            dragLayer:SetActive(true)
            scrollView:SetActive(true)
            roomMgr.ChatWinLastType = i
            tabs[i].value = true
            funcs[i]()
        else
            tabs[i].value = false
            dragLayer:SetActive(false)
            scrollView:SetActive(false)
        end
    end
end

local function clickInputCall(gameObject)
    local t = gTimer.registerOnceTimer(200, function() 
        _inputText.value = ""
    end)
    gTimer.setRecycler(wName, t)
end



-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
    _bgMask = UnityTools.FindGo(gameObject.transform, "Container/mask")
    UnityTools.AddOnClick(_bgMask.gameObject, ClickMaskAreaCall)

    _enterAction = UnityTools.FindCo(gameObject.transform, "TweenPosition", "Container/Layer/Win")

    _tab1 = UnityTools.FindCo(gameObject.transform, "UIToggle", "Container/Layer/Win/tabLayer/tab1")

    _tab2 = UnityTools.FindCo(gameObject.transform, "UIToggle", "Container/Layer/Win/tabLayer/tab2")

    _tab3 = UnityTools.FindCo(gameObject.transform, "UIToggle", "Container/Layer/Win/tabLayer/tab3")
    UnityTools.AddOnClick(_tab1.gameObject, clickTabCall)
    UnityTools.AddOnClick(_tab2.gameObject, clickTabCall)
    UnityTools.AddOnClick(_tab3.gameObject, clickTabCall)
    _cell1 = UnityTools.FindGo(gameObject.transform, "Container/Layer/Win/cell1")
    _cell1:AddComponent("UIDragScrollView")
    _sv_tab3 = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/Layer/Win/tabLayer/tab3/scrollView")
    _sv_tab3_mgr = UnityTools.FindCoInChild(_sv_tab3, "UIGridCellMgr")
    _sv_tab3_mgr.onShowItem = OnShowTab3
    _controller:SetScrollViewRenderQueue(_sv_tab3.gameObject)


    _inputText = UnityTools.FindCo(gameObject.transform, "UIInput", "Container/Layer/Win/InputText")
    UnityTools.AddOnClick(_inputText.gameObject, clickInputCall)

    _sendMsgBtn = UnityTools.FindGo(gameObject.transform, "Container/Layer/Win/send")
    UnityTools.AddOnClick(_sendMsgBtn.gameObject, sendMsgCall)

    _dragLayer = UnityTools.FindGo(gameObject.transform, "Container/Layer/Win/tabLayer/tab3/dragLayer")
    UIEventListener.Get(_dragLayer.gameObject).onPress = clickDragLayer

    _sv_tab2 = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/Layer/Win/tabLayer/tab2/scrollView")
    _sv_tab2_mgr = UnityTools.FindCoInChild(_sv_tab2, "UIGridCellMgr")
    _sv_tab2_mgr.onShowItem = OnShowTab2
    _controller:SetScrollViewRenderQueue(_sv_tab2.gameObject)

    _sv_tab1 = UnityTools.FindCo(gameObject.transform, "UIScrollView", "Container/Layer/Win/tabLayer/tab1/scrollView")
    _sv_tab1_mgr = UnityTools.FindCoInChild(_sv_tab1, "UIGridCellMgr")
    _sv_tab1_mgr.onShowItem = OnShowTab1
    _controller:SetScrollViewRenderQueue(_sv_tab1.gameObject)
    _thisObj = gameObject
    

--- [ALB END]


end

local function initChatConfig()
    local mySex = platformMgr.getSex()

    for index, data in pairs(LuaConfigMgr.ChatEmojiConfig) do
        if data.type == tostring(mySex) then
            _chatDefaultList[#_chatDefaultList + 1] = data
        elseif data.type == "2" then
            _emojiList[#_emojiList + 1] = data
        end
    end
end
local function resetBgmValue()
        -- _boxTaskTb.recordCount =0
    local bgm = UnityEngine.GameObject.Find("Scene"):GetComponent("AudioSource")
    -- bgm.enabled = true
    -- bgm:Play()
    local bgmValue = UnityEngine.PlayerPrefs.GetFloat("bgmValue", 50);
    bgm.volume = bgmValue/100.0;
    UnityEngine.PlayerPrefs.SetFloat("gameValue",_nEffectVolum);
end
local function OnChatOver()
    -- _recordCount = _recordCount-1
    _bPlaySound = false
    resetBgmValue()
end
function chatAudioReadyToStart(EventID, tMsgData)
    LogError("asdasdasd")
    if _bPlaySound ==false then
        return
    end
    if tMsgData.chat ~= nil then
        local tag =ComponentData.Get(tMsgData.chat.gameObject).Tag
        if tag == 30 or tag == 31 then
            return
        end
        local bgm = UnityEngine.GameObject.Find("Scene"):GetComponent("AudioSource")
        bgm.volume = 0
        _nEffectVolum = UnityEngine.PlayerPrefs.GetFloat("gameValue", 50);
        UnityEngine.PlayerPrefs.SetFloat("gameValue",0);
        local twAlpha = tMsgData.chat.gameObject:GetComponent("TweenAlpha")
        local sAudioInfo = ComponentData.Get(tMsgData.chat.gameObject).Text;
		local sAudioList = stringToTable(sAudioInfo, ",");
        LogError("sAudioList="..sAudioList[1])
        UnityTools.playAudio(sAudioList[1]);
        gTimer.registerOnceTimer(tonumber(sAudioList[2])*1000,OnChatOver)
		-- _recordCount = _recordCount + 1;
	end


end
local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)
    _needResetPos = true
    initChatConfig()
    registerScriptEvent(EVENT_ROOM_RECV_CHAT_INFO, "ChatMainWinRecvChatInfo")
    registerScriptEvent(EVENT_AUDIOLOAD_COMPLETE, "chatAudioReadyToStart");
    _nEffectVolum = UnityEngine.PlayerPrefs.GetFloat("gameValue", 50);
end


local function Start(gameObject)
    registerScriptEvent(EVENT_GAME_START_EFFECT, CloseWin)
    _enterAction:Play(true)
    local tabs = {_tab1, _tab2, _tab3}
    gTimer.registerOnceTimer(200, clickTabCall, tabs[roomMgr.ChatWinLastType])
end

local function OnDestroy(gameObject)
    resetBgmValue()
    unregisterScriptEvent(EVENT_GAME_START_EFFECT, CloseWin)
    _chatDefaultList = nil
    _emojiList = nil
    gTimer.recycling(wName)
    gTimer.removeTimer(OnChatOver);
    unregisterScriptEvent(EVENT_ROOM_RECV_CHAT_INFO, "ChatMainWinRecvChatInfo")
    unregisterScriptEvent(EVENT_AUDIOLOAD_COMPLETE, "chatAudioReadyToStart");
    CLEAN_MODULE("ChatMainWinMono")
end


function ChatMainWinRecvChatInfo(msgID, msgData)
    if UnityTools.IsWinShow(wName) == false or msgData.content_type ~= 0 then return nil end
    updateChatTextListLayer()
end

-- ------------------------
-- 模块导出设置
-- ------------------------
M.Awake = Awake
M.Start = Start
M.OnDestroy = OnDestroy



-- 返回当前模块
return M
