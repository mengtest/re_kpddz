-- -----------------------------------------------------------------
-- * Copyright (c) 2017 福建瑞趣创享网络科技有限公司

-- *
-- * Filename:    ShareWinMono.lua
-- * Summary:     分享
-- *
-- * Version:     1.0.0
-- * Author:      32QBHGQ5YOWLCFH
-- * Date:        5/18/2017 8:25:34 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ShareWinMono")



-- 界面名称
local wName = "ShareWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)



-- 获取控制器模块
local CTRL = IMPORT_MODULE(wName .. "Controller")
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")

local _winBg;
local _tfBg = nil;
local _myCodeLabel = nil;
local _gotObj = nil;
local _card1Go = nil;
local _friendCountLabel = nil;
local _redPoint2 = nil;
local _redPoint3 = nil;

local function CloseWin(gameObject)
    UnityTools.CloseByAction(_winBg,wName)
end


local function GoBack(gameObject)
	CloseWin(gameObject);
end


-- 根据名字索引常量配置表
local function getValueByName(strName)
	for k, v in pairs(LuaConfigMgr.ClientBaseConfig) do
		if v.name == strName then
			return v
		end
	end
end




local function Card1OnClick(gameObject)
	if not true then  -- UtilTools.IsOfficialApp()
		UnityTools.MessageDialog(LuaText.GetString("shar_exchange_not_offical"))
		return;
	end

	local level = nil
	local levelData = getValueByName("new_reward_lvl")
	if levelData ~= nil then
		level = levelData.value
		if level ~= nil then
			level = tonumber(level)
		end
	end
	if level == nil then
		level = 2
	end
	-- if GameDataMgr.PLAYER_DATA.Level >= level then
	-- 	UnityTools.MessageDialog(LuaText.GetString("shar_exchange_level"))
	-- 	return
	-- end
	UnityTools.CreateLuaWin("ShareExchangeWin")
end

local function Card2OnClick(gameObject)
	UnityTools.CreateLuaWin("ShareSelectWin")
	UnityEngine.PlayerPrefs.SetString("IsTodayShared","1")
	_redPoint2:SetActive(false);
end

local function Card3OnClick(gameObject)
	UnityTools.CreateLuaWin("ShareFriendWin")
end

local function ShareTipOnClick()
	UnityTools.CreateLuaWin("ShareTipsWin")
end


local function AutoLuaBind(gameObject)
	_winBg = UnityTools.FindGo(gameObject.transform, "Container")
	_tfBg = _winBg.transform:Find("Background");
	
    local backGo = _tfBg.transform:Find("CloseButton").gameObject;
    UIEventListener.Get(backGo).onClick = GoBack;
	
	_gotObj = _tfBg.transform:Find("InnerBack/back/Card1/Got").gameObject;
    _card1Go = _tfBg.transform:Find("InnerBack/back/Card1/BtnGet").gameObject;
    UIEventListener.Get(_card1Go).onClick = Card1OnClick;
    local card2Go = _tfBg.transform:Find("InnerBack/back/Card2/BtnGet").gameObject;
    UIEventListener.Get(card2Go).onClick = Card2OnClick;
    local card3Go = _tfBg.transform:Find("InnerBack/back/Card3/BtnGet").gameObject;
    UIEventListener.Get(card3Go).onClick = Card3OnClick;
    local shareTipGo = _tfBg.transform:Find("BtnTip").gameObject;
    UIEventListener.Get(shareTipGo).onClick = ShareTipOnClick;
	_redPoint2 = _tfBg.transform:Find("InnerBack/back/Card2/BtnGet/Red").gameObject;
	_redPoint3 = _tfBg.transform:Find("InnerBack/back/Card3/BtnGet/Red").gameObject;
	
    local LabelCode = _tfBg.transform:Find("MyCodeBox/LabelCode");
	_myCodeLabel = LabelCode:GetComponent("UILabel");
	
    local LabelCount = _tfBg.transform:Find("InnerBack/back/Card2/TextBack/Text");
	_friendCountLabel = LabelCount:GetComponent("UILabel");
	
end

