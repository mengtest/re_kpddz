//表名: 上庄筹码, 字段描述：_key:ID, _gold_carry:上庄携带金额,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class GoldCarryConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// ID
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _gold_carry;
	/// <summary>
	/// 上庄携带金额
	/// </summary>
	public int gold_carry { get { return (int)_gold_carry; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_gold_carry = Convert.ToInt32(element.Attribute("gold_carry"));base["gold_carry"]=_gold_carry;
	}
}

public class GoldCarryConfig : ConfigDataBase<GoldCarryConfigItem> {
	public GoldCarryConfig(){
		_fileName = "export_xml/gold_carry_config.bytes";
	}
}