using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor;
using System.Collections;
using System.Linq;
using sluaAuxEditor;
using sluaAux;
using System.Collections.Generic;

public static class LuaAutoComponentEditor
{
    [MenuItem("GameObject/Select All/Select By Name", false, 15)]
    static void SelectAll(MenuCommand command)
    {
        var selectGameObject = command.context as GameObject;
        string name = selectGameObject.name;
        Transform tfParent = selectGameObject.transform.parent;
        var children = tfParent.transform.root.GetComponentsInChildren<Transform>(true);
        List<Object> sObjs = new List<Object>();
        for (var i = 0; i < children.Length; i++)
        {
            if (children[i].name.Equals(name))
            {
                sObjs.Add(children[i].gameObject);
            }
        }
//        Debug.LogError("Search: " + children.Count().ToString() + " Find: " + sObjs.Count().ToString());
        Selection.objects = sObjs.ToArray();
    }

    [MenuItem("GameObject/Select All/Select By Path", false, 15)]
    static void SelectAllPath(MenuCommand command)
    {
        var selectGameObject = command.context as GameObject;
        string name = selectGameObject.name;
        Transform tfParent = selectGameObject.transform.parent;
        var children = tfParent.transform.root.GetComponentsInChildren<Transform>(true);
        List<Object> sObjs = new List<Object>();
        for (var i = 0; i < children.Length; i++)
        {
            if (children[i].name.Equals(name) && children[i].parent.name.Equals(tfParent.name))
            {
                sObjs.Add(children[i].gameObject);
            }
        }
//        Debug.LogError("Search: " + children.Count().ToString() + " Find: " + sObjs.Count().ToString());
        Selection.objects = sObjs.ToArray();

    }

    static void changeDepth(MenuCommand command, int val)
    {
        var selectGameObject = command.context as GameObject;
        string name = selectGameObject.name;
        Transform tfParent = selectGameObject.transform;
        var children = tfParent.transform.GetComponentsInChildren<Transform>(true);
        List<Object> sObjs = new List<Object>();
        for (var i = 0; i < children.Length; i++)
        {
            UIWidget wid = children[i].GetComponent<UIWidget>();
            if (wid != null)
            {
                if (val == 0)
                {
                    wid.depth = 0;
                }
                else
                {
                    wid.depth += val;
                }
                
            }
        }
    }

    [MenuItem("GameObject/Change Depth/Add 50", false, 16)]
    static void ChangeDepthAdd50(MenuCommand command)
    {
        changeDepth(command, 50);
    }

    [MenuItem("GameObject/Change Depth/Mul 50", false, 16)]
    static void ChangeDepthMul50(MenuCommand command)
    {
        changeDepth(command, -50);
    }

    [MenuItem("GameObject/Change Depth/Mul 2", false, 16)]
    static void ChangeDepthMul2(MenuCommand command)
    {
        changeDepth(command, -2);
    }

    [MenuItem("GameObject/Change Depth/Zero", false, 16)]
    static void ChangeDepthZero(MenuCommand command)
    {
        changeDepth(command, 0);
    }

    [MenuItem("GameObject/Print Path", false, 14)]
    static void PrintPath(MenuCommand command)
    {
        var selectGameObject = command.context as GameObject;
        string path = selectGameObject.name;
        Transform tfParent = selectGameObject.transform.parent;
        while (tfParent != null)
        {
            if (tfParent.name.Contains("UIRoot") || tfParent.parent == null) break;
            path = tfParent.name + "/" + path;
            tfParent = tfParent.parent;
        }
        Debug.Log("Copy-> " + path);
        TextEditor te = new TextEditor();
        te.text = path;
        te.SelectAll();
        te.Copy();
    }

