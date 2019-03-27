-- -----------------------------------------------------------------


-- *
-- * Filename:    ChatMainWinController.lua
-- * Summary:     聊天系统
-- *
-- * Version:     1.0.0
-- * Author:      EQ
-- * Date:        3/14/2017 11:27:03 AM
-- -----------------------------------------------------------------




-- 生成模块，模块导出接口需包含在M表中
local M = GENERATE_MODULE("ChatMainWinController")

local UnityTools = IMPORT_MODULE("UnityTools")

-- 界面名称
local wName = "ChatMainWin"
-- 获取界面控制器
local _controller = UI.Controller.UIManager.GetControler(wName)
local protobuf = sluaAux.luaProtobuf.getInstance()


local function OnCreateCallBack(gameObject)

end


local function OnDestoryCallBack(gameObject)

end

function AddMagicEmoji(layer, from, to, cell, key)
    local distance = UnityEngine.Vector3.Distance(from, to)
    if distance <= 0 then return nil end

    local emojiInfo = LuaConfigMgr.ChatEmojiConfig[key]
    if emojiInfo == nil or layer == nil then return nil end
    local chatCell = UtilTools.AddChild(layer, cell.gameObject, from)
    chatCell.name = "magic_" .. key
    chatCell:SetActive(true)
    local content = chatCell:GetComponent("UISprite")
    local sAction = chatCell:GetComponent("TweenScale")
    content.spriteName = emojiInfo.show_text
    sAction.enabled = false
    chatCell.transform.position = from
    UnityTools.PlaySound("Sounds/" .. emojiInfo.sound)
    local flyTime = distance / 5.2
    local pAction = TweenPosition.Begin(chatCell, flyTime, to, true)
    pAction:SetOnFinished(function() 
        content.spriteName = nil
        local effect_icon = UnityTools.AddEffect(chatCell.transform,emojiInfo.effect, {complete = function (obj) 
            local gObj = obj.EffectGameObj
            gObj.transform.localScale = UnityEngine.Vector3.one
        end, remove = function (obj) 
            if obj ~= nil and obj.EffectGameObj ~= nil then
                UnityTools.Destroy(obj.EffectGameObj.transform.parent.gameObject)
            end
        end})
    end)
end

function GetShowChatContent(content)
    local text = content
    local type = "-1"
    local key = content
    if string.find(content, "{#emoji}") ~= nil then
        key = string.gsub(content, "{#emoji}", "")
        local cData = LuaConfigMgr.ChatEmojiConfig[key]
        if cData ~= nil then
            text = cData.show_text
            type = cData.type
        end 
    end
    return text, type, key
end
local function AudioUpdateComplete(gameObject)
   
    local tagValue = ComponentData.Get(gameObject).Value
    LogError("tagValue="..tagValue)
	if tagValue == 111 then --百人牛牛
		local req = {};
		req.room_type = 2;
		req.content_type = 3;
		req.content = ComponentData.Get(gameObject).Text;
		req.obj_player_uuid = ComponentData.Get(gameObject).Name;
		protobuf:sendMessage(protoIdSet.cs_player_chat, req);
	elseif tagValue == 222 then --看牌
		local req = {};
		req.room_type = 1;
		req.content_type = 3;
		req.content = ComponentData.Get(gameObject).Text;
		req.obj_player_uuid = ComponentData.Get(gameObject).Name;
		protobuf:sendMessage(protoIdSet.cs_player_chat, req);
	elseif tagValue == 333 then --红包
		local req = {};
		req.room_type = 1;
		req.content_type = 3;
		req.content = ComponentData.Get(gameObject).Text;
		req.obj_player_uuid = ComponentData.Get(gameObject).Name;
		protobuf:sendMessage(protoIdSet.cs_player_chat, req);
	end
end
local function AudioLoadComplete(gameObject)
	LogWarn("-------------------->AudioLoadComplete")
	local tagValue = ComponentData.Get(gameObject).Value
	-- if tagValue == 1 then
		triggerScriptEvent(EVENT_AUDIOLOAD_COMPLETE, {chat = gameObject});
	-- elseif tagValue == 2 then
	-- 	triggerScriptEvent(EVENT_CHAT_AUDIOLOAD_COMPLETE, {chat = gameObject});
	-- elseif tagValue == 3 then
	-- 	triggerScriptEvent(EVENT_FRIEND_GROUP_AUDIOLOAD_COMPLETE, {chat = gameObject});
	-- end
end

UI.Controller.UIManager.RegisterLuaWinFunc("ChatMainWin", OnCreateCallBack, OnDestoryCallBack)
UI.Controller.UIManager.RegisterLuaFuncCall("ChatMainWin:AudioUpdateComplete", AudioUpdateComplete)
UI.Controller.UIManager.RegisterLuaFuncCall("ChatMainWin:AudioLoadComplete", AudioLoadComplete)

-- 返回当前模块
return M
