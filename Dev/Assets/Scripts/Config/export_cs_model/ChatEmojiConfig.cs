//表名: 聊天表情, 字段描述：_key:ID, _type:表情, _show_text:文本, _sound:声音, _effect:特效,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class ChatEmojiConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// ID
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _type;
	/// <summary>
	/// 表情
	/// </summary>
	public int type { get { return (int)_type; } }
	private System.Object _show_text;
	/// <summary>
	/// 文本
	/// </summary>
	public string show_text { get { return (string)_show_text; } }
	private System.Object _sound;
	/// <summary>
	/// 声音
	/// </summary>
	public string sound { get { return (string)_sound; } }
	private System.Object _effect;
	/// <summary>
	/// 特效
	/// </summary>
	public string effect { get { return (string)_effect; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_type = Convert.ToInt32(element.Attribute("type"));base["type"]=_type;
		_show_text = element.Attribute("show_text");base["show_text"]=_show_text;
		_sound = element.Attribute("sound");base["sound"]=_sound;
		_effect = element.Attribute("effect");base["effect"]=_effect;
	}
}

public class ChatEmojiConfig : ConfigDataBase<ChatEmojiConfigItem> {
	public ChatEmojiConfig(){
		_fileName = "export_xml/chat_emoji_config.bytes";
	}
}