using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Text;
using System.Xml.Linq;
using customerPath;


public class EngineManager
{
    private static EngineManager _instance;
    static public bool LoadWay = false;
    public string LocalResUrl;
    public string ClientResUrl;
    public string NewResVer;
    public string NewClientVer;

    public bool Logined;
    public bool Relogin = false;
    public bool HasAccount;
    public bool HasServerId;
    public Assembly Engine;
    public GameMessage Message;
    public string UpdateOnline = "http://192.168.4.166:1234/";
    public string RegistrationUrl = "http://192.168.4.166:9003/cgi-bin/account:register";

    public int SysType = 0; //系统类型

#if UNITY_ANDROID
    public AndroidJavaObject androidJO;
#endif
    public EngineManager()
    {
        if (_instance != null)
            throw new Exception("单件实例错误");
        _instance = this;
        Init();
    }

    private void Init()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer
            || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
        {
            ClientResUrl = LocalResUrl = "file:///" + Application.dataPath + "/StreamingAssets/";
            SysType = 0;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
#if UNITY_ANDROID
            ClientResUrl = IPath.streamingAssetsPathPlatform() + "/";
            LocalResUrl = "file:///" + Application.persistentDataPath + "/";
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            androidJO = jc.GetStatic<AndroidJavaObject>("currentActivity");
            SysType = 0;
#endif
        }

        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
			ClientResUrl ="file:///" + IPath.streamingAssetsPathPlatform() + "/";
			LocalResUrl = "file:///" + Application.persistentDataPath + "/";
            SysType = 2;
#endif
        }
        else if (Application.platform == RuntimePlatform.WP8Player)
        {
#if UNITY_WP8
            ClientResUrl = IPath.streamingAssetsPathPlatform() + "/";
            LocalResUrl = Application.persistentDataPath + "/";
            //ClientResUrl = "file:///" + Application.dataPath + "/StreamingAssets/";
            SysType = 3;
#endif
        }
        Message = new GameMessage();
        //BaseConfig.UpdateOnline = EngineUtils.Domain2ip(BaseConfig.UpdateOnline);
        //BaseConfig.GameConfigUrl = EngineUtils.Domain2ip(BaseConfig.GameConfigUrl);
    }

    public static EngineManager GetInstance()
    {
        if (_instance != null)
        {
            return _instance;
        }
        return new EngineManager();
    }

    public void AddComponentToGO(string name, GameObject go)
    {
        if (Engine == null)
        {
            //GameObject.AddComponent<T>()/GameObject.AddComponent(Type).'
            //UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(go, "Assets/Scripts/BaseCode/Engine/EngineManager.cs (99,13)", name);
            go.AddComponent(System.Type.GetType(name));
        }
        else
        {
            var mono = Engine.GetType(name);
            go.AddComponent(mono);
        }
    }

    public void InstallClient(string path)
    {
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //androidJO.Call("installApk", path);
#endif
        }
    }

    // 网络制式
    public string GetAPNType()
    {
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //return androidJO.Call<string>("getAPNType");
            return "";
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
            return ""; //IOSMessage.APNType;
#endif
        }
        return "";
    }

    private string _model = "";
    //机型
    public string GetModel()
    {
        if (_model != "") return _model;
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //_model = androidJO.Call<string>("getModel");
            _model = "";
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
            return _model; // IOSMessage.Model;
#endif
        }
        return _model;
    }

    public string GetMac()
    {
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //string mac = androidJO.Call<string>("getMac");
            //if (mac == string.Empty) return "0";
            //return mac;
            return "0";
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
            return ""; // IOSMessage.GetMacAddress();
#endif
        }
        else if (SysType == 3)
        {
            //#if UNITY_WP8
            //windows phone 暂时获取不到MAC地址
            return "";

        }
        return "";
    }

    public void InitSDK()
    {
        if (GetSDKType() == SDKType.No) return;
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //androidJO.Call("initGameInfo");
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
			//IOSSDKManager.InitSDK();
#endif
        }
    }

    public void LoginSDK()
    {
        if (GetSDKType() == SDKType.No) return;
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //androidJO.Call("login");
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
			//IOSSDKManager.CheckInit();
			//IOSSDKManager.Login();
#endif
        }
    }

    public void Logout()
    {
        if (GetSDKType() == SDKType.No) return;
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //androidJO.Call("logout");
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
			//IOSSDKManager.CheckInit();
			//IOSSDKManager.Logout();
#endif
        }
    }

    public void QuotaPay(params String[] args)
    {
        if (GetSDKType() == SDKType.No) return;
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //androidJO.Call("initQuotaPayView", args);
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
			//IOSSDKManager.QuotaPay(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9], args[10], args[11], args[12]);
#endif
        }
    }

    //0 支付完成 1 支付成功 2 支付失败 -1没有任何支付相关操作
    public int GetPayCode()
    {
        if (GetSDKType() == SDKType.No) return 0;
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //return androidJO.Call<int>("getPayCode");
            return 0;
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
            return 0; // IOSSDKManager.GetPayCode();
#endif
        }
        return 0;
    }

    public bool GetUserCenter()
    {
        if (GetSDKType() == SDKType.No) return false;
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //return androidJO.Call<bool>("getUserCenter");
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
            return false; // IOSSDKManager.HasUserCenter;
#endif
        }
        return false;
    }

    public void ShowUserCenter()
    {
        if (GetSDKType() == SDKType.No) return;
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //androidJO.Call("showUserCenter");
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
			//IOSSDKManager.CheckInit();
			//IOSSDKManager.ShowUserCenter();
#endif
        }
    }

    public string SDKState()
    {
        if (GetSDKType() == SDKType.No) return "";
        if (SysType == 1)
        {
#if UNITY_ANDROID
           //return androidJO.Call<string>("getState");
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
            return ""; // IOSSDKManager.SDKState();
#endif
        }
        return "";
    }

    public void SDKState(string state)
    {
        if (GetSDKType() == SDKType.No) return;
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //androidJO.Call("setState", state);
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
			//IOSSDKManager.SDKState(state);
#endif
        }
    }

    public string GetOpenId()
    {
        if (GetSDKType() == SDKType.No) return "";
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //return androidJO.Call<string>("getOpenId");
            return "";
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
            return ""; // IOSSDKManager.GetUid();
#endif
        }
        return "";
    }

    // 根据不同平台 发送Passwords
    public string[] GetPasswords()
    {
        if (GetSDKType() == SDKType.No) return new string[] { };
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //return new string[] { androidJO.Call<string>("getSession"), androidJO.Call<string>("getToken"), androidJO.Call<int>("getSDKId").ToString() };
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
            return new string[] { }; // IOSSDKManager.GetPasswords();
#endif
        }
        return new string[] { };
    }

    public int GetChannel()
    {
        if (GetSDKType() == SDKType.No) return 0;
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //return androidJO.Call<int>("getChannel");
#endif
        }
		else if (SysType == 2)
		{
#if UNITY_IOS
            return 0; // IOSSDKManager.GetChannel();
#endif
		}
        return 0;
    }
    public int GetSubChannel()
    {
        if (GetSDKType() == SDKType.No) return 0;
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //return androidJO.Call<int>("getSubChannel"); 
#endif
        }
		else if (SysType == 2)
		{
#if UNITY_IOS
            return 0; // IOSSDKManager.GetSubChannel();
#endif
		}
        return 0;
    }
    public int GetSDKId()
    {
        if (SysType == 0) 
        {
            return 1000001;
        }
        else if (SysType == 1)
        {
#if ANDROID_OUT
            return 1000002;
#elif ANDROID_IN
	        return 1000001;
#elif UNITY_ANDROID
            //return androidJO.Call<int>("getSDKId");
#endif
        }
		else if (SysType == 2)
		{
#if UNITY_IOS
            return 1010001; //IOSSDKManager.GetSDKId();
#endif
		}
        return 0;
    }

    public void SendLoginGame(params String[] args)
    {
        if (GetSDKType() == SDKType.No) return;
#if ANDROID_Complex
        return;
#endif
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //androidJO.Call("sendLoginGame", args);
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
			//IOSSDKManager.DataReport("1", args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
			//IOSSDKManager.CheckUncompletedTransaction();
#endif
        }
    }
    public void SendLevelUp(params String[] args)
    {
        if (GetSDKType() == SDKType.No) return;
#if ANDROID_Complex
        return;
#endif
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //androidJO.Call("sendLevelUp", args);
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
			//IOSSDKManager.DataReport("2", args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
#endif
        }
    }
    public void SendCreateRole(params String[] args)
    {
        if (GetSDKType() == SDKType.No) return;
#if ANDROID_Complex
        return;
#endif
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //androidJO.Call("sendCreateRole", args);
#endif
        }
        else if (SysType == 2)
        {
#if UNITY_IOS
			//IOSSDKManager.DataReport("3", args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
#endif
        }
    }
    //查询防沉迷
    public void DoAntiAddictionQuery()
    {
        if (GetSDKType() == SDKType.No) return;
#if ANDROID_Complex
        return;
#endif
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //androidJO.Call("doAntiAddictionQuery");
#endif
        }
    }

    public void Quit()
    {
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //androidJO.Call("onBackPressed");
#endif
        }
    }

    public void QuitSDK()
    {
#if ANDROID_Complex
        return;
#endif
        if (GetSDKType() == SDKType.No) return;
        if (SysType == 1)
        {
#if UNITY_ANDROID
            //androidJO.Call("UMIDestroy");
#endif
        }
    }

    private SDKType GetSDKType()
    {
        int id = GetSDKId();
        if (id < 0)
            return SDKType.Custom;
        else if (id <= 100000)
            return SDKType.Seed;
        else
            return SDKType.No;
    }

    //暂时使用
    public bool GetHasSDK()
    {
        return GetSDKId() <= 1000000;
    }
}

