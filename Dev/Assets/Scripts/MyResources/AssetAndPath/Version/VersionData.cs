/***************************************************************


 *
 *
 * Filename:  	VersionData.cs
 * Summary: 	版本管理数据
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/06/03 22:13
 ***************************************************************/

#region Using
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using asset;
using task;
using sdk;
#endregion //Using

namespace version
{
    public struct VersionNode//更新列表节点
    {
        public string _strVersion;	//版本号
        public float _fSize;	//更新包大小 
    }
    public struct FileMD5Node//要下载的文件配置
    {
        public string _path;	//版本号
        public string _md5;	//md5码
    }

    /// <summary>
    /// 版本数据部分
    /// </summary>
    [SLua.CustomLuaClass]
    public class VersionData
    {
        /// <summary>
        /// 本地程序版本
        /// </summary>
        //public const string LOCAL_PROGRAM_VERSION = "1.1.7";
		private string _strVersionXMLUrl = "";//所有资源版本列表
        private string _strDownloadUrl = "";
        public static bool _versionChecked = false;//用于判断是否要开启版本检测的标志
        public static string _strShowVersion = "1.0.0";//用于显示的版本号, 会从ftp上下载;
        public static string _strReViewingVersion = "0.0.0";//正在审核的版本;
        public static bool HealthTips = true; //是否弹出健康游戏忠告
        public static bool ShowTipWhenInToGame = false;//点"进入游戏"时,提示有新版本,请前往更新.
        public static string _strPackageSize = "50.0";
        public static string _toLoadProgramVersion = "1.0.1";
        string _sCurVersion = "";
        public enum VersionType
        {
            Checking,//下载版本xml中
            None,//无需更新
            Program,//程序大版本更新
            Inner,//资源内更
            Error,//出错，可通过ErrorCode查具体原因
        }
        VersionType _versionType = VersionType.Checking;
        string loadNewVersionPath = "";//跳转网页
        string loadPackagePath = "";//内部下载apk安装包地址
        public VersionType VersionTypeToUpdate
        { get { return _versionType; } }
        public string LoadNewVersionPath
        { get { return loadNewVersionPath; } }
        public string LoadPackagePath
        { get { return loadPackagePath; } }
        public enum ErrorCode
        {
            None = 1000,
            DownLoadVersionXMLFailed,//下载版本信息xml失败
            ParseVersionXMLFailed,//解释版本信息xml失败
            DownLoadFileListXMLFailed,//下载某个版本的文件列表失败
            ParseFileListXMLFailed,//解释某个版本的文件列表失败
            NotFoundVersion,//按列表下载资源时，没有找到对应的版本号
            DownLoadFileFailed,//下载某个文件失败
            DownLoadFileMD5Error,//下载某个文件失败
            CreateFileFailed,//创建某个文件失败
            WriteFileFailed,//保存某个文件失败
            RenameFileFailed,//重命名某个文件失败
        }
        ErrorCode _errorCode = ErrorCode.None;
        public ErrorCode ErrorCodeData
        { get { return _errorCode; } }

        /// <summary>
        /// 所有的资源版本列表
        /// </summary>
        List<VersionNode> _listVersionList = new List<VersionNode>();

        /// <summary>
        /// 要更新的资源版本列表
        /// </summary>
        List<VersionNode> _waitingList = new List<VersionNode>();
        int curWaiting = 0;

        /// <summary>
        /// 要下载的资源文件列表和MD5码
        /// </summary>
        List<FileMD5Node> _downloadFilesList = new List<FileMD5Node>();
        int _downloadIndex = 0;//下个要下载的index(可能是分割线version:xxxx)
        int _totalDownloadFile = 0;//要下载的总文件数
        int _loadedCount = 0;//已下载的总文件数
        LoadFileList _loadVersionFiles;
        bool _isAllFileListReady = false;
        bool _isSureToUpdateVersion = false;
        static string _defaultSourceVersion = "1000";
        static string _defaultProgressVersion = "1.0.0";

        public VersionData()
        {
            //SaveLocalVersion("1000");//测试用,还原为版本号1000
            Init();
            DownloadVersionsXML();
        }

        public void Init()
        {
            _strVersionXMLUrl = sdk.SDKManager.VersionXMLUrl + "?p=" + UtilTools.GetClientTime();
            _strDownloadUrl = sdk.SDKManager.VersionDownloadUrl + ClientDefine.LOCAL_PROGRAM_VERSION + "/";
        }

        /// <summary>
        /// 将当前版本号存在本地
        /// </summary>
        public static void SaveProgressVersion(string sVersion)
        {
            if (sVersion.Equals(GeProgressVersion()))
            {
                return;
            }
            SaveLocalVersion(_defaultSourceVersion);//内更版本号要清除
            PlayerPrefs.SetString("ProgressVersion", sVersion);
        }

