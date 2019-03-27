-- -------------------------------------------------------------------


-- *
-- * Summary: 	脚本事件机制，通过事件触发来实现事件响应和数据传输
-- *
-- * Version: 	1.0.0
-- * Author: 	WP.Chu
-- ----------------------------------------------------------------------


-- --------------------------------------
-- *
-- * 事件机制逻辑实现
-- *
-- --------------------------------------

-- 管理器
local tEventMgr = {
	tEvtSet = {},
}


-- 获取管理器
function getEventManager()
	return tEventMgr
end


-- 注册事件
function tEventMgr:registerScriptEvent(nEventID, sFunc)
	if sFunc == nil then
		return
	end

	if self.tEvtSet[nEventID] == nil then
		self.tEvtSet[nEventID] = {}
	end

	self.tEvtSet[nEventID][sFunc] = true
end


-- 取消注册
function tEventMgr:unregisterScriptEvent(nEventID, sFunc)
	if self.tEvtSet[nEventID] ~= nil and type(self.tEvtSet[nEventID]) == "table" then
		if sFunc ~= nil then
			self.tEvtSet[nEventID][sFunc] = nil
		else
			self.tEvtSet[nEventID] = nil
		end
	end
end


-- 触发事件
function tEventMgr:triggerScriptEvent(nEventID, ...)
	if nEventID == nil then
		return
	end
	
	-- For LuaJIT
	local arg = {...}
	
	if type(self.tEvtSet[nEventID]) ~= "table" then
		return
	end

	for k, v in pairs(self.tEvtSet[nEventID]) do
		if type(k) == "function" then
			pcall(k(...))
		elseif v and type(_G[k]) == "function" then
			local tParam = {}
			for i, p in ipairs(arg) do
				table.insert(tParam, p)
			end
			--执行函数
			_G[k](nEventID, unpack(tParam))
		end
	end
end

-- -------------------
-- * 导出接口
-- -------------------

-- 注册
function registerScriptEvent(nEventID, sFunc)
	tEventMgr:registerScriptEvent(nEventID, sFunc)
end

-- 取消注册
function unregisterScriptEvent(nEventID, sFunc)
	tEventMgr:unregisterScriptEvent(nEventID, sFunc)
end

-- 触发事件
function triggerScriptEvent(nEventID, ...)
	-- For LuaJIT
	-- local arg = {...}
	tEventMgr:triggerScriptEvent(nEventID, ...)
end


--[[

-- ---------------------
-- * 测试用例
-- ---------------------

SCRIPT_EVENT_TYPE_TEST = 1

function testFunc(nEventID, nVal, strVal, tData)
	CCLuaLog(nEventID, nVal, strVal, tData)
end

-- 注册
registerScriptEvent(SCRIPT_EVENT_TYPE_TEST, "testFunc")


-- 触发
triggerScriptEvent(SCRIPT_EVENT_TYPE_TEST, 123, "abc", {val=12, 123})


-- 输出结果
123, abc, table: 003CCF60

--]]
















