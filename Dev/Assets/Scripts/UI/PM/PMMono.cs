/***************************************************************


 *
 *
 * Filename:  	PMMono.cs	
 * Summary: 	
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/04/15 16:24
 ***************************************************************/

using UnityEngine;
using System.Collections;
using UI.Controller;
using network;
using network.protobuf;
using System.Collections.Generic;
using HeroData;
using System;
using Msg;

public class PMMono : MonoBehaviour
{
    private PMController controller;
    private UIInput inputEditbox;
    private UILabel _lb;
    void Awake()
    {
        controller = (PMController)UIManager.GetControler(UIName.PM_WIN);

        inputEditbox = transform.Find("Container/Win/InputText").GetComponent<UIInput>();
        _lb = inputEditbox.GetComponent<UILabel>();
        UIEventListener.Get(transform.Find("Container/Win/CloseButton").gameObject).onClick = OnClickClose;
        UIEventListener.Get(transform.Find("Container/Win/send").gameObject).onClick = OnClickButton1;
        MsgCallManager.AddCallback(ProtoID.sc_player_chat, OnChatBack);//登录回调
    }

    void SendPmMsg(string msg) {
        cs_player_chat req = new cs_player_chat();
        req.room_type = 1;
        req.content = TextUtils.GetBytes(msg);
        req.obj_player_uuid = GameDataMgr.PLAYER_DATA.Uuid;
        //TODO 后面需要再加。。。
        ClientNetwork.Instance.SendMsg(ProtoID.cs_player_chat, req);
        inputEditbox.value = "";

    }

    private void OnChatBack(object proto)
    {
        if (proto==null){
            return;
        }

        var msg = proto as sc_player_chat;
        UIManager.DestroyWin(UIName.PM_WIN);
    }

    void OnClickClose(GameObject go)
    {
        UIManager.DestroyWin(UIName.PM_WIN);
    }
    void OnClickButton1(GameObject go)
    {
        SendPmMsg(_lb.text);
    }
}
