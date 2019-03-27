//奖励金币数量,描述
//表名: 牛牛游戏红包奖励, 字段描述：_key:ID, _total_gold:ID, _red_packet:类型, _describe:看牌抢庄累计获得金币数,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class CardRewardConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// ID
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _total_gold;
	/// <summary>
	/// ID
	/// </summary>
	public int total_gold { get { return (int)_total_gold; } }
	private System.Object _red_packet;
	/// <summary>
	/// 类型
	/// </summary>
	public int red_packet { get { return (int)_red_packet; } }
	private System.Object _describe;
	/// <summary>
	/// 看牌抢庄累计获得金币数
	/// </summary>
	public string describe { get { return (string)_describe; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_total_gold = Convert.ToInt32(element.Attribute("total_gold"));base["total_gold"]=_total_gold;
		_red_packet = Convert.ToInt32(element.Attribute("red_packet"));base["red_packet"]=_red_packet;
		_describe = element.Attribute("describe");base["describe"]=_describe;
	}
}

public class CardRewardConfig : ConfigDataBase<CardRewardConfigItem> {
	public CardRewardConfig(){
		_fileName = "export_xml/card_reward_config.bytes";
	}
}