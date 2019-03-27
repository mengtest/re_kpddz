//宝箱3奖励金币
//表名: 百人宝箱, 字段描述：_key:ID, _reward_gold1:ID, _reward_gold2:宝箱1奖励金币, _reward_gold3:宝箱2奖励金币,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class CowChestConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// ID
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _reward_gold1;
	/// <summary>
	/// ID
	/// </summary>
	public int reward_gold1 { get { return (int)_reward_gold1; } }
	private System.Object _reward_gold2;
	/// <summary>
	/// 宝箱1奖励金币
	/// </summary>
	public int reward_gold2 { get { return (int)_reward_gold2; } }
	private System.Object _reward_gold3;
	/// <summary>
	/// 宝箱2奖励金币
	/// </summary>
	public int reward_gold3 { get { return (int)_reward_gold3; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_reward_gold1 = Convert.ToInt32(element.Attribute("reward_gold1"));base["reward_gold1"]=_reward_gold1;
		_reward_gold2 = Convert.ToInt32(element.Attribute("reward_gold2"));base["reward_gold2"]=_reward_gold2;
		_reward_gold3 = Convert.ToInt32(element.Attribute("reward_gold3"));base["reward_gold3"]=_reward_gold3;
	}
}

public class CowChestConfig : ConfigDataBase<CowChestConfigItem> {
	public CowChestConfig(){
		_fileName = "export_xml/cow_chest_config.bytes";
	}
}