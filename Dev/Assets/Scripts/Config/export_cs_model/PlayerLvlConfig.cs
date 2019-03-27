//表名: 玩家升级经验表, 字段描述：_key:等级, _exp:主角升级经验,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class PlayerLvlConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// 等级
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _exp;
	/// <summary>
	/// 主角升级经验
	/// </summary>
	public int exp { get { return (int)_exp; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_exp = Convert.ToInt32(element.Attribute("exp"));base["exp"]=_exp;
	}
}

public class PlayerLvlConfig : ConfigDataBase<PlayerLvlConfigItem> {
	public PlayerLvlConfig(){
		_fileName = "export_xml/player_lvl_config.bytes";
	}
}