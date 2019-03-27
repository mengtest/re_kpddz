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
using System.Collections.Generic;
using UI.Controller;






public class NGUIAutoDepth: EditorWindow {

    private static List<GameObject> _uiPanelList = new List<GameObject>();
    const string COMMON_1 = "CommonBackgroundAtlas";
    const string COMMON_2 = "CommonBackAtlas";
    const string COMMON_3 = "CommonAtlas";

    
    static UIAtlas uiAtlas = null;
    static GameObject selectTarget = null;
    static string UIATLAS_NAME = "";
    static int count_common1 = 0;
    static int count_common2 = 0;
    static int count_common3 = 0;
    static int count_uiatlas = 0;
    static List<string> _otherAtlas = new List<string>();

    [MenuItem("GameObject/AutoSetDepth", false, 12)]
    private static void AutoSetDepth(MenuCommand menuCommand)
    {
        var selectGameObject = menuCommand.context as GameObject;
        if (selectGameObject == null)
        {
            EditorUtility.DisplayDialog("目标错误！", "请选择一个UIPanel执行", "OK");
            return;
        }
        if (!IsPrefabInHierarchy(selectGameObject))
        {
            EditorUtility.DisplayDialog("目标错误！", "请选择一个UIPanel执行", "OK");
            return;
        }
        GetAllUIPanelGameObj(selectGameObject);//取所有UIPanel结点放_uiPanelList里
        if (_uiPanelList.Count == 0)
        {
            EditorUtility.DisplayDialog("目标错误！", "请选择一个UIPanel执行", "OK");
            return;
        }

        selectTarget = selectGameObject;
        UIATLAS_NAME = PlayerPrefs.GetString("AutoSetDepth___" + selectTarget.name);//"";

        //创建窗口
        Rect wr = new Rect(0, 0, 500, 100);
        NGUIAutoDepth window = (NGUIAutoDepth)EditorWindow.GetWindowWithRect(typeof(NGUIAutoDepth), wr, true, "选择当前界面的UIAtlas");
        window.Show();
    }

    //绘制窗口时调用
    void OnGUI()
    {

        GUILayout.Space(20f);

        GUILayout.BeginHorizontal();
        if (NGUIEditorTools.DrawPrefixButton("Atlas"))
            ComponentSelector.Show<UIAtlas>(OnSelectAtlas);
            
        GUILayout.Label(UIATLAS_NAME, "HelpBox", GUILayout.Height(18f));
        //GUILayout.Label(atlasName, GUILayout.MinHeight(30f));
        GUILayout.EndHorizontal();

        GUILayout.Space(12f);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("开始", GUILayout.MinHeight(20f), GUILayout.MaxWidth(100f)))
        {
            if (UIATLAS_NAME == "")
            {
                this.ShowNotification(new GUIContent("no select Atlas！"));
            }
            NGUIDepthManager.Show(selectTarget, UIATLAS_NAME);
            Close();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(6f);


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
    void OnSelectAtlas(Object obj)
    {
        uiAtlas = obj as UIAtlas;
        if (uiAtlas != null)
        {
            UIATLAS_NAME = uiAtlas.name;
            PlayerPrefs.SetString("AutoSetDepth___" + selectTarget.name, UIATLAS_NAME);
        }
    }

    private static bool IsPrefabInHierarchy(GameObject go)
    {
        return (PrefabUtility.GetPrefabParent(go) != null);
    }

    private static void GetAllUIPanelGameObj(GameObject target)
    {
        UIPanel[] panels = target.GetComponentsInChildren<UIPanel>(true);
        _uiPanelList.Clear();

        for (int i = 0; i < panels.Length; i++ )
        {
            _uiPanelList.Add(panels[i].gameObject);
        }
    }

}
