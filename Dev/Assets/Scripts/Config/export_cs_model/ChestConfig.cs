//表名: 对局宝箱, 字段描述：_key:编号, _name:名称, _min_door:最低门槛, _condition:领取宝箱需要对局数, _free_get:免费领取金币, _get2:领取镖票, _get3:领取镖票,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class ChestConfigItem:ConfigDataItemBase{
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
	private System.Object _min_door;
	/// <summary>
	/// 最低门槛
	/// </summary>
	public int min_door { get { return (int)_min_door; } }
	private System.Object _condition;
	/// <summary>
	/// 领取宝箱需要对局数
	/// </summary>
	public int condition { get { return (int)_condition; } }
	private System.Object _free_get;
	/// <summary>
	/// 免费领取金币
	/// </summary>
	public string free_get { get { return (string)_free_get; } }
	private System.Object _get2;
	/// <summary>
	/// 领取镖票
	/// </summary>
	public string get2 { get { return (string)_get2; } }
	private System.Object _get3;
	/// <summary>
	/// 领取镖票
	/// </summary>
	public string get3 { get { return (string)_get3; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_name = element.Attribute("name");base["name"]=_name;
		_min_door = Convert.ToInt32(element.Attribute("min_door"));base["min_door"]=_min_door;
		_condition = Convert.ToInt32(element.Attribute("condition"));base["condition"]=_condition;
		_free_get = element.Attribute("free_get");base["free_get"]=_free_get;
		_get2 = element.Attribute("get2");base["get2"]=_get2;
		_get3 = element.Attribute("get3");base["get3"]=_get3;
	}
}

public class ChestConfig : ConfigDataBase<ChestConfigItem> {
	public ChestConfig(){
		_fileName = "export_xml/chest_config.bytes";
	}
}