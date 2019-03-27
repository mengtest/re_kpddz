/***************************************************************


 *
 *
 * Filename:  	NGUIDepthManager.cs	
 * Summary: 	管理NGUI的Depth窗口
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/12/18 17:38
 ***************************************************************/
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using EventManager;
using UI.Controller;

public struct ItemDepthConfig
{
    public GameObject panel;//所属panel
    public GameObject target;//该item对应的obj
    public bool bIgnor;
    public int idepth;
    public NGUIDepthManager.EAtlasLayers belongTo;//所属分段区
    public string specialType;//auto无法满足时，指定的特殊depth
    public NGUIDepthManager.ESpecialType eSpecial;
    public string strContent;//图集名或文字text
    public void SetIgnor(bool bValue)
    {
        bIgnor = bValue;
    }
    public void SetDepth(int iValue)
    {
        idepth = iValue;
        UISprite sprite = target.GetComponent<UISprite>();
        if (sprite != null)
        {
            sprite.depth = idepth;
            return;
        }
        UITexture texture = target.GetComponent<UITexture>();
        if (texture != null)
        {
            texture.depth = idepth;
            return;
        }
        UILabel label = target.GetComponent<UILabel>();
        if (label != null)
        {
            label.depth = idepth;
            return;
        }
    }
    public void SetSpecialType( NGUIDepthManager.ESpecialType eType)
    {
        eSpecial = eType;
        if (eSpecial == NGUIDepthManager.ESpecialType.Other)
        {
            target.transform.tag = "OtherDepth";
        }
        else
        {
            if (target.CompareTag("OtherDepth"))
                target.tag = "";
        }

        if (eSpecial == NGUIDepthManager.ESpecialType.Depth0)
        {
            idepth = -1;
        }
        else if (eSpecial == NGUIDepthManager.ESpecialType.Depth99)
        {
            idepth = 99;
        }
        else if (eSpecial == NGUIDepthManager.ESpecialType.Depth199)
        {
            idepth = 199;
        }
        else if (eSpecial == NGUIDepthManager.ESpecialType.Depth299)
        {
            idepth = 299;
        }
        else if (eSpecial == NGUIDepthManager.ESpecialType.Depth399)
        {
            idepth = 399;
        }
        else if (eSpecial == NGUIDepthManager.ESpecialType.Depth499)
        {
            idepth = 499;
        }
        SetDepth(idepth);
    }

}

/// <summary>
/// 
/// </summary>
public class NGUIDepthManager : ScriptableWizard
{
    //*****************************************************************************************************
    //数据管理
    //*****************************************************************************************************
    public enum EAtlasLayers
    {
        Common_1,
        Depth199,
        Common_2,
        Depth399,
        Common_3,
        OtherAtlas,
        LabelText,
    };
    public enum ESpecialType
    {
        Auto,
        Depth0,
        Depth99,
        Depth199,
        Depth299,
        Depth399,
        Depth499,
        Other,

    };
    public static GameObject _selectObj;
    public static string UIATLAS_NAME;
    const string COMMON_1 = "CommonBackgroundAtlas_";
    const string COMMON_2 = "CommonBackAtlas_";
    const string COMMON_3 = "CommonAtlas";
    static int count_common1 = 0;
    static int count_common2 = 0;
    static int count_common3 = 0;
    static int count_uiatlas = 0;
    private static List<GameObject> _uiPanelList = new List<GameObject>();
    private static List<string> _otherAtlas = new List<string>();
    private static List<ItemDepthConfig> _allItemsConfig = new List<ItemDepthConfig>();

    private bool bSelectAll = false;

    private static void InitData()
    {
        if (_selectObj == null)
            return;

        GetAllUIPanelGameObj(_selectObj);
        ReadAllChildrenDepth();
    }

    private static void GetAllUIPanelGameObj(GameObject target)
    {
        UIPanel[] panels = target.GetComponentsInChildren<UIPanel>(true);
        _uiPanelList.Clear();

        for (int i = 0; i < panels.Length; i++)
        {
            _uiPanelList.Add(panels[i].gameObject);
        }
    }
    private static void AddItemConfig(ItemDepthConfig tempConfig)
    {
        for (int i = 0; i < _allItemsConfig.Count; i++ )
        {
            if (_allItemsConfig[i].target == tempConfig.target)
            {
                _allItemsConfig[i] = tempConfig;
                return;
            }
        }
        _allItemsConfig.Add(tempConfig);
    }

