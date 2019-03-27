using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using EventManager;
using network.protobuf;
using Scene;
using UI.Controller;

public struct SeverDetail
{
    public string key;
    public int newTag;
    public int state;
    public string serverName;
    public int httpPort;
    public int port;
    public string ip;
}

public struct UserData
{
    public string serverId;
    public string userName;
    public string userPassword;
}

/// <summary>
/// 账号来源
/// </summary>
public enum EAccountInfoSrc
{
    eLocal, //本地已经存储
    eNewInput, //新输入
    eNewRegist, //新注册
}


/// <summary>
/// 账号服务器返回action
/// </summary>
public enum EAccountServerAction
{
    eNone = -1,
    eLogin,     //登陆
    eRegist,    //注册
};

/// <summary>
/// 账号服务器返回错误代码
/// </summary>
public enum EAccountServerErrorCode
{
    eNoError,
    eError1,
    eError2,
};

/// <summary>
/// 账号服务器返回结果
/// </summary>
public struct AccountServerResult
{
    public string Action;
    public string ErrorCode;
    public string Content;
    public string ip;
    public string port;

    /// <summary>
    /// 是否出错
    /// </summary>
    public bool bError()
    {
        return !ErrorCode.Equals("0");
    }
}

public class LoginData : IGameData
{

    public string userName;
    public string userPassword;
    public string serverName = "双线1服";
    public string serverState;
    public string serverIp = "";
    public int serverPort;
    public int serverHttpPort;
    public int iServerState;
    public EAccountInfoSrc AccountSrc = EAccountInfoSrc.eLocal;
    public string serverId = "1";
    public int tag;
    private bool _isLoginFirst = true;
    private bool _isShowChangeUserWin = true;
    private bool _isLoginSuccess = false;//帐号登录成功
    private bool _isGetedGameServer = false;
    private bool _isConnectGameServer = false;//连接游服socket成功
    private bool _isLoginGameServer = false;//登录游服成功
    private int i = 0;
    private string[] _serverStateImg = { "free", "crowded", "busy", "maintenance" };
    public string nowLoginAccount { get; set; }
    public string nowLoginPassword { get; set; }
    public string lastLoginAccount { get; set; }
    public string lastLoginPassword { get; set; }
    public string lastLoginDeviceUUID { get; set; }//只有在快速登录成功时，此变量才不为空
    public string loginTokenID;
    public bool IsGetGameServerIP//是否已获得游服IP
    {
        set { _isGetedGameServer = value; }
        get { return _isGetedGameServer; }
    }
    public bool IsConnectGamerServer//是否已连接游服
    {
        set { _isConnectGameServer = value; }
        get { return _isConnectGameServer; }
    }
    public bool IsLoginGameServer//是否已登录游服
    {
        set { _isLoginGameServer = value; }
        get { return _isLoginGameServer; }
    }
    /// <summary>
    /// 连接账号服务器返回结果
    /// </summary>
    AccountServerResult _accountServerRlt = new AccountServerResult();


    private Dictionary<string, SeverDetail> serverDict;

    public LoginData()//构造函数,进行初始化
    {
        serverDict = new Dictionary<string, SeverDetail>();
        lastLoginAccount = PlayerPrefs.GetString("lastLoginAccount", "");
        lastLoginPassword = PlayerPrefs.GetString("lastLoginPassword", "");
        lastLoginDeviceUUID = PlayerPrefs.GetString("lastLoginDeviceUUID", "");
        //是否第三方登录
        if (ClientDefine.THIRD_PARTY_SDK)
        {
            _isLoginFirst = false;
        }
//        PlayerPrefs.SetString("FastLoginUUID", "");
    }

    /// <summary>
    /// 将用户最近登录的服务器信息存在本地
    /// </summary>
    public void SetUserData()
    {
        if (string.IsNullOrEmpty(userPassword))//游客
        {
            PlayerPrefs.SetString("lastUserName", WWW.EscapeURL(userName));
        }
        else
        {
            PlayerPrefs.SetString("lastUserName", userName);
        }
        PlayerPrefs.SetString("lastPassword", userPassword);
    }


    /// <summary>
    /// 获取本地数据
    /// </summary>
    public void GetLocalData()
    {
        //如果当前输入为空则显示前一用户登录的用户名及服务器
        if (userName == null && userPassword == null)
        {
            userPassword = PlayerPrefs.GetString("lastPassword", "");
            if(string.IsNullOrEmpty(userPassword))//游客
            {
                userName = WWW.UnEscapeURL(PlayerPrefs.GetString("lastUserName", ""));
            }
            else//非游客
            {
                userName = PlayerPrefs.GetString("lastUserName", "");
            }
        }
    }

