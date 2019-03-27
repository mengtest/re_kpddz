/***************************************************************

 *
 *
 * Filename:  	LocalNotificationMgr.cs	
 * Summary: 	本地推送管理类
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2016/03/17 22:29
 ***************************************************************/
using UnityEngine;
using System.Collections;
using Utils;
using task;
using System.Xml.Linq;
using System.Collections.Generic;

namespace PushNotification
{
    public class LocalNotificationMgr : Singleton<LocalNotificationMgr>
    {
        private ILocalNotification localNotification = null;
        /// <summary>
        /// 初始化场景管理类
        /// </summary>
        public void initialize()
        {
#if UNITY_IOS
            localNotification = new LocalNotificationIOS();
#elif UNITY_EDITOR
   
#endif
            if (localNotification == null)
                return;

            string xmlPath = "Config/localNotification.json";

            AssetLoadTask task = new AssetLoadTask(xmlPath, null);
            task.EventFinished += new task.TaskBase.FinishedHandler(delegate(bool manual, TaskBase currentTask)
            {
                Object assetObj = ((AssetLoadTask)currentTask).getTargetAsset();
                if (assetObj != null)
                {
                    localNotification.CleanNotification();
                    ParseXML(assetObj.ToString());
                    //LogSys.Log("load config success :" + xmlPath);
                }
                else
                {
                    LogSys.LogError("load config failed:" + xmlPath);
                }
            });
        }

        /// <summary>
        /// 解析xml数据
        /// </summary>
        /// <param name="Config">xml文件字符串</param>
        public void ParseXML(string Config)
        {
            JSONObject arrStr = new JSONObject(Config);
            if (arrStr == null || arrStr.Count == 0)
            {
                Utils.LogSys.LogError("-------------localNotification Config is null : -------------");
                return;
            }
            List<JSONObject> myList = arrStr[0].list;
            if (myList == null || myList.Count == 0)
            {
                return;
                //Utils.LogSys.Log("--------------AppStorePay callback 1-------------");
            }
            System.DateTime nowDay = System.DateTime.Now;
            System.DateTime tempWeek = System.DateTime.Now;

            for (int i=0; i< myList.Count; i++)
            {
                JSONObject item_temp = myList[i];
                Dictionary<string, string> item = item_temp.ToDictionary();
                string loopType = "day";
                if (item.ContainsKey("open"))
                {
                    if (item["open"].ToLower() != "true")
                    {
                        continue;
                    }
                }
                int key = 0;
                if (item.ContainsKey("id"))
                {
                    int.TryParse(item["id"], out key);
                }
                string sTitle = "";
                if (item.ContainsKey("title"))
                {
                    sTitle = item["title"];
                }
                string sMessage = "";
                if (item.ContainsKey("text"))
                {
                    sMessage = item["text"];
                }

                string sWeek = "-1";
                int weekDay = 1;
                if (item.ContainsKey("week"))
                {
                    sWeek = item["week"].ToLower();
                    if (sWeek != "-1")
                    {
                        loopType = "week";
                        int.TryParse(item["week"], out weekDay);
                        weekDay = Mathf.Clamp(weekDay, 1, 7);
                    }
                }

                int yy = 0;
                if (item.ContainsKey("yy"))
                {
                    int.TryParse(item["yy"], out yy);
                }
                int mm = 0;
                if (item.ContainsKey("mm"))
                {
                    int.TryParse(item["mm"], out mm);
                }
                int dd = 0;
                if (item.ContainsKey("dd"))
                {
                    int.TryParse(item["dd"], out dd);
                }
                int hour = 0;
                if (item.ContainsKey("hour"))
                {
                    int.TryParse(item["hour"], out hour);
                }
                int min = 0;
                if (item.ContainsKey("minute"))
                {
                    int.TryParse(item["minute"], out min);
                }
                int second = 0;

                yy = (yy == -1) ? nowDay.Year : yy;
                mm = (mm == -1) ? nowDay.Month : mm;
                dd = (dd == -1) ? nowDay.Day : dd;
                hour = (hour == -1) ? nowDay.Hour : hour;
                min = (min == -1) ? nowDay.Minute : min;
                System.DateTime newDate = new System.DateTime(yy, mm, dd, hour, min, second);
                if (loopType == "week")
                {
                    int nowWeek = (int)nowDay.DayOfWeek - 1;//周天是0,周一是1......
                    if (nowWeek == -1) nowWeek = 6;
                    nowWeek += 1;//要转成周天是7, 周一是1, 周二是2
                    int offset = weekDay - nowWeek;
                    offset = offset < 0 ? offset + 7 : offset;
                    if (offset > 0)
                        newDate = newDate.AddDays(offset);
                }
                else
                {
                    if (nowDay.CompareTo(newDate) >= 0)//nowDay >= newDate
                    {
                        newDate = newDate.AddDays(1);//如果现在时间比闹钟时大，则取第二天时间
                        //newDate = new System.DateTime(nowDay.Year, nowDay.Month, nowDay.Day, hour, min, second);
                        Utils.LogSys.Log("--------------add notification next:" + newDate.ToString() + "-------------");
                    }
                    else
                    {
                        Utils.LogSys.Log("--------------add notification :" + newDate.ToString() + "-------------");
                    }
                }
                localNotification.AddNotificationMessage(sTitle, sMessage, newDate, loopType, key);
            }

        }

    }
}