    private static ItemDepthConfig GetItemConfig(GameObject obj)
    {
        for (int i = 0; i < _allItemsConfig.Count; i++)
        {
            if (_allItemsConfig[i].target == obj)
            {
                return _allItemsConfig[i];
            }
        }
        ItemDepthConfig tempconfig = new ItemDepthConfig();
        tempconfig.bIgnor = false;
        tempconfig.belongTo = EAtlasLayers.OtherAtlas;
        tempconfig.strContent = "";
        return tempconfig;
    }

    private static void SelectAllItem()
    {
        ItemDepthConfig tempConfig;
        for (int i = 0; i < _allItemsConfig.Count; i++)
        {
            tempConfig = _allItemsConfig[i];
            tempConfig.SetIgnor(false);
            AddItemConfig(tempConfig);
        }
    }
    private static void UnSelectAllItem()
    {
        ItemDepthConfig tempConfig;
        for (int i = 0; i < _allItemsConfig.Count; i++)
        {
            tempConfig = _allItemsConfig[i];
            tempConfig.SetIgnor(true);
            AddItemConfig(tempConfig);
        }
    }
    private static void ClearAllItemConfig()
    {
        _allItemsConfig.Clear();
    }

    private static void ReadAllChildrenDepth()
    {
        int iCommon_1 = 1 << (int)EAtlasLayers.Common_1;
        int iDepth199 = 1 << (int)EAtlasLayers.Depth199;
        int iCommon_2 = 1 << (int)EAtlasLayers.Common_2;
        int iDepth399 = 1 << (int)EAtlasLayers.Depth399;
        int iCommon_3 = 1 << (int)EAtlasLayers.Common_3;
        int iOtherAtlas = 1 << (int)EAtlasLayers.OtherAtlas;
        int iLabelText = 1 << (int)EAtlasLayers.LabelText;
        int layers = iCommon_1 | iDepth199 | iCommon_2 | iDepth399 | iCommon_3 | iLabelText | iOtherAtlas;

        count_common1 = 0;
        count_common2 = 0;
        count_common3 = 0;
        count_uiatlas = 0;
        ClearAllItemConfig();
        for (int i = 0; i < _uiPanelList.Count; i++)
        {
            EachChildrenDepth(_uiPanelList[i], _uiPanelList[i], layers, false, true);
        }
        SortAllItems();
    }

    private static void SortAllItems()
    {
        List<ItemDepthConfig> tempList = new List<ItemDepthConfig>();
        for (int i = 0; i < _uiPanelList.Count; i++)
        {
            GameObject uiPanel = _uiPanelList[i];
            ItemDepthConfig tempConfig;
            for (int k=0; k< 8; k++)
            {
                for (int n = 0; n < _allItemsConfig.Count; n++)
                {
                    tempConfig = _allItemsConfig[n];

                    if (tempConfig.panel == uiPanel && tempConfig.belongTo == (EAtlasLayers)k)
                    {
                        tempList.Add(tempConfig);
                    }
                }

            }
        }
        _allItemsConfig = tempList;
    }

