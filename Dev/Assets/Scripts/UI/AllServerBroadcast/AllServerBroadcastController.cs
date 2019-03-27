using System.Text.RegularExpressions;
using UI.Controller;
using UnityEngine;
using System.Collections.Generic;
using network;
using network.protobuf;

public class AllServerBroadcastController : ControllerBase
{
    private AllServerBroadcastMono _mono;

    public List<string> broadcastContent;

    //红点是否显示
    public AllServerBroadcastController(string uiName)
    {
        sName = uiName;
        ELevel = UILevel.TOP;
        prefabsPath = new string[] {UIPrefabPath.BROADCAST_WIN};
        broadcastContent = new List<string>();
        MsgCallManager.AddCallback((ProtoID.sc_player_sys_notice), ShowBroadcast);
        MsgCallManager.AddCallback(ProtoID.sc_tips, OnServerTip);
    }

    private void ShowBroadcast(object proto)
    {
        if (proto == null) return;
        if (UIManager.IsWinShow("RichCarWin")) return;
        if (UIManager.IsWinShow("MainWin")) return;
        sc_player_sys_notice notice = (sc_player_sys_notice)proto;
        if (notice == null) return;
        if (notice.flag == 1)
        {
            broadcastContent.Add(TextUtils.GetString(notice.content));
        }
        if (broadcastContent.Count >= 1) UIManager.CreateWin(UIName.BROADCAST_WIN);
        
    }

    /// <summary>
    /// 后端主动发的Tip
    /// </summary>
    /// <param name="proto"></param>
    private void OnServerTip(object proto)
    {
        UtilTools.HideWaitFlag();
        if (proto == null) return;
        var msg = proto as sc_tips;
        switch (msg.type){
            case 1:
                UtilTools.ShowMessage(TextUtils.GetString(msg.text),TextColor.WHITE);
                break;
            case 2:
                UtilTools.MessageDialog(TextUtils.GetString(msg.text));
                break;
        }
    }


    /// <summary>
    /// 销毁前处理
    /// </summary>
    protected override void UIDestroyCallback()
    {
        if (_mono != null){
            UnityEngine.Object.DestroyImmediate(_mono);
            _mono = null;
        }
    }

    /// <summary>
    /// 界面加载完成后调用
    /// </summary>
    protected override void UICreateCallback()
    {
        _mono = winObject.AddComponent<AllServerBroadcastMono>();
    }

    void OnDestroy()
    {
//        MsgCallManager.RemoveCallback((ProtoID.SC_CNoticeWithLampResponse), ShowBroadcast);
        //broadcastContent.Clear();
    }

    public void ContinuePlay()
    {
        if (broadcastContent.Count >= 1){
            UIManager.CreateWin(UIName.BROADCAST_WIN);
        }
    }
}