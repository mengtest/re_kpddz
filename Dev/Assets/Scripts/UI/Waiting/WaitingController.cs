using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI.Controller;
using EventManager;
using network;
using network.protobuf;

public enum WaitFlag{
    /// <summary>
    /// 待定
    /// </summary>
    Unkown,
    /// <summary>
    /// 连接帐服
    /// </summary>
    ConnectAccountServer,
    /// <summary>
    /// 切换用户连接帐服
    /// </summary>
    ChangeUserToAccountServer,
    /// <summary>
    /// 超时后自动进行第二次连接
    /// </summary>
    ConnectSocketFirst,
    /// <summary>
    /// 弹窗提示
    /// </summary>
    ConnectSocketSecond,
    /// <summary>
    /// 超时后自动创建新连接，再登录
    /// </summary>
    LoginFirst,
    /// <summary>
    /// 弹窗提示
    /// </summary>
    LoginScecond,
    /// <summary>
    /// 切换帐号
    /// </summary>
    ChangeAccount,
    /// <summary>
    /// 
    /// </summary>
    RuningAction,
    /// <summary>
    /// 异步开界面
    /// </summary>
    OpenWindow,
    /// <summary>
    /// 正在做表现
    /// </summary>
    DoingAction,

    /// <summary>
    /// 清除所有
    /// </summary>
    ClearAllWait,
    /// <summary>
    /// 新手引导
    /// </summary>
    NewbieGuide,
    /// <summary>
    /// 快速购买
    /// </summary>
    FastBuyReply,
    /// <summary>
    /// 非强制引导
    /// </summary>
    LevelGuide,
    /// <summary>
    /// appStore支付
    /// </summary>
    AppStorePay,
    /// <summary>
    /// 邮件领取奖励等待
    /// </summary>
    MailRewardGet,
    /// <summary>
    /// 在线奖励领取
    /// </summary>
    OnlineGetReward,
    MoJinGetReward,
    /// <summary>
    /// 积分查询
    /// </summary>
    WorldBossListReq,
    /// <summary>
    /// 世界Boss战斗开始
    /// </summary>
    WorldBossBattleStart,
    /// <summary>
    /// 霸体觉醒
    /// </summary>
    PabodyWakeUp,
    /// <summary>
    /// 霸体升级
    /// </summary>
    PabodyLevelUp,

    TopArenaFightStart,

    SellItems,
    /// <summary>
    /// 世界Boss提交新阵容
    /// </summary>
    WorldBossChangeFormation,

    SaveFormation,
    /// <summary>
    /// 发送邀请
    /// </summary>
    InvitePeopleAdd,
    //洗练请求
    EquipCastReq,
    //救济请求
    Relife,
    //强化请求
    Strength,

    //退出战斗请求
    ExitBattle,
    //转盘
    DiscWin,
    //登陆
    LoginWin,
    //注册帐号
    RegisterAccount,
    //绑定
    BindPhone,
    //修改密码
    ChangePassword,
    ActivityRequest,
}

public class WaitingController : ControllerBase {

    private WaitingMono monoComponent;
    List<WaitFlag> _listWaitFlag = new List<WaitFlag>();
    Dictionary<WaitFlag, Timer> _dicTimer = new Dictionary<WaitFlag, Timer>();
    internal Action BackAction = null;
    public WaitingController(string uiName)
	{
		sName = uiName;
		ELevel = UILevel.TOP;
		prefabsPath = new string[] { UIPrefabPath.WAITING };
	}

    /// <summary>
    /// 界面加载完成后的回调
    /// </summary>
    protected override void UICreateCallback()
    {
        monoComponent = winObject.AddComponent<WaitingMono>();
    }

    /// <summary>
    /// 销毁后的处理
    /// </summary>
    protected override void UIDestroyCallback()
    {

        if (monoComponent != null)
        {
            UnityEngine.Object.DestroyImmediate(monoComponent);
            monoComponent = null;
        }
    }

    public void ShowWaitingWin(WaitFlag eFlag = WaitFlag.Unkown, float fWaitTime = 10f)
    {
        if (_dicTimer.ContainsKey(eFlag))
        {
            TimerManager.GetInstance().RemoveTimer(_dicTimer[eFlag]);
            _dicTimer.Remove(eFlag);
        }
        if (_listWaitFlag.Contains(eFlag))
            _listWaitFlag.Remove(eFlag);

        //_waitTimer = new Timer(fWaitTime, WaitForTimeOut, 0, false);
        Timer waitTimer = new Timer(
            fWaitTime,
            new TimerEvent(delegate(){WaitForTimeOut(eFlag);}),
            0,
            false);

        _dicTimer.Add(eFlag, waitTimer);
        _listWaitFlag.Add(eFlag);

        if (eFlag == WaitFlag.DoingAction && monoComponent != null)
        {
            monoComponent.HideJuHua();
        }
        if (eFlag == WaitFlag.NewbieGuide && monoComponent != null)
        {
            monoComponent.HideJuHua();
        }
        if (eFlag == WaitFlag.SaveFormation && monoComponent != null)
        {
            monoComponent.HideJuHua();
        }
        if (eFlag == WaitFlag.EquipCastReq && monoComponent != null)
        {
            monoComponent.HideJuHua();
        }

        if (monoComponent != null)
        {
            monoComponent.HideJuHua();
            monoComponent.ShowWin();
        }
            
    }

