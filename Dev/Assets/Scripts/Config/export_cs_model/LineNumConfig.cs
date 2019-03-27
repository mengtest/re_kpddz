//表名: 单线投注, 字段描述：_key:排行区间, _gold_bet:单线投注,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class LineNumConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// 排行区间
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _gold_bet;
	/// <summary>
	/// 单线投注
	/// </summary>
	public int gold_bet { get { return (int)_gold_bet; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_gold_bet = Convert.ToInt32(element.Attribute("gold_bet"));base["gold_bet"]=_gold_bet;
	}
}

public class LineNumConfig : ConfigDataBase<LineNumConfigItem> {
	public LineNumConfig(){
		_fileName = "export_xml/line_num_config.bytes";
	}
}