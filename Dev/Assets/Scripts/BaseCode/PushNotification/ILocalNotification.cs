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
namespace PushNotification
{

    public abstract class ILocalNotification
    {

        /// <summary>
        /// 添加一条本地推送
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="newDate">触发时间</param>
        /// <param name="isRepeatDay">每天重复</param>
        /// <param name="badgeNum">图标右上角的数量</param>
        /// <returns></returns>
        public abstract void AddNotificationMessage(string title, string message, System.DateTime newDate, string loopType, int badgeNum);


        /// <summary>
        /// 清空所有本地消息
        /// </summary>
        /// <returns></returns>
        public abstract void CleanNotification();
    }

}