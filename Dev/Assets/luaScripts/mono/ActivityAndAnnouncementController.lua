-- -----------------------------------------------------------------


-- *
-- * Filename:    ActivityAndAnnouncementController.lua
-- * Summary:     活动和公告信息界面
-- *
-- * Version:     1.0.0
-- * Author:      WP.Chu
-- * Date:        3/25/2017 1:56:29 PM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ActivityAndAnnouncementController")


-- 界面名称
local wName = "ActivityAndAnnouncement"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
-- proto消息
local protobuf = sluaAux.luaProtobuf.getInstance()
-- 载入工具模块
local UnityTools = IMPORT_MODULE("UnityTools")


-- /////////////////////////////////////////////////////////////////////////////////////////////
-- 通用数据结构、常量、枚举定义

-- 奖励状态(优先级: 低->高)
local EAwardGetStatus = {
	eNone 		= 0,	-- 无
	eGot		= 1,	-- 已领取
	eUnderway	= 2,	-- 进行中
	eComplete 	= 3,	-- 可领取
}

-- 活动类型
local EActivityType = {
	eNormal = 1,	-- 常规
	eOther = 2,		-- 其他类型
}

-- 活动标签
local EActivityTag = {
	eNone 	= 0,	-- 无
	eHot	= 1,	-- 热
	eNew	= 2,	-- 新
}

-- 窗口激活状态
local EWinActiveStatus = {
	eAnnouncement = 1,  -- 公告
	eActivity = 2,	-- 活动
}

-- 奖励物品数据
local CAwardItem = {
	idItem = 0;
	nCount = 0;
}

function CAwardItem:new(idItem, nCount)
	local o = {}
	setmetatable(o, self)
	self.__index = self

	-- 保存数据
	o.idItem = idItem
	o.nCount = nCount
	return o
end

-- /////////////////////////////////////////////////////////////////////////////////////////////
-- 常规活动任务, 奖励数据相关接口

-- 活动任务数据
local CNormalActAwardData = {
	_idTask = 0,		-- 任务ID
	_strTitle = "",		-- 任务标题
	_nCurrentValue = 0,	-- 当前进度值
	_nMaxValue = 0,		-- 最大进度值
	_tAwardItems = {},	-- 任务奖励物品
	_eStatus = EAwardGetStatus.eNone,  -- 领取状态1
}

-- constructor
function CNormalActAwardData:new(o)
	local o = o or {}
	setmetatable(o, self)
	self.__index = self

	-- 奖励表,因为是引用，所以需要单独建立
	o._tAwardItems = {}
	return o
end

-- 活动任务id
function CNormalActAwardData:idTask()
	return self._idTask;
end

-- 标题
function CNormalActAwardData:titleName()
	return self._strTitle;
end

-- 进度值-数值
function CNormalActAwardData:progressNumber()
	return self._nCurrentValue / self._nMaxValue;
end

-- 进度值-字符串表示
function CNormalActAwardData:progressString()
	return tostring(self._nCurrentValue) .. "/" .. tostring(self._nMaxValue)
end

-- 添加奖励物品
function CNormalActAwardData:addAwardItem(objItem)
	table.insert(self._tAwardItems, objItem) 
end

-- 奖励物品迭代器
function CNormalActAwardData:awardItemsIter()
	return next, self._tAwardItems, nil
end

-- 根据索引获取奖励物品数据
function CNormalActAwardData:awardItemByIndex(idx)
	return self._tAwardItems[idx]
end

-- 奖励领取状态
function CNormalActAwardData:status()
	return self._eStatus;
end


-- /////////////////////////////////////////////////////////////////////////////////////////////
-- 活动相关数据和接口

local CAcitivty = {
	_id = 0,   						-- 活动id
	_name = "",						-- 活动名字
	_eType = EActivityType.eNormal, -- 常规
	_eTag = EActivityTag.eNone,		-- 标签
	_strRules = "",					-- 规则信息
	_strAdImgName = "",				-- 广告图
	_strMainTitleImgName = "",		-- 主标题图片名字
	_strSubTitle = "",				-- 副标题
	_strDataTitleFmt = "",			-- 数据标题格式
	_nStartTime = 0,				-- 开始时间
	_nEndTime = 0,					-- 结束时间
	_nCurrentData = 0,				-- 当前数据
	_tAwardDatas = {}, 				-- 奖励数据

	-- 脏标记
	_bDirty = false,				-- 脏数据
	
	-- 额外的存储值, 以key-value的方式存储, 比如: recharge=100
	_tExtraValues = {},
}

