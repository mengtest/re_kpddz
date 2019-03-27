//表名: 商城VIP, 字段描述：_key:vip等级, _icon:头像框, _title:标题, _des:描述,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class ShopVipConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// vip等级
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _icon;
	/// <summary>
	/// 头像框
	/// </summary>
	public string icon { get { return (string)_icon; } }
	private System.Object _title;
	/// <summary>
	/// 标题
	/// </summary>
	public string title { get { return (string)_title; } }
	private System.Object _des;
	/// <summary>
	/// 描述
	/// </summary>
	public string des { get { return (string)_des; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_icon = element.Attribute("icon");base["icon"]=_icon;
		_title = element.Attribute("title");base["title"]=_title;
		_des = element.Attribute("des");base["des"]=_des;
	}
}

public class ShopVipConfig : ConfigDataBase<ShopVipConfigItem> {
	public ShopVipConfig(){
		_fileName = "export_xml/shop_vip_config.bytes";
	}
}