/***************************************************************


 *
 *
 * Filename:  	Object-CInterface.cs	
 * Summary: 	Object-C導出的接口
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2016/04/04 23:03
 ***************************************************************/
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using version;
using UI.Controller;
using sdk;
using Scene;

namespace object_c
{
    public class ObjectCCallback : MonoBehaviour
    {
        public delegate void PayCallBack(string nsRlt, string sIdentifier, string sReceipt);
        public PayCallBack _payCallBack;
        static public string _deviceUUID;
        static public string _IDFA = "";
        
		string _curProductID = "";
        void Awake()
        {
#if UNITY_IOS
            ObjectCInterface.registerCallbackGameObject("Singleton");
            ObjectCInterface.registerScriptOnResponseRequest("OnRequestItem");
            ObjectCInterface.registerScriptObserver("OnBuyCompleteItem", "OnBuyFailedItem");
#endif
        }

        void testResult(string msg)
        {
            Utils.LogSys.Log("testResult:" + msg);
        }

        //这个是C#里的一个回调。用来接收数据是否传送成功。----这里的作用就是OC里的回调。
        void testBtnResult(string msg)
        {
            Utils.LogSys.Log("btnPressSuccessssssssssss:" + msg);
        }

        //请求充值产品回调(sRlt"0"成功  "1"失败)
        void OnRequestItem(string sRlt)
        {

            //Utils.LogSys.Log("--------------AppStorePay request callback -------------sRlt:" + sRlt);
            if (sRlt == "0")
            {

            }
            else if (sRlt == "1")
            {

            }
        }
        //获取IDFA

        void OnGetIDFA(string sIDFA)
        {
            _IDFA = sIDFA;
			Utils.LogSys.Log("XXXXXXXXXXXXXXXX--OnGetIDFA--XXXXXXXXXXXXXXXX :" + _IDFA);
        }

        //购买商品回调
        void OnBuyCompleteItem(string sJason)
        {
            //Utils.LogSys.Log("--------------AppStorePay callback 0-------------sJason:" + sJason);
            JSONObject arrStr = new JSONObject(sJason);
            Dictionary<string, string> _dic = arrStr.ToDictionary();
            if (_dic == null)
            {
                Utils.LogSys.Log("--------------AppStorePay callback 1-------------");
            }
            string nsRlt = "";////nRlt:0进行中 1成功 2失败 3重新提交 4排队中
            string nsIdentifier = "";
            string nsReceipt = "";
            string nsTransaction = "";
            if (_dic.ContainsKey("nsRlt"))
            {
                nsRlt = _dic["nsRlt"];
            }
            if (_dic.ContainsKey("nsIdentifier"))
            {
                nsIdentifier = _dic["nsIdentifier"];
            }
            if (_dic.ContainsKey("nsReceipt"))
            {
                nsReceipt = _dic["nsReceipt"];
            }

            if (_dic.ContainsKey("nsTransactionNum"))
            {
                nsTransaction = _dic["nsTransactionNum"];
            }
            ShopData.IOSPay_AddOrderNum(nsTransaction, nsReceipt, nsIdentifier);
            Utils.LogSys.Log("--------------AppStorePay callback 2-------------nsRlt:" + nsRlt + " ,nsIdentifier:" + nsIdentifier + " , nsReceipt:" + nsReceipt);

            BuyItem_AppStore_Callback(nsRlt, nsIdentifier, nsReceipt, nsTransaction);
        }

        //购买商品回调
        void OnBuyFailedItem(string sJason)
        {
            Utils.LogSys.Log("--------------AppStorePay callback 3-------------sJason:" + sJason);
            JSONObject arrStr = new JSONObject(sJason);
            Dictionary<string, string> _dic = arrStr.ToDictionary();
            if (_dic == null)
            {
                //Utils.LogSys.Log("--------------AppStorePay callback 4-------------");
            }
            string nsRlt = "";////nRlt:0进行中 1成功 2失败 3重新提交 4排队中
            string nsIdentifier = "";
            string nsReceipt = "";
            if (_dic.ContainsKey("nsRlt"))
            {
                nsRlt = _dic["nsRlt"];
            }
            if (_dic.ContainsKey("nsIdentifier"))
            {
                nsIdentifier = _dic["nsIdentifier"];
            }
            Utils.LogSys.Log("--------------AppStorePay callback 5-------------nsRlt:" + nsRlt + " ,nsIdentifier:" + nsIdentifier);

            BuyItem_AppStore_Callback(nsRlt, nsIdentifier, nsReceipt,"");
        }

        //购买商品回调
        void OnRestoreCallback(string sJason)
        {
            Utils.LogSys.Log("--------------AppStorePay callback 6-------------");
        }


        void OnGetDeviceUUID(string sUUID)
        {
            _deviceUUID = sUUID;
            Utils.LogSys.Log("XXXXXXXXXXXXXXXX--OnGetDeviceUUID--XXXXXXXXXXXXXXXX");
        }



#region appStore支付代码

        public void BuyItem_AppStore_Callback(string nsRlt, string sIdentifier, string sReceipt, string sTransaction)
        {
            _curProductID = sIdentifier;
            Utils.LogSys.Log("--------------AppStorePay callback 9-------------");
            if (nsRlt == "1")//成功
            {
                Utils.LogSys.Log("--------------AppStorePay callback 10-------------");
                GameObject sceneObj = GameObject.Find("Scene");
                if (sceneObj)
                {
                    StartUpScene startUpMono = sceneObj.GetComponent<StartUpScene>();
                    if (startUpMono != null)
                    {
                        startUpMono.ApppStoreRechargeCallback(_curProductID, sReceipt, sTransaction);
                        _curProductID = "";
                    }
                }
            }
            else if (nsRlt == "0" || nsRlt == "2")//进行中 ｜｜ 失败或取消
            {
                Utils.LogSys.Log("--------------AppStorePay callback 11-------------");
                StartCoroutine("BuyItem_AppStore_FailedCallback");
            }
        }

        IEnumerator BuyItem_AppStore_FailedCallback()
        {
            Utils.LogSys.Log("--------------AppStorePay callback 12-------------");
            yield return new WaitForSeconds(1.0f);
            UtilTools.ShowMessage("支付取消");
            UtilTools.HideWaitWin(WaitFlag.AppStorePay);
        }
#endregion


    }
}

