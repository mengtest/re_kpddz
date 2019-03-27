/***************************************************************

 *
 *
 * Filename:  	BaiDuSDK.cs	
 * Summary: 	百度语音管理
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2018/02/7 15:54
 ***************************************************************/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;
using UnityEngine.Networking;
using System.Net;
using System.IO;
using System;
using System.Text;

namespace sdk
{
    public class BaiDuData
    {
        public string format;
        public string rate;
        public string channel;
        public string cuid;
        public string token;
        public string speech;
        public string len;
    }

    public class AcceptanceIdentification
    {
        public string err_no;
        public string err_msg;
        public string sn;
        public string[] result;
        public string corpus_no;
    }

    public class BaiduTokenCallback
    {
        public string access_token;
    }


    /// <summary>
    /// SDKs 管理器
    /// </summary>
    public class BaiDuSDK : Singleton<BaiDuSDK>
    {
        public delegate void delegateTranslateComplete(int errorCode, string result);
        static public string sToken = "";//百度语音token
        string token_url = "https://openapi.baidu.com/oauth/2.0/token";
        string sound_to_text_url = "http://vop.baidu.com/server_api";
        public BaiDuSDK(){}
        public void Init()
        {
            StartCoroutine(GetToken());
        }

        IEnumerator GetToken()
        {
            using (UnityWebRequest www = UnityWebRequest.Get(SDKManager.BaiduTokenUrl))
            {
                yield return www.Send();

                if (www.isError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    // Show results as text
                    Debug.Log(www.downloadHandler.text);
                    BaiduTokenCallback acceptData = new BaiduTokenCallback();
                    JsonUtility.FromJsonOverwrite(www.downloadHandler.text, acceptData);
                    sToken = acceptData.access_token;
                }
            }
        }

        //声音转文字
        public void SoundToText(string sSoundData, int len, delegateTranslateComplete callBack)
        {
            StartCoroutine(GetAudioString(sSoundData, len, callBack));
        }


        // 把语音转换为文字
        private IEnumerator GetAudioString(string speech, int len, delegateTranslateComplete callBack)
        {
            //sToken = "24.90902d79c6ee916b366106a4b8eea0e0.2592000.1520950489.282335-10805541";
            JSONObject jsSend = JSONObject.Create();
            jsSend.AddField("format", "amr");
            jsSend.AddField("rate", 16000);
            jsSend.AddField("channel", "1");
            jsSend.AddField("cuid", GameDataMgr.PLAYER_DATA.Uuid);
            jsSend.AddField("token", sToken);
            jsSend.AddField("speech", speech);
            jsSend.AddField("len", len);
            string jsData = jsSend.ToString();
            byte[] post_byte = System.Text.Encoding.UTF8.GetBytes(jsData);

            var www = new UnityWebRequest(sound_to_text_url, "POST");
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(post_byte);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.uploadHandler.contentType = "application/json; charset=utf-8";
            www.SetRequestHeader("Content-Type", "application/json; charset=utf-8");

            yield return www.Send();
            if (www.isError)
            {
                www.Dispose();
                yield return new WaitForSeconds(0.5f);
                www = new UnityWebRequest(sound_to_text_url, "POST");
                www.uploadHandler = (UploadHandler)new UploadHandlerRaw(post_byte);
                www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                www.uploadHandler.contentType = "application/json";
                www.SetRequestHeader("Content-Type", "application/json");
                yield return www.Send();
            }
            if (www.isError)
            {
                www.Dispose();
                yield return new WaitForSeconds(0.5f);
                www = new UnityWebRequest(sound_to_text_url, "POST");
                www.uploadHandler = (UploadHandler)new UploadHandlerRaw(post_byte);
                www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                www.uploadHandler.contentType = "application/json";
                www.SetRequestHeader("Content-Type", "application/json");
                yield return www.Send();
            }
            if (www.isDone)
            {
                if (!www.isError)
                {
                    AcceptanceIdentification acceptData = new AcceptanceIdentification();
                    JsonUtility.FromJsonOverwrite(www.downloadHandler.text, acceptData);
                    if (acceptData != null)
                    {
                        string rlt = acceptData.result[0];
                        if (rlt.Substring(rlt.Length - 1, 1) == "，")
                            rlt = rlt.Substring(0, rlt.Length - 1);
                        Debug.Log(rlt);
                        rlt = GameText.Instance.StrFilter(rlt);
                        //UtilTools.MessageDialog(rlt);
                        if (callBack != null)
                        {
                            callBack(0, rlt);
                        }
                    }
                    else
                    {
                        if (callBack != null)
                        {
                            callBack(1, "");
                        }
                    }
                }
                else
                {
                    Debug.LogError(www.error);
                    if (callBack != null)
                    {
                        callBack(2, "");
                    }
                }
            }
        }
    }
}




















