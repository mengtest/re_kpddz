//表名: 新手引导表, 字段描述：_key:ID, _character:文字, _image:形象, _position:位置, _reward:奖励金币, _choose:正确选项,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class GuideConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// ID
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _character;
	/// <summary>
	/// 文字
	/// </summary>
	public string character { get { return (string)_character; } }
	private System.Object _image;
	/// <summary>
	/// 形象
	/// </summary>
	public string image { get { return (string)_image; } }
	private System.Object _position;
	/// <summary>
	/// 位置
	/// </summary>
	public int position { get { return (int)_position; } }
	private System.Object _reward;
	/// <summary>
	/// 奖励金币
	/// </summary>
	public int reward { get { return (int)_reward; } }
	private System.Object _choose;
	/// <summary>
	/// 正确选项
	/// </summary>
	public int choose { get { return (int)_choose; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_character = element.Attribute("character");base["character"]=_character;
		_image = element.Attribute("image");base["image"]=_image;
		_position = Convert.ToInt32(element.Attribute("position"));base["position"]=_position;
		_reward = Convert.ToInt32(element.Attribute("reward"));base["reward"]=_reward;
		_choose = Convert.ToInt32(element.Attribute("choose"));base["choose"]=_choose;
	}
}

public class GuideConfig : ConfigDataBase<GuideConfigItem> {
	public GuideConfig(){
		_fileName = "export_xml/guide_config.bytes";
	}
}