-- constructor
function CAcitivty:new(o)
	local o = o or {}
	setmetatable(o, self)
	self.__index = self

	o._tAwardDatas = {}
	o._tExtraValues = {}

	return o
end

-- 活动类型
function CAcitivty:getType()
	return self._eType
end

-- 活动id
function CAcitivty:id()
	return self._id
end

-- 名字
function CAcitivty:name()
	return self._name
end

-- 活动标签
function CAcitivty:tag()
	return self._eTag
end

-- 活动规则
function CAcitivty:rules()
	return self._strRules;
end

-- 广告图
function CAcitivty:strAdImgName(isAlphaSplit)
	if isAlphaSplit then
		return self._strAdImgName .. "_RGB.png", self._strAdImgName .. "_Alpha.png"
	else
		return self._strAdImgName .. ".png"
	end
end

-- 主标题图片名字
function CAcitivty:mainTitleImgName(isAlphaSplit)
	if isAlphaSplit then
		return self._strMainTitleImgName .. "_RGB.png", self._strMainTitleImgName .. "_Alpha.png"
	else
		return self._strMainTitleImgName .. ".png"
	end
end

-- 副标题名字
function CAcitivty:strSubTitle()
	return self._strSubTitle
end

-- 开始时间, 返回格式: year,month(1-12), day(1-31), hour(0-23)
function CAcitivty:startTimeYYYYMMDDHH()
	local t= os.date("*t", self._nStartTime)
	return t.year, t.month, t.day, t.hour
end

-- 结束时间, 返回格式: year,month(1-12), day(1-31), hour(0-23)
function CAcitivty:endTimeYYYYMMDDHH()
	local t= os.date("*t", self._nEndTime)
	return t.year, t.month, t.day, t.hour
end

-- 当前数据
function CAcitivty:currentData()
	return self._nCurrentData
end

-- 数据标题
function CAcitivty:dataTitle()
	return string.format(self._strDataTitleFmt, self._nCurrentData)
end


-- 添加活动奖励数据
function CAcitivty:addAwardData(objAwardData)
	if objAwardData == nil then
		return
	end

	for k, v in ipairs(self._tAwardDatas) do
		if v:idTask() == objAwardData:idTask() then
			return
		end
	end

	table.insert(self._tAwardDatas, objAwardData)

	-- 排序
	self:resort()	
end

-- 重新排序
function CAcitivty:resort()
	table.sort(self._tAwardDatas, function(v1, v2)
		local s1 = v1._eStatus
		local s2 = v2._eStatus
		if s1 == s2 then
			return v1._idTask < v2._idTask
		else
			return s1 > s2
		end
	end)
end


-- 奖励数据迭代器
function CAcitivty:awardDatasIter()
	return next, self._tAwardDatas, nil
end

-- 根据索引获取奖励数据
function CAcitivty:getAwardDataByIndex(idx)
	return self._tAwardDatas[idx]
end

-- 根据id获取奖励数据
function CAcitivty:getAwardDataById(idAwardItem)
	for k, v in ipairs(self._tAwardDatas) do
		if v:idTask() == idAwardItem then
			return v
		end
	end

	return nil
end

-- 设置额外数据
function CAcitivty:setExtraValue(key, value)
	self._tExtraValues[key] = value
end

-- 获取额外数据
function CAcitivty:getExtraValue(key)
	return self._tExtraValues[key]
end

-- 红点提示是否可见
function CAcitivty:isRedDotVisible()
	for k, v in pairs(self._tAwardDatas) do
		if v:status() == EAwardGetStatus.eComplete then
			return true
		end
	end

	return false
end

-- 设置脏标记
function CAcitivty:setDirty()
	self._bDirty = true
end

-- 是否脏数据
function CAcitivty:isDirty()
	return self._bDirty
end


-- /////////////////////////////////////////////////////////////////////////////////////////////
-- 公告相关数据结构

local CGameAnnouncement = {
	_id = 0,			-- 公告编号
	_strTitle = "",		-- 标题
	_strContent = "",	-- 内容
}

-- constructor
function CGameAnnouncement:new(o)
	local o = o or {}
	setmetatable(o, self)
	self.__index = self
	return o
end

-- 标题
function CGameAnnouncement:title()
	return self._strTitle
