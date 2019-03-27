//表名: 招财进牛, 字段描述：_key:ID, _recharge:充值额度, _get_gold:财神鱼可领金币,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class GodGoldConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// ID
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _recharge;
	/// <summary>
	/// 充值额度
	/// </summary>
	public int recharge { get { return (int)_recharge; } }
	private System.Object _get_gold;
	/// <summary>
	/// 财神鱼可领金币
	/// </summary>
	public int get_gold { get { return (int)_get_gold; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_recharge = Convert.ToInt32(element.Attribute("recharge"));base["recharge"]=_recharge;
		_get_gold = Convert.ToInt32(element.Attribute("get_gold"));base["get_gold"]=_get_gold;
	}
}

public class GodGoldConfig : ConfigDataBase<GodGoldConfigItem> {
	public GodGoldConfig(){
		_fileName = "export_xml/god_gold_config.bytes";
	}
}