    //等待超时
    public void WaitForTimeOut(WaitFlag eFlag)
    {
        if (_listWaitFlag.Count == 0)
            return;

        if (_dicTimer.ContainsKey(eFlag))
            _dicTimer.Remove(eFlag);
        if (_listWaitFlag.Contains(eFlag))
            _listWaitFlag.Remove(eFlag);
        if (monoComponent != null && _listWaitFlag.Count == 0)
            monoComponent.HideWin();
        EventMultiArgs args;
        switch (eFlag){
			case WaitFlag.AppStorePay:
				break;
            case WaitFlag.ConnectSocketFirst:
                ShowWaitingWin(WaitFlag.ConnectSocketSecond, 10f);
                ClientNetwork.Instance.CloseSocket();
                ClientNetwork.Instance.Connect(); //第一次connect超时，直接尝试第二次连接socket。
                break;
            case WaitFlag.ConnectSocketSecond:
                UtilTools.PlaySoundEffect("Sounds/UISound/error");
                UtilTools.ErrorMessageDialog(GameText.GetStr("socket_connect_timeout"), "614d46", "Center",
                    OnOKButton); //第二次还超时，弹窗提示。
                UtilTools.HideWaitWin(WaitFlag.ClearAllWait);
                ClearAllWait();
                break;
            case WaitFlag.LoginFirst:
                ShowWaitingWin(WaitFlag.LoginScecond, 6f);
                EngineManager engine = EngineManager.GetInstance();
//                Msg.CS_loggin login = new Msg.CS_CAccountLoginRequest();
                var login = new cs_login_reconnection();
//                login.account = GameDataMgr.LOGIN_DATA.nowLoginAccount;//tools.GetDeviceUUID(); //
//                login.pwd = GameDataMgr.LOGIN_DATA.nowLoginPassword;
                /*login.platform_flag = (uint) engine.GetSDKId();
                login.user = GameDataMgr.LOGIN_DATA.userName;
                login.reconnect_key =
                ClientNetwork.Instance.SendMsg(ProtoID.SC_LOGIN_RECONNECTION_REPLY, login);*/
                //TODO loging问题后面再看看
                ClientNetwork.ToSendReconnectMsgSecond();
                break;
            case WaitFlag.LoginScecond:
                UtilTools.LoginFailedAndShowLoginWin();
                break;
            case WaitFlag.ChangeAccount:
                GameDataMgr.LOGIN_DATA.IsConnectGamerServer = false;
                //UIManager.GetControler<ChangeAccountController>().ChangeAccountTimeOut();
                break;
            default://菊花超时
                ClientNetwork.Instance.ReconnectWithoutAsk(false);
                break;
        }
        Utils.LogSys.Log("WaitForTimeOut:" + eFlag.ToString());
    }

    public void HideWaitingWin(WaitFlag eFlag = WaitFlag.Unkown)
    {
        if (eFlag == WaitFlag.ClearAllWait)
        {
            ClearAllWait();
            return;
        }

        if (_dicTimer.ContainsKey(eFlag))
        {
            TimerManager.GetInstance().RemoveTimer(_dicTimer[eFlag]);
            _dicTimer.Remove(eFlag);
        }

        if (_listWaitFlag.Contains(eFlag))
            _listWaitFlag.Remove(eFlag);

        if (eFlag == WaitFlag.DoingAction && monoComponent != null)
        {
            monoComponent.ShowJuHua();
        }
        if (eFlag == WaitFlag.NewbieGuide && monoComponent != null)
        {
            monoComponent.ShowJuHua();
        }

        if (monoComponent != null && _listWaitFlag.Count == 0)
            monoComponent.HideWin();
    }

    public void ClearAllWait()
    {
        foreach (KeyValuePair<WaitFlag,Timer> keyValue in _dicTimer)
        {
            TimerManager.GetInstance().RemoveTimer(keyValue.Value);
        }
        _dicTimer.Clear();
        _listWaitFlag.Clear();
        if (monoComponent != null)
            monoComponent.HideWin();
    }

    void OnOKButton()
    {
        UtilTools.ReturnToLoginScene();
    }

    public bool IsWaitShowing(WaitFlag eFlag)
    {
        if (eFlag == null)
            return false;
        if (_dicTimer.ContainsKey(eFlag))
        {
            return true;
        }
        return false;
    }
}