public enum SDKType
{
    No = 0,
    Seed = 1,
    Custom = 2,
}

#if !UNITY_WP8


#region XmlDocument等类型在.net Framework 4.5之后不支持 推荐使用XDocument
public class GameMessage
{
    private XmlDocument doc = new XmlDocument();


    public XmlDocument Doc
    {
        get { return doc; }
    }

    public bool HasInitialize;
    public void ReadXMLConfig(string text)
    {
        HasInitialize = true;
        doc.RemoveAll();
        doc.LoadXml(text);
    }

    public string GetAttribute(string name)
    {
        XmlElement root = doc.DocumentElement;
        if (root == null || root.Name != "root")
        {
            root = doc.CreateElement("root");
            doc.AppendChild(root);
        }

        return root.GetAttribute(name);
    }

    public void SetAttribute(string key,string value)
    {
        XmlElement root = doc.DocumentElement;
        if (root == null || root.Name != "root")
        {
            root = doc.CreateElement("root");
            doc.AppendChild(root);
        }
        XmlAttribute Attribute = doc.CreateAttribute(key);
        Attribute.Value = value;
        root.Attributes.Append(Attribute);
    }

    public void Save()
    {
        string path = EngineManager.GetInstance().LocalResUrl;
        string savePath = path.Replace("file:///", "");
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        FileInfo t = new FileInfo(path.Replace("file:///", "") + "GameConfig.xml");
        byte[] data = TextUtils.GetBytes(doc.InnerXml);
        Stream sw = t.Create();
        sw.Write(data, 0, data.Length);
        sw.Close();
        sw.Dispose();
    }
}
#endregion
#else
public class GameMessage
{
    XDocument doc = new XDocument();


    public XDocument Doc
    {
        get { return doc; }
    }

    public bool HasInitialize;
	public void ReadXMLConfig(string text)
    {
        HasInitialize = true;
        doc.RemoveNodes();
        if (text.Length > 0)
            doc = XDocument.Parse(text);
    }

    public string GetAttribute(string name)
    {
        XElement root = doc.Root;
        if (root == null || root.Name != "root")
        {
            root = new XElement("root");
            doc.Add(root);
        }

        if (root.Attribute(name) is XAttribute)
            return root.Attribute(name).Value;
        else
            return "";
    }

    public void SetAttribute(string key, string value)
    {
        XElement root = doc.Root;
        if (root == null || root.Name != "root")
        {
            root = new XElement("root");
            doc.Add(root);
        }

        root.SetAttributeValue(key, value);
    }

    public void Save()
    {
        string path = EngineManager.GetInstance().LocalResUrl;
        string savePath = path.Replace("file:///", "");
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        FileInfo t = new FileInfo(path.Replace("file:///", "") + "GameConfig.xml");
        byte[] data = TextUtils.GetBytes(doc.ToString());
        Stream sw = t.Create();
        sw.Write(data, 0, data.Length);
        sw.Close();
        sw.Dispose();
    }

}

#endif