/***************************************************************


 *
 *
 * Filename:  	PackageSourceVersion.cs	
 * Summary: 	资源版本打包工具
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/07/11 15:33
 ***************************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml.Linq;
using EventManager;
using task;
using System.IO;
using customerPath;
using sdk;
using version;
using System.Collections.Generic;

public class PakageSourceVersion: EditorWindow {

    //************************************************************************************************
    //窗口显示部分
    //************************************************************************************************
    //输入文字的内容
    private string _curPlatform = "";
    private string _curVersion = "";
    public static string _lastVersion = "";
    private string _sProgramVersion = "";
    private string _sResultDirName = "";
    private string _savedPath = "";//版本包的保存路径
    private string _versionXMLPath = "";//version.xml和保存路径
    private string _outSidePath = "";//上个版本的资源地址
    private string _curVersionBakePath = "";//当前版本的资源将备份到该地址
    private string _log = "";
    private string _error = "";
    private bool _bIniting = false;//正在查询要生成的版本号
    private bool _bSaving = false;//正在生成
#if UNITY_IOS
    const string _HardDisk = "/Users/wodong/Documents";
#else
    const string _HardDisk = "f:";//G盘
#endif
    double packageSize = 0;//KB
    WWW _downloadTask = null;
    XDocument docVersionXML;
    static List<string> _listPlatform = new List<string>();

	private static string CurSDK = "version_poker_ios_appstore";

    //功能部分
    //从版本信息中查找当前版本号
    const string version_dir = "VERSION_POKER";
    const string _strVersionXMLUrl_format = "http://update.game3336.com/{0}/{1}/version.xml";//版本xml
    public static string _strVersionXMLUrl = "";//版本xml
    XDocument _xmlFilesList = null;
    //public static string VersionXMLUrl = "http://ksyx.update.iwodong.com/war3g_version/version_war3g_window_test/version.xml";//版本xml
    public static void ShowWindow()
    {
        _listPlatform.Clear();
        _listPlatform.Add("version_poker_window");
        _listPlatform.Add("version_poker_android_official");
		_listPlatform.Add("version_poker_ios_appstore");
        //创建窗口
        Rect wr = new Rect(0, 0, 500, 540);
        PakageSourceVersion window = (PakageSourceVersion)EditorWindow.GetWindowWithRect(typeof(PakageSourceVersion), wr, true, "生成资源更新包");
        window.Show();
    }


    public void Awake()
    {
		TextAsset text_asset = Resources.Load("SDKManager") as TextAsset;
		INIParser ini = new INIParser();
		ini.Open(text_asset);
		CurSDK = ini.ReadValue("platform", "name", "version_poker_window");
		ini.Close();
    }


    //绘制窗口时调用
    void OnGUI()
    {

        GUILayout.Space(20f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("选择平台:");
        //不同平台
        if (GUILayout.Button(_curPlatform, EditorStyles.layerMaskField, GUILayout.MaxWidth(450f), GUILayout.MinHeight(30f)))
        {
            if (_bIniting)
                return;
            if (_bSaving)
                return;

            bool isSelect = false;
            GenericMenu menu = new GenericMenu();
            for (int i=0; i< _listPlatform.Count; i++)
            {
                isSelect = false;
                EventMultiArgs args = new EventMultiArgs();
                args.AddArg("platform", _listPlatform[i]);
                if (_curPlatform != null && _curPlatform.Equals(_listPlatform[i]))
                {
                    isSelect = true;
                }
                menu.AddItem(new GUIContent(_listPlatform[i]), isSelect, ChangePlatform, args);
            }
            
            menu.ShowAsContext();
        }
        GUILayout.EndHorizontal();
        //输入框控件
        //GUILayout.Space(5f);
        GUILayout.Label("外部版本号:" + _sProgramVersion + "    本地版本号:" + ClientDefine.LOCAL_PROGRAM_VERSION, GUILayout.MinHeight(30f));
        //输入框控件
        //GUILayout.Space(5f);
        GUILayout.Label("资源包编号:" + _curVersion, GUILayout.MinHeight(30f));

        //开始生成目录
        if (_savedPath == null)
        {
            _savedPath = "";
        }
        //GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("资源包生成目录:");
        if (GUILayout.Button("打开", GUILayout.MinHeight(20f), GUILayout.MaxWidth(100f)))
        {
            if (_savedPath != "")
            {
                System.IO.Directory.CreateDirectory(_savedPath);
                System.IO.File.SetAttributes(_savedPath, FileAttributes.Normal);
                EditorUtility.RevealInFinder(_savedPath);
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.TextArea(_savedPath, GUILayout.MinHeight(50f));


        //打Log
        if (_log == null)
        {
            _log = "";
        }
        GUILayout.Space(20f);
        GUILayout.Label("LOG:");
        GUILayout.TextArea(_log, GUILayout.MinHeight(50f));

        //打Log
        if (_error == null)
        {
            _error = "";
        }
        GUILayout.Space(20f);
        GUILayout.Label("错误:");
        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(1, 0, 0);   //设置字体颜色的
        style.wordWrap = true;
        style.fixedWidth = 480f;
        style.contentOffset = new Vector2(10f, 0f);
        style.fontSize = 20;
        GUILayout.TextArea(_error, style, GUILayout.MinHeight(140f));


        //开始生成
        GUILayout.Space(20f);
        if (GUILayout.Button("开始生成", GUILayout.MinHeight(30f)))
        {
            if (_bIniting)
                return;
            if (_bSaving)
                return;

            if (_error != null && _error != "")
            {
                this.ShowNotification(new GUIContent("请先处理错误！"));
                return;
            }

            if (_curPlatform == null || _curPlatform == "")
            {
                this.ShowNotification(new GUIContent("请先选择平台！"));
                return;
            }

            if (!Directory.Exists(GetBakeUpAssetsPath()))
            {
                System.IO.Directory.CreateDirectory(GetBakeUpAssetsPath());
//                 _error = "备份目录不存在: " + GetBakeUpAssetsPath();
//                 this.ShowNotification(new GUIContent("备份目录不存在！"));
//                 return;
            }

            string[] files = Directory.GetFiles(GetBakeUpAssetsPath(), "*.*", SearchOption.AllDirectories);
            if (files.Length <= 99)
            {
                _error = "备份目录是空的, 或者备份不完整: " + GetBakeUpAssetsPath();
                this.ShowNotification(new GUIContent("备份目录是空的, 或者备份不完整！"));
                return;
            }

            //清除names
            //customer.clearAllAssetbundlesName();
            //生成差异文件的names
            //customer.collectXMLAssetsPath();
            //
            //customer.collectUITextureAssetsPath();
            //打包assetbundles
            //customer.collectAllAssetbundles();

            //生成内更资源目录
            customer.createPersistentPathXML();

            AssetDatabase.Refresh();
            //开始生成版本包
            StartCreatePackage();
        }

        //文本框显示鼠标在窗口的位置
//         EditorGUILayout.LabelField("鼠标在窗口的位置", Event.current.mousePosition.ToString());
// 
//         if (GUILayout.Button("关闭窗口", GUILayout.Width(200)))
//         {
//             //关闭窗口
//             this.Close();
//         }

    }
    public void ChangePlatform(object obj)
    {
        EventMultiArgs func = obj as EventMultiArgs;
        string platform = func.GetArg<string>("platform");
        _curPlatform = platform;
        _log = "";
        _error = "";
        _curVersion = "";
        _savedPath = "";
        //_strVersionXMLUrl = "";
        _sProgramVersion = "";
        _sResultDirName = "";
        _sResultDirName = platform;
        _strVersionXMLUrl = string.Format(_strVersionXMLUrl_format, version_dir, platform);
        Init(_curPlatform);
    }

    //更新
    void Update()
    {
        if (_downloadTask != null)
        {
            if (_downloadTask.isDone)
            {
                ParseVersionsXML(_downloadTask);
                _downloadTask = null;
            }
        }
    }

    void OnFocus()
    {
        Debug.Log("当窗口获得焦点时调用一次");
    }

    void OnLostFocus()
    {
        Debug.Log("当窗口丢失焦点时调用一次");
    }

    void OnHierarchyChange()
    {
        Debug.Log("当Hierarchy视图中的任何对象发生改变时调用一次");
    }

    void OnProjectChange()
    {
        Debug.Log("当Project视图中的资源发生改变时调用一次");
    }

    void OnInspectorUpdate()
    {
        //Debug.Log("窗口面板的更新");
        //这里开启窗口的重绘，不然窗口信息不会刷新
        this.Repaint();
    }

    void OnSelectionChange()
    {
        //当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
        foreach (Transform t in Selection.transforms)
        {
            //有可能是多选，这里开启一个循环打印选中游戏对象的名称
            Debug.Log("OnSelectionChange" + t.name);
        }
    }

    void OnDestroy()
    {
        Debug.Log("当窗口关闭时调用");
        _lastVersion = "";
    }

    //************************************************************************************************
    //功能实现部分
    //************************************************************************************************

	
    public PakageSourceVersion(string sPlatform)
    {
        if (sPlatform == "WINDOW")
        {
        }
        else if (sPlatform == "IOS")
        {
        }
        else if (sPlatform == "IOS_BREAK")
        {
        }
        else if (sPlatform == "ANDROID")
        {
        }
        Init(sPlatform);
    }

    public void Init(string platformName)
    {
        _bIniting = true;
        DownloadVersionsXML();
    }

    /// <summary>
    /// 第1步：下载版本列表xml
    /// </summary>
    private void DownloadVersionsXML()
    {
        try
        {
            _downloadTask = new WWW(_strVersionXMLUrl);
            _log = "下载版本信息文件：" + _strVersionXMLUrl;
        }
        catch (System.Exception ex)
        {
            _error = "下载的版本信息文件不存在(" + _strVersionXMLUrl + ")";
            this.ShowNotification(new GUIContent("初始化失败！" + ex.ToString()));
            _bIniting = false;
            return;
        }
    }

    /// <summary>
    /// 第2步：取版本号
    /// </summary>
    private void ParseVersionsXML(WWW _download)
    {
        if (_download == null || _download.text.Length == 0)
        {
            _downloadTask = null;
            _bIniting = false;
            _error = "下载版本信息文件失败(" + _strVersionXMLUrl + ")";
            this.ShowNotification(new GUIContent("初始化失败！"));
            return;
        }
        docVersionXML = null;
        try
        {
            docVersionXML = XDocument.Parse(_download.text);
        }
        catch (System.Exception ex)
        {
            _downloadTask = null;
            _bIniting = false;
            this.ShowNotification(new GUIContent("初始化失败！" + ex.ToString()));
            _error = "解释版本信息文件失败：" + _download.text;
            return;
        }

        int iVersion = 1000;
        //取版本列表
        foreach (XElement item in docVersionXML.Root.Descendants("programVersion"))
        {
            _sProgramVersion = item.Attribute("version").Value;
            foreach (XElement param in item.Descendants("sourceVersion"))
            {
                if (param.Attribute("version") != null)
                {
                    int tempVersion = 0;
                    int.TryParse(param.Attribute("version").Value, out tempVersion);
                    if (tempVersion > iVersion)
                    {
                        iVersion = tempVersion;
                    }
                }
            }
        }
        _lastVersion = iVersion.ToString();
        iVersion++;
        _curVersion = iVersion.ToString();
        _savedPath = _HardDisk + "/" + version_dir + "/" + _sResultDirName + "/" + ClientDefine.LOCAL_PROGRAM_VERSION + "/" + iVersion + "/";
        _versionXMLPath = _HardDisk + "/" + version_dir + "/" + _sResultDirName + "/version.xml";
        _outSidePath = _HardDisk + "/" + version_dir + "/" + _sResultDirName + "_outside/" + ClientDefine.LOCAL_PROGRAM_VERSION + "/" + _lastVersion + "/Assets/StreamingAssets/" + IPath.getPlatformName() + "/";
        _curVersionBakePath = _HardDisk + "/" + version_dir + "/" + _sResultDirName + "_outside/" + ClientDefine.LOCAL_PROGRAM_VERSION + "/" + _curVersion + "/Assets/StreamingAssets/" + IPath.getPlatformName() + "/";
        _bIniting = false;
        if(!Directory.Exists(_outSidePath))
        {
            System.IO.Directory.CreateDirectory(_outSidePath);
            this.ShowNotification(new GUIContent("最新资源备份不存在: " + _outSidePath));
            _error = "最新资源备份不存在：" + _outSidePath;
        }
    }

    //每次出大版本时备份一次的Assets目录(内更版本不会)
    public static string GetBakeUpAssetsPath()
    {
		string _DirName = CurSDK;
        return _HardDisk + "/" + version_dir + "/" + _DirName + "_outside/" + ClientDefine.LOCAL_PROGRAM_VERSION + "/1000/";
    }
    //每次出大版本时备份一次的Assets目录(内更版本不会)
    public static string GetBakeUpAssetbundlePath()
    {
		string _DirName = CurSDK;
        return _HardDisk + "/" + version_dir + "/" + _DirName + "_outside/" + ClientDefine.LOCAL_PROGRAM_VERSION + "/";
    }

    public void StartCreatePackage()
    {
        packageSize = 0f;
        //_xmlFilesList = XDocument.Parse("<FilesLocation></FilesLocation>");
        _xmlFilesList = new XDocument(new XDeclaration("1.0", "utf-8", null),new XElement("FilesLocation"));
        //CompareTwoDir("Assets/Resources/", "E:/sanguo/code_android/SG.proj/Assets/Resources/");
        string platformName = IPath.getPlatformName();
        CompareTwoDir("Assets/StreamingAssets/" + platformName + "/", _outSidePath);
        SaveFilesList();
        SaveVersionList();
        EditorUtility.RevealInFinder( _savedPath);
        this.ShowNotification(new GUIContent("导出资源包完成！"));
        BakeToDir("Assets/StreamingAssets/" + platformName + "/", _curVersionBakePath);
        this.ShowNotification(new GUIContent("备份资源包完成！"));
    }

    private void CompareTwoDir(string sNewAssetPath, string sOldAssetPath)
    {
        string[] files = Directory.GetFiles(sNewAssetPath, "*.*", SearchOption.AllDirectories);
        //遍历资源文件
        foreach (string file in files)
        {
            if (file.Contains(".meta")) continue;
            if (file.Contains(".svn")) continue;
            if (file.Contains(".manifest")) continue;
            //if (file.Contains("Assets/Resources/Levels")) continue;
            //if (file.Contains("Assets/Resources/Models")) continue;

            string normalFile = file.Replace("\\", "/");
            normalFile = normalFile.Substring(sNewAssetPath.Length);

            if (File.Exists(sOldAssetPath + normalFile))
            {
                try
                {
                    FileStream _stream_new = File.Open(sNewAssetPath + normalFile, FileMode.Open, FileAccess.Read);
                    byte[] byte_new = new byte[_stream_new.Length];
                    _stream_new.Read(byte_new, 0, byte_new.Length);
                    string md5_new = UtilTools.GetFileMD5(byte_new);
                    _stream_new.Close();

                    FileStream _stream_old = File.Open(sOldAssetPath + normalFile, FileMode.Open, FileAccess.Read);
                    byte[] byte_old = new byte[_stream_old.Length];
                    _stream_old.Read(byte_old, 0, byte_old.Length);
                    string md5_old = UtilTools.GetFileMD5(byte_old);
                    _stream_old.Close();

                    if (md5_new != md5_old)
                    {
                        SaveFileToDir(_savedPath, normalFile, byte_new, md5_new);
                    }
                }
                catch (System.Exception ex)
                {
                    _error = "Read2FilesFailed:" + normalFile;
                    _bSaving = false;
                    Debug.LogException(ex);
                    return;
                }
            }
            else
            {
                try
                {
                    FileStream _stream_new = File.Open(sNewAssetPath + normalFile, FileMode.Open, FileAccess.Read);
                    byte[] byte_new = new byte[_stream_new.Length];
                    _stream_new.Read(byte_new, 0, byte_new.Length);
                    _stream_new.Close();
                    string md5_new = UtilTools.GetFileMD5(byte_new);
                    SaveFileToDir(_savedPath, normalFile, byte_new, md5_new);
                }
                catch (System.Exception ex)
                {
                    _error = "ReadFileFailed:" + normalFile;
                    _bSaving = false;
                    Debug.LogException(ex);
                    return;
                	
                }
            }
        }

    }
    private void BakeToDir(string sCurPath, string sSaveToPath)
    {
        string[] files = Directory.GetFiles(sCurPath, "*.*", SearchOption.AllDirectories);
        //遍历资源文件
        foreach (string file in files)
        {
            if (file.Contains(".meta")) continue;
            if (file.Contains(".svn")) continue;
            if (file.Contains(".manifest")) continue;
            //if (file.Contains("Assets/Resources/Levels")) continue;
            //if (file.Contains("Assets/Resources/Models")) continue;

            string normalFile = file.Replace("\\", "/");
            normalFile = normalFile.Substring(sCurPath.Length);

            if (File.Exists(sSaveToPath + normalFile))
            {
                try
                {
                    FileStream _stream_new = File.Open(sCurPath + normalFile, FileMode.Open, FileAccess.Read);
                    byte[] byte_new = new byte[_stream_new.Length];
                    _stream_new.Read(byte_new, 0, byte_new.Length);
                    _stream_new.Close();
                    string md5_new = UtilTools.GetFileMD5(byte_new);

                    FileStream _stream_old = File.Open(sSaveToPath + normalFile, FileMode.Open, FileAccess.Read);
                    byte[] byte_old = new byte[_stream_old.Length];
                    _stream_old.Read(byte_old, 0, byte_old.Length);
                    _stream_old.Close();
                    string md5_old = UtilTools.GetFileMD5(byte_old);

                    if (md5_new != md5_old)
                    {
                        SaveFileToDir(sSaveToPath, normalFile, byte_new, null);
                    }
                }
                catch (System.Exception ex)
                {
                    _error = "Bake2FilesFailed:" + normalFile;
                    _bSaving = false;
                    Debug.LogException(ex);
                    return;
                }
            }
            else
            {
                try
                {
                    FileStream _stream_new = File.Open(sCurPath + normalFile, FileMode.Open, FileAccess.Read);
                    byte[] byte_new = new byte[_stream_new.Length];
                    _stream_new.Read(byte_new, 0, byte_new.Length);
                    _stream_new.Close();
                    //string md5_new = UtilTools.GetFileMD5(byte_new);
                    SaveFileToDir(sSaveToPath, normalFile, byte_new, null);
                }
                catch (System.Exception ex)
                {
                    _error = "BakeFileFailed:" + normalFile;
                    _bSaving = false;
                    Debug.LogException(ex);
                    return;

                }
            }
        }
    }

    private void SaveFileToDir(string savedPath, string fileDir, byte[] data, string fileMD5)
    {
        //对比MD5
//         string md5 = UtilTools.GetFileMD5(download.bytes);
//         if (serverMD5 != md5)
//         {
//             LoadError(VersionData.ErrorCode.DownLoadFileMD5Error);
//             return;
//        }

        _log = "CopyFile:" + fileDir;
        //创建临时文件
        string filePath = savedPath + fileDir;
        string tempFilePath = savedPath + fileDir + "temp";
        string dirPath = Path.GetDirectoryName(filePath);
        FileStream stream;
        try
        {
            System.IO.Directory.CreateDirectory(dirPath);
            System.IO.File.SetAttributes(dirPath, FileAttributes.Normal);
            stream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write);
        }
        catch (System.Exception ex)
        {
            _error = "CreateFileFailed:" + tempFilePath;
            _bSaving = false;
            Debug.LogException(ex);
            return;
        }

        //写临时文件
        try
        {
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();
        }
        catch (System.Exception e)
        {
            _error = "WriteFileFailed:" + tempFilePath;
            _bSaving = false;
            Debug.LogException(e);
            return;
        }

        //名字改为正式
        try
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.SetAttributes(filePath, FileAttributes.Normal);
                System.IO.File.Delete(filePath);
            }
            System.IO.File.Move(tempFilePath, filePath);
        }
        catch (System.Exception e)
        {
            _error = "RenameFileFailed:" + tempFilePath;
            _bSaving = false;
            Debug.LogException(e);
            System.IO.File.SetAttributes(filePath, FileAttributes.Normal);
            System.IO.File.Delete(filePath);
            return;
        }
        if (fileMD5 != null && fileMD5.Length > 0)
        {
            float fileSize = (float)data.Length / 1024f;
            packageSize += fileSize;
            AddToFilesList(fileDir, fileMD5);
        }
    }
    private void AddToFilesList(string filePath, string md5)
    {
        XElement targetItem = new XElement("File");
        targetItem.SetAttributeValue("name", filePath);
        targetItem.SetAttributeValue("md5", md5);
        _xmlFilesList.Root.Add(targetItem);
    }

    //保存资源文件列表XML
    private void SaveFilesList()
    {
        if (_xmlFilesList.Root.IsEmpty)
            return;
        StreamWriter sw = new StreamWriter(_savedPath + "filespath.xml", false, new System.Text.UTF8Encoding(false));
        _xmlFilesList.Save(sw);
    }

    //保存版本号列表XML
    private void SaveVersionList()
    {
        int package_size = (int)packageSize;
        foreach (XElement item in docVersionXML.Root.Descendants("programVersion")) //得到每一个Sence节点
        {

            XElement targetVersion = null;
            foreach (XElement version in item.Descendants("sourceVersion")) //得到每一个Sence节点
            {
                if (version.Attribute("version").Value == _curVersion)
                {
                    targetVersion = version;
                    targetVersion.RemoveNodes();
                    //targetItem.RemoveAttributes();
                    break;
                }
            }
            if (targetVersion == null)
            {
                targetVersion = new XElement("sourceVersion");
                item.Add(targetVersion);
            }
            targetVersion.SetAttributeValue("version", _curVersion);
            targetVersion.SetAttributeValue("size", package_size);
            break;
        }
        if (System.IO.File.Exists(_versionXMLPath))
        {
            System.IO.File.SetAttributes(_versionXMLPath, FileAttributes.Normal);
            System.IO.File.Delete(_versionXMLPath);
        }
        StreamWriter sw = new StreamWriter(_versionXMLPath, false, new System.Text.UTF8Encoding(false));
        docVersionXML.Save(sw);
    }
}
