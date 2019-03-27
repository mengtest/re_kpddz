-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    ShareFriendWinController.lua
-- * Summary:     分享的好友列表
-- *
-- * Version:     1.0.0
-- * Author:      MMCUXDSPE5IA8O3
-- * Date:        5/18/2017 9:41:48 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ShareFriendWinController")



-- 界面名称
local wName = "ShareFriendWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local _protoBuf=sluaAux.luaProtobuf.getInstance()

local UnityTools =IMPORT_MODULE("UnityTools")
local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end



-- 领取新人奖励返回
function onShareMissionRewardReply(idMsg,tMsgData)
    if tMsgData == nil then
        return
    end

    if tMsgData.result == 0 then  -- 领取成功
		ShowAwardWin(tMsgData.rewards)

        -- 更新奖励状态
        local shareWinCtrl = IMPORT_MODULE("ShareWinController")
        local unGetList = {}
        local count = 0
        for i,v in pairs(shareWinCtrl.FriendList) do
			if not (v.friend_id == tMsgData.friend_id and v.type == tMsgData.type) then
				count = count+1
				unGetList[count]=v
			end
		end
	    shareWinCtrl.FriendList = unGetList
	    triggerScriptEvent(EVENT_UPDATE_SHARE_MAIN_UPDATE,{})

		-- 主界面红点更新
		triggerScriptEvent(UPDATE_MAIN_WIN_RED,"shareAward")

	elseif tMsgData.result == 1 then -- 领取失败
		UnityTools.ShowMessage(tMsgData.err)
	end

end


_protoBuf:registerMessageScriptHandler(protoIdSet.sc_share_mission_reward_reply, "onShareMissionRewardReply")
UI.Controller.UIManager.RegisterLuaWinFunc("ShareFriendWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
