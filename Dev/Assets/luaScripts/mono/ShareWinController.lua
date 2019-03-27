-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    ShareWinController.lua
-- * Summary:     分享
-- *
-- * Version:     1.0.0
-- * Author:      32QBHGQ5YOWLCFH
-- * Date:        5/18/2017 8:25:34 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ShareWinController")
M.MyCode=""--我的邀请码
M.FriendCode=""--分享给我的邀请码
M.Get=0--1 已领
M.Count=0--分享好友数
M.FriendList={}--好友任务列表

-- 分享新人奖励物品
g_newbieShareAward_1 = 101
g_newbieShareAward_2 = 101


-- 界面名称
local wName = "ShareWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local _protoBuf=sluaAux.luaProtobuf.getInstance()



local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end

function HasAward()
	if M.FriendList == nil or #M.FriendList == 0 then
		return false
	else
		return true
	end
end

-- 可领取的分享奖励个数
local function availableShareAwardCount()
	local count = 0
	-- 红点提示
	if  UnityEngine.PlayerPrefs.GetString("IsTodayShared") == "0" then
		count = count + 1
	end
	
	if HasAward() then
		count = count + #M.FriendList
	end

	return count
end



-- 设置分享奖励UI组件信息
local function setShareAwardUIComponentInfo(tfItem, idItem, nCount, strPrefix)
	local UnityTools = IMPORT_MODULE("UnityTools")
	-- 设置奖励物品icon
	local itemIcon = UnityTools.FindCo(tfItem, "UISprite", "img")
	local prfx = strPrefix or "C"
	if itemIcon ~= nil then
	    itemIcon.spriteName = prfx .. tostring(idItem)
	end	
	-- 设置数量
	local lblCount = UnityTools.FindCo(tfItem, "UILabel",  "num")
	if lblCount ~= nil then
	    local strCount = tostring(nCount)
	    if idItem == 109 then
	        strCount = nCount/10 .. "元"
	    end
	    lblCount.text = strCount
	end
end

-- 分享成功通知服务端
function ShareSuccess()
    local protobuf = sluaAux.luaProtobuf.getInstance()
	local req = {}
    req.type = 2;
    protobuf:sendMessage(protoIdSet.cs_niu_subsidy_req, req);
end

-- 初始分享奖励消息
function OnShareInfoResponse(idMsg,tMsgData)
	M.MyCode = tMsgData.my_code
	M.FriendCode = tMsgData.code
	M.Get = (tMsgData.free and 1 or 0)
	M.Count = tMsgData.count

	local unGetList = {}
	local count = 0;
	if tMsgData.list ~= nil then
		for i,v in pairs(tMsgData.list) do
			if v.status == 1 then
				count = count+1
				unGetList[count]=v
			end
		end
	end
	M.FriendList = unGetList
	triggerScriptEvent(EVENT_UPDATE_SHARE_MAIN_UPDATE,{})

	-- 主界面红点更新
	triggerScriptEvent(UPDATE_MAIN_WIN_RED,"shareAward")
end

-- 分享更新消息
function onShareMissionUpdate(idMsg,tMsgData)
	M.Count = tMsgData.count
	local unGetList = {}
	local count = 0
	if tMsgData.list ~= nil then
		for k1, v1 in pairs(tMsgData.list) do
			local bIsNew = true
			for k2, v2 in pairs(M.FriendList) do
				if v1.friend_id == v2.friend_id and v1.type == v2.type then
					bIsNew = false
					M.FriendList[k2] = v1  -- 更新
				end
			end

			if bIsNew then
				table.insert(unGetList, v1)  -- 新增
			end
		end

		-- 合并
		for k, v in pairs(unGetList) do
			table.insert(M.FriendList, v)
		end

		-- 主界面红点更新
		triggerScriptEvent(UPDATE_MAIN_WIN_RED,"shareAward")
	end

	triggerScriptEvent(EVENT_UPDATE_SHARE_MAIN_UPDATE,{})
end


-- 监听消息 
_protoBuf:registerMessageScriptHandler(protoIdSet.sc_share_info, "OnShareInfoResponse")
_protoBuf:registerMessageScriptHandler(protoIdSet.sc_share_mission_update, "onShareMissionUpdate")


UI.Controller.UIManager.RegisterLuaFuncCall("ShareWin:ShareSuccess", ShareSuccess)
UI.Controller.UIManager.RegisterLuaWinFunc("ShareWin", OnCreateCallBack, OnDestoryCallBack)



-- 导出接口
M.HasAward = HasAward
M.availableShareAwardCount = availableShareAwardCount
M.setShareAwardUIComponentInfo = setShareAwardUIComponentInfo

-- 返回当前模块
return M
