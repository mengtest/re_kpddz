-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    ShareExchangeWinController.lua
-- * Summary:     分享邀请码界面
-- *
-- * Version:     1.0.0
-- * Author:      MMCUXDSPE5IA8O3
-- * Date:        5/19/2017 8:49:41 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ShareExchangeWinController")
local _protoBuf=sluaAux.luaProtobuf.getInstance()

-- 界面名称
local wName = "ShareExchangeWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")


local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end

-- 领取新人奖励返回
function onShareNewbeeRewardReply(idMsg,tMsgData)
    if tMsgData == nil then
        return
    end

    if tMsgData.result == 0 then  -- 领取成功
		ShowAwardWin(tMsgData.rewards)
	elseif tMsgData.result == 1 then -- 领取失败
		UnityTools.ShowMessage(tMsgData.err)
	end

end


_protoBuf:registerMessageScriptHandler(protoIdSet.sc_share_new_bee_reward_reply, "onShareNewbeeRewardReply")
UI.Controller.UIManager.RegisterLuaWinFunc("ShareExchangeWin", OnCreateCallBack, OnDestoryCallBack)


-- 返回当前模块
return M
