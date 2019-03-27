using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using customerPath;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditor.SceneManagement;
using FtpLib;

public class customer {
    //引用结构
    public class RefRecord
    {
        public string _strAssetPath;
        public int _nRefCount;
    };

    ///////////////////////////////////////////////////////////////////////////////////

    //文件位置信息
    private static Dictionary<string, string> _dictAssetLocation = new Dictionary<string,string>();

    //文件位置信息配置文件
    private static string ASSETS_LOCATION_CONFIG = @"Assets/Resources/AssetsPathsInfo.xml";
    private static string ASSETS_PERSISTENT_CONFIG = @"Assets/StreamingAssets/{0}/AssetsPersistentPathsInfo.xml";

    public static string _strCurrentModelName = "";
    private static string[] _scriptFileType = new string[] { ".js", ".cs", ".mat", ".meta", ".anim", ".controller" };//这些文件将不会被单独打包
    private static int[] _scriptFileTypeLength = new int[] { 3, 3, 4, 5, 5, 11 };

    ///////////////////////////////////////////////////////////////////////////////////


    #region 排序函数

    //按照文件的引用个数降序排序
    public class DinoComparer : IComparer<RefRecord>
    {
        public int Compare(RefRecord x, RefRecord y)
        {
            if (x._nRefCount > y._nRefCount)
            {
                return -1;
            }
            else if (x._nRefCount < y._nRefCount)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    };

    #endregion


    ///////////////////////////////////////////////////////////////////////////////////

    //[MenuItem("FilePackage/生成依赖文件")]
    public static void collectAllAssetsDependencePath()
    {
        string[] files = Directory.GetFiles("Assets/Resources", "*.*", SearchOption.AllDirectories);

        //引用计数列表
        List<RefRecord> listRef = new List<RefRecord>();

        //快速索引表
        Dictionary<string, int> dicQuickIndex = new Dictionary<string, int>();

        //依赖表
        Dictionary<string, List<RefRecord>> dicDependencies = new Dictionary<string, List<RefRecord>>();

        //保存引用关系
        ResDependenciesHolder ResDependenciesContent = ScriptableObject.CreateInstance<ResDependenciesHolder>();

        //遍历资源文件
        foreach (string file in files)
        {
            if (file.Contains(".meta")) continue;

            string normalFile = file.Replace("\\", "/");

            RefRecord rf = new RefRecord();
            rf._strAssetPath = normalFile;
            rf._nRefCount = 0;

            listRef.Add(rf);
            dicQuickIndex[normalFile] = listRef.Count - 1;
        }

        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            Object obj = AssetDatabase.LoadMainAssetAtPath(file);
            if (obj != null)
            {
                //如果是材质保存shader信息，生成assetbundle时shader并没有一起打包，所以需要单独保存
                Material matObj = obj as Material;
                if (matObj != null)
                {
                    string strMat = strNormalPath.Substring(@"Assets/Resources/".Length);
                    string strShaderName = matObj.shader.name;

                    Debug.Log(strMat + ": " + strShaderName);

                    ResDependenciesContent.addMatShader(strMat, strShaderName);
                }

                Object[] dependObjs = EditorUtility.CollectDependencies(new Object[] { obj });

                //依赖asset路径
                List<string> dependenciesPaths = new List<string>();

                //依赖信息表
                List<RefRecord> dependenciesRecord = new List<RefRecord>();

                foreach (var dependObj in dependObjs)
                {
                    string strPath = AssetDatabase.GetAssetPath(dependObj);
                    if (strPath != file)
                    {
                        if (strPath.Contains("Assets/Resources") && !dependenciesPaths.Contains(strPath) && strPath != strNormalPath)
                        {
                            dependenciesPaths.Add(strPath);

                            //更新引用计数列表相应资源的引用计数
                            int nQuickIndx = dicQuickIndex[strPath];
                            listRef[nQuickIndx]._nRefCount++;

                            dependenciesRecord.Add(listRef[nQuickIndx]);
                        }
                    }
                }

                //保存依赖表
                dicDependencies[strNormalPath] = dependenciesRecord;
            }
        }

        //排序
        DinoComparer dc = new DinoComparer();

        //按照引用计数排序，防止因为顺序导致的依赖资源没有预先加载
        foreach (KeyValuePair<string, List<RefRecord>> kvp in dicDependencies)
        {
            kvp.Value.Sort(dc);
        }

        #region 保存依赖关系到文件

        foreach (KeyValuePair<string, List<RefRecord>> kvp in dicDependencies)
        {
            AssetDepRecord pAssetDepObj = new AssetDepRecord(kvp.Key.Substring(@"Assets/Resources/".Length));
            Debug.Log(kvp.Key);
            foreach (var value in kvp.Value)
            {
                Debug.Log("            " + value._strAssetPath + ": " + Convert.ToString(value._nRefCount));
                pAssetDepObj._listDependencies.Add(value._strAssetPath.Substring(@"Assets/Resources/".Length));
            }

            ResDependenciesContent.addAssetDependencies(pAssetDepObj);
        }


        //生成引用关系asset
        string p = "Assets/Resources/ResDependenciesContent.asset";
        if (File.Exists(p)) File.Delete(p);
        AssetDatabase.CreateAsset(ResDependenciesContent, p);

        string strTargetDependenciesContentAssetbundlePath = targetAssetbundlePath(p);

        //打包引用关系
        UnityEngine.Object depContentObj = AssetDatabase.LoadMainAssetAtPath(p);
        BuildPipeline.BuildAssetBundle(depContentObj, null, strTargetDependenciesContentAssetbundlePath, EditorVariables.eBuildABOpt, EditorVariables.eBuildTarget);

        #endregion  //保存依赖关系到文件
    }

    ///////////////////////////////////////////////////////////////////////////////////
    /*
    [MenuItem("FilePackage/打包Assetbundle")]
    public static void collectAllAssetsPath()
    {
        string[] files = Directory.GetFiles("Assets/Resources", "*.*", SearchOption.AllDirectories);

        //引用计数列表
        List<RefRecord> listRef = new List<RefRecord>();

        //快速索引表
        Dictionary<string, int> dicQuickIndex = new Dictionary<string, int>();

        //依赖表
        Dictionary<string, List<RefRecord>> dicDependencies = new Dictionary<string, List<RefRecord>>();

        //遍历资源文件
        foreach (string file in files)
        {
            if (file.Contains(".meta")) continue;

            string normalFile = file.Replace("\\", "/");

            RefRecord rf = new RefRecord();
            rf._strAssetPath = normalFile;
            rf._nRefCount = 0;

            listRef.Add(rf);
            dicQuickIndex[normalFile] = listRef.Count - 1;
        }

        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            Object obj = AssetDatabase.LoadMainAssetAtPath(file);
            if (obj != null)
            {
                Object[] dependObjs = EditorUtility.CollectDependencies(new Object[] { obj });

                //依赖asset路径
                List<string> dependenciesPaths = new List<string>();

                //依赖信息表
                List<RefRecord> dependenciesRecord = new List<RefRecord>();

                foreach (var dependObj in dependObjs)
                {
                    string strPath = AssetDatabase.GetAssetPath(dependObj);
                    if (strPath != file)
                    {
                        if (strPath.Contains("Assets/Resources") && !dependenciesPaths.Contains(strPath) && strPath != strNormalPath)
                        {
                            dependenciesPaths.Add(strPath);

                            //更新引用计数列表相应资源的引用计数
                            int nQuickIndx = dicQuickIndex[strPath];
                            listRef[nQuickIndx]._nRefCount++;

                            dependenciesRecord.Add(listRef[nQuickIndx]);
                        }
                    }
                }

                //保存依赖表
                dicDependencies[strNormalPath] = dependenciesRecord;
            }
        }
 
        //排序
        DinoComparer dc = new DinoComparer();
        listRef.Sort(dc);


        #region 生成assetbundle文件

        ////打包
        foreach (var refData in listRef)
        {
            //依赖关系
            BuildPipeline.PushAssetDependencies();

            string strTargetAssetbundlePath = targetAssetbundlePath(refData._strAssetPath);
            Debug.Log(strTargetAssetbundlePath);
            UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath(refData._strAssetPath);
            BuildPipeline.BuildAssetBundle(asset, null, strTargetAssetbundlePath, EditorVariables.eBuildABOpt, EditorVariables.eBuildTarget);
        }

        foreach (var refData in listRef)
        {
            //依赖关系
            BuildPipeline.PopAssetDependencies();
        }

        #endregion //生成assetbundle文件
    }


    ///////////////////////////////////////////////////////////////////////////////////
    */
    //目标assetbundle路径
    public static string targetAssetbundlePath(string strResPath)
    {
        //生成目标路径
        string strResDir = Path.GetDirectoryName(strResPath);
        string strTargetFileName = Path.GetFileName(strResPath) + ".assetbundle";
        string strTargetDir = "";
        if (strResDir == "Assets/Resources" || strResDir == "Assets/Resources/")
            strTargetDir = IPath.streamingAssetsPathPlatform();
        else
            strTargetDir = IPath.streamingAssetsPathPlatform() + "/" + strResDir.Substring(@"Assets/Resources/".Length);

        string strTargetAssettBundleFilePath = strTargetDir + "/" + strTargetFileName;
        //目标路径不存在则创建
        if (!Directory.Exists(strTargetDir))
        {
            Directory.CreateDirectory(strTargetDir);
        }

        return strTargetAssettBundleFilePath;
    }


