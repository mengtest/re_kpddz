//跳转界面ID
//表名: 红包场复活任务, 字段描述：_key:ID, _icon:图标, _title:开启等级, _achieve_conditicon:任务标题, _achieve_type:数组, _parameter1:活动类型, _parameter2:参数, _parameter3:参数, _item1_id:条件参数, _item1_num:奖励物品1ID, _item2_id:奖励物品1数量, _item2_num:奖励物品2ID, _desc:奖励物品2数量, _skip_id:描述,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class DiamondReviveTaskItem:ConfigDataItemBase{
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
	private System.Object _title;
	/// <summary>
	/// 开启等级
	/// </summary>
	public string title { get { return (string)_title; } }
	private System.Object _achieve_conditicon;
	/// <summary>
	/// 任务标题
	/// </summary>
	public string achieve_conditicon { get { return (string)_achieve_conditicon; } }
	private System.Object _achieve_type;
	/// <summary>
	/// 数组
	/// </summary>
	public int achieve_type { get { return (int)_achieve_type; } }
	private System.Object _parameter1;
	/// <summary>
	/// 活动类型
	/// </summary>
	public int parameter1 { get { return (int)_parameter1; } }
	private System.Object _parameter2;
	/// <summary>
	/// 参数
	/// </summary>
	public int parameter2 { get { return (int)_parameter2; } }
	private System.Object _parameter3;
	/// <summary>
	/// 参数
	/// </summary>
	public string parameter3 { get { return (string)_parameter3; } }
	private System.Object _item1_id;
	/// <summary>
	/// 条件参数
	/// </summary>
	public int item1_id { get { return (int)_item1_id; } }
	private System.Object _item1_num;
	/// <summary>
	/// 奖励物品1ID
	/// </summary>
	public int item1_num { get { return (int)_item1_num; } }
	private System.Object _item2_id;
	/// <summary>
	/// 奖励物品1数量
	/// </summary>
	public int item2_id { get { return (int)_item2_id; } }
	private System.Object _item2_num;
	/// <summary>
	/// 奖励物品2ID
	/// </summary>
	public int item2_num { get { return (int)_item2_num; } }
	private System.Object _desc;
	/// <summary>
	/// 奖励物品2数量
	/// </summary>
	public string desc { get { return (string)_desc; } }
	private System.Object _skip_id;
	/// <summary>
	/// 描述
	/// </summary>
	public string skip_id { get { return (string)_skip_id; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_icon = element.Attribute("icon");base["icon"]=_icon;
		_title = element.Attribute("title");base["title"]=_title;
		_achieve_conditicon = element.Attribute("achieve_conditicon");base["achieve_conditicon"]=_achieve_conditicon;
		_achieve_type = Convert.ToInt32(element.Attribute("achieve_type"));base["achieve_type"]=_achieve_type;
		_parameter1 = Convert.ToInt32(element.Attribute("parameter1"));base["parameter1"]=_parameter1;
		_parameter2 = Convert.ToInt32(element.Attribute("parameter2"));base["parameter2"]=_parameter2;
		_parameter3 = element.Attribute("parameter3");base["parameter3"]=_parameter3;
		_item1_id = Convert.ToInt32(element.Attribute("item1_id"));base["item1_id"]=_item1_id;
		_item1_num = Convert.ToInt32(element.Attribute("item1_num"));base["item1_num"]=_item1_num;
		_item2_id = Convert.ToInt32(element.Attribute("item2_id"));base["item2_id"]=_item2_id;
		_item2_num = Convert.ToInt32(element.Attribute("item2_num"));base["item2_num"]=_item2_num;
		_desc = element.Attribute("desc");base["desc"]=_desc;
		_skip_id = element.Attribute("skip_id");base["skip_id"]=_skip_id;
	}
}

public class DiamondReviveTask : ConfigDataBase<DiamondReviveTaskItem> {
	public DiamondReviveTask(){
		_fileName = "export_xml/diamond_revive_task.bytes";
	}
}