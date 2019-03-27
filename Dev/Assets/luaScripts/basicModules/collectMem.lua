-- -------------------------------------------------------------------
 
 
-- *
-- * Summary: 	垃圾回收
-- *
-- * Version: 	1.0.0
-- * Author: 	chr
-- * Date:		2016-12-8 16:12:25
-- ----------------------------------------------------------------------

-- avoid memory leak

-- 分步回收，步长1024
collectgarbage("setpause", 100)
collectgarbage("setstepmul", 5000)
collectgarbage("step", 1024)


local function collectImpl()
	collectgarbage("collect")
	-- MemorySampleLog(true)
end

function CollectLua()
	collectImpl()
end

-- gTimer.registerRepeatTimer(12000, collectImpl)

-- 开关
local bLuaMM = false
local minMM = 0
local maxMM = 0

local UnityTools = IMPORT_MODULE("UnityTools")

function luaMemoryLog()
	local cm = collectgarbage("count")
	if minMM == 0 or minMM > cm then minMM = cm end
	if maxMM < cm then maxMM = cm end
	print("Lua memeory: MinM-> " .. minMM .. " MaxM-> " .. maxMM .. " CurM-> " .. cm .. " ChgM-> " .. (cm - minMM))
	UnityTools.PrintTable(GET_UNUSE_MODULE())
end

if bLuaMM then
	gTimer.registerRepeatTimer(1500, "luaMemoryLog")
end

local _lastGlobalData = {}
setmetatable(_lastGlobalData, {__mode = "kv"})

-- clean参数一般为false， 为ture则表示对_G进行强制清理(慎用)
function MemorySampleLog(clean)
	clean = clean or false
	local different = {}
	for k, v in pairs(_G) do
		if _lastGlobalData[k] == nil or _lastGlobalData[k] ~= _G[k] then
			different[#different + 1] = k
		end
	end
	-- UnityTools.PrintTable(different)
	-- print("-------------MemorySampleLog------------------:" .. #different)

	-- 执行强制清理，还原最初采样的G
	if clean then
		-- UnityEngine.Debug.Log(#different)
		for i = 1, #different, 1 do
			local key = different[i]
			_G[key] = nil
			package.loaded[key] = nil
		end

	end
	different = nil
end

function MemorySample()
	_lastGlobalData = {}
	for k, v in pairs(_G) do
		_lastGlobalData[k] = v
	end
end