    public int CurrServerNum { set; get; }

    private void UpdateLastLoginInfo()
    {
        
        BaseConfig.IP = _accountServerRlt.ip;
        int.TryParse(_accountServerRlt.port, out BaseConfig.port);
    }

    /// <summary>
    /// 保存服务器列表的数据
    /// </summary>
    public void AddServerData(string id, int newTag, int state, string srvName, int httpPort, int port, string ip)
    {
        if (serverDict.ContainsKey(id))
        {
            Utils.LogSys.Log("server error!!!!");
            return;
        }
        // TODO: Complete member initialization
        SeverDetail serverData = new SeverDetail();
        serverData.key = id;
        serverData.newTag = newTag;
        serverData.state = state;
        serverData.serverName = srvName;
        serverData.httpPort = httpPort;
        serverData.port = port;
        serverData.ip = ip;
        serverDict.Add(id, serverData);

        if (newTag == 1 && i == 0)
        {
            BaseConfig.IP = ip;
            BaseConfig.HttpPort = httpPort;
            BaseConfig.port = port;
            BaseConfig.ServerState = state;
            i++;
        }
    }

    /// <summary>
    /// 获取服务器数据
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, SeverDetail> GetServerData()
    {
        return serverDict;
    }
    public bool IsRegister()
    {
        return _accountServerRlt.Action.Equals("1");
    }
    public string _UserId { set; get; }

    /// <summary>
    /// 解析账号服务器返回结果(登录)
    /// </summary>
    /// <param name="str"></param>
    public bool parseAccountReturn(string str, bool bLogin = false, bool bAutoLogin = false, bool bWXlogin = false)
    {
        Utils.LogSys.Log(str);

        //解析返回内容
        JSONObject arrStr = new JSONObject(str);
        bool isSuccess = false;
        //解析返回内容
        if (bLogin)
        {
            _accountServerRlt.Action = arrStr[0].str;//0登陆 1注册
            _accountServerRlt.ErrorCode = arrStr[1].str;
            _accountServerRlt.Content = arrStr[2].str;//
            _accountServerRlt.ip = arrStr[3].str;//
            _accountServerRlt.port = arrStr[4].str;//servier_id, head_img, level,  
            _accountServerRlt.Content = _accountServerRlt.Content.Replace("[", "").Replace("]", "");
            //错误提示
            isSuccess = ErrorTip(_accountServerRlt.Action, _accountServerRlt.ErrorCode, _accountServerRlt.Content);
            if (isSuccess)
            {
                //TODO 先接捕鱼登陆平台，IP及Port使用默认的
                if (!ClientDefine.isUseBuYuNet){
                    BaseConfig.IP = _accountServerRlt.ip;
                    int.TryParse(_accountServerRlt.port, out BaseConfig.port);
                }
                if (_accountServerRlt.Action.Equals("0"))//登录成功
                {
                    //UtilTools.ShowMessage(GameText.GetStr("loginTip1"), TextColor.GREEN);
                }
                else if (_accountServerRlt.Action.Equals("1"))//注册成功
                {
                    UtilTools.ShowMessage(GameText.GetStr("loginTip2"), TextColor.GREEN);
                }
                if (bWXlogin)
                {
                    if (!string.IsNullOrEmpty(_accountServerRlt.Content))
                    {
                        PlayerPrefs.SetString("accountServerLoginContent", _accountServerRlt.Content);
                    }
                    if (bAutoLogin)
                    {
                        _accountServerRlt.Content = PlayerPrefs.GetString("accountServerLoginContent", "");
                    }
                }
                else
                {

                }
            }
            else
            {
                PlayerPrefs.SetString("accountServerLoginContent", "");
            }
        }
        else
        {
            string action = string.IsNullOrEmpty(arrStr[0].str)?"0" : arrStr[0].str;
            string code = string.IsNullOrEmpty(arrStr[1].str) ? "0" : arrStr[1].str;
            string content = string.IsNullOrEmpty(arrStr[2].str) ? "0" : arrStr[2].str;
            isSuccess = ErrorTip(action, code, content);
        }

        //ReYunUtils.Track_Register(_UserId);
        //打开通告
        return isSuccess;
    }

