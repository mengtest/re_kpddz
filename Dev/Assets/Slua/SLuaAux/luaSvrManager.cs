/***************************************************************


 *
 *
 * Filename:  	luaSvrManager.cs	
 * Summary: 	lua服务管理
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/10/14 5:35
 ***************************************************************/

using UnityEngine;
using System.Collections;
using SLua;
using Utils;
using sluaAux.proto;
using System.IO;
using UI.Controller;
using customerPath;
using System.Security.Cryptography;
using System.Text;
using asset;
using version;

namespace sluaAux
{
    [SLua.CustomSingletonLuaClass]
    public class luaSvrManager : Singleton<luaSvrManager>
    {
        //lua服务
        LuaSvr _luaSvr = null;

        //脚本主循环
        LuaFunction _funcLuaLoop = null;

        //动态脚本绑定列表
        LuaTable _dynLuaMonoTbl = null;

        //lua消息定义表
        LuaTable _luaProtoMsgDefTbl = null;

        //lua动态界面
        LuaTable _luaWinsTbl = null;

        //luaFileCodes
        LuaTable _luaFileCodes = null;

        // lua加载完成
        bool _luaLoaded = false;

        #region 属性
        
        public bool IsLoaded
        {
            get { return _luaLoaded; }
            set { _luaLoaded = value; }
        }

        public LuaSvr luaSvr
        {
            get { return _luaSvr; }
        }
        
        //lua的消息定义
        public LuaTable LuaProtoDefTbl
        {
            get { return _luaProtoMsgDefTbl; }
        }

        public LuaTable LuaWinsDef
        {
            get { return _luaWinsTbl; }
        }

        AssetBundle _bundleLuaData;
        #endregion

        /////////////////////////////////////////////////////////

        /// <summary>
        /// 初始化
        /// </summary>
        public void initialize()
        {
            _luaSvr = new LuaSvr();
#if UNITY_EDITOR
            _luaSvr.init(null, svrInitComplete, LuaSvrFlag.LSF_DEBUG);
            //设置脚本加载器
            LuaState.loaderDelegate = scriptLoaderEditor;
            if (AssetManager.getInstance().IsFirstUseStreamingAssets)
            {
                InitLuaData();
            }
#else
            _luaSvr.init(null, svrInitComplete, LuaSvrFlag.LSF_BASIC);
            //设置脚本加载器
            LuaState.loaderDelegate = scriptLoaderRelease;
            InitLuaData();
#endif

        }

        public void InitLuaData()
        {
            string persistenPath = string.Format("{0}/{1}/lua.data", Application.persistentDataPath, ClientDefine.LOCAL_PROGRAM_VERSION);
            if (File.Exists(persistenPath))
            {
            }
            else
            {
                persistenPath = string.Format("{0}/lua.data", IPath.streamingAssetsPathPlatform());
            }
            if (_bundleLuaData != null)
            {
                _bundleLuaData.Unload(false);
            }
            _bundleLuaData = AssetBundle.LoadFromFile(persistenPath);
        }
        
