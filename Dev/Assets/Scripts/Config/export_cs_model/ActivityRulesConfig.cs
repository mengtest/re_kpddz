//活动说明
//表名: 活动规则, 字段描述：_key:ID,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class ActivityRulesConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// ID
	/// </summary>
	public string key { get { return (string)_key; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
	}
}

public class ActivityRulesConfig : ConfigDataBase<ActivityRulesConfigItem> {
	public ActivityRulesConfig(){
		_fileName = "export_xml/activity_rules_config.bytes";
	}
}