/***************************************************************


 *
 *
 * Filename:  	CustomLuaMenu.cs	
 * Summary: 	Slua辅助工具菜单
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/10/17 22:21
 ***************************************************************/


using UnityEngine;
using System.Collections;
using sluaAux;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace sluaAuxEditor
{
    public enum EScriptElementType
    {
        Table,
        Function,
        Variable,
    }

    //脚本元素信息
    public class ScriptElementInfo
    {
        public EScriptElementType _type = EScriptElementType.Table;  //类型
        public string _name = ""; //名称
        public bool _bGlobal = false; //是否全局
        public string _module = ""; //所在导出模块
        public string _description = ""; //描述
    }

    //类信息编辑窗口
    public class ScriptClassEditorInfoWindow : EditorWindow
    {
        //模块名
        string _module = "";

        //类名
        string _class = "";

        //描述
        string _description = "";

        //是否全局
        bool _bGlobal = false;

        //是否模块导出
        bool _bModuleExport = true;

        //信息引用
        Dictionary<string, ScriptElementInfo> _dictInfos = null;

        //父窗口
        EditorWindow _parentWindow = null;

        string _strHelpMsg = "带有*标识的为必填项";
        MessageType _msgType = MessageType.Info;

        /////////////////////////////////////////////////////////////////

        //初始化
        public void initialize(string module, Dictionary<string, ScriptElementInfo> infos, EditorWindow parent = null)
        {
            _module = module;
            _dictInfos = infos;
            _parentWindow = parent;
        }

        //绘制窗口内容
        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("*类名");
            _class = EditorGUILayout.TextField(_class, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("*描述");
            _description = EditorGUILayout.TextField(_description, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("全局空间可见");
            _bGlobal = EditorGUILayout.Toggle(_bGlobal);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("模块导出");
            _bModuleExport = EditorGUILayout.Toggle(_bModuleExport);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox(_strHelpMsg, _msgType, true);

            //操作按钮
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("添加", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
            {
                if (string.IsNullOrEmpty(_class) || string.IsNullOrEmpty(_description))
                {
                    _strHelpMsg = "类名和注释不能为空";
                    _msgType = MessageType.Error;
                    return;
                }

                if (_dictInfos.ContainsKey(_class))
                {
                    _strHelpMsg = "已存在的类名";
                    _msgType = MessageType.Error;
                    return;
                }

                ScriptElementInfo eleInfo = new ScriptElementInfo();
                eleInfo._type = EScriptElementType.Table;
                eleInfo._name = _class;
                eleInfo._description = _description;
                eleInfo._bGlobal = _bGlobal;
                eleInfo._module = _bModuleExport ? _module : "";
                _dictInfos.Add(eleInfo._name, eleInfo);

                //刷新父窗口
                if (_parentWindow != null)
                    _parentWindow.Repaint();

                this.Close();
            }

            if (GUILayout.Button("取消", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
            {
                this.Close();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    //非绑定脚本信息窗口
    public class UnbindScriptInfoEditorWindow : EditorWindow
    {
        //生成路径
        string _strDirectory = "";

        //文件名
        string _strFileName = "";

        //脚本描述
        string _strSummary = "";

        //作者
        string _strAuthor = "";

        //模块名
        string _strModule = "";

        //类名  K-V = [名称, 元素信息]
        Dictionary<string, ScriptElementInfo> _dictElements = new Dictionary<string, ScriptElementInfo>();

        //帮助信息
        string _strHelpMsg = "带有*标识的为必填项";
        MessageType _msgType = MessageType.Info;

        //滚动条
        Vector2 _v2ScrollPos = new Vector2(0.0f, 0.0f);

        //全选
        bool _bSelectAll = false;

        //列表选中信息
        Dictionary<string, bool> _elementsToggleInfo = new Dictionary<string, bool>();

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        //初始化
        public void initialize(string directory)
        {
            _strDirectory = directory;
        }


        //绘制窗口内容
        void OnGUI()
        {
            EditorGUILayout.HelpBox(_strHelpMsg, _msgType, true);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("目录：" + _strDirectory);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("*文件名");
            _strFileName = EditorGUILayout.TextField(_strFileName, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("*脚本描述");
            _strSummary = EditorGUILayout.TextField(_strSummary, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("*作者");
            _strAuthor = EditorGUILayout.TextField(_strAuthor, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("模块名");
            _strModule = EditorGUILayout.TextField(_strModule, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("脚本类 ");
            if (GUILayout.Button("添加", GUILayout.Width(100)))
            {
                var classEdtorWin = (ScriptClassEditorInfoWindow)EditorWindow.GetWindow(typeof(ScriptClassEditorInfoWindow));
                classEdtorWin.name = "类信息";
                classEdtorWin.initialize(_strModule, _dictElements, this);
            }
            if (GUILayout.Button("删除", GUILayout.Width(100)))
            {
                var keys = new List<string>(_elementsToggleInfo.Keys);
                foreach (var k in keys)
                {
                    if (!_elementsToggleInfo[k])
                    {
                        _dictElements.Remove(k);
                        _elementsToggleInfo.Remove(k);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            ////////生成类列表信息

            //菜单
            EditorGUILayout.BeginHorizontal();
            _bSelectAll = EditorGUILayout.Toggle(_bSelectAll, GUILayout.Width(30)); //选中

            EditorGUILayout.LabelField("名称", GUILayout.Width(30), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("属性", GUILayout.Width(30), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("类型", GUILayout.Width(30), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("模块", GUILayout.Width(30), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            //信息
            _v2ScrollPos = EditorGUILayout.BeginScrollView(_v2ScrollPos);
            foreach (var kvp in _dictElements)
            {
                var elementInfo = kvp.Value;
                string strAttr = elementInfo._bGlobal ? "global" : "local";
                string strName = elementInfo._name;
                string strTypeName = elementInfo._type.ToString();
                string strModule = string.IsNullOrEmpty(elementInfo._module) ? "NULL" : elementInfo._module;

                //保存toggle信息
                bool bSaved = false;
                if (_elementsToggleInfo.ContainsKey(strName))
                    bSaved = _elementsToggleInfo[strName];
                else
                    _elementsToggleInfo.Add(strName, bSaved);


                //是否全选
                bool bToggle = _bSelectAll ? _bSelectAll : bSaved;
                EditorGUILayout.BeginHorizontal();
                bToggle = EditorGUILayout.Toggle(bToggle, GUILayout.Width(30)); //选中
                EditorGUILayout.LabelField(strName, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(strAttr, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(strTypeName, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(strModule, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                if (_bSelectAll && !bToggle)
                    _bSelectAll = false;

                //全选状态选中一个，取消全选
                _elementsToggleInfo[strName] = bToggle;
            }
            EditorGUILayout.EndScrollView();

            //操作按钮
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
            {
                if (string.IsNullOrEmpty(_strFileName) || string.IsNullOrEmpty(_strSummary) || string.IsNullOrEmpty(_strAuthor))
                {
                    _strHelpMsg = "信息不完整";
                    _msgType = MessageType.Error;
                    return;
                }

                //还原提示信息
                _strHelpMsg = "带有*标识的为必填项";
                _msgType = MessageType.Info;

                if (_dictElements.Count == 0)
                {
                    ScriptElementInfo dft = new ScriptElementInfo();
                    dft._name = "DefaultTable";
                    dft._description = "默认生成的类";
                    dft._type = EScriptElementType.Table;
                    dft._bGlobal = false;
                    _dictElements.Add(dft._name, dft);
                }

                //生成脚本
                string file = _strDirectory + "/" + _strFileName + ".lua";
                CustomLuaMenu.generateScriptFile(file, _strSummary,_strAuthor, _strModule, _dictElements);
            }

            if (GUILayout.Button("取消", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
            {
                this.Close();
            }
            EditorGUILayout.EndHorizontal();
        }
    }


    //绑定脚本信息窗口
    public class BindScriptInfoEditorWindow :EditorWindow
    {
        //生成路径
        string _strDirectory = "";

        //文件名
        string _strFileName = "";

        //脚本描述
        string _strSummary = "";

        //作者
        string _strAuthor = "";

        //模块名
        string _strModule = "";

        //函数  K-V = [名称, 元素信息]
        Dictionary<string, ScriptElementInfo> _dictElements = new Dictionary<string, ScriptElementInfo>();

        //帮助信息
        string _strHelpMsg = "带有*标识的为必填项";
        MessageType _msgType = MessageType.Info;

        //滚动条
        Vector2 _v2ScrollPos = new Vector2(0.0f, 0.0f);

        //选中的枚举
        ELuaMonoFunc _display = ELuaMonoFunc.Awake;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //初始化
        public void initialize(string directory)
        {
            _strDirectory = directory;
        }

        public void AddFuncToScript(string funcName)
        {
            if (!_dictElements.ContainsKey(funcName))
            {
                var funcInfo = new ScriptElementInfo();
                funcInfo._name = funcName;
                funcInfo._bGlobal = false;
                funcInfo._module = "M";
                funcInfo._type = EScriptElementType.Function;

                _dictElements.Add(funcName, funcInfo);
            }
        }

        //绘制窗口内容
        void OnGUI()
        {
            EditorGUILayout.HelpBox(_strHelpMsg, _msgType, true);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("目录：" + _strDirectory);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("*文件名");
            _strFileName = EditorGUILayout.TextField(_strFileName, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("*脚本描述");
            _strSummary = EditorGUILayout.TextField(_strSummary, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("*作者");
            _strAuthor = EditorGUILayout.TextField(_strAuthor, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();
           
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("*模块名");
            _strModule = EditorGUILayout.TextField(_strModule, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("函数信息 ");
            _display = (ELuaMonoFunc)EditorGUILayout.EnumPopup(_display, GUILayout.Width(200), GUILayout.ExpandWidth(true));
            if (GUILayout.Button("添加", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
            {
                string funcName = _display.ToString();
                AddFuncToScript(funcName);
            }
            EditorGUILayout.EndHorizontal();

            //菜单
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("名称", GUILayout.Width(30), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("属性", GUILayout.Width(30), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("类型", GUILayout.Width(30), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("模块", GUILayout.Width(30), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            //函数列表信息
            _v2ScrollPos = EditorGUILayout.BeginScrollView(_v2ScrollPos);
            foreach (var kvp in _dictElements)
            {
                var elementInfo = kvp.Value;
                string strAttr = elementInfo._bGlobal ? "global" : "local";
                string strName = elementInfo._name;
                string strTypeName = elementInfo._type.ToString();
                string strModule = _strModule;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(strName, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(strAttr, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(strTypeName, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(strModule, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            

            //操作按钮
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
            {
                if (string.IsNullOrEmpty(_strFileName) 
                    || string.IsNullOrEmpty(_strSummary) 
                    || string.IsNullOrEmpty(_strAuthor)
                    || string.IsNullOrEmpty(_strModule))
                {
                    _strHelpMsg = "信息不完整";
                    _msgType = MessageType.Error;
                    return;
                }

                //还原提示信息
                _strHelpMsg = "带有*标识的为必填项";
                _msgType = MessageType.Info;

                //生成脚本
                string file = _strDirectory + "/" + _strFileName + ".lua";
                if (!Directory.Exists(_strDirectory))
                {
                    Directory.CreateDirectory(_strDirectory);
                }
                CustomLuaMenu.generateScriptFile(file, _strSummary, _strAuthor, _strModule, _dictElements);
            }

            if (GUILayout.Button("取消", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
            {
                this.Close();
            }
            EditorGUILayout.EndHorizontal();
        }


    }

    public class LuaMonoScriptEditorWindow : EditorWindow
    {
        //生成路径
        string _strDirectory = "";

        //文件名
        string _strFileName = "";

        //脚本描述
        string _strSummary = "";

        //作者
        string _strAuthor = "";

        //模块名
        string _strModule = "";

        //编辑的脚本
        luaMonoBehaviour _parentBehaviour = null;

        #region Get/Set
        public luaMonoBehaviour ParentBehaviour
        {
            set { _parentBehaviour = value; }
        }

        public string ScriptName
        {
            get { return _strFileName; }
            set { _strFileName = value; }
        }

        public string ScriptAuthor
        {
            get { return _strAuthor; }
            set { _strAuthor = value; }
        }

        public string ScriptSummary
        {
            get { return _strSummary; }
            set { _strSummary = value; }
        }

        public string ScriptModule
        {
            get { return _strModule; }
            set { _strModule = value; }
        }

        #endregion

        //函数  K-V = [名称, 元素信息]
        [SerializeField]
        Dictionary<string, ScriptElementInfo> _dictElements = new Dictionary<string, ScriptElementInfo>();

        //帮助信息
        string _strHelpMsg = "带有*标识的为必填项";
        MessageType _msgType = MessageType.Info;

        //滚动条
        Vector2 _v2ScrollPos = new Vector2(0.0f, 0.0f);

        //选中的枚举
        ELuaMonoFunc _display = ELuaMonoFunc.Awake;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //初始化
        public void initialize(string directory)
        {
            _strDirectory = directory;
        }

        public void AddFuncToScript(string funcName)
        {
            if (!_dictElements.ContainsKey(funcName))
            {
                var funcInfo = new ScriptElementInfo();
                funcInfo._name = funcName;
                funcInfo._bGlobal = false;
                funcInfo._module = "M";
                funcInfo._type = EScriptElementType.Function;

                _dictElements.Add(funcName, funcInfo);
            }
        }

        void OnDestory()
        {
            Debug.LogError("OnDestory");
        }

        //绘制窗口内容
        void OnGUI()
        {
            EditorGUILayout.HelpBox(_strHelpMsg, _msgType, true);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("目录：" + _strDirectory);
            /*
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("*文件名");
            _strFileName = EditorGUILayout.TextField(_strFileName, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();
            */
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("*脚本描述");
            _strSummary = EditorGUILayout.TextField(_strSummary, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("*作者");
            _strAuthor = EditorGUILayout.TextField(_strAuthor, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("*模块名");
            _strModule = EditorGUILayout.TextField(_strModule, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("函数信息 ");
            _display = (ELuaMonoFunc)EditorGUILayout.EnumPopup(_display, GUILayout.Width(120), GUILayout.ExpandWidth(true));
            if (GUILayout.Button("添加", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
            {
                string funcName = _display.ToString();
                AddFuncToScript(funcName);
            }
            EditorGUILayout.EndHorizontal();

            //菜单
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("名称", GUILayout.Width(30), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("属性", GUILayout.Width(30), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("类型", GUILayout.Width(30), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("模块", GUILayout.Width(30), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            //函数列表信息
            _v2ScrollPos = EditorGUILayout.BeginScrollView(_v2ScrollPos);
            foreach (var kvp in _dictElements)
            {
                var elementInfo = kvp.Value;
                string strAttr = elementInfo._bGlobal ? "global" : "local";
                string strName = elementInfo._name;
                string strTypeName = elementInfo._type.ToString();
                string strModule = _strModule;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(strName, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(strAttr, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(strTypeName, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(strModule, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();


            //操作按钮
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("生成", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
            {
                if (string.IsNullOrEmpty(_strFileName)
                    || string.IsNullOrEmpty(_strSummary)
                    || string.IsNullOrEmpty(_strAuthor)
                    || string.IsNullOrEmpty(_strModule))
                {
                    _strHelpMsg = "信息不完整";
                    _msgType = MessageType.Error;
                    return;
                }

                //还原提示信息
                _strHelpMsg = "带有*标识的为必填项";
                _msgType = MessageType.Info;
                
                //生成脚本
                string file = _strDirectory + "/" + _strModule + "Mono.lua";
                System.IO.FileInfo fi = new System.IO.FileInfo(file);
                if (fi.Exists)
                {
                    EditorUtility.DisplayDialog("创建失败", "存在相同命名的脚本，请删除或者重命名后再创建。", "确认");
                    return;
                }

                if (!Directory.Exists(_strDirectory))
                {
                    Directory.CreateDirectory(_strDirectory);
                }
                CustomLuaMenu.CreateMonoScriptFile(file, _strSummary, _strAuthor, _strModule, _dictElements);

                if (_parentBehaviour != null)
                {
                    _parentBehaviour.bindScript = CustomLuaMonoEditor.MonoScriptsPath + _strModule + "Mono.lua";
                }

                string controllerPath = _strDirectory + "/" + _strModule + "Controller.lua";
                CustomLuaMenu.CreateControllerScriptFile(controllerPath, _strSummary, _strAuthor, _strModule);
                this.Close();
            }

            if (GUILayout.Button("取消", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
            {
                this.Close();
            }

            EditorGUILayout.EndHorizontal();
        }


    }


    public class CustomLuaMenu
    {
        //检查字符串
        static string checkString(string str)
        {
            return string.IsNullOrEmpty(str) ? "" : str;
        }

        // 分隔
        static void writeInterval(StreamWriter writer)
        {
            writer.WriteLine("\n");
        }

        // 块注释
        static void writeBlockComments(StreamWriter writer, string comments)
        {
            if (string.IsNullOrEmpty(comments))
                return;

            writer.WriteLine("-- ------------------------");
            writer.WriteLine("-- {0}", comments);
            writer.WriteLine("-- ------------------------");
        }

        //写入文件头
        static void writeFileHead(StreamWriter writer, string fileName, string summary, string author)
        {
            fileName = checkString(fileName);
            summary = checkString(summary);
            author = checkString(author);

            writer.WriteLine("-- -----------------------------------------------------------------");
            writer.WriteLine("-- *");
            writer.WriteLine("-- * Filename:    {0}", fileName);
            writer.WriteLine("-- * Summary:     {0}", summary);
            writer.WriteLine("-- *");
            writer.WriteLine("-- * Version:     1.0.0");
            writer.WriteLine("-- * Author:      {0}",
                string.IsNullOrEmpty(ClientDefine.EditorAuthor) ? author : ClientDefine.EditorAuthor);
            writer.WriteLine("-- * Date:        {0}", System.DateTime.Now.ToString());
            writer.WriteLine("-- -----------------------------------------------------------------");
            writeInterval(writer);
        }

        //模块开始
        static void writeModuleBegin(StreamWriter writer, string moduleName)
        {
            writeInterval(writer);
            writer.WriteLine("-- 生成模块，模块导出接口需包含在M表中");
            writer.WriteLine("local M = GENERATE_MODULE(\"{0}\")", moduleName);
            writer.WriteLine("\n\n");
        }

        static void writeModuleName(StreamWriter writer, string winName)
        {
            writer.WriteLine("-- 界面名称");
            writer.WriteLine("local wName = \"{0}\"", winName);
            writer.WriteLine("-- 获取界面控制器");
            writer.WriteLine("local _controller = UI.Controller.UIManager.GetControler(wName)");
            writer.WriteLine("\n\n");
        }

        //模块结束
        static void writeModuleEnd(StreamWriter writer)
        {
            writeInterval(writer);
            writer.WriteLine("-- 返回当前模块");
            writer.WriteLine("return M");
        }

        //生成脚本类对象
        static void writeScriptClass(StreamWriter writer, bool global, string className, string description)
        {
            writeInterval(writer);
            writeBlockComments(writer, description);
            string attr = global ? "" : "local ";
            writer.WriteLine("{0}{1} = {{\n", attr, className);
            writer.WriteLine("}");
            writeInterval(writer);

            //构造函数
            writer.WriteLine("function {0}:new(o)", className);
            writer.WriteLine("    o = o or {}");
            writer.WriteLine("    setmetatable(o, self)");
            writer.WriteLine("    self.__index = self");
            writer.WriteLine("    return o");
            writer.WriteLine("end");
        }

        //生成脚本函数
        static void writeScriptFunction(StreamWriter writer, bool global, string funcName, string description,string monoName = "")
        {
            writeBlockComments(writer, description);
            string attr = global ? "" : "local ";
            writer.WriteLine("{0}function {1}(gameObject)", attr, funcName);
            if (funcName=="OnDestroy"){
                writer.WriteLine("    CLEAN_MODULE(\"{0}\")",monoName);
            }
            else{
                writer.WriteLine("");
            }
            writer.WriteLine("end");
            writeInterval(writer);
        }

        static void writeScriptCloseFunction(StreamWriter writer)
        {
            writer.WriteLine("local function CloseWin(gameObject)");
            writer.WriteLine("    UnityTools.DestroyWin(wName)");
            writer.WriteLine("end");
            writeInterval(writer);
        }

        //生成导出设置
        static void writeExportSetting(StreamWriter writer, string className)
        {
            writer.WriteLine("M.{0} = {1}", className, className);
        }

        //生成注册设置
        static void writeRegisterSetting(StreamWriter writer, string moduleName)
        {
            writer.WriteLine("UI.Controller.UIManager.RegisterLuaWinFunc(\"{0}\", OnCreateCallBack, OnDestoryCallBack)", moduleName);
        }

        public static void CreateMonoScriptFile(string file, string summary, string author, string module, Dictionary<string, ScriptElementInfo> elements)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(file, FileMode.CreateNew);
                using (StreamWriter writer = new StreamWriter(fs, new UTF8Encoding(false)))
                {
                    //文件头
                    string fileName = file.Substring(file.LastIndexOf("/") + 1);
                    writeFileHead(writer, fileName, summary, author);

                    string moduleName = module + "Mono";

                    //模块信息开始
                    if (!string.IsNullOrEmpty(moduleName))
                        writeModuleBegin(writer, moduleName);

                    if (!string.IsNullOrEmpty(module))
                    {
                        writeModuleName(writer, module);
                        writer.WriteLine("-- 获取控制器模块");
                        writer.WriteLine("local CTRL = IMPORT_MODULE(wName .. \"Controller\")");
                        writer.WriteLine("-- 载入工具模块");
                        writer.WriteLine("local UnityTools = IMPORT_MODULE(\"UnityTools\")");
                        writer.WriteLine("\n");
                    }
                        

                    writeScriptCloseFunction(writer);

                    //类和函数信息
                    foreach (var kvp in elements)
                    {
                        var v = kvp.Value;
                        if (v._type == EScriptElementType.Table)
                            writeScriptClass(writer, v._bGlobal, v._name, v._description);
                        else if (v._type == EScriptElementType.Function)
                            writeScriptFunction(writer, v._bGlobal, v._name, v._description,moduleName);
                    }

                    // 模块导出信息
                    if (!string.IsNullOrEmpty(moduleName))
                    {
                        writeInterval(writer);
                        writeBlockComments(writer, "模块导出设置");
                        foreach (var kvp in elements)
                        {
                            var v = kvp.Value;
                            if (!string.IsNullOrEmpty(v._module))
                                writeExportSetting(writer, v._name);
                        }

                        //模块信息结束
                        writeModuleEnd(writer);
                    }

                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }

            AssetDatabase.Refresh();
        }

        public static void CreateControllerScriptFile(string file, string summary, string author, string module)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(file, FileMode.CreateNew);
                using (StreamWriter writer = new StreamWriter(fs, new UTF8Encoding(false)))
                {
                    //文件头
                    string fileName = file.Substring(file.LastIndexOf("/") + 1);
                    writeFileHead(writer, fileName, summary, author);

                    string moduleName = module + "Controller";

                    //模块信息开始
                    if (!string.IsNullOrEmpty(moduleName))
                        writeModuleBegin(writer, moduleName);

                    if (!string.IsNullOrEmpty(module))
                        writeModuleName(writer, module);

                    // 注册信息
                    if (!string.IsNullOrEmpty(module))
                    {
                        writeScriptFunction(writer, false, "OnCreateCallBack", "");
                        writeScriptFunction(writer, false, "OnDestoryCallBack", "");

                        writeInterval(writer);
                        writeRegisterSetting(writer, module);
                    }

                    //模块信息结束
                    writeModuleEnd(writer);

                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }

            AssetDatabase.Refresh();
        }

        //生成脚本文件
        public static void generateScriptFile(string file, string summary, string author, string module, Dictionary<string, ScriptElementInfo> elements, bool needReg = false, string winName = "")
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(file, FileMode.CreateNew);
                using (StreamWriter writer = new StreamWriter(fs, new UTF8Encoding(false)))
                {
                    //文件头
                    string fileName = file.Substring(file.LastIndexOf("/") + 1);
                    writeFileHead(writer, fileName, summary, author);

                    //模块信息开始
                    if (!string.IsNullOrEmpty(module))
                        writeModuleBegin(writer, module);

                    if (!string.IsNullOrEmpty(winName))
                        writeModuleName(writer, winName);

                    //类和函数信息
                    foreach (var kvp in elements)
                    {
                        var v = kvp.Value;
                        if (v._type == EScriptElementType.Table)
                            writeScriptClass(writer, v._bGlobal, v._name, v._description);
                        else if (v._type == EScriptElementType.Function)
                            writeScriptFunction(writer, v._bGlobal, v._name, v._description);
                    }

                    // 注册信息
                    if (needReg && !string.IsNullOrEmpty(module))
                    {
                        writeScriptFunction(writer, false, "OnCreateCallBack", "");
                        writeScriptFunction(writer, false, "OnDestoryCallBack", "");

                        writeInterval(writer);
                        writeRegisterSetting(writer, module);
                    }

                    // 模块导出信息
                    if (!string.IsNullOrEmpty(module))
                    {
                        writeInterval(writer);
                        writeBlockComments(writer, "模块导出设置");
                        foreach (var kvp in elements)
                        {
                            var v = kvp.Value;
                            if (!string.IsNullOrEmpty(v._module))
                                writeExportSetting(writer, v._name);
                        }

                        //模块信息结束
                        writeModuleEnd(writer);
                    }

                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 生成非绑定脚本
        /// </summary>
        [MenuItem("Assets/Create/Lua/Unbind Script")]
        static void createUnbindLuaScript()
        {
            UnbindScriptInfoEditorWindow window = (UnbindScriptInfoEditorWindow)EditorWindow.GetWindow(typeof(UnbindScriptInfoEditorWindow));
            string targetPath = "";

            string dataPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/") + 1);
            Object[] objs = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);
            foreach (var obj in objs)
            {
                string strObjPath = AssetDatabase.GetAssetPath(obj);
                targetPath = dataPath + strObjPath;

                if (Directory.Exists(targetPath))
                    break;
                else
                {
                    if (File.Exists(targetPath))
                    {
                        targetPath = targetPath.Substring(1, targetPath.LastIndexOf("/") + 1);
                        break;
                    }
                }
            }

            if (Directory.Exists(targetPath))
                window.initialize(targetPath);

            return;
        }


        //生成绑定脚本
        [MenuItem("Assets/Create/Lua/Bind Script")]
        static void CreateBindLuaScript()
        {
            BindScriptInfoEditorWindow window = (BindScriptInfoEditorWindow)EditorWindow.GetWindow(typeof(BindScriptInfoEditorWindow));
            string targetPath = "";

            string dataPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/") + 1);
            Object[] objs = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);
            foreach (var obj in objs)
            {
                string strObjPath = AssetDatabase.GetAssetPath(obj);
                targetPath = dataPath + strObjPath;

                if (Directory.Exists(targetPath))
                    break;
                else
                {
                    if (File.Exists(targetPath))
                    {
                        targetPath = targetPath.Substring(1, targetPath.LastIndexOf("/") + 1);
                        break;
                    }
                }
            }

            if (Directory.Exists(targetPath))
                window.initialize(targetPath);

            return;
        }

        private static void addFunToElement(Dictionary<string, ScriptElementInfo> dictElements, string funcName)
        {
            if (!dictElements.ContainsKey(funcName))
            {
                var funcInfo = new ScriptElementInfo();
                funcInfo._name = funcName;
                funcInfo._bGlobal = false;
                funcInfo._module = "M";
                funcInfo._type = EScriptElementType.Function;

                dictElements.Add(funcName, funcInfo);
            }
        }

        public static void OpenMonoLuaScriptEditor(luaMonoBehaviour parentBehaviour)
        {
            string targetPath = Application.dataPath + "/" + CustomLuaMonoEditor.MonoScriptsPath;
            LuaMonoScriptEditorWindow window = (LuaMonoScriptEditorWindow)EditorWindow.GetWindow(typeof(LuaMonoScriptEditorWindow));
            if (string.IsNullOrEmpty(window.ScriptAuthor)) window.ScriptAuthor = SystemInfo.deviceName;
            window.ParentBehaviour = parentBehaviour;
            window.ScriptName = parentBehaviour.gameObject.name;
            window.ScriptModule = window.ScriptName;
            window.AddFuncToScript("Awake");
            window.AddFuncToScript("Start");
            window.AddFuncToScript("Update");
            window.initialize(targetPath);
        }

        public static bool CreateMonoLuaScript(string monoName, string desc)
        {
            string targetPath = Application.dataPath + "/" + CustomLuaMonoEditor.MonoScriptsPath;
            if (!string.IsNullOrEmpty(desc) && !desc.Equals("脚本描述"))
            {
                string monoPath = targetPath + monoName + "Mono.lua";
                Dictionary<string, ScriptElementInfo> dictElements = new Dictionary<string, ScriptElementInfo>();
                addFunToElement(dictElements, "Awake");
                addFunToElement(dictElements, "Start");
                addFunToElement(dictElements, "OnDestroy");
                addFunToElement(dictElements, "OnEnable");
                addFunToElement(dictElements, "OnDisable");
                //addFunToElement(dictElements, "Update");
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                CustomLuaMenu.CreateMonoScriptFile(monoPath, desc, SystemInfo.deviceName, monoName, dictElements);

                string controllerPath = targetPath + monoName + "Controller.lua";
                CustomLuaMenu.CreateControllerScriptFile(controllerPath, desc, SystemInfo.deviceName, monoName);

                return true;
            }
            else
            {
                EditorUtility.DisplayDialog("创建失败", "请填写正确的脚本描述", "确认");
                return false;
            }
        }

    }
}


