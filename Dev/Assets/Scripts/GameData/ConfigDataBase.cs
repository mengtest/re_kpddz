/***************************************************************


 *
 *
 * Filename:  	ConfigDataBase.cs	
 * Summary: 	配置数据基类
 *
 * Version:   	1.0.0
 * Author: 		XB.Wu
 * Date:   		2015/04/01 10:49
 ***************************************************************/


using System;
using MyExtensionMethod;
using Object = UnityEngine.Object;

#region Using

using System.Collections.Generic;
using System.Collections;
using Mono.Xml;
using System.Security;
using Utils;
using asset;
#endregion


#region Typedef
using NetObject = System.Object;
using task;
using UnityEngine;
#endregion


public class ConfigDataItemBase
{
    //key值
    protected string _key = default(string);

    //key-value键值对
    protected Dictionary<string, NetObject> _dictKVP = new Dictionary<string, NetObject>();

    //keys 缓存，用于快速返回
    protected List<string> _listKeys = new List<string>();

    ///////////////////////////////////////////////////////////////////
    /// <summary>
    /// 获取该item的key
    /// </summary>
    /// <returns>key</returns>
    public string GetKey()
    {
        return _key;
    }

    /// <summary>
    /// 获取keys
    /// </summary>
    public List<string> getKeys()
    {
        return _listKeys;
    }

    /// <summary>
    /// 索引器
    /// </summary>
    /// <returns>返回对应的object</returns>
    public NetObject this[string strIdx]
    {
        get {
            object value;
            return _dictKVP.TryGetValue(strIdx, out value) ? value : null;
        }

        //防止被外部修改
        protected set
        {
            if (!string.IsNullOrEmpty(strIdx))
            {
                //防止key重复添加
                if (!_dictKVP.ContainsKey(strIdx))
                {
                    _listKeys.Add(strIdx);
                }
                _dictKVP[strIdx] = value;
            }
        }
    }


    //虚接口，依赖不同配置生成不同解析方式
    public virtual void Parse(SecurityElement element)
    {

    }
}

public class ConfigDataCommon
{

    virtual public void LoadXML() {; }
    virtual public void ClearData() {; }
    virtual public void ReloadData() { ; }
    virtual public bool IsLoadedXML() { return false; }
}

public class ConfigDataBase<T> : ConfigDataCommon where T : ConfigDataItemBase, new()
{

    /// <summary>
    /// 配置文件夹
    /// </summary>
    protected string _configPath = "Config/";

    /// <summary>
    /// 配置文件名
    /// </summary>
    protected string _fileName;
    /// <summary>
    /// 配置文件名
    /// </summary>
    public string fileName { get { return _fileName; } }

    /// <summary>
    /// 全部数据
    /// </summary>
    public Dictionary<string, T> ConfigDataDic = new Dictionary<string, T>();

    private bool _bLoadedXML = false;
    public ConfigDataBase()
    {

    }

    public override bool IsLoadedXML()
    {
        return _bLoadedXML;
    }

    /// <summary>
    /// 载入xml配置
    /// </summary>
    public override void LoadXML()
    {
        string xmlPath = _configPath + _fileName;
//         Object assets = AssetManager.getInstance().loadXML(xmlPath);//同步加载XML
//         if (assets != null) {
//             ParseXML(assets.ToString());
//             LogSys.Log("load config success :" + xmlPath);
//         }
        //if (AssetManager.getInstance().IsFirstUseStreamingAssets)
        if (AssetManager.getInstance().IsStreamingAssets(xmlPath))
        {
            xmlPath = UtilTools.PathCheck(xmlPath);
            AssetBundleLoadTask task = new AssetBundleLoadTask(xmlPath, null);
            task.EventFinished += new task.TaskBase.FinishedHandler(delegate(bool manual, TaskBase currentTask)
            {
                TextAsset assetObj = ((AssetBundleLoadTask)currentTask).getTargetAsset() as TextAsset;
                if (assetObj != null)
                {
                    byte[] text_byte = assetObj.bytes;
                    if (UtilTools.ArrayHeadIsWoDong(assetObj.bytes))
                    {
                        CMyEncryptFile _encrypte = new CMyEncryptFile();
                        text_byte = _encrypte.Decrypt(assetObj.bytes, assetObj.bytes.Length);
                    }
                    ParseXML(text_byte);
                    //LogSys.Log("load config success :" + xmlPath);
                }
                else
                {
                    LogSys.LogError("load config failed:" + xmlPath);
                }
                _bLoadedXML = true;
            });
        }
        else
        {
            AssetLoadTask task = new AssetLoadTask(xmlPath, null);
            task.EventFinished += new task.TaskBase.FinishedHandler(delegate(bool manual, TaskBase currentTask)
            {
                TextAsset assetObj = ((AssetLoadTask)currentTask).getTargetAsset() as TextAsset;
                if (assetObj != null)
                {
                    byte[] text_byte = assetObj.bytes;
                    if (UtilTools.ArrayHeadIsWoDong(assetObj.bytes))
                    {
                        CMyEncryptFile _encrypte = new CMyEncryptFile();
                        text_byte = _encrypte.Decrypt(assetObj.bytes, assetObj.bytes.Length);
                    }
                    float start_time = Time.realtimeSinceStartup;
                    ParseXML(text_byte);
                    //Utils.LogSys.Log(string.Format("{0:0.00}", Time.realtimeSinceStartup - start_time) + " parse :" + currentTask._taskName);
                    //LogSys.Log("load config success :" + xmlPath);

                }
                else
                {
                    LogSys.LogError("load config failed:" + xmlPath);
                }
                _bLoadedXML = true;
            });
        }
    }


