-- --------------------------------------------------


-- *
-- * Summary: 	主函数模块
-- * Version: 	1.0.0
-- * Author: 	WP.Chu
-- --------------------------------------------------


-- 设置模块
local M = GENERATE_MODULE("main")
local UnityTools = IMPORT_MODULE("UnityTools")
-- 导入模块
local gameMgr = IMPORT_MODULE("GameMgr")
local pokerMgr = IMPORT_MODULE("PokerMgr")
local platformMgr = IMPORT_MODULE("PlatformMgr")

-- 导入函数
local print = print

-- ---------------
-- 脚本主函数
-- ---------------
function main( ... )
	-- print("脚本主函数调用成功")
	-- if g_openAnnouncement == true then
	-- 	UnityTools.CreateLuaWin("PublicSignWin");
	-- end
    -- TODO: 脚本操作
    M.helloWorld()
end


-- ---------------
-- 脚本主循环
-- ---------------
function luaLoop(fDeltaTime)
    --print("****************** Timer: " .. tostring(fDeltaTime))
	gTimer.doTimer(fDeltaTime)

	UnityTools.Update(fDeltaTime)
end

function memorySample()
	-- 对G进行采样
	MemorySample()
end

-- 打印helloworld
function M.helloWorld()
    print("Hello world", _G._VERSION)
end

-- 注册函数
function M.registerFunc(func)
	if not func then
		return
	end
	
	print(tostring(func) .. " is registered!!")
	
	func()
end

local function htmlLua()
	-- require "html"
	local url = "https://ichart.yahoo.com/table.csv?s=002373.SZ&a=04&b=25&c=2017&d=04&e=26&f=2017&g=d"
	local totalNumUrl = "http://quote.eastmoney.com/stocklist.html"
	local www = UnityEngine.WWW(url)
	while www.isDone == false do end
	local content = www.text
	LogError(content)
	local findKey = '<li><a target="_blank" href="http://quote.eastmoney.com/'
	local gpType = "sh"
	findKey = findKey .. gpType
	local gpNumbers = {}
	local times = 10
	while string.find(content, findKey) ~= nil and times > 0 do
		local i, j = string.find(content, findKey)
		-- LogError("----------  " .. j)
		gpNumbers[#gpNumbers + 1] = string.sub(content, j + 1, j + 6)
		-- LogError(string.sub(content, j + 1, j + 6))
		content = string.sub(content, j - 1)
		-- times = times - 1
	end
	-- UnityTools.PrintTable(gpNumbers)
end

function LuaLoaded() 
	gameMgr.InitGameMgr()
	-- print("脚本主函数调用成功")
	if g_openAnnouncement == true then
		gTimer.registerOnceTimer(500, function () 
			UnityTools.CreateLuaWin("PublicSignWin")
		end)
	end

	-- htmlLua()

end








