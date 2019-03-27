//商品纹理
//表名: 活动数据, 字段描述：_key:Id, _name:活动描述, _activity_type:活动类型, _data1:数据条件, _reward2:奖励物品id, _tex:每日可重复领取,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class ActivityConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// Id
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _name;
	/// <summary>
	/// 活动描述
	/// </summary>
	public string name { get { return (string)_name; } }
	private System.Object _activity_type;
	/// <summary>
	/// 活动类型
	/// </summary>
	public int activity_type { get { return (int)_activity_type; } }
	private System.Object _data1;
	/// <summary>
	/// 数据条件
	/// </summary>
	public int data1 { get { return (int)_data1; } }
	private System.Object _reward2;
	/// <summary>
	/// 奖励物品id
	/// </summary>
	public string reward2 { get { return (string)_reward2; } }
	private System.Object _tex;
	/// <summary>
	/// 每日可重复领取
	/// </summary>
	public string tex { get { return (string)_tex; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_name = element.Attribute("name");base["name"]=_name;
		_activity_type = Convert.ToInt32(element.Attribute("activity_type"));base["activity_type"]=_activity_type;
		_data1 = Convert.ToInt32(element.Attribute("data1"));base["data1"]=_data1;
		_reward2 = element.Attribute("reward2");base["reward2"]=_reward2;
		_tex = element.Attribute("tex");base["tex"]=_tex;
	}
}

public class ActivityConfig : ConfigDataBase<ActivityConfigItem> {
	public ActivityConfig(){
		_fileName = "export_xml/activity_config.bytes";
	}
}