    ///////////////////////////////////////////////////////////////////////////////////

    //生成文件列表文件
    //用来标识文件的真实存在位置
    [MenuItem("FilePackage/生成文件位置信息(iOS和Android)")]
    public static void createAssetsLocationFile()
    {
        createAssetsLocaltionFile();

    }

    //sVersion: 1000表示程序大版本,资源都在Resources(除了模型)
    //sVersion: >1000表示程序资源版本,StreamingAssets下都是PERSISTENT
    public static void createAssetsLocaltionFile()
    {
        //清空旧数据
        _dictAssetLocation.Clear();

        //记录Resources目录中的资源
        string[] files = Directory.GetFiles("Assets/Resources", "*.*", SearchOption.AllDirectories);
        string strAssetKey = "";
        foreach (string file in files)
        {
            if (file.Contains(".meta")) continue;
            if (file.Contains(".manifest")) continue;
            if (file.Contains(".svn")) continue;
            if (file.Contains("Resources/Models/")) continue;
            if (file.Contains("Resources/modelsassetbundles/")) continue;
            strAssetKey = file.Replace("\\", "/").Substring(@"Assets/Resources/".Length);
            _dictAssetLocation[strAssetKey] = "RESOURCES";
        }

        //记录StreamingAssets
        string streamingAssetPlat = "Assets/StreamingAssets/" + IPath.getPlatformName();
        files = Directory.GetFiles(streamingAssetPlat, "*.*", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            if (file.Contains(".meta")) continue;
            if (file.Contains(".manifest")) continue;
            if (file.Contains(".svn")) continue;
            string strFormatFilePath = file.Replace("\\", "/");
            //Debug.Log(strFormatFilePath);
            string strFileNameWithoutExt = Path.GetFileName(strFormatFilePath);//strFormatFilePath;//
            string strPath = Path.GetDirectoryName(strFormatFilePath);
            string strDirectory = "";
            strAssetKey = strFileNameWithoutExt;
            if (strPath != streamingAssetPlat)
            {
                strDirectory = strPath.Substring(streamingAssetPlat.Length + 1);
                strAssetKey = strDirectory + "/" + strFileNameWithoutExt;
            }
            _dictAssetLocation[strAssetKey] = "STREAMINGASSETS";
        }
//         _dictAssetLocation["windows.manifest"] = "STREAMINGASSETS";
//         _dictAssetLocation["ios.manifest"] = "STREAMINGASSETS";
//         _dictAssetLocation["android.manifest"] = "STREAMINGASSETS";
        //判断文件是否存在
        if (File.Exists(ASSETS_LOCATION_CONFIG))
        {
            File.Delete(ASSETS_LOCATION_CONFIG);
        }

        //写入文件
        XmlDocument xmldoc = new XmlDocument();

        //写入xml声明
        XmlDeclaration xmldec = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", "");
        xmldoc.AppendChild(xmldec);

        //写入根节点
        XmlNode xmlRootNode = xmldoc.CreateElement("AssetsLocation");
        xmldoc.AppendChild(xmlRootNode);

        //写入asset信息
        XmlElement xmlAssetElement = null;
        foreach (KeyValuePair<string, string> kvp in _dictAssetLocation)
        {
            xmlAssetElement = xmldoc.CreateElement("Asset");
            xmlAssetElement.SetAttribute("name", kvp.Key.ToLower());
            xmlAssetElement.SetAttribute("type", kvp.Value);
            xmlRootNode.AppendChild(xmlAssetElement);
        }

        //保存
        StreamWriter sw = new StreamWriter(ASSETS_LOCATION_CONFIG, false, new UTF8Encoding(false));
        xmldoc.Save(sw);
        sw.WriteLine();
        sw.Close();

        AssetDatabase.Refresh();
    }

    //sVersion: 1000表示程序大版本,资源都在Resources(除了模型)
    //sVersion: >1000表示程序资源版本,StreamingAssets下都是PERSISTENT
    public static void createPersistentPathXML()
    {
        //清空旧数据
        _dictAssetLocation.Clear();
        //记录Resources目录中的资源
        string[] files;
        string strAssetKey = "";
        //记录StreamingAssets
        string streamingAssetPlat = "Assets/StreamingAssets/" + IPath.getPlatformName();
        files = Directory.GetFiles(streamingAssetPlat, "*.*", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            if (file.Contains(".meta")) continue;
            if (file.Contains(".manifest")) continue;
            if (file.Contains(".svn")) continue;
            string strFormatFilePath = file.Replace("\\", "/");
            //Debug.Log(strFormatFilePath);
            string strFileNameWithoutExt = Path.GetFileName(strFormatFilePath);//strFormatFilePath;//
            string strPath = Path.GetDirectoryName(strFormatFilePath);
            string strDirectory = "";
            strAssetKey = strFileNameWithoutExt;
            if (strPath != streamingAssetPlat)
            {
                strDirectory = strPath.Substring(streamingAssetPlat.Length + 1);
                strAssetKey = strDirectory + "/" + strFileNameWithoutExt;
            }
            if (!EquilToOldAssetBundle(file, "1000"))//与原始备份一样
            {
                _dictAssetLocation[strAssetKey] = "PERSISTENT";
            }
        }

        if (_dictAssetLocation.Count > 0)
        {
            string persistent_path = string.Format(ASSETS_PERSISTENT_CONFIG, IPath.getPlatformName());
            //判断文件是否存在
            if (File.Exists(persistent_path))
            {
                File.Delete(persistent_path);
            }

            //写入文件
            XmlDocument xmldoc = new XmlDocument();

            //写入xml声明
            XmlDeclaration xmldec = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", "");
            xmldoc.AppendChild(xmldec);

            //写入根节点
            XmlNode xmlRootNode = xmldoc.CreateElement("AssetsLocation");
            xmldoc.AppendChild(xmlRootNode);

            //写入asset信息
            XmlElement xmlAssetElement = null;
            foreach (KeyValuePair<string, string> kvp in _dictAssetLocation)
            {
                xmlAssetElement = xmldoc.CreateElement("Asset");
                xmlAssetElement.SetAttribute("name", kvp.Key.ToLower());
                xmlAssetElement.SetAttribute("type", kvp.Value);
                xmlRootNode.AppendChild(xmlAssetElement);
            }

            //保存
            StreamWriter sw = new StreamWriter(persistent_path, false, new UTF8Encoding(false));
            xmldoc.Save(sw);
            sw.WriteLine();
            sw.Close();

            AssetDatabase.Refresh();
        }
    }
    //asset右键菜单
    //[MenuItem("Assets/", false, 0)]
    //static public void OpenSeparator2() { }


    static void ClearAssetBundlesName()
    {
        int length = AssetDatabase.GetAllAssetBundleNames().Length;
        Debug.Log(length);
        string[] oldAssetBundleNames = new string[length];
        for (int i = 0; i < length; i++)
        {
            oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
        }

        for (int j = 0; j < oldAssetBundleNames.Length; j++)
        {
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
        }
        length = AssetDatabase.GetAllAssetBundleNames().Length;
        Debug.Log(length);
    }
    static void Pack(string source)
    {
        DirectoryInfo folder = new DirectoryInfo(source);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        int length = files.Length;
        for (int i = 0; i < length; i++)
        {
            if (files[i] is DirectoryInfo)
            {
                Pack(files[i].FullName);
            }
            else
            {
                if (!files[i].Name.EndsWith(".meta"))
                {
                    file(files[i].FullName);
                }
            }
        }
    }