function ShareMainUpdate()
	if CTRL.Get == 1 then
		_gotObj:SetActive(true)
		_card1Go:SetActive(false)
	end
	if  UnityEngine.PlayerPrefs.GetString("IsTodayShared") == "0" then
		_redPoint2:SetActive(true)
	else
		_redPoint2:SetActive(false)
	end
	if CTRL.HasAward() then
		_redPoint3:SetActive(true)
	else
		_redPoint3:SetActive(false)
	end

	local strInviteDesc = string.format(LuaText.GetString("shar_friend_count"), CTRL.Count)
	_friendCountLabel.text = strInviteDesc
	_myCodeLabel.text = CTRL.MyCode
end

local function Awake(gameObject)
	AutoLuaBind(gameObject);
    UnityTools.OpenAction(_winBg);
	
	
	registerScriptEvent(EVENT_UPDATE_SHARE_MAIN_UPDATE, "ShareMainUpdate")
end


local function Start(gameObject)
	ShareMainUpdate()
	
	local desc = _tfBg.transform:Find("InnerBack/back/Card1/Desc")
	if desc ~= nil then
		LogWarn("ShareWinMono 1")
		if true then  -- 官方渠道(apple)  UtilTools.IsOfficialApp()
			LogWarn("ShareWinMono 2")
			desc:GetComponent("UILabel").text = LuaText.GetString("share_desc_1")
		else
			LogWarn("ShareWinMono 3")
			desc:GetComponent("UILabel").text = LuaText.GetString("shar_exchange_not_offical")
		end
	end
	
	-- 新人奖励
	local item1 = _tfBg.transform:Find("InnerBack/back/Card1/Item1/Item/img_bg")
	local iType = g_newbieShareAward_1
	local iNum = tonumber(getValueByName("share_new_reward").value)
	CTRL.setShareAwardUIComponentInfo(item1,iType,iNum, "award_")

	-- 邀请有奖
	local title = _tfBg.transform:Find("InnerBack/back/Card2/Desc")
	local strNum = getValueByName("share_friend_reward").value
	local strFmtDesc2 = LuaText.GetString("share_desc_2")
	local strDesc2 = string.format(strFmtDesc2, strNum)
	title:GetComponent("UILabel").text = strDesc2

	local item2 = _tfBg.transform:Find("InnerBack/back/Card2/Item1/Item/img_bg")
	iType = g_newbieShareAward_2
	CTRL.setShareAwardUIComponentInfo(item2,iType, tonumber(strNum), "award_")
	local strInviteDesc = string.format(LuaText.GetString("shar_friend_count"), CTRL.Count)
	_friendCountLabel.text = strInviteDesc
	
	-- 好友进阶
	local sType
	for i=1, 3 do
		sType = tostring(i)
		if LuaConfigMgr.ShareRewardConfig[sType] ~= nil then
			local item_transform = _tfBg.transform:Find("InnerBack/back/Card3/ListContainer/Item" .. sType)
			-- local desc_label = item_transform.transform:Find("ItemDes").gameObject
			-- desc_label:GetComponent("UILabel").text = LuaConfigMgr.ShareRewardConfig[sType]["descirbe"]
			if LuaConfigMgr.ShareRewardConfig[sType]["reward"] ~= nil then
				local icon_transform = item_transform.transform:Find("Item/img_bg")
				
				iType = tonumber(LuaConfigMgr.ShareRewardConfig[sType]["reward"][1][2])
				iNum = tonumber(LuaConfigMgr.ShareRewardConfig[sType]["reward"][1][3])
				LogWarn(tostring(icon_transform) .. ", " .. iType .. ", " .. iNum)
				CTRL.setShareAwardUIComponentInfo(icon_transform,iType,iNum,"award_")
			end
		end
	end

	-- 红点提示
	if  UnityEngine.PlayerPrefs.GetString("IsTodayShared") == "0" then
		_redPoint2:SetActive(true)
	else
		_redPoint2:SetActive(false)
	end
	if CTRL.HasAward() then
		_redPoint3:SetActive(true)
	else
		_redPoint3:SetActive(false)
	end
end


local function OnDestroy(gameObject)
	unregisterScriptEvent(EVENT_UPDATE_SHARE_MAIN_UPDATE, "ShareMainUpdate")
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
