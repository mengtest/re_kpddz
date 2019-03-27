//表名: VIP, 字段描述：_key:排行区间, _next_lv:下一等级, _need_gold:升级需求, _title1:称呼, _title2:称号, _desc:VIP描述, _reward:每日签到赠送, _open_function:功能开启, _desc2:商城VIP特权显示描述,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class VipConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// 排行区间
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _next_lv;
	/// <summary>
	/// 下一等级
	/// </summary>
	public int next_lv { get { return (int)_next_lv; } }
	private System.Object _need_gold;
	/// <summary>
	/// 升级需求
	/// </summary>
	public int need_gold { get { return (int)_need_gold; } }
	private System.Object _title1;
	/// <summary>
	/// 称呼
	/// </summary>
	public string title1 { get { return (string)_title1; } }
	private System.Object _title2;
	/// <summary>
	/// 称号
	/// </summary>
	public string title2 { get { return (string)_title2; } }
	private System.Object _desc;
	/// <summary>
	/// VIP描述
	/// </summary>
	public string desc { get { return (string)_desc; } }
	private System.Object _reward;
	/// <summary>
	/// 每日签到赠送
	/// </summary>
	public string reward { get { return (string)_reward; } }
	private System.Object _open_function;
	/// <summary>
	/// 功能开启
	/// </summary>
	public string open_function { get { return (string)_open_function; } }
	private System.Object _desc2;
	/// <summary>
	/// 商城VIP特权显示描述
	/// </summary>
	public int desc2 { get { return (int)_desc2; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_next_lv = Convert.ToInt32(element.Attribute("next_lv"));base["next_lv"]=_next_lv;
		_need_gold = Convert.ToInt32(element.Attribute("need_gold"));base["need_gold"]=_need_gold;
		_title1 = element.Attribute("title1");base["title1"]=_title1;
		_title2 = element.Attribute("title2");base["title2"]=_title2;
		_desc = element.Attribute("desc");base["desc"]=_desc;
		_reward = element.Attribute("reward");base["reward"]=_reward;
		_open_function = element.Attribute("open_function");base["open_function"]=_open_function;
		_desc2 = Convert.ToInt32(element.Attribute("desc2"));base["desc2"]=_desc2;
	}
}

public class VipConfig : ConfigDataBase<VipConfigItem> {
	public VipConfig(){
		_fileName = "export_xml/vip_config.bytes";
	}
}