/***************************************************************


 *
 *
 * Filename:  	ConfigDataMgr.cs	
 * Summary: 	配置数据管理类
 *
 * Version:   	1.0.0
 * Author: 		XB.Wu
 * Date:   		2015/04/01 10:56
 ***************************************************************/

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

public enum ConfigDataType
{

    /// <summary>
    /// 模型基本组件表
    /// </summary>
    ModelElementConfig,
    //专属材料快速购买配置
    QuickBuyExclusiveConfig,
    //物品基础表
    ItemBaseConfig,
    StrengthConfig,
    GunBaseConfig,
    ResearchConfig,
    ShopConfig,
    HeadBaseConfig,
    MaskBaseConfig,
    FishArrayPath,//鱼阵表(游动的)
    FishArrayPosition,//鱼阵表(固定位置的)
    LevelNode,//关卡节点表
    BossFishBirth,//Boss出生时间
    StorageParam,//库存参数
    NormalFishBirth,//关卡出怪时间表
    FishNpcConfig,//关卡出怪时间表
    SkillBaseConfig,
    MagicBaseConfig,
    StaticConfig,
    VipConfig,
    LevelNeedConfig,
    NewbieguideConfig,//新手引导
    BattleTaskConfig,//战斗任务
    RewardPoolConfig,
    TitleConfig,
    BattleOnlineConfig, //战斗在线奖励表
    BossFishScoreConfig,
    BaseShopItemConfig, // 商店配置
    SharePictureConfig,
}

[SLua.CustomSingletonLuaClass]
public class ConfigDataMgr : Singleton<ConfigDataMgr>
{
    private Dictionary<ConfigDataType, ConfigDataCommon> allConfigData;

    public ConfigDataMgr()
    {


    }