        /// <summary>
        /// 取本地版本号
        /// </summary>
        public static string GeProgressVersion()
        {
            return PlayerPrefs.GetString("ProgressVersion", _defaultProgressVersion);
        }
        /// <summary>
        /// 将当前版本号存在本地
        /// </summary>
        public static void SaveLocalVersion(string sVersion)
        {
            PlayerPrefs.SetString("SourceVersion", sVersion);
        }

        /// <summary>
        /// 取本地版本号
        /// </summary>
        public static string GetLocalVersion()
        {
            string curVersion = PlayerPrefs.GetString("SourceVersion", _defaultSourceVersion);
            if (CompareVersion(curVersion, ClientDefine.LOCAL_SOURCE_VERSION) == -1)
                return ClientDefine.LOCAL_SOURCE_VERSION;

            return curVersion;
        }

        /// <summary>
        /// 比较版本号：0一样  1前者大 -1后者大
        /// </summary>
        /// <param name="sVersion1"></param>
        /// <param name="sVersion2"></param>
        /// <returns></returns>
        private static int CompareVersion(string sVersionSrc, string sVersionDes)
        {
            int iSrc = 0;
            int iDes = 0;
            int.TryParse(sVersionSrc, out iSrc);
            int.TryParse(sVersionDes, out iDes);
            if (iSrc < iDes)
            {
                return -1;
            }
            else if (iSrc > iDes)
            {
                return 1;
            }
            return 0;
        }