    [MenuItem("GameObject/Auto Lua Bind", false, 13)]
    static void AutoLuaGameObject(MenuCommand command)
    {
        GameObject go = command.context as GameObject;
        UIWidget wid = go.GetComponent<UIWidget>();
        Object gObj = null;
        if (wid != null)
        {
            gObj = wid;
        }
        else
        {
            gObj = go.GetComponent<UIPanel>();
        }
        string findPath = getWidgetPath(gObj);
        string comptName = command.context.name;
        string comptType = command.context.GetType().ToString();
        comptType = comptType.Substring(comptType.LastIndexOf(".") + 1);
        string file = getLuaFilePath(gObj);

        LuaAutoComponentEditorWindow window = (LuaAutoComponentEditorWindow)EditorWindow.GetWindow(typeof(LuaAutoComponentEditorWindow));
        window.position = new Rect(960, 540, 515, 135);
        window.ComptName = comptName;
        window.ComptType = comptType;
        window.ComptPath = findPath;
        window.initialize(file);
    }

    [MenuItem("CONTEXT/Component/Auto Lua Bind")]
    static void AutoLuaComponent(MenuCommand command)
    {
        string findPath = getWidgetPath(command.context);
        string comptName = command.context.name;
        string comptType = command.context.GetType().ToString();
        comptType = comptType.Substring(comptType.LastIndexOf(".") + 1);
        string file = getLuaFilePath(command.context);
        
        LuaAutoComponentEditorWindow window = (LuaAutoComponentEditorWindow)EditorWindow.GetWindow(typeof(LuaAutoComponentEditorWindow));
        window.position = new Rect(960, 540, 515, 135);
        window.ComptName = comptName;
        window.ComptType = comptType;
        window.ComptPath = findPath;
        window.initialize(file);
    }

    /// <summary>
    /// 获取组件所在根窗口的绑定脚本路径
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    static string getLuaFilePath(Object obj)
    {
        string targetPath = Application.dataPath  + "/";
        luaMonoBehaviour luaBeh = getWidgetRoot(obj).GetComponent<luaMonoBehaviour>();
        if (luaBeh == null)
        {
            Debug.LogError("UIRoot can not find LuaMonoBehaviour");
            return "";
        }
        string monoPath = luaBeh.bindScript;
        if (string.IsNullOrEmpty(monoPath))
        {
            Debug.LogError("UIRoot<LuaMonoBehaviour> can not find lua file");
            return "";
        }

        string file = targetPath + monoPath;
        return file;
    }

    /// <summary>
    /// 获取组件所在的根窗口
    /// </summary>
    /// <param name="selectObj"></param>
    /// <returns></returns>
    static GameObject getWidgetRoot(Object selectObj)
    {
        var selectWidget = selectObj as Component;
        return selectWidget.GetComponentInParent<UIRoot>().gameObject;
    }

    /// <summary>
    /// 获取组件的路径
    /// </summary>
    /// <param name="selectObj"></param>
    /// <returns></returns>
    static string getWidgetPath(Object selectObj)
    {
        var selectWidget = selectObj as Component;
        var selectGameObject = selectWidget.gameObject;
        string path = selectGameObject.name;
        Transform tfParent = selectGameObject.transform.parent;
        while (tfParent != null)
        {
            if (tfParent.name.Contains("UIRoot") || tfParent.parent == null) break;
            path = tfParent.name + "/" + path;
            tfParent = tfParent.parent;
        }
        return path;
    }

