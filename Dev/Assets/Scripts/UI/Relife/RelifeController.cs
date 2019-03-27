using System.Text.RegularExpressions;
using UI.Controller;
using UnityEngine;
using System.Collections.Generic;
using Msg;
using network.protobuf;
public class RelifeController : ControllerBase
{
    private RelifeMono _mono;
    public int _getCount = 0;
    //红点是否显示
    public RelifeController(string uiName)
    {
        sName = uiName;
        ELevel = UILevel.NORMAL;
        prefabsPath = new string[] { UIPrefabPath.RELIFE_WIN };
//        MsgCallManager.AddCallback((ProtoID.SC_CReliefAwardResponse), ShowWin);
    }

    private void ShowWin(object proto)
    {
        UtilTools.HideWaitWin(WaitFlag.Relife);
        if (proto == null)
            return;
        SC_CReliefAwardResponse msg = (SC_CReliefAwardResponse)proto;
        if (msg == null)
            return;
        _getCount = msg.money;
        if (_getCount > 0)
        {
            UIManager.CreateWinByAction(UIName.RELIFE_WIN);
        }
        
    }
    public static void SendRelifeRequest()
    {
        CS_CReliefRequest req = new CS_CReliefRequest();
//        ClientNetwork.Instance.SendMsg(ProtoID.CS_CReliefRequest, req);
    }

    /// <summary>
    /// 销毁前处理
    /// </summary>
    protected override void UIDestroyCallback()
    {
        if (_mono != null)
        {
            UnityEngine.Object.DestroyImmediate(_mono);
            _mono = null;
        }
    }

    /// <summary>
    /// 界面加载完成后调用
    /// </summary>
    protected override void UICreateCallback()
    {
        _mono = winObject.AddComponent<RelifeMono>();
    }

}
