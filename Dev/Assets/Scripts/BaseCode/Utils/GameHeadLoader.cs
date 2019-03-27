/***************************************************************
 * Copyright (c) 2013 福建沃动计算机技术有限公司
 *         All rights reserved.
 *
 *
 * Filename:  	GameHeadLoader.cs
 * Summary: 	专门用来加载图片显示的
 *
 * Version:   	1.0.0
 * Author: 		YQ.Qu
 * Date:   		2017/3/1 0001 上午 9:39
 ***************************************************************/

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using asset;
using Utils;

public class GameHeadLoader : PhotoPickHander
{
    public static GameHeadLoader Instance;
    private GameHeadHttpUp httpUp;
    private static Dictionary<string, Texture2D> savePic = new Dictionary<string, Texture2D>();
    private static Dictionary<UITexture, string> waitUI = new Dictionary<UITexture, string>();

    void Awake()
    {
        Instance = this;
        httpUp = gameObject.GetComponent<GameHeadHttpUp>();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static IEnumerator LoadHeadS(string path, UITexture head, bool isPlayer)
    {
//        LogSys.LogWarning("图片已经加载到url=" + path);
        WWW www = new WWW(path);
        yield return www;
        if (head != null){
//            try{
            if (www != null && string.IsNullOrEmpty(www.error)){
                head.mainTexture = www.texture;
                if (isPlayer){
                    GameDataMgr.PLAYER_DATA.PlayerHead = www.texture;
                }
                else{
                    savePic[path] = www.texture;
                    //所有需要显示icon显示出来
                    foreach (KeyValuePair<UITexture, string> dic in waitUI){
                        if (dic.Key != head && dic.Value == path){
                            dic.Key.mainTexture = www.texture;
                            waitUI.Remove(dic.Key);
                        }
                    }
                }
                waitUI.Remove(head);
//                LogSys.LogWarning("图片已经加载到了。。。。" + path);
            }
            else{
//                head.mainTexture = www.texture;
//                LogSys.LogError("图片已经错误："+www.error);
            }
        }
    }

    public void LoadHead(string path, UITexture head, bool isPlayer)
    {
        if (string.IsNullOrEmpty(path)) return;
        if (savePic.ContainsKey(path)){
            head.mainTexture = savePic[path];
            return;
        }
        bool isWaiting = false;
        foreach (KeyValuePair<UITexture, string> dic in waitUI){
            if (dic.Value == path){
                waitUI[head] = path;
                break;
            }
        }
        StartCoroutine(LoadHeadS(path, head, isPlayer));
    }

    public override void onSuccessObject(string result, string filePath)
    {
//        UtilTools.MessageDialog("选择头像成功。。。result="+result+"  filePath = "+filePath);

        if (result.Equals("success")){
            Debug.Log("onsuccess:asyncHttpUploadFile=" + GameDataMgr.PLAYER_DATA.PicCount);
            int picount = int.Parse(GameDataMgr.PLAYER_DATA.PicCount);
            var count = picount + 1;

            if (count > 1){
                count = 0;
            }
            UtilTools.asyncHttpUploadFile(BaseConfig.HeadUpImgUrl,
                GameDataMgr.PLAYER_DATA.Account + count.ToString() + ".png", filePath);
        }
    }

    public override void onFailureObject(string error, string msg)
    {
        UtilTools.MessageDialog(GameText.GetStr("headIcon_selected_failed"));
    }

    public static IEnumerator LoadMainHead(string path)
    {
		Debug.Log (">>>>>>>>>>> LoadMainHead: " + path);

        WWW www = new WWW(path);
        yield return www;

		Debug.Log (">>>>>>>>>>>> LoadMainHead: " + www.error);

        if (www != null && string.IsNullOrEmpty(www.error)){
            GameDataMgr.PLAYER_DATA.PlayerHead = www.texture;
            LoginInputController.UpdateHeadShow();
        }
    }

    public void LoadMainTexture(string path)
    {
        if (string.IsNullOrEmpty(path)) return;
        StartCoroutine(LoadMainHead(path));
    }
}