//表名: 牌型倍率, 字段描述：_key:编号, _name:名称, _commision:倍率,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class PatternsPowerConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// 编号
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _name;
	/// <summary>
	/// 名称
	/// </summary>
	public string name { get { return (string)_name; } }
	private System.Object _commision;
	/// <summary>
	/// 倍率
	/// </summary>
	public int commision { get { return (int)_commision; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_name = element.Attribute("name");base["name"]=_name;
		_commision = Convert.ToInt32(element.Attribute("commision"));base["commision"]=_commision;
	}
}

public class PatternsPowerConfig : ConfigDataBase<PatternsPowerConfigItem> {
	public PatternsPowerConfig(){
		_fileName = "export_xml/patterns_power_config.bytes";
	}
}