/***************************************************************

 *
 *
 * Filename:  	LocalNotificationIOS.cs	
 * Summary: 	IOS的本地推送
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2016/03/17 21:33
 ***************************************************************/
using UnityEngine;
using System.Collections;
using PushNotification;


namespace PushNotification
{

#if UNITY_IOS

    public class LocalNotificationIOS : ILocalNotification
    {
        //本地推送 你可以传入一个固定的推送时间
        public override void AddNotificationMessage(string title, string message, System.DateTime newDate, string loopType, int badgeNum)
        {
            //推送时间需要大于当前时间
             if (newDate > System.DateTime.Now)
             {
                 UnityEngine.iOS.NotificationType notifiType = new UnityEngine.iOS.NotificationType();
                 notifiType = UnityEngine.iOS.NotificationType.Alert;
                 notifiType = UnityEngine.iOS.NotificationType.Badge;
                 notifiType = UnityEngine.iOS.NotificationType.Sound;
                 UnityEngine.iOS.NotificationServices.RegisterForNotifications(notifiType);


                 UnityEngine.iOS.LocalNotification localNotification = new UnityEngine.iOS.LocalNotification();
                 localNotification.fireDate = newDate;
                 localNotification.alertBody = message;
                 localNotification.alertAction = title;
                 localNotification.applicationIconBadgeNumber = 1;
                 localNotification.hasAction = true;
                 if (loopType == "day")
                 {
                     //是否每天定期循环
                     localNotification.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.ChineseCalendar;
                     localNotification.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;
                 }
                 else if (loopType == "week")
                 {
                     //是否每周定期循环
                     localNotification.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.ChineseCalendar;
                     localNotification.repeatInterval = UnityEngine.iOS.CalendarUnit.Week;
                 }
                 localNotification.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
                 UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(localNotification);
             }
        }

        //清空所有本地消息
        public override void CleanNotification()
        {
             UnityEngine.iOS.LocalNotification loc = new UnityEngine.iOS.LocalNotification();
             loc.applicationIconBadgeNumber = -1;
             UnityEngine.iOS.NotificationServices.PresentLocalNotificationNow(loc);
             UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
             UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
        }
    }

#endif
}