        private static int CompareProgramVersion(string sVersion1, string sVersion2)
        {
            string[] version1 = sVersion1.Split( new char[] { '.' });
            string[] version2 = sVersion2.Split(new char[] { '.' });
            
            int iv1_1 = 0;
            int iv1_2 = 0;
            int iv1_3 = 0;

            int iv2_1 = 0;
            int iv2_2 = 0;
            int iv2_3 = 0;
            int.TryParse(version1[0], out iv1_1);
            int.TryParse(version2[0], out iv2_1);
            if (iv1_1 < iv2_1)
            {
                return -1;
            }
            else if (iv1_1 > iv2_1)
            {
                return 1;
            }

            int.TryParse(version1[1], out iv1_2);
            int.TryParse(version2[1], out iv2_2);
            if (iv1_2 < iv2_2)
            {
                return -1;
            }
            else if (iv1_2 > iv2_2)
            {
                return 1;
            }

            int.TryParse(version1[2], out iv1_3);
            int.TryParse(version2[2], out iv2_3);
            if (iv1_3 < iv2_3)
            {
                return -1;
            }
            else if (iv1_3 > iv2_3)
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// 第1步：下载版本列表xml
        /// </summary>
        private void DownloadVersionsXML()
        {
            WWWLoadTask task = new WWWLoadTask("", _strVersionXMLUrl);
            task.EventFinished += new task.TaskBase.FinishedHandler(delegate(bool manual, TaskBase currentTask)
            {
                if (manual)
                    return;

                WWW _download = currentTask.GetWWW();
                if (_download == null || _download.text.Length == 0)
                {
                    _versionType = VersionType.Error;
                    _errorCode = ErrorCode.DownLoadVersionXMLFailed;
                    return;//下载版本信息文件失败
                }

                //解释版本列表xml。
                ParseVersionsXML(_download.text);
                CheckVersionsToUpdate();
            });
        }

        /// <summary>
        /// 第2步：判断是否要更新程序版本
        /// </summary>
        private void ParseVersionsXML(string xmlFile)
        {
            XDocument docTemp = XDocument.Parse(xmlFile);

            if (docTemp == null)
            {
                _versionType = VersionType.Error;
                _errorCode = ErrorCode.ParseVersionXMLFailed;
                return;//下载版本信息文件失败
            }

            //取新版本下载地址
            string cur_platform_type = "0";
            if (ClientDefine.THIRD_PARTY_SDK)
            {
                if (SDKManager.CurSDK == "version_war3g_android" 
                    || SDKManager.CurSDK == "version_war3g_ios_quick" 
                    || SDKManager.CurSDK == "version_war3g_android_cps"
                    || SDKManager.CurSDK == "version_war3g_android_ylYueDongJuHe")
                {
                    cur_platform_type = SDKManager.getInstance().CommonSDKInterface.channelType();
                }
            }
            foreach (XElement https_item in docTemp.Root.Descendants("newVersionHttp"))
            {
                string platform_type = https_item.Attribute("typeID").Value;
                if (!string.IsNullOrEmpty(platform_type) && cur_platform_type == platform_type)
                {
                    loadNewVersionPath = https_item.Attribute("http").Value;
                }
                if (https_item.Attribute("package") != null && !string.IsNullOrEmpty(https_item.Attribute("package").Value))
                {
                    loadPackagePath = https_item.Attribute("package").Value;
                }
                
            }
            
            //取版本列表
            foreach (XElement item in docTemp.Root.Descendants("programVersion"))
            {
                string programVersion = item.Attribute("version").Value;
                _strShowVersion = item.Attribute("showVersion").Value;
                if (item.Attribute("reviewingVersion") != null && !string.IsNullOrEmpty(item.Attribute("reviewingVersion").Value))
                {
                    _strReViewingVersion = item.Attribute("reviewingVersion").Value;
                    if (IsReviewingVersion())
                    {
                        ClientDefine._NoVip = true;//appStore审核关闭vip功能
                    }
                }
                if (item.Attribute("size") != null && !string.IsNullOrEmpty(item.Attribute("size").Value))
                {
                    _strPackageSize = item.Attribute("size").Value;
                }
				Debug.LogError ("ClientDefine.LOCAL_PROGRAM_VERSION" + ClientDefine.LOCAL_PROGRAM_VERSION + ",programVersion=" + programVersion);
                if (CompareProgramVersion(ClientDefine.LOCAL_PROGRAM_VERSION, programVersion) == -1)//
                {
                    _toLoadProgramVersion = programVersion;
                    _versionType = VersionType.Program;//需要更新程序版本
                    return;
                }
                SaveProgressVersion(programVersion);
                _listVersionList.Clear();
                if (CompareProgramVersion(ClientDefine.LOCAL_PROGRAM_VERSION, programVersion) == 0)
                {
                    foreach (XElement param in item.Descendants("sourceVersion"))
                    {
                        VersionNode nodeData = new VersionNode();
                        if (param.Attribute("version") != null)
                        {
                            nodeData._strVersion = param.Attribute("version").Value;
                        }
                        if (param.Attribute("size") != null)
                        {
                            float.TryParse(param.Attribute("size").Value, out nodeData._fSize);
                        }
                        _listVersionList.Add(nodeData);
                    }
                }
            }
        }

        /// <summary>
        /// 第3步：判断是否要更新资源
        /// </summary>
        private void CheckVersionsToUpdate()
        {
            if (_versionType == VersionType.Program)
            {
                return;
            }

            if (_listVersionList.Count == 0)
            {
                _versionType = VersionType.None;//无需要更新版本。
                SaveLocalVersion(_defaultSourceVersion);//设为默认版本号1000
                return;
            }

            _waitingList.Clear();
            string curVersion = GetLocalVersion();
            for (int i = 0; i < _listVersionList.Count; i++)
            {
                string tempVersion = _listVersionList[i]._strVersion;
                if (CompareVersion(curVersion, tempVersion) == -1)
                    _waitingList.Add(_listVersionList[i]);
            }

            if (_waitingList.Count == 0)
            {
                _versionType = VersionType.None;//无需要更新版本。
                return;
            }
            _versionType = VersionType.Inner;//资源更新版本。
            _downloadFilesList.Clear();
            curWaiting = 0;
            _totalDownloadFile = 0;
            CheckFilesToUpdate();
        }

        /// <summary>
        /// 第4步：过滤出要更新的所有文件
        /// </summary>
        private void CheckFilesToUpdate()
        {
            if (curWaiting >= _waitingList.Count)//完成
            {
                _isAllFileListReady = true;
                if (_isSureToUpdateVersion)
                    DownloadVersion();
                return;
            }
            string sVersion = _waitingList[curWaiting]._strVersion;
            string xmlUrl = _strDownloadUrl + sVersion + "/filespath.xml";
            WWWLoadTask task = new WWWLoadTask("",xmlUrl);//下载该版本的文件列表xml
            task.EventFinished += new task.TaskBase.FinishedHandler(delegate(bool manual, TaskBase currentTask)
            {
                if (manual)
                    return;
                
                WWW _download = currentTask.GetWWW();
                if ( _download == null || _download.text.Length == 0)
                {
                    _versionType = VersionType.Error;
                    _errorCode = ErrorCode.DownLoadFileListXMLFailed;
                    return;//下载版本信息文件失败
                }
                RecordFilesNeedToDownload(_download.text, sVersion);
                curWaiting++;
                CheckFilesToUpdate();//继续下个版本
                
            });
        }

        private void RecordFilesNeedToDownload(string xmlFile, string sVersion)
        {
            XDocument docTemp = null;
            try
            {
                docTemp = XDocument.Parse(xmlFile);
            }
            catch (System.Exception ex)
            {
                _versionType = VersionType.Error;
                _errorCode = ErrorCode.ParseFileListXMLFailed;
                Debug.LogException(ex);
                return;//下载版本信息文件失败
            }

            if (docTemp == null)
            {
                _versionType = VersionType.Error;
                _errorCode = ErrorCode.ParseFileListXMLFailed;
                return;//下载版本信息文件失败
            }

            FileMD5Node nodeVersion = new FileMD5Node();
            nodeVersion._path = "version";
            nodeVersion._md5 = sVersion;
            _downloadFilesList.Add(nodeVersion);//version标记：版本分割线
            //收集需要下载的文件
            foreach (XElement item in docTemp.Root.Descendants("File"))
            {
                FileMD5Node nodeFile = new FileMD5Node();
                nodeFile._path = item.Attribute("name").Value;
                nodeFile._md5 = item.Attribute("md5").Value;

                RemoveFileFromListByPath(nodeFile._path);
                _downloadFilesList.Add(nodeFile);
                _totalDownloadFile++;//要下载的总文件数
            }
        }

        private void RemoveFileFromListByPath(string filePath)
        {
            int count = _downloadFilesList.Count;
            FileMD5Node tempNode;
            for (int i = 0; i < count; i++ )
            {
                tempNode = _downloadFilesList[i];
                if (tempNode._path == filePath)
                {
                    _downloadFilesList.Remove(tempNode);
                    return;
                }
            }
        }

        /// <summary>
        /// 第5步：递归下载每个版本的所有文件
        /// </summary>
        private void DownloadVersion()
        {
            List<FileMD5Node> fileList = new List<FileMD5Node>();
            
            bool bIsCurVersion = false;
            int listLength = _downloadFilesList.Count;
            int startIndex = _downloadIndex;
            FileMD5Node fileNode;
            for(int i=startIndex; i<listLength; i++)
            {
                _downloadIndex = i;
                fileNode = _downloadFilesList[i];
                if (fileNode._path == "version")//版本分割线
                {
                    if (bIsCurVersion)//当前版本的文件已过滤完成
                    {
                        break;
                    }
                    bIsCurVersion = true;
                    _sCurVersion = fileNode._md5;
                }
                else//要下载的文件
                {
                    fileList.Add(fileNode);
                }
            }
            if (bIsCurVersion == false)//按列表下载资源时，没有找到对应的版本号
            {
                _versionType = VersionType.Error;
                _errorCode = ErrorCode.NotFoundVersion;
                return;
            }
            if (fileList.Count == 0)//当前版本的文件都无需下载的情况
            {
                downloadVersionCallback(_sCurVersion);
                return;
            }

            _loadVersionFiles = new LoadFileList(fileList, _sCurVersion, _strDownloadUrl);
            _loadVersionFiles.EventFinished += new LoadFileList.LoadFinishedHandler(delegate(string sVersion, int loadFileCount)
            {
                _loadedCount = _loadedCount + loadFileCount;//已下载的总文件数
                downloadVersionCallback(sVersion);
            });
            _loadVersionFiles.EventError += new LoadFileList.LoadErrorHandler(delegate(ErrorCode errorCode)
            {
                _versionType = VersionType.Error;
                _errorCode = errorCode;
                return;//失败
            });
        }

        // 下载完指定版本的所有文件时的回调
        private void downloadVersionCallback(string sVersion)
        {
            _loadVersionFiles = null;
            int listLength = _downloadFilesList.Count;
            SaveLocalVersion(sVersion);
            if (_downloadIndex < listLength - 1)
            {
                DownloadVersion();
            }
            else//已全部下载完成
            {
                _versionType = VersionType.None;
            }
        }


        public VersionType GetUpdateVersionType()
        {
            return _versionType;
        }
        public float GetUpdateVersionSize()
        {
            float totalSize = 0f;
            for (int i = 0; i < _waitingList.Count; i++ )
            {
                totalSize += _waitingList[i]._fSize;
            }
            return totalSize;
        }
        public void StartUpdateVersion()
        {
            _isSureToUpdateVersion = true;
            if (_isAllFileListReady)
                DownloadVersion();
        }
        public float GetUpdatePercent()
        {
            int loadedFilesCount = _loadedCount;
            if (_loadVersionFiles != null)
            {
                loadedFilesCount += _loadVersionFiles.LoadedCount;
            }
            Debug.LogError("per=" + (float)loadedFilesCount / (float)_totalDownloadFile);
            return (float)loadedFilesCount/(float)_totalDownloadFile ;
        }
        public string GetDownloadingVersion()
        {
            return _sCurVersion;
        }

        //是否正要审核的版本(appStore)
        public static bool IsReviewingVersion()
        {
            if (CompareProgramVersion(ClientDefine.LOCAL_PROGRAM_VERSION, _strReViewingVersion) == 0)
                return true;

            return false;
        }

        // 是否是苹果官方版本 (导出给lua使用，C#请使用: SDKManager.isAppStoreVersion())
        public static bool isAppStoreVersion()
        {
            return SDKManager.isAppStoreVersion();
        }
    }
}


