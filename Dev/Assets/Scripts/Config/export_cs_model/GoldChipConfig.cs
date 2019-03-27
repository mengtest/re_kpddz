//表名: 豪车下注金额, 字段描述：_key:ID, _gold_chip:押注额,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class GoldChipConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// ID
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _gold_chip;
	/// <summary>
	/// 押注额
	/// </summary>
	public int gold_chip { get { return (int)_gold_chip; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_gold_chip = Convert.ToInt32(element.Attribute("gold_chip"));base["gold_chip"]=_gold_chip;
	}
}

public class GoldChipConfig : ConfigDataBase<GoldChipConfigItem> {
	public GoldChipConfig(){
		_fileName = "export_xml/gold_chip_config.bytes";
	}
}