//表名: 分享奖励, 字段描述：_key:好友进阶任务, _descirbe:描述 , _condition:条件, _reward:奖励,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class ShareRewardConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// 好友进阶任务
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _descirbe;
	/// <summary>
	/// 描述 
	/// </summary>
	public string descirbe { get { return (string)_descirbe; } }
	private System.Object _condition;
	/// <summary>
	/// 条件
	/// </summary>
	public int condition { get { return (int)_condition; } }
	private System.Object _reward;
	/// <summary>
	/// 奖励
	/// </summary>
	public string reward { get { return (string)_reward; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_descirbe = element.Attribute("descirbe");base["descirbe"]=_descirbe;
		_condition = Convert.ToInt32(element.Attribute("condition"));base["condition"]=_condition;
		_reward = element.Attribute("reward");base["reward"]=_reward;
	}
}

public class ShareRewardConfig : ConfigDataBase<ShareRewardConfigItem> {
	public ShareRewardConfig(){
		_fileName = "export_xml/share_reward_config.bytes";
	}
}