    static void file(string source)
    {
        string _source = Replace(source);
        string _assetPath = "Assets" + _source.Substring(Application.dataPath.Length);
        string _assetPath2 = _source.Substring(Application.dataPath.Length + 1);
        //Debug.Log (_assetPath);

        //在代码中给资源设置AssetBundleName
        AssetImporter assetImporter = AssetImporter.GetAtPath(_assetPath);
        string assetName = _assetPath2.Substring(_assetPath2.IndexOf("/") + 1);
        assetName = assetName.Replace(Path.GetExtension(assetName), ".unity3d");
        //Debug.Log (assetName);
        assetImporter.assetBundleName = assetName;
    }

    static string Replace(string s)
    {
        return s.Replace("\\", "/");
    }



    static void SetFileAssetName(string source)
    {
        string _source = Replace(source);
        if (IsScript(_source))
        {
            return;
        }
        SetBundleAndVariant(_source, _source);
    }

	static void SetBundleAndVariant(string _source, string bundleName, string variant = "")
    {
        AssetImporter assetImporter = AssetImporter.GetAtPath(_source);
        if (assetImporter == null)
        {
            Utils.LogSys.Log("Do Not Find: " + _source);
            return;
        }

        if (!string.IsNullOrEmpty(_source) && _source.Length > 6 && _source.Substring(_source.Length - 6) == ".unity")
        {
            string strSceneName = Path.GetFileNameWithoutExtension(_source);
            bundleName = "resources/levels/" + strSceneName + "/" + strSceneName + ".unity3d";
            assetImporter.assetBundleName = bundleName;
            return;
        }

        int iLength = @"Assets/".Length;
        if (_source.Length < iLength || _source.Substring(0, iLength) != "Assets/")
        {
            Utils.LogSys.Log("Do Not Create Assetbundle: " + _source);
            return;
        }
        if (bundleName.Length >= iLength && bundleName.Substring(0, iLength) == "Assets/")
        {
        	bundleName = bundleName.Substring(iLength);
        }
        //在代码中给资源设置AssetBundleName
        //Debug.Log (assetName);
        assetImporter.assetBundleName = bundleName;
        if (variant != "")
        {
			assetImporter.assetBundleVariant = variant;
			//assetImporter.assetPath = bundleName;
		}
	}

    static void RemoveAssetName(string source)
    {
        string _source = Replace(source);
        int iLength = @"Assets/".Length;
        if (_source.Length < iLength || _source.Substring(0, iLength) != "Assets/")
        {
            Utils.LogSys.Log("Do Not Create Assetbundle: " + _source);
            return;
        }
        _source = _source.Substring(iLength);
        AssetDatabase.RemoveAssetBundleName(_source, true);
    }
    

    /// <summary>
    /// 清空assetbundle names
    /// </summary>
    [MenuItem("FilePackage/清空assetbundle names")]
    public static void clearAllAssetbundlesName()
    {
        ResourceFileNameValidate.bSwitch = false;
        ClearAssetBundlesName();
        AssetDatabase.Refresh();
        Debug.Log("清空完成");
        ResourceFileNameValidate.bSwitch = true;
    }
    /// <summary>
    /// UI打包
    /// </summary>
//     [MenuItem("FilePackage/设置所有assetbundle_name")]
//     public static void collectAllAssetsPath()
//     {
//         collectUIAssetsPath();
//         collectEffectAssetsPath();
//         collectSceneAssetsPath();
//     }
    /// <summary>
    /// UI打包
    /// </summary>
    //[MenuItem("FilePackage/设置assetbundle_name->ui")]
    public static void collectUIAssetsPath()
    {
        ResourceFileNameValidate.bSwitch = false;
        string[] files = Directory.GetFiles("Assets/Resources/UI", "*.prefab", SearchOption.AllDirectories);

        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            Object obj = AssetDatabase.LoadMainAssetAtPath(strNormalPath);

            SetFileAssetName(strNormalPath);
            if (obj != null)
            {
                Object[] dependObjs = EditorUtility.CollectDependencies(new Object[] { obj });

                foreach (var dependObj in dependObjs)
                {
                    string strPath = AssetDatabase.GetAssetPath(dependObj);
                    if (strPath != strNormalPath )
                    {
                        if (strPath.IndexOf(".prefab") != -1 || strPath.IndexOf(".TTF") != -1)
                        {
                            SetFileAssetName(strPath);
                        }
                    }
                }
            }
        }


        #region 生成assetbundle文件

        ////打包
        //根据BuildSetting里面所激活的平台进行打包
        //BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/" + IPath.getPlatformName(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        ResourceFileNameValidate.bSwitch = true;

