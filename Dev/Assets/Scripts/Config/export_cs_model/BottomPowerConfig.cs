//表名: 下注倍率, 字段描述：_key:编号, _commision:下注倍率,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class BottomPowerConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// 编号
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _commision;
	/// <summary>
	/// 下注倍率
	/// </summary>
	public string commision { get { return (string)_commision; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_commision = element.Attribute("commision");base["commision"]=_commision;
	}
}

public class BottomPowerConfig : ConfigDataBase<BottomPowerConfigItem> {
	public BottomPowerConfig(){
		_fileName = "export_xml/bottom_power_config.bytes";
	}
}