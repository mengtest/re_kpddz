//表名: 客户端常量表, 字段描述：_key:字段名, _name:名字, _value:数值, _value1:数值, _desc:描述,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class ClientBaseConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// 字段名
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _name;
	/// <summary>
	/// 名字
	/// </summary>
	public string name { get { return (string)_name; } }
	private System.Object _value;
	/// <summary>
	/// 数值
	/// </summary>
	public int value { get { return (int)_value; } }
	private System.Object _value1;
	/// <summary>
	/// 数值
	/// </summary>
	public float value1 { get { return (float)_value1; } }
	private System.Object _desc;
	/// <summary>
	/// 描述
	/// </summary>
	public string desc { get { return (string)_desc; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_name = element.Attribute("name");base["name"]=_name;
		_value = Convert.ToInt32(element.Attribute("value"));base["value"]=_value;
		_value1 = (float)Convert.ToDouble(element.Attribute("value1"));base["value1"]=_value1;
		_desc = element.Attribute("desc");base["desc"]=_desc;
	}
}

public class ClientBaseConfig : ConfigDataBase<ClientBaseConfigItem> {
	public ClientBaseConfig(){
		_fileName = "export_xml/client_base_config.bytes";
	}
}