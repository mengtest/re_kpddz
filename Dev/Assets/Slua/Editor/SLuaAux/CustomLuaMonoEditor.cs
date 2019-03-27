/***************************************************************


 *
 *
 * Filename:  	CustomLuaMonoEditor.cs	
 * Summary: 	lua行为组件Inspector定制
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/10/15 1:26
 ***************************************************************/


using UnityEngine;
using System.Collections;
using UnityEditor;
using sluaAux;

namespace sluaAuxEditor
{

    [CustomEditor(typeof(luaMonoBehaviour))]
    public class CustomLuaMonoEditor : Editor
    {
        //脚本文件夹路径
        static string scriptFolderPath = "Assets/Resources/";

        public static string MonoScriptsPath = "luaScripts/mono/";

        //编辑的脚本
        luaMonoBehaviour info = null;

        //临时对象
        Object obj = null;

        //创建脚本的描述
        string _cScriptDesc = "";
    
        /////////////////////////////////////////////////////////////////////////////

        public void OnEnable()
        {
            info = (luaMonoBehaviour)target;
        }

        private bool checkAsset(string assetPath)
        {
            string ext = System.IO.Path.GetExtension(assetPath);
            if (ext.Equals(".txt") || ext.Equals(".lua"))
                return true;

            return false;
        }

        private bool checkLuaExsit(string luaName)
        {
            string targetPath = Application.dataPath + "/" + MonoScriptsPath;
            string file = targetPath + luaName + "Mono.lua";
            System.IO.FileInfo fi = new System.IO.FileInfo(file);
            if (fi.Exists)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("LuaScript");
            if (string.IsNullOrEmpty(info.bindScript))
            {
                
                obj = EditorGUILayout.ObjectField(obj, typeof(Object), false, GUILayout.Width(170), GUILayout.ExpandWidth(true));
                if (obj != null)
                {
                    string assetPath = AssetDatabase.GetAssetPath(obj);
                    if (!string.IsNullOrEmpty(assetPath) && assetPath.Contains("/luaScripts/") && checkAsset(assetPath))
                    {
                        int begin = assetPath.IndexOf("luaScripts");
                        int end = assetPath.LastIndexOf("/");

                        info.bindScript = assetPath.Substring(begin, end - begin + 1) + obj.name + ".lua";
                    }
                    else
                    {
                        Debug.LogError("请选择正确的Lua脚本文件(*.txt、*.lua)  GameObject: " + info.gameObject.name);
                        obj = null;
                    }
                }
                
            }
            else
            {
                string objPath = scriptFolderPath + info.bindScript;
                obj = AssetDatabase.LoadAssetAtPath(objPath + ".lua", typeof(Object));
                if (obj == null)
                    obj = AssetDatabase.LoadAssetAtPath(objPath + ".txt", typeof(Object));

                EditorGUILayout.ObjectField(obj, typeof(Object), false, GUILayout.Width(170), GUILayout.ExpandWidth(true));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("BindedScript", string.IsNullOrEmpty(info.bindScript) ? "None" : info.bindScript);

            if (string.IsNullOrEmpty(info.bindScript) && checkLuaExsit(info.gameObject.name))
            {
                if (GUILayout.Button("Reload"))
                {
                    info.bindScript = MonoScriptsPath + info.gameObject.name + "Mono.lua";
                }
            }
            
            GUILayout.EndHorizontal();
            //按钮
            if (string.IsNullOrEmpty(info.bindScript))
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Create"))
                {
                    if (checkLuaExsit(info.gameObject.name))
                    {
                        EditorUtility.DisplayDialog("创建失败", "存在相同命名的脚本，请删除了再试。", "确认");
                    }
                    else
                    {
                        try
                        {
                            CustomLuaMenu.CreateMonoLuaScript(info.gameObject.name, _cScriptDesc);
                            if (checkLuaExsit(info.gameObject.name)) info.bindScript = MonoScriptsPath + info.gameObject.name + "Mono.lua";
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError(e.Message);
                        }
                    }
                }
                /* 界面名有bug
                if (GUILayout.Button("Edit"))
                {
                    try
                    {
                        CustomLuaMenu.OpenMonoLuaScriptEditor(info);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
                 * */
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("-> " + info.gameObject.name, GUILayout.Width(150));
                _cScriptDesc = EditorGUILayout.TextField(string.IsNullOrEmpty(_cScriptDesc) ? "脚本描述" : _cScriptDesc, GUILayout.Width(50), GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
            }
            else
            {

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Reset"))
                {
                    try
                    {
                        info.bindScript = "";
                        obj = null;
                    }
                    catch (System.Exception e)
                    {
                        info.bindScript = "";
                        Debug.Log(e.Message);
                    }
                }

                if (GUILayout.Button("Reload"))
                {
                    try
                    {
                        if (EditorApplication.isPlaying && info != null)
                            info.loadLuaScript();
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }
                GUILayout.EndHorizontal();
            }

            SceneView.RepaintAll();
        }
    }
}