end

-- 内容
function CGameAnnouncement:content()
	return self._strContent
end

-- /////////////////////////////////////////////////////////////////////////////////////////////
-- 活动数据管理相关接口

-- 管理类
local ActivitiesManager = {
	_bInitialized = false,  -- 是否初始化

	_eWinActiveStatus = EWinActiveStatus.eActivity,  -- 当前激活的子窗口
	activeActivityIdx = 0,	-- 当前激活的活动索引
	activeAnnoucementIdx = 0,	-- 当前激活的公告索引

	_tActivies = {}, 		-- 活动列表
	_tAnnnouncements = {}, 	-- 公告列表

	hideFuntion = 0,  -- 隐藏功能
}

-- 初始化
function ActivitiesManager:init()
	if self._bInitialized then
		return
	end
	self._bInitialized =  true

	-- 初始化公告数据
	local gameAnnouncement = ___activity_announcement_config___.announcement
	if gameAnnouncement ~= nil then
		for k, v in pairs(gameAnnouncement) do
			self:addAnnouncement(CGameAnnouncement:new{
				_id = k,			
				_strTitle = v.title,
				_strContent = v.content,
			})
		end

		-- 设置初始公告索引
		if table.getn(self._tAnnnouncements) > 0 then
			self.activeAnnoucementIdx = 1
		end
	end
end

-- 是否初始化
function ActivitiesManager:isInitialized()
	return self._bInitialized
end

-- 添加活动
function ActivitiesManager:addAcivity(objActivity)
	if objActivity == nil then
		return
	end

	for k, v in ipairs(self._tActivies) do
		if v:id() == objActivity:id() then
			return
		end
	end

	table.insert(self._tActivies, objActivity)

	-- 排序
	table.sort(self._tActivies, function(v1, v2)
		local id1 = v1._id
		local id2 = v2._id
		return id1 < id2
	end)
end


--主界面上活动按钮是否显示红点及红点数
function ActivitiesManager:MainWinRedCount()
	local count = 0
	for k, v in ipairs(self._tActivies) do

		for j, m in pairs(v._tAwardDatas) do
			if m:status() == EAwardGetStatus.eComplete then
				count = count + 1
			end
		end
	end

	return count
 end

-- 根据索引获取活动数据
function ActivitiesManager:getActivityByIndex(index)
	return self._tActivies[index]
end

-- 当前激活的活动
function ActivitiesManager:activeActivity()
	return self._tActivies[self.activeActivityIdx]
end

-- 根据id获取活动数据
function ActivitiesManager:getActivityById(idActivity)
	for k, v in ipairs(self._tActivies) do
		if v:id() == idActivity then
			return v
		end
	end
 
	return nil
end

-- 活动id转为索引
function ActivitiesManager:id2Index(idActivity)
	for k, v in ipairs(self._tActivies) do
		if v:id() == idActivity then
			return k
		end
	end

	return nil
end

-- 添加公告
function ActivitiesManager:addAnnouncement(objAnnouncement)
	if objAnnouncement == nil then
		return
	end

	for k, v in ipairs(self._tAnnnouncements) do
		if v._id == objAnnouncement._id then
			return
		end
	end

	table.insert(self._tAnnnouncements, objAnnouncement)

	-- 排序
	table.sort(self._tAnnnouncements, function(v1, v2)
		local id1 = v1._id
		local id2 = v2._id
		return id1 < id2
	end)
end

-- 根据索引获取公告数据
function ActivitiesManager:getAnnoucementyIndex(index)
	return self._tAnnnouncements[index]
end

-- 当前激活的公告
function  ActivitiesManager:activeAnnoucement()
	return self._tAnnnouncements[self.activeAnnoucementIdx]
end

-- 活动数据迭代器
function ActivitiesManager:activitiesIter()
	return next, self._tActivies, nil
end

-- 公告数据迭代
function ActivitiesManager:annoucementsIter()
	return next, self._tAnnnouncements, nil
end

-- 当前激活页面
function ActivitiesManager:getWinActiveStatus()
	return self._eWinActiveStatus;
end

-- 设置当前激活页面
function ActivitiesManager:setWinActiveStatus(status)
	self._eWinActiveStatus = status
end

-- 发送活动查询消息
function ActivitiesManager:sendActivityQueryInfo(idActivity)
	local tMsg = {}
	if idActivity > 2 then
		return
	end
	tMsg.id = idActivity
    protobuf:sendMessage( protoIdSet.cs_activity_info_query_req, tMsg)
