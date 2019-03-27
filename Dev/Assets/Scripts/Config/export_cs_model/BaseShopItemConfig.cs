//表名: 商城表, 字段描述：_key:编号, _shop_type:商城类型, _item_id:物品, _item_num:物品数, _item_extra_num:额外获得数百分比, _cost_list:货币类型, _discount:折扣, _special_flag:特卖, _start_time:开始时间, _end_time:结束时间, _limit_condition:限制次数和刷新, _name:商品名称, _tex:商品纹理, _sort:商品排序, _vip_require:需要VIP等级,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class BaseShopItemConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// 编号
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _shop_type;
	/// <summary>
	/// 商城类型
	/// </summary>
	public int shop_type { get { return (int)_shop_type; } }
	private System.Object _item_id;
	/// <summary>
	/// 物品
	/// </summary>
	public int item_id { get { return (int)_item_id; } }
	private System.Object _item_num;
	/// <summary>
	/// 物品数
	/// </summary>
	public int item_num { get { return (int)_item_num; } }
	private System.Object _item_extra_num;
	/// <summary>
	/// 额外获得数百分比
	/// </summary>
	public int item_extra_num { get { return (int)_item_extra_num; } }
	private System.Object _cost_list;
	/// <summary>
	/// 货币类型
	/// </summary>
	public string cost_list { get { return (string)_cost_list; } }
	private System.Object _discount;
	/// <summary>
	/// 折扣
	/// </summary>
	public int discount { get { return (int)_discount; } }
	private System.Object _special_flag;
	/// <summary>
	/// 特卖
	/// </summary>
	public int special_flag { get { return (int)_special_flag; } }
	private System.Object _start_time;
	/// <summary>
	/// 开始时间
	/// </summary>
	public string start_time { get { return (string)_start_time; } }
	private System.Object _end_time;
	/// <summary>
	/// 结束时间
	/// </summary>
	public string end_time { get { return (string)_end_time; } }
	private System.Object _limit_condition;
	/// <summary>
	/// 限制次数和刷新
	/// </summary>
	public string limit_condition { get { return (string)_limit_condition; } }
	private System.Object _name;
	/// <summary>
	/// 商品名称
	/// </summary>
	public string name { get { return (string)_name; } }
	private System.Object _tex;
	/// <summary>
	/// 商品纹理
	/// </summary>
	public string tex { get { return (string)_tex; } }
	private System.Object _sort;
	/// <summary>
	/// 商品排序
	/// </summary>
	public int sort { get { return (int)_sort; } }
	private System.Object _vip_require;
	/// <summary>
	/// 需要VIP等级
	/// </summary>
	public int vip_require { get { return (int)_vip_require; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_shop_type = Convert.ToInt32(element.Attribute("shop_type"));base["shop_type"]=_shop_type;
		_item_id = Convert.ToInt32(element.Attribute("item_id"));base["item_id"]=_item_id;
		_item_num = Convert.ToInt32(element.Attribute("item_num"));base["item_num"]=_item_num;
		_item_extra_num = Convert.ToInt32(element.Attribute("item_extra_num"));base["item_extra_num"]=_item_extra_num;
		_cost_list = element.Attribute("cost_list");base["cost_list"]=_cost_list;
		_discount = Convert.ToInt32(element.Attribute("discount"));base["discount"]=_discount;
		_special_flag = Convert.ToInt32(element.Attribute("special_flag"));base["special_flag"]=_special_flag;
		_start_time = element.Attribute("start_time");base["start_time"]=_start_time;
		_end_time = element.Attribute("end_time");base["end_time"]=_end_time;
		_limit_condition = element.Attribute("limit_condition");base["limit_condition"]=_limit_condition;
		_name = element.Attribute("name");base["name"]=_name;
		_tex = element.Attribute("tex");base["tex"]=_tex;
		_sort = Convert.ToInt32(element.Attribute("sort"));base["sort"]=_sort;
		_vip_require = Convert.ToInt32(element.Attribute("vip_require"));base["vip_require"]=_vip_require;
	}
}

public class BaseShopItemConfig : ConfigDataBase<BaseShopItemConfigItem> {
	public BaseShopItemConfig(){
		_fileName = "export_xml/base_shop_item_config.bytes";
	}
}