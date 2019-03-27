-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    ShareFriendWinMono.lua
-- * Summary:     分享的好友列表
-- *
-- * Version:     1.0.0
-- * Author:      MMCUXDSPE5IA8O3
-- * Date:        5/18/2017 9:41:47 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ShareFriendWinMono")



-- 界面名称
local wName = "ShareFriendWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)


-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
local CTRL_MAIN = IMPORT_MODULE("ShareWinController")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")
local _winBg = nil;
local _scrollview = nil;
local _cellMgr = nil;
local _nullBox = nil;
local _go
local function OnClickGetAward(gameObject)
	local friendId = ComponentData.Get(gameObject).Text
	local iType = ComponentData.Get(gameObject).Tag
    local protobuf = sluaAux.luaProtobuf.getInstance()
    local req ={}
    req.friend_id = friendId
    req.type = iType
    protobuf:sendMessage(protoIdSet.cs_share_mission_reward_req, req)
end

local function UpdateList()
	if #CTRL_MAIN.FriendList == 0 then
		_nullBox:SetActive(true)
	else
		_nullBox:SetActive(false)
	end
	_cellMgr:ClearCells()
	for i=1,#CTRL_MAIN.FriendList do
		_cellMgr:NewCellsBox(_cellMgr.Go)
	end
	_cellMgr.Grid:Reposition()
	_cellMgr:UpdateCells()
end

local function OnShowItem(cellbox, index, item)
	if index >= #CTRL_MAIN.FriendList then
		return
	end
	local data = CTRL_MAIN.FriendList[index+1]
	local head_transform = UnityTools.FindGo(item.transform, "HeadBox/hero_img_bg").transform
    local vip_sprite = UnityTools.FindGo(item.transform, "HeadBox/hero_img_bg/mask/vip/vipBox")
    local name_label = UnityTools.FindGo(item.transform,"Name").gameObject:GetComponent("UILabel")
    local desc_label = UnityTools.FindGo(item.transform,"Desc").gameObject:GetComponent("UILabel")
    local getObj = UnityTools.FindGo(item.transform,"BtnGet").gameObject
	local item_transform = UnityTools.FindGo(item.transform, "Item/img_bg").transform
	ComponentData.Get(getObj).Text = data.friend_id
	ComponentData.Get(getObj).Tag = data.type

	-- 设置头像
	local goCustomerHeadTexture =  UnityTools.FindGo(head_transform, "img") 
	local goDefaultHeadIcon = UnityTools.FindGo(head_transform, "headIcon")
	if data.head == "0" or data.head == "1" then
		goDefaultHeadIcon:SetActive(true)
		goCustomerHeadTexture:SetActive(false)

		local sprDefaultHeadIcon = goDefaultHeadIcon:GetComponent("UISprite")
		sprDefaultHeadIcon.spriteName = data.head == "0" and "boyHead" or "girlHead"
	else
		goDefaultHeadIcon:SetActive(false)
		goCustomerHeadTexture:SetActive(true)

		local textureHead = goCustomerHeadTexture:GetComponent("UITexture")
		UnityTools.SetPlayerHead(tostring(data.head), textureHead, true);
	end

	-- 设置VIP图标
	if data.vip_lv >= 0 then
		UnityTools.SetNewVipBox(vip_sprite,data.vip,"vip",_go)
		-- vip_sprite.spriteName = "v" .. tostring(data.vip_lv)
	else
		-- vip_sprite.spriteName = ""
		UnityTools.SetNewVipBox(vip_sprite,0,"vip")
	end

	-- 设置名字
	name_label.text = data.name
	local sType = tostring(data.type)
	if LuaConfigMgr.ShareRewardConfig[sType] ~= nil then
		desc_label.text = LuaConfigMgr.ShareRewardConfig[sType]["descirbe"]
		if LuaConfigMgr.ShareRewardConfig[sType]["reward"] ~= nil then
			local iType = tonumber(LuaConfigMgr.ShareRewardConfig[sType]["reward"][1][2])
			local iNum = tonumber(LuaConfigMgr.ShareRewardConfig[sType]["reward"][1][3])
			CTRL_MAIN.setShareAwardUIComponentInfo(item_transform,iType,iNum)
		end
	end
	UnityTools.AddOnClick(getObj, OnClickGetAward)
end

local function CloseWin(gameObject)
    UnityTools.DestroyWin(wName)
end


local function AutoLuaBind(gameObject)
	_winBg = UnityTools.FindGo(gameObject.transform, "Container")
	
    local backGo = _winBg.transform:Find("closeBtn").gameObject
    UIEventListener.Get(backGo).onClick = CloseWin
    _scrollview = _winBg.transform:Find("bg/bg2/scrollview"):GetComponent("UIScrollView") 
	_nullBox = _winBg.transform:Find("bg/bg2/NullBox").gameObject

    _cellMgr = _winBg.transform:Find("bg/bg2/scrollview/Grid"):GetComponent("UIGridCellMgr")
    _cellMgr.onShowItem = OnShowItem
    _controller:SetScrollViewRenderQueue(_scrollview.gameObject)
end

function ShareFriendUpdate()
	UpdateList()
end


local function Awake(gameObject)
	_go = gameObject
	AutoLuaBind(gameObject);
	registerScriptEvent(EVENT_UPDATE_SHARE_MAIN_UPDATE, "ShareFriendUpdate")
end


local function Start(gameObject)
	UpdateList()
end


local function OnDestroy(gameObject)
	unregisterScriptEvent(EVENT_UPDATE_SHARE_MAIN_UPDATE, "ShareFriendUpdate")
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