        AssetDatabase.Refresh();
        Debug.Log("设置assetbundle_name->ui完成");
        #endregion //生成assetbundle文件
        //collectAllAssetbundles();
    }

    /// <summary>
    /// Effect打包
    /// </summary>
    //[MenuItem("FilePackage/设置assetbundle_name->effects")]
    public static void collectEffectAssetsPath()
    {

        ResourceFileNameValidate.bSwitch = false;
        string[] files = Directory.GetFiles("Assets/Resources/Effects", "*.prefab", SearchOption.AllDirectories);

        bool needPackage = false;
        //计算索引
        foreach (string file in files)
        {
            needPackage = false;
            string strNormalPath = file.Replace("\\", "/");
            Object obj = AssetDatabase.LoadMainAssetAtPath(strNormalPath);

            SetFileAssetName(strNormalPath);
            if (obj != null)
            {
                Object[] dependObjs = EditorUtility.CollectDependencies(new Object[] { obj });

                foreach (var dependObj in dependObjs)
                {
                    string strPath = AssetDatabase.GetAssetPath(dependObj);
                    if (strPath != strNormalPath)
                    {
                        if (strPath.IndexOf(".prefab") != -1 )
                        {
                            SetFileAssetName(strPath);
                        }
                    }
                }
            }
        }

//         files = Directory.GetFiles("Assets/Resources/Prefabs", "*.prefab", SearchOption.AllDirectories);
// 
//         //计算索引
//         foreach (string file in files)
//         {
//             string strNormalPath = file.Replace("\\", "/");
//             Object obj = AssetDatabase.LoadMainAssetAtPath(strNormalPath);
// 
//             SetFileAssetName(strNormalPath);
//             if (obj != null)
//             {
//                 Object[] dependObjs = EditorUtility.CollectDependencies(new Object[] { obj });
// 
//                 foreach (var dependObj in dependObjs)
//                 {
//                     string strPath = AssetDatabase.GetAssetPath(dependObj);
//                     if (strPath != strNormalPath)
//                     {
//                         if (strPath.IndexOf(".prefab") != -1 )
//                         {
//                             SetFileAssetName(strPath);
//                         }
//                     }
//                 }
//             }
//         }
        #region 生成assetbundle文件

        ////打包
        //根据BuildSetting里面所激活的平台进行打包
        //BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/" + IPath.getPlatformName(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        ResourceFileNameValidate.bSwitch = true;

        AssetDatabase.Refresh();
        Debug.Log("设置assetbundle_name->effects完成");
        #endregion //生成assetbundle文件

    }

    /// <summary>
    /// XML打包
    /// </summary>
    //[MenuItem("FilePackage/设置assetbundle_name->xml")]
    public static void collectXMLAssetsPath()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Config", "*.xml", SearchOption.AllDirectories);

        ResourceFileNameValidate.bSwitch = false;
        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            //if (!EquilToOldFile(strNormalPath))
                SetFileAssetName(strNormalPath);
        }
        //SetFileAssetName("Assets/Resources/AssetsPathsInfo.xml");
        //CopyFileToStreamingAssets("/Resources/AssetsPathsInfo.xml");

        files = Directory.GetFiles("Assets/Resources/Config", "*.bytes", SearchOption.AllDirectories);
        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            //if (!EquilToOldFile(strNormalPath))
                SetFileAssetName(strNormalPath);
        }

        //加密
        EncryptAllXML();

        ResourceFileNameValidate.bSwitch = true;
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// uiTexture打包
    /// </summary>
    //[MenuItem("FilePackage/设置assetbundle_name->uiTexture")]
    public static void collectUITextureAssetsPath()
    {
        string[] files = Directory.GetFiles("Assets/Resources/UI/Texture", "*.png", SearchOption.AllDirectories);

        ResourceFileNameValidate.bSwitch = false;
        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            //if (!EquilToOldFile(strNormalPath))
                SetFileAssetName(strNormalPath);
        }
        ResourceFileNameValidate.bSwitch = true;
        AssetDatabase.Refresh();
    }
        /// <summary>
    /// sound打包
    /// </summary>
    //[MenuItem("FilePackage/设置assetbundle_name->sound")]
    public static void collectMusicAssetsPath()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Sounds", "*.mp3", SearchOption.AllDirectories);
        string[] oggfiles = Directory.GetFiles("Assets/Resources/Sounds", "*.ogg", SearchOption.AllDirectories);
        ResourceFileNameValidate.bSwitch = false;
        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            //if (!EquilToOldFile(strNormalPath))
            SetFileAssetName(strNormalPath);
        }
        foreach (string file in oggfiles)
        {
            string strNormalPath = file.Replace("\\", "/");
            //if (!EquilToOldFile(strNormalPath))
            SetFileAssetName(strNormalPath);
        }
        ResourceFileNameValidate.bSwitch = true;
        AssetDatabase.Refresh();
    }
    static void CopyFileToStreamingAssets(string filePath)
    {
        string fromPath = "Assets" + filePath;
        string toPath = "Assets/StreamingAssets/" + IPath.getPlatformName() + filePath.ToLower();
        if (System.IO.File.Exists(toPath))
        {
            System.IO.File.SetAttributes(toPath, FileAttributes.Normal);
            System.IO.File.Delete(toPath);
        }
        File.Copy(fromPath, toPath);
    }

    [MenuItem("FilePackage/一键生成界面assetbundle_name")]
    public static void collectUIAssetName()
    {
        collectUIAssetsPath();//UI界面

        collectEffectAssetsPath();//特效

        collectXMLAssetsPath();//配置

        collectUITextureAssetsPath();//uiTexture

        collectMusicAssetsPath();//声音
    }

    /// <summary>
    /// Models打包
    /// </summary>
    [MenuItem("FilePackage/一键生成模型assetbundle_name")]
	public static void collectModelsAssetsPath() {
        //clearAllAssetbundlesName();

        string[] files = Directory.GetFiles("Assets/Resources/Models", "*.*", SearchOption.AllDirectories);

		ResourceFileNameValidate.bSwitch = false;
        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            //SetFileAssetName(strNormalPath);
            if (file.Substring(file.Length - 5).Equals(".meta"))
            {

            }
            else if (file.Substring(file.Length-7).Equals(".prefab"))//泡泡鱼
            {
                Object obj = AssetDatabase.LoadMainAssetAtPath(strNormalPath);
                collectPrefabModel(obj);
            }
            else
            {
                //生成中间文件
                Object obj = AssetDatabase.LoadMainAssetAtPath(strNormalPath);//FBX鱼
                collectModel(obj);
            }
        }
        //SetFileAssetName("Assets/Resources/GameModelData/GameModelData.asset");
		ResourceFileNameValidate.bSwitch = true;
        AssetDatabase.Refresh();

        //collectAllAssetbundles();

        //clearAllAssetbundlesName();
	}

    /// <summary>
    /// Models打包
    /// </summary>
    //[MenuItem("FilePackage/打包选中的models")]
	public static void collectSelectModels()
	{
        //string[] files = Directory.GetFiles("Assets/Resources/Models", "*.*", SearchOption.AllDirectories);
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            collectModel(o);
        }
	}
    public static string ModelMiddleFilesPath
    {
        get{return "Assets/Resources/modelsassetbundles" + "/" + _strCurrentModelName + Path.DirectorySeparatorChar;}
    }
    public static string ModelAssetbundlePath
    {
        get{return IPath.streamingAssetsPathPlatform() + "/modelsassetbundles/" + _strCurrentModelName + Path.DirectorySeparatorChar;}
    }
    public static string ModelFBXPath
    {
        get{return "Assets/Resources/Models/" + _strCurrentModelName + ".FBX";}
    }
    public static string ModelAnimPath
    {
        get{return "Assets/Resources/Models/" + _strCurrentModelName + "@anim.FBX";}
    }

    static string CreatePrefab(GameObject go, string name, bool needSetBundleName = true)
    {
    	string path = "Assets/Resources/modelsassetbundles/" + _strCurrentModelName + "/" + name + ".prefab";
        Object tempPrefab = PrefabUtility.CreateEmptyPrefab(path);
        tempPrefab = PrefabUtility.ReplacePrefab(go, tempPrefab);
        Object.DestroyImmediate(go);
        if (needSetBundleName)
        	SetFileAssetName(path);
        return path;
    }
    static string CreateAsset(StringHolder go, string name, bool bNeedAssetbundle = true)
    {
    	string path = "Assets/Resources/modelsassetbundles/" + _strCurrentModelName + "/" + name + ".asset";
		AssetDatabase.CreateAsset(go, path);
        if (bNeedAssetbundle)
            SetFileAssetName(path);
		return path;
    }

    public static void collectPrefabModel(Object o)
    {
        if (!(o is GameObject)) return;
        if (o.name.Contains("@")) return; //animations is processed seperated later
        if (!AssetDatabase.GetAssetPath(o).Contains("/Models/")) return;

        GameObject characterPrefab = (GameObject)o;
        string name = characterPrefab.name;
        _strCurrentModelName = name;
        Debug.Log("******* Creating assetbundles for: " + name + " *******");
        // Create a directory to store the middle files.
        if (!Directory.Exists(ModelMiddleFilesPath))
            Directory.CreateDirectory(ModelMiddleFilesPath);
        // Delete existing assetbundles for current character.
        string[] existingAssetbundles = Directory.GetFiles(ModelMiddleFilesPath);
        foreach (string bundle in existingAssetbundles)
        {
            File.Delete(bundle);
        }

        GameObject characterClone = (GameObject)Object.Instantiate(characterPrefab);
        string characterBasePrefab = CreatePrefab(characterClone, "base");
    }

    public static void collectModel(Object o)
	{
        if (!(o is GameObject)) return;
        if (o.name.Contains("@")) return; //animations is processed seperated later
        if (!AssetDatabase.GetAssetPath(o).Contains("/Models/")) return;

        GameObject characterFBX = (GameObject)o;
        string name = characterFBX.name;
        _strCurrentModelName = name;

        Debug.Log("******* Creating assetbundles for: " + name + " *******");

        // Create a directory to store the middle files.
        if (!Directory.Exists(ModelMiddleFilesPath))
            Directory.CreateDirectory(ModelMiddleFilesPath);
        // Create a directory to store the generated assetbundles.
        //if (!Directory.Exists(ModelAssetbundlePath))
        //    Directory.CreateDirectory(ModelAssetbundlePath);


        // Delete existing assetbundles for current character.
        string[] existingAssetbundles = Directory.GetFiles(ModelMiddleFilesPath);
        foreach (string bundle in existingAssetbundles) {
                File.Delete(bundle);
        }
        
        // Save bones and animations to a seperate assetbundle. Any 
        // possible combination of CharacterElements will use these
        // assets as a base. As we can not edit assets we instantiate
        // the fbx and remove what we dont need. As only assets can be
        // added to assetbundles we save the result as a prefab and delete
        // it as soon as the assetbundle is created.
        GameObject characterClone = (GameObject)Object.Instantiate(characterFBX);

        // postprocess animations: we need them animating even offscreen
        foreach (Animation anim in characterClone.GetComponentsInChildren<Animation>())
            anim.cullingType = AnimationCullingType.BasedOnRenderers;

        foreach (SkinnedMeshRenderer smr in characterClone.GetComponentsInChildren<SkinnedMeshRenderer>())
            Object.DestroyImmediate(smr.gameObject);

        characterClone.AddComponent<SkinnedMeshRenderer>();
        //characterClone.materials = 
        bool bNeedAssetbundle = true;
//         if (_strCurrentModelName.Equals("13013") || _strCurrentModelName.Equals("13016") || _strCurrentModelName.Equals("13017") || _strCurrentModelName.Equals("13018") || _strCurrentModelName.Equals("13019"))
//         {
//             bNeedAssetbundle = false;
//         }
        if (IsSkinModelID(_strCurrentModelName))
        {
            bNeedAssetbundle = false;
        }
        string characterBasePrefab = CreatePrefab(characterClone, "base", bNeedAssetbundle);
        // Create assetbundles for each SkinnedMeshRenderer.
        foreach (SkinnedMeshRenderer smr in characterFBX.GetComponentsInChildren<SkinnedMeshRenderer>(true)) {
        	
            // Save the current SkinnedMeshRenderer as a prefab so it can be included
            // in the assetbundle. As instantiating part of an fbx results in the
            // entire fbx being instantiated, we have to dispose of the entire instance
            // after we detach the SkinnedMeshRenderer in question.
        	//List<string> element_path = new List<string>();
            GameObject rendererClone = (GameObject)PrefabUtility.InstantiatePrefab(smr.gameObject);
            GameObject rendererParent = rendererClone.transform.parent.gameObject;
            rendererClone.transform.parent = null;
            Object.DestroyImmediate(rendererParent);
            
            //置空材质球
            var mats = new Material[1];
            mats[0] = Resources.Load("Materials/" + _strCurrentModelName + "/" + smr.name) as Material;
            rendererClone.GetComponent<SkinnedMeshRenderer>().materials = mats;

            string rendererPrefab = CreatePrefab(rendererClone, smr.name, false);
            if (bNeedAssetbundle)
			    SetBundleAndVariant(rendererPrefab,rendererPrefab);
            Object obj = AssetDatabase.LoadMainAssetAtPath(rendererPrefab);
            
            //element_path.Add(rendererPrefab);
            //toinclude.Add(rendererPrefab);

            // Don't Collect applicable materials, as the elements materials is constantly 
            // changing in different level. 
#if COLLECT_ELEMENTS_MATERIALS
                foreach (Material m in materials)
                {
                    //if (m.name.Contains(smr.name)) toinclude.Add(m);
                }
#endif

            // When assembling a character, we load SkinnedMeshRenderers from assetbundles,
            // and as such they have lost the references to their bones. To be able to
            // remap the SkinnedMeshRenderers to use the bones from the characterbase assetbundles,
            // we save the names of the bones used.
            List<string> boneNames = new List<string>();
            foreach (Transform t in smr.bones)
                boneNames.Add(t.name);
            StringHolder holder = ScriptableObject.CreateInstance<StringHolder>();
            holder.content = boneNames.ToArray();
            string stringholderpath = CreateAsset(holder, smr.name + "bonenames", bNeedAssetbundle);
            if (bNeedAssetbundle)
                SetBundleAndVariant(stringholderpath,rendererPrefab);

        }
    }


    /// <summary>
    /// Scene打包
    /// </summary>
    //[MenuItem("FilePackage/设置assetbundle_name->scene")]
    public static void collectSceneAssetsPath()
    {

        ResourceFileNameValidate.bSwitch = false;

        string[] files = Directory.GetFiles("Assets/Resources/Levels/ScenesCommonRes", "*.prefab", SearchOption.AllDirectories);
        
        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            Object obj = AssetDatabase.LoadMainAssetAtPath(strNormalPath);
            SetFileAssetName(strNormalPath);
            if (obj != null)
            {
                Object[] dependObjs = EditorUtility.CollectDependencies(new Object[] { obj });

                foreach (var dependObj in dependObjs)
                {
                    string strPath = AssetDatabase.GetAssetPath(dependObj);
                    if (strPath != strNormalPath)
                    {
                        if (strPath.IndexOf(".prefab") != -1 || strPath.IndexOf(".TTF") != -1)
                        {
                            SetFileAssetName(strPath);
                        }
                    }
                }
            }
        }


        files = Directory.GetFiles("Assets/Resources/Levels", "*.unity", SearchOption.AllDirectories);
        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            Object obj = AssetDatabase.LoadMainAssetAtPath(strNormalPath);
            SetFileAssetName(strNormalPath);
            if (obj != null)
            {
                Object[] dependObjs = EditorUtility.CollectDependencies(new Object[] { obj });

                foreach (var dependObj in dependObjs)
                {
                    string strPath = AssetDatabase.GetAssetPath(dependObj);
                    if (strPath != strNormalPath)
                    {
                        if (strPath.IndexOf(".prefab") != -1 )
                        {
                            SetFileAssetName(strPath);
                        }
                    }
                }
            }
        }

