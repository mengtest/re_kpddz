using System;


namespace battleBaseDefine
{

    [Obsolete("弃用的类")]
    public class MonsterDataStruct
    {
        public string key;

        /// <summary>
        /// 英雄ID
        /// </summary>
        public int hero_id;

        /// <summary>
        /// 英雄外形ID
        /// </summary>
        public string hero_view;

        /// <summary>
        /// 英雄名称
        /// </summary>
        public string hero_name;

        /// <summary>
        /// 英雄品质
        /// </summary>
        public int hero_colour;

        /// <summary>
        /// 英雄等级
        /// </summary>
        public int hero_lv;

        /// <summary>
        /// 是否为BOSS
        /// </summary>
        public float boss;

        /// <summary>
        /// 英雄战力
        /// </summary>
        public int hero_power;

        /// <summary>
        /// 最大生命值
        /// </summary>
        public float hp_max;

        /// <summary>
        /// 物理护甲
        /// </summary>
        public float armor;

        /// <summary>
        /// 魔法抗性
        /// </summary>
        public float resist;

        /// <summary>
        /// 护甲穿透
        /// </summary>
        public float armor_penet;

        /// <summary>
        /// 魔法穿透
        /// </summary>
        public float resist_penet;

        /// <summary>
        /// 物理攻击力
        /// </summary>
        public float phy_atk;

        /// <summary>
        /// 魔法攻击力
        /// </summary>
        public float mag_atk;

        /// <summary>
        /// 物理暴击
        /// </summary>
        public float phy_crit;

        /// <summary>
        /// 魔法暴击
        /// </summary>
        public float mag_crit;

        /// <summary>
        /// 闪避
        /// </summary>
        public float dodge;

        /// <summary>
        /// 吸血
        /// </summary>
        public float vampire;

        /// <summary>
        /// 治疗效果
        /// </summary>
        public float healing_effect;

        /// <summary>
        /// 技能冷却
        /// </summary>
        public float cooldowns;

        /// <summary>
        /// 生命秒回
        /// </summary>
        public float ph_regen;

        /// <summary>
        /// 物理伤害减免
        /// </summary>
        public float phy_reduce;

        /// <summary>
        /// 魔法伤害减免
        /// </summary>
        public float mag_reduce;

        /// <summary>
        /// 物理伤害提升
        /// </summary>
        public float phy_lift;

        /// <summary>
        /// 魔法伤害提升
        /// </summary>
        public float mag_lift;

        /// <summary>
        /// 技能0
        /// </summary>
        public int skill0_id;

        /// <summary>
        /// 技能等级
        /// </summary>
        public int skill0_lv;

        /// <summary>
        /// 技能1
        /// </summary>
        public int skill1_id;

        /// <summary>
        /// 技能等级
        /// </summary>
        public int skill1_lv;

        /// <summary>
        /// 技能2
        /// </summary>
        public int skill2_id;

        /// <summary>
        /// 技能等级
        /// </summary>
        public int skill2_lv;

        /// <summary>
        /// 技能3
        /// </summary>
        public int skill3_id;

        /// <summary>
        /// 技能等级
        /// </summary>
        public int skill3_lv;

        /// <summary>
        /// 技能4
        /// </summary>
        public int skill4_id;

        /// <summary>
        /// 技能等级
        /// </summary>
        public int skill4_lv;

        /// <summary>
        /// 技能5
        /// </summary>
        public int skill5_id;

        /// <summary>
        /// 技能等级
        /// </summary>
        public int skill5_lv;

        /// <summary>
        /// 推图固定奖励
        /// </summary>
        public string tuitu_drop_fixed;

        /// <summary>
        /// 推图随机奖励
        /// </summary>
        public string tuitu_drop_rand;

        /// <summary>
        /// 物理受伤比
        /// </summary>
        public float armor_hurt;

        /// <summary>
        /// 魔法受伤比
        /// </summary>
        public float resist_hurt;

        /// <summary>
        /// 索引器访问字段
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public object this[string keyName]
        {
            get { return GetType().GetField(keyName).GetValue(this); }
        }
    }
}