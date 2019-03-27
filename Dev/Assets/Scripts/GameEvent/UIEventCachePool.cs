using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EventManager;

namespace EventManager
{
    /// <summary>
    /// UI事件缓存池，每个cotroller都会new一个
    /// </summary>
    [SLua.CustomLuaClass]
    public class UIEventCachePool
    {
        /// <summary>
        /// 事件缓存池
        /// </summary>
        private Queue<EventMultiArgs> _eventCachePool = new Queue<EventMultiArgs>();


        /// <summary>
        /// 添加缓存的事件
        /// </summary>
        public void SaveUIEvent(EventMultiArgs eventObj)
        {
            _eventCachePool.Enqueue(eventObj);
        }

        /// <summary>
        /// 执行所有缓存事件
        /// </summary>
        public void CallAllCacheEvent()
        {
            foreach (EventMultiArgs evntObj in _eventCachePool)
            {
                RunUIEvent(evntObj);
            }
            _eventCachePool.Clear();
        }

        /// <summary>
        /// 清空所有缓存事件
        /// </summary>
        public void ClearAllCacheEvent()
        {
            _eventCachePool.Clear();
        }

        //存储UI的回调函数
        private Dictionary<short, DelegateType.UIEventCallback> idEventCallback = new Dictionary<short, DelegateType.UIEventCallback>();

        public Dictionary<short, DelegateType.UIEventCallback> IdEventCallback
        {
            get
            {
                return idEventCallback;
            }
        }

        public void RegisterUIEvent(short eventID, DelegateType.UIEventCallback de)
        {
            if (idEventCallback.ContainsKey(eventID))
            {
                idEventCallback[eventID] += de;
            }
            else
            {
                idEventCallback[eventID] = de;
            }
        }
        public void UnRegisterUIEvent(short eventID, DelegateType.UIEventCallback de)
        {
            if (idEventCallback.ContainsKey(eventID))
                idEventCallback[eventID] -= de;
        }
        public void UnRegisterAllUIEvent()
        {
            idEventCallback.Clear();
        }
        public void RunUIEvent(EventMultiArgs args)
        {
            short eventID = (short)args.GetArg<short>("UI_EVENT_ID");
            if (idEventCallback.ContainsKey(eventID))
            {
                idEventCallback[eventID](args);
            }
        }

    }

    [SLua.CustomLuaClass]
    public static class UIEventID
    {
        public static short HERO_LIST_UPDATE_HEROS     = 1;//socket连接结果(bool result)
        //public static short HERO_DETAL_UPDATE_HERO   = 2;//刷新英雄装备界面
        //public static short AUTO_WEAR_UPDATE         = 2;//更新一键穿戴
        public static short MESSAGE_DIALOG_SET_TEXT    = 3;//设置弹窗提示文字
        public static short LIFE_POINT_UPDATE          = 4;//生命值刷新
        public static short SHOP_BUY_RESULT            = 5;//抽奖事件回调
        public static short LOTTERY_GET_HERO           = 6;//抽到英雄
        public static short STAR_UPGRADE_UPDATE        = 7;//更新升星成功界面
        public static short PHASE_UPGRADE_UPDATE       = 8;//更新升阶成功界面
        public static short SKILL_TIP_UPDATE           = 9;//更新技能提示界面
        public static short SKILL_TIP_CLOSE            = 10;//关闭技能提示界面
        public static short HERO_STONE_WIN_UPDATE      = 11;//获取灵魂石界面更新
        public static short TIPS                       = 12;//Tips 窗口
        public static short MESSAGE_WIN_SET_TEXT       = 13;//messagewin 窗口
        public static short CREATE_WIN_ACTION          = 14;//创建窗口时做动画
        public static short DESTROY_WIN_ACTION         = 15;//销毁窗口前做动画
        public static short ARENA_CLICK_RIVAL          = 16;//竞技场点击对手
        public static short ARENA_LONG_PRESS_RIVAL     = 17;//竞技场长按对手
        public static short ARENA_PRESS_CANCEL_RIVAL   = 18;//竞技场 取消长按 对手
        public static short ACTIVITY_WIN_UPDATE        = 19;//活动界面更新

        public static short MAIN_TOP_UPDATE            = 20;//主城顶部界面
        public static short LOTTERY_CUP_BOOM_BEGIN     = 21;//英雄招募时，酒杯开始爆炸
        public static short RESULT_WIN_BACK_TO_LOTERRY = 22;//招募结果界面返回到招募界面
        public static short HIDE_WIN                   = 23;
        public static short SHOW_WIN                   = 24;
        public static short LOTTERY_HERO_RUN_TO_TARGET = 25;//招募的英雄跑到了目标点
        public static short COPY_STARS_ACTION_START    = 26;//副本飞星动画开始
        public static short UPDATE_COPY_MAP            = 27;//更新副本界面
        public static short COPY_CLICK_BIG_NODE        = 28;//点副本场景中的大节点

        public static short COPY_PVP_UPDATE_ALL_AREA   = 29;//刷新所有章节的州牧信息
        public static short COPY_PVP_UPDATE_ONE_AREA   = 30;//刷新单个章节的官位信息
        public static short COPY_PVP_UPDATE_ONE_JOB    = 31;//刷新当前官位的防守阵容

        public static short UPDATE_GUIDE_DATA = 32;//更新新手教程完成情况
        public static short PHASE_EQUIP_UPGRADE_UPDATE = 33;//更新升阶成功界面
        public static short SET_MESSAGE_WITH_ICON = 34;//带图标的道具获取消息提示
        public static short LIMIT_BUY_RESULT = 35;//限时武将抽取
        public static short LIMIT_HERO_BUY_RESULT = 36;//限时神将抽取
        public static short SHOW_HELP_SURE = 37;//功能说明确认
        public static short MAIN_CITY_UPDATE = 38;//主城界面刷新
        //最大id为32767
    }
}