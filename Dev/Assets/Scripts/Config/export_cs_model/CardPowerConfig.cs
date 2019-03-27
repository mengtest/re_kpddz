//权重,图标
//表名: 豪车倍率表, 字段描述：_key:ID, _name:名称, _icon:倍率,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class CardPowerConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// ID
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _name;
	/// <summary>
	/// 名称
	/// </summary>
	public string name { get { return (string)_name; } }
	private System.Object _icon;
	/// <summary>
	/// 倍率
	/// </summary>
	public string icon { get { return (string)_icon; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_name = element.Attribute("name");base["name"]=_name;
		_icon = element.Attribute("icon");base["icon"]=_icon;
	}
}

public class CardPowerConfig : ConfigDataBase<CardPowerConfigItem> {
	public CardPowerConfig(){
		_fileName = "export_xml/card_power_config.bytes";
	}
}