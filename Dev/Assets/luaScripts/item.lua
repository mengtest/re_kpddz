

local M_main = IMPORT_MODULE("main")

local function getItemName()
	print("含笑半步癫")
end


M_main.registerFunc(getItemName)

local act = M_main.Account:new()
print(act:toString())


local act1 = M_main.Account:new()
act1:setName("Hello")
-- print(act1:toString())




local protobuf = sluaAux.luaProtobuf.getInstance()


function onMsg9000000(idMsg, tMsgData)
	print(idMsg)
	PrintTable(tMsgData)
end

protobuf:registerMessageScriptHandler(900000, "onMsg9000000")