		public LuaState GetLuaState()
        {
            if (_luaSvr == null)
                return null;

            LuaState ls = _luaSvr.luaState;
            return ls;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 初始化完成回调
        /// </summary>
        public void svrInitComplete()
        {
            if (_luaSvr == null)
                return;

            LuaState ls = _luaSvr.luaState;
            if (ls == null)
                return;

            //获取lua文件列表与检验消息
            try
            {
                doHotScript("fileCode.lua");
                _luaFileCodes = ls.getTable("___files_code___");
            }
            catch
            {

            }
            

            //加载脚本
            _luaLoaded = false;
            ls.doFile("luaScripts/make.lua");
            LuaTable makeTbl = ls.getTable("___all_unbind_scripts___");
            if (makeTbl != null)
            {
                int nLen = makeTbl.length();
                for (int i=1; i<=nLen; i++)
                {
                    //Debug.Log(makeTbl[i]);
                    ls.doFile((string)makeTbl[i]);
                }
            }

            //加载动态绑定表
            _dynLuaMonoTbl = ls.getTable("___dynamic_lua_mono_table___");

            //加载lua消息定义
            _luaProtoMsgDefTbl = ls.getTable("___lua_proto_msg_def_table___");

            //加载lua动态界面
            _luaWinsTbl = ls.getTable("___lua_win_table___");
            if (_luaWinsTbl != null)
            {
                foreach (var data in _luaWinsTbl)
                {
                    LuaTable tb = (LuaTable)data.value;
                    if (tb[3] != null) continue;
                    string name = data.key.ToString();
                    string ctrlPath = "luaScripts/mono/" + name + "Controller.lua";
                    ls.doFile(ctrlPath);
                }
            }

            LuaTable configTbl = ls.getTable("___lua_config_path___");
            if (configTbl != null)
            {
                int nLen = configTbl.length();
                for (int i = 1; i <= nLen; i++)
                {
                    string ctrlPath = "luaScripts/config/data/" + (string)configTbl[i] + ".lua";
                    ls.doFile(ctrlPath);
                }
            }
            
            //执行main函数
            LuaFunction funcLuaMain = (LuaFunction)ls["main"];
            if (funcLuaMain != null)
                funcLuaMain.call();

            //脚本主循环
            _funcLuaLoop = (LuaFunction)ls["luaLoop"];

            //Lua消息初始化
            luaProtoHelper.initLuaProto();

            // lua内存采样记录
            LuaFunction luaMMSample = (LuaFunction)ls["memorySample"];
            if (luaMMSample != null)
                luaMMSample.call();


            #if UNITY_EDITOR
            //执行热脚本代码
            //doHotScript("hotScript.lua");
#else
            //执行热脚本代码
            try
            {
                doHotScript("hotScript.lua");
            }
            catch
            {

            }
#endif
            //初始化完成之后你可能需要做一些其他的工作
            otherWorks();
            LogSys.Log("------------------------------Lua脚步加载完成------------------------------------");
        }

        //////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 初始完成后的其他工作
        /// </summary>
        void otherWorks()
        {
            

            //lua加载完成
            SetLuaLoaded();
            network.protobuf.LuaStreamCache.ProcessCaches();

            //执行main函数
            LuaState ls = _luaSvr.luaState;
            LuaFunction funcLuaMain = (LuaFunction)ls["LuaLoaded"];
            if (funcLuaMain != null)
                funcLuaMain.call();

            /************ 测试 ************/

            /*
            //静态绑定
            Object o = Resources.Load<Object>("MyStaticLuaWindow");
            if (o != null)
                GameObject.Instantiate(o);

            // 动态绑定
            GameObject dynLuaWindow = new GameObject("MyDynamicLuaWindow");
            dynLuaWindow.AddComponent<luaMonoBehaviour>();
            */

            //界面测试
            //UIManager.CreateLuaWin("LuaTestWin");

            //消息测试
            //luaProtoTest.test();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////

        public void SetLuaLoaded()
        {
            _luaLoaded = true;
        }
        

        /// <summary>
        /// 获取动态的luaMono
        /// </summary>
        public string getDynLuaMono(string goName)
        {
            if (_dynLuaMonoTbl == null)
                return "";

            object o = _dynLuaMonoTbl[goName];
            if (o != null)
                return ((string)o);

            return "";
        }

        //////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 获取Lua中的消息定义
        /// </summary>
        /// <returns></returns>
        public LuaTable getLuaProtoMsgDef(string key)
        {
            if (_luaProtoMsgDefTbl != null)
                return (LuaTable)_luaProtoMsgDefTbl[key];

            return null;
        }

        public LuaTable getLuaProtoMsgDef(int key)
        {
            if (_luaProtoMsgDefTbl != null)
                return (LuaTable)_luaProtoMsgDefTbl[key];

            return null;
        }

        public LuaTable getLuaWinDef(string key)
        {
            if (_luaWinsTbl != null)
                return (LuaTable)_luaWinsTbl[key];

            return null;
        }

        public UILevel GetLuaWinLevel(string key)
        {
            LuaTable lt = getLuaWinDef(key);
            if (lt != null)
            {
                return (UILevel)int.Parse(lt[1].ToString());
            }
            else
            {
                return UILevel.BACKGROUND;
            }
        }

        public string GetLuaWinPrefabPath(string key)
        {
            LuaTable lt = getLuaWinDef(key);
            if (lt != null)
            {
                return lt[2].ToString();
            }
            else
            {
                return null;
            }
        }

        public bool GetLuaAddScene(string key)
        {
            LuaTable lt = getLuaWinDef(key);
            if (lt != null && lt[4] != null)
            {
                return bool.Parse(lt[4].ToString());
            }
            else
            {
                return false;
            }
        }
        

        public void loadLuaFile(string path)
        {
            if (_luaSvr == null)
                return;

            LuaState ls = _luaSvr.luaState;
            if (ls == null)
                return;

            //加载脚本
            ls.doFile("luaScripts/" + path);
        }

        /// <summary>
        /// 获取脚本模块
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public LuaTable getLuaModuleData(string module)
        {
            if (_luaSvr == null)
                return null;

            LuaState ls = _luaSvr.luaState;
            if (ls == null)
                return null;

            //执行main函数
            LuaFunction funcLuaModuleName = (LuaFunction)ls["MODULE_NAME"];
            if (funcLuaModuleName == null)
                return null;

            string  realModuleName = (string)funcLuaModuleName.call(module);
            if (string.IsNullOrEmpty(realModuleName))
                return null;

            return (LuaTable)ls[realModuleName];
        }

        private string hotScriptUrl()
        {
            return sdk.SDKManager.LuaHotScriptURL + ClientDefine.LOCAL_PROGRAM_VERSION + "/" + VersionData.GetLocalVersion() + "/luaCode/";
        }
         

        //////////////////////////////////////////////////////////////////////////////////////////////

        public byte[] getEncryptBytes(byte[] bytes, string fn)
        {
#if UNITY_EDITOR
           
#else
            // 开关检测
            if (_luaFileCodes != null && _luaFileCodes["_CHECK"].ToString().Equals("1"))
            {
                string fileName = fn.Substring(0, fn.Length - 4);
                if (_luaFileCodes[fileName] != null)
                {
                    string md5 = _luaFileCodes[fileName].ToString();
                    string cMD5 = TextUtils.MD5(bytes);
                    // MD5检测
                    if (!cMD5.Equals(md5))
                    {
                        //LogSys.LogError(fn + "->" + cMD5 + ", reload:" + hotScriptUrl() + fn);
                        WWW www = new WWW(hotScriptUrl() + fn);
                        while (!www.isDone) { };
                        // 向FTP获取最新文件
                        bytes = www.bytes;
                         /* 这里注释掉，就不再覆盖本地文件，而是下载最新的脚本，直接覆盖内存
                        string streamAsset = Application.streamingAssetsPath;
                        string plat_name = IPath.getPlatformName();
                        string luaFilePath = streamAsset + "/" + plat_name + "/" + fn;
                        string direName = Path.GetDirectoryName(luaFilePath);
                        if (!System.IO.Directory.Exists(direName))                       {
                            System.IO.Directory.CreateDirectory(direName);
                        }
                        // 覆盖本地文件
                        File.WriteAllBytes(luaFilePath, bytes);*/
                    }
                    else
                    {
                        //LogSys.LogError(fn + "->match");
                    }
                }
            }
#endif
            byte[] text_byte = bytes;
            if (UtilTools.ArrayHeadIsWoDong(bytes))
            {
                CMyEncryptFile _encrypte = new CMyEncryptFile();
                text_byte = _encrypte.Decrypt(bytes, bytes.Length);
            }
            return text_byte;
        }

#if UNITY_EDITOR

        /// <summary>
        /// 脚本加载器
        /// </summary>
        public byte[] scriptLoaderEditor(string fn)
        {
            try
            {
                byte[] bytes;

                string assetPath = UnityEngine.Application.dataPath;
                string luaFilePath = assetPath + "/" + fn;
//                 if (!File.Exists(luaFilePath))
//                 {
//                     string streamAsset = Application.streamingAssetsPath;
//                     string plat_name = IPath.getPlatformName();
//                     luaFilePath = streamAsset + "/" + plat_name + "/" + fn;
//                     
//                     if (!File.Exists(luaFilePath))
//                     {
//                         LogSys.LogError("脚本不存在: " + luaFilePath);
//                         return getEncryptBytes(null, fn);
//                     }
//                 }

                if (AssetManager.getInstance().IsFirstUseStreamingAssets)
                {
                    TextAsset text = (TextAsset)_bundleLuaData.LoadAsset(Path.GetFileNameWithoutExtension(fn));
                    bytes = text.bytes;
                }
                else
                {
                    bytes = File.ReadAllBytes(luaFilePath);
                }
                return getEncryptBytes(bytes, fn);
            }
            catch (System.Exception e)
            {
                throw new System.Exception(e.Message);
            }
        }
#endif
        /// <summary>
        /// 发布环境加载脚本
        /// </summary>
        /// <param name="fn"></param>
        /// <returns></returns>
        public byte[] scriptLoaderRelease(string fn)
        {
            TextAsset text = (TextAsset)_bundleLuaData.LoadAsset(Path.GetFileNameWithoutExtension(fn));
            int length = -1;
            if (text != null && text.bytes != null)
                length = text.bytes.Length;
            return getEncryptBytes(text.bytes, fn);
        }

        private static string Encrypt(string input, byte[] key, byte[] iv)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            RijndaelManaged rijndael = new RijndaelManaged();
            ICryptoTransform transform = rijndael.CreateEncryptor(key, iv);
            byte[] encrytData = null;
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (CryptoStream inputStream = new CryptoStream(outputStream, transform, CryptoStreamMode.Write))
                {
                    inputStream.Write(inputBytes, 0, inputBytes.Length);
                    inputStream.FlushFinalBlock();
                    encrytData = outputStream.ToArray();
                }

            }
            
            return System.Convert.ToBase64String(encrytData);
        }

        public object doHotScript(string file)
        {

            LuaState ls = _luaSvr.luaState;
            if (ls == null)
                return null;

            WWW www = new WWW(hotScriptUrl() + file + "?p=" + UtilTools.GetClientTime());
            while (!www.isDone) { };

            object obj;
            if (ls.doBuffer(www.bytes, "@" + file, out obj))
                return obj;
            return null;
        }

        public bool GetLogState()
        {
            return LogSys._bEnableLog;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 脚本重载
        /// </summary>
        public void reload()
        {
            //TODO: @WP.Chu
        }


        public void clear()
        {
            //TODO: @WP.Chu
        }

        //////////////////////////////////////////////////////////////////////////////////////////////


#region MONO

        public void Awake()
        {
        }

        /// <summary>
        /// 每帧执行
        /// </summary>
        public void Update()
        {
            if (_funcLuaLoop != null)
                _funcLuaLoop.call(Time.deltaTime);
        }

#endregion
    }
}


