//表名: 音效数据表, 字段描述：_key:ID, _time:时间,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class VoiceEffectConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// ID
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _time;
	/// <summary>
	/// 时间
	/// </summary>
	public int time { get { return (int)_time; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_time = Convert.ToInt32(element.Attribute("time"));base["time"]=_time;
	}
}

public class VoiceEffectConfig : ConfigDataBase<VoiceEffectConfigItem> {
	public VoiceEffectConfig(){
		_fileName = "export_xml/voice_effect_config.bytes";
	}
}