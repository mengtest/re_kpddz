//表名: 拉霸任务, 字段描述：_key:ID, _icon:图标, _account_level:开启等级, _pre_id:前置任务, _post_id:后置任务, _title:任务标题, _achieve_conditicon:数组, _achieve_condition_type:活动类型, _achieve_condition_param1s:参数1, _achieve_condition_param2s:参数2, _achieve_condition_param3s:参数3, _item1_id:奖励物品1ID, _item1_num:奖励物品1数量, _desc:描述,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class FruitDayConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// ID
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _icon;
	/// <summary>
	/// 图标
	/// </summary>
	public string icon { get { return (string)_icon; } }
	private System.Object _account_level;
	/// <summary>
	/// 开启等级
	/// </summary>
	public int account_level { get { return (int)_account_level; } }
	private System.Object _pre_id;
	/// <summary>
	/// 前置任务
	/// </summary>
	public int pre_id { get { return (int)_pre_id; } }
	private System.Object _post_id;
	/// <summary>
	/// 后置任务
	/// </summary>
	public int post_id { get { return (int)_post_id; } }
	private System.Object _title;
	/// <summary>
	/// 任务标题
	/// </summary>
	public string title { get { return (string)_title; } }
	private System.Object _achieve_conditicon;
	/// <summary>
	/// 数组
	/// </summary>
	public string achieve_conditicon { get { return (string)_achieve_conditicon; } }
	private System.Object _achieve_condition_type;
	/// <summary>
	/// 活动类型
	/// </summary>
	public int achieve_condition_type { get { return (int)_achieve_condition_type; } }
	private System.Object _achieve_condition_param1s;
	/// <summary>
	/// 参数1
	/// </summary>
	public string achieve_condition_param1s { get { return (string)_achieve_condition_param1s; } }
	private System.Object _achieve_condition_param2s;
	/// <summary>
	/// 参数2
	/// </summary>
	public int achieve_condition_param2s { get { return (int)_achieve_condition_param2s; } }
	private System.Object _achieve_condition_param3s;
	/// <summary>
	/// 参数3
	/// </summary>
	public int achieve_condition_param3s { get { return (int)_achieve_condition_param3s; } }
	private System.Object _item1_id;
	/// <summary>
	/// 奖励物品1ID
	/// </summary>
	public int item1_id { get { return (int)_item1_id; } }
	private System.Object _item1_num;
	/// <summary>
	/// 奖励物品1数量
	/// </summary>
	public int item1_num { get { return (int)_item1_num; } }
	private System.Object _desc;
	/// <summary>
	/// 描述
	/// </summary>
	public string desc { get { return (string)_desc; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_icon = element.Attribute("icon");base["icon"]=_icon;
		_account_level = Convert.ToInt32(element.Attribute("account_level"));base["account_level"]=_account_level;
		_pre_id = Convert.ToInt32(element.Attribute("pre_id"));base["pre_id"]=_pre_id;
		_post_id = Convert.ToInt32(element.Attribute("post_id"));base["post_id"]=_post_id;
		_title = element.Attribute("title");base["title"]=_title;
		_achieve_conditicon = element.Attribute("achieve_conditicon");base["achieve_conditicon"]=_achieve_conditicon;
		_achieve_condition_type = Convert.ToInt32(element.Attribute("achieve_condition_type"));base["achieve_condition_type"]=_achieve_condition_type;
		_achieve_condition_param1s = element.Attribute("achieve_condition_param1s");base["achieve_condition_param1s"]=_achieve_condition_param1s;
		_achieve_condition_param2s = Convert.ToInt32(element.Attribute("achieve_condition_param2s"));base["achieve_condition_param2s"]=_achieve_condition_param2s;
		_achieve_condition_param3s = Convert.ToInt32(element.Attribute("achieve_condition_param3s"));base["achieve_condition_param3s"]=_achieve_condition_param3s;
		_item1_id = Convert.ToInt32(element.Attribute("item1_id"));base["item1_id"]=_item1_id;
		_item1_num = Convert.ToInt32(element.Attribute("item1_num"));base["item1_num"]=_item1_num;
		_desc = element.Attribute("desc");base["desc"]=_desc;
	}
}

public class FruitDayConfig : ConfigDataBase<FruitDayConfigItem> {
	public FruitDayConfig(){
		_fileName = "export_xml/fruit_day_config.bytes";
	}
}