/***************************************************************


 *
 *
 * Filename:  	ObjectCInterface.cs	
 * Summary: 	ObjectCInterface導出的接口
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2016/04/04 23:03
 ***************************************************************/
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace object_c
{
    public class ObjectCInterface
    {
        /// <summary>
        /// 注册回调函数绑定的gameobject
        /// </summary>
        /// <param name="objName"></param>
        [DllImport("__Internal")]
        public static extern void registerCallbackGameObject(string objName);
        [DllImport("__Internal")]
        public static extern void registerScriptOnResponseRequest(string callbackFunc);
        [DllImport("__Internal")]
		public static extern void registerScriptObserver(string completeCallbackFunc, string failedCallbackFunc);
        [DllImport("__Internal")]
        public static extern void requestItemInfo(string strItemList);
        [DllImport("__Internal")]
        public static extern void buyItem(string identifier);
        [DllImport("__Internal")]
        public static extern void openURL(string sUrl);
        [DllImport("__Internal")]
        public static extern void copyToPasteBoard(string copyStr);
        [DllImport("__Internal")]
		public static extern void getUUID();
		[DllImport("__Internal")]
		public static extern void getIDFA ();
		[DllImport("__Internal")]
		public static extern string getIPv6(string mHost,string mPort);

    }
}