end

-- 发送活动奖励领取请求
function ActivitiesManager:sendActivityDrawReq(idActivity, idAward)
	local tMsg = {}
	tMsg.activity_id = idActivity
	tMsg.sub_id = idAward
    protobuf:sendMessage( protoIdSet.cs_activity_draw_req, tMsg)
end

-- 是否隐藏窗口
function ActivitiesManager:isHideWindow()
	if self.isHideWindow == 1 then
		return true
	end

	return false
end

-- 设置活动数据脏标记
function ActivitiesManager:setActivityDirty()
	for k, v in ipairs(self._tActivies) do
		v:setDirty()
	end
end

-- 红点个数
function ActivitiesManager:redDotCount()
	local nCount = 0
	for k, v in ipairs(self._tActivies) do
		if v:isRedDotVisible() then
			nCount = nCount + 1
		end
	end

	return nCount
end

-- 查询所有活动信息
function ActivitiesManager:queryAllActivityInfo()
	for k, v in ipairs(self._tActivies) do
		self:sendActivityQueryInfo(v:id())
	end
end


-- 清理数据
function ActivitiesManager:clear()
	self._bInitialized = false
	self._eWinActiveStatus = EWinActiveStatus.eActivity
	self.activeActivityIdx = 0
	self.activeAnnoucementIdx = 0
	self._tActivies = {}
	self._tAnnnouncements = {}
end

-- /////////////////////////////////////////////////////////////////////////////////////////////
-- 服务端通信相关处理

-- 接受活动配置消息
function onActivityConfigInfoUpdate(msgId, tMsgData)
	if tMsgData == nil or tMsgData.activity_list == nil then
		print("活动数据错误")
		return
	end

	-- 隐藏标记
	ActivitiesManager.isHideWindow = tMsgData.hide_function_flag

	-- 屏蔽功能
	local _platformMgr = IMPORT_MODULE("PlatformMgr")
    _platformMgr.config_vip = tMsgData.hide_function_flag == 0;
    triggerScriptEvent(EVNET_HIDE_FUNCTION_FLAG_UDPATE,_platformMgr.config_vip);

	-- 全局配置表
	local cfg = ___activity_announcement_config___.activity
	local tAuxQuick = {}  -- 快速辅助表
	for i=1,LuaConfigMgr.ActivityTitleConfigLen do 
		tMsgData.activity_list[i]={}
		tMsgData.activity_list[i].activity_data= {}
		tMsgData.activity_list[i].activity_data.id = i
		tMsgData.activity_list[i].activity_data.current_data = 0
		tMsgData.activity_list[i]._bDirty = false
	end
	for k, v in ipairs(tMsgData.activity_list) do
		local actInfo = v.activity_data
			-- 创建活动对象
			local objAct = CAcitivty:new()
			objAct._id = actInfo.id  -- 活动ID
			objAct._nCurrentData =  actInfo.current_data -- 活动当前获取的数据

			-- 已领取的奖励
			if actInfo.draw_info_list ~= nil then
				for _, idAward in ipairs(actInfo.draw_info_list) do
					tAuxQuick[idAward] = true
				end
			end

			-- 活动奖励条目信息
			local actConfig = LuaConfigMgr.ActivityTitleConfig[tostring(actInfo.id)]
			local actAwardListInfo = v.sub_list
			if actAwardListInfo ~= nil then
				objAct._eType = EActivityType.eNormal -- 类型： 常规活动
				for kIdx, vAward in ipairs(actAwardListInfo) do
					local objAward = CNormalActAwardData:new()
					objAward._idTask = vAward.id  -- 条目id
					if LuaConfigMgr[actConfig.activityTable] ~= nil then
						objAward._strTitle = tostring(LuaConfigMgr[actConfig.activityTable][tostring(vAward.id)].describe) -- 标题
					else
						objAward._strTitle = tostring(kIdx)
					end
					objAward._nCurrentValue = objAct._nCurrentData -- 当前值
					objAward._nMaxValue = vAward.data -- 目标值

					-- 设置领取状态
					if tAuxQuick[objAward._idTask] then
						objAward._eStatus = EAwardGetStatus.eGot
					elseif objAct._nCurrentData >= vAward.data then
						objAward._eStatus = EAwardGetStatus.eComplete
					else
						objAward._eStatus = EAwardGetStatus.eUnderway
					end

					-- 保存奖励物品数据
					if vAward.reward_list ~= nil then
						for _, reward in ipairs(vAward.reward_list) do
							local objAwardItem = CAwardItem:new(reward.base_id, reward.count)
							objAward:addAwardItem(objAwardItem)
						end
					end

					-- 奖励物品
					objAct:addAwardData(objAward)
				end
			else
				objAct._eType = EActivityType.eOther -- 类型： 其他活动
			end
			-- LogError("actConfig.name="..actConfig.name)
			-- 读取活动配置数据
			if actConfig ~= nil then
				objAct._name = actConfig.name
				objAct._eTag = tonumber(actConfig.tag)
				objAct._strMainTitleImgName = actConfig.mainTitle
				objAct._strSubTitle = actConfig.subTitle
				objAct._strRules = actConfig.rules
				objAct._strAdImgName = actConfig.imgAd
				objAct._strDataTitleFmt = actConfig.dataTitle
			end	

			-- 添加至管理器
			ActivitiesManager:addAcivity(objAct)

			-- 清空辅助表
			tAuxQuick = {}
		end

	-- 设置初始激活的活动索引
	if table.getn(ActivitiesManager._tActivies) > 0 and ActivitiesManager.activeActivityIdx == 0 then
		ActivitiesManager.activeActivityIdx = 1
	end

	-- 设置初始化标记
	ActivitiesManager:init()

	triggerScriptEvent(UPDATE_MAIN_WIN_RED,"activity")
