/***************************************************************


 *
 *
 * Filename:  	IPath.cs	
 * Summary: 	路径管理基础接口
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/06/03 20:31
 ***************************************************************/

#region Using
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using version;
using Mono.Xml;
using System.Security;
#endregion


namespace customerPath
{

    /// <summary>
    /// 资源位置类型枚举
    /// </summary>
    public enum EAssetPathType
    {
        eNone,
        eResources,
        eStreamingAssets,
        ePersistent,

    };

    /// <summary>
    /// 路径抽象接口
    /// </summary>
    public abstract class IPath
    {
        #region DATA
        
        /// <summary>
        /// 资源记录(string, EAssetPath)
        /// </summary>
        protected Dictionary<string, EAssetPathType> _dictAssetsPathRecordBuff = new Dictionary<string, EAssetPathType>();
        protected static string _persistentDataPath = "";
        protected static string _persistentPathPlatform = "";
        protected static string _streamingAssetsPath = "";
        protected static string _streamingAssetsPathPlatform = "";
        #endregion //DATA

        /// <summary>
        /// 构造函数
        /// </summary>
        public IPath()
        {
            //检测版本资源路径，没有则创建
            string strVersionDir = persistentDataPath();
            if (!Directory.Exists(strVersionDir))
            {
                Directory.CreateDirectory(strVersionDir);
                Utils.LogSys.Log(strVersionDir);
            }
        }

        //加载包内资源目录
        public void LoadAssetsPathsInfo()
        {
            TextAsset text_asset = Resources.Load("AssetsPathsInfo") as TextAsset;
            if (text_asset != null && text_asset.text != null && text_asset.text.Length > 0)
            {
                SecurityParser SP = new SecurityParser();
                SP.LoadXml(text_asset.text);
                SecurityElement SE = SP.ToXml();
                foreach (SecurityElement element in SE.Children)
                {
                    string strAssetPath = element.Attribute("name").Replace("\\", "/");
                    string strType = element.Attribute("type");
                    switch (strType)
                    {
                        case "STREAMINGASSETS":
                            updateAssetRecordBuff(strAssetPath, EAssetPathType.eStreamingAssets);
                            break;
                        case "RESOURCES":
                        default:
                            updateAssetRecordBuff(strAssetPath, EAssetPathType.eResources);
                            break;
                    }
                }
            }
        }

        //加载内更资源目录
        public void LoadPersistentPathInfo()
        {
            //查找最新的AssetsPathsInfo.xml
            string text = "";
            string strPersistent = persistentDataPath() + "/AssetsPersistentPathsInfo.xml";
            strPersistent = strPersistent.Replace("\\", "/");
            if (File.Exists(strPersistent))//是否内更目录下有
            {
                WWW loader;
                RuntimePlatform rp = Application.platform;
                if (rp == RuntimePlatform.WindowsEditor)
                {
                    loader = new WWW("file:///" + strPersistent);
                }
                else
                {
                    loader = new WWW("file://" + strPersistent);//OSXEditor || IPhonePlayer || Android
                }
                while (!loader.isDone) { }
                Utils.LogSys.Log("LoadPersistentPathInfo Step 4");
                if (string.IsNullOrEmpty(loader.error))
                {
                    text = loader.text;
                }

            }

            if (text != null && text.Length > 0)
            {
                Utils.LogSys.Log("LoadPersistentPathInfo Step 6");
                SecurityParser SP = new SecurityParser();
                SP.LoadXml(text);
                Utils.LogSys.Log("LoadPersistentPathInfo Step 7");
                SecurityElement SE = SP.ToXml();
                Utils.LogSys.Log("LoadPersistentPathInfo Step 8");
                foreach (SecurityElement element in SE.Children)
                {
                    string strAssetPath = element.Attribute("name").Replace("\\", "/");
                    string strType = element.Attribute("type");
                    switch (strType)
                    {
                        case "PERSISTENT":
                            updateAssetRecordBuff(strAssetPath, EAssetPathType.ePersistent);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 更新资源路径信息
        /// </summary>
        protected void updateAssetRecordBuff(string strAssetPath, EAssetPathType eAstPath)
        {
            _dictAssetsPathRecordBuff[strAssetPath.ToLower()] = eAstPath;
        }

        /// <summary>
        /// 获取资源路径类型
        /// </summary>
        public EAssetPathType getAssetPathType(string strAssetPath)
        {
            strAssetPath = strAssetPath.ToLower();
            if (_dictAssetsPathRecordBuff.ContainsKey(strAssetPath))
            {
                return _dictAssetsPathRecordBuff[strAssetPath];
            }

            return EAssetPathType.eNone;
        }

        public System.Type getAssetType(string strAssetPath)
        {
            strAssetPath = strAssetPath.ToLower();
            string[] arr = strAssetPath.Split('.');
            if (arr == null || arr.Length == 0)
                return null;

            string sExtension = arr[arr.Length - 1];
            switch(sExtension)
            {
                case "png":
                    return typeof(Texture2D);
                case "mat":
                    return typeof(Material);
                case "prefab":
                    return typeof(GameObject);
            }
            return null;
        }

        /// <summary>
        /// Resources路径
        /// </summary>
        protected string resourcesPath()
        {
            return "";
        }

        /// <summary>
        /// StreamAssets路径
        /// </summary>
        public string streamingAssetsPath()
        {
            if (_streamingAssetsPath.Length == 0 )
            {
                _streamingAssetsPath = Application.streamingAssetsPath;
                _streamingAssetsPath = _streamingAssetsPath.Replace("\\", "/");
            }
            return _streamingAssetsPath;
        }

        /// <summary>
        /// StreamAssets路径+当前平台名
        /// </summary>
        public static string streamingAssetsPathPlatform()
        {
            if ( _streamingAssetsPathPlatform.Length == 0)
            { 
                _streamingAssetsPathPlatform = Application.streamingAssetsPath + "/" + getPlatformName();
                _streamingAssetsPathPlatform = _streamingAssetsPathPlatform.Replace("\\", "/");
            }
            return _streamingAssetsPathPlatform;
        }


        /// <summary>
        /// persistenData路径
        /// </summary>
        public string persistentDataPath()
        {
            if (_persistentDataPath.Length == 0)
            {
                _persistentDataPath = Application.persistentDataPath + "/" + ClientDefine.LOCAL_PROGRAM_VERSION;
                _persistentDataPath = _persistentDataPath.Replace("\\", "/");
            }
            return _persistentDataPath;
        }

        /// <summary>
        /// persistent路径+当前平台名
        /// </summary>
        public static string persistentDataPathPlatform()
        {
            if (_persistentPathPlatform.Length == 0)
            {
                _persistentPathPlatform = Application.persistentDataPath + "/" + getPlatformName();
                _persistentPathPlatform = _persistentPathPlatform.Replace("\\", "/");
            }
            return _persistentPathPlatform;
        }

        /// <summary>
        /// 获取指定文件的最新版本位置
        /// </summary>
        /// <param name="strFileName">相对于Resources的路径</param>
        /// <returns></returns>
        public abstract string getLatestVersionPath(string strFilePath);

        /// <summary>
        /// 生成指定文件路径的的URL地址
        /// </summary>
        /// <param name="strFilePath">必须支持相对于Resources的路径和全路径两种类型的路径</param>
        /// <returns></returns>
        public abstract string urlForWWW(string strFilePath);

        public static string getPlatformName()
        {
            
#if UNITY_ANDROID
            return "android";
#elif UNITY_IPHONE
            return "ios";
#elif UNITY_STANDALONE
           return "windows";
#endif
        }
    }
} 


