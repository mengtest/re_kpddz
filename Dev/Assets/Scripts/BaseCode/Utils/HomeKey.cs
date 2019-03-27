using UnityEngine;
using System.Collections;
using network.protobuf;
using Scene;
using UI.Controller;

public class HomeKey : MonoBehaviour {

    public static bool _isLeaveTooLong = false;
    public static bool _isPause = false;
    private int _pauseTime = 0;
    private int _lostTime = 300;//5分钟后掉线
    void OnApplicationFocus(bool isFocus)
    {
                Utils.LogSys.Log("--------OnApplicationPause---" + isFocus+"     _isPause = "+_isPause);
        if (isFocus)
        {
            if (_isPause)
            {
                _isPause = false;
                int nowTime = UtilTools.GetClientTime();
				if (_pauseTime > 0 && nowTime - _pauseTime > _lostTime) {
					Utils.LogSys.Log("--------OnApplicationPause--- _isLeaveTooLong");
					_isLeaveTooLong = true;
					//UIManager.CallLuaFuncCall("OnApplicationFocus", gameObject);
					OnOKButton ();
					//UtilTools.ErrorMessageDialog(GameText.GetStr("leave_long_time"), "614d46", "Center", OnOKButton);
					//UIManager.CallLuaFuncCall("OnApplicationFocusReconnect", gameObject);
				} else if (ClientNetwork.Instance.IsConnected ()) {
					Utils.LogSys.Log("--------OnApplicationPause--- 1111");
					ClientNetwork.Instance.HeartBeastSwitch (true);
					UIManager.CallLuaFuncCall ("OnApplicationFocus", gameObject);
				} else if (!UIManager.IsWinShow(UIName.LOGIN_INPUT_WIN)){
					Utils.LogSys.Log("--------OnApplicationPause--- 2222");
					_isLeaveTooLong = true;
					OnOKButton();
				}
            }
        }
        else
        {
//            ClientNetwork.Instance.HeartBeastSwitch(false);
//            _pauseTime = UtilTools.GetClientTime();
            Utils.LogSys.Log("离开游戏 激活推送");  //  返回游戏的时候触发     执行顺序 1
        }
    }


    void OnOKButton()
    {
        _isLeaveTooLong = false;
        UtilTools.ShowWaitFlag();
        ClientNetwork.Instance.CloseSocket();
        ClientNetwork.Instance.Init();
        LoginInputController.LoginAccountServer();
        //UtilTools.ReturnToLoginScene();
    }

    void OnApplicationPause(bool isPause)
    {
        if (isPause)
        {
            _isPause = true;
            /*if (UIManager.IsWinShow(UIName.BATTLE_MAIN_WIN))
            {
                ClientNetwork.Instance.HeartBeastSwitch(false);
                ClientNetwork.Instance.CloseSocket();
            }
            else if (!UIManager.IsWinShow(UIName.LOGIN_INPUT))
            {
                //非登录界面时，暂停心跳包
                ClientNetwork.Instance.HeartBeastSwitch(false);
            }*/
            UIManager.CallLuaFuncCall("OnApplicationFocus", gameObject);
            Utils.LogSys.LogWarning("游戏已经游戏暂停");
            _pauseTime = UtilTools.GetClientTime();
			UIManager.DestroyWin(UIName.SHOP_RECHARGE_OTHER_WIN);
            if (ClientNetwork.Instance.IsConnected()){
                ClientNetwork.Instance.HeartBeastSwitch(false);
            }
            Utils.LogSys.Log("游戏暂停 一切停止");  // 缩到桌面的时候触发
        }
        else
        {
            Utils.LogSys.Log("游戏开始  万物生机");  //回到游戏的时候触发 最晚
        }
    }
}
