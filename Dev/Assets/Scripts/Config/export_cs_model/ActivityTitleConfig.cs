//表名: 活动数据标题表, 字段描述：_key:ID, _name:活动名字, _tag:活动标签, _mainTitle:主标题, _subTitle:副标题, _dataTitle:数据统计标题, _imgAd:广告图, _rules:活动规则, _activityTable:对应的活动表名,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class ActivityTitleConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// ID
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _name;
	/// <summary>
	/// 活动名字
	/// </summary>
	public string name { get { return (string)_name; } }
	private System.Object _tag;
	/// <summary>
	/// 活动标签
	/// </summary>
	public int tag { get { return (int)_tag; } }
	private System.Object _mainTitle;
	/// <summary>
	/// 主标题
	/// </summary>
	public string mainTitle { get { return (string)_mainTitle; } }
	private System.Object _subTitle;
	/// <summary>
	/// 副标题
	/// </summary>
	public string subTitle { get { return (string)_subTitle; } }
	private System.Object _dataTitle;
	/// <summary>
	/// 数据统计标题
	/// </summary>
	public string dataTitle { get { return (string)_dataTitle; } }
	private System.Object _imgAd;
	/// <summary>
	/// 广告图
	/// </summary>
	public string imgAd { get { return (string)_imgAd; } }
	private System.Object _rules;
	/// <summary>
	/// 活动规则
	/// </summary>
	public string rules { get { return (string)_rules; } }
	private System.Object _activityTable;
	/// <summary>
	/// 对应的活动表名
	/// </summary>
	public string activityTable { get { return (string)_activityTable; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_name = element.Attribute("name");base["name"]=_name;
		_tag = Convert.ToInt32(element.Attribute("tag"));base["tag"]=_tag;
		_mainTitle = element.Attribute("mainTitle");base["mainTitle"]=_mainTitle;
		_subTitle = element.Attribute("subTitle");base["subTitle"]=_subTitle;
		_dataTitle = element.Attribute("dataTitle");base["dataTitle"]=_dataTitle;
		_imgAd = element.Attribute("imgAd");base["imgAd"]=_imgAd;
		_rules = element.Attribute("rules");base["rules"]=_rules;
		_activityTable = element.Attribute("activityTable");base["activityTable"]=_activityTable;
	}
}

public class ActivityTitleConfig : ConfigDataBase<ActivityTitleConfigItem> {
	public ActivityTitleConfig(){
		_fileName = "export_xml/activity_title_config.bytes";
	}
}