    /// <summary>
    /// 创建自动绑定标签
    /// </summary>
    /// <param name="fsStr"></param>
    /// <returns></returns>
    static string createAutoLuaTag(string fsStr)
    {
        if (fsStr.IndexOf("--- [ALD END]") < 0)
        {
            if (fsStr.IndexOf("local function Awake(gameObject)") >= 0 && fsStr.IndexOf("local UnityTools = IMPORT_MODULE(\"UnityTools\")") >= 0)
            {
                fsStr = fsStr.Replace("local function Awake(gameObject)", @"-- Lua Editor 自动绑定
local function AutoLuaBind(gameObject)
--- [ALB END]
end

local function Awake(gameObject)
    -- Lua Editor 自动绑定
    AutoLuaBind(gameObject)");
                fsStr = fsStr.Replace("local UnityTools = IMPORT_MODULE(\"UnityTools\")", @"local UnityTools = IMPORT_MODULE(""UnityTools"")

--- [ALD END]

--- [ALF END]
");
            }
            else
            {
                Debug.LogError("Lua file can not find auto bind tag");
                return null;
            }
        }
        return fsStr;
    }

    public static string readLuaFile(string file)
    {
        string result = null;
        if (!string.IsNullOrEmpty(file))
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(file, FileMode.Open);
                int fsLen = (int)fs.Length;
                byte[] heByte = new byte[fsLen];
                fs.Read(heByte, 0, heByte.Length);
                result = System.Text.Encoding.UTF8.GetString(heByte);

                fs.Dispose();
                fs.Close();
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }
        return result;
    }

    /// <summary>
    /// 写入自动绑定的内容
    /// </summary>
    /// <param name="file"></param>
    /// <param name="defineStr"></param>
    /// <param name="bindStr"></param>
    public static void writeLuaFile(string file, string content, string defineStr, string bindStr, string funcStr = null)
    {
        if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(content) || (string.IsNullOrEmpty(defineStr) && string.IsNullOrEmpty(bindStr) && string.IsNullOrEmpty(funcStr))) return;
        content = createAutoLuaTag(content);
        if (!string.IsNullOrEmpty(defineStr))
        {
            content = content.Replace("--- [ALD END]", defineStr + "\n--- [ALD END]\n");
        }
        if (!string.IsNullOrEmpty(bindStr))
        {
            content = content.Replace("--- [ALB END]", bindStr + "\n\n--- [ALB END]\n");
        }
        if (!string.IsNullOrEmpty(funcStr))
        {
            content = content.Replace("--- [ALF END]", funcStr + "\n\n--- [ALF END]\n");
        }
        using (StreamWriter writer = new StreamWriter(file, false, new UTF8Encoding(false)))
        {
            writer.Write(content);
            writer.Dispose();
            writer.Close();
        }
    }
}


public class LuaAutoComponentEditorWindow : EditorWindow
{
    
    //生成路径
    string _strDirectory = "";
    
    //组件名称
    string _strComptName = "";

    //组件类型
    string _strComptType = "";

    //组件路径
    string _strComptPath = "";

    //响应事件
    string _strOnClickEvent = "";

    //编辑的脚本
    luaMonoBehaviour _parentBehaviour = null;

    #region Get/Set
    public luaMonoBehaviour ParentBehaviour
    {
        set { _parentBehaviour = value; }
    }

    public string ComptName
    {
        set { _strComptName = value; }
    }

    public string ComptType
    {
        set { _strComptType = value; }
    }

    public string ComptPath
    {
        set { _strComptPath = value; }
    }
    #endregion


    //帮助信息
    string _strHelpMsg = "带有*标识的为必填项";
    MessageType _msgType = MessageType.Info;

    bool _isBindToggleEvent = false;
    bool _isBindTouchEvent = false;
    bool _isCreateSVEvent = false;


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////


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
        EditorGUILayout.LabelField("*绑定变量名");
        _strComptName = EditorGUILayout.TextField(_strComptName, GUILayout.Width(300), GUILayout.ExpandWidth(true));

        EditorGUILayout.EndHorizontal();

