//表名: 模型基础组件表, 字段描述：_key:模型ID, _element_body:身体, _element_head:头盔, _element_l_shoulder:左肩膀, _element_r_shoulder:右肩膀, _element_chest:胸甲, _element_belt:腰带, _element_leg:护腿, _element_l_foot:左脚, _element_r_foot:右脚, _element_cloak:披风, _element_primary_weapon:主武器, _element_sub_weapon:副武器,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class ElementConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// 模型ID
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _element_body;
	/// <summary>
	/// 身体
	/// </summary>
	public bool element_body { get { return (bool)_element_body; } }
	private System.Object _element_head;
	/// <summary>
	/// 头盔
	/// </summary>
	public bool element_head { get { return (bool)_element_head; } }
	private System.Object _element_l_shoulder;
	/// <summary>
	/// 左肩膀
	/// </summary>
	public bool element_l_shoulder { get { return (bool)_element_l_shoulder; } }
	private System.Object _element_r_shoulder;
	/// <summary>
	/// 右肩膀
	/// </summary>
	public bool element_r_shoulder { get { return (bool)_element_r_shoulder; } }
	private System.Object _element_chest;
	/// <summary>
	/// 胸甲
	/// </summary>
	public bool element_chest { get { return (bool)_element_chest; } }
	private System.Object _element_belt;
	/// <summary>
	/// 腰带
	/// </summary>
	public bool element_belt { get { return (bool)_element_belt; } }
	private System.Object _element_leg;
	/// <summary>
	/// 护腿
	/// </summary>
	public bool element_leg { get { return (bool)_element_leg; } }
	private System.Object _element_l_foot;
	/// <summary>
	/// 左脚
	/// </summary>
	public bool element_l_foot { get { return (bool)_element_l_foot; } }
	private System.Object _element_r_foot;
	/// <summary>
	/// 右脚
	/// </summary>
	public bool element_r_foot { get { return (bool)_element_r_foot; } }
	private System.Object _element_cloak;
	/// <summary>
	/// 披风
	/// </summary>
	public bool element_cloak { get { return (bool)_element_cloak; } }
	private System.Object _element_primary_weapon;
	/// <summary>
	/// 主武器
	/// </summary>
	public bool element_primary_weapon { get { return (bool)_element_primary_weapon; } }
	private System.Object _element_sub_weapon;
	/// <summary>
	/// 副武器
	/// </summary>
	public bool element_sub_weapon { get { return (bool)_element_sub_weapon; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_element_body = EngineUtils.ToBoolean(element.Attribute("element_body"));base["element_body"]=_element_body;
		_element_head = EngineUtils.ToBoolean(element.Attribute("element_head"));base["element_head"]=_element_head;
		_element_l_shoulder = EngineUtils.ToBoolean(element.Attribute("element_l_shoulder"));base["element_l_shoulder"]=_element_l_shoulder;
		_element_r_shoulder = EngineUtils.ToBoolean(element.Attribute("element_r_shoulder"));base["element_r_shoulder"]=_element_r_shoulder;
		_element_chest = EngineUtils.ToBoolean(element.Attribute("element_chest"));base["element_chest"]=_element_chest;
		_element_belt = EngineUtils.ToBoolean(element.Attribute("element_belt"));base["element_belt"]=_element_belt;
		_element_leg = EngineUtils.ToBoolean(element.Attribute("element_leg"));base["element_leg"]=_element_leg;
		_element_l_foot = EngineUtils.ToBoolean(element.Attribute("element_l_foot"));base["element_l_foot"]=_element_l_foot;
		_element_r_foot = EngineUtils.ToBoolean(element.Attribute("element_r_foot"));base["element_r_foot"]=_element_r_foot;
		_element_cloak = EngineUtils.ToBoolean(element.Attribute("element_cloak"));base["element_cloak"]=_element_cloak;
		_element_primary_weapon = EngineUtils.ToBoolean(element.Attribute("element_primary_weapon"));base["element_primary_weapon"]=_element_primary_weapon;
		_element_sub_weapon = EngineUtils.ToBoolean(element.Attribute("element_sub_weapon"));base["element_sub_weapon"]=_element_sub_weapon;
	}
}

public class ElementConfig : ConfigDataBase<ElementConfigItem> {
	public ElementConfig(){
		_fileName = "export_xml/element_config.bytes";
	}
}