    private static void SetChildrenDepth()
    {
        int iCommon_1 = 1 << (int)EAtlasLayers.Common_1;
        int iDepth199 = 1 << (int)EAtlasLayers.Depth199;
        int iCommon_2 = 1 << (int)EAtlasLayers.Common_2;
        int iDepth399 = 1 << (int)EAtlasLayers.Depth399;
        int iCommon_3 = 1 << (int)EAtlasLayers.Common_3;
        int iOtherAtlas = 1 << (int)EAtlasLayers.OtherAtlas;
        int iLabelText = 1 << (int)EAtlasLayers.LabelText;
        int layers = iCommon_1 | iDepth199 | iCommon_2 | iDepth399 | iCommon_3 | iLabelText | iOtherAtlas;
        
        count_common1 = 0;
        count_common2 = 0;
        count_common3 = 0;
        count_uiatlas = 0;
        for (int i = 0; i < _uiPanelList.Count; i++)
        {
            EachChildrenDepth(_uiPanelList[i], _uiPanelList[i], layers, true, true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target">处理该目标下的孩子</param>
    /// <param name="atlasLayers">指定要处理的哪些层</param>
    /// <param name="bAutoSet">是否开启自动设置depth</param>
    /// <param name="bReadDepth">是否记录当前的depth</param>
    private static void EachChildrenDepth(GameObject uiPanel, GameObject target, int atlasLayers, bool bAutoSet = true, bool bReadDepth = false)
    {
        int iCommon_1 = atlasLayers & (1 << (int)EAtlasLayers.Common_1);
        int iDepth199 = atlasLayers & (1 << (int)EAtlasLayers.Depth199);
        int iCommon_2 = atlasLayers & (1 << (int)EAtlasLayers.Common_2);
        int iDepth399 = atlasLayers & (1 << (int)EAtlasLayers.Depth399);
        int iCommon_3 = atlasLayers & (1 << (int)EAtlasLayers.Common_3);
        int iOtherAtlas = atlasLayers & (1 << (int)EAtlasLayers.OtherAtlas);
        int iLabelText = atlasLayers & (1 << (int)EAtlasLayers.LabelText);
        int iCount = target.transform.childCount;
        int curDepth = 0;
        int tempDepth = 10000000;
        EAtlasLayers tempType = EAtlasLayers.OtherAtlas;
        ItemDepthConfig tempConfig;
        string tempContent;
        bool isAtlas;
        UIPanel panel;
        UISprite sprite;
        UILabel label;
        UITexture texture;
        for (int i = 0; i < iCount; i++)
        {
            GameObject objChild = target.transform.GetChild(i).gameObject;
            panel = objChild.GetComponent<UIPanel>();
            if (panel != null) continue;//该孩子为uipanel时跳过
            tempConfig = GetItemConfig(objChild);
            sprite = objChild.GetComponent<UISprite>();
            curDepth = 0;
            tempDepth = 10000000;
            tempType = tempConfig.belongTo;
            isAtlas = false;
            tempContent = tempConfig.strContent;
            if (sprite != null && sprite.atlas != null) //如果是sprite
            {
//                 tempConfig = new ItemDepthConfig();
//                 tempConfig.bIgnor = false;
                
                isAtlas = true;
                curDepth = sprite.depth;
                string atlasName = sprite.atlas.name;
                if (IsIconAtlas(atlasName))
                    atlasName = "SOME_ICON_ATLAS";
                if (atlasName != "" && atlasName == UIATLAS_NAME)
                    atlasName = "CURRENT_UI_ATLAS";

                if (atlasName == COMMON_1 )
                {
                    if (iCommon_1 != 0)
                    {
                        tempDepth = count_common1;
                        count_common1++;
                        tempType = EAtlasLayers.Common_1;
                        tempContent = "sprite:" + sprite.spriteName;
                    }
                 }
                else if (atlasName == "CURRENT_UI_ATLAS" )
                 {
                    if (iDepth199 != 0)
                    {
                        tempDepth = 100 + count_uiatlas;
                        count_uiatlas++;
                        tempType = EAtlasLayers.Depth199;
                        tempContent = "sprite:" + sprite.spriteName;
                    }
                 }
                else if (atlasName == COMMON_2)
                 {
                     if (iCommon_2 != 0)
                     {
                         tempDepth = 200 + count_common2;
                         count_common2++;
                         tempType = EAtlasLayers.Common_2;
                         tempContent = "sprite:" + sprite.spriteName;
                    }
                 }
                else if (atlasName == "SOME_ICON_ATLAS")
                 {
                     if (iDepth399 != 0)
                     {
                         tempDepth = 301;
                         tempType = EAtlasLayers.Depth399;
                         tempContent = "sprite:" + sprite.spriteName;
                    }
                 }
                else if (atlasName == COMMON_3)
                 {
                     if (iCommon_3 != 0)
                     {
                         tempDepth = 400 + count_common3;
                         count_common3++;
                         tempType = EAtlasLayers.Common_3;
                         tempContent = "sprite:" + sprite.spriteName;
                    }
                 }
                else if (iOtherAtlas != 0)
                 {
                     int index = GetIndexInOtherAtlas(atlasName);
                     tempDepth = 500 + index * 10 + 1;
                     tempType = EAtlasLayers.OtherAtlas;
                     tempContent = "Atlas:" + sprite.atlas.name + ", sprite:" + sprite.spriteName;
                 }

                if (tempDepth != 10000000 && bAutoSet && !tempConfig.bIgnor)//有适配到值
                {
                    sprite.depth = tempDepth;
                    curDepth = tempDepth;
                    if (objChild.CompareTag("OtherDepth"))
                        objChild.tag = "";
                }
            }

            label = objChild.GetComponent<UILabel>();
            if (label != null && iLabelText != 0) //如果是label
            {
//                 tempConfig = new ItemDepthConfig();
//                 tempConfig.bIgnor = false;
                
                isAtlas = true;
                curDepth = label.depth;
                tempType = EAtlasLayers.LabelText;
                tempContent = "text:" + label.text;
                if (bAutoSet && !tempConfig.bIgnor)
                {
                    label.depth = 901;
                    curDepth = label.depth;
                    if (objChild.CompareTag("OtherDepth"))
                        objChild.tag = "";
                }
            }
            texture = objChild.GetComponent<UITexture>();
            if (texture != null && iOtherAtlas != 0) //如果是uitexture
            {
//                 tempConfig = new ItemDepthConfig();
//                 tempConfig.bIgnor = false;
                
                isAtlas = true;
                curDepth = texture.depth;
                tempType = EAtlasLayers.OtherAtlas;
                tempContent = "texture:"+texture.name;
                if (bAutoSet && !tempConfig.bIgnor)
                {
                    int index = GetIndexInOtherAtlas(texture.name);
                    texture.depth = 500 + index * 10 + 1;
                    curDepth = texture.depth;
                    if (objChild.CompareTag("OtherDepth"))
                        objChild.tag = "";
                }
            }
            if (isAtlas && bReadDepth)
            {
                ESpecialType eType = GetSpecialDepth(curDepth);
                if (objChild.CompareTag("OtherDepth"))
                {
                    eType = ESpecialType.Other;
                }
                tempConfig.belongTo = tempType;
                if (!tempConfig.bIgnor)
                {
                    tempConfig.bIgnor = (eType != ESpecialType.Auto);
                }
                tempConfig.target = objChild;
                tempConfig.specialType = "";
                tempConfig.eSpecial = eType;
                tempConfig.panel = uiPanel;
                tempConfig.idepth = curDepth;
                tempConfig.strContent = tempContent;
                AddItemConfig(tempConfig);
            }
            EachChildrenDepth(uiPanel, objChild, atlasLayers, bAutoSet, bReadDepth);
        }
    }





    //判断是否是一些图标
    private static bool IsIconAtlas(string atlasName)
    {

        for (int i = 0; i < ControllerBase.AtlasType.allIconAtlas.Count; i++)
        {
            if (atlasName == ControllerBase.AtlasType.allIconAtlas[i])
            {
                return true;
            }
        }
        return false;
    }

    private static int GetIndexInOtherAtlas(string atlasName)
    {
        for (int i = 0; i < _otherAtlas.Count; i++)
        {
            if (atlasName == _otherAtlas[i])
            {
                return i;
            }
        }
        _otherAtlas.Add(atlasName);
        return _otherAtlas.Count - 1;
    }



    //*****************************************************************************************************
    //窗口绘制
    //*****************************************************************************************************
	Vector2 mScroll = Vector2.zero;

	/// <summary>
	/// Show the selection wizard.
	/// </summary>

	static public void Show(GameObject target, string uiAtlasName)
	{
        _selectObj = target;
        UIATLAS_NAME = uiAtlasName;
        string targetName = target.transform.name;
        NGUIDepthManager comp = ScriptableWizard.DisplayWizard<NGUIDepthManager>("'" + targetName + "'的depth管理");
        InitData();
	}


	/// <summary>
	/// Draw the custom wizard.
	/// </summary>

	void OnGUI ()
	{

		mScroll = GUILayout.BeginScrollView(mScroll);

        GameObject tempPanel = null;
        bool bBegin = false;
        bool bOpen = false;
        EAtlasLayers tempLayer = EAtlasLayers.Common_1;
        ItemDepthConfig tempConfig;
        for (int i = 0; i < _allItemsConfig.Count;i++ )
        {

            tempConfig = _allItemsConfig[i];
            if ((tempPanel != null && tempPanel != tempConfig.panel) || (tempPanel != null && tempLayer != tempConfig.belongTo))
            {
                if (bBegin)
                    NGUIEditorTools.EndContents();
                bBegin = false;
            }

            if (tempPanel == null || tempPanel != tempConfig.panel)
            {
                DrawOnePanelName(tempConfig.panel);
            }
            if (tempPanel == null || tempLayer != tempConfig.belongTo)
            {
                //DrawOneTitle(tempConfig);
                string sTitle = GetLayerName(tempConfig.belongTo);
                string sKey = tempConfig.panel.name + tempConfig.belongTo.ToString();
                bOpen = NGUIEditorTools.DrawHeader(sTitle, sKey);
                if (bOpen)
                {
                    NGUIEditorTools.BeginContents();
                    bBegin = true;
//                     GUILayout.BeginHorizontal();
//                     GUILayout.FlexibleSpace();
//                     bool bClick = GUILayout.Button("重置选中项depth", "LargeButton", GUILayout.Width(160f), GUILayout.Height(25f));
//                     if (bClick)
//                     {
//                         int layers = 1 << (int)tempConfig.belongTo;
// 
//                         count_common1 = 0;
//                         count_common2 = 0;
//                         count_common3 = 0;
//                         count_uiatlas = 0;
//                         EachChildrenDepth(tempConfig.panel, tempConfig.panel, layers, true, true);
//                         SortAllItems();
//                     }
//                     GUILayout.FlexibleSpace();
//                     GUILayout.EndHorizontal();
                }
                else
                {
                    bBegin = false;
                }
            }
            if (bBegin)
            DrawOneItem(ref tempConfig);

            tempPanel = tempConfig.panel;
            tempLayer = tempConfig.belongTo;
        }
        if (bBegin)
            NGUIEditorTools.EndContents();

		GUILayout.EndScrollView();
        GUILayout.Space(6f);
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
        bool change = GUILayout.Button("自动适配所有选中项", "LargeButton", GUILayout.Width(160f));
        if (change)
        {
            SetChildrenDepth();
            ReadAllChildrenDepth();
        }
        GUILayout.FlexibleSpace();
        GUILayout.Space(-90f);
        bool bClickInstruction = GUILayout.Button("分段说明", "LargeButton", GUILayout.Width(90f));
        if (bClickInstruction)
        {
            NGUIDepthInstruction.ShowDialog();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
	}

    private static string GetLayerName(EAtlasLayers eLayer)
    {
        if (eLayer == EAtlasLayers.Common_1)
        {
            return "0~99:common_1(" + COMMON_1 + ")";
        }
        else if (eLayer == EAtlasLayers.Depth199)
        {
            return "100~199:当前界面图集(" + UIATLAS_NAME + ")";
        }
        else if (eLayer == EAtlasLayers.Common_2)
        {
            return "200~299:common_2(" + COMMON_2 + ")";
        }
        else if (eLayer == EAtlasLayers.Depth399)
        {
            return "300~399:各种图标图集";
        }
        else if (eLayer == EAtlasLayers.Common_3)
        {
            return "400~499:common_3(" + COMMON_3 + ")";
        }
        else if (eLayer == EAtlasLayers.OtherAtlas)
        {
            return "500~599:其它图集";
        }
        else if (eLayer == EAtlasLayers.LabelText)
        {
            return "900~999:文字图集";
        }
        return "";
    }

    private string GetSpecialTypeName(ESpecialType eType)
    {
        if (eType == ESpecialType.Auto)
        {
            return "Auto";
        }
        else if (eType == ESpecialType.Depth0)
        {
            return "Depth-1";
        }
        else if (eType == ESpecialType.Depth99)
        {
            return "Depth99";
        }
        else if (eType == ESpecialType.Depth199)
        {
            return "Depth199";
        }
        else if (eType == ESpecialType.Depth299)
        {
            return "Depth299";
        }
        else if (eType == ESpecialType.Depth399)
        {
            return "Depth399";
        }
        else if (eType == ESpecialType.Depth499)
        {
            return "Depth499";
        }
        else if (eType == ESpecialType.Other)
        {
            return "Other";
        }
        return "Auto";
    }

    private static ESpecialType GetSpecialDepth(int Depth)
    {
        if (Depth == -1)
            return ESpecialType.Depth0;
        else if (Depth == 99)
            return ESpecialType.Depth99;
        else if (Depth == 199)
            return ESpecialType.Depth199;
        else if (Depth == 299)
            return ESpecialType.Depth299;
        else if (Depth == 399)
            return ESpecialType.Depth399;
        else if (Depth == 499)
            return ESpecialType.Depth499;
        else
            return ESpecialType.Auto;

    }


    //判断是否是一些图标
    private static bool IsSpecialDepth(int Depth)
    {
        if (Depth == -1 || Depth == 99 || Depth == 199 || Depth == 299 || Depth == 399 || Depth == 499 || Depth == 599)
        {
            return true;
        }
        return false;
    }

    void DrawOnePanelName(GameObject PanelObj)
    {
        string mTitle = "panel: " + PanelObj.transform.name;
        NGUIEditorTools.SetLabelWidth(80f);
        GUILayout.Label(mTitle, "LODLevelNotifyText");
    }

    //打印panel中的一个atlas的title
    void DrawOneTitle(ItemDepthConfig tempConfig)
    {
        float height = 32f;
        GameObject PanelObj = tempConfig.panel;
        EAtlasLayers eLayer = tempConfig.belongTo;
        string sTitle = GetLayerName(tempConfig.belongTo);
        GUILayout.BeginHorizontal();
        GUILayout.Box(sTitle, GUILayout.Width(250f));
        bool bClick = GUILayout.Button("重置选中项depth", "LargeButton", GUILayout.Width(160f), GUILayout.Height(height-5f));
        if (bClick)
        {
            int layers = 1 << (int)eLayer;

            count_common1 = 0;
            count_common2 = 0;
            count_common3 = 0;
            count_uiatlas = 0;
            EachChildrenDepth(tempConfig.panel, tempConfig.panel, layers, true, true);
        }
        GUILayout.EndHorizontal();
    }

    //打印panel中的一个atlas的一个sprite

    void DrawOneItem(ref ItemDepthConfig depthConfig)
	{
        float height = 25f;
        GameObject PanelObj = depthConfig.panel;
        GameObject obj = depthConfig.target;
        bool bSelect = !depthConfig.bIgnor;
        int iDepth = depthConfig.idepth;
        string sSpecialType = depthConfig.specialType;
        ESpecialType eSpecial = depthConfig.eSpecial;
        GUILayout.BeginHorizontal();
        GUILayout.Space(30f);
        bool bSelectNow = EditorGUILayout.Toggle("", bSelect, GUILayout.Width(25f));
        if (bSelectNow != bSelect)
        {
            depthConfig.SetIgnor(!bSelectNow);//此数据是复制的，不能直接调
            AddItemConfig(depthConfig);
        }

        bool bPush1 = GUILayout.Button(obj.transform.name, "AS TextArea", GUILayout.Width(120f));
        bool bPush2 = GUILayout.Button(depthConfig.strContent, "AS TextArea", GUILayout.Width(200f));
        if (bPush1 || bPush2 )
        {
            GameObject[] objs = new GameObject[1];
            objs[0] = obj;
            Selection.objects = objs;
        }

        if (eSpecial == ESpecialType.Other)
        {
            string sdepth = GUILayout.TextArea(iDepth.ToString(), GUILayout.Width(60f));
            int int_depth = int.Parse(sdepth);
            if (int_depth != depthConfig.idepth)
            {
                depthConfig.SetDepth(int_depth);
                AddItemConfig(depthConfig);
            }
        }
        else
        {
            GUILayout.Box(iDepth.ToString(), GUILayout.Width(60f));
        }

        //特殊处理下拉菜单
        if (GUILayout.Button(GetSpecialTypeName(eSpecial), EditorStyles.layerMaskField, GUILayout.Width(250f), GUILayout.MinHeight(height)))
        {
            bool isSelect = false;
            GenericMenu menu = new GenericMenu();
            EventMultiArgs args = new EventMultiArgs();
            args.AddArg("target", depthConfig);
            args.AddArg("specialType", ESpecialType.Auto);
            if (sSpecialType == "Auto")
                isSelect = true;
            menu.AddItem(new GUIContent("Auto"), isSelect, ChangeSpecialType, args);
            isSelect = false;
            if (sSpecialType == "Depth-1")
                isSelect = true;
            EventMultiArgs args2 = new EventMultiArgs();
            args2.AddArg("target", depthConfig);
            args2.AddArg("specialType", ESpecialType.Depth0);
            menu.AddItem(new GUIContent("-1:最底"), isSelect, ChangeSpecialType, args2);
            isSelect = false;
            if (sSpecialType == "Depth99")
                isSelect = true;
            EventMultiArgs args3 = new EventMultiArgs();
            args3.AddArg("target", depthConfig);
            args3.AddArg("specialType", ESpecialType.Depth99);
            menu.AddItem(new GUIContent("99:Common_1顶层"), isSelect, ChangeSpecialType, args3);
            isSelect = false;
            if (sSpecialType == "Depth199")
                isSelect = true;
            EventMultiArgs args4 = new EventMultiArgs();
            args4.AddArg("target", depthConfig);
            args4.AddArg("specialType", ESpecialType.Depth199);
            menu.AddItem(new GUIContent("199:界面图集顶层"), isSelect, ChangeSpecialType, args4);
            isSelect = false;
            if (sSpecialType == "Depth299")
                isSelect = true;
            EventMultiArgs args5 = new EventMultiArgs();
            args5.AddArg("target", depthConfig);
            args5.AddArg("specialType", ESpecialType.Depth299);
            menu.AddItem(new GUIContent("299:Common_2顶层"), isSelect, ChangeSpecialType, args5);
            isSelect = false;
            if (sSpecialType == "Depth399")
                isSelect = true;
            EventMultiArgs args6 = new EventMultiArgs();
            args6.AddArg("target", depthConfig);
            args6.AddArg("specialType", ESpecialType.Depth399);
            menu.AddItem(new GUIContent("399:图标图集顶层"), isSelect, ChangeSpecialType, args6);
            isSelect = false;
            if (sSpecialType == "Depth499")
                isSelect = true;
            EventMultiArgs args7 = new EventMultiArgs();
            args7.AddArg("target", depthConfig);
            args7.AddArg("specialType", ESpecialType.Depth499);
            menu.AddItem(new GUIContent("499:Common_3顶层"), isSelect, ChangeSpecialType, args7);
            isSelect = false;
            if (sSpecialType == "Other")
                isSelect = true;
            EventMultiArgs args8 = new EventMultiArgs();
            args8.AddArg("target", depthConfig);
            args8.AddArg("specialType", ESpecialType.Other);
            menu.AddItem(new GUIContent("Other"), isSelect, ChangeSpecialType, args8);
            menu.ShowAsContext();
        }
        GUILayout.Space(30f);
        GUILayout.EndHorizontal();

	}

    void ChangeSpecialType(object param)
    {
        EventMultiArgs args = (EventMultiArgs)param;
        ESpecialType specialType = args.GetArg<ESpecialType>("specialType");
        ItemDepthConfig target = args.GetArg<ItemDepthConfig>("target");
        target.SetSpecialType(specialType);
        if (specialType != ESpecialType.Auto)
        {
            target.SetIgnor(true);
        }
        else
        {
            target.SetIgnor(false);
        }
        AddItemConfig(target);

    }

}
