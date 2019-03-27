//表名: 百人赚金排行榜, 字段描述：_key:ID, _ranking:排名区间, _reward:奖励, _des:奖励,  -->
using System;
using System.Collections.Generic;
using Mono.Xml;
using System.Security;
using Utils;

public class RewardRankingConfigItem:ConfigDataItemBase{
//	private System.Object _key;
	/// <summary>
	/// ID
	/// </summary>
	public string key { get { return (string)_key; } }
	private System.Object _ranking;
	/// <summary>
	/// 排名区间
	/// </summary>
	public string ranking { get { return (string)_ranking; } }
	private System.Object _reward;
	/// <summary>
	/// 奖励
	/// </summary>
	public int reward { get { return (int)_reward; } }
	private System.Object _des;
	/// <summary>
	/// 奖励
	/// </summary>
	public string des { get { return (string)_des; } }

	public override void Parse(SecurityElement element) {
		_key = element.Attribute("key");base["key"]=_key;
		_ranking = element.Attribute("ranking");base["ranking"]=_ranking;
		_reward = Convert.ToInt32(element.Attribute("reward"));base["reward"]=_reward;
		_des = element.Attribute("des");base["des"]=_des;
	}
}

public class RewardRankingConfig : ConfigDataBase<RewardRankingConfigItem> {
	public RewardRankingConfig(){
		_fileName = "export_xml/reward_ranking_config.bytes";
	}
}