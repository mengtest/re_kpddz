//表名: 物品基础, 字段描述：_key:编号, _name:名称, _desc:物品描述, _icon:物品图标, _phase:品质, _cls:物品类型, _level_require:等级限制, _buy_gold:购买价格, _price:出售价格, _max_count:最大叠加, _tips:TIP信息,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class ItemBaseConfigItem:ConfigDataItemBase{
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
	private System.Object _desc;
	/// <summary>
	/// 物品描述
	/// </summary>
	public string desc { get { return (string)_desc; } }
	private System.Object _icon;
	/// <summary>
	/// 物品图标
	/// </summary>
	public string icon { get { return (string)_icon; } }
	private System.Object _phase;
	/// <summary>
	/// 品质
	/// </summary>
	public int phase { get { return (int)_phase; } }
	private System.Object _cls;
	/// <summary>
	/// 物品类型
	/// </summary>
	public int cls { get { return (int)_cls; } }
	private System.Object _level_require;
	/// <summary>
	/// 等级限制
	/// </summary>
	public int level_require { get { return (int)_level_require; } }
	private System.Object _buy_gold;
	/// <summary>
	/// 购买价格
	/// </summary>
	public string buy_gold { get { return (string)_buy_gold; } }
	private System.Object _price;
	/// <summary>
	/// 出售价格
	/// </summary>
	public int price { get { return (int)_price; } }
	private System.Object _max_count;
	/// <summary>
	/// 最大叠加
	/// </summary>
	public int max_count { get { return (int)_max_count; } }
	private System.Object _tips;
	/// <summary>
	/// TIP信息
	/// </summary>
	public string tips { get { return (string)_tips; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_name = element.Attribute("name");base["name"]=_name;
		_desc = element.Attribute("desc");base["desc"]=_desc;
		_icon = element.Attribute("icon");base["icon"]=_icon;
		_phase = Convert.ToInt32(element.Attribute("phase"));base["phase"]=_phase;
		_cls = Convert.ToInt32(element.Attribute("cls"));base["cls"]=_cls;
		_level_require = Convert.ToInt32(element.Attribute("level_require"));base["level_require"]=_level_require;
		_buy_gold = element.Attribute("buy_gold");base["buy_gold"]=_buy_gold;
		_price = Convert.ToInt32(element.Attribute("price"));base["price"]=_price;
		_max_count = Convert.ToInt32(element.Attribute("max_count"));base["max_count"]=_max_count;
		_tips = element.Attribute("tips");base["tips"]=_tips;
	}
}

public class ItemBaseConfig : ConfigDataBase<ItemBaseConfigItem> {
	public ItemBaseConfig(){
		_fileName = "export_xml/item_base_config.bytes";
	}
}