        if (_strComptType.Equals("UIScrollView"))
        {
            EditorGUILayout.BeginHorizontal();
            _isCreateSVEvent = EditorGUILayout.Toggle("绑定OnShowItem", _isCreateSVEvent);
            if (_isCreateSVEvent)
            {
                _strOnClickEvent = EditorGUILayout.TextField(_strOnClickEvent, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();


            if (!_isBindToggleEvent)
            {
                _isBindTouchEvent = EditorGUILayout.Toggle("是否绑定OnClick", _isBindTouchEvent);
            }

            if (!_isBindTouchEvent)
            {
                _isBindToggleEvent = EditorGUILayout.Toggle("是否绑定Toggle", _isBindToggleEvent);
            }

            if (_isBindTouchEvent) 
            {
                _strOnClickEvent = EditorGUILayout.TextField(_strOnClickEvent, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            }

            EditorGUILayout.EndHorizontal();

            
        }

        //操作按钮
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("生成", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
        {
            if (string.IsNullOrEmpty(_strComptName))
            {
                _strHelpMsg = "信息不完整";
                _msgType = MessageType.Error;
                return;
            }

            string content = LuaAutoComponentEditor.readLuaFile(_strDirectory);
            if (content.IndexOf("local " + _strComptName) >= 0)
            {
                _strHelpMsg = "变量名重复";
                _msgType = MessageType.Error;
                return;
            }

            if (_isBindTouchEvent && string.IsNullOrEmpty(_strOnClickEvent))
            {
                _strHelpMsg = "请填写响应函数";
                _msgType = MessageType.Error;
                return;
            }

            string defineStr = "local " + _strComptName;
            string bindStr = null;
            if (_strComptType.Equals("GameObject"))
            {
                bindStr = "    " + _strComptName + " = UnityTools.FindGo(gameObject.transform, \"" + _strComptPath + "\")";
            }
            else
            {
                bindStr = "    " + _strComptName + " = UnityTools.FindCo(gameObject.transform, \"" + _strComptType + "\", \"" + _strComptPath + "\")";
            }
            
            string funcStr = null;
            if (_isCreateSVEvent)
            {
                defineStr += "\nlocal " + _strComptName + "_mgr";
                bindStr += ("\n    " + _strComptName + "_mgr = UnityTools.FindCoInChild(" + _strComptName + ", \"UIGridCellMgr\")");
                bindStr += "\n    " + _strComptName + "_mgr.onShowItem = "+_strOnClickEvent;
                bindStr += "\n    -- _controller.SetScrollViewRenderQueue(" + _strComptName + ")";

                if (content.IndexOf("local function " + _strOnClickEvent) < 0)
                {
                    funcStr = "local function " + _strOnClickEvent + "(cellbox, index, item)\nend";
                }
            }
            else if (_isBindToggleEvent)
            {
                string togList = _strComptName + "_list";
                string togSelected = _strComptName + "_selected";
                defineStr += "\nlocal " + togList + " = {}";
                defineStr += "\nlocal " + togSelected;
                string toggles = "\n    UnityTools.CallFuncInChildren(" + _strComptName + ", \"UIToggle\", function (tab)";
                toggles += "\n        " + togList + "[#" + togList + " + 1] = tab";
                toggles += "\n        tab:GetComponent(\"UIToggle\").value = false";
                toggles += "\n        UnityTools.AddOnClick(tab, OnClick" + _strComptName + "Call)";
                toggles += "\n    end)";
                toggles += "\n    change" + _strComptName + "Select(1)";
                bindStr += toggles;
                string funcs = "local function OnClick" + _strComptName + "Call(gameObject)";
                funcs += "\n    if " + togSelected + " ~= nil then";
                funcs += "\n        " + togSelected + ":GetComponent(\"UIToggle\").value = false";
                funcs += "\n    end";
                funcs += "\n    " + togSelected + " = gameObject";
                funcs += "\n    " + togSelected + ":GetComponent(\"UIToggle\").value = true";
                funcs += "\nend\n";
                string temps = @"local function FUNC(index)
    if LIST[index] ~= nil then
        local tab = LIST[index]
        if OBJ ~= tab then
            CLICK(tab)
        end
    end
end";
                temps = temps.Replace("FUNC", "change" + _strComptName + "Select");
                temps = temps.Replace("LIST", togList);
                temps = temps.Replace("OBJ", togSelected);
                temps = temps.Replace("CLICK", "OnClick" + _strComptName + "Call");
                funcs += "\n" + temps;
                funcStr = funcs;
            }
            else if (_isBindTouchEvent)
            {
                if (_strComptType.Equals("GameObject"))
                {
                    bindStr += "\n    UnityTools.AddOnClick(" + _strComptName + ".gameObject, " + _strOnClickEvent + ")";
                }
                else
                {
                    bindStr += "\n    UnityTools.AddOnClick(" + _strComptName + ", " + _strOnClickEvent + ")";
                }
                
                if (content.IndexOf("local function " + _strOnClickEvent) < 0)
                {
                    funcStr = "local function " + _strOnClickEvent + "(gameObject)\nend";
                }
            }
            LuaAutoComponentEditor.writeLuaFile(_strDirectory, content, defineStr, bindStr, funcStr);

            //还原提示信息
            _strHelpMsg = "带有*标识的为必填项";
            _msgType = MessageType.Info;
            this.Close();
        }

        if (GUILayout.Button("取消", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
        {
            this.Close();
        }

        EditorGUILayout.EndHorizontal();
    }


}