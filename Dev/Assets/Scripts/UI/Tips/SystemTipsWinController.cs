/***************************************************************


 *
 *
 * Filename:  	DetialTipsWinController.cs	
 * Summary: 	
 *
 * Version:   	1.0.0
 * Author: 		HR.Chen
 * Date:   		2016/3/14 17:42:39
 ***************************************************************/

#region Using
using EventManager;
using System.Collections.Generic;
using UI.Controller;
using UnityEngine;
using network;
using network.protobuf;
using System.Text.RegularExpressions;
#endregion


internal class SystemTipsWinController : ControllerBase
{
    #region Variable

    private SystemTipsWinMono _monoComponent;
    public List<string> _message=new List<string>();
    private static readonly Regex RegMsg = new Regex(@"\S+(?=\{)");
    public static bool isGetTip = false;
    #endregion

    #region Override And Constructor
   
    /// <summary>
    /// 构造函数，初始化类
    /// </summary>
    /// <param name="uiName"></param>
    public SystemTipsWinController(string uiName)
    {
        sName = uiName;
        Utils.LogSys.Log("Name = " + sName);
        ELevel = UILevel.TOP_TOP;
        prefabsPath = new string[] { UIPrefabPath.SYSTEM_TIPS_WIN };
       // MsgCallManager.AddCallback(ProtoID.SC_PLAYER_CHAT, OnChatCallBack);
    }
    public static void SetTip(bool isShow)
    {
        isGetTip = isShow;
    }
    private void OnChatCallBack(object proto)
    {
        if (proto == null)
            return;
//         sc_player_chat msg = (sc_player_chat)proto;
//         if (msg.flag == 9)
//         {
//             if(!UIManager.IsWinShow(UIName.CHAT_WIN))
//                 SetTip(true);
//             _message.Add(RegMsg.Match(TextUtils.GetString(msg.content)).ToString());
//         }
//         if (!UIManager.IsWinShow(UIName.SYSTEM_TIPS_WIN))
//         {
//             UIManager.CreateWin(UIName.SYSTEM_TIPS_WIN);
//         }
    }
    public void test()
    {
//         if (!UIManager.IsWinShow(UIName.CHAT_WIN))
//             SetTip(true);
//         _message.Add(RegMsg.Match("军师提示：[5bf6de]$name$[-]在竞技场中将你击败，你从第[e90101]$old_rank$名跌落至第[e90101]$new_rank$[-]名{Comm,ArenaWin}").ToString());
//         if (!UIManager.IsWinShow(UIName.SYSTEM_TIPS_WIN))
//         {
//             UIManager.CreateWin(UIName.SYSTEM_TIPS_WIN);
//         }
    }
    //void Start()
    //{

    //}

    /// <summary>
    /// 界面加载完成后的回调
    /// </summary>
    protected override void UICreateCallback()
    {
        _monoComponent = winObject.AddComponent<SystemTipsWinMono>();
    }

    /// <summary>
    /// 销毁后的处理
    /// </summary>
    protected override void UIDestroyCallback()
    {

        if (_monoComponent != null)
        {
            UnityEngine.Object.DestroyImmediate(_monoComponent);
            _monoComponent = null;
        }
    }

    /// <summary>
    /// 返回按钮
    /// </summary>
    /// <param name="go"></param>
    internal void GoBack()
    {
        UIManager.DestroyWin(UIName.SYSTEM_TIPS_WIN);
    }
    public void RemoveMessage()
    {
        if (_message.Count > 0)
        {
            _message.RemoveAt(0);
        }
    }
    #endregion
}