/***************************************************************
 

 *
 *
 * Filename:  	LocalSDKListener.cs	
 * Summary: 	本地SDK回调
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2017/08/02 20:03
 ***************************************************************/

using UnityEngine;
using System.Collections;
using UI.Controller;
using Scene;
namespace sdk.local
{
    public class LocalSDKListener : MonoBehaviour
    {
		string _signResult = "";
		public void onGetAvmpSignResult(string msg)
		{
			_signResult = msg;
		}
        public void onGetAvmpSign(string msg)
        {
            if (msg.Length == 0)
            {
                return;
            }
            var data = new JSONObject(msg);
            string phoneNum = extractString(data["input"].ToString());
            string type = extractString(data["type"].ToString());
            string signResult = extractString(data["signResult"].ToString());
			if (!string.IsNullOrEmpty (_signResult)) {
				signResult = _signResult;
			}
			Utils.LogSys.Log (signResult);
            if (type.Equals("1"))//登录
            {
                LoginInputController.startUpMono.VerificationLoginSign(phoneNum, signResult);
            }
            else if (type.Equals("1001"))//语音
            {
                LoginInputController.startUpMono.PhoneVerificationCode(phoneNum, type, signResult);
            }
            else if (type.Equals("1002"))//语音
            {
                LoginInputController.startUpMono.PhoneVerificationCode(phoneNum, type, signResult);
            }
            else if (type.Equals("1003"))//语音
            {
                LoginInputController.startUpMono.PhoneVerificationCode(phoneNum, type, signResult);
            }
        }
		/// <summary>
		/// 微信分享返回
		/// </summary>
		/// <param name="msg"></param>
		public void onWXShare(string msg)
		{
			if (msg.Length == 0)
			{
				return;
			}

			var data = new JSONObject(msg);
			string errCode = extractString(data["errCode"].ToString());
			Utils.LogSys.Log("share successfully - share result :" + errCode);

			if (errCode == "0") {
				UIManager.CallLuaFuncCall("ShareWin:ShareSuccess", null);
			}
		}
        public void onWXLogin(string msg)
        {
            if (msg.Length == 0)
            {
                Debug.Log("WeChat login error!");
                UtilTools.HideWaitWin(WaitFlag.LoginFirst);
                return;
            }

            var data = new JSONObject(msg);
            string errCode = extractString(data["errCode"].ToString());
            string wxCode = extractString(data["wxCode"].ToString());
            
            StartCoroutine("send2LocalAccountServer", wxCode);
        }
        /// <summary>
        /// 发送给本地账号服务器验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IEnumerator send2LocalAccountServer(object value)
        {
            string wxCode = value as string;

            WWWForm dataForm = new WWWForm();
            dataForm.AddField("t", "weixin");
            dataForm.AddField("code", wxCode);
            dataForm.AddField("qid", SDKManager.Q_ID);
            dataForm.AddField("iid", StartUpScene._bindStrng);
            string devid = "";
            if (sdk.SDKManager.isAppStoreVersion())
            {
                devid = object_c.ObjectCCallback._IDFA;
            }
            else
            {
                JARUtilTools tools = Scene.GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
                if (tools != null)
                {
                    devid = tools.GetIMEI(); //"fdsfd2";//
                    if (string.IsNullOrEmpty(devid))
                        devid = GameDataMgr.LOGIN_DATA.GetFastLoginUUID();
                }
                
            }
            dataForm.AddField("devid", devid);
            WWW www = new WWW(SDKManager.WxLoginUrl, dataForm);
            //string loginUrl = BetterString.Builder(SDKManager.LoginUrl, "?code=", wxCode, "&qid=", SDKManager.Q_ID.ToString());
            //WWW www = new WWW(loginUrl);

            yield return www;

            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                // 解析账号服务器返回的结果
                bool bRlt = GameDataMgr.LOGIN_DATA.parseAccountReturn(www.text, true, false, true);
                if (bRlt)
                {
                    GameDataMgr.LOGIN_DATA.IsLoginSuccess = true;
                    // 验证成功直接连接游戏服务器
                    LoginInputController.ConnectToServer();
                }
            }
            else
            {
                //TODO: process login exception
            }
        }
        public void OnGetShareParam(string msg)
        {
            Debug.Log("OpenInstall share 1!");
            if (msg.Length == 0)
            {
                Debug.Log("OpenInstall error");
                return;
            }
            var data = new JSONObject(msg);
            string errCode = extractString(data["errCode"].ToString());
            string bindData = extractString(data["bindData"].ToString());
            string channel = extractString(data["channelCode"].ToString());
            StartUpScene._bindStrng = bindData;

        }
        /// <summary>
        /// 抽取字符串值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string extractString(string str)
        {
            return str.Replace("\"", "");
        }
    }
}