    private bool ErrorTip(string action, string code, string content)
    {
        if (!code.Equals("0"))
        {
            UtilTools.PlaySoundEffect("Sounds/UISound/error");
            string errorCode = GameText.Format("login_errorCode", code);
            UtilTools.ErrorMessageDialog(content + errorCode);
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 保存登录过的所有玩家账号
    /// </summary>
    /// <param name="userName"></param>
    public void SaveAllUserName(string userName,string password="")
    {
        if (string.IsNullOrEmpty(userName))
            return;
        string allUserName = PlayerPrefs.GetString("allUser", "");
        JSONObject userList = new JSONObject(allUserName);

        if (string.IsNullOrEmpty(allUserName))
        {
            //服务器id
            userList.AddField(userName, password);
        }
        else
        {
            /*/游客不加入所有玩家账号列表中
            if(string.IsNullOrEmpty(userPassword))
            {
                return;
            }*/
            int haveSaveIndex = -1;
            for (int i = 0; i < userList.Count; i++)
            {
                if (userList.keys[i].Equals(userName))
                {
                    haveSaveIndex = i;
                }
            }
            if (haveSaveIndex >= 0)
            {
                userList.RemoveField(userName);
                userList.AddField(userName, password);
            }
            else
            {
                userList.AddField(userName, password);
            }/*
            bool isLogined = false;
            for (int i = 0; i < userList[0].Count; i++)
            {
                if (userList[0].list[i].str == userName){
                    haveSaveIndex = i;
                    isLogined = true;
                }
            }
            if (isLogined){
                userList[0].list.RemoveAt(haveSaveIndex);
                userList[0].Add(userName);
            }
            else
//            if (!isLogined)
            {
                userList[0].Add(userName);
            }*/
        }

        PlayerPrefs.SetString("allUser", userList.ToString());
       
    }

    public string GetAllUserName()
    {
        return PlayerPrefs.GetString("allUser", "");
    }

    public bool IsAutoLogin
    {
        set { _isLoginFirst = value; }
        get { return _isLoginFirst; }
    }

    public bool IsShowChangeUserWin
    {
        set { _isShowChangeUserWin = value; }
        get { return _isShowChangeUserWin; }
    }

    public bool IsLoginSuccess
    {
        set { _isLoginSuccess = value; }
        get { return _isLoginSuccess; }
    }
    
    /// <summary>
    /// 账号服务器返回的账号验证码
    /// </summary>
    public string accountServerVerificationMD5()
    {
        return _accountServerRlt.Content;
    }
    public string GetOpenId()
    {
        return PlayerPrefs.GetString("accountServerLoginContent", "null");
    }
    /// <summary>
    /// 获取本地的uuid
    /// </summary>
    /// <returns></returns>
    public string GetFastLoginUUID()
    {
        string _uuid = PlayerPrefs.GetString("FastLoginUUID", "");
        if (string.IsNullOrEmpty(_uuid))
        {
            //_uuid = System.Guid.NewGuid().ToString();
            JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
            if (tools != null)
            {
                _uuid = tools.GetDeviceUUID(); //"fdsfd2";//
                _uuid = _uuid.Replace("-", "");
            }
            PlayerPrefs.SetString("FastLoginUUID", _uuid);
        }
        return _uuid;
    }

    //获取uuid的验证码
    public string GetFastLoginKey()
    {
        string uuid = GetFastLoginUUID();
        uuid = uuid + "X-zciS6y(*W+ww,k";
        return UtilTools.GetStringMD5(uuid, Encoding.UTF8);
    }

    public string GetTruisKey(string keyBase)
    {
        string uuid = keyBase + "X-zciS6y(*W+ww,k";
        return UtilTools.GetStringMD5(uuid, Encoding.UTF8);
    }

    // 清空数据
    public void ClearData()
    {
        //serverDict.Clear();
    }

    //手机登录成功时保存用户信息
    public void SavePhoneLoginInfo()
    {
        GameDataMgr.PLAYER_DATA.IsTouris = false;
        lastLoginAccount = nowLoginAccount;
        lastLoginPassword = nowLoginPassword;
        PlayerPrefs.SetString("lastLoginAccount", lastLoginAccount);
        PlayerPrefs.SetString("lastLoginPassword", lastLoginPassword);
        SaveAllUserName(lastLoginAccount, lastLoginPassword);
        PlayerPrefs.SetString("lastLoginDeviceUUID", "");
        lastLoginDeviceUUID = "";
    }

    //快速登录成功时保存用户信息
    public void SaveFastLoginInfo()
    {
        lastLoginDeviceUUID = GetFastLoginUUID();
        PlayerPrefs.SetString("lastLoginDeviceUUID", lastLoginDeviceUUID);
    }
}
