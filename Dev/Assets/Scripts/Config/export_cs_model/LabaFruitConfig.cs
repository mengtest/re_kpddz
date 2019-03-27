//表名: 中奖倍数, 字段描述：_key:ID, _name:名称, _power2:2连线倍率, _power3:3连线倍率, _power4:4连线倍率, _power5:5连线倍率, _rate:权重, _icon:图标,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class LabaFruitConfigItem:ConfigDataItemBase{
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
	private System.Object _power2;
	/// <summary>
	/// 2连线倍率
	/// </summary>
	public int power2 { get { return (int)_power2; } }
	private System.Object _power3;
	/// <summary>
	/// 3连线倍率
	/// </summary>
	public int power3 { get { return (int)_power3; } }
	private System.Object _power4;
	/// <summary>
	/// 4连线倍率
	/// </summary>
	public int power4 { get { return (int)_power4; } }
	private System.Object _power5;
	/// <summary>
	/// 5连线倍率
	/// </summary>
	public int power5 { get { return (int)_power5; } }
	private System.Object _rate;
	/// <summary>
	/// 权重
	/// </summary>
	public int rate { get { return (int)_rate; } }
	private System.Object _icon;
	/// <summary>
	/// 图标
	/// </summary>
	public string icon { get { return (string)_icon; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_name = element.Attribute("name");base["name"]=_name;
		_power2 = Convert.ToInt32(element.Attribute("power2"));base["power2"]=_power2;
		_power3 = Convert.ToInt32(element.Attribute("power3"));base["power3"]=_power3;
		_power4 = Convert.ToInt32(element.Attribute("power4"));base["power4"]=_power4;
		_power5 = Convert.ToInt32(element.Attribute("power5"));base["power5"]=_power5;
		_rate = Convert.ToInt32(element.Attribute("rate"));base["rate"]=_rate;
		_icon = element.Attribute("icon");base["icon"]=_icon;
	}
}

public class LabaFruitConfig : ConfigDataBase<LabaFruitConfigItem> {
	public LabaFruitConfig(){
		_fileName = "export_xml/laba_fruit_config.bytes";
	}
}