    /// <summary>
    /// 解析xml数据
    /// </summary>
    /// <param name="Config">xml文件字符串</param>
    public void ParseXML(byte[] ConfigByte)
    {
        string Config = System.Text.Encoding.UTF8.GetString(ConfigByte);
        SecurityParser SP = new SecurityParser();
        SecurityElement SE = null;
        try {
            SP.LoadXml(Config);
            SE = SP.ToXml();
        }
        catch (System.Exception ex) {
            LogSys.LogError(ex.Message + " " + _fileName + " 加载配置文件错误！！！！！！！！！！！！！！！！！");
            return;
        }

        foreach (SecurityElement child in SE.Children) {
            string itemKey = "";
            T item = new T();
            try {
                item.Parse(child);
                itemKey = item.GetKey();
                if (string.IsNullOrEmpty(item.GetKey()))
                {
                    LogSys.LogError("key is empty! file: " + _fileName);
                }
                ConfigDataDic.Add(item.GetKey(), item);
            }
            catch (Exception e) {

                LogSys.LogError(string.Format("配置表出错 fileName:[{0}]    key:[{1}]    error:{2}", _fileName, item.GetKey(), e.Message));
            }
        }
    }

    /// <summary>
    /// 读取文件成功
    /// </summary>
    /// <param name="assets">文件Object</param>
    /// <param name="path">文件有效路径</param>
    //     public void LoaderComplete(Object assets, string path) {
    //         ParseXML(assets.ToString());
    //         LogSys.Log("load config success :" + path);
    //     }

    /// <summary>
    /// 读取文件失败
    /// </summary>
    /// <param name="error">失败描述</param>
    /// <param name="path">文件有效路径</param>
    //     public void LoaderError(string error, string path) {
    //         LogSys.LogError("load config error : " + error + ",path is: " + path);
    //     }

    public override void ClearData()
    {
        ConfigDataDic.Clear();
    }

    /// <summary>
    /// 重新载入数据
    /// </summary>
    public override void ReloadData()
    {
        ClearData();
        LoadXML();
    }

    /// <summary>
    /// 根据key获取单挑数据
    /// </summary>
    /// <param name="key">key</param>
    /// <returns></returns>
    public T GetDataByKey(string key)
    {
        if (ConfigDataDic.ContainsKey(key)) {
            return ConfigDataDic[key];
        }
        LogSys.LogWarning(string.Format("表【{0}】未找到key【{1}】", fileName, key));
        return null;
        //return ConfigDataDic.GetValue(key, null);
    }

    public T GetDataByKey(int key)
    {
        return GetDataByKey(key.ToString());
    }

    public T GetDataByKey(uint key)
    {
        return GetDataByKey(key.ToString());
    }

    public T GetDataByKey(int key1, int key2)
    {
        return GetDataByKey(key1.ToString() + "," + key2.ToString());
    }

    public T GetDataByKey(uint key1, uint key2)
    {
        return GetDataByKey(key1.ToString() + "," + key2.ToString());
    }

    public T GetDataByKey(uint key1, int key2)
    {
        return GetDataByKey(key1.ToString() + "," + key2.ToString());
    }

    public T GetDataByKey(int key1, int key2, int key3)
    {
        return GetDataByKey(key1.ToString() + "," + key2.ToString() + "," + key3.ToString());
    }

    /// <summary>
    /// 获取全部数据
    /// </summary>
    /// <returns>全部数据 Dictionary</returns>
    public Dictionary<string, T> GetAllData()
    {
        return ConfigDataDic;
    }

    public T this[string strIdx] {
        get {
            return GetDataByKey(strIdx);
        }
    }

    public bool ContainsKey(string key)
    {
        return ConfigDataDic.ContainsKey(key);
    }

    public T GetDataByKey(uint id, uint phase, int star)
    {
        return GetDataByKey("" + id + "," + phase + "," + star);
    }
}
