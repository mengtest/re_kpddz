//表名: 分享图片尺寸, 字段描述：_key:排名, _qrisize:大小, _pos_x:X轴, _pos_y:Y轴,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class SharePictureConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// 排名
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _qrisize;
	/// <summary>
	/// 大小
	/// </summary>
	public int qrisize { get { return (int)_qrisize; } }
	private System.Object _pos_x;
	/// <summary>
	/// X轴
	/// </summary>
	public int pos_x { get { return (int)_pos_x; } }
	private System.Object _pos_y;
	/// <summary>
	/// Y轴
	/// </summary>
	public int pos_y { get { return (int)_pos_y; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_qrisize = Convert.ToInt32(element.Attribute("qrisize"));base["qrisize"]=_qrisize;
		_pos_x = Convert.ToInt32(element.Attribute("pos_x"));base["pos_x"]=_pos_x;
		_pos_y = Convert.ToInt32(element.Attribute("pos_y"));base["pos_y"]=_pos_y;
	}
}

public class SharePictureConfig : ConfigDataBase<SharePictureConfigItem> {
	public SharePictureConfig(){
		_fileName = "export_xml/share_picture_config.bytes";
	}
}