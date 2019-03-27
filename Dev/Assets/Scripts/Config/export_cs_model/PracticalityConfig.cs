//表名: 实物表, 字段描述：_key:编号, _name:名称, _name2:名称2, _cost_type:商品类型, _item_num:物品数, _cost_num:兑换需要钞票数, _cost_num2:兑换需要话费券, _cost_num3:兑换需要红包数, _discount_flag:折扣标识, _desc_common:描述, _exchange:每天每人限定兑换次数, _stock:总库存, _stock2:卡密库存, _vip_lvl:需要VIP等级, _tex:商品纹理, _sort:商品排序, _hot_sell:商品推荐,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class PracticalityConfigItem:ConfigDataItemBase{
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
	private System.Object _name2;
	/// <summary>
	/// 名称2
	/// </summary>
	public string name2 { get { return (string)_name2; } }
	private System.Object _cost_type;
	/// <summary>
	/// 商品类型
	/// </summary>
	public int cost_type { get { return (int)_cost_type; } }
	private System.Object _item_num;
	/// <summary>
	/// 物品数
	/// </summary>
	public int item_num { get { return (int)_item_num; } }
	private System.Object _cost_num;
	/// <summary>
	/// 兑换需要钞票数
	/// </summary>
	public string cost_num { get { return (string)_cost_num; } }
	private System.Object _cost_num2;
	/// <summary>
	/// 兑换需要话费券
	/// </summary>
	public int cost_num2 { get { return (int)_cost_num2; } }
	private System.Object _cost_num3;
	/// <summary>
	/// 兑换需要红包数
	/// </summary>
	public int cost_num3 { get { return (int)_cost_num3; } }
	private System.Object _discount_flag;
	/// <summary>
	/// 折扣标识
	/// </summary>
	public int discount_flag { get { return (int)_discount_flag; } }
	private System.Object _desc_common;
	/// <summary>
	/// 描述
	/// </summary>
	public string desc_common { get { return (string)_desc_common; } }
	private System.Object _exchange;
	/// <summary>
	/// 每天每人限定兑换次数
	/// </summary>
	public int exchange { get { return (int)_exchange; } }
	private System.Object _stock;
	/// <summary>
	/// 总库存
	/// </summary>
	public int stock { get { return (int)_stock; } }
	private System.Object _stock2;
	/// <summary>
	/// 卡密库存
	/// </summary>
	public int stock2 { get { return (int)_stock2; } }
	private System.Object _vip_lvl;
	/// <summary>
	/// 需要VIP等级
	/// </summary>
	public int vip_lvl { get { return (int)_vip_lvl; } }
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
	private System.Object _hot_sell;
	/// <summary>
	/// 商品推荐
	/// </summary>
	public int hot_sell { get { return (int)_hot_sell; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_name = element.Attribute("name");base["name"]=_name;
		_name2 = element.Attribute("name2");base["name2"]=_name2;
		_cost_type = Convert.ToInt32(element.Attribute("cost_type"));base["cost_type"]=_cost_type;
		_item_num = Convert.ToInt32(element.Attribute("item_num"));base["item_num"]=_item_num;
		_cost_num = element.Attribute("cost_num");base["cost_num"]=_cost_num;
		_cost_num2 = Convert.ToInt32(element.Attribute("cost_num2"));base["cost_num2"]=_cost_num2;
		_cost_num3 = Convert.ToInt32(element.Attribute("cost_num3"));base["cost_num3"]=_cost_num3;
		_discount_flag = Convert.ToInt32(element.Attribute("discount_flag"));base["discount_flag"]=_discount_flag;
		_desc_common = element.Attribute("desc_common");base["desc_common"]=_desc_common;
		_exchange = Convert.ToInt32(element.Attribute("exchange"));base["exchange"]=_exchange;
		_stock = Convert.ToInt32(element.Attribute("stock"));base["stock"]=_stock;
		_stock2 = Convert.ToInt32(element.Attribute("stock2"));base["stock2"]=_stock2;
		_vip_lvl = Convert.ToInt32(element.Attribute("vip_lvl"));base["vip_lvl"]=_vip_lvl;
		_tex = element.Attribute("tex");base["tex"]=_tex;
		_sort = Convert.ToInt32(element.Attribute("sort"));base["sort"]=_sort;
		_hot_sell = Convert.ToInt32(element.Attribute("hot_sell"));base["hot_sell"]=_hot_sell;
	}
}

public class PracticalityConfig : ConfigDataBase<PracticalityConfigItem> {
	public PracticalityConfig(){
		_fileName = "export_xml/practicality_config.bytes";
	}
}