end

-- 活动查询返回消息处理
function onActivityInfoQueryReply(msgId, tMsgData)
	if tMsgData == nil or tMsgData.activity_data == nil then
		return 
	end

	local activity_data = tMsgData.activity_data
	local objAct = ActivitiesManager:getActivityById(activity_data.id)
	if objAct ~= nil  then
		objAct._bDirty = false
		objAct._nCurrentData = activity_data.current_data

		local tAuxQuick = {}  -- 快速辅助表
		-- 已领取的奖励
		if activity_data.draw_info_list ~= nil then
			for _, idTask in ipairs(activity_data.draw_info_list) do
				tAuxQuick[idTask] = true
			end
		end

		-- 更新 
		for _, award in objAct:awardDatasIter() do
			award._nCurrentValue = activity_data.current_data
			if tAuxQuick[award._idTask] then
				award._eStatus = EAwardGetStatus.eGot
			elseif award._nCurrentValue > award._nMaxValue then
				award._eStatus = EAwardGetStatus.eComplete
			end
		end

		-- 触发UI更新事件
        triggerScriptEvent(ACTIVITY_AND_ANNOUNCEMENT_UPDATE_ACTIVITY_PAGE, activity_data.id)
		triggerScriptEvent(UPDATE_MAIN_WIN_RED,"activity")
	end
end

-- 奖励领取返回
function  onActivityDrawReply(msgId, tMsgData)
	if tMsgData == nil then
		return
	end

	if tMsgData.result == 0 then  -- 领取成功
		-- 更新活动数据
		local idAct = tMsgData.activity_id
		local idAward = tMsgData.sub_id
		local objAct = ActivitiesManager:getActivityById(idAct)
		if objAct ~= nil then
			local objActAward = objAct:getAwardDataById(idAward)
			if objActAward ~= nil then
				objActAward._eStatus = EAwardGetStatus.eGot
				objAct:resort()

				-- 触发UI更新事件
           		triggerScriptEvent(ACTIVITY_AND_ANNOUNCEMENT_UPDATE_ACTIVITY_AWARD_ITEM, idAct, idAward)
			end
		end

		-- 显示奖励
		ShowAwardWin(tMsgData.reward_list)
		triggerScriptEvent(UPDATE_MAIN_WIN_RED,"activity")
	elseif tMsgData.result == 1 then -- 领取失败
		UnityTools.ShowMessage(tMsgData.err)
	end
end


-- des: 活动配置 只登入时下发
protobuf:registerMessageScriptHandler(protoIdSet.sc_activity_config_info_update, "onActivityConfigInfoUpdate")
-- des: 活动数据查询 返回
protobuf:registerMessageScriptHandler(protoIdSet.sc_activity_info_query_reply, "onActivityInfoQueryReply")
-- des:活动领奖
protobuf:registerMessageScriptHandler(protoIdSet.sc_activity_draw_reply, "onActivityDrawReply")

