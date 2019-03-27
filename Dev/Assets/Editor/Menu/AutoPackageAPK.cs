/***************************************************************


 *
 *
 * Filename:  	PackageSourceVersion.cs	
 * Summary: 	APK安装打包工具
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

public class AutoPackageAPK : EditorWindow {

    //************************************************************************************************
    //窗口显示部分
    //************************************************************************************************
    //输入文字的内容
    private static string Out_Path = "";
    private static string sPath = "D:/VERSION_BUYU/APK/";
    private string _key;
    private string _packageName;
    private bool _bPackageAll = false;
    private static string _log = "";
    private int logTime = 0;
    private string _version = "";
    private static string _versionCode = "0";
    GUIStyle style;
#if UNITY_IOS
    const string _HardDisk = "/Users/wodong/Documents";
#else
    const string _HardDisk = "D:";//G盘
#endif
    static Dictionary<string, string> _listPlatform = new Dictionary<string, string>();
    static Dictionary<string, string>.Enumerator _dicEnum;
    static Dictionary<string, bool> _dicEnumSelect = new Dictionary<string, bool>();//是否选中
    static string[] SCENES;
    //功能部分
    //从版本信息中查找当前版本号
    const string version_dir = "VERSION_BUYU";
    const string _strVersionXMLUrl_format = "http://ksyx.update.iwodong.com/{0}/{1}/version.xml";//版本xml
    public static string _strVersionXMLUrl = "";//版本xml
    XDocument _xmlFilesList = null;
    static bool bSelectAll = false;
    Vector2 mScroll = Vector2.zero;
    //public static string VersionXMLUrl = "http://ksyx.update.iwodong.com/war3g_version/version_war3g_window_test/version.xml";//版本xml
    public static void ShowWindow()
    {
        InitList();
        //创建窗口
        Rect wr = new Rect(0, 0, 600, 540);
        AutoPackageAPK window = (AutoPackageAPK)EditorWindow.GetWindowWithRect(typeof(AutoPackageAPK), wr, true, "打包工具");
        window.Show();
    }

    private static void InitList()
    {
        string iniPath = Application.dataPath + "/Resources/SDKManager.bytes";
        INIParser ini = new INIParser();
        ini.Open(iniPath);
        List<string> sections = ini.GetAllSections();
        _listPlatform.Clear();
        _dicEnumSelect.Clear();
        bSelectAll = false;
        for (int k = 0; k < sections.Count; k++)
        {
            if (ini.IsKeyExists(sections[k], "apkname"))
            {
                string apk_name = ini.ReadValue(sections[k], "apkname", sections[k]);
                _listPlatform.Add(sections[k], apk_name);
                _dicEnumSelect.Add(sections[k], false);
            }
        }
        ini.Close();
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    public void Awake()
    {

        style = new GUIStyle();
        style.normal.textColor = new Color(0, 1, 0);   //设置字体颜色的
        style.wordWrap = true;
        style.fixedWidth = 480f;
        style.contentOffset = new Vector2(10f, 0f);
        style.fontSize = 20;
        _versionCode = PlayerSettings.Android.bundleVersionCode.ToString();
        SCENES = FindEnabledEditorScenes();
        //Out_Path = sPath + ClientDefine.LOCAL_PROGRAM_VERSION + "/" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        Out_Path = string.Format("{0}/{1}/{2}({3})", sPath, ClientDefine.LOCAL_PROGRAM_VERSION, System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm"), ClientDefine.LOCAL_SOURCE_VERSION);
    }


    //绘制窗口时调用
    void OnGUI()
    {
        if (_versionCode == "0")
            _versionCode = PlayerSettings.Android.bundleVersionCode.ToString();
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("bundle id:     ");
        GUILayout.TextField(PlayerSettings.bundleIdentifier, GUILayout.MinHeight(20f), GUILayout.MinWidth(190f));
        GUILayout.Label("               version:");
        GUILayout.TextField(ClientDefine.LOCAL_PROGRAM_VERSION, GUILayout.MinHeight(20f), GUILayout.MinWidth(150f));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("source version:");
        GUILayout.TextField(ClientDefine.LOCAL_SOURCE_VERSION, GUILayout.MinHeight(20f), GUILayout.MinWidth(200f));
        GUILayout.Label("         version code:");
        _versionCode = GUILayout.TextField(_versionCode, GUILayout.MinHeight(20f), GUILayout.MinWidth(160f));
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);

        GUILayout.Label("渠道:");
        GUILayout.BeginHorizontal();
        GUILayout.Space(6f);
        bool bSelect = EditorGUILayout.ToggleLeft("选中所有", bSelectAll, GUILayout.Width(60f));
        if (bSelectAll != bSelect)
        {
            if (bSelect)
            {
                SelectAllItem();
            }
            else
            {
                UnSelectAllItem();
            }
            bSelectAll = bSelect;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(6f);
        //不同平台
        int Index = 0;
        if (_listPlatform.Count == 0)
        {
            InitList();
        }
        //mScroll.y = 150f;
        mScroll = GUILayout.BeginScrollView(mScroll, GUILayout.Width(0), GUILayout.Height(250));
        GUILayout.BeginHorizontal();
        _dicEnum = _listPlatform.GetEnumerator();
        for (int i=0; i<_listPlatform.Count; i++)
        {
            if (Index % 3 == 0 )
            {
                GUILayout.EndHorizontal();
                GUILayout.Space(10f);
                GUILayout.BeginHorizontal();
            }
            if (_dicEnum.MoveNext())
            {
                bool bSele = EditorGUILayout.ToggleLeft(_dicEnum.Current.Value, _dicEnumSelect[_dicEnum.Current.Key], GUILayout.Width(180f));
                if (_dicEnumSelect[_dicEnum.Current.Key] != bSele)
                {
                    _dicEnumSelect[_dicEnum.Current.Key] = bSele;
                    if (bSele)
                    {
                        //_key = _dicEnum.Current.Key;
                        //_packageName = _dicEnum.Current.Value;
                    }
                }
            }
            Index++;
        }
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.EndScrollView();
        //开始生成
        GUILayout.Space(10f);
        if (GUILayout.Button("测试包", GUILayout.MinHeight(40f), GUILayout.MaxWidth(180f)))
        {
            _key = "version_buyu_android_test";
            _packageName = "BuYu_Test";
        }

        //开始生成
        GUILayout.Space(10f);
        if (GUILayout.Button("打包选中渠道", GUILayout.MinHeight(50f)))
        {
            _bPackageAll = true;
        }

        GUILayout.Space(20f);
        GUILayout.TextArea(_log, style, GUILayout.MinHeight(140f));
    }

    void Update()
    {
        if (string.IsNullOrEmpty( Out_Path))
        {
            Out_Path = string.Format("{0}/{1}/{2}({3})", sPath, ClientDefine.LOCAL_PROGRAM_VERSION, System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm"), ClientDefine.LOCAL_SOURCE_VERSION);
        }
        //打包单个
        if (!string.IsNullOrEmpty(_key))
        {
            logTime = UtilTools.GetClientTime();
            _log = string.Format("正在打包：{0}.apk（0/1）", _packageName);
            bool rlt = BulidTarget(_key, _packageName);
            if (rlt)
            {
                logTime = UtilTools.GetClientTime() - logTime;
                _log = string.Format("打包花费时间: {0} 秒", logTime.ToString());
                Debug.Log(_log);
                EditorUtility.RevealInFinder(Path.GetFullPath(Out_Path));
            }
            else
            {

            }
            _key = "";
            _packageName = "";
        }
        //打包选中项
        else if (_bPackageAll)
        {
            _bPackageAll = false;
            logTime = UtilTools.GetClientTime();
            Dictionary<string, bool>.Enumerator  en = _dicEnumSelect.GetEnumerator();
            int count = 0;
            List<string> listSelect = new List<string>();
            for (int i = 0; i < _dicEnumSelect.Count; i++)
            {
                if (en.MoveNext())
                {
                    if (en.Current.Value)
                    {
                        listSelect.Add(en.Current.Key);
                    }
                }
            }
            for (int i = 0; i < listSelect.Count; i++)
            {
                string skey = listSelect[i];
                string apk_name = _listPlatform[skey];
                _log = string.Format("正在打包：{0}.apk（{1}/{2}）", apk_name, (i + 1).ToString(), listSelect.Count.ToString());
                bool rlt = BulidTarget(skey, apk_name);
                if (!rlt)
                    break;
            }
            if (listSelect.Count == 0)
            {
                Debug.Log("未选中渠道");
            }
            else
            {
                logTime = UtilTools.GetClientTime() - logTime;
                _log = string.Format("打包花费时间: {0} 秒", logTime.ToString());
                Debug.Log(_log);
                EditorUtility.RevealInFinder(Path.GetFullPath(Out_Path));
            }
        }
    }

    //这里封装了一个简单的通用方法。
    static bool BulidTarget(string sKey, string apkName)
    {
        string app_name = apkName;
        string target_name = app_name + ".apk";
        //BuildTargetGroup targetGroup = BuildTargetGroup.Android;
        BuildTarget buildTarget = BuildTarget.Android;
        string applicationPath = Application.dataPath.Replace("/Assets", "");

#if UNITY_ANDROID
        target_name = app_name + ".apk";
        PlayerSettings.keystorePass = "Wodong1802";
        PlayerSettings.keyaliasPass = "Wodong1802";
        //targetGroup = BuildTargetGroup.Android;
        //PlayerSettings.bundleIdentifier = "com.game.qq";
#elif UNITY_IOS

#endif
        //PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, sKey);

        string iniPath = Application.dataPath + "/Resources/SDKManager.bytes";
        INIParser ini = new INIParser();
        ini.Open(iniPath);
        ini.WriteValue("platform", "name", sKey);
        ini.Close();

        //每次build删除之前的残留
        if (Directory.Exists(Out_Path))
        {
            if (File.Exists(Out_Path + "/" + target_name))
            {
                File.Delete(Out_Path + "/" + target_name);
            }
        }
        else
        {
            Directory.CreateDirectory(Out_Path);
        }
        PlayerSettings.bundleVersion = ClientDefine.LOCAL_PROGRAM_VERSION;
        int code = 0;
        if (int.TryParse(_versionCode, out code))
            PlayerSettings.Android.bundleVersionCode = code;
        else
        {
            _log = "version code 错误！";
            return false;
        }
        AssetDatabase.Refresh();
        //==================这里是比较重要的东西=======================

        //开始Build场景，等待吧～
        return GenericBuild(SCENES, Out_Path + "/" + target_name, buildTarget, BuildOptions.None);

    }

    static bool GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        //EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
        //return true;
        string res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);

        if (res.Length > 0)
        {
            Debug.LogError("BuildPlayer failure: " + res);
            return false;
        }
        else
        {
            Debug.Log("成功：" + target_dir);
            return true;
        }
    }

    private static void SelectAllItem()
    {

        Dictionary<string, string>.Enumerator dicEnum = _listPlatform.GetEnumerator();
        for (int i = 0; i < _listPlatform.Count; i++)
        {
            if (dicEnum.MoveNext())
                _dicEnumSelect[dicEnum.Current.Key] = true;
        }
    }
    private static void UnSelectAllItem()
    {
        Dictionary<string, string>.Enumerator dicEnum = _listPlatform.GetEnumerator();
        for (int i = 0; i < _listPlatform.Count; i++)
        {
            if (dicEnum.MoveNext())
                _dicEnumSelect[dicEnum.Current.Key] = false;
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
    }
    
}