//         files = Directory.GetFiles("Assets/Resources/Levels", "*.unity", SearchOption.AllDirectories);
//         //计算索引
//         foreach (string file in files)
//         {
//             SetFileAssetName(file);
//         }
        #region 生成assetbundle文件

        ////打包
        //根据BuildSetting里面所激活的平台进行打包
        //BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/" + IPath.getPlatformName(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        ResourceFileNameValidate.bSwitch = true;

        AssetDatabase.Refresh();
        Debug.Log("设置assetbundle_name->scene完成");
        #endregion //生成assetbundle文件
    }


    public static void BuildAllMapAssetbendle()
    {
        List<string> sceneNames = SearchFiles("Assets/Resources/Levels/", "*.unity");
        foreach (string f in sceneNames)
        {
            //BuildAssetBundle(f);
        }
    }

    /// <summary>
    /// lua打包
    /// </summary>
    //[MenuItem("FilePackage/打包assetbundle->lua")]
    public static void collectLuaAssetsPath()
    {

        string[] files = Directory.GetFiles("Assets/luaScripts", "*.lua", SearchOption.AllDirectories);

        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
        buildMap[0].assetBundleName = "lua.data";
        string[] enemyAssets = new string[files.Length];
        int count = 0;
        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            strNormalPath = file.Replace(".lua", ".bytes");
            enemyAssets[count] = strNormalPath;
            count++;
        }
        buildMap[0].assetNames = enemyAssets;
        EncryptAllLua();
        AssetDatabase.Refresh();
        // Create the array of bundle build details.
        BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/" + IPath.getPlatformName(), buildMap, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);


        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            strNormalPath = file.Replace(".lua", ".bytes");
            if (System.IO.File.Exists(strNormalPath))
            {
                System.IO.File.SetAttributes(strNormalPath, FileAttributes.Normal);
                System.IO.File.Delete(strNormalPath);
            }
        }
        Debug.Log("lua打包完成");
    }

    /// <summary>
    /// 打包Assetbundle
    /// </summary>
    [MenuItem("FilePackage/打包所有Assetbundle")]
    public static void collectAllAssetbundles()
    {
        collectLuaAssetsPath();
        ResourceFileNameValidate.bSwitch = false;

        string direName = "Assets/StreamingAssets/" + IPath.getPlatformName();
        //根据BuildSetting里面所激活的平台进行打包
        if (!System.IO.Directory.Exists(direName))
        {
            System.IO.Directory.CreateDirectory(direName);
        }
        BuildPipeline.BuildAssetBundles(direName, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        Debug.Log("打包完成");
        AssetDatabase.Refresh();
        createAssetsLocaltionFile();//AssetsPathsInfo.xml重新生成
        Debug.Log("AssetsPathsInfo.xml重新生成");
        //SetFileAssetName("Assets/Resources/AssetsPathsInfo.xml");
        //根据BuildSetting里面所激活的平台进行打包
        //BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/" + IPath.getPlatformName(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        //Debug.Log("AssetsPathsInfo.xml打包完成");

        ResourceFileNameValidate.bSwitch = true;
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 打包Assetbundle
    /// </summary>
    //[MenuItem("FilePackage/打包场景")]
    public static void collectSceneAssetbundles()
    {
        ResourceFileNameValidate.bSwitch = false;
        //根据BuildSetting里面所激活的平台进行打包
        string[] levels = new string[] { "Assets/Resources/Levels/MainCity/mainScene.unity" };
        string targetPath = "Assets/StreamingAssets/" + IPath.getPlatformName() + "/resources/levels/mainscene/mainscene.unity3d";
        BuildPipeline.BuildStreamedSceneAssetBundle(levels, targetPath, EditorUserBuildSettings.activeBuildTarget);
//         string[] levels2 = new string[] { "Assets/Resources/Levels/JiuGuang/JiuGuang.unity" };
//         string targetPath2 = "Assets/StreamingAssets/" + IPath.getPlatformName() + "/resources/levels/jiuguang/jiuguang.unity3d";
//         BuildPipeline.BuildStreamedSceneAssetBundle(levels2, targetPath2, EditorUserBuildSettings.activeBuildTarget, BuildOptions.UncompressedAssetBundle);
        AssetDatabase.Refresh();
        ResourceFileNameValidate.bSwitch = true;
        Debug.Log("打包场景完成");
    }

    /// <summary>
    /// 如果是脚本或mata文件等，不需要打包的资源返回true
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    static bool IsScript(string file)
    {
        for (int i = 0; i < _scriptFileType.Length; i++ )
        {
            if (file.Length > _scriptFileTypeLength[i] && file.Substring(file.Length - _scriptFileTypeLength[i]) == _scriptFileType[i])
            {
	            if (file.Contains("AnimatorController.cs"))
	            {
	            	int k=0;
	            }
                return true;
            }
        }

        string path = file.ToLower();
        if (path.IndexOf("/resources/ui/texture") != -1)//该目录下的png都会被打包成assetbundle
        {
            if (file.Length > 4 && file.Substring(file.Length - 4) == ".png")
            {
                return false;
            }
        }
        if (path.IndexOf("/resources/fonts/") != -1)
        {
            if (file.Length > 4 && file.Substring(file.Length - 4) == ".mat")
            {
                return true;
            }
            if (file.Length > 4 && file.Substring(file.Length - 4) == ".png")
            {
                return true;
            }
            if (file.Length > 4 && file.Substring(file.Length - 4) == ".txt")
            {
                return true;
            }
        }
        if (path.IndexOf("/resources/ui/") != -1)
        {
            if (file.Length > 4 && file.Substring(file.Length - 4) == ".mat")
            {
                return false;
            }
            if (file.Length > 4 && file.Substring(file.Length - 4) == ".png")
            {
                return true;
            }
            if (file.Length > 4 && file.Substring(file.Length - 4) == ".txt")
            {
                return true;
            }
        }
        return false;
    }
    static List<string> SearchFiles(string dir, string pattern)
    {
        List<string> sceneNames = new List<string>();
        foreach (string f in Directory.GetFiles(dir, pattern, SearchOption.AllDirectories))
        {
            sceneNames.Add(f);
        }
        return sceneNames;
    }

    static bool EquilToOldFile(string filePath)
    {
        string sOldAssetPath = PakageSourceVersion.GetBakeUpAssetsPath();
        if (File.Exists(sOldAssetPath + filePath))
        {
            try
            {
                FileStream _stream_new = File.Open(filePath, FileMode.Open, FileAccess.Read);
                byte[] byte_new = new byte[_stream_new.Length];
                _stream_new.Read(byte_new, 0, byte_new.Length);
                _stream_new.Close();
                string md5_new = UtilTools.GetFileMD5(byte_new);

                FileStream _stream_old = File.Open(sOldAssetPath + filePath, FileMode.Open, FileAccess.Read);
                byte[] byte_old = new byte[_stream_old.Length];
                _stream_old.Read(byte_old, 0, byte_old.Length);
                _stream_old.Close();
                string md5_old = UtilTools.GetFileMD5(byte_old);

                if (md5_new != md5_old)
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                Utils.LogSys.LogError("Compare To Old File Error, Can't read: " + sOldAssetPath + filePath);
            }
        }
        else
        {
            return false;
        }
        return true;
        
    }

    /// <summary>
    /// 相同时反回true, 不存在和不同时返回false
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="oldVersion"></param>
    /// <returns></returns>

    static bool EquilToOldAssetBundle(string filePath, string oldVersion)
    {
        string sOldAssetPath = PakageSourceVersion.GetBakeUpAssetbundlePath();
        sOldAssetPath += oldVersion + "/";
//         while (filePath.IndexOf("\\") != -1)
//         {
//             filePath = filePath.Replace("\\", "/");
//         }
        if (File.Exists(sOldAssetPath + filePath))
        {
            try
            {
                FileStream _stream_new = File.Open(filePath, FileMode.Open, FileAccess.Read);
                byte[] byte_new = new byte[_stream_new.Length];
                _stream_new.Read(byte_new, 0, byte_new.Length);
                _stream_new.Close();
                string md5_new = UtilTools.GetFileMD5(byte_new);

                FileStream _stream_old = File.Open(sOldAssetPath + filePath, FileMode.Open, FileAccess.Read);
                byte[] byte_old = new byte[_stream_old.Length];
                _stream_old.Read(byte_old, 0, byte_old.Length);
                _stream_old.Close();
                string md5_old = UtilTools.GetFileMD5(byte_old);

                if (md5_new != md5_old)
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                Utils.LogSys.LogError("Compare To Old File Error, Can't read: " + sOldAssetPath + filePath);
            }
        }
        else
        {
            return false;
        }
        return true;

    }

    [MenuItem("FilePackage/自动生成更新包")]
    static void AutoCreateSourceVersionPakage()
    {
        PakageSourceVersion.ShowWindow();
    }

    [MenuItem("FilePackage/打包所有特效")]
    static void ExportAllEffects()
    {
        if (Selection.objects == null) return;
        List<string> paths = new List<string>();
        string[] files = Directory.GetFiles("Assets/Resources/Effects", "*.prefab", SearchOption.AllDirectories);
        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            paths.Add(strNormalPath);
        }

        AssetDatabase.ExportPackage(paths.ToArray(), "AllEffects.unitypackage", ExportPackageOptions.IncludeDependencies);
        AssetDatabase.Refresh();
        Debug.Log("Build all Done!");
    }


    //生成加密后的lua中间文件->后面会全部打包到lua.data中
    //[MenuItem("FilePackage/加密所有Lua")]
    static void EncryptAllLua()
    {
        if (!System.IO.Directory.Exists("Assets/luaScripts"))
        {
            return;
        }
        
        string toFilePath = "";
        CMyEncryptFile encrypt = new CMyEncryptFile();
        string[] files = Directory.GetFiles("Assets/luaScripts", "*.lua", SearchOption.AllDirectories);

        ResourceFileNameValidate.bSwitch = false;

        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            byte[] bytes = File.ReadAllBytes(strNormalPath);
            if (bytes != null && bytes.Length > 0 )
            {
                if (!UtilTools.ArrayHeadIsWoDong(bytes))
                {
                    bytes = encrypt.Encrypt(bytes, bytes.Length);
                }
                toFilePath = strNormalPath.Replace(".lua", ".bytes");
                if (System.IO.File.Exists(toFilePath))
                {
                    System.IO.File.SetAttributes(toFilePath, FileAttributes.Normal);
                    System.IO.File.Delete(toFilePath);
                }
                File.WriteAllBytes(toFilePath, bytes);
                Debug.Log("加密文件: " + strNormalPath);
            }
        }
        
    }

    [MenuItem("FilePackage/打包工具")]
    static void AutoPakageAPK()
    {
        AutoPackageAPK.ShowWindow();
    }

    //[MenuItem("FilePackage/LuaFtp上传工具")]
    
    static void CreateLuaFTPFile()
    {
        LuaFtpEditorWindow window = (LuaFtpEditorWindow)EditorWindow.GetWindow(typeof(LuaFtpEditorWindow));
        window.position = new Rect(960, 540, 515, 135);
        string versionPath = sdk.SDKManager.LuaHotScriptURL.Replace("http://ksyx.update.iwodong.com", "");
        versionPath = versionPath.Replace("luaCode/", "");
        window.initialize(versionPath);
    }

    private static void moveFile(string copyFile, string endFilePath)
    {
        string direName = Path.GetDirectoryName(endFilePath);
        if (!System.IO.Directory.Exists(direName))
        {
            System.IO.Directory.CreateDirectory(direName);
        }
        System.IO.File.Copy(copyFile, endFilePath);
    }

    //[MenuItem("FilePackage/加密所有XML")]
    static void EncryptAllXML()
    {
    	if (!System.IO.Directory.Exists("Assets/Resources/Config/export_xml"))
    	{
    		return;
    	}
        CMyEncryptFile encrypt = new CMyEncryptFile();
        string[] files = Directory.GetFiles("Assets/Resources/Config/export_xml", "*.bytes", SearchOption.AllDirectories);

        ResourceFileNameValidate.bSwitch = false;
        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            byte[] bytes = File.ReadAllBytes(strNormalPath);
            if (!UtilTools.ArrayHeadIsWoDong(bytes))
            {
                //strNormalPath = strNormalPath.Replace(".xml", ".bytes");
                bytes = encrypt.Encrypt(bytes, bytes.Length);
                //string Config = System.Text.Encoding.UTF8.GetString(bytes);
                //File.WriteAllText(strNormalPath, Config, Encoding.UTF8);
                File.WriteAllBytes(strNormalPath, bytes);
                Debug.Log("加密文件: " + strNormalPath);
            }

        }

    }

    static void DeleteUnsedUIPng()
    {
    	if (!System.IO.Directory.Exists("Assets/Resources/UI"))
    	{
    		return;
    	}
        string[] files = Directory.GetFiles("Assets/Resources/UI", "*.png", SearchOption.AllDirectories);
        //计算索引
        foreach (string file in files)
        {
            if (file.IndexOf("_RGB.png") != -1)
            {
                string alpha_png = file.Replace("_RGB", "_Alpha");
                string base_png = file.Replace("_RGB", "");

                if (System.IO.File.Exists(alpha_png) && System.IO.File.Exists(base_png))
                {
                    string meta_png = base_png + ".meta";
                    System.IO.File.SetAttributes(base_png, FileAttributes.Normal);
                    System.IO.File.Delete(base_png);
                    if (System.IO.File.Exists(meta_png))
                    {
                        System.IO.File.SetAttributes(meta_png, FileAttributes.Normal);
                        System.IO.File.Delete(meta_png);
                    }
                    Debug.Log("Delete UI PNG: " + base_png);
                }
            }
            if (System.IO.File.Exists(file))
            {
#if UNITY_ANDROID
                TextureImporter textureImporter = AssetImporter.GetAtPath(file) as TextureImporter;
                if (textureImporter.textureType != TextureImporterType.Advanced)
                {
                    textureImporter.textureType = TextureImporterType.Advanced;
                    textureImporter.npotScale = TextureImporterNPOTScale.ToNearest;
                    textureImporter.isReadable = false;
                    textureImporter.alphaIsTransparency = false;
                    textureImporter.spriteImportMode = SpriteImportMode.None;

                    textureImporter.mipmapEnabled = false;
                    textureImporter.filterMode = FilterMode.Bilinear;
                    textureImporter.anisoLevel = 0;

                    textureImporter.SetPlatformTextureSettings("Android", 1024, TextureImporterFormat.ETC2_RGB4, 1, false);
                    Debug.Log("Reset Image Ispector: " + file);
                }
                else
                {
                    int maxSize;
                    TextureImporterFormat format;
                    bool hasOverride = textureImporter.GetPlatformTextureSettings("Android", out maxSize, out format);
                    //如果已经Advanced, 但是还没override
                    if (!hasOverride)
                    {
                        textureImporter.SetPlatformTextureSettings("Android", 1024, TextureImporterFormat.ETC2_RGB4, 1, false);
                    }
                    else if ( format != TextureImporterFormat.ETC2_RGB4)
                    {
                        Debug.LogWarning("Image format is not ETC2_RGB4: " + file);
                    }
                }
#endif
#if UNITY_IOS
                TextureImporter textureImporter = AssetImporter.GetAtPath(file) as TextureImporter;
                if (textureImporter.textureType != TextureImporterType.Advanced)
                {
                    textureImporter.textureType = TextureImporterType.Advanced;
                    textureImporter.npotScale = TextureImporterNPOTScale.ToNearest;
                    //textureImporter.mapping
                    textureImporter.isReadable = false;
                    textureImporter.alphaIsTransparency = false;
                    textureImporter.spriteImportMode = SpriteImportMode.None;

                    textureImporter.mipmapEnabled = false;
                    textureImporter.filterMode = FilterMode.Bilinear;
                    textureImporter.anisoLevel = 0;

                    textureImporter.SetPlatformTextureSettings("iPhone", 1024, TextureImporterFormat.PVRTC_RGB4, 1, false);
                }
                else
                {
                    int maxSize;
                    TextureImporterFormat format;
                    bool hasOverride = textureImporter.GetPlatformTextureSettings("iPhone", out maxSize, out format);
                    if (!hasOverride)
                    {
                         //如果已经Advanced, 但是还没override
                        textureImporter.SetPlatformTextureSettings("iPhone", 1024, TextureImporterFormat.PVRTC_RGB4, 1, false);
                    }
                    else if (format != TextureImporterFormat.PVRTC_RGB4)
                    {
                        Debug.LogWarning("Image format is not PVRTC_RGB4: " + file);
                    }
                }
#endif
            }
        }
        AssetDatabase.Refresh();
    }

	public static void DeleteUnsedManifest()
	{
    	if (!System.IO.Directory.Exists("Assets/StreamingAssets"))
    	{
    		return;
    	}
        string[] files = Directory.GetFiles("Assets/StreamingAssets", "*.manifest", SearchOption.AllDirectories);
        //计算索引
        foreach (string file in files)
        {
            if (!file.Contains("StreamingAssets/ios.manifest") && !file.Contains("StreamingAssets/android.manifest"))
            {
                System.IO.File.SetAttributes(file, FileAttributes.Normal);
                System.IO.File.Delete(file);
			}
        }
	}

    
    public static void DeleteUnsedResource()
    {

        //删除动作
        if (System.IO.Directory.Exists("Assets/Animation"))
        {
            System.IO.Directory.Delete("Assets/Animation", true);
        }
        
        //删除perfAssist的Demo
        if (System.IO.Directory.Exists("Assets/PerfAssist/PA_TableView_Demo"))
        {
            System.IO.Directory.Delete("Assets/PerfAssist/PA_TableView_Demo", true);
        }
        if (System.IO.Directory.Exists("Assets/PerfAssist/ResourceTracker_Demo"))
        {
            System.IO.Directory.Delete("Assets/PerfAssist/ResourceTracker_Demo", true);
        }

        //删除NGUI的Demo
        if (System.IO.Directory.Exists("Assets/NGUI/Editor"))
        {
            System.IO.Directory.Delete("Assets/NGUI/Editor", true);
        }

        string[] files;
        //删除配置
        if (System.IO.Directory.Exists("Assets/Resources/Config"))
        {
            //System.IO.Directory.Delete("Assets/Resources/Config", true);

            files = Directory.GetDirectories("Assets/Resources/Config", "*.*", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                string strNormalPath = file.Replace("\\", "/");
                System.IO.Directory.Delete(strNormalPath, true);
            }
        }
        if (System.IO.Directory.Exists("Assets/Resources/Config"))
        {
            files = Directory.GetFiles("Assets/Resources/Config", "*.*", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                string strNormalPath = file.Replace("\\", "/");
                if (!strNormalPath.Contains("sensitivewords.txt"))
                {
                    System.IO.File.SetAttributes(strNormalPath, FileAttributes.Normal);
                    System.IO.File.Delete(strNormalPath);
                }
            }
        }

        
        //删除光效
        if (System.IO.Directory.Exists("Assets/Resources/Effects"))
        {
            files = Directory.GetDirectories("Assets/Resources/Effects", "*.*", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                string strNormalPath = file.Replace("\\", "/");
                if (strNormalPath.Contains("Resources/Effects/shader"))
                    continue;

                System.IO.Directory.Delete(strNormalPath, true);

            }
        }

        if (System.IO.Directory.Exists("Assets/Resources/Effects"))
        {
            files = Directory.GetFiles("Assets/Resources/Effects", "*.*", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                string strNormalPath = file.Replace("\\", "/");

                System.IO.File.SetAttributes(strNormalPath, FileAttributes.Normal);
                System.IO.File.Delete(strNormalPath);
            }
        }

        //删除字体
        if (System.IO.Directory.Exists("Assets/Resources/Fonts"))
        {
            files = Directory.GetFiles("Assets/Resources/Fonts", "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string strNormalPath = file.Replace("\\", "/");
                if (strNormalPath.Contains("FZY4JW") || strNormalPath.Contains("FZY4"))
                    continue;

                System.IO.File.SetAttributes(strNormalPath, FileAttributes.Normal);
                System.IO.File.Delete(strNormalPath);
            }
        }

        //删除模型相关
        if (System.IO.Directory.Exists("Assets/Resources/GameModelData"))
        {
            System.IO.Directory.Delete("Assets/Resources/GameModelData", true);
        }
        if (System.IO.Directory.Exists("Assets/Resources/Models"))
        {
            System.IO.Directory.Delete("Assets/Resources/Models", true);
        }
        if (System.IO.Directory.Exists("Assets/Resources/modelsassetbundles"))
        {
            System.IO.Directory.Delete("Assets/Resources/modelsassetbundles", true);
        }
        if (System.IO.Directory.Exists("Assets/Resources/Materials"))
        {
            files = Directory.GetDirectories("Assets/Resources/Materials", "*.*", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                string strNormalPath = file.Replace("\\", "/");
                System.IO.Directory.Delete(strNormalPath, true);
            }
        }
        if (System.IO.Directory.Exists("Assets/Resources/Textures"))
        {
            files = Directory.GetDirectories("Assets/Resources/Textures", "*.*", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                string strNormalPath = file.Replace("\\", "/");
                string[] dirs = strNormalPath.Split(new char[] { '/'});
                string model_id = dirs[dirs.Length - 1];
//                 if (!strNormalPath.Contains("/13013") && !strNormalPath.Contains("/13016") && !strNormalPath.Contains("/13017") && !strNormalPath.Contains("/13018") && !strNormalPath.Contains("/13019"))
//                 {
//                     System.IO.Directory.Delete(strNormalPath, true);
//                 }
                if (!IsSkinModelID(model_id))
                {
                    System.IO.Directory.Delete(strNormalPath, true);
                }
            }
        }

        //删除UI
        if (System.IO.Directory.Exists("Assets/Resources/UI"))
        {
            System.IO.Directory.Delete("Assets/Resources/UI", true);
        }

        //删除luaScripts
        if (System.IO.Directory.Exists("Assets/luaScripts"))
        {
            System.IO.Directory.Delete("Assets/luaScripts", true);
        }

        //删除window
        if (System.IO.Directory.Exists("Assets/StreamingAssets/windows"))
        {
            System.IO.Directory.Delete("Assets/StreamingAssets/windows", true);
            System.IO.File.SetAttributes("Assets/StreamingAssets/windows.meta", FileAttributes.Normal);
            System.IO.File.Delete("Assets/StreamingAssets/windows.meta");
        }
          //删除声音
        if (System.IO.Directory.Exists("Assets/Resources/Sounds"))
        {
            System.IO.Directory.Delete("Assets/Resources/Sounds", true);
        }
    }