-- /////////////////////////////////////////////////////////////////////////////////////////////
-- 测试数据

--[[
ActivitiesManager:addAcivity(
	CAcitivty:new{
		_id = 1,   						-- 活动id
		_name = "测试1",						-- 活动名字
		_eType = EActivityType.eNormal, -- 常规
		_eTag = EActivityTag.eHot,		-- 标签
		_strRules = "活动1的规则",					-- 规则信息
		_strAdImgName = "",				-- 广告图
		_nStartTime = 0,				-- 开始时间
		_nEndTime = 0,					-- 结束时间
		_tAwardDatas = {
			CNormalActAwardData:new{
				_idTask = 1,		-- 任务ID
				_strTitle = "第一条",		-- 任务标题
				_nCurrentValue = 100,	-- 当前进度值
				_nMaxValue = 100,		-- 最大进度值
				_tAwardItems = {
					{
						idItem = 101,
						nCount = 100,
					}
				},	-- 任务奖励物品
				_eStatus = EAwardGetStatus.eComplete,  -- 领取状态
			},
			CNormalActAwardData:new{
				_idTask = 2,		-- 任务ID
				_strTitle = "第二条",		-- 任务标题
				_nCurrentValue = 100,	-- 当前进度值
				_nMaxValue = 500,		-- 最大进度值
				_tAwardItems = {
					{
						idItem = 101,
						nCount = 1000,
					}
				},	-- 任务奖励物品
				_eStatus = EAwardGetStatus.eUnderway,  -- 领取状态
			}
		}, 				-- 奖励数据

		-- 额外的存储值, 以key-value的方式存储, 比如: recharge=100
		_tExtraValues = {},
	})

ActivitiesManager:addAcivity(
	CAcitivty:new{
		_id = 2,   						-- 活动id
		_name = "测试2",						-- 活动名字
		_eType = EActivityType.eNormal, -- 常规
		_eTag = EActivityTag.eNew,		-- 标签
		_strRules = "活动2的规则",					-- 规则信息
		_strAdImgName = "",				-- 广告图
		_nStartTime = 0,				-- 开始时间
		_nEndTime = 0,					-- 结束时间
		_tAwardDatas = {
			CNormalActAwardData:new{
				_idTask = 1,		-- 任务ID
				_strTitle = "测试2第一条",		-- 任务标题
				_nCurrentValue = 300,	-- 当前进度值
				_nMaxValue = 100,		-- 最大进度值
				_tAwardItems = {
					{
						idItem = 101,
						nCount = 100,
					}
				},	-- 任务奖励物品
				_eStatus = EAwardGetStatus.eComplete,  -- 领取状态
			},
			CNormalActAwardData:new{
				_idTask = 2,		-- 任务ID
				_strTitle = "测试2第二条",		-- 任务标题
				_nCurrentValue = 300,	-- 当前进度值
				_nMaxValue = 800,		-- 最大进度值
				_tAwardItems = {
					{
						idItem = 101,
						nCount = 1000,
					}
				},	-- 任务奖励物品
				_eStatus = EAwardGetStatus.eGot,  -- 领取状态
			}
		}, 				-- 奖励数据

		-- 额外的存储值, 以key-value的方式存储, 比如: recharge=100
		_tExtraValues = {},
	}
)
--]]

-- /////////////////////////////////////////////////////////////////////////////////////////////
-- 窗口事件响应

-- 响应创建
local function OnCreateCallBack(gameObject)

end

-- 响应销毁
local function OnDestoryCallBack(gameObject)

end


UI.Controller.UIManager.RegisterLuaWinFunc("ActivityAndAnnouncement", OnCreateCallBack, OnDestoryCallBack)

-- ////////////////////////////////////////////////////////////////////////////////////
-- 游戏事件

-- 清理数据
function onClearActivityAndAnnouncementData()
    ActivitiesManager:clear()
end


registerScriptEvent(EXIT_CLEAR_ALL_DATA, "onClearActivityAndAnnouncementData")

-- ------------------------
-- 模块导出设置
-- ------------------------
M.ActivitiesManager = ActivitiesManager
M.EAwardGetStatus = EAwardGetStatus
M.EActivityType = EActivityType
M.EActivityTag = EActivityTag
M.EWinActiveStatus = EWinActiveStatus

-- 返回当前模块
return M
