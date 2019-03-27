//表名: 免费金币, 字段描述：_key:ID, _icon:图标, _title:标题, _desc:描述, _skip_name:未完成按钮文字, _skip1_name:达成条件按钮文字, _skip2_name:完成按钮文字, _skip_id:未完成跳转界面ID, _skip_id1:满足条件需要处理, _skip_id2:完成跳转界面ID, _red:满足条件时是否显示红点,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class FreeConfigItem:ConfigDataItemBase{
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
	/// 标题
	/// </summary>
	public string title { get { return (string)_title; } }
	private System.Object _desc;
	/// <summary>
	/// 描述
	/// </summary>
	public string desc { get { return (string)_desc; } }
	private System.Object _skip_name;
	/// <summary>
	/// 未完成按钮文字
	/// </summary>
	public string skip_name { get { return (string)_skip_name; } }
	private System.Object _skip1_name;
	/// <summary>
	/// 达成条件按钮文字
	/// </summary>
	public string skip1_name { get { return (string)_skip1_name; } }
	private System.Object _skip2_name;
	/// <summary>
	/// 完成按钮文字
	/// </summary>
	public string skip2_name { get { return (string)_skip2_name; } }
	private System.Object _skip_id;
	/// <summary>
	/// 未完成跳转界面ID
	/// </summary>
	public string skip_id { get { return (string)_skip_id; } }
	private System.Object _skip_id1;
	/// <summary>
	/// 满足条件需要处理
	/// </summary>
	public int skip_id1 { get { return (int)_skip_id1; } }
	private System.Object _skip_id2;
	/// <summary>
	/// 完成跳转界面ID
	/// </summary>
	public string skip_id2 { get { return (string)_skip_id2; } }
	private System.Object _red;
	/// <summary>
	/// 满足条件时是否显示红点
	/// </summary>
	public int red { get { return (int)_red; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_icon = element.Attribute("icon");base["icon"]=_icon;
		_title = element.Attribute("title");base["title"]=_title;
		_desc = element.Attribute("desc");base["desc"]=_desc;
		_skip_name = element.Attribute("skip_name");base["skip_name"]=_skip_name;
		_skip1_name = element.Attribute("skip1_name");base["skip1_name"]=_skip1_name;
		_skip2_name = element.Attribute("skip2_name");base["skip2_name"]=_skip2_name;
		_skip_id = element.Attribute("skip_id");base["skip_id"]=_skip_id;
		_skip_id1 = Convert.ToInt32(element.Attribute("skip_id1"));base["skip_id1"]=_skip_id1;
		_skip_id2 = element.Attribute("skip_id2");base["skip_id2"]=_skip_id2;
		_red = Convert.ToInt32(element.Attribute("red"));base["red"]=_red;
	}
}

public class FreeConfig : ConfigDataBase<FreeConfigItem> {
	public FreeConfig(){
		_fileName = "export_xml/new_task_config.bytes";
	}
}