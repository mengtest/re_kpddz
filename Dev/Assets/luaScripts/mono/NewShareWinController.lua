-- -----------------------------------------------------------------
-- * Copyright (c) 2018 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    NewShareWinController.lua
-- * Summary:     NewShareWin
-- *
-- * Version:     1.0.0
-- * Author:      WIN701207261038
-- * Date:        1/25/2018 4:43:22 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("NewShareWinController")



-- 界面名称
local wName = "NewShareWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local UnityTools = IMPORT_MODULE("UnityTools")
local protobuf = sluaAux.luaProtobuf.getInstance()
M.lottoCount = 0
M.lottoCountSeven = 0
M.lottoCountOne = 0
M.beginTime = 0
M.TabIndex = -1
local function ShareToWeChatFriend()
    UtilTools.ShareWeChat(GameDataMgr.PLAYER_DATA.Account,LuaText.GetString("shar_friend_big_gift"),LuaText.GetString("shar_friend_desc"))
end
local function ShareToWeBoFriend()

end
-- 可领取的分享奖励个数
local function availableShareAwardCount()
	local count = 0
	-- 红点提示
	count = M.lottoCount+M.lottoCountSeven+M.lottoCountOne

	return count
end

local function ShareToWeQQFriend()
    
end
local function ShareToMoments()
    local descStr = "new_share_desc"..math.random(1,10)
    local picCount = BarcodeCam.getInstance().GetPicListLenth()
    local selecetIndex = math.random(2,picCount-1)
    UtilTools.ShareWeChatPic(1, tostring(GameDataMgr.PLAYER_DATA.Account), GameText.GetStr(descStr),BarcodeCam.getInstance().GetSharePic(0),BarcodeCam.getInstance().GetSharePic(1),BarcodeCam.getInstance().GetSharePic(selecetIndex),"","","")
    local protobuf = sluaAux.luaProtobuf.getInstance()
	protobuf:sendMessage(protoIdSet.cs_share_with_friends_req,{})
end

local function OnCreateCallBack(gameObject)
  
end


local function OnDestoryCallBack(gameObject)
    M.beginTime = 0
    M.TabIndex = -1
end

function OnShareRankResponse(msgId, msgData)
    LogWarn("-kkkkkkkkkkkkkkkk--OnShareRankResponse--")
    if msgData == nil then
        return
    end
    M.beginTime = msgData.beginTime
    triggerScriptEvent(EVENT_SHARE_RANK_RESPONSE, msgData)
end

function OnShareHistoryResponse(msgId, msgData)
    LogWarn("-kkkkkkkkkkkkkkkk--OnShareHistoryResponse--")
    UtilTools.HideWaitFlag()
    if msgData == nil then
        return
    end
  
    triggerScriptEvent(EVENT_SHARE_HISTORY_RESPONSE, msgData)
end

function OnLottoInfoResponse(msgId, msgData)
    LogWarn("-kkkkkkkkkkkkkkkk--OnLottoInfoResponse--")
    if msgData == nil then
        return
    end
    M.lottoCount = msgData.draw_count
    M.lottoCountSeven = msgData.draw_count_seven
    M.lottoCountOne = msgData.draw_count_one
    triggerScriptEvent(UPDATE_MAIN_WIN_RED,"shareAward")
end

function OnLottoResponse(msgId, msgData)
    LogWarn("-kkkkkkkkkkkkkkkk--OnLottoResponse--")
    if msgData == nil then
        return
    end
    if msgData.result == 0 then
        triggerScriptEvent(EVENT_NEW_SHARE_LOTTO_RESPONSE, msgData.index)
    else
        UnityTools.ShowMessage(tMsgData.err)
    end
end
local function ShowShareWin(tabIndex)
    M.TabIndex = tabIndex
    UnityTools.CreateLuaWin(wName)
end

protobuf:registerMessageScriptHandler(protoIdSet.sc_share_draw_response, "OnLottoResponse") 
protobuf:registerMessageScriptHandler(protoIdSet.sc_draw_count_response, "OnLottoInfoResponse") 
protobuf:registerMessageScriptHandler(protoIdSet.sc_share_history_response, "OnShareHistoryResponse")
protobuf:registerMessageScriptHandler(protoIdSet.sc_share_rank_response, "OnShareRankResponse") 

UI.Controller.UIManager.RegisterLuaWinFunc("NewShareWin", OnCreateCallBack, OnDestoryCallBack)
M.ShareToMoments = ShareToMoments
M.ShareToWeChatFriend =ShareToWeChatFriend
M.ShowShareWin = ShowShareWin
M.ShareToWeBoFriend = ShareToWeBoFriend
M.ShareToWeQQFriend = ShareToWeQQFriend
M.availableShareAwardCount = availableShareAwardCount
-- 返回当前模块
return M
