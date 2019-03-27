//表名: 赌注房间表, 字段描述：_key:编号, _name:名称, _commision:佣金, _score:底分, _doorsill:门槛, _door_des:门槛描述, _taxed:对应场抽税比例,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class BettingRoomConfigItem:ConfigDataItemBase{
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
	private System.Object _commision;
	/// <summary>
	/// 佣金
	/// </summary>
	public int commision { get { return (int)_commision; } }
	private System.Object _score;
	/// <summary>
	/// 底分
	/// </summary>
	public int score { get { return (int)_score; } }
	private System.Object _doorsill;
	/// <summary>
	/// 门槛
	/// </summary>
	public string doorsill { get { return (string)_doorsill; } }
	private System.Object _door_des;
	/// <summary>
	/// 门槛描述
	/// </summary>
	public string door_des { get { return (string)_door_des; } }
	private System.Object _taxed;
	/// <summary>
	/// 对应场抽税比例
	/// </summary>
	public float taxed { get { return (float)_taxed; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_name = element.Attribute("name");base["name"]=_name;
		_commision = Convert.ToInt32(element.Attribute("commision"));base["commision"]=_commision;
		_score = Convert.ToInt32(element.Attribute("score"));base["score"]=_score;
		_doorsill = element.Attribute("doorsill");base["doorsill"]=_doorsill;
		_door_des = element.Attribute("door_des");base["door_des"]=_door_des;
		_taxed = (float)Convert.ToDouble(element.Attribute("taxed"));base["taxed"]=_taxed;
	}
}

public class BettingRoomConfig : ConfigDataBase<BettingRoomConfigItem> {
	public BettingRoomConfig(){
		_fileName = "export_xml/betting_room_config.bytes";
	}
}