//     public static void MoveLuaFile()
//     {
//         string sourceFilePath = Application.dataPath + "/luaScripts";
//         string toFilePath = Application.streamingAssetsPath + "/" + IPath.getPlatformName() + "/luaScripts";
//         if (System.IO.Directory.Exists(sourceFilePath) && System.IO.Directory.GetFiles(sourceFilePath).Length > 0)
//         {
//             if (System.IO.Directory.Exists(toFilePath))
//             {
//                 System.IO.Directory.Delete(toFilePath);
//             }
//             System.IO.Directory.Move(sourceFilePath, toFilePath);
//         }
//     }
    ///////////////////////////////////////////////////////////////////////////////////

    [MenuItem("FilePackage/一键处理版本信息")]
    public static void OneKeyVersionInfo()
    {
        if (Application.dataPath.Contains("PokerGame.proj"))
        {
            EncryptAllXML();//加密bytes
            DeleteUnsedUIPng();//删除无用png, 并设置_RGB和_Alpha的图片格式
            DeleteUnsedManifest();//删除无用manifesst文件
            DeleteUnsedResource();//删除无用Resource
            createAssetsLocaltionFile();//生成文件位置信息
        }
    }

    static bool IsSkinModelID(string modelID)
    {
        if (modelID.Equals("13013") || modelID.Equals("13016") || modelID.Equals("13017") || modelID.Equals("13018") || modelID.Equals("13019"))
        {
            return true;
        }
        return false;
    }

    [MenuItem("Assets/SelectPrefabsDependIt")]
    static void SelectAllPrefabDependenceIt()
    {
        Object obj = Selection.activeObject;
        string[] files = Directory.GetFiles("Assets/Resources", "*.prefab", SearchOption.AllDirectories);
        List<Object> _to_select = new List<Object>();

        //计算索引
        foreach (string file in files)
        {
            Object objTemp = AssetDatabase.LoadMainAssetAtPath(file);
            Object[] dependObjs = EditorUtility.CollectDependencies(new Object[] { objTemp });
            for (int i = 0; i < dependObjs.Length; i++)
            {
                if (dependObjs[i] == obj)
                {
                    if (!_to_select.Contains(objTemp))
                        _to_select.Add(objTemp);
                }
            }
        }
        Selection.objects = _to_select.ToArray();
        Utils.LogSys.Log("there are " + _to_select.Count.ToString() + " prefabs depend it.");
    }

    [MenuItem("FilePackage/查找Lua差异文件")]
    static void FindChangeLua()
    {
        if (!System.IO.Directory.Exists("Assets/luaScripts"))
        {
            return;
        }

        string toFilePath = Application.streamingAssetsPath + "/" + IPath.getPlatformName() + "/luaScripts";

        CMyEncryptFile encrypt = new CMyEncryptFile();
        string[] files = Directory.GetFiles("Assets/luaScripts", "*.lua", SearchOption.AllDirectories);

        ResourceFileNameValidate.bSwitch = false;
        Dictionary<string, string> fileCodes = new Dictionary<string, string>();
        Dictionary<string, string> filePaths = new Dictionary<string, string>();
        Dictionary<string, byte[]> fileBytes = new Dictionary<string, byte[]>();
        //计算索引
        foreach (string file in files)
        {
            string strNormalPath = file.Replace("\\", "/");
            string fileName = strNormalPath.Substring(0, strNormalPath.Length - 4);
            fileName = fileName.Replace("Assets/", "");
            byte[] bytes = File.ReadAllBytes(strNormalPath);
            if (bytes != null && bytes.Length > 0 && !UtilTools.ArrayHeadIsWoDong(bytes))
            {
                bytes = encrypt.Encrypt(bytes, bytes.Length);
                strNormalPath = strNormalPath.Replace("Assets/luaScripts", toFilePath);
                //string direName = Path.GetDirectoryName(strNormalPath);
                //if (!System.IO.Directory.Exists(direName))
                //{
                //    System.IO.Directory.CreateDirectory(direName);
                //}
                //File.WriteAllBytes(strNormalPath, bytes);
                filePaths.Add(fileName, strNormalPath);
                fileBytes.Add(fileName, bytes);
                fileCodes.Add(fileName, TextUtils.MD5(bytes));
                Debug.Log("加密文件: " + fileName);
            }
        }

        string fileCodePath = Application.streamingAssetsPath + "/" + IPath.getPlatformName();
        string diffPath = fileCodePath + "/diff";
        if (!System.IO.Directory.Exists(diffPath))
        {
            System.IO.Directory.CreateDirectory(diffPath);
        }


        if (!System.IO.File.Exists(fileCodePath + "/fileCode.lua"))
        {
            StreamWriter sWriter = new StreamWriter(fileCodePath + "/fileCode.lua", false, Encoding.UTF8);
            sWriter.WriteLine("___files_code___ = {}\n");
            sWriter.WriteLine("___files_code___[\"_CHECK\"] = 1\n");

            foreach (var key in fileCodes.Keys)
            {
                string value = fileCodes[key];
                sWriter.WriteLine("___files_code___[\"" + key + "\"] = \"" + value + "\"\n");
            }
            sWriter.Close();
        }
        else
        {
            StreamReader sReader = new StreamReader(fileCodePath + "/fileCode.lua", Encoding.UTF8);
            if (sReader != null)
            {
                string text_all = sReader.ReadToEnd();
                sReader.Close();
                StreamWriter sWriter = new StreamWriter(diffPath + "/fileCode.lua", false, Encoding.UTF8);
                sWriter.WriteLine("___files_code___ = {}\n");
                sWriter.WriteLine("___files_code___[\"_CHECK\"] = 1\n");
                foreach (var key in fileCodes.Keys)
                {
                    string value = fileCodes[key];
                    string newText = "___files_code___[\"" + key + "\"] = \"" + value + "\"\n";
                    if (text_all.IndexOf(newText) < 0)
                    {
                        sWriter.WriteLine(newText);
                        byte[] bytes = fileBytes[key];
                        string strNormalPath = filePaths[key];
                        string direName = Path.GetDirectoryName(strNormalPath);
                        if (!System.IO.Directory.Exists(direName))
                        {
                            System.IO.Directory.CreateDirectory(direName);
                        }
                        File.WriteAllBytes(strNormalPath, bytes);
                        //string keyStr = "___files_code___[\"" + key + "\"] = ";
                        //int keyIndex = text_all.IndexOf(keyStr) + keyStr.Length;
                        //string oldVal = text_all.Substring(keyIndex, 34);
                        //text_all = text_all.Replace(oldVal, "\"" + value + "\"");
                        Debug.Log("找到差异文件： " + key);
                    }

                }

                //sWriter.Write(text_all);
                sWriter.Close();
            }
        }
        fileBytes.Clear();
        fileCodes.Clear();
        filePaths.Clear();
    }
   
}

