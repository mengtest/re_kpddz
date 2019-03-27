using UnityEngine;
using System.Collections;
using System;

namespace sdk.local
{
    public class LocalSDKInterface : ISDKCommonInterfce
    {
        // 主activity对象
        private AndroidJavaObject _javaMainActivity = null;

        // SDK回调监听
        LocalSDKListener _listenerImpl = null;

        // 通知Go名字
        string _goName = "";

        public LocalSDKInterface(GameObject go)
        {
            _listenerImpl = go.AddComponent<LocalSDKListener>();
            _goName = go.name;
        }

        public override void exit()
        {
            ApplicationMgr.showExitDialog();
        }

        public override void init()
        {
#if UNITY_EDITOR
            return;
#endif
#if UNITY_ANDROID
            AndroidJavaClass ac = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            if (ac != null) _javaMainActivity = ac.GetStatic<AndroidJavaObject>("currentActivity");
            if (_javaMainActivity == null)
            {
                Utils.LogSys.LogError("setListener error, current activity is null");
            }
            else
            {
                _javaMainActivity.Call("setUnityGameObjectName", _goName);
            }
#elif UNITY_IOS
			_object_c_interface_.WXSDK.setUnityGameObjectName(_goName);
#endif
        }

        public override void login()
        {
            #if UNITY_ANDROID
            if (_javaMainActivity == null) return;
            _javaMainActivity.Call("requestWXLogin");
            #elif UNITY_IOS
                _object_c_interface_.WXSDK.requestWXLogin();
            #endif
        }

        public override void logout()
        {
            UtilTools.ReturnToLoginScene();
        }

        public override void pay(CommonOrderInfo orderInfo, CommonGameRoleInfo roleInfo)
        {
			#if UNITY_ANDROID

            if (_javaMainActivity == null) return;

            if (orderInfo == null || roleInfo == null) return;

            _javaMainActivity.Call("requestPay", orderInfo.goodsID, orderInfo.goodsName, orderInfo.goodsDesc,
                orderInfo.quantifier, orderInfo.cpOrderID, orderInfo.callbackUrl, orderInfo.extrasParams,
                orderInfo.price + "", orderInfo.amount + "", orderInfo.count + "", roleInfo.serverName,
                roleInfo.serverID, roleInfo.gameRoleName, roleInfo.gameRoleID, roleInfo.gameRoleBalance,
                roleInfo.vipLevel, roleInfo.gameRoleLevel, roleInfo.partyName);
#else
            _object_c_interface_.SystemUtilsOperation.requestPay(orderInfo.goodsID, orderInfo.goodsName, orderInfo.goodsDesc,
				orderInfo.quantifier, orderInfo.cpOrderID, orderInfo.callbackUrl, orderInfo.extrasParams,
				orderInfo.price + "", orderInfo.amount + "", orderInfo.count + "", roleInfo.serverName,
				roleInfo.serverID, roleInfo.gameRoleName, roleInfo.gameRoleID, roleInfo.gameRoleBalance,
                roleInfo.vipLevel, roleInfo.gameRoleLevel, roleInfo.partyName);
			#endif
        }
        public override void InitPaySdk(string appId, string partnerId)
        {
#if UNITY_ANDROID

            if (_javaMainActivity == null) return;

            _javaMainActivity.Call("InitPaySdk", appId,partnerId);
#elif UNITY_IOS
			_object_c_interface_.SystemUtilsOperation.InitPaySdk("wxca7116033db16bdf", appId, partnerId);
#endif
        }
        public override void Share(CommonShareInfo shareInfo)
        {
            if (_javaMainActivity == null) return;
            if (shareInfo == null) return;
            _javaMainActivity.Call("ShareWebPage", shareInfo.shareUrl, shareInfo.title, shareInfo.description, shareInfo.iconUrl, shareInfo.isToFriend);
        }
        public override void OnRegister(string Id)
        {
            if (_javaMainActivity == null) return;
            _javaMainActivity.Call("SignIn", Id);
        }
        public override void OnUmSdkInit(string appkey, string channel)
        {
            if (_javaMainActivity == null) return;
            _javaMainActivity.Call("InitUmSdk", appkey, channel);
        }
        public override void GetAvmpSign(string input,int type)
		{
			#if UNITY_EDITOR
			#elif UNITY_IOS
				_object_c_interface_.SystemUtilsOperation.avmSign(input,type);
			#else
				if (_javaMainActivity == null) return;
				_javaMainActivity.Call("GetAvmpSign", input, type);
			#endif

        }
        public override void updateRoleInfo(CommonGameRoleInfo roleInfo, bool bIsCreatRole)
        {
        }
        public override void ShareMutilPic(int flag, string desc, string img1, string img2, string img3, string img4, string img5, string img6)
        {
#if UNITY_EDITOR
#elif UNITY_IOS
			_object_c_interface_.WXSDK.ShareMutilPic(flag, desc, img1,img2,img3,img4,img5,img6);
#else
            if (_javaMainActivity == null) return;
            _javaMainActivity.Call("ShareMutilPic", flag, desc, img1, img2, img3, img4, img5, img6);
#endif
        }

    }
}