    //清空所有数据对象
    public void ClearAll()
    {
        if (allConfigData != null)
        {
            allConfigData.Clear();
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void initialize()
    {
        //在此添加各个配置类
        allConfigData = new Dictionary<ConfigDataType, ConfigDataCommon>();
        
		AddData(ConfigDataType.ModelElementConfig, new ElementConfig());
        AddData(ConfigDataType.ItemBaseConfig, new ItemBaseConfig());
        AddData(ConfigDataType.BaseShopItemConfig, new BaseShopItemConfig());
        AddData(ConfigDataType.SharePictureConfig, new SharePictureConfig());        
        /*AddData(ConfigDataType.StrengthConfig, new StrengthConfig());
        AddData(ConfigDataType.GunBaseConfig, new GunLevelConfig());
        AddData(ConfigDataType.ResearchConfig, new ResearchConfig());
        AddData(ConfigDataType.ShopConfig, new ShopConfig());
        AddData(ConfigDataType.HeadBaseConfig, new HeadBaseConfig());
        AddData(ConfigDataType.MaskBaseConfig, new MaskBaseConfig());
        AddData(ConfigDataType.FishArrayPath, new FishArrayPath()); 
        AddData(ConfigDataType.FishArrayPosition, new FishArrayPosition()); 
        AddData(ConfigDataType.LevelNode, new LevelNode());
        AddData(ConfigDataType.BossFishBirth, new BossFishBirth()); 
        AddData(ConfigDataType.StorageParam, new StorageParam()); 
        AddData(ConfigDataType.NormalFishBirth, new NormalFishBirth()); 
        AddData(ConfigDataType.FishNpcConfig, new FishNpcConfig()); 
        AddData(ConfigDataType.SkillBaseConfig, new SkillBaseConfig());
        AddData(ConfigDataType.MagicBaseConfig, new MagicBaseConfig());
        AddData(ConfigDataType.StaticConfig, new StaticConfig());
        AddData(ConfigDataType.VipConfig, new VipConfig());
        AddData(ConfigDataType.LevelNeedConfig, new LevelNeedConfig());
//        AddData(ConfigDataType.NewbieguideConfig,new GuideConfig());
        AddData(ConfigDataType.RewardPoolConfig, new RewardPoolConfig());
        AddData(ConfigDataType.TitleConfig, new DesignationConfig());
        AddData(ConfigDataType.BattleTaskConfig, new BattleTaskConfig());
        AddData(ConfigDataType.BattleOnlineConfig, new BattleOnlineConfig());
        
        AddData(ConfigDataType.BossFishScoreConfig, new DiscFishConfig());*/
        //载入配置
        InitConfigData();
    }

    /// <summary>
    /// 消除本单件时调用
    /// </summary>
    /// 
    public void DestroyInstance()
    {
        ClearConfigData();
    }

    /// <summary>
    /// 重新载入配置数据
    /// </summary>
    public void ReloadConfigData()
    {
        foreach (var item in allConfigData)
        {
            if (item.Value != null)
            {
                item.Value.ReloadData();
            }
        }
    }

    /// <summary>
    /// 初始化配置数据
    /// </summary>
    private void InitConfigData()
    {
        //heroConfigBase.LoadXML();
        foreach (var item in allConfigData)
        {
            if (item.Value != null)
            {
                item.Value.LoadXML();
            }
        }
    }

    //清除配置数据
    private void ClearConfigData()
    {
        foreach (var item in allConfigData)
        {
            if (item.Value != null)
            {
                item.Value.ClearData();
            }
        }
    }

    public bool IsAllConfigLoaded()
    {
        foreach (var item in allConfigData)
        {
            if (item.Value != null && !item.Value.IsLoadedXML())
            {
                return false;
            }
        }
        return true;
    }

    public float GetConfigLoadProcess()
    {
        int count = allConfigData.Count;
        int ok_num = 0;
        foreach (var item in allConfigData)
        {
            if (item.Value != null && item.Value.IsLoadedXML())
            {
                ok_num++;
            }
        }
        return (float)ok_num / (float)count;
    }

    //添加一个配置数据
    private void AddData(ConfigDataType type, ConfigDataCommon data)
    {
        if (allConfigData.ContainsKey(type))
        {
            Utils.LogSys.Log(type.ToString() + " is already exist");
            return;
        }

        allConfigData.Add(type, data);
    }

    /// <summary>
    /// 获取配置数据
    /// </summary>
    /// <param name="type">配置数据类型</param>
    /// <returns>配置数据类</returns>
    public ConfigDataCommon GetData(ConfigDataType type)
    {
        if (allConfigData == null)
            return null;

        if (!allConfigData.ContainsKey(type))
        {
            LogSys.LogError("key don't exist " + type);
            return null;
        }
        return allConfigData[type];
    }
    public ItemBaseConfig ItemBaseConfig { get { return (ItemBaseConfig)GetData(ConfigDataType.ItemBaseConfig); } }
    public BaseShopItemConfig BaseShopItemConfig { get { return (BaseShopItemConfig)GetData(ConfigDataType.BaseShopItemConfig); } }
    public SharePictureConfig SharePictureConfig { get { return (SharePictureConfig)GetData(ConfigDataType.SharePictureConfig); } }
    /*public StrengthConfig StrengthConfig { get { return (StrengthConfig)GetData(ConfigDataType.StrengthConfig); } }
    public GunLevelConfig GunLevelConfig { get { return (GunLevelConfig)GetData(ConfigDataType.GunBaseConfig); } }
    public ResearchConfig ResearchConfig { get { return (ResearchConfig)GetData(ConfigDataType.ResearchConfig); } }
    public ShopConfig ShopConfig { get { return (ShopConfig)GetData(ConfigDataType.ShopConfig); } }
    public HeadBaseConfig HeadBaseConfig { get { return (HeadBaseConfig)GetData(ConfigDataType.HeadBaseConfig); } }
    public MaskBaseConfig MaskBaseConfig { get { return (MaskBaseConfig)GetData(ConfigDataType.MaskBaseConfig); } }
    public FishArrayPath FishArrayPath { get { return (FishArrayPath)GetData(ConfigDataType.FishArrayPath); } }
    public FishArrayPosition FishArrayPosition { get { return (FishArrayPosition)GetData(ConfigDataType.FishArrayPosition); } }
    public LevelNode LevelNode { get { return (LevelNode)GetData(ConfigDataType.LevelNode); } }
    public BossFishBirth BossFishBirth { get { return (BossFishBirth)GetData(ConfigDataType.BossFishBirth); } }
    public StorageParam StorageParam { get { return (StorageParam)GetData(ConfigDataType.StorageParam); } }
    public NormalFishBirth NormalFishBirth { get { return (NormalFishBirth)GetData(ConfigDataType.NormalFishBirth); } }
    public FishNpcConfig FishNpcConfig { get { return (FishNpcConfig)GetData(ConfigDataType.FishNpcConfig); } }


    public MagicBaseConfig MagicBaseConfig { get { return (MagicBaseConfig)GetData(ConfigDataType.MagicBaseConfig); } }
    public SkillBaseConfig SkillBaseConfig { get { return (SkillBaseConfig)GetData(ConfigDataType.SkillBaseConfig); } }
    public StaticConfig StaticConfig { get { return (StaticConfig)GetData(ConfigDataType.StaticConfig); } }
    public VipConfig VipConfig { get { return (VipConfig)GetData(ConfigDataType.VipConfig); } }
    public LevelNeedConfig LevelNeedConfig { get { return (LevelNeedConfig)GetData(ConfigDataType.LevelNeedConfig); } }
//    public GuideConfig NewbieguideConfig { get { return (GuideConfig)GetData(ConfigDataType.NewbieguideConfig); } }

    public BattleTaskConfig BattleTaskConfig { get { return (BattleTaskConfig)GetData(ConfigDataType.BattleTaskConfig); } }

    public RewardPoolConfig RewardPoolConfig { get { return (RewardPoolConfig)GetData(ConfigDataType.RewardPoolConfig); } }
    public DesignationConfig TitleConfig { get { return (DesignationConfig)GetData(ConfigDataType.TitleConfig); } }
    public BattleOnlineConfig BattleOnlineConfig { get { return (BattleOnlineConfig)GetData(ConfigDataType.BattleOnlineConfig); } }
    public DiscFishConfig BossFishScoreConfig { get { return (DiscFishConfig)GetData(ConfigDataType.BossFishScoreConfig); } }*/
    

    // public QuickBuyExclusiveConfig QuickBuyExclusiveConfig { get { return (QuickBuyExclusiveConfig)GetData(ConfigDataType.QuickBuyExclusiveConfig); } }
}
